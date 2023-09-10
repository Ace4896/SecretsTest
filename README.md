# C# - DBus Secrets Test App

A test app for the DBus Secret Service API using [Tmds.Dbus](https://github.com/tmds/Tmds.DBus) for cross-checking against [Tmds.Dbus.SourceGenerator](https://github.com/affederaffe/Tmds.DBus.SourceGenerator).

`Secrets.DBus.cs` is generated using `Tmds.Dbus.Tool` (with minor modifications + a namespace change):

```sh
dotnet dbus codegen --bus session --service org.freedesktop.secrets
```
