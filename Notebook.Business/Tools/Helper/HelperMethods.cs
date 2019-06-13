using Newtonsoft.Json;
using System;
using System.IO;

public class HelperMethods
{
    #region JsonConvert

    public static string JsonConvertString(object model)
    {
        return JsonConvert.SerializeObject(model);
    }

    public static object JsonConvertObject(string data)
    {
        return JsonConvert.DeserializeObject(data);
    }

    public static T JsonConvertObject<T>(string data)
    {
        return JsonConvert.DeserializeObject<T>(data);
    }

    #endregion

    #region File Operations

    public static bool FileExists(string root)
    {
        return Directory.Exists(root);
    }

    #endregion
}
