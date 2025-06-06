namespace Domain.Entities
{
    public class Application : Domain.Base.Base
    {
        public string Name { get; set; } = null!;
        public string Password { get; set; } = null!;

        public List<User> Users { get; set; } = null!;

        public List<Scope> Scopes { get; set; } = null!;
    }
}