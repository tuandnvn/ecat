using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    public class RigFigure<T>
    {
        public Dictionary<string, T> rigJoints { get; }
        public List<Tuple<string, string>> rigBones { get; }

        public RigFigure(Dictionary<string, T> rigJoints, List<Tuple<string, string>> rigBones)
        {
            this.rigBones = rigBones;
            this.rigJoints = rigJoints;
        }
    }

}
