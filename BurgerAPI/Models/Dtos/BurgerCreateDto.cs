using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static BurgerAPI.Models.Burger;

namespace BurgerAPI.Models.Dtos
{
    public class BurgerCreateDto
    {
        [Required]
        public string Name { get; set; }
        public BurgerType Type { get; set; }
        public BurgerSize Size { get; set; }
        [Required]
        public int PlaceId { get; set; }
    }
}
