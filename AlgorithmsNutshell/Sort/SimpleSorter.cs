using System;
using System.CodeDom;
using System.Diagnostics.Contracts;

namespace AlgorithmsNutshell.Sort
{
    public class SimpleSorter
    {
        /// <summary>
        /// 插入排序
        /// O(n)    O(n^2)  O(n^2)
        /// sort(A)
        /// 1. for i=1 to n-1 do
        /// 2.  insert(A, i, A[i])
        /// end
        /// 
        /// insert(A, pos, value)
        /// 1. i = pos-1
        /// 2. while(i>=0 and A[i]>value) do
        /// 3.  A[i+1] = A[i]
        /// 4.  i=i-1
        /// 5. A[i+1] = value
        /// end
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public void InsertSort<T>(T[] data, int left, int right)
            where T : IComparable
        {
            if (data == null || data.Length <= 1 || left == right)
                return;

            for (var pos = left + 1; pos < right + 1; pos++)
            {
                var index = pos - 1;
                var value = data[pos];
                while (index >= left && data[index].CompareTo(value) > 0)
                {
                    data[index + 1] = data[index];
                    index--;
                }
                data[index + 1] = value;
            }
        }

        /// <summary>
        /// 中值排序 
        /// O(n logn)   O(n logn)   O(n^2)
        /// sort(A)
        /// 1.  medianSort(A, 0, n-1)
        /// end
        /// 
        /// medianSort(A, left, right)
        /// 1. if(left less right) then
        /// 2.  find median value A[me] in A[left, right]
        /// 3.  mid=(right+left)/2
        /// 4.  swap A[mid] and A[me]
        /// 5.  for i=left to mid-1 do
        /// 6.      if(A[i]>A[mid]) then
        /// 7.          find A[k] lessequal A[mid] where k>mid
        /// 8.          swap A[i] and A[k]
        /// 9.  medianSort(A, left, mid-1)
        /// 10. medianSort(A, mid+1, right)
        /// end
        /// </summary>
        public void MedianSort<T>(T[] data, int left, int right)
            where T : IComparable
        {
            if (right <= left) return;
            var mid = (right - left + 1)/2;
            var me = SelectKth(data, mid + 1, left, right);
            MedianSort(data, left, left + mid - 1);
            MedianSort(data, left + mid + 1, right);
        }

        /// <summary>
        /// 将[pivot_index]存储在正确的位置store，[left, store)中的元素都小于等于pivot
        /// [store+1, right]中的元素>pivot
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="pivot_index"></param>
        /// <returns>[pivot_index]在data中的正确位置索引</returns>
        public int Partition<T>(T[] data, int left, int right, int pivot_index)
            where T : IComparable
        {
            int idx, store;
            T pivot = data[pivot_index]; // 保存中枢值

            // 将中枢值移至末尾
            T tmp = data[right];
            data[right] = data[pivot_index];
            data[pivot_index] = tmp;

            // 将所有小于等于中枢值的元素移到数组前面，然后将中枢值插在它们后面
            store = left;
            for (idx = left; idx < right; idx++)
            {
                if (data[idx].CompareTo(pivot) <= 0)
                {
                    tmp = data[idx];
                    data[idx] = data[store];
                    data[store] = tmp;
                    store++;
                }
            }
            tmp = data[right];
            data[right] = data[store];
            data[store] = tmp;
            return store;
        }

        /// <summary>
        /// 在数组data中寻找第k大元素的位置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="k"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public int SelectKth<T>(T[] data, int k, int left, int right)
            where T : IComparable
        {
            int idx = SelectPivotIndex(data, left, right);
            int pivot_index = Partition(data, left, right, idx);
            if (left + k - 1 == pivot_index) return pivot_index;

            return left + k - 1 < pivot_index
                ? SelectKth(data, k, left, pivot_index - 1)
                : SelectKth(data, k - (pivot_index - left + 1), pivot_index + 1, right);
        }

        private int SelectPivotIndex<T>(T[] data, int left, int right)
        {
            return (left + right)/2;
        }

