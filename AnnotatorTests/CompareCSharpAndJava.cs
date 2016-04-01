using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AnnotatorTests
{
    [TestClass]
    public class CompareCSharpAndJava
    {
        /// <summary>
        /// Test C sharp features in compare with Java
        /// </summary>
        [TestMethod]
        public void TestGeneric()
        {
            // -----------------------
            // - Java: Cannot Instantiate Generic Types with Primitive Types
            // - CSharp: Can Instantiate Generic Types with Primitive Types and struct type

            List<int> t = new List<int>();

            // -----------------------
            // - Java: Cannot Create Instances of Type Parameters
            // - CSharp: Cannot Create Instances of Type Parameters
            // Both has a workaround using reflection
            // See public static void createInstance<T> ()
            int a = createInstance<int>();
            Assert.AreEqual(a, 0);

            // -----------------------
            // - Java: Cannot Declare Static Fields Whose Types are Type Parameters
            //         because that would make one static variable shared between concrete type
            // - CSharp: Can Declare Static Fields Whose Types are Type Parameters
            //           one static variable is created for one 
            // Use class Demo<T> below
            Demo<int>.q = 3;
            Demo<string>.q = 2;
            Assert.AreEqual(Demo<int>.q, 3);

            // -----------------------
            // - Java: Cannot Use Casts or instanceof with Parameterized Types
            //         The reason is that in runtime, generic parameter is erase (Type erasure)
            // - CSharp:  Can Use Casts or instanceof with Parameterized Types

            var l = new List<int>();
            Assert.IsTrue(l is List<int>);

            // -----------------------
            // - Java: Cannot Create Arrays of Parameterized Types
            // - CSharp:  Can Create Arrays of Parameterized Types
            //            However, assignment of wrong type throw System.ArrayTypeMismatchException 
            Object[] arrayOfList = new List<int>[1];
            try
            {
                arrayOfList[0] = new List<string>();
                Assert.Fail();
            }
            catch (System.ArrayTypeMismatchException e)
            {
            }

            // -----------------------
            // - Java: Cannot Catch, or Throw Objects of Parameterized Types
            //        A generic class cannot extend the Throwable class directly or indirectly. 
            //        class MathException<T> extends Exception { /* ... */ }    // compile-time error
            //        class QueueFullException<T> extends Throwable { /* ... */ // compile-time error
            // - CSharp: Can extend, catch and throw
            A<int> exc = new A<int>(4);
            try
            {
                throw exc;
            }
            catch (A<int> e) {
                Assert.AreEqual(exc.t, 4);
            }

            // -----------------------
            // - Java: Cannot Overload a Method Where the Formal Parameter Types of Each Overload Erase to the Same Raw Type
            // - CSharp: Can overload
            Assert.AreEqual(new CompareCSharpAndJava().doSomethingWithList(new List<int>()), 0);
            Assert.AreEqual(new CompareCSharpAndJava().doSomethingWithList(new List<string>()), 1);
            Assert.AreEqual(new CompareCSharpAndJava().doSomethingWithList(new List<object>()), 2);

            // This one doesn't compile because List<float> could not be casted to List<object>
            //Assert.AreEqual(new CompareCSharpAndJava().doSomethingWithList(new List<float>()), 2);

        }

        public int doSomethingWithList(List<int> l)
        {
            return 0;
        }

        public int doSomethingWithList(List<string> l)
        {
            return 1;
        }

        public int doSomethingWithList(List<object> l)
        {
            return 2;
        }

        public class A<T> : Exception
        {
            public T t;
            public A(T t)
            {
                this.t = t;
            }
        }

        public static T createInstance<T>()
        {
            // Cannot create instance of the variable type T because it doesn't not have 'new' constraint
            // T t = new T();

            T t = (T)Activator.CreateInstance(typeof(T));
            return t;
        }

        class Demo<T>
        {
            public static int q;
        }

        [TestMethod]
        public void TestReflection()
        {
            System.Type type = 40.GetType();
            Assert.AreEqual(type.ToString(), "System.Int32");

            System.Reflection.Assembly info = typeof(System.Int32).Assembly;
        }

        [TestMethod]
        public void TestRandom()
        {

        }
    }
}
