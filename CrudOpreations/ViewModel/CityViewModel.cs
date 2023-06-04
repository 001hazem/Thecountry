using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CrudOpreations.ViewModel
{
    public class CityViewModel
    {
        public string Name { get; set; }
        public IFormFile Image { get; set; }

        [DisplayName("Select the country")]
        public int CountryId { get; set; }
    }
}
