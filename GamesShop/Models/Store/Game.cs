using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesShop.Models.Store
{
    [Table("Games")]
    public class Game
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int IdDeveloper { get; set; }
        [ForeignKey("IdDeveloper")]
        public virtual Developer Developer { get; set; }

        public virtual ICollection<GenreAssigment> GenreAssigment { get; set; }
    }
}
