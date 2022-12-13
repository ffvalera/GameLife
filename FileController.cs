using System.Text;

namespace GameLife
{
    internal class FileController
    {
        const string PathToSaves = "../../../Saves";
        const string autosave = "autosave.txt";
        string standartField;        
        public FileController()
        {
             standartField = "20\n" + "20\n" +"000100000\n"+ "001100000\n" +String.Join("", (new int[20]).Select(s => "00000000000000000000"));
        }
        public void Save(Field field, int CellSize, string fullname = PathToSaves + "/" + autosave)
        {
            if (fullname == null)
                return;
            var t = fullname.Split('.').Last();
            if (t == "txt")
                SaveTxt(field, CellSize, fullname);
            else if (t == "jpg")
                SaveJpg(field, CellSize, fullname);

        }
        void SaveJpg(Field field, int CellSize, string fullname = PathToSaves + "/" + autosave)
        {            
        }
        void SaveTxt(Field field, int CellSize, string fullname = PathToSaves + "/" + autosave)        
        {
            if(fullname == null)
                return;
            
            StringBuilder sb = new StringBuilder(field.FieldSize*field.FieldSize + 10);

            sb.AppendLine(field.FieldSize.ToString());
            sb.AppendLine(CellSize.ToString());
            sb.AppendLine(String.Join("", field.burn.Select(x => x ? '1' : '0')));
            sb.AppendLine(String.Join("", field.live.Select(x => x ? '1' : '0')));

            for (int i = 0; i < field.cells.Length; i++)
            {
                for (int j = 0; j < field.cells[0].Length; j++)
                {
                    if (field.cells[i][j].Status == CellStatus.Alive)                        
                        sb.Append("1");
                    else                        
                        sb.Append("0");
                }                
            }

            if (!File.Exists(fullname))
                File.Create(fullname).Close();

            File.WriteAllText(fullname, sb.ToString());
        }
        private string? GetFileName(FileDialog fileDialog)
        {
            fileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            fileDialog.FilterIndex = 2;
            fileDialog.RestoreDirectory = true;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                return fileDialog.FileName;
            }
            return null;
        }
        public string? GetFileNameForSaving()
        {
            return GetFileName(new SaveFileDialog());
        }
        public string? GetFileNameForDownloading()
        {
            return GetFileName(new OpenFileDialog());
        }
        public void Download(ref Field? field, ref int CellSize, string fullname = PathToSaves + "/"+autosave)
        {
            string? s;
            if (fullname == null)
            {
                field = null;
                return;
            }
            if (File.Exists(fullname))
                s= File.ReadAllText(fullname);
            else
                s= standartField;

            

            var x = s.Split('\n');
            int size = int.Parse(x[0]);            
            CellSize = int.Parse(x[1]);
            var b = x[2].Where(c => c == '0' || c == '1').Select(c => c == '0' ? false : true).ToArray();
            var l = x[3].Where(c => c == '0' || c == '1').Select(c => c == '0' ? false : true).ToArray();
            field = new Field(int.Parse(x[0]), b, l);
            var t = x[4];
             
            for (int i = 0; i < field.FieldSize; i++)
                for (int j = 0; j < field.FieldSize; j++)
                    field.cells[i][j] = t[i * field.FieldSize + j] == '1' ? new Cell(CellStatus.Alive, CellStatus.Alive) :
                                                                 new Cell(CellStatus.Dead, CellStatus.Dead);            

        }
    }
}
