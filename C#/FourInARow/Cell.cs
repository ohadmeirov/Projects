using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace FourInARow
{
    class Cell : MouseClickForCell
    {       
        private int color; // אחד - דיסקית אדומה ||| שתים - דיסקית צהובה
        private bool real;//האם התא אמיתי
        public Cell()
        {
            this.x = 0;
            this.y = 0;
            this.color = 0;
            real = false;
        }

        public void SetReal(bool b)
        {
            this.real = b;
        }

        public bool GetReal()
        {
            return this.real;
        }
        public void SetColor(int color)
        {
            this.color = color;
        }

        public int GetColor()
        {
            return this.color;
        }
    
        public void PaintCell(Graphics g)
        {// לוקח מברשת בצבע וצובע את התא
            Pen p = new Pen(Color.Gold, 4);
            g.DrawRectangle(p, this.x, this.y, 100, 100);
            
            switch(this.color)
            {
                case 0:
                    SolidBrush brush9 = new SolidBrush(Color.Transparent);
                    g.FillRectangle(brush9, this.x, this.y, 100, 100);
                    break;
                case 1:
                    SolidBrush  brush1 = new SolidBrush(Color.Red);
                    g.FillEllipse(brush1, this.x+5, this.y+5, 75, 75);
                    break;
                case 2:
                    SolidBrush brush2 = new SolidBrush(Color.Yellow);
                    g.FillEllipse(brush2, this.x+5, this.y+5, 75, 75);
                    break;
            }
        }  
    }
}
