using AlgorithmsNutshell.Sort;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlgorithmsNutshell.Sort.Tests
{
    [TestClass()]
    public class SimpleSorterTests
    {
        [TestMethod()]
        public void InsertSortTest()
        {
            var data = new int[] { 5, 3, 6, 10, 9 };
            new SimpleSorter().InsertSort(data, 0, data.Length - 1);
            Assert.AreEqual(5, data.Length);
            Assert.AreEqual(3, data[0]);
            Assert.AreEqual(5, data[1]);
            Assert.AreEqual(6, data[2]);
            Assert.AreEqual(9, data[3]);
            Assert.AreEqual(10, data[4]);

            data = new int[] { 5, 4, 3, 2, 1 };
            new SimpleSorter().InsertSort(data, 0, data.Length - 1);
            Assert.AreEqual(5, data.Length);
            Assert.AreEqual(1, data[0]);
            Assert.AreEqual(2, data[1]);
            Assert.AreEqual(3, data[2]);
            Assert.AreEqual(4, data[3]);
            Assert.AreEqual(5, data[4]);

            data = new int[] { 5, 4, 3, 2, 1 };
            new SimpleSorter().InsertSort(data, 1, 3);
            Assert.AreEqual(5, data.Length);
            Assert.AreEqual(5, data[0]);
            Assert.AreEqual(2, data[1]);
            Assert.AreEqual(3, data[2]);
            Assert.AreEqual(4, data[3]);
            Assert.AreEqual(1, data[4]);

            data = new int[] { 5, 3, 6, 10, 9 };
            new SimpleSorter().InsertSort(data, 1, 3);
            Assert.AreEqual(5, data.Length);
            Assert.AreEqual(5, data[0]);
            Assert.AreEqual(3, data[1]);
            Assert.AreEqual(6, data[2]);
            Assert.AreEqual(10, data[3]);
            Assert.AreEqual(9, data[4]);
        }

        [TestMethod()]
        public void PartitionTest()
        {
            var data = new int[] { 15, 9, 8, 1, 4, 11, 7, 12, 13, 6, 5, 3, 16, 2, 10, 14 };
            var p = new SimpleSorter().Partition(data, 0, data.Length - 1, 9);
            Assert.AreEqual(16, data.Length);
            Assert.AreEqual(1, data[0]);
            Assert.AreEqual(4, data[1]);
            Assert.AreEqual(5, data[2]);
            Assert.AreEqual(3, data[3]);
            Assert.AreEqual(2, data[4]);
            Assert.AreEqual(6, data[5]);
            Assert.AreEqual(7, data[6]);
            Assert.AreEqual(12, data[7]);
            Assert.AreEqual(13, data[8]);
            Assert.AreEqual(14, data[9]);
            Assert.AreEqual(8, data[10]);
            Assert.AreEqual(15, data[11]);
            Assert.AreEqual(16, data[12]);
            Assert.AreEqual(9, data[13]);
            Assert.AreEqual(10, data[14]);
            Assert.AreEqual(11, data[15]);
        }

        [TestMethod()]
        public void MedianSortTest()
        {
            var data = new int[] { 15, 9, 8, 1, 4, 11, 7, 12, 13, 6, 5, 3, 16, 2, 10, 14 };
            new SimpleSorter().MedianSort(data, 0, data.Length - 1);
            Assert.AreEqual(16, data.Length);
            for (int i = 0; i < data.Length; i++)
            {
                Assert.AreEqual(i + 1, data[i]);
            }
        }

        [TestMethod()]
        public void QuickSortTest()
        {
            var data = new int[] { 15, 9, 8, 1, 4, 11, 7, 12, 13, 6, 5, 3, 16, 2, 10, 14 };
            new SimpleSorter().QuickSort(data, 0, data.Length - 1);
            Assert.AreEqual(16, data.Length);
            for (int i = 0; i < data.Length; i++)
            {
                Assert.AreEqual(i + 1, data[i]);
            }

            data = new int[] { 5, 3, 6, 10, 9 };
            new SimpleSorter().QuickSort(data, 0, data.Length - 1);
            Assert.AreEqual(5, data.Length);
            Assert.AreEqual(3, data[0]);
            Assert.AreEqual(5, data[1]);
            Assert.AreEqual(6, data[2]);
            Assert.AreEqual(9, data[3]);
            Assert.AreEqual(10, data[4]);

            data = new int[] { 5, 4, 3, 2, 1 };
            new SimpleSorter().QuickSort(data, 0, data.Length - 1);
            Assert.AreEqual(5, data.Length);
            Assert.AreEqual(1, data[0]);
            Assert.AreEqual(2, data[1]);
            Assert.AreEqual(3, data[2]);
            Assert.AreEqual(4, data[3]);
            Assert.AreEqual(5, data[4]);

            data = new int[] { 5, 4, 3, 2, 1 };
            new SimpleSorter().QuickSort(data, 1, 3);
            Assert.AreEqual(5, data.Length);
            Assert.AreEqual(5, data[0]);
            Assert.AreEqual(2, data[1]);
            Assert.AreEqual(3, data[2]);
            Assert.AreEqual(4, data[3]);
            Assert.AreEqual(1, data[4]);

            data = new int[] { 5, 3, 6, 10, 9 };
            new SimpleSorter().QuickSort(data, 1, 3);
            Assert.AreEqual(5, data.Length);
            Assert.AreEqual(5, data[0]);
            Assert.AreEqual(3, data[1]);
            Assert.AreEqual(6, data[2]);
            Assert.AreEqual(10, data[3]);
            Assert.AreEqual(9, data[4]);
        }

        [TestMethod()]
        public void HeapSortTest()
        {
            var data = new int[] { 15, 9, 8, 1, 4, 11, 7, 12, 13, 6, 5, 3, 16, 2, 10, 14 };
            new SimpleSorter().HeapSort(data);
            Assert.AreEqual(16, data.Length);
            for (int i = 0; i < data.Length; i++)
            {
                Assert.AreEqual(i + 1, data[i]);
            }

            data = new int[] { 5, 3, 6, 10, 9 };
            new SimpleSorter().HeapSort(data);
            Assert.AreEqual(5, data.Length);
            Assert.AreEqual(3, data[0]);
            Assert.AreEqual(5, data[1]);
            Assert.AreEqual(6, data[2]);
            Assert.AreEqual(9, data[3]);
            Assert.AreEqual(10, data[4]);

            data = new int[] { 5, 4, 3, 2, 1 };
            new SimpleSorter().HeapSort(data);
            Assert.AreEqual(5, data.Length);
            Assert.AreEqual(1, data[0]);
            Assert.AreEqual(2, data[1]);
            Assert.AreEqual(3, data[2]);
            Assert.AreEqual(4, data[3]);
            Assert.AreEqual(5, data[4]);
        }

        [TestMethod()]
        public void CountingSortTest()
        {
            var data = new int[] {3, 0, 2, 0, 0, 2, 2};
            new SimpleSorter().CountingSort(data, 4);
            Assert.AreEqual(7, data.Length);
            Assert.AreEqual(0, data[0]);
            Assert.AreEqual(0, data[1]);
            Assert.AreEqual(0, data[2]);
            Assert.AreEqual(2, data[3]);
            Assert.AreEqual(2, data[4]);
            Assert.AreEqual(2, data[5]);
            Assert.AreEqual(3, data[6]);
        }
    }
}