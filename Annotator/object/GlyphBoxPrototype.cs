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
        public string prototypeName { get; private set; }

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
        public GlyphBoxPrototype(string prototypeName, Dictionary<int, GlyphFace> indexToGlyphFaces, int glyphSize)
        {
            this.prototypeName = prototypeName;
            this.indexToGlyphFaces = indexToGlyphFaces;
            this.glyphSize = glyphSize;
        }

        public static GlyphBoxPrototype prototype1;
        public static GlyphBoxPrototype prototype2;
        public static GlyphBoxPrototype prototype3;
        public static GlyphBoxPrototype prototype4;

        static GlyphBoxPrototype(){
            // string _426 = Properties.Resources._426;

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


            prototype1 = new GlyphBoxPrototype("", indexToGlyphFaces, 3);

            indexToGlyphFaces = new Dictionary<int, GlyphFace>();
            indexToGlyphFaces[4] = getGlyphFaceFromByteArrayMirror(Properties.Resources._5);
            indexToGlyphFaces[3] = getGlyphFaceFromByteArrayMirror(Properties.Resources._477);
            indexToGlyphFaces[0] = getGlyphFaceFromByteArrayMirror(Properties.Resources._71);

            //indexToGlyphFaces[1] = indexToGlyphFaces[2] = indexToGlyphFaces[3] = indexToGlyphFaces[4] = indexToGlyphFaces[5] = indexToGlyphFaces[0];

            prototype2 = new GlyphBoxPrototype("Stella Artois", indexToGlyphFaces, 5);

            indexToGlyphFaces = new Dictionary<int, GlyphFace>();
            indexToGlyphFaces[4] = getGlyphFaceFromByteArrayMirror(Properties.Resources._6);
            indexToGlyphFaces[1] = getGlyphFaceFromByteArrayMirror(Properties.Resources._566);
            indexToGlyphFaces[0] = getGlyphFaceFromByteArrayMirror(Properties.Resources._155);
            // indexToGlyphFaces[1] = indexToGlyphFaces[2] = indexToGlyphFaces[3] = indexToGlyphFaces[4] = indexToGlyphFaces[5] = indexToGlyphFaces[0];

            prototype3 = new GlyphBoxPrototype("Pepsi", indexToGlyphFaces, 5);

            indexToGlyphFaces = new Dictionary<int, GlyphFace>();
            indexToGlyphFaces[4] = getGlyphFaceFromByteArrayMirror(Properties.Resources._426);
            indexToGlyphFaces[1] = getGlyphFaceFromByteArrayMirror(Properties.Resources._303);
            indexToGlyphFaces[2] = getGlyphFaceFromByteArrayMirror(Properties.Resources._521);
            // indexToGlyphFaces[1] = indexToGlyphFaces[2] = indexToGlyphFaces[3] = indexToGlyphFaces[4] = indexToGlyphFaces[5] = indexToGlyphFaces[0];

            prototype4 = new GlyphBoxPrototype("Shell", indexToGlyphFaces, 5);
        }

        /**
         * byteArray: A resource file that define the glyph face
         */
        static GlyphFace getGlyphFaceFromByteArray(byte[] byteArray)
        {
            String[] lines = Encoding.Default.GetString(byteArray).Split('\n');
            int size = lines.Length;

            bool[,] values = new bool[size, size];
            
            for (int i = 0; i < size; i ++ )
            {
                String line = lines[i];

                if (line.Length != size)
                {
                    throw new ArgumentException("Wrong form of byte array");
                }

                for (int j = 0; j < size; j ++)
                {
                    char c = line[j];
                    bool v = c == '1' ? true : false;
                    values[i, j] = v;
                }
            }

            return new GlyphFace(values, size);
        }

        /**
         * 
         * Because the video is mirror, you might want to get the mirror GlyphFace instead
         */
        static GlyphFace getGlyphFaceFromByteArrayMirror(byte[] byteArray)
        {
            String[] lines = Encoding.Default.GetString(byteArray).Split('\n');
            int size = lines.Length;

            bool[,] values = new bool[size, size];

            for (int i = 0; i < size; i++)
            {
                String line = lines[i];

                if (line.Length != size)
                {
                    throw new ArgumentException("Wrong form of byte array");
                }

                for (int j = 0; j < size; j++)
                {
                    char c = line[j];
                    bool v = c == '1' ? true : false;
                    values[i, size - 1 - j] = v;
                }
            }

            return new GlyphFace(values, size);
        }

        static int Main(string[] args)
        {
            Console.WriteLine(Encoding.Default.GetString(Properties.Resources._426));
            return 0;
        }
    }
}
