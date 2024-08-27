using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorialsEUIdentity.Models
{
    public class ProductModel
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int Price { get; set; }

        public string? Image { get; set; }

        public string? Category { get; set; }

        //this is used for handling file uploads and wont be mapped to the db
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
