using System.ComponentModel.DataAnnotations;

namespace ToDo.Api.entities
{
    public class Todo
    {
        public Guid Id { get; set; }

        public DateTime moment { get; set; } = DateTime.Now;
        [Required]
        public string Title { get; set; } = default!;
        [MaxLength(500)]
        public string? Description { get; set; }
        
        public bool IsComplete { get; set; }=false;

        [Required]
        public string OwnerId { get; set; } = default!;
    }

}
