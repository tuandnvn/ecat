using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Annotator
{
    public class PredicateMarkComparer : IComparer<PredicateMark>
    {
        public int Compare(PredicateMark x, PredicateMark y)
        {
            // Sort by frame
            if (x.frame < y.frame) return -1;
            if (x.frame > y.frame) return 1;

            // Sort by number of arguments
            if (x.predicate.combination.size < y.predicate.combination.size ) return -1;
            if (x.predicate.combination.size > y.predicate.combination.size ) return 1;

            // Sort by predicate string
            if (x.predicate.predicate.CompareTo(y.predicate.predicate) == -1) return -1;
            if (x.predicate.predicate.CompareTo(y.predicate.predicate) == 1) return 1;

            // Qualified before not qualified
            if (x.qualified && !y.qualified) return -1;
            if (!x.qualified && y.qualified) return 1;

            return x.GetHashCode() - y.GetHashCode();
        }
    }

    public class PredicateMark
    {
        internal int frame;
        internal bool qualified;
        internal Predicate predicate;
        internal Session currentSession;
        internal Object[] objects;

        public PredicateMark(int frame, bool qualified, Predicate predicate, Session currentSession, Object[] objects)
        {
            if (objects.Count() != predicate.combination.size)
            {
                throw new ArgumentException("Size of objects need to be the same as dimension of predicate");
            }
            this.frame = frame;
            this.qualified = qualified;
            this.currentSession = currentSession;
            this.predicate = predicate;
            this.objects = objects;
        }

        public bool isNegateOf(PredicateMark other)
        {
            if (!predicate.Equals(other.predicate)) return false;
            if (objects.Length != other.objects.Length) return false;
            for (int i = 0; i < objects.Length; i ++ )
            {
                if (objects[i] != other.objects[i]) return false;
            }
            if (this.qualified == other.qualified) return false;
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(PredicateMark)) return false;
            var casted = obj as PredicateMark;

            if (frame != casted.frame) return false;
            if (qualified != casted.qualified) return false;
            if (!predicate.Equals(casted.predicate)) return false;
            if (objects.Length != casted.objects.Length) return false;
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i] != casted.objects[i]) return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            int prime = 31;
            int result = 1;
            result = prime * result + frame;
            result = prime * result + (qualified ? 0 : 1);
            result = prime * result + ((predicate == null) ? 0 : predicate.GetHashCode());
            for (int i = 0; i < objects.Length; i++)
            {
                result = prime * result + ((objects[i] == null) ? 0 : objects[i].GetHashCode());
            }
            
            return result;
        }

        public override string ToString()
        {
            var pred = predicate.predicate;

            String q = predicate.predicate + "( " + String.Join(",", predicate.combination.values.Select(v =>
                      (objects[v - 1].session.name == currentSession.name ? "" : objects[v - 1].session.name + "/") +
                      objects[v - 1].id + (objects[v - 1].name.Equals("") ? "" : (" (\"" + objects[v - 1].name + "\")")))) + " )";
            if (!qualified)
            {
                q = "NOT( " + q + " )";
            }
            return q;
        }

        internal void writeToXml(XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartElement(Session.PREDICATE);

            xmlWriter.WriteAttributeString(Object.FRAME, "" + this.frame);
            xmlWriter.WriteAttributeString(Session.ARGUMENTS, "" + this.predicate.combination.size);
            xmlWriter.WriteAttributeString(Session.IDS, String.Join(",", objects.Select(o => (o.session.name == currentSession.name ? "" : o.session.name + "/") + o.id) ));
            xmlWriter.WriteAttributeString(Object.QUALIFIED, "" + this.qualified);
            xmlWriter.WriteAttributeString(Object.TYPE, this.predicate.ToString());
            xmlWriter.WriteEndElement();
        }
    }
}
