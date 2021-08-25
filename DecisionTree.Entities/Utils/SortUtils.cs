using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree.Entities.Utils
{
    public class SortUtils
    {     
        // assuming min heap
        private static void heapify<T>(T[] arr, int bound, int root) where T : IComparable<T>
        {
            int smallest = root, lInx = (2 * root) + 1, rInx = (2 * root) + 2;
            if (lInx < bound && arr[lInx].CompareTo(arr[root]) < 1)
            {
                smallest = lInx;
            }
            if (rInx < bound && arr[rInx].CompareTo(arr[smallest]) < 1)
            {
                smallest = rInx;
            }
            if(smallest != root)
            {
                Swap(ref arr[smallest], ref arr[root]);
                heapify(arr, bound, smallest);
            }
        }

        private static void heapify<T>(T[] arr, int bound, int root, Comparison<T> comp) where T : IComparable<T>
        {
            int smallest = root, lInx = (2 * root) + 1, rInx = (2 * root) + 2;
            if (lInx < bound && comp(arr[lInx],arr[smallest]) < 1)
            {
                smallest = lInx;
            }
            if (rInx < bound && comp(arr[rInx],arr[smallest]) < 1)
            {
                smallest = rInx;
            }
            if (smallest != root)
            {
                Swap(ref arr[smallest], ref arr[root]);
                heapify(arr, bound, smallest, comp);
            }
        }

        public static void BuildHeap<T>(T[] arr, Comparison<T> comp) where T : IComparable<T>
        {
            for (int c = (arr.Length / 2) - 1; c > 0; c--)
            {
                heapify(arr, arr.Length, c, comp);
            }
        }

        public static T[] HeapSort<T>(T[] arr, Comparison<T> comp) where T : IComparable<T>
        {
            var newArr = new T[arr.Length];
            Array.Copy(arr, newArr, arr.Length);
            BuildHeap(newArr);
            // gradually "pop" elements off heap and re-heapify
            for (int k = newArr.Length - 1; k > 0; k--)
            {
                Swap(ref newArr[k], ref newArr[0]);
                heapify(newArr, k, 0, comp);
            }
            return newArr;
        }

        public static void BuildHeap<T>(T[] arr) where T : IComparable<T>
        {
            for(int c = (arr.Length / 2) - 1; c > 0; c--)
            {
                heapify(arr, arr.Length, c);
            }
        }

        //public static void SortHeap<T>(T[] arr) where T: IComparable<T>
        //{
        //    BuildHeap(arr);
        //    // gradually "pop" elements off heap and re-heapify
        //    for (int k = arr.Length - 1; k > 0; k--)
        //    {
        //        Swap(ref arr[k], ref arr[0]);
        //        heapify(arr, k, 0);
        //    }
        //}

        public static T[] HeapSort<T>(T[] arr) where T : IComparable<T>
        {
            T[] newArr = new T[arr.Length];
            Array.Copy(arr, newArr, arr.Length);
            BuildHeap(newArr);
            // gradually "pop" elements off heap and re-heapify
            for (int k = arr.Length - 1; k > 0; k--)
            {
                Swap(ref newArr[k], ref newArr[0]);
                heapify(newArr, k, 0);
            }
            // return sorted array
            return newArr;
        }

        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
    }
}
