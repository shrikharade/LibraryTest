using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Domain.Dto
{
    public class BookDto
    {
        public Guid  Id { get; set; }
        
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? AuthorName { get; set; }
    }
}
