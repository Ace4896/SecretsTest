# C# - DBus Secrets Test App

A test app for the DBus Secret Service API using [Tmds.Dbus](https://github.com/tmds/Tmds.DBus) for cross-checking against [Tmds.Dbus.SourceGenerator](https://github.com/affederaffe/Tmds.DBus.SourceGenerator).

`Secrets.DBus.cs` is generated using `Tmds.Dbus.Tool` (with minor modifications + a namespace change):

```sh
dotnet dbus codegen --bus session --service org.freedesktop.secrets
```

## Comparing Outputs

On my system, the expected output should be something like this (modification timestamps will be different):

```
Locked? False
Label: kdewallet
Created: 1676796746
Modified: 1694360288
Total Items: 4
```

Regular Tmds seems to be working when retrieving individual properties, but is broken when retrieving all properties at once (results in uninitialized values):

```
$ dotnet run --project SecretsTest.Standard/
Connecting to secret service...
Opened session at /org/freedesktop/secrets/session/44
Retrieving default collection...
Retrieved default collection

Retrieving individual properties...
Locked? False
Label: kdewallet
Created: 1676796746
Modified: 1694366682
Total Items: 4

Retrieving all properties...
Locked? False
Label: 
Created: 0
Modified: 0

Closing session...
Closed session
```

The source generator appears to be broken when retrieving individual properties, but works when retrieving all properties at once. The label can't be retrieved individually as it results in an `IndexOutOfRangeException`, while the total items can't be retrieved as it results in an `ArgumentOutOfRangeException`:

```
$ dotnet run --project SecretsTest.Generated/
Connecting to secret service...
Opened session at /org/freedesktop/secrets/session/45
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
Modified: 1694366682
Total Items: 4

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

The `ArgumentOutOfRangeException` has the following stack trace:

```
Unhandled exception. System.ArgumentOutOfRangeException: Specified argument was out of the range of valid values. (Parameter 'count')
   at System.ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument argument)
   at System.Buffers.SequenceReader`1.AdvanceToNextSpan(Int64 count)
   at Tmds.DBus.Protocol.Reader.ReadSpan(Int32 length)
   at Tmds.DBus.Protocol.Reader.ReadSpan()
   at Tmds.DBus.Protocol.Reader.ReadString()
   at Tmds.DBus.Protocol.Reader.ReadObjectPath()
   at Tmds.DBus.SourceGenerator.ReaderExtensions.ReadArray_ao(Reader& reader) in /home/jpacheco/Documents/Repositories/Local/C#/SecretsTest/SecretsTest.Generated/Tmds.DBus.SourceGenerator/Tmds.DBus.SourceGenerator.DBusSourceGenerator/Tmds.DBus.SourceGenerator.ReaderExtensions.cs:line 37
   at Tmds.DBus.SourceGenerator.ReaderExtensions.ReadMessage_ao(Message message, Object _) in /home/jpacheco/Documents/Repositories/Local/C#/SecretsTest/SecretsTest.Generated/Tmds.DBus.SourceGenerator/Tmds.DBus.SourceGenerator.DBusSourceGenerator/Tmds.DBus.SourceGenerator.ReaderExtensions.cs:line 96
   at Tmds.DBus.Protocol.DBusConnection.<>c__41`1.<CallMethodAsync>b__41_0(Exception exception, Message message, Object state1, Object state2, Object state3)
--- End of stack trace from previous location ---
   at Tmds.DBus.Protocol.DBusConnection.MyValueTaskSource`1.System.Threading.Tasks.Sources.IValueTaskSource<T>.GetResult(Int16 token)
   at Tmds.DBus.Protocol.DBusConnection.CallMethodAsync[T](MessageBuffer message, MessageValueReader`1 valueReader, Object state)
   at Tmds.DBus.Protocol.Connection.CallMethodAsync[T](MessageBuffer message, MessageValueReader`1 reader, Object readerState)
   at SecretsTest.Generated.Program.Main(String[] args) in /home/jpacheco/Documents/Repositories/Local/C#/SecretsTest/SecretsTest.Generated/Program.cs:line 38
   at SecretsTest.Generated.Program.<Main>(String[] args)
```
