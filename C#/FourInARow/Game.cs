using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace FourInARow
{
    
    public partial class Game : Form
    {
        Graphics g;
        Board b = new Board();
        string playerName;
        int count = 0;//מספר התורות שהיו במשחק
        int level = 0;
        Random rnd = new Random();
        int win = 0;
        int lose = 0;
        FileStream fs;
        BinaryWriter br;
        

        //בונה את המשחק ההתחלתי
        public Game(string playerName, int level)
        {
            InitializeComponent();
            this.level = level;
            this.playerName = playerName;           
        }
        private void Game_Load(object sender, EventArgs e)
        {// אם יש משחק שמור - טען אותו
            
            textBox1.Text = ("Good Luck "+ this.playerName + " !!!   :)");
            try//המשחק שוחק כבר על ידי אותו שם משתמש והוא שמר אותו
            {                
                fs = new FileStream("Test.dat", FileMode.Open, FileAccess.Read);
                DialogResult result;              
                result = MessageBox.Show("יש משחק שמור במערכת - תרצה לחזור אליו ", "משחק שמור", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    fs.Close();
                    OpenFile();
                }
                fs.Close();
                Refresh();
                
            }
            catch { }
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            DialogResult result;
            result = MessageBox.Show("האם אתה בטוח שברצונך לצאת", "יציאה", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                result = MessageBox.Show("האם אתה מעוניין לשמור את המשחק?", "שמירה", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    SaveFile();
                    Records a = new Records(this.playerName, this.win, this.lose);
                    a.Show();
                    this.Close();
                }
                else
                {
                    Records a = new Records(this.playerName, this.win, this.lose);
                    a.Show();
                    this.Close();
                }
            }
        }

       

        private void Game_Paint(object sender, PaintEventArgs e)
        {//מצייר את הטבלה
            g = e.Graphics;
            b.PaintBoard(g);
        }
       
        private void Game_MouseClick(object sender, MouseEventArgs e)
        {//האם הלחיצה בוצעה במקום תקין ואם כן ממשיכה
            g = CreateGraphics();
            int x = e.X;
            int y = e.Y;

            if ((x > 500) && (x <= 1200) && (y > 300) && (y < 900))
            //האם הלחיצה בוצעה בתחומי המטריצה
            {
                int i = 3;//3 זה העמודה הראשונה 
                int j = (x - 200) / 100;//לגלות באיזו עמודה התבצעה הלחיצה
                                        //האיקס של הלחיצה פחות הרווח ההתחלתי לחלק לרוחב של התא   
                if (b.GetBoard()[i, j].GetColor() == 0)
                {
                    b.GetBoard()[i, j].SetColor(1);
                    b.PaintBoard(g);
                    Down1(j, 1);
                    count++;
                    if (count >= 6)
                    {
                        if (b.CheckWin(1) == true)
                        {//אם יש ניצחון, מציג את זה ומעדכן בטבלת שיאים
                            win++;
                            DialogResult result;
                            result = MessageBox.Show("YES, You WON!", "יציאה", MessageBoxButtons.OK);
                            if (result == DialogResult.OK)
                            {
                                this.Close();
                                Records a = new Records(this.playerName, this.win, this.lose);
                                a.Show();
                            }
                        }
                    }

                    Comp();
                    if (count >= 6)
                    {
                        if (b.CheckWin(2) == true)
                        {//אם יש הפסד, מציג את זה ומעדכן בטבלת שיאים
                            lose++;
                            DialogResult result;
                            result = MessageBox.Show("YES I Did It, Yellow! I can't believe You LOST!", "יציאה", MessageBoxButtons.OK);
                            if (result == DialogResult.OK)
                            {
                                
                                this.Close();
                                Records a = new Records(this.playerName, this.win, this.lose);
                                a.Show();
                            }
                        }
                    }
                }
            }
        }
        public void Comp()
        {
            //מהלכים של המחשב
            switch (this.level)
            {
                case 0:
                    Easy();
                    break;
                case 1:
                    int num = rnd.Next(2);
                    if (num == 0)
                        Easy();
                    else
                        Hard();
                    break;
                case 2:
                    Hard();
                    break;
            }
            count++;
        }

        public void Easy()
        {
            int a = rnd.Next(7) + 3; //מגרילים מספר עמודה
            while (b.GetBoard()[3, a].GetColor() != 0) //כל עוד העמודה שהגרלנו לא פנויה
            {
                a = rnd.Next(7) + 3; //מגרילים מספר עמודה חדש 
            }
            Down1(a, 2);
        }


        public void Hard()
        {           
            if (Find3(2) == false) //המחשב מחפש אפשרות של ניצחון שלו
            {
                if (Find3(1) == false)//המחשב מחפש אם הוא יכול למנוע ניצחון של השחקן
                {
                    if (Find2(1) == false)//המחשב מחפש אם הוא יכול למנוע המשך משתי דיסקית של השחקן
                    {
                        if (Find2(2) == false)//המחשב מחפש רצף של שני דיסקיות שלו
                        {
                            if (Find1(1) == false) //המחשב מחפש אם הוא יכול למנוע המשך מדיסקית אחת של השחקן
                            {
                                if (Find1(2) == false) //המחשב מחפש דיסקית אחת שלו
                                {
                                    Easy();                                  
                                }
                            }
                        }
                    }
                }
            }
        }

        public bool Find3(int fill)
        {
            //המחשב מחפש רצף של שלוש של המשתנה fill
            //אם המחשב מצא - הוא ישלים ויחזיר אמת
            //אחרת - יחזיר שקר
            for (int i = 3; i <  b.GetBoard().GetLength(0) - 3; i++)
            {
                for (int j = 3; j < b.GetBoard().GetLength(1) - 3; j++)
                {
                    if (b.GetBoard()[i, j].GetColor() == fill && b.GetBoard()[i, j].GetColor() != 0)
                    {
                        //בדיקה ימינה                        
                        if (b.GetBoard()[i, j].GetColor() == b.GetBoard()[i, j + 1].GetColor() && b.GetBoard()[i, j].GetColor() == b.GetBoard()[i, j + 2].GetColor() && b.GetBoard()[i, j + 3].GetColor() == 0 && b.GetBoard()[i, j + 3].GetReal() == true)
                        {
                            Down1(j + 3, 2);
                            return true;
                        }
                        //בדיקה שמאלה 
                        if (b.GetBoard()[i, j].GetColor() == b.GetBoard()[i, j - 1].GetColor() && b.GetBoard()[i, j].GetColor() == b.GetBoard()[i, j - 2].GetColor() && b.GetBoard()[i, j - 3].GetColor() == 0 && b.GetBoard()[i, j - 3].GetReal() == true)
                        {
                            Down1(j - 3, 2);
                            return true;
                        }
                        // בדיקה למעלה 
                        if (b.GetBoard()[i, j].GetColor() == b.GetBoard()[i + 1, j].GetColor() && b.GetBoard()[i, j].GetColor() == b.GetBoard()[i + 2, j].GetColor() && b.GetBoard()[i + 3, j].GetColor() == 0 && b.GetBoard()[i + 3, j].GetReal() == true)
                        {
                            Down1(j, 2);
                            return true;
                        }
                        //בדיקה למטה 
                        if (b.GetBoard()[i, j].GetColor() == b.GetBoard()[i - 1, j].GetColor() && b.GetBoard()[i, j].GetColor() == b.GetBoard()[i - 2, j].GetColor() && b.GetBoard()[i - 3, j].GetColor() == 0 && b.GetBoard()[i - 3, j].GetReal() == true)
                        {
                            Down1(j, 2);
                            return true;
                        }
                        //בדיקת אלכסון ימינה ולמעלה 
                        if (b.GetBoard()[i, j].GetColor() == b.GetBoard()[i + 1, j + 1].GetColor() && b.GetBoard()[i, j].GetColor() == b.GetBoard()[i + 2, j + 2].GetColor() && b.GetBoard()[i + 3, j + 3].GetColor() == 0 && b.GetBoard()[i + 3, j + 3].GetReal() == true)
                        {
                            Down1(j + 3, 2);
                            return true;
                        }
                        //בדיקת אלכסון ימינה ולמטה 
                        if (b.GetBoard()[i, j].GetColor() == b.GetBoard()[i - 1, j + 1].GetColor() && b.GetBoard()[i, j].GetColor() == b.GetBoard()[i - 2, j + 2].GetColor() && b.GetBoard()[i - 3, j + 3].GetColor() == 0 && b.GetBoard()[i - 3, j + 3].GetReal() == true)
                        {
                            Down1(j + 3, 2);
                            return true;
                        }
                        //בדיקת אלכסון שמאלה ולמעלה 
                        if (b.GetBoard()[i, j].GetColor() == b.GetBoard()[i + 1, j - 1].GetColor() && b.GetBoard()[i, j].GetColor() == b.GetBoard()[i + 2, j - 2].GetColor() && b.GetBoard()[i + 3, j - 3].GetColor() == 0 && b.GetBoard()[i + 3, j - 3].GetReal() == true)
                        {
                            Down1(j - 3, 2);
                            return true;
                        }
                        //בדיקת אלכסון שמאלה ולמטה 
                        if (b.GetBoard()[i, j].GetColor() == b.GetBoard()[i - 1, j - 1].GetColor() && b.GetBoard()[i, j].GetColor() == b.GetBoard()[i - 2, j - 2].GetColor() && b.GetBoard()[i - 3, j - 3].GetColor() == 0 && b.GetBoard()[i - 3, j - 3].GetReal() == true)
                        {
                            Down1(j - 3, 2);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool Find2(int fill)
        {
            //המחשב מחפש רצף של שתיים של המשתנה fill
            //אם המחשב מצא - הוא ישלים ויחזיר אמת
            //אחרת - יחזיר שקר
            for (int i = 3; i < b.GetBoard().GetLength(0) - 3; i++)
            {
                for (int j = 3; j < b.GetBoard().GetLength(1) - 3; j++)
                {
                    if (b.GetBoard()[i, j].GetColor() == fill && b.GetBoard()[i, j].GetColor() != 0)
                    {
                        //בדיקה ימינה                        
                        if (b.GetBoard()[i, j].GetColor() == b.GetBoard()[i, j + 1].GetColor() && b.GetBoard()[i, j + 2].GetColor() == 0 && b.GetBoard()[i, j + 2].GetReal() == true)
                        {
                            Down1(j + 2, 2);
                            return true;
                        }
                        //בדיקה שמאלה 
                        if (b.GetBoard()[i, j].GetColor() == b.GetBoard()[i, j - 1].GetColor() && b.GetBoard()[i, j - 2].GetColor() == 0 && b.GetBoard()[i, j - 2].GetReal() == true)
                        {
                            Down1(j - 2, 2);
                            return true;
                        }
                        // בדיקה למעלה 
                        if (b.GetBoard()[i, j].GetColor() == b.GetBoard()[i + 1, j].GetColor() && b.GetBoard()[i + 2, j].GetColor() == 0 && b.GetBoard()[i + 2, j].GetReal() == true)
                        {
                            Down1(j, 2);
                            return true;
                        }
                        //בדיקה למטה 
                        if (b.GetBoard()[i, j].GetColor() == b.GetBoard()[i - 1, j].GetColor() && b.GetBoard()[i - 2, j + 2].GetColor() == 0 && b.GetBoard()[i - 2, j + 2].GetReal() == true)
                        {
                            Down1(j, 2);
                            return true;
                        }
                        //בדיקת אלכסון ימינה ולמעלה 
                        if (b.GetBoard()[i, j].GetColor() == b.GetBoard()[i + 1, j + 1].GetColor() && b.GetBoard()[i + 2, j + 2].GetColor() == 0 && b.GetBoard()[i + 2, j + 2].GetReal() == true)
                        {
                            Down1(j + 2, 2);
                            return true;
                        }
                        //בדיקת אלכסון ימינה ולמטה 
                        if (b.GetBoard()[i, j].GetColor() == b.GetBoard()[i - 1, j + 1].GetColor() && b.GetBoard()[i - 2, j + 2].GetColor() == 0 && b.GetBoard()[i - 2, j + 2].GetReal() == true)
                        {
                            Down1(j + 2, 2);
                            return true;
                        }
                        //בדיקת אלכסון שמאלה ולמעלה 
                        if (b.GetBoard()[i, j].GetColor() == b.GetBoard()[i + 1, j - 1].GetColor() && b.GetBoard()[i + 2, j - 2].GetColor() == 0 && b.GetBoard()[i + 2, j - 2].GetReal() == true)
                        {
                            Down1(j - 2, 2);
                            return true;
                        }
                        //בדיקת אלכסון שמאלה ולמטה 
                        if (b.GetBoard()[i, j].GetColor() == b.GetBoard()[i - 1, j - 1].GetColor() && b.GetBoard()[i - 2, j - 2].GetColor() == 0 && b.GetBoard()[i - 2, j - 2].GetReal() == true)
                        {
                            Down1(j - 2, 2);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool IsEmptyRealCell(int i, int j) // הפעולה בודקת אם התא ריק ואמיתי
        {   
            return b.GetBoard()[i, j].GetColor() == 0 && b.GetBoard()[i, j].GetReal() == true;
        }

        public bool Find1(int fill)
        {
            //המחשב מחפש רצף של אחד של המשתנה fill
            //אם המחשב מצא - הוא ישלים ויחזיר אמת
            //אחרת - יחזיר שקר
            for (int i = 3; i < b.GetBoard().GetLength(0) - 3; i++)
            {
                for (int j = 3; j < b.GetBoard().GetLength(1) - 3; j++)
                {
                    if (b.GetBoard()[i, j].GetColor() != 0 && b.GetBoard()[i, j].GetColor() == fill)
                    {
                        if (IsEmptyRealCell(i, j + 1))
                        {
                            Down1(j + 1, 2);
                            return true;
                        }
                        else if (IsEmptyRealCell(i, j - 1))
                        {
                            Down1(j - 1, 2);
                            return true;
                        }
                        else if (IsEmptyRealCell(i + 1, j))
                        {
                            Down1(j, 2);
                            return true;
                        }
                        else if (IsEmptyRealCell(i - 1, j))
                        {
                            Down1(j, 2);
                            return true;
                        }
                        else if (IsEmptyRealCell(i + 1, j + 1))
                        {
                            Down1(j + 1, 2);
                            return true;
                        }
                        else if (IsEmptyRealCell(i + 1, j - 1))
                        {
                            Down1(j - 1, 2);
                            return true;
                        }
                        else if (IsEmptyRealCell(i - 1, j + 1))
                        {
                            Down1(j + 1, 2);
                            return true;
                        }
                        else if(IsEmptyRealCell(i - 1, j - 1))
                        {
                            Down1(j - 1, 2);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

    public void Down1(int j, int t)
        {
            //הפעולה מקבלת את העמודה ומשלשלת מטה את הדיסקית
            int i = 3;
            while (i < 8 && b.GetBoard()[i + 1, j].GetColor() == 0)
            {
                b.GetBoard()[i, j].SetColor(0);
                b.GetBoard()[i + 1, j].SetColor(t);
                Refresh();             
                Thread.Sleep(300);
                i++;
            }
        }

       

        public void SaveFile()
        {//שומר את הקובץ
            fs = new FileStream("Test.dat", FileMode.Create, FileAccess.Write);            
            br = new BinaryWriter(fs);
            for (int i = 3; i < b.GetBoard().GetLength(0) - 3; i++)
                for (int j = 3; j < b.GetBoard().GetLength(1) - 3; j++)
                    br.Write(b.GetBoard()[i, j].GetColor());
            br.Close();
            fs.Close();          
        }

        public void OpenFile()
        {// פותח קובץ שנשמר
            fs = new FileStream("Test.dat", FileMode.Open, FileAccess.Read);
            BinaryReader brend = new BinaryReader(fs);
            for (int i = 3; i < b.GetBoard().GetLength(0) - 3; i++)
                for (int j = 3; j < b.GetBoard().GetLength(1) - 3; j++)
                    b.GetBoard()[i, j].SetColor(brend.ReadInt32());           
            brend.Close();
            fs.Close();
        }       
    }
}
