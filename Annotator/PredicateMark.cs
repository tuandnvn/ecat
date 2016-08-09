using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    public class PredicateMark
    {
        internal bool qualified;

        internal Predicate predicate;

        internal Object[] objects;

        public PredicateMark(bool qualified, Predicate predicate, Object[] objects)
        {
            if (objects.Count() != predicate.combination.size)
            {
                throw new ArgumentException("Size of objects need to be the same as dimension of predicate");
            }

            this.qualified = qualified;
            this.objects = objects;
            this.predicate = predicate;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(PredicateMark)) return false;
            var casted = obj as PredicateMark;

            if (qualified != casted.qualified) return false;
            if (!predicate.Equals(casted.predicate)) return false;
            if (!objects.Equals(casted.objects)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            int prime = 31;
            int result = 1;
            result = prime * result + (qualified ? 0 : 1);
            result = prime * result + ((predicate == null) ? 0 : predicate.GetHashCode());
            result = prime * result + ((objects == null) ? 0 : objects.GetHashCode());
            return result;
        }

        public override string ToString()
        {
            var pred = predicate.predicate;

            String q = predicate.predicate + "( " + String.Join(",", predicate.combination.values.Select(v =>
                      (objects[v - 1].session.sessionName == objects[0].session.sessionName ? "" : objects[v - 1].session.sessionName + "/") +
                      objects[v - 1].id + (objects[v - 1].name.Equals("") ? "" : (" (\"" + objects[v - 1].name + "\")")))) + " )";
            if (!qualified)
            {
                q = "NOT( " + q + " )";
            }
            return q;
        }
    }
}
