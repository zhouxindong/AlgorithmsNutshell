using System;

namespace AlgorithmsNutshell.Search
{
    public class SimpleSearch
    {
        /// <summary>
        /// 二分查找
        /// 在预先排好序的集合上进行数据查找
        /// O(1)    O(logn)     O(logn)
        /// 
        /// search(A, t)
        /// 1. low = 0
        /// 2. hight = n-1
        /// 3. while(low lessequal high) do
        /// 4.  ix = (low+high)/2
        /// 5.  if(t==A[ix]) then
        /// 6.      return true
        /// 7.  else if(t less A[ix]) then
        /// 8.      high = ix-1
        /// 9.  else low = ix+1
        /// 10. return false
        /// end
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="t"></param>
        public bool BinarySearch<T>(T[] data, T t)
            where T : IComparable
        {
            if (t == null) return false;
            int low = 0, high = data.Length - 1;
            while (low <= high)
            {
                int ix = (low + high)/2;
                int rc = t.CompareTo(data[ix]);
                if (rc < 0)
                    high = ix - 1;
                else if (rc > 0)
                    low = ix + 1;
                else
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 基于散列的查找
        /// O(1)    O(1)    O(n)
        /// 
        /// loadTable(size, C)
        /// 1. A = new array of given size
        /// 2. for i=0 to n-1 do
        /// 3.  h = hash(C[i])
        /// 4.  if(A[h] is empty) then
        /// 5.      A[h] = new LinkedList
        /// 6.  add C[i] to A[h]
        /// 7. return A
        /// end
        /// 
        /// search(A, t)
        /// 1. h = hash(t)
        /// 2. list = A[h]
        /// 3. if(list is empty) then
        /// 4.  return false
        /// 5. if(list contains t) then
        /// 6.  return true;
        /// 7. return false
        /// end
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="hash_size"></param>
        /// <returns></returns>
        public bool HashSearch<T>(T[] data, int hash_size)
        {
            return false; // TODO: no implement
        }

        public int HashCode(string s)
        {
            int h = 0;
            foreach (char t in s)
            {
                h = 31*h + t;
            }
            return h;
        }

        /*
        * 顺序查找：适用于小规模数据
        * 折半查找：适用于已排序数据
        * 散列查找：适用于数据规模不定、非序数据，但数据不变
        * 二叉树查找：数据规模任意，数据集高度动态，需要升序或降序遍历元素
        */
        /* 如果需要从任何节点起有序遍历数据集，则节点的结构中必须包括指向父节点的指针
         * 如果数据是动态的，必须平衡树(AVL树或红黑树)
         */

    }
}