namespace GameLife
{
    internal class Field
    {
        public int FieldSize = 20;
        public Cell[][] cells;

        Coord[] Neighbors = { new Coord(1, 1), new Coord(0, 1), new Coord(-1, 1), new Coord(-1, 0),
            new Coord(-1, -1), new Coord(0, -1), new Coord(1, -1), new Coord(1, 0) };

        int CountToLive = 2;
        int CountToBurn = 3;
        int CountToDead = 4;

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

                    if (alive_count == CountToBurn && cells[i][j].Status == CellStatus.Dead)
                    {
                        cells[i][j].NextStatus = CellStatus.Alive;

                    }

                    else if ((alive_count < CountToLive || alive_count >= CountToDead) && cells[i][j].Status == CellStatus.Alive)
                        cells[i][j].NextStatus = CellStatus.Dead;
                }
        }
        public Field(int size)
        {
            FieldSize = size;
            cells = (new int[size]).Select(x => new int[size].Select(x => new Cell()).ToArray()).ToArray();
            Coord.n = size;
        }
        
    }
}
