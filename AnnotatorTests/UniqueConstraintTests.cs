using Microsoft.VisualStudio.TestTools.UnitTesting;
using Annotator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator.Tests
{
    [TestClass()]
    public class UniqueConstraintTests
    {
        [TestMethod()]
        public void ParseTest()
        {
            UniqueConstraint.Parse("EAT(X,#Y)");

            UniqueConstraint.Parse("EAT(#X,#Y)");
        }
    }
}