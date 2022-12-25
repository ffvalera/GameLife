namespace GameLife
{
    public partial class Form1 : Form
    {

        const string cellName = "cell";
        Color DeadColor = Color.GreenYellow;
        Color AliveColor = Color.DeepPink;
        const int shiftx = 80, shifty = 20;

        FileController fc = new FileController();
        PictureBox pictureBox = new PictureBox();
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        Form3 form3 = new Form3();


        bool isPlayed = false;
        public int countOfMoves = 0;
        public bool infCountOfMoves = true;
        public Field field;
        int CellSize = 20;
        int movex = 0, movey = 0;
        int maxsize = 1000;

        public Form1()
        {
            InitializeComponent();
            Init();
        }
        void Init()
        {
            BackColor = Color.White;

            fc.Download(ref field, ref CellSize);

            textBox1.Text = field.FieldSize.ToString();

            form3.Owner = this;

            this.FormClosing += new FormClosingEventHandler(Form_Closing);
            this.MouseWheel += new MouseEventHandler(Pb_MouseWheel);
            this.KeyPreview = true;
            this.KeyDown+= new KeyEventHandler(Pb_keyDown);

            pictureBox.Location = new Point(shiftx, shifty);
            pictureBox.Size = new Size(maxsize+shiftx, maxsize + shifty);
            pictureBox.MouseClick += Cell_click;
            pictureBox.Paint += new PaintEventHandler(pictureBox1_Paint);
            pictureBox.MouseMove += new MouseEventHandler(Pb_MouseMove);
            Controls.Add(pictureBox);

            timer.Interval = 100;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {    
            Graphics g = e.Graphics;
            g.FillRectangle(new SolidBrush(DeadColor),movex, movey, CellSize * field.FieldSize, CellSize * field.FieldSize);


            for (int i = 0; i < field.FieldSize; i++)
                for (int j = 0; j < field.FieldSize; j++)
                {
                    if (field.cells[i][j].Status == CellStatus.Alive)
                    {
                        Brush b = new SolidBrush(AliveColor);
                        g.FillRectangle(b, i * CellSize +movex, j * CellSize +movey, CellSize, CellSize);
                       // g.DrawRectangle(new Pen(Color.White), i * CellSize + shiftx, j * CellSize + shifty, CellSize, CellSize);
                    }
                    
                }
        }

        void Cell_click(object sender, MouseEventArgs e)
        {
            int x = e.X, y = e.Y;
            Coord c = new Coord((x - movex) / CellSize, (y - movey) / CellSize);
            if (c.X < 0 || c.X >= field.FieldSize || c.Y < 0 || c.Y >= field.FieldSize)
                return;

            field[c].Status = 1 - field[c].Status;
            field[c].NextStatus = field[c].Status;
            pictureBox.Invalidate();
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            const string message = "Сохранить текущее поле?";
            var result = MessageBox.Show(message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                fc.Save(field, CellSize);
            }
        }
        private void makeTurn()
        {
            field.UpdateStatus();

            for (int i = 0; i < field.FieldSize; i++)
                for (int j = 0; j < field.FieldSize; j++)
                {
                    field.cells[i][j].Status = field.cells[i][j].NextStatus;

                    if (field.cells[i][j].Status == CellStatus.Alive)
                        field.cells[i][j].lifetime++;
                    else
                        field.cells[i][j].lifetime = 0;
                }
            pictureBox.Invalidate();
        }
        
        private void Resize(int size)
        {
            field = new Field(size, field.burn, field.live);            
            pictureBox.Invalidate();
        }
        private void Turn_click(object sender, EventArgs e)
        {
            makeTurn();
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < field.FieldSize; i++)
                for (int j = 0; j < field.FieldSize; j++)
                {
                    field.cells[i][j].Status = field.cells[i][j].NextStatus = CellStatus.Dead;
                }
            pictureBox.Invalidate();
        }

        private void Generate_Click(object sender, EventArgs e)
        {
            Random r = new Random();            

            for (int i = 0; i < field.FieldSize; i++)
                for (int j = 0; j < field.FieldSize; j++)
                    field.cells[i][j].Status = field.cells[i][j].NextStatus = r.Next(2) == 0 ? CellStatus.Alive : CellStatus.Dead;

            pictureBox.Invalidate();
        }

        private void Play_Click(object sender, EventArgs e)
        {
            var b = sender as Button;
            if (isPlayed)
            {
                b.Text = "Play";                                
            }
            else
            {
                b.Text = "Stop";             
            }
            isPlayed = !isPlayed;            
        }
        void timer_Tick(object sender, EventArgs e)
        {
            if (isPlayed)
            {

                if(!infCountOfMoves && countOfMoves <= 0)
                {
                    isPlayed = false;
                    PlayButton.Text = "Play";
                    return;
                }
                if (!infCountOfMoves && countOfMoves > 0)
                    countOfMoves--;
                makeTurn();
            }
            
        }

        void Confirm_Click(object sender, EventArgs e)
        {
            int size; 

            if (int.TryParse(textBox1.Text, out size))
            {
                Resize(size);
            }
            textBox1.Text = field.FieldSize.ToString();
        }
        void Pb_MouseMove(object sender, MouseEventArgs e)
        {
            int CursorX = e.X;
            int CursorY = e.Y;
            Coord c = new Coord((CursorX - shiftx) / CellSize, (CursorY - shifty) / CellSize);

            if (c.X < 0 || c.X >= field.FieldSize || c.Y < 0 || c.Y >= field.FieldSize)
                return;

            this.Text = field[c].lifetime.ToString();
        }

        private void RulesChange_Click(object sender, EventArgs e)
        {
            Form f = new Form2(ref field);
            f.Owner = this;
            f.ShowDialog();
        }
        private void Set_Click(object sender, EventArgs e)
        {

            form3.ShowDialog();

        }
        private void Save_Click_1(object sender, EventArgs e)
        {
            fc.Save(field, CellSize, fc.GetFileNameForSaving());
        }

        private void Download_Click_1(object sender, EventArgs e)
        {
            Field? f = null;
            int cellsize = 0;
            fc.Download(ref f, ref cellsize, fc.GetFileNameForDownloading());
            if (f != null)
            {
                field = f;
                CellSize = cellsize;
            }
            pictureBox.Invalidate();
        }

        

        void Pb_MouseWheel(object sender, MouseEventArgs e)
        {
            int a = e.Delta/20;
            if (CellSize + a > 0)
            {
                CellSize += a;
                pictureBox.Invalidate();
            }
        }
        private void Pb_keyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
                movey-=10;
            if (e.KeyCode == Keys.S)
                movey+=10;
            if (e.KeyCode == Keys.D)
                movex+=10;
            if (e.KeyCode == Keys.A)
                movex-=10;
            pictureBox.Invalidate();
        }
    }
}