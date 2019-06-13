using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

public class FileOperations
{
    private readonly IHostingEnvironment env;
    public FileOperations(IHostingEnvironment _env)
    {
        env = _env;
    }

    public string GetWebRootPath()
    {
        return env.WebRootPath;
    }

    public string GetContentRootPath()
    {
        return env.ContentRootPath;
    }

    public string AddFile(IFormFile file, string _url = "System/Others", string Name = "")
    {
        string Url = "";

        if (file != null)
        {
            string fileExtension = file.FileName.Substring(file.FileName.LastIndexOf("."));
            string fileName = file.FileName.Substring(0, file.FileName.LastIndexOf("."));
            fileName = (!string.IsNullOrEmpty(Name) ? Name : (fileName + "_" + DateTime.Now)) + fileExtension;
            
            try
            {
                string root = env.WebRootPath + "/" + _url + "/";

                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }

                root = root + fileName;

                using (var stream = File.Open(root, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Flush();
                }

                Url = "/" + _url + "/" + fileName;
            }
            catch (Exception ex)
            {
                // Hata Ekle
            }
        }

        return Url;
    }

    public bool RemoveFile(string Url = "")
    {
        bool snc = false;

        if (!string.IsNullOrEmpty(Url))
        {
            Url = env.WebRootPath + Url;
            File.Delete(Url);
            snc = true;
        }

        return snc;
    }

    //public bool Logger(LogVM log, string Location = "System/Logs")
    //{
    //    string root = env.WebRootPath + "/" + Location + "/";
    //    List<LogVM> logs;

    //    if (!Directory.Exists(root))
    //    {
    //        Directory.CreateDirectory(root);
    //    }

    //    root = root + DateTime.Now.ToShortDateString().Replace(".", "-") + ".json";

    //    if (!File.Exists(root))
    //    {
    //        var stream = File.Open(root, FileMode.Create);
    //        stream.Close();
    //    }

    //    using (StreamReader _StreamReader = new StreamReader(root))
    //    {
    //        string data = _StreamReader.ReadToEnd();
    //        logs = JsonConvert.DeserializeObject<List<LogVM>>(data);
    //    }

    //    if (logs == null)
    //    {
    //        logs = new List<LogVM>();
    //    }

    //    logs.Add(log);
    //    string jsonData = JsonConvert.SerializeObject(logs);
    //    File.WriteAllText(root, jsonData);

    //    return true;
    //}

    public object ReadJson<T>(string url, bool IsListType = true)
    {
        string root = env.WebRootPath;
        using (StreamReader _StreamReader = new StreamReader(root + url))
        {
            string jsonData = _StreamReader.ReadToEnd();
            if (IsListType)
            {
                var list = JsonConvert.DeserializeObject<List<T>>(jsonData);
                return list;
            }
            else
            {
                var list = JsonConvert.DeserializeObject<T>(jsonData);
                return list;
            }
        }

    }

    public void WriteJson(string url, object model)
    {
        string root = env.WebRootPath;
        string jsonData = JsonConvert.SerializeObject(model);
        File.WriteAllText(root + url, jsonData);
    }

    public FileInfo[] GetFiles(string url)
    {
        string root = env.WebRootPath + "/" + url + "/";
        DirectoryInfo folder = new DirectoryInfo(root);
        FileInfo[] files = folder.GetFiles();
        return files;
    }
}
