using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLife
{
    internal class FileController
    {
        const string PathToSaves = "../../../Saves";
        const string autosave = "/autosave.txt";
        string standartField =new string('0', 400);
        public void Safe(Button[,] Field)
        {
            if (!Directory.Exists(PathToSaves))
            {
                Directory.CreateDirectory(PathToSaves);
            }
            string s = "";
            for(int i = 0; i < Field.GetLength(0); i++)
                for(int j =0; j <Field.GetLength(1); j++)
                {
                    if (((Cell)Field[i, j].Tag).Status == CellStatus.Alive)
                        s += "1";
                    else
                        s += "0";
                }
            File.WriteAllText(PathToSaves+autosave, s);
        }
        public string Download()
        {
            if (File.Exists(PathToSaves + autosave))
                return File.ReadAllText(PathToSaves + autosave);
            else
                return standartField;
        }
    }
}
