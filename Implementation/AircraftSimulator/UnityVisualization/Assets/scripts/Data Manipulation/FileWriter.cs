using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts.Data_Manipulation
{
    public class FileWriter : MonoBehaviour
    {
        private IDictionary<string, StreamWriter> files = new Dictionary<string, StreamWriter>();

        public void Write(string fileName, string line)
        {
            //files[fileName].WriteLine(line);
        }

        public void CreateFile(string name)
        {
            //if (File.Exists(name))
            //    File.Delete(name);


            //files.Add(new KeyValuePair<string, StreamWriter>(name,
            //    new StreamWriter(name)));
        }

        private void OnApplicationQuit()
        {
            foreach (var streamWriter in files.Values)
            {
                streamWriter.Close();
            }
        }
    }
}
