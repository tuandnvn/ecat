using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    partial class Main
    {
        private List<MemoryStream> savedSessions;
        private List<String> logs;

        private int _currentSessionIndex;
        internal int currentSessionIndex
        {
            get
            {
                return _currentSessionIndex;
            }

            set
            {
                _currentSessionIndex = value;
                if (_currentSessionIndex <= 0)
                {
                    undoBtn.Enabled = false;
                }
                else
                {
                    undoBtn.Enabled = true;
                }

                if (savedSessions == null || _currentSessionIndex >= savedSessions.Count - 1)
                {
                    redoBtn.Enabled = false;
                }
                else
                {
                    redoBtn.Enabled = true;
                }
            }
        }

        private void undoBtn_Click(object sender, EventArgs e)
        {
            cancelSelectObject();
            undo();
        }

        private void redoBtn_Click(object sender, EventArgs e)
        {
            cancelSelectObject();
            redo();
        }

        protected void InitMemento()
        {
            clearMemento();
        }

        internal void clearMemento()
        {
            currentSessionIndex = -1;
            savedSessions = new List<MemoryStream>();
            logs = new List<string>();
        }

        /// <summary>
        /// Log session add an image of current session into a list of MemoryStream for later recovery
        /// 
        /// 
        /// </summary>
        /// <param name="log">An accompanying log message</param>
        internal void logSession(String log)
        {
            MemoryStream ms = this.currentSession?.saveToMemento();

            if (ms != null)
            {
                if (currentSessionIndex < savedSessions.Count - 1)
                {
                    // Remove all sessions from currentSessionIndex + 1
                    savedSessions.RemoveRange(currentSessionIndex + 1, savedSessions.Count - currentSessionIndex - 1);
                }
                savedSessions.Add(ms);
                logMessage(log);
            }

            currentSessionIndex++;
        }

        /// <summary>
        /// Log a message only
        /// </summary>
        /// <param name="log"></param>
        internal void logMessage(String log)
        {
            logs.Add(log);

            logGridView.Rows.Add(logs.Count.ToString(), log);
        }

        /// <summary>
        /// Load an image of the current session given the index in the image list
        /// </summary>
        /// <param name="index">Position of memento point in the list</param>
        /// <returns>True if loading is sucessful</returns>
        private bool loadFromMemory(int index)
        {
            if (index >= 0 && index < savedSessions.Count)
            {
                try
                {
                    var ms = savedSessions[index];
                    ms.Position = 0;
                    currentSession.restoreFromMemento(savedSessions[index]);
                    currentSessionIndex = index;

                    return true;
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.ToString());
                }
            }

            return false;
        }

        private void redo()
        {
            if (loadFromMemory(currentSessionIndex + 1))
            {
                rerenderAnnotation();
                logMessage("Redo");
            }
                
        }

        private void undo()
        {
            if (loadFromMemory(currentSessionIndex - 1))
            {
                rerenderAnnotation();
                logMessage("Undo");
            }
        }
    }
}
