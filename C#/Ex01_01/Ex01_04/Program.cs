using System;


namespace Ex01_04
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Please type a 10 letters or digits string: ");
            string userString = Console.ReadLine();

            userString = ChecksIfAStingIsLegal(userString);

            int pointsOnStartOfString = 0;
            int pointsOnEndOfString = userString.Length - 1;
            //The string is Legal.
            IsPalindrom(userString, pointsOnStartOfString, pointsOnEndOfString); // Part 1 done

            bool checkIfALetterIsLower = false;
            int counterWheterALetterIsLower = 0;
            bool checkWheterTheStringIsANumber = Int32.TryParse(userString, out int theStringAsANumber);
            int counterWheterItsADistinctAlphabeticOrder = 0;
            if (checkWheterTheStringIsANumber)
            { //Its a number, part 2 done
                if (theStringAsANumber % 4 == 0)
                {
                    Console.WriteLine("Is it divisible by 4 without a remainder: Yes");
                }
                else
                {
                    Console.WriteLine("Is it divisible by 4 without a remainder: No");
                }
            }
            else //Its a word
            {
                for (int i = 0; i < userString.Length - 1; i++)
                {
                    checkIfALetterIsLower = char.IsLower(userString[i]);
                    if (checkIfALetterIsLower)
                    {
                        counterWheterALetterIsLower++;
                    }

                    if (userString[i] > userString[i + 1])
                    {
                        counterWheterItsADistinctAlphabeticOrder++;
                    }
                }

                checkIfALetterIsLower = char.IsLower(userString[userString.Length - 1]);
                if (checkIfALetterIsLower)
                {
                    counterWheterALetterIsLower++;
                }
                Console.WriteLine("The number of lower cases in this word is: " + counterWheterALetterIsLower); // Part 3 done

                if (counterWheterItsADistinctAlphabeticOrder == userString.Length - 1)
                {
                    Console.WriteLine("Is in a distinct alphabetic order: yes");
                }
                else
                {
                    Console.WriteLine("Is in a distinct alphabetic order: no"); // Part 4 done
                }
            } 

            Console.ReadLine();
        }

        public static void IsPalindrom(string i_UserString, int i_PointsOnStartOfString, int i_PointsOnEndOfString)
        {
            if (i_UserString[i_PointsOnStartOfString] != i_UserString[i_PointsOnEndOfString])
            {
                Console.WriteLine("Is palindrom: no");
                return;
            }
            if(i_PointsOnStartOfString >= i_PointsOnEndOfString) 
            {
                Console.WriteLine("Is palindrom: yes");
                return;
            }
            IsPalindrom(i_UserString, i_PointsOnStartOfString + 1, i_PointsOnEndOfString - 1);
        }



        public static string ChecksIfAStingIsLegal(string io_String)
        {
            bool stringLegal = false;
            int counterLetters = 0;
            int counterDigits = 0;
            int stringLength = io_String.Length;
            int index = 0;
            do 
            {
                if (stringLength == 10)
                {
                    if (char.IsLetterOrDigit(io_String[index]))
                    {
                        if (char.IsLetter(io_String[index]))
                        {
                            counterLetters++;
                        }
                        else
                        {
                            counterDigits++;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Fix this string please. You need a 10 letters or digits string: ");
                        io_String = Console.ReadLine();
                        index = 0;
                        continue;
                    }                    
                    index++;
                    if ((counterLetters == 10) || (counterDigits == 10))
                    {
                        stringLegal = true;
                        break;
                    }
                    else if (counterLetters + counterDigits == 10)
                    {
                        Console.WriteLine("Fix this string please. You need a 10 letters or digits string: ");
                        io_String = Console.ReadLine();
                        index = 0;
                        continue;
                    }
                }
                else 
                { 
                Console.WriteLine("Fix this string please. You need a 10 letters or digits string: ");
                io_String = Console.ReadLine();
                index = 0;
                continue;
                }
            } while (!stringLegal);

            return io_String;
        }

    }
}
