namespace CrudOpreations.Models
{
    public class BaseEntity
    {
        public DateTime createAt { get; set; }
        public DateTime UpdateaAt { get; set; }
        public bool IsDelete { get; set; }

    }
}
