using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Cloudinary;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Infrastructure.CloudinaryService
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration configuration)
        {
            var cloudName = configuration["CloudinarySettings:CloudName"] ?? "";
            var apiKey = configuration["CloudinarySettings:ApiKey"] ?? "";
            var apiSecret = configuration["CloudinarySettings:ApiSecret"] ?? "";

            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }
        public async Task DeleteAsync(string publicId)
        {
            var deletedParam = new DeletionParams(publicId)
            {
                ResourceType = CloudinaryDotNet.Actions.ResourceType.Video
            };

            await _cloudinary.DestroyAsync(deletedParam);
        }

        public async Task DeleteImageAsync(string publicId)
        {
            var deletedParam = new DeletionParams(publicId);

            await _cloudinary.DestroyAsync(deletedParam);
        }

        public async Task<string> UploadAsync(IFormFile file, string folderName)
        {
            using var stream = file.OpenReadStream();

            var uploadParam = new VideoUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folderName
            };

            var result = await _cloudinary.UploadAsync(uploadParam);

            if (result.Error != null)
                throw new Exception($"Cloudinary upload failed: {result.Error.Message}");

            return result.SecureUrl.ToString();
        }

        public async Task<string> UploadImageAsync(IFormFile file, string folderName)
        {
            using var stream = file.OpenReadStream();

            var uploadParam = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folderName
            };

            var result = await _cloudinary.UploadAsync(uploadParam);

            if (result.Error != null)
                throw new Exception($"Cloudinary upload failed: {result.Error.Message}");

            return result.SecureUrl.ToString();
        }
    }

    public class CloudinarySettings
    {
        public string CloudName { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
    }
}

