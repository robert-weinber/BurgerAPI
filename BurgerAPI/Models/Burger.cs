using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BurgerAPI.Models
{
    public class Burger
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public enum BurgerType { Vegan, Normal, Carnivore }
        public BurgerType Type { get; set; }
        public enum BurgerSize { Small, Medium, Big, Impossible }
        public BurgerSize Size { get; set; }
        [Required]
        public int PlaceId { get; set; }

        [ForeignKey("PlaceId")]
        public Place Place { get; set; }
    }
}
