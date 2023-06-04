namespace CrudOpreations.ViewModel
{
    public class UpdateCityViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public int CountryId { get; set; }
    }
}
