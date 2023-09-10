using SecretsTest.Generated;
using Tmds.DBus;

namespace SecretsTest;

public sealed class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Connecting to secret service...");

        var connection = Connection.Session;
        var secretService = connection.CreateProxy<IService>("org.freedesktop.secrets", "/org/freedesktop/secrets");
        
        (var _, var sessionPath) = await secretService.OpenSessionAsync("plain", "");
        var session = connection.CreateProxy<ISession>("org.freedesktop.secrets", sessionPath);

        Console.WriteLine($"Opened session at {sessionPath}");

        Console.WriteLine("Retrieving default collection...");

        var defaultCollectionPath = await secretService.ReadAliasAsync("default");
        var defaultCollection = connection.CreateProxy<ICollection>("org.freedesktop.secrets", defaultCollectionPath);
        
        Console.WriteLine("Retrieved default collection");

        var locked = await defaultCollection.GetAsync<bool>("Locked");
        var label = await defaultCollection.GetAsync<string>("Label");
        var created = await defaultCollection.GetAsync<ulong>("Created");
        var modified = await defaultCollection.GetAsync<ulong>("Modified");

        Console.WriteLine($"Locked? {locked}");
        Console.WriteLine($"Label: {label}");
        Console.WriteLine($"Created: {created}");
        Console.WriteLine($"Modified: {modified}");

        Console.WriteLine("Closing session...");
        await session.CloseAsync();
        Console.WriteLine("Closed session");

        Console.WriteLine("Finished; press any key to exit");
        Console.ReadKey();
    }
}