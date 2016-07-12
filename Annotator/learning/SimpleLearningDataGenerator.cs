using Accord.Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    class SimpleLearningDataGenerator : LearningDataGenerator
    {

        // Assume that we only care about the upper body of humand rig
        List<string> extractedJointNames = new List<string>() {
                "SpineShoulder", "ShoulderLeft", "ElbowLeft", "WristLeft", "HandLeft", "HandTipLeft", "ThumbLeft",
                "ShoulderRight", "ElbowRight", "WristRight", "HandRight", "HandTipRight", "ThumbRight"
            };

        public object extractDataFromProject(Project project)
        {
            var data = new Dictionary<string, Tuple<List<List<float>>, List<string>>>();

            // For each session, there is a list of data points and label points
            // Each data point and label point corresponds to a frame
            for (int i = 0; i < project.getSessionN(); i++)
            {
                Session session = project[i];

                session.loadIfNotLoaded();

                List<List<float>> sessionData = new List<List<float>>();
                List<string> sessionLabels = new List<string>();

                Object sub = null;
                Object obj = null;

                for (int frameNo = 1; frameNo <= session.sessionLength; frameNo++)
                {
                    sessionLabels.Add("None");
                }

                Console.WriteLine(session.sessionLength + " " + session.sessionName + " " + session.events.Count);

                // Assume that all events are non-overlapping, this is the simplest case
                foreach (Event e in session.events)
                {
                    try
                    {
                        int startFrame = e.startFrame;
                        int endFrame = e.endFrame;
                        Console.WriteLine(startFrame + " " + endFrame);

                        // Simply assume that the event participants are all the objects mentioned
                        List<Event.Reference> references = e.references;

                        // Assume that there is only one event mentioned
                        Event.Action action = e.actions[0];

                        for (int frameNo = startFrame; frameNo <= endFrame; frameNo++)
                        {
                            sessionLabels[frameNo - 1] = action.semanticType;
                        }

                        // Assume that the sentence is in active voice, so that the orders of objects are subject - direct object
                        // Assume that the action verb is transitive with exact two references involved.

                        Event.Reference subRef = references[0];
                        Event.Reference objRef = references[1];

                        Console.WriteLine(subRef.refObjectId + " " + objRef.refObjectId);

                        // Currenly only consider the case when subject is of shape="Annotator.RigObject" and object of shape="Annotator.GlyphBoxObject"
                        Object tempoSub = session.getObject(subRef.refObjectId);
                        Object tempoObj = session.getObject(objRef.refObjectId);

                        if ((tempoSub != null && sub != null && tempoSub.id != sub.id) || (tempoObj != null && obj != null && tempoObj.id != obj.id))
                        {
                            throw new Exception("Assumption for Simple learning is that all events are overlapping and share the same participants. This event will be ignored.");
                        }

                        if (tempoSub != null)
                        {
                            sub = tempoSub;
                        }

                        if (tempoObj != null)
                        {
                            obj = tempoObj;
                        }

                        Console.WriteLine(sub + " " + obj);
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc);
                    }
                }



                // Interpolating for objects will be carried out here
                // Interpolating objectives:
                // - To accommodate for frames where there is no tracking result (objects in motion, for example)
                // Interpolating methods:
                // - Linear interpolation between two tracking results, or identical interpolation from the beginning or to the end
                if (sub != null && obj != null)
                {
                    if (sub.GetType() == typeof(Annotator.RigObject) && obj.GetType() == typeof(Annotator.GlyphBoxObject))
                    {
                        RigObject castedSub = (RigObject)sub;
                        GlyphBoxObject castedObj = (GlyphBoxObject)obj;



                        for (int frameNo = 1; frameNo <= session.sessionLength; frameNo++)
                        {
                            List<float> frameData = new List<float>();

                            // -----------------------------------------------------------------------------------
                            // -----------------------------------------------------------------------------------
                            // Interpolation for rig object
                            int prev = castedSub.object3DMarks.Keys.LastOrDefault(x => x <= frameNo);
                            int next = castedSub.object3DMarks.Keys.FirstOrDefault(x => x > frameNo);

                            // Has yet to detect the object
                            // Identical interpolation from the first marker
                            if (prev == 0)
                            {
                                var locationMark3dNext = (RigLocationMark<Point3>)castedSub.object3DMarks[next];
                                var rigJoints3DNext = locationMark3dNext.rigFigure.rigJoints;
                                insertDataFromJointsToFrameData(frameData, rigJoints3DNext);
                            }
                            // At the end, identical interpolation
                            else if (next == 0)
                            {
                                var locationMark3dPrev = (RigLocationMark<Point3>)castedSub.object3DMarks[prev];
                                var rigJoints3DPrev = locationMark3dPrev.rigFigure.rigJoints;

                                insertDataFromJointsToFrameData(frameData, rigJoints3DPrev);
                            }
                            // Linear interpolation
                            else
                            {
                                // both first and last != 0 
                                var locationMark3dPrev = (RigLocationMark<Point3>)castedSub.object3DMarks[prev];
                                var rigJoints3DPrev = locationMark3dPrev.rigFigure.rigJoints;

                                var locationMark3dNext = (RigLocationMark<Point3>)castedSub.object3DMarks[next];
                                var rigJoints3DNext = locationMark3dNext.rigFigure.rigJoints;

                                foreach (string jointName in extractedJointNames)
                                {
                                    var jointPrev = rigJoints3DPrev[jointName];
                                    var jointNext = rigJoints3DNext[jointName];

                                    Point3 joint = jointPrev.Multiple((float)(next - frameNo) / (next - prev)).
                                        Add(jointNext.Multiple((float)(frameNo - prev) / (next - prev)));
                                    frameData.Add(joint.X);
                                    frameData.Add(joint.Y);
                                    frameData.Add(joint.Z);
                                }
                            }

                            // -----------------------------------------------------------------------------------
                            // -----------------------------------------------------------------------------------
                            // Interpolation for glyph objects
                            // Fix Inf and -Inf for castedObj.objectMarks

                            prev = castedObj.object3DMarks.Keys.LastOrDefault(x => x <= frameNo);
                            next = castedObj.object3DMarks.Keys.FirstOrDefault(x => x > frameNo);

                            // Has yet to detect the object
                            // Identical interpolation from the first marker
                            if (prev == 0)
                            {
                                var locationMark3dNext = ((GlyphBoxLocationMark<Point3>)castedObj.object3DMarks[next]).boundingPolygons;
                                var fixedLocationMark3dNext = fixListOfPoints(locationMark3dNext[0]);

                                foreach (Point3 p in fixedLocationMark3dNext)
                                {
                                    frameData.Add(p.X);
                                    frameData.Add(p.Y);
                                    frameData.Add(p.Z);
                                }
                            }
                            // At the end, identical interpolation
                            else if (next == 0)
                            {
                                var locationMark3dPrev = ((GlyphBoxLocationMark<Point3>)castedObj.object3DMarks[prev]).boundingPolygons;
                                var fixedLocationMark3dPrev = fixListOfPoints(locationMark3dPrev[0]);

                                foreach (Point3 p in fixedLocationMark3dPrev)
                                {
                                    frameData.Add(p.X);
                                    frameData.Add(p.Y);
                                    frameData.Add(p.Z);
                                }
                            }
                            // Linear interpolation
                            else
                            {
                                // both first and last != 0 
                                var locationMark3dPrev = ((GlyphBoxLocationMark<Point3>)castedObj.object3DMarks[prev]).boundingPolygons;
                                var locationMark3dNext = ((GlyphBoxLocationMark<Point3>)castedObj.object3DMarks[next]).boundingPolygons;
                                
                                var fixedLocationMark3dPrev = fixListOfPoints(locationMark3dPrev[0]);
                                var fixedLocationMark3dNext = fixListOfPoints(locationMark3dNext[0]);

                                for (int iter = 0; iter < fixedLocationMark3dPrev.Count; iter++)
                                {
                                    var jointPrev = fixedLocationMark3dPrev[iter];
                                    var jointNext = fixedLocationMark3dNext[iter];

                                    Point3 joint = jointPrev.Multiple((float)(next - frameNo) / (next - prev)).
                                        Add(jointNext.Multiple((float)(frameNo - prev) / (next - prev)));
                                    frameData.Add(joint.X);
                                    frameData.Add(joint.Y);
                                    frameData.Add(joint.Z);
                                }
                            }

                            sessionData.Add(frameData);
                        }
                    }
                }

                data[session.sessionName] = new Tuple<List<List<float>>, List<string>>(sessionData, sessionLabels);
            }

            return data;
        }

        /// <summary>
        /// Replace location that has value of Inf/-Inf with 
        /// value of other locations. Effectively this collapses the original 
        /// geometric form. 
        /// </summary>
        /// <param name="locations"></param>
        /// <returns></returns>
        private List<Point3> fixListOfPoints(List<Point3> locations)
        {
            var fixedPoints = new List<Point3>();

            for (int i = 0; i < locations.Count; i ++)
            {
                var point = locations[i];

                int j;
                for (j = 0; j < locations.Count - 1; j++)
                {
                    var pointNext = locations[(i + j) % locations.Count];

                    if (!float.IsInfinity(pointNext.X) && !float.IsInfinity(pointNext.Y) && !float.IsInfinity(pointNext.Z))
                    {
                        fixedPoints.Add(pointNext);
                        break;
                    }
                }

                if ( j == locations.Count - 1)
                {
                    fixedPoints.Add(new Point3(0,0,0));
                }
            }

            if ( locations.Count != fixedPoints.Count)
            {
                locations.ForEach(l => Console.WriteLine(l));
                fixedPoints.ForEach(l => Console.WriteLine(l));
            }

            return fixedPoints;
        }

        private void insertDataFromJointsToFrameData(List<float> frameData, Dictionary<string, Point3> rigJoints3DNext)
        {
            foreach (string jointName in extractedJointNames)
            {
                var joint = rigJoints3DNext[jointName];
                frameData.Add(joint.X);
                frameData.Add(joint.Y);
                frameData.Add(joint.Z);
            }
        }

        public void writeExtractedDataIntoFile(Project project, string filename)
        {
            var data = (Dictionary<string, Tuple<List<List<float>>, List<string>>>)extractDataFromProject(project);

            using (StreamWriter file = new StreamWriter(filename))
            {
                foreach (string sessionName in data.Keys)
                {
                    file.WriteLine(sessionName);

                    var sessionData = data[sessionName].Item1;
                    var sessionLabels = data[sessionName].Item2;

                    for (int i = 0; i < sessionData.Count; i++)
                    {
                        var frameData = sessionData[i];
                        var frameLbl = sessionLabels[i];

                        var line = (i + 1) + " " + string.Join(",", frameData) + " " + frameLbl;

                        file.WriteLine(line);
                    }

                    file.WriteLine();
                }
            }
        }
    }
}