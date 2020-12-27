namespace AzureFunctionHost.Domain
{
    public interface IIdentifiable<TId>
    {
        TId Id { get; }
    }
}
