# C# - DBus Secrets Test App

A test app for the DBus Secret Service API using [Tmds.Dbus](https://github.com/tmds/Tmds.DBus) for cross-checking against [Tmds.Dbus.SourceGenerator](https://github.com/affederaffe/Tmds.DBus.SourceGenerator).

`Secrets.DBus.cs` is generated using `Tmds.Dbus.Tool` (with minor modifications + a namespace change):

```sh
dotnet dbus codegen --bus session --service org.freedesktop.secrets
```

## Comparing Outputs

On my system, the expected output should be:

```
Locked? False
Label: kdewallet
Created: 1676796746
Modified: 1694360288
```

Regular Tmds seems to be working when retrieving individual properties, but is broken when retrieving all properties at once:

```
$ dotnet run --project SecretsTest.Standard/
Connecting to secret service...
Opened session at /org/freedesktop/secrets/session/30
Retrieving default collection...
Retrieved default collection

Retrieving individual properties...
Locked? False
Label: kdewallet
Created: 1676796746
Modified: 1694360288

Retrieving all properties...
Locked? False
Label: 
Created: 0
Modified: 0

Closing session...
Closed session
```

The source generator appears to be broken when retrieving individual properties, but works when retrieving all properties at once. The label can't be retrieved individually as it results in an `IndexOutOfRangeException`:

```
$ dotnet run --project SecretsTest.Generated/
Connecting to secret service...
Opened session at /org/freedesktop/secrets/session/31
Retrieving default collection...
Retrieved default collection

Retrieving individual properties...
Locked? True
Created: 29697
Modified: 29697

Retrieving all properties...
Locked? False
Label: kdewallet
Created: 1676796746
Modified: 1694360288

Closing session...
Closed session
```

The `IndexOutOfRangeException` has the following stack trace:

```
Unhandled exception. System.IndexOutOfRangeException: Index was outside the bounds of the array.
   at Tmds.DBus.Protocol.ThrowHelper.ThrowIndexOutOfRange()
   at Tmds.DBus.Protocol.Reader.ReadSpan(Int32 length)
   at Tmds.DBus.Protocol.Reader.ReadSpan()
   at Tmds.DBus.Protocol.Reader.ReadString()
   at Tmds.DBus.SourceGenerator.ReaderExtensions.ReadMessage_s(Message message, Object _) in /home/jpacheco/Documents/Repositories/Local/C#/SecretsTest/SecretsTest.Generated/Tmds.DBus.SourceGenerator/Tmds.DBus.SourceGenerator.DBusSourceGenerator/Tmds.DBus.SourceGenerator.ReaderExtensions.cs:line 111
   at Tmds.DBus.Protocol.DBusConnection.<>c__41`1.<CallMethodAsync>b__41_0(Exception exception, Message message, Object state1, Object state2, Object state3)
--- End of stack trace from previous location ---
   at Tmds.DBus.Protocol.DBusConnection.MyValueTaskSource`1.System.Threading.Tasks.Sources.IValueTaskSource<T>.GetResult(Int16 token)
   at Tmds.DBus.Protocol.DBusConnection.CallMethodAsync[T](MessageBuffer message, MessageValueReader`1 valueReader, Object state)
   at Tmds.DBus.Protocol.Connection.CallMethodAsync[T](MessageBuffer message, MessageValueReader`1 reader, Object readerState)
   at SecretsTest.Generated.Program.Main(String[] args) in /home/jpacheco/Documents/Repositories/Local/C#/SecretsTest/SecretsTest.Generated/Program.cs:line 35
   at SecretsTest.Generated.Program.<Main>(String[] args)
```
