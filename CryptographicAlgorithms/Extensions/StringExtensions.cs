using System;
using System.Linq;

namespace CryptographicAlgorithms.Extensions {
    public static class StringExtensions {
        public static int[,] ToIntSquare(this string str) {
            var arr = str.Split(' ', '\n', '\r')
                .Where(x => x != "")
                .ToArray();
            var size = Convert.ToInt32(Math.Sqrt(arr.Length));
            var intArr = new int[size, size];
            int count = 0;
            for (int i = 0; i < size; i++) {
                for (int j = 0; j < size; j++) {
                    intArr[i, j] = int.Parse(arr[count]);
                    count++;
                }

            }
            return intArr;
        }

        public static char[,] ToCharSquare(this string str) {
            var arr = str.Split(' ', '\n', '\r')
                .Where(x => x != "")
                .ToArray();
            var size = Convert.ToInt32(Math.Sqrt(arr.Length));
            var charArr = new char[size, size];
            int count = 0;
            for (int i = 0; i < size; i++) {
                for (int j = 0; j < size; j++) {
                    charArr[i, j] = Convert.ToChar(arr[count]);
                    count++;
                }

            }
            return charArr;
        }

        public static char[,] FillTwoDimensArray(this char[,] arr, char value) {
            for (int i = 0; i < arr.GetLength(0); i++) {
                for (int j = 0; j < arr.GetLength(1); j++) {
                    arr[i, j] = value;
                }
            }
            return arr;
        }
    }
}