using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLife
{
    internal class FileController
    {
        const string PathToSaves = "../../../Saves";
        const string autosave = "autosave.txt";
        string standartField =new string('0', 400);

        public void Save(Button[,] Field, string fullname = PathToSaves + "/" + autosave)        
        {
            string s = "";
            for(int i = 0; i < Field.GetLength(0); i++)
                for(int j =0; j <Field.GetLength(1); j++)
                {
                    if (((Cell)Field[i, j].Tag).Status == CellStatus.Alive)
                        s += "1";
                    else
                        s += "0";
                }

            if (!File.Exists(fullname))
                File.Create(fullname).Close();

            File.WriteAllText(fullname, s);
        }
        private string GetFileName(FileDialog fileDialog)
        {
            fileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            fileDialog.FilterIndex = 2;
            fileDialog.RestoreDirectory = true;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                return fileDialog.FileName;
            }
            return "";
        }
        public string GetFileNameForSaving()
        {
            return GetFileName(new SaveFileDialog());
        }
        public string GetFileNameForDownloading()
        {
            return GetFileName(new OpenFileDialog());
        }
        public string Download(string fullname = PathToSaves + "/"+autosave)
        {
            if (File.Exists(fullname))
                return File.ReadAllText(fullname);
            else
                return standartField;
        }
    }
}
