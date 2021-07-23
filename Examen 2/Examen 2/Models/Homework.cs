using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Examen_2.Models
{
    public class Homework
    {
        [Key]
        public int TaskId { get; set; }

        [Display(Name = "Nombre")]
        [StringLength(70, ErrorMessage = "No debe tener mas de 70 caracteres")]
        [MinLength(3, ErrorMessage = "Debe de tener menos de 3 caracteres")]
        public string Description { get; set; }

        [Display(Name = "Fecha de Creacion")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Fecha de Finalizacion")]
        public DateTime FinalDate { get; set; }
        [Display(Name = "Categoria")]
        public int CategoryId { get; set; }

        [Display(Name ="Categoria")]
        [ForeignKey("CategoryId")]
        public CategoryType CategoryType { get; set; }
    }
}
