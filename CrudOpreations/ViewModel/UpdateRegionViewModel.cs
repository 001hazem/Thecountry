namespace CrudOpreations.ViewModel
{
    public class UpdateRegionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public int CityId { get; set; }
    }
}
