using Entities.Notificacoes;
using System.ComponentModel.DataAnnotations;

namespace Entities.Entidades
{
    public class Base
    {
        [Display(Name = "Código")]
        public int Id { get; set; }       
    }
}
