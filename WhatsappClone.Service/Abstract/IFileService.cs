using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Service.Abstract
{
    public interface IFileService
    {

        Task<string> SaveFileAsync(IFormFile file, string subDirectory);
        Task DeleteFileAsync(string filePath);
    }
}
