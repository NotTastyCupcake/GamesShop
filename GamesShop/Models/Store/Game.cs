using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamesShop.Models.Store
{
    [Table("Games")]
    public class Game
    {
        [Key]
        public int Id { get; set; }
        [Required (ErrorMessage = "Не указано название игры")]
        [RegularExpression(@"[A-Za-z0-9]", ErrorMessage = "Некорректное название игры")]
        [Display(Name = "Название игры")]
        public string Name { get; set; }
        [Required (ErrorMessage = "Не указана команда разработчиков")]
        [Display(Name = "Команда разработчиков")]
        public int IdDeveloper { get; set; }
        [ForeignKey("IdDeveloper")]
        [Display(Name = "Команда разработчиков")]
        public virtual Developer Developer { get; set; }
        [Display(Name = "Жанр игры")]
        public virtual ICollection<GenreAssigment> GenreAssigment { get; set; }
    }
}
