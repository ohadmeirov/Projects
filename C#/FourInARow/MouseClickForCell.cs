using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourInARow
{
    internal class MouseClickForCell
    {
        public int x; // X-מיקום הדיסקית לפי ציר ה
        public int y; // Y-מיקום הדיסקית לפי ציר ה


        public void SetX(int x)
        {
            this.x = x;
        }

        public int GetX()
        {
            return this.x;
        }

        public void SetY(int y)
        {
            this.y = y;
        }

        public int GetY()
        {
            return this.y;
        }
    }
}
