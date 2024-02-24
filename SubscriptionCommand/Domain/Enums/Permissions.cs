namespace SubscriptionCommand.Domain.Enums;

[Flags]
public enum Permissions : long
{
    Transfer,
    PurchaseCards,
    ManageDevices
}