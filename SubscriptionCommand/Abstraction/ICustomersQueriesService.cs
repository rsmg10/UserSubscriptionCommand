namespace SubscriptionCommand.Abstraction
{
    public interface ICustomersQueriesService
    {
        Task EnsurePhoneAndEmailAreUniqueAsync(string phone, string email, CancellationToken cancellationToken);
        Task EnsurePhoneAndEmailAreUniqueAsync(Guid customerId, string phone, string email, CancellationToken cancellationToken);
    }
}
