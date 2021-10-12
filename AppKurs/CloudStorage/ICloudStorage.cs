using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AppKurs.CloudStorage
{
    public interface ICloudStorage
    {
        Task<string> UploadFileAsync(IFormFile imageFile, string fileNameForStorage);
        Task DeleteFileAsync(string fileNameForStorage);
    }
}
