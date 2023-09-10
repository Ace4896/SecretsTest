using Tmds.DBus.Protocol;
using Tmds.DBus.SourceGenerator;

namespace SecretsTest.Generated;

public sealed class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Connecting to secret service...");

        var connection = Connection.Session;
        var secretService = new OrgFreedesktopSecretService(connection, "org.freedesktop.secrets", "/org/freedesktop/secrets");
        
        (var _, var sessionPath) = await secretService.OpenSessionAsync("plain", new DBusVariantItem("s", new DBusStringItem(string.Empty)));
        var session = new OrgFreedesktopSecretSession(connection, "org.freedesktop.secrets", sessionPath);

        Console.WriteLine($"Opened session at {sessionPath}");

        Console.WriteLine("Retrieving default collection...");

        var defaultCollectionPath = await secretService.ReadAliasAsync("default");
        var defaultCollection = new OrgFreedesktopSecretCollection(connection, "org.freedesktop.secrets", defaultCollectionPath);
        
        Console.WriteLine("Retrieved default collection");

        // There's three issues while retrieving individual properties:
        // - If the collection is unlocked, it still returns true
        // - Retrieving the label causes an IndexOutOfRange exception
        // - The created/modified timestamps are incorrect
        Console.WriteLine();
        Console.WriteLine("Retrieving individual properties...");

        var locked = await defaultCollection.GetLockedPropertyAsync();
        // var label = await defaultCollection.GetLabelPropertyAsync();
        var created = await defaultCollection.GetCreatedPropertyAsync();
        var modified = await defaultCollection.GetModifiedPropertyAsync();

        Console.WriteLine($"Locked? {locked}");
        // Console.WriteLine($"Label: {label}");
        Console.WriteLine($"Created: {created}");
        Console.WriteLine($"Modified: {modified}");

        // Everything is read correctly when retrieving all properties at once
        Console.WriteLine();
        Console.WriteLine("Retrieving all properties...");

        var properties = await defaultCollection.GetAllPropertiesAsync();

        Console.WriteLine($"Locked? {properties.Locked}");
        Console.WriteLine($"Label: {properties.Label}");
        Console.WriteLine($"Created: {properties.Created}");
        Console.WriteLine($"Modified: {properties.Modified}");

        Console.WriteLine();
        Console.WriteLine("Closing session...");
        await session.CloseAsync();
        Console.WriteLine("Closed session");
    }
}
