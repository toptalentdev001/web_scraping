using CsvHelper;
using System.Dynamic;
using System.Globalization;
using System.Text;

namespace WebScraping
{
    internal class ExportCsv
    {
        public static void CreateCsvFile<T>(string name, List<T> objectsList, List<string> fieldsList)
        {
            //filePath = Path.GetFullPath(filePath);
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\" + name;

            using (var writer = new StreamWriter(filePath))
            using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                for (int i = 0; i < fieldsList.Count; i++)
                {
                    csvWriter.WriteField(fieldsList[i]);
                }

                // Iterate through the objects list
                foreach (var objectToProcess in objectsList)
                {
                    // Create a dynamic object to hold the attribute values
                    var dynamicObject = new ExpandoObject();

                    // Assign properties to the dynamic object based on the objectToProcess
                    foreach (var field in fieldsList)
                    {
                        var propertyValue = objectToProcess.GetType().GetProperty(field);
                        if (propertyValue != null)
                        {
                            dynamicObject.TryAdd(field, propertyValue.GetValue(objectToProcess));
                        }
                    }

                    // CRLF between records
                    csvWriter.NextRecord();

                    // Write the dynamic object to the CSV file
                    csvWriter.WriteRecord(dynamicObject);
                }
            }
        }
    }
}
