using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notebook.Web.Models
{
    public class FileModel
    {
        public IFormFile FormFile { get; set; }
        public string Extension { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public bool IsWebRoot { get; set; }
        public float Length { get; set; }
        public FileType Type { get; set; }
        public DateTime LastModified { get; set; }
    }

    public enum FileType
    {
        File,
        Image,
        Video
    }
}
