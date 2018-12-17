using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlgorithmsNutshell.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgorithmsNutshell.Tree.Tests
{
    [TestClass()]
    public class RedBlackTreeTests
    {
        [TestMethod()]
        public void RedBlackTreeTest()
        {
            RedBlackTree<int, GenericParameterHelper> target = new RedBlackTree<int, GenericParameterHelper>();
            Assert.IsTrue(target.IsEmpty());
            Assert.AreEqual(0, target.Count);
        }

        [TestMethod()]
        public void AddTest()
        {
            var target = new RedBlackTree<int, GenericParameterHelper>();
            var data = new GenericParameterHelper(5);
            target.Add(5, data);
            Assert.AreEqual(new GenericParameterHelper(5), data, "Objects are not the same");
        }

        [TestMethod()]
        public void ClearTest()
        {
            var target = new RedBlackTree<int, GenericParameterHelper>();
            Assert.IsTrue(target.IsEmpty());
            target.Add(5, new GenericParameterHelper(5));
            Assert.AreEqual(1, target.Count);
            target.Clear();
            Assert.IsTrue(target.IsEmpty());
        }

        [TestMethod()]
        public void ContainsTest()
        {
            var target = new RedBlackTree<int, GenericParameterHelper>();
            var item = new GenericParameterHelper(5);
            const bool expected = false;
            bool actual = target.Contains(item);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void CopyToTest()
        {
            var target = new RedBlackTree<string, GenericParameterHelper>();
            var array = new KeyValuePair<string, GenericParameterHelper>[1];
            const int arrayIndex = 0;
            target.CopyTo(array, arrayIndex);
        }

        [TestMethod()]
        public void EqualsTest()
        {
            var target = new RedBlackTree<string, GenericParameterHelper>();
            object obj = 5;
            const bool expected = false;
            bool actual = target.Equals(obj);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetDataTest()
        {
            var target = new RedBlackTree<int, GenericParameterHelper> {{5, new GenericParameterHelper(5)}};
            var expected = new GenericParameterHelper(5);
            var actual = target.GetData(5);
            Assert.AreEqual(expected, actual);
        }

        private void GetEnumeratorTestHelper<TKey, T>(KeyValuePair<TKey, T>[] items)
            where T : class
            where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
        {
            RedBlackTree<TKey, T> target = new RedBlackTree<TKey, T>();
            foreach (KeyValuePair<TKey, T> item in items)
            {
                target.Add(item);
            }
            List<T> list = new List<T>(target.Select(i => i.Value));
            Assert.AreEqual(3, list.Count);
        }

        [TestMethod]
        public void GetEnumeratorTest()
        {
            GetEnumeratorTestHelper(
                new[]
                {
                    new KeyValuePair<int, GenericParameterHelper>
                        (5, new GenericParameterHelper(5)),
                    new KeyValuePair<int, GenericParameterHelper>
                        (6, new GenericParameterHelper(6)),
                    new KeyValuePair<int, GenericParameterHelper>
                        (7, new GenericParameterHelper(7))
                });
        }

        public void GetMaxKeyTestHelper<TKey, T>(KeyValuePair<TKey, T>[] items, TKey max)
            where T : class
            where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
        {
            RedBlackTree<TKey, T> target = new RedBlackTree<TKey, T>();
            foreach (KeyValuePair<TKey, T> item in items)
                target.Add(item);
            TKey expected = max;
            IComparable actual = target.GetMaxKey();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetMaxKeyTest()
        {
            GetMaxKeyTestHelper(
                new[]
                {
                    new KeyValuePair<int, GenericParameterHelper>(
                        5, new GenericParameterHelper(5)),
                    new KeyValuePair<int, GenericParameterHelper>(
                        10, new GenericParameterHelper(10)),
                    new KeyValuePair<int, GenericParameterHelper>(
                        6, new GenericParameterHelper(6))
                }, 10);
        }

        public void GetMaxValueTestHelper<TKey, T>(KeyValuePair<TKey, T>[] items, T max)
            where T : class
            where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
        {
            RedBlackTree<TKey, T> target = new RedBlackTree<TKey, T>();
            foreach (KeyValuePair<TKey, T> item in items)
                target.Add(item);
            T expected = max;
            T actual = target.GetMaxValue();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetMaxValueTest()
        {
            GetMaxValueTestHelper(new[]
            {
                new KeyValuePair<int, GenericParameterHelper>(
                    5, new GenericParameterHelper(5)),
                new KeyValuePair<int, GenericParameterHelper>(
                    10, new GenericParameterHelper(10)),
                new KeyValuePair<int, GenericParameterHelper>(
                    6, new GenericParameterHelper(6))
            }, new GenericParameterHelper(10));
        }

        public void GetMinKeyTestHelper<TKey, T>(KeyValuePair<TKey, T>[] items, TKey min)
            where T : class
            where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
        {
            RedBlackTree<TKey, T> target = new RedBlackTree<TKey, T>();
            foreach (KeyValuePair<TKey, T> item in items)
                target.Add(item);
            IComparable expected = min;
            IComparable actual = target.GetMinKey();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetMinKeyTest()
        {
            GetMinKeyTestHelper(new[]
            {
                new KeyValuePair<int, GenericParameterHelper>(
                    5, new GenericParameterHelper(5)),
                new KeyValuePair<int, GenericParameterHelper>(
                    10, new GenericParameterHelper(10)),
                new KeyValuePair<int, GenericParameterHelper>(
                    6, new GenericParameterHelper(6))
            }, 5);
        }

        public void GetMinValueTestHelper<TKey, T>(KeyValuePair<TKey, T>[] items, T min)
            where T : class
            where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
        {
            RedBlackTree<TKey, T> target = new RedBlackTree<TKey, T>();
            foreach (KeyValuePair<TKey, T> item in items)
                target.Add(item);
            T expected = min;
            T actual = target.GetMinValue();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetMinValueTest()
        {
            GetMinValueTestHelper(new[]
            {
                new KeyValuePair<int, GenericParameterHelper>(
                    5, new GenericParameterHelper(5)),
                new KeyValuePair<int, GenericParameterHelper>(
                    10, new GenericParameterHelper(10)),
                new KeyValuePair<int, GenericParameterHelper>(
                    6, new GenericParameterHelper(6))
            }, new GenericParameterHelper(5));
        }


        public void RemoveTestHelper<TKey, T>(KeyValuePair<TKey, T> item)
            where T : class
            where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
        {
            RedBlackTree<TKey, T> target = new RedBlackTree<TKey, T> {item};
            const bool expected = true;
            bool actual = target.Remove(item);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveTest()
        {
            RemoveTestHelper(
                new KeyValuePair<int, GenericParameterHelper>(5, new GenericParameterHelper(5)));
        }

    }
}