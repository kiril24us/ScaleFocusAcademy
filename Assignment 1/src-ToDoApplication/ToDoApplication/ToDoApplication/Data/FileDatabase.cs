using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace ToDoApplication.Data
{
    public class FileDatabase
    {
        public void Write<T>(string fileName, T data)
        {
            string serializedData = JsonSerializer.Serialize(data);

            File.WriteAllText(fileName, serializedData);

        }

        public T Read<T>(string fileName) where T : class
        {          
            if (File.Exists(fileName))
            {
                string serializedData = File.ReadAllText(fileName);
                return JsonSerializer.Deserialize<T>(serializedData);
            }
            else
            {
                return null;
            }
        }

        public int ReadNumber(string fileName)
        {           
            if(File.Exists(fileName))
            {
                string serializedData = File.ReadAllText(fileName);
                return int.Parse(serializedData);
            }
            else
            {
                return 1;
            }
        }
    }
}
