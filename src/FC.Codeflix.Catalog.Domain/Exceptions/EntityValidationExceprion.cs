namespace FC.Codeflix.Catalog.Domain.Exceptions
{
    public class EntityValidationExceprion : Exception
    {
        public EntityValidationExceprion(string? message) : base(message)
        {
        }
    }
}
