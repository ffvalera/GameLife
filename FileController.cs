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
        public string GetFileNameForSaving()
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                return saveFileDialog1.FileName;
            }
            return "";
        }
        public string GetFileNameForDownloading()
        {
            Stream myStream;
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog.FileName;
            }
            return "";
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
