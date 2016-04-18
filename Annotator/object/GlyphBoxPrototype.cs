using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    /// <summary>
    /// Each glyph face is a square matrix of 1 and 0
    /// Glyph face is considered invariant to 90-degree rotating (but not flipping)
    /// </summary>
    public class GlyphFace
    {
        public bool[,] glyphValues { get; private set; }
        public int glyphSize { get; private set; }

        public GlyphFace(bool[,] glyphValues, int glyphSize)
        {
            this.glyphValues = glyphValues;
            this.glyphSize = glyphSize;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            GlyphFace other = (GlyphFace)obj;

            bool[,] otherGlyphValues = other.glyphValues;

            if (glyphSize != other.glyphSize) return false;
            bool same = true;

            // exactly the same
            for (int i = 0; i < glyphSize; i++)
                for (int j = 0; j < glyphSize; j++)
                {
                    if (this.glyphValues[i, j] != otherGlyphValues[i, j])
                    {
                        same = false;
                        break;
                    }
                }

            if (same) return true;
            same = true;

            // Rotate 90 degree
            for (int i = 0; i < glyphSize; i++)
                for (int j = 0; j < glyphSize; j++)
                {
                    if (this.glyphValues[i, j] != otherGlyphValues[j, glyphSize - 1 - i])
                    {
                        same = false;
                        break;
                    }
                }

            if (same) return true;
            same = true;

            // Rotate 180 degree
            for (int i = 0; i < glyphSize; i++)
                for (int j = 0; j < glyphSize; j++)
                {
                    if (this.glyphValues[i, j] != otherGlyphValues[glyphSize - 1 - i, glyphSize - 1 - j])
                    {
                        same = false;
                        break;
                    }
                }

            if (same) return true;
            same = true;

            // Rotate 270 degree
            for (int i = 0; i < glyphSize; i++)
                for (int j = 0; j < glyphSize; j++)
                {
                    if (this.glyphValues[i, j] != otherGlyphValues[glyphSize - 1 - j, i])
                    {
                        same = false;
                        break;
                    }
                }

            if (same) return true;
            same = true;

            return false;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            int hashCode1 = 0;
            int hashCode2 = 0;
            int hashCode3 = 0;
            int hashCode4 = 0;

            for (int i = 0; i < glyphSize; i++)
                for (int j = 0; j < glyphSize; j++)
                {
                    hashCode1 = hashCode1 * 2 + Convert.ToInt16(this.glyphValues[i, j]);
                }

            for (int i = 0; i < glyphSize; i++)
                for (int j = 0; j < glyphSize; j++)
                {
                    hashCode1 = hashCode1 * 2 + Convert.ToInt16(this.glyphValues[j, glyphSize - 1 - i]);
                }

            for (int i = 0; i < glyphSize; i++)
                for (int j = 0; j < glyphSize; j++)
                {
                    hashCode1 = hashCode1 * 2 + Convert.ToInt16(this.glyphValues[glyphSize - 1 - i, glyphSize - 1 - j]);
                }

            for (int i = 0; i < glyphSize; i++)
                for (int j = 0; j < glyphSize; j++)
                {
                    hashCode1 = hashCode1 * 2 + Convert.ToInt16(this.glyphValues[glyphSize - 1 - j, i]);
                }


            return (new int[] { hashCode1, hashCode2, hashCode3, hashCode4 }).Min();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("");
            for (int i = 0; i < glyphSize; i++)
            {
                for (int j = 0; j < glyphSize; j++)
                {
                    sb.Append((glyphValues[i, j]) ? "1 " : "0 ");
                    sb.Append(",");
                }
            }
            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }
    }

    public class GlyphBoxPrototype
    {
        public Dictionary<int, GlyphFace> indexToGlyphFaces { get; private set; }
        public int glyphSize { get; private set; }

        /// <summary>
        /// Prototype for a glyphbox
        /// Each face of the box is a glyph of same size.
        /// Faces are indexed by a right-hand 3 coordinate at the center of the box.
        /// Details:
        /// - Face that perpendicular to Ox and on the positive side is indexed 0.
        /// - _________________________________ on the negative side is indexed 1.
        /// - ___________________________Oy____ on the positive side is indexed 2.
        /// - _________________________________ on the negative side is indexed 3.
        /// - ___________________________Oz____ on the positive side is indexed 4.
        /// - _________________________________ on the negative side is indexed 5.
        /// </summary>
        public GlyphBoxPrototype(Dictionary<int, GlyphFace> indexToGlyphFaces, int glyphSize)
        {
            this.indexToGlyphFaces = indexToGlyphFaces;
            this.glyphSize = glyphSize;
        }

        public static GlyphBoxPrototype prototype1;
        public static GlyphBoxPrototype prototype2;

        static GlyphBoxPrototype(){
            var indexToGlyphFaces = new Dictionary<int, GlyphFace>();
            indexToGlyphFaces[0] = new GlyphFace(new bool[,] {  { true, true,  true},
                                                                { true, false, false },
                                                                { false,true,  false } }, 3);
            indexToGlyphFaces[1] = new GlyphFace(new bool[,] {  { false, true, false},
                                                                { true,  true, true },
                                                                { true,  false,true } }, 3);
            indexToGlyphFaces[2] = new GlyphFace(new bool[,] {  { false, true,  true},
                                                                { true,  false, true },
                                                                { false, true,  true } }, 3);
            indexToGlyphFaces[3] = new GlyphFace(new bool[,] {  { true, false, false},
                                                                { true, true,  true },
                                                                { true, true,  false } }, 3);
            indexToGlyphFaces[4] = new GlyphFace(new bool[,] {  { false, true,  true},
                                                                { true,  true,  true },
                                                                { false, true,  true } }, 3);
            indexToGlyphFaces[5] = new GlyphFace(new bool[,] {  { true, false, false},
                                                                { false,true,  true },
                                                                { true, true,  false } }, 3);


            prototype1 = new GlyphBoxPrototype(indexToGlyphFaces, 3);

            indexToGlyphFaces = new Dictionary<int, GlyphFace>();
            indexToGlyphFaces[0] = new GlyphFace(new bool[,] {  { false, false, false, false, true},
                                                                { false, false, false, false, true},
                                                                { false, false, false, false, true},
                                                                { true, true, true, false, true},
                                                                { true, true, true, false, true}, }, 5);
            indexToGlyphFaces[1] = new GlyphFace(new bool[,] {  { true, false, false, false, false},
                                                                { true, false, false, false, false},
                                                                { true, false, false, false, false},
                                                                { true, false, true, true, true},
                                                                { true, false, true, true, true}, }, 5);
            indexToGlyphFaces[2] = new GlyphFace(new bool[,] {  { true, false, false, false, false},
                                                                { true, false, false, false, false},
                                                                { true, false, false, false, false},
                                                                { true, false, true, true, true},
                                                                { true, false, true, true, true}, }, 5);
            indexToGlyphFaces[3] = new GlyphFace(new bool[,] {  { true, false, false, false, false},
                                                                { true, false, false, false, false},
                                                                { true, false, false, false, false},
                                                                { true, false, true, true, true},
                                                                { true, false, true, true, true}, }, 5);
            indexToGlyphFaces[4] = new GlyphFace(new bool[,] {  { true, false, false, false, false},
                                                                { true, false, false, false, false},
                                                                { true, false, false, false, false},
                                                                { true, false, true, true, true},
                                                                { true, false, true, true, true}, }, 5);
            indexToGlyphFaces[5] = new GlyphFace(new bool[,] {  { true, false, false, false, false},
                                                                { true, false, false, false, false},
                                                                { true, false, false, false, false},
                                                                { true, false, true, true, true},
                                                                { true, false, true, true, true}, }, 5);


            prototype2 = new GlyphBoxPrototype(indexToGlyphFaces, 5);
        }
    }
}
