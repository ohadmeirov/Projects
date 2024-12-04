using System;
using Ex01_02;



namespace Ex01_03
{
    class Program
    {
        static void Main()
        {
            bool flagToCheckWheterItsAnActuallNumber = false;
            bool flagToCheckIfTheNumberIsBetween0To15 = false;
            int treeHeight;
            int timesLettersAppearInALine = 1;
            int spacesToMove;
            int numberOfTheRow = 1;
            int lastLetterPlacesToMove;
            int currentPlaceInTheArray = 0;
            do
            {
                Console.WriteLine("Type the tree height (from 0 to 15): ");
                flagToCheckWheterItsAnActuallNumber = Int32.TryParse(Console.ReadLine(), out treeHeight);
                if (treeHeight > -1 && treeHeight < 16)
                {
                    flagToCheckIfTheNumberIsBetween0To15 = true;
                }
            } while (!flagToCheckWheterItsAnActuallNumber || !flagToCheckIfTheNumberIsBetween0To15);

            Console.WriteLine("\n");


            if (treeHeight == 0)
            {
                Console.ReadLine();
                return;
            }
            else if (treeHeight == 1)
            {
                Console.WriteLine("1 |A|");
                Console.ReadLine();
                return;
            }


            

            int arrayLength = ((treeHeight - 2) * 2) - 1;
            int countArrayLengthTillIts0 = arrayLength - 2;
            while (countArrayLengthTillIts0 != -1)
            {
                arrayLength += countArrayLengthTillIts0;
                countArrayLengthTillIts0 -= 2;
            }
            arrayLength++;

            char[] lettersToPresent = new char[arrayLength];
            int copyIToFitAscii = 0;

            for (int i = 0; i < arrayLength; i++)
            {
                if(i % 26 == 0)
                {
                    copyIToFitAscii = 0;
                }
                lettersToPresent[i] = (char)('A' + copyIToFitAscii);
                copyIToFitAscii++;
            }

            spacesToMove = (int)((treeHeight * 1.7));
            lastLetterPlacesToMove = spacesToMove - 1;
            Ex01_02.Program.TreeLetterPrinter(lettersToPresent, timesLettersAppearInALine, currentPlaceInTheArray, spacesToMove, lastLetterPlacesToMove, numberOfTheRow);
            Console.ReadLine();
        }
    }
}
