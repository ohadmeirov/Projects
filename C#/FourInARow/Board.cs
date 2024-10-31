using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Media;
using System.Security.Cryptography.X509Certificates;


namespace FourInARow
{
    class Board : Cell
    {
        private Cell[,] board; // מטריצה של תאים

        public Board()
        {//בנאי של המחלקה, בניית טבלה
            this.board = new Cell[12, 13];
            int x = 200;
            int y = 0;
            for (int i = 0; i < board.GetLength(0); i++)
            {                 
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    Cell c = new Cell();
                    c.SetX(x);
                    c.SetY(y);
                    board[i, j] = c;
                    x += 100;
                }
                x = 200;
                y += 100;
            }
            SetReal();
        }
        public void SetReal()
        {//האם התא אמיתי, הכוונה האם הוא לא מחוץ למטריצה
            for (int i = 3; i < board.GetLength(0) - 3; i++)
                for (int j = 3; j < board.GetLength(1) - 3; j++)
                    board[i, j].SetReal(true);
         }                       


        public Cell[,] GetBoard()
        {
            return this.board;
        }

         
        public void PaintBoard(Graphics g)
        {  // מצייר את הטבלה ויזואלית
            for (int i = 3; i < 9 ; i++)
            {
                for (int j = 3; j < 10; j++)
                {
                        board[i, j].PaintCell(g);
                }
            }
        }
        public bool CheckWin(int num)
        {
            //הפעולה תקבל 1 כדי לבדוק ניצחון של השחקן
            //או שהפעולה תקבל 2 לבדוק ניצחון של המחשב
            for (int i = 3; i < board.GetLength(0) - 3; i++)
            {
                for (int j = 3; j < board.GetLength(1) - 3; j++)
                {
                    if (board[i, j].GetColor() != 0)
                    {
                        //בדיקה ימינה                        
                        if (board[i, j].GetColor() == board[i, j + 1].GetColor() && board[i, j].GetColor() == board[i, j + 2].GetColor() && board[i, j].GetColor() == board[i, j + 3].GetColor() && board[i, j + 1].GetColor() == num)
                        {
                            return true;
                        }
                        //בדיקה שמאלה 
                        if (board[i, j].GetColor() == board[i, j - 1].GetColor() && board[i, j].GetColor() == board[i, j - 2].GetColor() && board[i, j].GetColor() == board[i, j - 3].GetColor() && board[i, j - 1].GetColor() == num)
                        {
                            return true;
                        }
                        // בדיקה למעלה 
                        if (board[i, j].GetColor() == board[i + 1, j].GetColor() && board[i, j].GetColor() == board[i + 2, j].GetColor() && board[i, j].GetColor() == board[i + 3, j].GetColor() && board[i + 1, j].GetColor() == num)
                        {
                            return true;
                        }
                        //בדיקה למטה 
                        if (board[i, j].GetColor() == board[i - 1, j].GetColor() && board[i, j].GetColor() == board[i - 2, j].GetColor() && board[i, j].GetColor() == board[i - 3, j].GetColor() && board[i - 1, j].GetColor() == num)
                        {
                            return true;
                        }
                        //בדיקת אלכסון ימינה ולמעלה 
                        if (board[i, j].GetColor() == board[i + 1, j + 1].GetColor() && board[i, j].GetColor() == board[i + 2, j + 2].GetColor() && board[i, j].GetColor() == board[i + 3, j + 3].GetColor() && board[i + 1, j + 1].GetColor() == num)
                        {
                            return true;
                        }
                        //בדיקת אלכסון ימינה ולמטה 
                        if (board[i, j].GetColor() == board[i - 1, j + 1].GetColor() && board[i, j].GetColor() == board[i - 2, j + 2].GetColor() && board[i, j].GetColor() == board[i - 3, j + 3].GetColor() && board[i - 1, j + 1].GetColor() == num)
                        {
                            return true;
                        }
                        //בדיקת אלכסון שמאלה ולמעלה 
                        if (board[i, j].GetColor() == board[i + 1, j - 1].GetColor() && board[i, j].GetColor() == board[i + 2, j - 2].GetColor() && board[i, j].GetColor() == board[i + 3, j - 3].GetColor() && board[i + 1, j - 1].GetColor() == num)
                        {
                            return true;
                        }
                        //בדיקת אלכסון שמאלה ולמטה 
                        if (board[i, j].GetColor() == board[i - 1, j - 1].GetColor() && board[i, j].GetColor() == board[i - 2, j - 2].GetColor() && board[i, j].GetColor() == board[i - 3, j - 3].GetColor() && board[i - 1, j - 1].GetColor() == num)
                        {
                            return true;
                        }
                    }                      
                }
            }
            return false;
        }
    }
}
