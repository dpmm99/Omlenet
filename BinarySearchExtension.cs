using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omlenet
{
    public static class BinarySearchExtension
    {
        /// <summary>
        /// Binary search to obtain a list of matching elements, preserving their original order
        /// </summary>
        /// <param name="list">List to search within</param>
        /// <param name="searchBy">Function to get a value from the intended member</param>
        /// <param name="searchFor"></param>
        /// <returns></returns>
        public static List<T> BinarySearch<T, U>(this List<T> list, Func<T, U> searchBy, U searchFor) where U : IComparable<U>
        {
            int left = 0, right = list.Count - 1, mid = 0;
            do
            {
                mid = (left + right) / 2; //Halfway point
                var comparison = searchBy(list[mid]).CompareTo(searchFor);
                if (comparison == 0) //If the first character matches, we're in the right region of the list
                {
                    //Get all the items nearby, preserving order, and return them
                    int min = mid, max = mid;
                    for (int x = mid - 1; x >= left && searchBy(list[x]).CompareTo(searchFor) == 0; x--) //Scan left until a mismatch
                    {
                        min = x;
                    }
                    for (int x = mid + 1; x <= right && searchBy(list[x]).CompareTo(searchFor) == 0; x++) //Scan right until a mismatch
                    {
                        max = x;
                    }
                    return list.Skip(min).Take(max - min + 1).ToList();
                }
                else if (comparison > 0)
                    //Search on left only
                    right = mid - 1;
                else if (comparison < 0)
                    //Search on right only
                    left = mid + 1;
            } while (left <= right);

            //Found nothing
            return new List<T>();
        }
    }
}
