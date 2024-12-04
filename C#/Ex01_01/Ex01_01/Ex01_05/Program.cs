using System;
using System.Text;

namespace Ex01_05
{
    class Program
    {
        static void Main()
        {
            string userNumber = GetValidUserInput();
            int actuallNumber = int.Parse(userNumber);

            string actuallNumberAsString = actuallNumber.ToString().PadLeft(9, '0');
            int actuallNumberLength = actuallNumberAsString.Length;
            int lastDigit = actuallNumber % 10;

            // Part 1 done
            int bigDigitsCount = CountBigDigitsThanLastDigit(actuallNumber, lastDigit, actuallNumberLength);
            Console.WriteLine($"Number of digits greater than last digit ({lastDigit}): {bigDigitsCount}");

            // Part 2 done
            string divisibleBy4Digits = CountDivisibleBy4Digits(actuallNumber, actuallNumberLength);
            Console.WriteLine(divisibleBy4Digits);

            // Part 3 done
            string ratioMessage = CalculateLargestSmallestDigitsRatio(actuallNumber, actuallNumberLength);
            Console.WriteLine(ratioMessage);

            // Part 4 done
            string duplicateDigitsMessage = CountDuplicateDigits(actuallNumberAsString);
            Console.WriteLine(duplicateDigitsMessage);

            Console.ReadLine();
        }

        public static string GetValidUserInput()
        {
            string userNumber;
            bool isNumber9Digits;
            bool isTheNumberLegal;

            do
            {
                Console.WriteLine("Please type a 9 digits number: ");
                userNumber = Console.ReadLine();
                isNumber9Digits = userNumber.Length == 9;
                isTheNumberLegal = Int32.TryParse(userNumber, out _);
            } while (!isNumber9Digits || !isTheNumberLegal);

            return userNumber;
        }

        public static int CountBigDigitsThanLastDigit(int i_ActuallNumber, int i_LastDigit, int i_ActuallNumberLength)
        {
            int count = 0;
            int copyOfActuallNumber = i_ActuallNumber / 10;

            for (int i = 0; i < i_ActuallNumberLength - 1; i++)
            {
                if (copyOfActuallNumber % 10 > i_LastDigit)
                {
                    count++;
                }
                copyOfActuallNumber /= 10;
            }
            return count;
        }

        public static string CountDivisibleBy4Digits(int i_ActuallNumber, int i_ActuallNumberLength)
        {
            int counter = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            int copyOfActuallNumber = i_ActuallNumber;

            for (int i = 0; i < i_ActuallNumberLength; i++)
            {
                int digit = copyOfActuallNumber % 10;
                if (digit % 4 == 0)
                {
                    counter++;
                    sb.Append(digit).Append(",");
                }
                copyOfActuallNumber /= 10;
            }

            if (sb.Length == 1)
            {
                return "Number of digits mod 4 is 0: 0";
            }
            else
            {
                sb[sb.ToString().LastIndexOf(',')] = ')';
                return $"Number of digits mod 4 is 0: {counter} {sb}";
            }
        }

        public static string CalculateLargestSmallestDigitsRatio(int i_ActuallNumber, int i_ActuallNumberLength)
        {
            int copyOfActuallNumber = i_ActuallNumber;
            int largestDigit = copyOfActuallNumber % 10;
            int smallestDigit = copyOfActuallNumber % 10;
            copyOfActuallNumber /= 10;

            for (int i = 0; i < i_ActuallNumberLength - 1; i++)
            {
                int currentDigit = copyOfActuallNumber % 10;
                if (currentDigit > largestDigit)
                {
                    largestDigit = currentDigit;
                }
                if (currentDigit != 0 && currentDigit < smallestDigit)
                {
                    smallestDigit = currentDigit;
                }
                copyOfActuallNumber /= 10;
            }

            if (smallestDigit == 0)
            {
                return "The smallest digit is 0. Cannot calculate the ratio.";
            }
            else
            {
                double ratio = (double)largestDigit / smallestDigit;
                return $"The ratio of the largest digit to the smallest digit is: {Math.Round(ratio, 2)} ({largestDigit}/{smallestDigit})";
            }
        }

        public static string CountDuplicateDigits(string i_ActuallNumberAsString)
        {
            int[] digitCounts = new int[10];
            foreach (char c in i_ActuallNumberAsString)
            {
                digitCounts[c - '0']++;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            int duplicateCount = 0;

            for (int i = 0; i < 10; i++)
            {
                if (digitCounts[i] > 1)
                {
                    sb.Append(i).Append(",");
                    duplicateCount++;
                }
            }

            if (sb.Length == 1)
            {
                return "There are no duplicate digits.";
            }
            else
            {
                sb[sb.ToString().LastIndexOf(',')] = ')';
                return $"Number of duplicate digits: {duplicateCount} {sb}";
            }
        }
    }
}
