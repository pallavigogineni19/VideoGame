

using System;
using System.ComponentModel.DataAnnotations;

namespace VideoGame.Domain.Entities
{
    public class VideoGameModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Genre { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Publisher { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Range(0, 100)]
        public int MetacriticScore { get; set; }
    }
}