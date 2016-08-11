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
    public class Predicate
    {
        [DataMember]
        internal string predicate;
        [DataMember]
        internal Permutation combination;

        /// <summary>
        /// Example of Predicate:
        /// PART_OF(A,B) translate to
        /// predicate = PART_OF
        /// combination = [1,2]
        /// </summary>
        /// <param name="predicate">Predicate form</param>
        /// <param name="combination">Combination is the order of X,Y,Z in the predicate formula</param>
        public Predicate(string predicate, Permutation combination)
        {
            this.predicate = predicate;
            this.combination = combination;
        }

        public override string ToString()
        {
            return predicate + "(" + combination.ToString() + ")";
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(Predicate)) return false;
            var casted = obj as Predicate;

            if (predicate != casted.predicate) return false;
            if (!combination.Equals(casted.combination)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            int prime = 31;
            int result = 1;
            result = prime * result + ((predicate == null) ? 0 : predicate.GetHashCode());
            result = prime * result + ((combination == null) ? 0 : combination.GetHashCode());
            return result;
        }

        public static Predicate Parse(String value)
        {
            Regex rgx = new Regex(@"^([a-zA-Z0-9_]+)\(([X-Y](,[X-Y])?)\)$");

            if (!rgx.IsMatch(value))
            {
                return null;
            }

            var m = rgx.Match(value);
            string pred = m.Groups[1].Captures[0].Value;
            string argumentForms = m.Groups[2].Captures[0].Value;

            Predicate newPredicate = null;

            // Unary
            if (argumentForms.Length == 1)
            {
                newPredicate = new Predicate(pred, new Permutation(new int[1] { 1 }));
            }

            // Binary
            if (argumentForms == "X,Y")
            {
                newPredicate = new Predicate(pred, new Permutation(new int[2] { 1, 2 }));
            }

            if (argumentForms == "Y,X")
            {
                newPredicate = new Predicate(pred, new Permutation(new int[2] { 2, 1 }));
            }

            return newPredicate;
        }

        /// <summary>
        /// Previously, predicate doesn't have specifications of arguments
        /// Parse a predicate string form to Predicate with one argument
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Predicate ParseToUnary(String value)
        {
            Regex rgx = new Regex(@"^[a-zA-Z0-9_]+$");

            if (!rgx.IsMatch(value))
            {
                return null;
            }

            var m = rgx.Match(value);
            string pred = m.Groups[0].Captures[0].Value;

            Predicate newPredicate = null;

            return new Predicate(value, new Permutation(new int[] { 1 }));
        }

        /// <summary>
        /// Previously, predicate doesn't have specifications of arguments
        /// Parse a predicate string form to Predicate with two argument
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Predicate ParseToBinary(String value)
        {
            Regex rgx = new Regex(@"^[a-zA-Z0-9_]+$");

            if (!rgx.IsMatch(value))
            {
                return null;
            }

            var m = rgx.Match(value);
            string pred = m.Groups[0].Captures[0].Value;

            Predicate newPredicate = null;

            return new Predicate(value, new Permutation(new int[] { 1, 2 }));
        }
    }

    [DataContract]
    public class Combination
    {
        [DataMember]
        internal int size;
        [DataMember]
        internal int[] values;

        public Combination(int size, int[] values)
        {
            this.size = size;
            var v = new HashSet<int>(values);
            if (v.Count > size || (size > 0 && (v.Max() > size || v.Min() < 1))) throw new ArgumentException("Values should be a combination from 1 to values.Count");
            this.values = values;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                if (i != 0) sb.Append(',');
                sb.Append((char)((int)'X' + values[i] - 1));
            }
            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(Permutation)) return false;
            var casted = obj as Permutation;

            if (size != casted.size) return false;
            if (!values.SequenceEqual(casted.values)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            int prime = 31;
            int result = 1;
            result = prime * result + size;
            for (int i = 0; i < values.Length; i++)
            {
                result = prime * result + ((values[i] == null) ? 0 : values[i].GetHashCode());
            }
            return result;
        }
    }

    [DataContract]
    public class Permutation
    {
        [DataMember]
        internal int size;
        [DataMember]
        internal int[] values;

        public Permutation(int[] values)
        {
            this.size = values.Length;
            var v = new HashSet<int>(values);
            if (v.Count != size || (size > 0 && (v.Max() != size || v.Min() != 1))) throw new ArgumentException("Values should be a permutation of 1 to values.Count");
            this.values = values;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                if (i != 0) sb.Append(',');
                sb.Append((char)((int)'X' + values[i] - 1));
            }
            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(Permutation)) return false;
            var casted = obj as Permutation;

            if (size != casted.size) return false;
            if (!values.SequenceEqual(casted.values)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            int prime = 31;
            int result = 1;
            result = prime * result + size;
            for (int i = 0; i < values.Length; i++)
            {
                result = prime * result + ((values[i] == null) ? 0 : values[i].GetHashCode());
            }
            return result;
        }
    }
}
