using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static BurgerAPI.Models.Burger;

namespace BurgerAPI.Models.Dtos
{
    public class ReviewCreateDto
    {
        [Required]
        public string Title { get; set; }
        public string Text { get; set; }
        public string ImageUrl { get; set; }
        [Required]
        public int Taste { get; set; }
        [Required]
        public int Texture { get; set; }
        [Required]
        public int Visual { get; set; }
        [Required]
        public int BurgerId { get; set; }
        [Required]
        public int UserId { get; set; }
        DateTime DateCreated { get; set; }
    }
}
