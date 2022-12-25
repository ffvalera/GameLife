namespace GameLife
{
    public enum CellStatus
    {
        Alive = 0, Dead = 1
    }

    public struct Coord
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
    public record class Cell(CellStatus status = CellStatus.Dead, CellStatus nextStatus = CellStatus.Dead)
    {
        public CellStatus Status = status;
        public CellStatus NextStatus = nextStatus;
        public int lifetime = 0;
    };

    public class Field
    {
        public int FieldSize = 20;
        public Cell[][] cells;

        Coord[] Neighbors = { new Coord(1, 1), new Coord(0, 1), new Coord(-1, 1), new Coord(-1, 0),
            new Coord(-1, -1), new Coord(0, -1), new Coord(1, -1), new Coord(1, 0) };
        public bool[] live = {false, false, true, true, false, false , false , false ,false ,false };
        public bool[] burn = { false, false, false, true, false, false, false, false, false, false };

        public Cell this[Coord coord]
        {
            get { return cells[coord.X][coord.Y]; }
            set { cells[coord.X][coord.Y] = value; }
        }
        public void UpdateStatus()
        {
            for (int i = 0; i < FieldSize; i++)
                for (int j = 0; j < FieldSize; j++)
                {
                    int alive_count = 0;
                    foreach (var p in Neighbors)
                    {
                        Coord c = new Coord(i, j) + p;
                        Cell neighbor = this[c];
                        if (neighbor.Status == CellStatus.Alive)
                            alive_count++;
                    }

                    if (burn[alive_count] && cells[i][j].Status == CellStatus.Dead)
                    {
                        cells[i][j].NextStatus = CellStatus.Alive;

                    }

                    else if (!live[alive_count] && cells[i][j].Status == CellStatus.Alive)
                        cells[i][j].NextStatus = CellStatus.Dead;
                }
        }
        public Field(int size, bool[] born, bool[] live)
        {
            FieldSize = size;
            cells = (new int[size]).Select(x => new int[size].Select(x => new Cell()).ToArray()).ToArray();
            Coord.n = size;
            this.burn = born;
            this.live = live;
        }
        
    }
}
