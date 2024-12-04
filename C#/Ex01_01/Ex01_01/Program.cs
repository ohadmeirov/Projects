using System;

namespace Ex01_01
{
    class Program
    {

        static void Main()
        {
            Console.WriteLine("Type 3 binary numbers which each one is 8 in length, and then press enter: ");
            int numbersInTheArray = 3;

            string stringNumber1 = Console.ReadLine();
            int binaryNumber1 = ChecksIfNumberIsLegalAndIfNotFixHimAndSendItAsAnBinary(stringNumber1);    

            string stringNumber2 = Console.ReadLine();
            int binaryNumber2 = ChecksIfNumberIsLegalAndIfNotFixHimAndSendItAsAnBinary(stringNumber2);

            string stringNumber3 = Console.ReadLine();
            int binaryNumber3 = ChecksIfNumberIsLegalAndIfNotFixHimAndSendItAsAnBinary(stringNumber3);

            int[] binaryNumbersArray = new int[numbersInTheArray];
            binaryNumbersArray[0] = binaryNumber1;
            binaryNumbersArray[1] = binaryNumber2;
            binaryNumbersArray[2] = binaryNumber3;

            int decimalNumber1 = FromBinaryToDecimal(binaryNumber1);
            int decimalNumber2 = FromBinaryToDecimal(binaryNumber2);
            int decimalNumber3 = FromBinaryToDecimal(binaryNumber3);

            int[] decimalNumbersArray = new int[numbersInTheArray];
            decimalNumbersArray[0] = decimalNumber1;
            decimalNumbersArray[1] = decimalNumber2;
            decimalNumbersArray[2] = decimalNumber3;

            Array.Sort(decimalNumbersArray);
            Console.WriteLine("The decimal numbers in ascending order: ");
            for (int i = 0; i < decimalNumbersArray.Length; i++)
            {
                Console.Write(decimalNumbersArray[i] + " ");//Solves part A
            }

            double avgNumberOfTheArray = (decimalNumber1 + decimalNumber2 + decimalNumber3) / (double)numbersInTheArray;
            Console.WriteLine("\nThe Avg of those 3 decimal numbers is: " + Math.Round(avgNumberOfTheArray, 2)); // Solves part B

            int[] copyOfBinaryNumbersArray = (int[])binaryNumbersArray.Clone();
            FindsLongestBitsStrike(copyOfBinaryNumbersArray); //Solves part C

            copyOfBinaryNumbersArray = (int[])binaryNumbersArray.Clone();
            ExchangesTimesBetweenZeroAndOne(copyOfBinaryNumbersArray, numbersInTheArray);//Solves part D

            copyOfBinaryNumbersArray = (int[])binaryNumbersArray.Clone();
            NumberWithMostZeroAndLessOne(copyOfBinaryNumbersArray, numbersInTheArray);//Solves part E

            Console.ReadLine();
        }

        public static void NumberWithMostZeroAndLessOne(int[] i_BinaryNumbersArray, int i_NumbersInTheArray)
        {
            int conditionInTheWhileLoopToReview8Digits;
            int zeroCounter;
            int[] copyI_BinaryNumbersArray = (int[])i_BinaryNumbersArray.Clone();
            int maxZeroCounter = 0;
            int[] arrayOfBothZeroCountersAndBinaryNumbers = new int[i_NumbersInTheArray * 2];

            for (int i = 0;i < i_NumbersInTheArray * 2; i+=2)
            {
                conditionInTheWhileLoopToReview8Digits = 8;
                zeroCounter = 0;

                while (conditionInTheWhileLoopToReview8Digits != 0)
                {
                    if (copyI_BinaryNumbersArray[i / 2] % 10 == 0)
                    {
                        zeroCounter++;                       
                    }                  
                    conditionInTheWhileLoopToReview8Digits--;
                    copyI_BinaryNumbersArray[i / 2] /= 10;                
                }
                if(maxZeroCounter < zeroCounter)
                {
                    maxZeroCounter = zeroCounter;
                }
                arrayOfBothZeroCountersAndBinaryNumbers[i] = zeroCounter;
                arrayOfBothZeroCountersAndBinaryNumbers[i + 1] = i_BinaryNumbersArray[i / 2];
            }

            for (int i = 0; i < i_NumbersInTheArray * 2; i += 2)
            {
                if (arrayOfBothZeroCountersAndBinaryNumbers[i] == maxZeroCounter)
                {
                    if (arrayOfBothZeroCountersAndBinaryNumbers[i + 1] == 0)
                    {
                        Console.WriteLine("The number with most 0 and less 1 is: " + FromBinaryToDecimal(arrayOfBothZeroCountersAndBinaryNumbers[i + 1]) + "(00000000)");
                        break;
                    }
                    Console.WriteLine("The number with most 0 and less 1 is: " + FromBinaryToDecimal(arrayOfBothZeroCountersAndBinaryNumbers[i + 1]) + "(" + arrayOfBothZeroCountersAndBinaryNumbers[i + 1] + ")");
                    break;
                }
            }
        }

