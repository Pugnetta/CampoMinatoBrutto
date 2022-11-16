



namespace CampoMinatoWinForms
{
    public partial class Form1 : Form
    {
        
        private MyButton[,] _btnGriglia;
        private int _rows, _cols; 
        private int _bombs = 50;
        private int _flagNumber = 50;
        private bool _flag;
        public Form1()
        {
            InitializeComponent();
            InitializeCampo();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
        }      

        private void Flag_Click(object sender, EventArgs e)
        {
            
            if (_flag == true)
            {
                _flag = false;
                Flag.BackColor = Color.Gainsboro;

            }
            else
            {
                _flag = true;
                Flag.BackColor = Color.Aquamarine;
            }

            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            InitializeCampo();
            _flag = false;
            Flag.BackColor = Color.Gainsboro;
            _flagNumber = _bombs;           

        }

        private void Griglia_Click(object? sender, EventArgs e)
        {
            MyButton btn = (MyButton)sender;

            if (_flag)
            {
                if (btn.IsFlagged == false && btn.IsSelected == false)
                {
                    if (_flagNumber > 0)
                    {
                        _flagNumber--;
                        btn.IsFlagged = true;
                        Flag.Text = $"F({_flagNumber})";
                        btn.Text = "F";
                        btn.BackColor = Color.Aquamarine;
                    }                   
                }
                else
                {
                    
                    if (btn.IsSelected == false && btn.IsFlagged)
                    {
                        btn.IsFlagged = false;
                        btn.Text = "";
                        btn.BackColor = Color.GhostWhite;
                        _flagNumber++;
                        Flag.Text = $"F({_flagNumber})";
                    }


                }
                
            }
            else if (btn.IsBomb && btn.IsFlagged == false)
            {
                foreach (MyButton b in _btnGriglia)
                {
                    if (b.Name == "" && b.IsFlagged == false)
                    {
                        b.BackColor = Color.LightGray;
                    }
                    if (b.IsBomb)
                    {
                        b.BackColor = Color.MistyRose;
                    }
                    if (b.IsFlagged == false)
                    {
                        b.Text = b.Name;
                    }                    
                    b.IsSelected = true;
                }
            }
            else if (btn.IsSelected == false && btn.IsFlagged == false)
            {
                btn.IsSelected = true;
                btn.Text = btn.Name;
                if (btn.Name == "")
                {
                    GetConsecutiveZero(_btnGriglia);
                    foreach (MyButton b in _btnGriglia)
                    {
                        if (b.IsSelected && btn.IsFlagged == false)
                        {
                            b.Text = b.Name;
                        }
                    }
                }
            }

        }

        public void InitializeCampo()
        {
            _rows = panel1.Height / MyButton.BtnSize;
            _cols = panel1.Width / MyButton.BtnSize;
            _btnGriglia = new MyButton[_rows, _cols];
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    _btnGriglia[i, j] = new MyButton();
                    _btnGriglia[i, j].X = i;
                    _btnGriglia[i, j].Y = j;
                    _btnGriglia[i, j].Click += Griglia_Click;


                    panel1.Controls.Add(_btnGriglia[i, j]);
                    _btnGriglia[i, j].Location = new Point(i * MyButton.BtnSize, j * MyButton.BtnSize);
                }
            }
            int temp = _bombs;
            Flag.Text = $"F({_bombs})";
            Random rnd = new Random();
            while (temp > 0)
            {
                int x = rnd.Next(0, _rows);
                int y = rnd.Next(0, _cols);
                if (_btnGriglia[x, y].IsBomb == false)
                {
                    _btnGriglia[x, y].IsBomb = true;
                    _btnGriglia[x, y].Name = "X";
                    temp--;
                }
            }
            foreach (MyButton btn in _btnGriglia)
            {
                GetButtonNames(btn, _btnGriglia, _rows, _cols);
            }

        }

        private bool SafeIndexCheck(MyButton btn, int x, int y)
        {
            return btn.X >= 0 && btn.Y >= 0 && btn.X < x && btn.Y < y;
        }

        private void GetButtonNames(MyButton btn, MyButton[,] campo, int x, int y)
        {
            //lista di bombe nel 3x3
            List<MyButton> listaBombe = new List<MyButton>();
            if (btn.IsBomb == false)
            {

                for (int i = -1; i < 2; i++)
                {
                    for (int j = 1; j >= -1; j--)
                    {

                        if (btn.X + i >= 0 && btn.Y + j >= 0 && btn.X + i < x && btn.Y + j < y)
                        {
                            if (campo[btn.X + i, btn.Y + j].IsBomb) listaBombe.Add(campo[btn.X + i, btn.Y + j]);
                        }

                    }
                }
                switch (listaBombe.Count)
                {
                    case 1: btn.Name = "1"; break;
                    case 2: btn.Name = "2"; break;
                    case 3: btn.Name = "3"; break;
                    case 4: btn.Name = "4"; break;
                    case 5: btn.Name = "5"; break;
                    case 6: btn.Name = "6"; break;
                    case 7: btn.Name = "7"; break;
                    case 8: btn.Name = "8"; break;
                    default: btn.Name = ""; break;
                }
            }
        }

        private void SelectButtonsAroundZero(MyButton casellaAttuale, MyButton[,] campo, int x, int y)
        {


            for (int i = -1; i < 2; i++)
            {
                for (int j = 1; j >= -1; j--)
                {

                    if (casellaAttuale.X + i >= 0 && casellaAttuale.Y + j >= 0 && casellaAttuale.X + i < x && casellaAttuale.Y + j < y)
                    {
                        if (campo[casellaAttuale.X + i, casellaAttuale.Y + j].IsBomb == false && campo[casellaAttuale.X + i, casellaAttuale.Y + j].IsFlagged == false)
                        {
                            campo[casellaAttuale.X + i, casellaAttuale.Y + j].IsSelected = true;
                        }
                    }

                }
            }

        }


        private void GetConsecutiveZero(MyButton[,] campo)
        {


            int count = 0;
            while (count != -1)
            {
                int internalCount = 0;
                foreach (MyButton btn in campo)
                {
                    if (btn.Name == "" && btn.IsSelected && btn.IsVisited == false && btn.IsFlagged == false)
                    {
                        SelectButtonsAroundZero(btn, campo, _rows, _cols);
                        btn.BackColor = Color.LightGray;
                        btn.IsVisited = true;
                        internalCount++;
                    }
                }
                if (internalCount == 0) count = -1;

            }
        }
    }
}