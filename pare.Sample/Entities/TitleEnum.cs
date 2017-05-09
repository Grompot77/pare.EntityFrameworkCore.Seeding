using pare.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pare.Sample.Entities
{
    public enum TitleEnum
    {
        [Display(Description = "Mister")]
        Mr = 1,
        Mrs,
        Miss,
        [Display(Description = "Doctor")]
        Dr,
        Ds,
        [Display(Description = "Professor")] 
        Prof
    }

    [Table(nameof(Title))]
    public class Title : DbEnum<TitleEnum>
    {
        public Title() : this(default(TitleEnum)) { }
        public Title(TitleEnum value) : base(value) { }

        public static implicit operator Title(TitleEnum value)
        {
            return new Title(value);
        }

        public static implicit operator TitleEnum(Title value)
        {
            return value.ToEnum();
        }
    }
}
