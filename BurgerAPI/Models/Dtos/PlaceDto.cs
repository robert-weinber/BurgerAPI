using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BurgerAPI.Models.Dtos
{
    public class PlaceDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string City { get; set; }
        public string Address { get; set; }
        public string Openings { get; set; }
        public string Info { get; set; }
        DateTime DateFounded { get; set; }
    }
}
