using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MongoDBAdmin.Dto.WhatWeHaveDone
{
    public class CreateWhatWeHaveDoneDto
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        public string? ImageURL { get; set; }

        public IFormFile? ImageFile { get; set; }

        public bool IsActive { get; set; }
    }
}
