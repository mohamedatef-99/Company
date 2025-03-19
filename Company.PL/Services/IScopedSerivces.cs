namespace Company.PL.Services
{
    public interface IScopedSerivces
    {
        public Guid Guid { get; set; }

        string GetGuid();
    }
}
