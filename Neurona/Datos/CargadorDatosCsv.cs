using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Neurona.Datos
{
    class CargadorDatosCsv
    {
        public static List<string[]> LoadCsv(string @filePath)
        {
            using StreamReader reader = new(@filePath);
            Regex csvParser = new(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

            List<string[]> csvData = new();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (line != null)
                {
                    csvData.Add(csvParser.Split(line));
                }
            }

            return csvData;
        }
    }
}
