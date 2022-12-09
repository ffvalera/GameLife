using System.Diagnostics;
using System.Text;
using System.Text.Json.Serialization.Metadata;

namespace GameLife
{
    internal class FileController
    {
        const string PathToSaves = "../../../Saves";
        const string autosave = "autosave.txt";
        string standartField;

        
        public FileController()
        {
             standartField = "20\n"+String.Join("", (new int[20]).Select(s => "00000000000000000000"));
        }
        public void Save(Field field, string fullname = PathToSaves + "/" + autosave)        
        {
            if(fullname == null)
                return;
            
            StringBuilder sb = new StringBuilder(field.FieldSize*field.FieldSize + 10);
            sb.AppendLine(field.FieldSize.ToString());
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
        public Field? Download(string fullname = PathToSaves + "/"+autosave)
        {
            string? s;
            if (fullname == null)
                return null;
            if (File.Exists(fullname))
                s= File.ReadAllText(fullname);
            else
                s= standartField;

            

            var x = s.Split('\n');
            Field field = new Field(int.Parse(x[0]));
            var t = x[1];
             
            for (int i = 0; i < field.FieldSize; i++)
                for (int j = 0; j < field.FieldSize; j++)
                    field.cells[i][j] = t[i * field.FieldSize + j] == '1' ? new Cell(CellStatus.Alive, CellStatus.Alive) :
                                                                 new Cell(CellStatus.Dead, CellStatus.Dead);            
            return field;
        }
    }
}
