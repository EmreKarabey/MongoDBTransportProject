using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Cloudinary
{
    public interface ICloudinaryService
    {
        public Task<string> UploadAsync(IFormFile file, string folderName);
        public Task DeleteAsync(string publicId);

        public Task<string> UploadImageAsync(IFormFile file, string folderName);
        public Task DeleteImageAsync(string publicId);
    }
}
