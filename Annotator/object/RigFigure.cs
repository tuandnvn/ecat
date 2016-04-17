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
        public List<Tuple<T, T>> rigBones { get; }

        public RigFigure(Dictionary<string, T> rigJoints, List<Tuple<T, T>> rigBones)
        {
            this.rigBones = rigBones;
            this.rigJoints = rigJoints;
        }
    }

}
