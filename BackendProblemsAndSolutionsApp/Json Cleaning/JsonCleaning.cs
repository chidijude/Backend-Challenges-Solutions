using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BackendProblemsAndSolutionsApp;

public class JsonCleaning
{
    static void Main(string[] args)
    {
        string uri = "https://coderbyte.com/api/challenges/json/json-cleaning";
        
        var result = GetCleandedJson(uri);
        Console.WriteLine(result);
    }

    private static string GetCleandedJson(string url)
    {
        HttpClient client = new();
        string result = client.GetStringAsync(url).Result;
        return FormatJsonString(result);
       
    }
    private static string FormatJsonString(string strJson)
    {
        var data = JsonConvert.DeserializeObject<Dictionary<string,dynamic>>(strJson);

        Dictionary<string, dynamic> result = [];

        foreach (var item in data!)
        {            
            RepopulateObject(item.Key, item.Value, result);
        }
        return JsonConvert.SerializeObject(result);
    }

    private static void RepopulateObject(string key, dynamic value, Dictionary<string, dynamic> resultDictionary)
    {
        if (value is Int64)
        {
            resultDictionary.Add(key, value);
        }
        else if (value is String)
        {
            if (value != "" && value != "-" && value != "N/A")
            {
                resultDictionary.Add(key, value);
            }
        }
        else if (value is JArray jArray)
        {
            List<string> arrResult = [];
            foreach (var itemobj in jArray)
            {
                if (itemobj.Value<string>() != "" && itemobj.Value<string>() != "-" && itemobj.Value<string>() != "N/A")
                {
                    arrResult.Add(itemobj.Value<string>()!);
                }
            }
            resultDictionary.Add(key, arrResult);
        }
        else
        {
            var json = JsonConvert.SerializeObject(value);
            var data = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);

            Dictionary<string, dynamic> recursionResult = [];
            foreach (var item in data!)
            {               
                RepopulateObject(item.Key, item.Value, recursionResult);
            }
            resultDictionary.Add(key, recursionResult);
        }
    }
}
