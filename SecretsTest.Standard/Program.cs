﻿using Tmds.DBus;

namespace SecretsTest.Standard;

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

        // Retrieving individual properties works correctly
        Console.WriteLine();
        Console.WriteLine("Retrieving individual properties...");

        var locked = await defaultCollection.GetLockedAsync();
        var label = await defaultCollection.GetLabelAsync();
        var created = await defaultCollection.GetCreatedAsync();
        var modified = await defaultCollection.GetModifiedAsync();
        var items = await defaultCollection.GetItemsAsync();

        Console.WriteLine($"Locked? {locked}");
        Console.WriteLine($"Label: {label}");
        Console.WriteLine($"Created: {created}");
        Console.WriteLine($"Modified: {modified}");
        Console.WriteLine($"Total Items: {items.Length}");

        // Retrieving all properties seems to be broken?
        Console.WriteLine();
        Console.WriteLine("Retrieving all properties...");

        var properties = await defaultCollection.GetAllAsync();

        Console.WriteLine($"Locked? {properties.Locked}");
        Console.WriteLine($"Label: {properties.Label}");
        Console.WriteLine($"Created: {properties.Created}");
        Console.WriteLine($"Modified: {properties.Modified}");
        // Console.WriteLine($"Total Items: {properties.Items.Length}");    // Null reference exception

        Console.WriteLine();
        Console.WriteLine("Closing session...");
        await session.CloseAsync();
        Console.WriteLine("Closed session");
    }
}
