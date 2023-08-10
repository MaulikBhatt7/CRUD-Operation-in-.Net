using System.ComponentModel.DataAnnotations;

namespace Data_Annotation.Models
{
    public class CountryModel
    {

        public int CountryId { get; set; }

        [Required]
        public String CountryName { get; set; }
        public String CountryCode { get; set; }
    }
}
