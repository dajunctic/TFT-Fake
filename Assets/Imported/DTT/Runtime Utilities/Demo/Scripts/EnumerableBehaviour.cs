using System;
using System.Collections.Generic;
using UnityEngine;

namespace DTT.Utils.Extensions.Demo
{
    public class EnumerableBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Uses an enumerable of integer numbers.
        /// </summary>
        /// <param name="numbers">The enumerable of integer numbers.</param>
        public void UseNumbersIEnumerable(IEnumerable<int> numbers)
        {
            // Check whether the numbers enumerable is null or empty and throw an exception if it is.
            if (numbers.IsNullOrEmpty())
                throw new ArgumentException("There were no numbers given because the IEnumerable was null or empty.");

            foreach (int number in numbers)
                Debug.Log(number);
        }
    }

}