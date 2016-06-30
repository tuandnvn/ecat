using Emgu.CV;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Annotator
{
    partial class RecordPanel
    {
        VideoReader videoReader;
        IDepthReader depthReader;
        object playBackRgbLock;
        int depthFrame;
        int depthWidth;
        int depthHeight;
        int rgbPlaybackFrameNo;
        private Body[] playbackBodies = null;

        private async void handlePlayButtonOn()
        {
            recordMode = RecordMode.Playingback;
            finishRecording.Wait();

            playButton.ImageIndex = 3;

            playBackRgbLock = new object();
            rgbBoard.playbackLock = playBackRgbLock;

            // Reread rgb file

            videoReader = new VideoReader(tempRgbFileName, (int)lastWrittenRgbTime.TotalMilliseconds);
            rgbBoard.mat = videoReader.getFrame(0);

            if (rgbBoard.mat == null)
            {
                // video is null
                return;
            }
            rgbBoard.Image = rgbBoard.mat.Bitmap;

            rgbPlaybackFrameNo = videoReader.frameCount;
            int frameWidth = videoReader.frameWidth;
            int frameHeight = videoReader.frameHeight;

            depthReader = new BaseDepthReader(tempDepthFileName);
            depthFrame = depthReader.getFrameCount();
            depthWidth = depthReader.getWidth();
            depthHeight = depthReader.getHeight();
            depthBitmap = new Bitmap(depthWidth, depthHeight, PixelFormat.Format32bppRgb);
            depthValuesToByte = new byte[depthWidth * depthHeight * 4];

            endTimeLabel.Text = (int)lastWrittenRgbTime.TotalMinutes + ":" + lastWrittenRgbTime.Seconds.ToString("00") + "." + lastWrittenRgbTime.Milliseconds.ToString("000");

            // Set values for playBar
            playBar.Enabled = true;
            playBar.Minimum = 1;
            playBar.Maximum = rgbPlaybackFrameNo;
            playBar.Value = 1;

            StringBuilder sb = new StringBuilder();

            sb.Append("Temporary rgb file has written " + rgbStreamedFrame + " frames \n");
            sb.Append("Temporary rgb file has " + rgbPlaybackFrameNo + " frames of size = ( " + frameWidth + " , " + frameHeight + " ) \n");
            sb.Append("Temporary depth file has " + depthFrame + " frames of size = ( " + depthWidth + " , " + depthHeight + " ) \n");
            sb.Append("RecordingTime " + lastWrittenRgbTime + "\n");

            if (rigDetected.Value)
            {
                sb.Append("Rig(s) is detected.\n");
            } else
            {
                sb.Append("No rig is detected.\n");
            }

            await DetectObjects();

            Console.WriteLine("In playback " + detectedObjects.Count);

            sb.Append(detectedObjects.Count + " objects is detected.\n");

            helperTextBox.Text = sb.ToString();

            main.Invalidate();
        }

        private void updateRgbBoardWithFrame()
        {
            if (videoReader == null)
            {
                return;
            }

            lock (playBackRgbLock)
            {
                Mat m = videoReader.getFrame(playBar.Value - 1);
                if (m != null)
                {
                    rgbBoard.mat = m;
                    rgbBoard.Image = rgbBoard.mat.Bitmap;
                }
            }
        }

        private void updateDepthBoardWithFrame()
        {
            try
            {
                int recordedTimeForRgbFrame = (int)(lastWrittenRgbTime.TotalMilliseconds * (playBar.Value - 1) / (rgbPlaybackFrameNo - 1));

                depthReader.readFrameAtTimeToBitmap(recordedTimeForRgbFrame, depthBitmap, depthValuesToByte, scale);

                this.depthBoard.Image = depthBitmap;
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e);
            }
            catch (IOException e)
            {
                Console.WriteLine(e);
            }catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void updateRigWithFrame()
        {
            int recordedTimeForRgbFrame = (int)(lastWrittenRgbTime.TotalMilliseconds * (playBar.Value - 1) / (rgbPlaybackFrameNo - 1));
            int appropriateRigFrame = recordedRigTimePoints.BinarySearch(recordedTimeForRgbFrame);

            // bitwise complement if it is not found
            if (appropriateRigFrame < 0)
            {
                appropriateRigFrame = ~appropriateRigFrame;

                if (appropriateRigFrame == recordedRigTimePoints.Count)
                {
                    appropriateRigFrame = recordedRigTimePoints.Count - 1;
                }
                else if (appropriateRigFrame > 0)
                {
                    // Smaller timepoint is closer
                    if ((recordedTimeForRgbFrame - recordedRigTimePoints[appropriateRigFrame - 1]) < recordedRigTimePoints[appropriateRigFrame] - recordedTimeForRgbFrame)
                    {
                        appropriateRigFrame--;
                    }
                }
            }

            if (appropriateRigFrame != 0)
            {
                this.playbackBodies = recordedRigs[appropriateRigFrame];
            } else
            {
                if ( recordedRigTimePoints[0] - recordedTimeForRgbFrame > 100 )
                {
                    this.playbackBodies = new Body[0];
                } else
                {
                    this.playbackBodies = recordedRigs[appropriateRigFrame];
                }
            }
            
            Invalidate();
        }

        internal void setTrackbarLocation(int value)
        {
            playBar.Value = value;
        }

        private void handlePlayButtonOff()
        {
            playButton.ImageIndex = 2;

            recordMode = RecordMode.None;
            optionsTable.Enabled = true;

            tmspStartRecording = null;

            helperTextBox.Text = "";

            if (videoReader != null)
            {
                videoReader.Dispose();
                videoReader = null;
            }

            if (depthReader != null)
            {
                depthReader.Dispose();
                depthReader = null;
            }
            endTimeLabel.Text = "00:00.000";

            main.Invalidate();
        }



        private void saveRecordedSession_Click(object sender, EventArgs e)
        {
            Project currentProject = main.selectedProject;

            if (currentProject == null)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                DialogResult folderResult = fbd.ShowDialog(main);
                if (folderResult == DialogResult.OK)
                {
                    string pathToFolder = fbd.SelectedPath;

                    foreach (String fileName in new[] { tempRgbFileName, tempDepthFileName, tempConfigFileName })
                    {
                        string dstFileName = pathToFolder + Path.DirectorySeparatorChar + mapFileName[fileName];
                        if (!File.Exists(dstFileName))
                            File.Copy(fileName, dstFileName);
                    }
                }
                return;
            }

            var result = MessageBox.Show(main, "Do you want to add captured session into project " + currentProject.getProjectName() +
                "?. Yes if you do, no if you want to save it into a separate folder", "Save session", MessageBoxButtons.YesNoCancel);

            switch (result)
            {
                case DialogResult.Yes:
                    SessionInfo sessionInfo = new SessionInfo(main, currentProject.getProjectName());
                    sessionInfo.Location = new Point(this.Location.X + (int)(sessionInfo.Width / 2.5), this.Location.Y + sessionInfo.Height / 2);
                    if (videoReader != null)
                    {
                        videoReader.Dispose();
                        videoReader = null;
                    }

                    if (depthReader != null)
                    {
                        depthReader.Dispose();
                        depthReader = null;
                    }

                    sessionInfo.okButton.Click += new System.EventHandler(this.addSessionOkClick);
                    sessionInfo.Show();
                    break;
                case DialogResult.No:
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    DialogResult folderResult = fbd.ShowDialog(main);
                    if (folderResult == DialogResult.OK)
                    {
                        string pathToFolder = fbd.SelectedPath;

                        foreach (String fileName in new[] { tempRgbFileName, tempDepthFileName, tempConfigFileName })
                        {
                            string dstFileName = pathToFolder + Path.DirectorySeparatorChar + mapFileName[fileName];
                            Console.WriteLine("Copy to file; dstFileName " + dstFileName);
                            if (!File.Exists(dstFileName))
                                File.Copy(fileName, dstFileName);
                        }
                    }
                    break;
                case DialogResult.Cancel:
                    break;
                default:
                    break;
            }

            // Back to annotating
            main.tabs.SelectedIndex = 0;
        }

        private void addSessionOkClick(object sender, EventArgs e)
        {
            if (main.currentSession != null)
            {
                var localFiles = new Dictionary<string, string>();
                foreach (String fileName in new[] { tempRgbFileName, tempDepthFileName, tempRigFileName })
                {
                    Console.WriteLine("Copy to file; dstFileName " + mapFileName[fileName]);
                    localFiles[fileName] = main.copyFileIntoLocalSession(fileName, mapFileName[fileName]);
                }

                main.currentSession.duration = (long) lastWrittenRgbTime.TotalMilliseconds;
                if (startRecordingRgb.HasValue)
                {
                    main.currentSession.startWriteRGB = startRecordingRgb.Value;
                }

                // Write rig into file as a new object                
                if (rigDetected.Value)
                {
                    // Copy the standard body schema file into local session directory
                    string schemeFilePath = "bodyScheme.xml";

                    main.copyFileIntoLocalSession(schemeFilePath, schemeFilePath);

                    Rigs rigs = Rigs.getRigFromSource(localFiles[tempRigFileName], schemeFilePath);
                    var objects = rigs.generateObjects(main.currentSession, localFiles[tempRgbFileName]).Values.ToList();
                    foreach (var o in objects)
                    {
                        main.currentSession.addObject(o);
                        main.addObjectAnnotation(o);
                    }
                }

                // Add objects into current session
                foreach (var o in detectedObjects)
                {
                    o.session = main.currentSession;
                    main.currentSession.addObject(o);
                }

                main.saveCurrentSession();
            }
        }
    }
}