        public static void ExchangesTimesBetweenZeroAndOne(int[] i_BinaryNumbersArray, int i_NumbersInTheArray)
        {
            int conditionInTheWhileLoopToReview8Digits;
            int currentDigit;
            int counterOfSwaps;
            int[] copyI_BinaryNumbersArray = (int[])i_BinaryNumbersArray.Clone();

            for (int i = 0; i < i_NumbersInTheArray; i++)
            {
                counterOfSwaps = 0;
                conditionInTheWhileLoopToReview8Digits = 7;
                currentDigit = copyI_BinaryNumbersArray[i] % 10;
                copyI_BinaryNumbersArray[i] /= 10;

                while (conditionInTheWhileLoopToReview8Digits != 0)
                {
                    if (copyI_BinaryNumbersArray[i] % 10 != currentDigit)
                    {
                        counterOfSwaps++;                                    
                    }                   
                    conditionInTheWhileLoopToReview8Digits--;                  
                    currentDigit = copyI_BinaryNumbersArray[i] % 10;
                    copyI_BinaryNumbersArray[i] /= 10; 
                }
                if (i_BinaryNumbersArray[i] == 0)
                {
                    Console.WriteLine("Number of swaps: " + counterOfSwaps + "(00000000)");
                }
                else
                {
                Console.WriteLine("Number of swaps: " + counterOfSwaps + "(" + i_BinaryNumbersArray[i] + ")");
                }
            }
        }

        public static void FindsLongestBitsStrike(int[] i_BinaryNumbersArray)
        {
            int maxLongestBitsStrikeToReturnFinaly = 0;
            int counterToFindTheLongestBitsStrikeOnEveryNumber;
            int maxCounterAboutEachNumber;
            int[] copyI_BinaryNumbersArray = (int[])i_BinaryNumbersArray.Clone();
            int conditionInTheWhileLoopToReview8Digits;
            int[] findBitsStrike = new int[i_BinaryNumbersArray.Length * 2];

            for (int i = 0; i< i_BinaryNumbersArray.Length * 2; i+=2)
            {
                conditionInTheWhileLoopToReview8Digits = 8;
                maxCounterAboutEachNumber = 0;
                counterToFindTheLongestBitsStrikeOnEveryNumber = 1;

                while (conditionInTheWhileLoopToReview8Digits != 1)
                {
                    if (copyI_BinaryNumbersArray[i / 2] % 10 == (copyI_BinaryNumbersArray[i / 2] % 100) / 10)
                    {
                        counterToFindTheLongestBitsStrikeOnEveryNumber++;
                    }
                    else
                    {                                               
                        counterToFindTheLongestBitsStrikeOnEveryNumber = 1;
                    }
                    if(maxCounterAboutEachNumber < counterToFindTheLongestBitsStrikeOnEveryNumber)
                    {
                        maxCounterAboutEachNumber = counterToFindTheLongestBitsStrikeOnEveryNumber;
                    }
                    conditionInTheWhileLoopToReview8Digits--;
                    copyI_BinaryNumbersArray[i / 2] /= 10;
                }
                
                if(maxLongestBitsStrikeToReturnFinaly < maxCounterAboutEachNumber)
                {
                    maxLongestBitsStrikeToReturnFinaly = maxCounterAboutEachNumber;
                }
                findBitsStrike[i] = maxCounterAboutEachNumber;
                findBitsStrike[i + 1] = i_BinaryNumbersArray[i / 2];
            }
            
            for (int i = 0;i < findBitsStrike.Length; i+=2)
            {
                if (findBitsStrike[i] == maxLongestBitsStrikeToReturnFinaly)
                {
                    if (findBitsStrike[i + 1] == 0)
                    {
                        Console.WriteLine("The longest big strike is: " + maxLongestBitsStrikeToReturnFinaly + "(00000000)");
                        break;
                    }
                    Console.WriteLine("The longest big strike is: " + maxLongestBitsStrikeToReturnFinaly +"(" + findBitsStrike[i+1] + ")");
                    break;
                }
            }
        }


        public static int FromBinaryToDecimal(int i_BinaryNumber)
        {
            int lengthI_BinaryNumber = 8;
            int decimalValue = 0;
            int power = 0;

            for (int i = 0; i < lengthI_BinaryNumber; i++)
            {
                if ((i_BinaryNumber%10) == 1)
                {
                    decimalValue += (int)Math.Pow(2, power);                                  
                }
                i_BinaryNumber /= 10;
                power++;
            }

            return decimalValue;
        }


        public static int ChecksIfNumberIsLegalAndIfNotFixHimAndSendItAsAnBinary(string i_StringNumber)
        {
            int numberLength;
            bool checksIfNumbersOfDigitsLegal = false;
            bool checksIfEveryDigitIsANumber = false;
            do
            {
                numberLength = i_StringNumber.Length;
                if (numberLength == 8)
                {
                    checksIfNumbersOfDigitsLegal = true;         
                    for (int i = 0; i < numberLength; i++)
                    {
                        if (char.IsDigit(i_StringNumber[i]) && (i_StringNumber[i] < '2'))
                        {
                            checksIfEveryDigitIsANumber = true;
                        }
                        else
                        {
                            checksIfEveryDigitIsANumber = false;
                            Console.WriteLine("Fix this number please. You need an 8 digits binary number: ");
                            i_StringNumber = Console.ReadLine();
                            break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Fix this number please. You need an 8 digits binary number: ");
                    i_StringNumber = Console.ReadLine();
                }
            } while (checksIfNumbersOfDigitsLegal != true || checksIfEveryDigitIsANumber != true);

            int binaryNumber = Int32.Parse(i_StringNumber);

            return binaryNumber; 
        }
    }
}
