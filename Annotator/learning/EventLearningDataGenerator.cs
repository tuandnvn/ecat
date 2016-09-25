using Accord.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Annotator
{
    class EventLearningDataGenerator : SimpleLearningDataGenerator
    {
        class ReferenceComparer : IComparer<Event.Reference>
        {
            public int Compare(Event.Reference x, Event.Reference y)
            {
                return x.start - y.start;
            }
        }

        public override void writeExtractedDataIntoFile(Project project, string filename)
        {
            try
            {
                XmlWriterSettings ws = new XmlWriterSettings();
                ws.Indent = true;

                using (XmlWriter writer = XmlWriter.Create(filename, ws))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("project");

                    // For each session, there is a list of data points and label points
                    // Each data point and label point corresponds to a frame
                    for (int i = 0; i < project.sessions.Count; i++)
                    {
                        Session session = project.getSession(i);

                        session.loadIfNotLoaded();

                        Console.WriteLine("Write learning data for session " + session.name);

                        writer.WriteStartElement("session");
                        writer.WriteAttributeString("name", session.name);

                        var objects = session.getObjects().Where(o => o.GetType() == typeof(RigObject) || o.GetType() == typeof(GlyphBoxObject));

                        // We only take into account session of 3 objects, one rig object and two glyphbox objects
                        if (objects.Count() != 3 || objects.Where(o => o.GetType() == typeof(RigObject)).Count() != 1)
                        {
                            writer.WriteEndElement();
                            continue;
                        }

                        var rigObject = objects.Where(o => o.GetType() == typeof(RigObject)).First();
                        var glyphObjects = objects.Where(o => o.GetType() == typeof(GlyphBoxObject)).ToList();

                        // By default, we will insert the rig object at the first place, two other objects are inserted based on their order in the session
                        writer.WriteStartElement("data");

                        for (int frameNo = 0; frameNo < session.sessionLength; frameNo++)
                        {
                            writer.WriteStartElement("frame");
                            writer.WriteAttributeString("no", "" + frameNo);
                            var rigMark = rigObject.getLocationMark3DLeftExtrapolated(frameNo);
                            var glyphMarks = glyphObjects.Select(g => g.getLocationMark3DLeftExtrapolated(frameNo)).ToList();

                            writer.WriteStartElement("o");
                            writer.WriteAttributeString("type", "rig");
                            List<float> frameData = new List<float>();
                            insertDataFromJointsToFrameData(frameData, (rigMark as RigLocationMark3D).rigFigure.rigJoints);
                            writer.WriteString(String.Join(",", frameData));
                            writer.WriteEndElement();

                            foreach (var glyphMark in glyphMarks)
                            {
                                writer.WriteStartElement("o");
                                writer.WriteAttributeString("type", "glyph");

                                frameData = new List<float>();
                                foreach (var p in fixListOfPoints((glyphMark as GlyphBoxLocationMark3D).boundingPolygons[0]))
                                {
                                    frameData.Add(p.X);
                                    frameData.Add(p.Y);
                                    frameData.Add(p.Z);
                                }
                                writer.WriteString(String.Join(",", frameData));
                                writer.WriteEndElement();
                            }

                            // end of writer.WriteStartElement("frame");
                            writer.WriteEndElement();
                        }

                        // end of writer.WriteStartElement("data");
                        writer.WriteEndElement();

                        writer.WriteStartElement("events");
                        // For each event, output a tuple of 5 elements
                        // 1. Role of rig = Subject or None
                        // 2. Role of first glyph = Subject or Object or Theme or None
                        // 3. Role of second glyph = Subject or Object or Theme or None
                        // 4. Verb type = Verb or None
                        // 5. Preposition type = Preposition or None
                        // Note that if subject == None -> Verb == None && Preposition type == None
                        // Preposition == None iff No role == Theme
                        // Verb == None iff tuple = [None, None, None, None, None]

                        // Verb type list = [Pull, Push, Roll, Slide]
                        // Preposition list = [Across, To, From]

                        string[] prepositions = new string[] { "Across", "From", "To" };
                        String[] tuple = new string[] { "None", "None" , "None" , "None" , "None" };

                        foreach (Event e in session.events)
                        {
                            writer.WriteStartElement("event");
                            writer.WriteAttributeString("start", "" + e.startFrame);
                            writer.WriteAttributeString("end", "" + e.endFrame);
                            StringBuilder sb = new StringBuilder();
                            // Assume that the order of objects is Subject, Object, Theme

                            // Sort the references depends on the location on the text
                            var newReferences = new Event.Reference[e.references.Count];
                            e.references.CopyTo(newReferences);
                            var newReferencesList = newReferences.ToList();
                            newReferencesList.Sort(new ReferenceComparer());

                            if (newReferencesList.Count == 0)
                            {
                                writer.WriteString(String.Join(",", tuple));
                                writer.WriteEndElement();
                                continue;
                            }

                            string subjectId = newReferencesList[0].refObjectId;

                            setRole(rigObject, glyphObjects[0], glyphObjects[1], subjectId, "Subject", tuple);

                            var verb = e.text.Substring(newReferencesList[0].end + 1).Trim().Split(new char[] { ' ' })[0];

                            tuple[3] = verb;

                            // Next word is the verb type
                            if (newReferencesList.Count == 1)
                            {
                                writer.WriteString(String.Join(",", tuple));
                                writer.WriteEndElement();
                                continue;
                            }

                            if (newReferencesList[1].start < e.text.IndexOf(verb) + verb.Length + 1)
                                continue;

                            var strBtwVerbAndNextObject = e.text.Substring(e.text.IndexOf(verb) + verb.Length + 1, newReferencesList[1].start - e.text.IndexOf(verb) - verb.Length - 1);
                            
                            // newReferencesList[1] is object
                            if ( strBtwVerbAndNextObject.Trim() == "" )
                            {
                                string objectId = newReferencesList[1].refObjectId;

                                setRole(rigObject, glyphObjects[0], glyphObjects[1], objectId, "Object", tuple);
                            } else
                            // newReferencesList[1] is theme
                            {
                                string themeId = newReferencesList[1].refObjectId;

                                foreach (var preposition in prepositions)
                                {
                                    if (strBtwVerbAndNextObject.Trim().Contains(preposition.ToLower()))
                                    {
                                        tuple[4] = preposition;
                                        break;
                                    }
                                }
                                setRole(rigObject, glyphObjects[0], glyphObjects[1], themeId, "Theme", tuple);
                            }

                            if (newReferencesList.Count == 2)
                            {
                                writer.WriteString(String.Join(",", tuple));
                                writer.WriteEndElement();
                                continue;
                            }

                            // At this point, it is assumed that the last object is a theme
                            // and the preposition is between two objects
                            if (newReferencesList[2].start <= newReferencesList[1].end)
                                continue;

                            var strBtwTwoObjects = e.text.Substring(newReferencesList[1].end + 1, newReferencesList[2].start - newReferencesList[1].end);

                            foreach (var preposition in prepositions)
                            {
                                if (strBtwTwoObjects.Trim().Contains(preposition.ToLower()))
                                {
                                    tuple[4] = preposition;
                                    break;
                                }
                            }
                            setRole(rigObject, glyphObjects[0], glyphObjects[1], newReferencesList[2].refObjectId, "Theme", tuple);

                            writer.WriteString(String.Join(",", tuple));
                            writer.WriteEndElement();
                        }
                        // end of writer.WriteStartElement("event");
                        writer.WriteEndElement();

                        // end of writer.WriteStartElement("session");
                        writer.WriteEndElement();
                    }


                    // end of writer.WriteStartElement("project");
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void setRole(Object o1, Object o2, Object o3, string id, string role, string [] output)
        {
            if (o1.id == id)
            {
                output[0] = role;
            }

            if (o2.id == id)
            {
                output[1] = role;
            }

            if (o3.id == id)
            {
                output[2] = role;
            }
        }
    }
}
