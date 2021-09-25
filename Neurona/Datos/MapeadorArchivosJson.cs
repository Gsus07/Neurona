using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Neurona.Datos
{
    class MapeadorArchivosJson
    {
        public static void SaveObject(string fileName, object o)
        {
            string jsonString = JsonSerializer.Serialize(o);
            using StreamWriter streamWriter = new(fileName);
            streamWriter.Write(jsonString);
        }

        public static T LoadObject<T>(string fileName)
        {
            string jsonString = File.ReadAllText(fileName);
            return JsonSerializer.Deserialize<T>(jsonString);
        }
    }
}
