namespace GameLife
{
    enum CellStatus
    {
        Alive=0, Dead=1
    }

    struct Coord
    {
        public int X;
        public int Y;
        public static int n;
        public Coord(int x, int y)
        {
            X = x; Y = y;
        }
        public static Coord operator +(Coord a, Coord b)
        {
            return new Coord((a.X + b.X + n) % n, (a.Y + b.Y + n) % n);
        }
    }
    record class Cell(CellStatus status = CellStatus.Dead, CellStatus nextStatus = CellStatus.Dead)
    {
        public CellStatus Status = status;
        public CellStatus NextStatus = nextStatus;        
    };

    public partial class Form1 : Form
    {
        const int CellSize = 20;
        const string cellName = "cell";
        Color DeadColor = Color.GreenYellow;
        Color AliveColor = Color.DeepPink;
        
        FileController fc = new FileController();
        PictureBox pictureBox = new PictureBox();
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        int shiftx = 40, shifty = 2;
        bool isPlayed = false;
        Field field;

        public Form1()
        {
            InitializeComponent();
            Init();
        }
        void Init()
        {
            BackColor = Color.White;

            field = fc.Download();

            textBox1.Text = field.FieldSize.ToString();

            this.FormClosing += new FormClosingEventHandler(Form_Closing);

            pictureBox.Location = new Point(shiftx, shifty);
            pictureBox.Size = new Size(field.FieldSize*CellSize+shiftx, field.FieldSize * CellSize + shifty);
            pictureBox.MouseClick += Cell_click;
            pictureBox.Paint += new PaintEventHandler(pictureBox1_Paint);
            Controls.Add(pictureBox);

            timer.Interval = 100; //интервал между срабатывани€ми 1000 миллисекунд
            timer.Tick += new EventHandler(timer_Tick); //подписываемс€ на событи€ Tick
            timer.Start();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {    
            Graphics g = e.Graphics;

            for (int i = 0; i < field.FieldSize; i++)
                for (int j = 0; j < field.FieldSize; j++)
                {
                    Brush b = new SolidBrush(field.cells[i][j].Status == CellStatus.Dead ? DeadColor : AliveColor);
                    g.FillRectangle(b, i * CellSize + shiftx, j * CellSize + shifty, CellSize, CellSize);
                    g.DrawRectangle(new Pen(Color.White), i * CellSize + shiftx, j * CellSize + shifty, CellSize, CellSize);
                }
        }

        void Cell_click(object sender, MouseEventArgs e)
        {
            int x = e.X, y = e.Y;
            Coord c = new Coord((x - shiftx) / CellSize, (y - shifty) / CellSize);
            if (c.X < 0 || c.X >= field.FieldSize || c.Y < 0 || c.Y >= field.FieldSize)
                return;

            field[c].Status = 1 - field[c].Status;
            field[c].NextStatus = field[c].Status;
            pictureBox.Invalidate();
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            const string message = "—охранить текущее поле?";
            var result = MessageBox.Show(message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                fc.Save(field);
            }
        }
        private void makeTurn()
        {
            field.UpdateStatus();

            for (int i = 0; i < field.FieldSize; i++)
                for (int j = 0; j < field.FieldSize; j++)
                {
                    field.cells[i][j].Status = field.cells[i][j].NextStatus;
                }
            pictureBox.Invalidate();
        }
        private void Turn_click(object sender, EventArgs e)
        {
            makeTurn();
        }

        private void Save_click(object sender, EventArgs e)
        {
            fc.Save(field, fc.GetFileNameForSaving());
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

        private void Download_click(object sender, EventArgs e)
        {
            Field f = fc.Download(fc.GetFileNameForDownloading());
            if (f != null)
                field = f;
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
                b.Text = "Play";
            else
                b.Text = "Stop";
            isPlayed = !isPlayed;
            
        }
        void timer_Tick(object sender, EventArgs e)
        {
            if(isPlayed)
                makeTurn(); 
        }

        private void Confirm_Click(object sender, EventArgs e)
        {
            int size; 

            if (int.TryParse(textBox1.Text, out size))
            {
                field = new Field(size);
                pictureBox.Size = new Size(field.FieldSize * CellSize + shiftx, field.FieldSize * CellSize + shifty);
                pictureBox.Invalidate();
            }
            textBox1.Text = field.FieldSize.ToString();
        }
    }
}