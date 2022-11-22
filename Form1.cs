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
        Field field;
        const int CellSize = 20;
        const string cellName = "cell";
        Color DeadColor = Color.GreenYellow;
        Color AliveColor = Color.DeepPink;
        FileController fc = new FileController();
        Button[,] Buttons;
        
        public Form1()
        {
            InitializeComponent();
            Init();
        }
        void Init()
        {
            BackColor = Color.Black;            
            field = fc.Download();
            Coord.n = field.FieldSize;
            this.FormClosing += new FormClosingEventHandler(Form_Closing);
            CreateBoard(field);
        }
        void CreateBoard(Field field)
        {        
            this.field = field;
            Buttons = new Button[field.FieldSize, field.FieldSize];

            for (int i = 0; i < field.FieldSize; i++)
            {
                for (int j = 0; j < field.FieldSize; j++)
                {
                    Button button = new Button();

                    button.Location = new Point(j * CellSize, i * CellSize);
                    button.Size = new Size(CellSize, CellSize);
                    button.MouseDown += new MouseEventHandler(Cell_click);
                    button.Name = cellName;
                    button.FlatAppearance.BorderSize = 1;
                    button.FlatAppearance.BorderColor = Color.White;
                    button.FlatStyle = FlatStyle.Flat;

                    Cell cell = field.cells[i][j];
                    UpdateButton(ref button, cell);
                    button.Tag = new Coord(i, j);
                    Buttons[i,j] = button;
                    this.Controls.Add(Buttons[i, j]);
                }
            }
        }
        void UpdateBoard(Field? f)
        {
            if (f == null)
                return;
            field = f;
            for (int i = 0; i < field.FieldSize; i++)
                for (int j = 0; j < field.FieldSize; j++)
                    UpdateButton(ref Buttons[i, j], f.cells[i][j]);
        }
        void UpdateButton(ref Button button, Cell cell)
        {
            if (cell.Status == CellStatus.Dead)
            {
                button.BackColor = DeadColor;
            }
            else
            {
                button.BackColor = AliveColor;
            }
        }
       
        
        void Cell_click(object sender, EventArgs e)
        {            
            Button button = (Button)sender;
            Cell cell = field[(Coord)button.Tag];

            cell.Status = 1 - cell.Status;

            UpdateButton(ref button, cell);
            cell.NextStatus = cell.Status;
        }
        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            const string message = "Сохранить текущее поле?";
            var result = MessageBox.Show(message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                fc.Save(field);
            }
        }  
        private void Turn_click(object sender, EventArgs e)
        {          
            field.UpdateStatus();            

            for (int i = 0; i < field.FieldSize; i++)
                for (int j = 0; j < field.FieldSize; j++)
                {
                    field.cells[i][j].Status = field.cells[i][j].NextStatus;
                    UpdateButton(ref Buttons[i, j], field.cells[i][j]);
                }
        }

        private void Save_click(object sender, EventArgs e)
        {
            fc.Save(field, fc.GetFileNameForSaving());
        }
        private void Clear_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < field.FieldSize; i++)
                for(int j = 0; j < field.FieldSize; j++)
                {                
                    field.cells[i][j].Status = field.cells[i][j].NextStatus = CellStatus.Dead;
                    UpdateButton(ref Buttons[i, j], field.cells[i][j]);
                }
        }
        private void Download_click(object sender, EventArgs e)
        {         
            UpdateBoard(fc.Download(fc.GetFileNameForDownloading()));
        }

        private void Generate_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            Field f = new Field();

            for (int i = 0; i < field.FieldSize; i++)
                for (int j = 0; j < field.FieldSize; j++)
                    f.cells[i][j].Status = f.cells[i][j].NextStatus = r.Next(2) == 0 ? CellStatus.Alive : CellStatus.Dead;

            UpdateBoard(f);
        }
    }
}