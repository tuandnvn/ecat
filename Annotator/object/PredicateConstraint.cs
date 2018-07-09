using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Annotator
{
    [DataContract]
    [KnownType(typeof(UniqueConstraint))]
    [KnownType(typeof(ExclusiveConstraint))]
    public abstract class PredicateConstraint
    {
        /// <summary>
        /// Check if two predicate mark are conflict on the current predicate constraint
        /// 
        /// </summary>
        /// <param name="pm1"></param>
        /// <param name="pm2"></param>
        /// <returns></returns>
        public abstract bool isConflict(PredicateMark pm1, PredicateMark pm2);
    }

    [DataContract]
    public class UniqueConstraint : PredicateConstraint
    {
        [DataMember]
        /// <summary>
        /// Constraint is set on this predicate
        /// </summary>
        public Predicate predicate;

        [DataMember]
        /// <summary>
        /// If Predicate is 
        /// LOOK_AT(X,Y) 
        /// and constraint is LOOK_AT(X,#Y)
        /// => constraintedArgumentIndices = [2]
        /// </summary>
        public Combination constraintedArgumentIndices;

        public UniqueConstraint(Predicate predicate, Combination constraintedArgumentIndices)
        {
            this.predicate = predicate;
            this.constraintedArgumentIndices = constraintedArgumentIndices;
            if (predicate.combination.size != constraintedArgumentIndices.size)
            {
                throw new ArgumentException("predicate.combination.size needs to equal constraintedArgumentIndices.size");
            }
        }

        public static UniqueConstraint Parse(String text)
        {
            Regex rgx = new Regex(@"^([a-zA-Z0-9_]+)\(((#?)([X-Y]),(#?)([X-Y]))\)$");

            if (!rgx.IsMatch(text))
            {
                return null;
            }

            var m = rgx.Match(text);

            string predForm = m.Groups[1].Captures[0].Value;
            string unique1 = m.Groups[3].Captures[0].Value;
            string unique2 = m.Groups[5].Captures[0].Value;
            int arg1 = (int)m.Groups[4].Captures[0].Value[0];
            int arg2 = (int)m.Groups[6].Captures[0].Value[0];

            var predicate = new Predicate(predForm, new Permutation(new int[2] { arg1 - (int)'X' + 1, arg2 - (int)'X' + 1 }));

            List<int> indices = new List<int>();
            if (unique1 == "#")
            {
                indices.Add(1);
            }

            if (unique2 == "#")
            {
                indices.Add(2);
            }

            return new UniqueConstraint( predicate, new Combination(2, indices.ToArray()));
        }

        public override bool isConflict(PredicateMark pm1, PredicateMark pm2)
        {
            if (!pm1.qualified || !pm2.qualified)
                return false;

            if (!pm1.predicate.Equals(predicate) || !pm2.predicate.Equals(predicate))
            {
                return false;
            }

            if (!pm1.qualified || !pm2.qualified)
                return false;

            // Object index should be added 1 to match with constraintedArgumentIndices
            for (int i = 1; i <= predicate.combination.size; i ++ )
            {
                // On argument that is not uniquely constraint, they need to be the same
                if (!constraintedArgumentIndices.values.Contains(i) && !pm1.objects[i - 1].Equals(pm2.objects[i - 1]) )
                {
                    return false;
                }
            }

            for (int i = 1; i <= predicate.combination.size; i++)
            {
                // On argument that is not uniquely constraint, they need to be the same
                if (constraintedArgumentIndices.values.Contains(i) && !pm1.objects[i - 1].Equals(pm2.objects[i - 1]))
                {
                    return true;
                }
            }

            return false;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("UNIQUE=" + predicate.predicate);
            sb.Append("(");
            for (int i = 0; i < predicate.combination.size; i++)
            {
                if (i != 0) sb.Append(',');
                
                // Index is increased by 1 because combination has value started from 1
                if (constraintedArgumentIndices.values.Contains(i + 1))
                {
                    sb.Append('#');
                }
                sb.Append((char)((int)'X' + predicate.combination.values[i] - 1));
            }
            sb.Append(")");
            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(UniqueConstraint)) return false;
            var casted = obj as UniqueConstraint;

            if (!predicate.Equals(casted.predicate)) return false;
            if (!constraintedArgumentIndices.Equals(casted.constraintedArgumentIndices)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            int prime = 31;
            int result = 1;
            result = prime * result + ((predicate == null) ? 0 : predicate.GetHashCode());
            result = prime * result + ((constraintedArgumentIndices == null) ? 0 : constraintedArgumentIndices.GetHashCode());
            return result;
        }
    }

    [DataContract]
    public class ExclusiveConstraint : PredicateConstraint
    {
        [DataMember]
        /// <summary>
        /// Set of mutual exclusive unary predicate
        /// </summary>
        public HashSet<Predicate> predicates;

        public ExclusiveConstraint(HashSet<Predicate> predicates)
        {
            this.predicates = predicates;
        }

        public static ExclusiveConstraint Parse(String text)
        {
            HashSet<Predicate> predicates = new HashSet<Predicate>();

            Regex rgx = new Regex(@"^([a-zA-Z0-9_]+\(X\))(\s*#\s*([a-zA-Z0-9_]+\(X\)))+$");

            var m = rgx.Match(text);

            if (!rgx.IsMatch(text))
            {
                return null;
            }

            string firstForm = m.Groups[1].Captures[0].Value;
            var firstPred = Predicate.Parse(firstForm);
            predicates.Add(firstPred);

            foreach (Capture c in m.Groups[3].Captures)
            {
                string otherForm = c.Value;
                var otherPred = Predicate.Parse(otherForm);
                predicates.Add(otherPred);
            }

            return new ExclusiveConstraint(predicates);
        }

        public override bool isConflict(PredicateMark pm1, PredicateMark pm2)
        {
            if (!pm1.qualified || !pm2.qualified)
                return false;

            // Both predicate mark need to be unary
            if (pm1.predicate.combination.size != 1 || pm2.predicate.combination.size != 1)
                return false;

            // Predicates of the same object
            if (!pm1.objects[0].Equals(pm2.objects[0]))
                return false;

            if (predicates.Contains(pm1.predicate) && predicates.Contains(pm2.predicate) && !pm1.predicate.Equals(pm2.predicate) )
            {
                return true;
            }
            return false;
        }


        public override string ToString()
        {
            return "EXCLUSIVE=" + String.Join(" # ", predicates);
        }
    }

}
