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
    record struct Cell(CellStatus Status, CellStatus NextStatus, Coord Coord);

    public partial class Form1 : Form
    {
        const int FieldSize = 20;
        const int CellSize = 20;
        const string cellName = "cell";
        Color DeadColor = Color.GreenYellow;
        Color AliveColor = Color.DeepPink;
        int CountToLive = 2;
        int CountToBurn = 3;
        int CountToDead = 4;
        Coord[] Neighbors = { new Coord(1, 1), new Coord(0, 1), new Coord(-1, 1), new Coord(-1, 0),
            new Coord(-1, -1), new Coord(0, -1), new Coord(1, -1), new Coord(1, 0) };
        FileController fc = new FileController();


        Button[,] Field = new Button[FieldSize, FieldSize];
        
        public Form1()
        {
            InitializeComponent();
            Init();
        }
        void Init()
        {

            BackColor = Color.Black;
            Coord.n = FieldSize;
            string s = fc.Download();
            this.FormClosing += new FormClosingEventHandler(Form_Closing);
            CreateBoard(s);
        }
        void CreateBoard(string s)
        {
            Controls.Find(cellName, true).ToList().ForEach(elem => Controls.Remove(elem));

            Controls.RemoveByKey(cellName);

            for (int i = 0; i < FieldSize; i++)
            {
                for (int j = 0; j < FieldSize; j++)
                {
                    Button button = new Button();

                    button.Location = new Point(j * CellSize, i * CellSize);
                    button.Size = new Size(CellSize, CellSize);
                    button.MouseDown += new MouseEventHandler(Cell_click);
                    button.Name = cellName;
                    button.FlatAppearance.BorderSize = 1;
                    button.FlatAppearance.BorderColor = Color.White;
                    button.FlatStyle = FlatStyle.Flat;

                    Cell cell;
                    if (s[FieldSize * i + j] == '0')
                    {
                        cell = new Cell(CellStatus.Dead, CellStatus.Dead, new Coord(i, j));
                        button.BackColor = DeadColor;
                    }
                    else
                    {
                        cell = new Cell(CellStatus.Alive, CellStatus.Alive, new Coord(i, j));
                        button.BackColor = AliveColor;
                    }

                    button.Tag = cell;
                    Field[i, j] = button;
                    this.Controls.Add(button);
                }
            }
        }
        void SetStatus(ref Button button)
        {
            Cell cell =(Cell)button.Tag;
            CellStatus status = cell.NextStatus;

            cell.Status = status;
            if(status == CellStatus.Dead)
            {
                button.BackColor = DeadColor;
            }
            else
            {
                button.BackColor = AliveColor;
            }
            button.Tag = cell;
        }        
        void UpdateStatus(ref Button button)
        {
            Cell cell = (Cell)button.Tag;
            int alive_count = 0;
            foreach(var p in Neighbors)
            {
                Coord c = cell.Coord + p;
                Cell neighbor = (Cell)Field[c.X, c.Y].Tag;
                if(neighbor.Status == CellStatus.Alive)
                    alive_count++;
            }

            if (alive_count == CountToBurn && cell.Status == CellStatus.Dead)
                cell.NextStatus = CellStatus.Alive;
            else if ((alive_count < CountToLive || alive_count >= CountToDead) && cell.Status == CellStatus.Alive)
                cell.NextStatus = CellStatus.Dead;
            button.Tag = cell;
        }
        void Cell_click(object sender, EventArgs e)
        {
            
            Button button = (Button)sender;
            Cell cell = (Cell)button.Tag;
            if(cell.Status == CellStatus.Dead)
            {
                cell.Status = CellStatus.Alive;                
                button.BackColor = AliveColor;
            }
            else
            {
                cell.Status = cell.Status = CellStatus.Dead;
                button.BackColor = DeadColor;
            }
            cell.NextStatus = cell.Status;
            button.Tag = cell;
        }
        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            const string message = "Сохранить текущее поле?";
            const string caption = "Form Closing";
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            // If the no button was pressed ...
            if (result == DialogResult.Yes)
            {
                fc.Save(Field);
            }

        }  
        private void Turn_click(object sender, EventArgs e)
        {          
            for (int i = 0; i < FieldSize; i++)
                for (int j = 0; j < FieldSize; j++)
                {
                    UpdateStatus(ref Field[i, j]);
                }

            for (int i = 0; i < FieldSize; i++)
                for (int j = 0; j < FieldSize; j++)
                {
                    SetStatus(ref Field[i, j]);
                }
        }

        private void Save_click(object sender, EventArgs e)
        {
            fc.Save(Field, fc.GetFileNameForSaving());
        }
        private void Clear_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < FieldSize; i++)
                for(int j = 0; j < FieldSize; j++)
                {
                    Cell cell = (Cell)Field[i, j].Tag;
                    cell.Status = CellStatus.Dead;
                    cell.NextStatus = CellStatus.Dead;
                    Field[i, j].Tag = cell;
                    Field[i, j].BackColor = DeadColor;
                }
        }
        private void Download_click(object sender, EventArgs e)
        {         
            CreateBoard(fc.Download(fc.GetFileNameForDownloading()));
        }

        private void Generate_Click(object sender, EventArgs e)
        {
            CreateBoard(String.Join("", Enumerable.Range(0, 400).Select(a => (new Random().Next(2)).ToString())));
        }
    }
}