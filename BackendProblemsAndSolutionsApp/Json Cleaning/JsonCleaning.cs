using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BackendProblemsAndSolutionsApp;

public class JsonCleaning
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        string uri = "https://coderbyte.com/api/challenges/json/json-cleaning";
        
        var result = getString(uri);
        Console.WriteLine(result);
    }

    private static string getString(string url)
    {
        HttpClient client = new();
        string result = client.GetStringAsync(url).Result;
        return FormatJsonString(result);
       
    }
    private static string FormatJsonString(string strJson)
    {
        var data = JsonConvert.DeserializeObject<Dictionary<string,dynamic>>(strJson);

        Dictionary<string, dynamic> resultListMain = [];

        foreach (var item in data!)
        {            
            RepopulateObject(item.Key, item.Value, resultListMain);
        }
        return JsonConvert.SerializeObject(resultListMain);
    }

    private static void RepopulateObject(string key, dynamic value, Dictionary<string, dynamic> resultListMain)
    {
        Type type = value.GetType();
        if (value is Int64)
        {
            resultListMain.Add(key, value);
        }
        else if (value is String)
        {
            if (value != "" && value != "-" && value != "N/A")
            {
                resultListMain.Add(key, value);
            }
        }
        else if (value is JArray jArray)
        {
            List<string> strarr = [];
            foreach (var itemobj in jArray)
            {
                if (itemobj.Value<string>() != "" && itemobj.Value<string>() != "-" && itemobj.Value<string>() != "N/A")
                {
                    strarr.Add(itemobj.Value<string>()!);
                }
            }
            resultListMain.Add(key, strarr);
        }
        else
        {
            var json = JsonConvert.SerializeObject(value);
            var data = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);

            Dictionary<string, dynamic> resultListMain2 = [];
            foreach (var item in data!)
            {               
                RepopulateObject(item.Key, item.Value, resultListMain2);
            }
            resultListMain.Add(key, resultListMain2);
        }
    }
}
