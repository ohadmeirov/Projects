using System;
using System.Text;

namespace Ex01_02
{
    public class Program
    {
        public static void Main()
        {
            char[] lettersToPresent = new char[26];
            int timesLettersAppearInALine = 1;
            int spacesToMove = 11;
            int lastLetterPlacesToMove = 10;
            int currentPlaceInTheArray = 0;
            int numberOfTheRow = 1;
            for (int i = 0; i < lettersToPresent.Length; i++)
            {
                lettersToPresent[i] = (char)('A' + i);
            }
            lettersToPresent[25] = 'Z';
            TreeLetterPrinter(lettersToPresent, timesLettersAppearInALine, currentPlaceInTheArray, spacesToMove, lastLetterPlacesToMove, numberOfTheRow);

            Console.ReadLine();
        }

        public static void TreeLetterPrinter(char[] io_LettersToPresent, int i_TimesLettersAppearInALine, int i_CurrentPlaceInTheArray, int i_SpacesToMove , int i_LastLetterPlacesToMove, int i_NumberOfTheRow)
        {
            bool flagToCheckOneTimeInTheLoop = false;
            StringBuilder sbOneLineBeforeTheLastLine = new StringBuilder();
            StringBuilder sbTheLastLine = new StringBuilder();
            if (io_LettersToPresent.Length == i_CurrentPlaceInTheArray + 1)
            {
                sbOneLineBeforeTheLastLine.Append(i_NumberOfTheRow).Append(' ', i_LastLetterPlacesToMove);
                sbOneLineBeforeTheLastLine.Append('|').Append(io_LettersToPresent[i_CurrentPlaceInTheArray]).Append('|').Append('\n');
                sbTheLastLine.Append(i_NumberOfTheRow + 1).Append(' ', i_LastLetterPlacesToMove);
                sbTheLastLine.Append('|').Append(io_LettersToPresent[i_CurrentPlaceInTheArray]).Append('|').Append('\n');
                Console.WriteLine(sbOneLineBeforeTheLastLine.ToString());
                Console.WriteLine(sbTheLastLine.ToString());
                return;
            }

            flagToCheckOneTimeInTheLoop = true;
            for (int i = 0;i < i_TimesLettersAppearInALine; i++)
            {                
                if (flagToCheckOneTimeInTheLoop == true)
                {
                    Console.Write(i_NumberOfTheRow);
                    for (int j = 0; j < i_SpacesToMove; j++)
                    {
                        Console.Write(' ');
                    }
                    flagToCheckOneTimeInTheLoop = false;
                }
                    Console.Write(io_LettersToPresent[i_CurrentPlaceInTheArray + i] + " ");
            }
            Console.WriteLine();
            Console.WriteLine();
            TreeLetterPrinter(io_LettersToPresent, i_TimesLettersAppearInALine + 2, i_CurrentPlaceInTheArray + i_TimesLettersAppearInALine, i_SpacesToMove - 2, i_LastLetterPlacesToMove, i_NumberOfTheRow + 1);
        }              
    }
}
