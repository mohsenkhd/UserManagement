namespace Domain.Entities
{
    public class Scope : Domain.Base.Base
    {
        public string Name { get; set; } = null!;

        public virtual List<Application> Applications { get; set; } = null!;
    }
}