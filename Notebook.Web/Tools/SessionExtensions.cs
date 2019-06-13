using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;

public static class SessionExtensions
{
    public static void SetSession(this ISession session, string key, object value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value, Formatting.Indented,
            new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            }));
    }

    public static T GetSession<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
    }

    public static void SetCookies(this IResponseCookies Response,string key, string value, int dayTime = 365)
    {
        CookieOptions option = new CookieOptions();
        option.Expires = DateTime.Now.AddDays(dayTime);
        value = value.Encrypt("notebook");

        Response.Append(key, value , option);
    }

    public static string GetCookies(this IRequestCookieCollection Request,string key)
    {
        string _key = Request[key] as string;

        if (!string.IsNullOrEmpty(_key))
            return _key = _key.Decrypt("notebook");
        else
            return null;
    }
}