        /// <summary>
        /// 快速排序
        /// O(n logn)   O(n logn)   O(n^2)
        /// sort(A)
        /// 1. quickSort(A, 0, n-1)
        /// end
        /// 
        /// quickSort(A, left, right)
        /// 1. if(left less right) then
        /// 2.  pi = partition(A, left, right)
        /// 3.  quickSort(A, left, pi-1)
        /// 4.  quickSort(A, pi+1, right)
        /// end
        /// 
        /// partition(A, left, right)
        /// 1. p=select pivot in A[left, right]
        /// 2. swap A[p] and A[right]
        /// 3. store=left
        /// 4. for i=left to right-1 do
        /// 5.  if(A[i] lessequal A[right] then
        /// 6.      swap A[i] and A[store]
        /// 7.      store++
        /// 8. swap A[store] and A[right]
        /// 9. return store
        /// end
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="min_size"></param>
        public void QuickSort<T>(T[] data, int left, int right, int min_size = 4)
            where T : IComparable
        {
            int pivot_index;
            if (right <= left) return;

            // 切分(最理想是平均切分原始数组)
            pivot_index = SelectPivotIndex(data, left, right);
            pivot_index = Partition(data, left, right, pivot_index);

            if (pivot_index - 1 - left <= min_size) // 待排序规模小于阈值时使用插入排序
            {
                InsertSort(data, left, pivot_index - 1);
            }
            else
            {
                QuickSort(data, left, pivot_index - 1, min_size);
            }
            if (right - pivot_index - 1 <= min_size)
            {
                InsertSort(data, pivot_index + 1, right);
            }
            else
            {
                QuickSort(data, pivot_index + 1, right, min_size);
            }
        }

        /// <summary>
        /// 堆排序
        /// 性质: 一个节点大于任意一个子节点
        /// 如何将堆存储在数组中而不损失任何结构信息:
        /// 1. 给堆中每个节点都赋予一个整数值标签，根节点为0
        /// 2. 对于每一个标记为i的节点，其左子节点为2*i+1, 其右子节点为2*i+2
        /// 3. 反之，对于一个标记为i的非根节点，其父节点为(i-1)/2
        /// 4. 在数组中标签即为元素的索引位
        /// 
        /// O(n logn)   O(n logn)   O(n logn)
        /// sort(A)
        /// 1. buildHeap(A)
        /// 2. for i=n-1 downto 1 do
        /// 3.  swap A[0] with A[i]
        /// 4.  heapify(A, 0, i);
        /// end
        /// 
        /// buildHeap(A)
        /// 1. for i=n/2 -1 downto 0 do
        /// 2.  heapify(A, i, n)
        /// end
        /// 
        /// heapify(A, idx, max)
        /// 1. left = 2*idx + 1
        /// 2. right = 2*idx + 2
        /// 3. if(left less max and A[left] > A[idx] then
        /// 4.  largest = left
        /// 5. else largest = idx
        /// 6. if(right less max and A[right] > A[largest] then
        /// 7.  largest = right
        /// 8. if(largest != idx) then
        /// 9.  swap A[i] and A[largest]
        /// 10. heapify(A, largest, max)
        /// end
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        public void HeapSort<T>(T[] data)
            where T : IComparable
        {
            int i;
            BuildHeap(data, data.Length);
            for (i = data.Length - 1; i >= 1; i--)
            {
                T tmp = data[0];
                data[0] = data[i];
                data[i] = tmp;
                Heapify(data, 0, i);
            }
        }

        private void Heapify<T>(T[] data, int idx, int max)
            where T : IComparable
        {
            int left = 2*idx + 1;
            int right = 2*idx + 2;
            int largest;

            // 在A[idx], A[left], A[right]中寻找最大的元素
            if (left < max && data[left].CompareTo(data[idx]) > 0)
            {
                largest = left;
            }
            else
            {
                largest = idx;
            }
            if (right < max && data[right].CompareTo(data[largest]) > 0)
            {
                largest = right;
            }

            // 如果最大的不是父节点，那么交换并递归执行
            if (largest != idx)
            {
                T tmp = data[idx];
                data[idx] = data[largest];
                data[largest] = tmp;
                Heapify<T>(data, largest, max);
            }
        }

        private void BuildHeap<T>(T[] data, int n)
            where T : IComparable
        {
            int i;
            for (i = n/2 - 1; i >= 0; i--)
            {
                Heapify(data, i, n);
            }
        }

        /// <summary>
        /// 计数排序
        /// 1. 不需要一个比较函数
        /// 2. 对元素值固定在[0, k)的整数排序是最佳选择
        /// 3. n >> k(数组中有许多重复元素)
        /// 
        /// O(n)    O(n)    O(n)
        /// sort(A)
        /// 1. create k buckets B
        /// 2. for i=0 to n-1 do
        /// 3.  B[A[i]]++
        /// 4. idx = 0
        /// 5. for i=0 to k-1 do
        /// 6.  while(B[i]>0) do
        /// 7.      A[idx++] = i
        /// 8.      B[i]--
        /// end
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="k">数组中的元素值范围[0, k)</param>
        public void CountingSort(int[] data, int k)
        {
            if (data == null || data.Length <= 1) return;
            var idx = 0;
            var buckets = new int[k];
            foreach (var t in data)
            {
                buckets[t]++;
            }
            for (var i = 0; i < k; i++)
            {
                while (buckets[i]-- > 0)
                {
                    data[idx++] = i;
                }
            }
        }
    }
}