using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping
{
    internal class ExportJson
    {
        public static void CreateJsonFile<T>(string name, List<T> objectsList, List<string> fieldsList)
        {
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\webscraping\\" + name + ".json";

            using (var writer = new StreamWriter(filePath))
            {
                // Create a list to store dynamic objects
                var dynamicObjectsList = new List<dynamic>();

                // Iterate through the objects list
                foreach (var objectToProcess in objectsList)
                {
                    // Create a dynamic object to hold the attribute values
                    var dynamicObject = new ExpandoObject();

                    // Assign properties to the dynamic object based on the fieldsList
                    foreach (var field in fieldsList)
                    {
                        var propertyValue = objectToProcess.GetType().GetProperty(field);
                        if (propertyValue != null)
                        {
                            ((IDictionary<string, object>)dynamicObject).Add(field, propertyValue.GetValue(objectToProcess));
                        }
                    }

                    // Add the dynamic object to the list
                    dynamicObjectsList.Add(dynamicObject);
                }

                // Convert the list of dynamic objects to a JSON-formatted string with lowercase property names
                string json = JsonConvert.SerializeObject(dynamicObjectsList, Formatting.Indented, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                // Write the JSON string to the file
                writer.Write(json);
            }

            Console.WriteLine("\nJSON file exported successfully.");
        }
    }
}
