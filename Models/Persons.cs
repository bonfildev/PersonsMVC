namespace PersonsMVC.Models
{
    public class Persons
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required int Age { get; set; }
        public string Email { get; set; }

        public Persons()
        {

        }

    }
}
