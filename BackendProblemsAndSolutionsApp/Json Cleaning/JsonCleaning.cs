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
        var exclusions = new HashSet<string> { "", "-", "N/A", "n/a" };

        switch (value)
        {
            case long _:
                resultDictionary.Add(key, value);
                break;

            case string str:
                if(!exclusions.Contains(str))
                    resultDictionary.Add(key, str);
                break;

            case JArray jArray:
                var arrResult = new List<string>();
                foreach (var itemobj in jArray)
                {
                    var itemStr = itemobj.Value<string>();
                    if (!exclusions.Contains(itemStr!))
                    {
                        arrResult.Add(itemStr!);
                    }
                }
                resultDictionary.Add(key, arrResult);
                break;

            default:
                var json = JsonConvert.SerializeObject(value);
                var data = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);
                var recursionResult = new Dictionary<string, dynamic>();
                foreach (var item in data!)
                {
                    RepopulateObject(item.Key, item.Value, recursionResult);
                }
                resultDictionary.Add(key, recursionResult);
                break;
        }
    }

}
