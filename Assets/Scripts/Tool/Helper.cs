using System.Collections.Generic;
using UnityEngine;

namespace Tool
{
    public static class Helper
    {
        public static List<string> ConvertStringToStringList(string input)
        {
            List<string> strings = new List<string>();

            string[] parts = input.Split(',');

            foreach (string part in parts)
            {
                strings.Add(part.Trim());
            }

            return strings;
        }

        public static List<int> ConvertStringToIntList(string input)
        {
            List<int> numbers = new List<int>();

            string[] parts = input.Split(',');

            foreach (string part in parts)
            {
                if (int.TryParse(part.Trim(), out int number))
                {
                    numbers.Add(number);
                }
                else
                {
                    Debug.Log($"Invalid input: {part}");
                }
            }

            return numbers;
        }
    }
}