using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Examen_2.Models
{
    public class CategoryType
    {
        [Key]
        public int CategoryId { get; set; }

        [Display(Name = "Categoria")]
        [StringLength(40, ErrorMessage = "No debe tener mas de 40 caracteres")]
        [MinLength(3, ErrorMessage = "Debe de tener menos de 3 caracteres")]
        public string CategoryName { get; set; }
        public IEnumerable<Homework> Homeworks { get; set; }
    }
}
