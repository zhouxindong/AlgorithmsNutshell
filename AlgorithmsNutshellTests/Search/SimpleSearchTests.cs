using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlgorithmsNutshell.Search;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using AlgorithmsNutshell.Sort;

namespace AlgorithmsNutshell.Search.Tests
{
    [TestClass()]
    public class SimpleSearchTests
    {
        [TestMethod()]
        public void BinarySearchTest()
        {
            var data = new int[10000];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = i;
            }
            var searcher = new SimpleSearch();
            Assert.IsTrue(searcher.BinarySearch(data, 0));
            Assert.IsTrue(searcher.BinarySearch(data, 9999));
            Assert.IsFalse(searcher.BinarySearch(data, 10000));
        }
    }
}