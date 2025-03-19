namespace Company.PL.Services
{
    public interface ISingletionService
    {
        public Guid Guid { get; set; }

        string GetGuid();
    }
}
