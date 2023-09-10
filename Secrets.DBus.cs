using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Tmds.DBus;

[assembly: InternalsVisibleTo(Tmds.DBus.Connection.DynamicAssemblyName)]
namespace SecretsTest.Generated
{
    [DBusInterface("org.freedesktop.Secret.Service")]
    interface IService : IDBusObject
    {
        Task<(object output, ObjectPath result)> OpenSessionAsync(string Algorithm, object Input);
        Task<(ObjectPath collection, ObjectPath prompt)> CreateCollectionAsync(IDictionary<string, object> Properties, string Alias);
        Task<(ObjectPath[] unlocked, ObjectPath[] locked)> SearchItemsAsync(IDictionary<string, string> Attributes);
        Task<(ObjectPath[] unlocked, ObjectPath prompt)> UnlockAsync(ObjectPath[] Objects);
        Task<(ObjectPath[] locked, ObjectPath prompt)> LockAsync(ObjectPath[] Objects);
        Task<IDictionary<ObjectPath, (ObjectPath, byte[], byte[], string)>> GetSecretsAsync(ObjectPath[] Items, ObjectPath Session);
        Task<ObjectPath> ReadAliasAsync(string Name);
        Task SetAliasAsync(string Name, ObjectPath Collection);
        Task<IDisposable> WatchCollectionCreatedAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchCollectionDeletedAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchCollectionChangedAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
        Task<T> GetAsync<T>(string prop);
        Task<ServiceProperties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }

    [Dictionary]
    class ServiceProperties
    {
        private ObjectPath[] _collections = default(ObjectPath[]);
        public ObjectPath[] Collections
        {
            get
            {
                return _collections;
            }

            set
            {
                _collections = (value);
            }
        }
    }

    static class ServiceExtensions
    {
        public static Task<ObjectPath[]> GetCollectionsAsync(this IService o) => o.GetAsync<ObjectPath[]>("Collections");
    }

    [DBusInterface("org.freedesktop.Secret.Collection")]
    interface ICollection : IDBusObject
    {
        Task<ObjectPath> DeleteAsync();
        Task<ObjectPath[]> SearchItemsAsync(IDictionary<string, string> Attributes);
        Task<(ObjectPath item, ObjectPath prompt)> CreateItemAsync(IDictionary<string, object> Properties, (ObjectPath, byte[], byte[], string) Secret, bool Replace);
        Task<IDisposable> WatchItemCreatedAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchItemDeletedAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchItemChangedAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
        Task<T> GetAsync<T>(string prop);
        Task<CollectionProperties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }

    [Dictionary]
    class CollectionProperties
    {
        private ObjectPath[] _items = default(ObjectPath[]);
        public ObjectPath[] Items
        {
            get
            {
                return _items;
            }

            set
            {
                _items = (value);
            }
        }

        private string _label = default(string);
        public string Label
        {
            get
            {
                return _label;
            }

            set
            {
                _label = (value);
            }
        }

        private bool _locked = default(bool);
        public bool Locked
        {
            get
            {
                return _locked;
            }

            set
            {
                _locked = (value);
            }
        }

        private ulong _created = default(ulong);
        public ulong Created
        {
            get
            {
                return _created;
            }

            set
            {
                _created = (value);
            }
        }

        private ulong _modified = default(ulong);
        public ulong Modified
        {
            get
            {
                return _modified;
            }

            set
            {
                _modified = (value);
            }
        }
    }

    static class CollectionExtensions
    {
        public static Task<ObjectPath[]> GetItemsAsync(this ICollection o) => o.GetAsync<ObjectPath[]>("Items");
        public static Task<string> GetLabelAsync(this ICollection o) => o.GetAsync<string>("Label");
        public static Task<bool> GetLockedAsync(this ICollection o) => o.GetAsync<bool>("Locked");
        public static Task<ulong> GetCreatedAsync(this ICollection o) => o.GetAsync<ulong>("Created");
        public static Task<ulong> GetModifiedAsync(this ICollection o) => o.GetAsync<ulong>("Modified");
        public static Task SetLabelAsync(this ICollection o, string val) => o.SetAsync("Label", val);
    }

    [DBusInterface("org.freedesktop.Secret.Item")]
    interface IItem : IDBusObject
    {
        Task<ObjectPath> DeleteAsync();
        Task<(ObjectPath secret, byte[], byte[], string)> GetSecretAsync(ObjectPath Session);
        Task SetSecretAsync((ObjectPath, byte[], byte[], string) Secret);
        Task<T> GetAsync<T>(string prop);
        Task<ItemProperties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }

    [Dictionary]
    class ItemProperties
    {
        private bool _locked = default(bool);
        public bool Locked
        {
            get
            {
                return _locked;
            }

            set
            {
                _locked = (value);
            }
        }

        private IDictionary<string, string> _attributes = default(IDictionary<string, string>);
        public IDictionary<string, string> Attributes
        {
            get
            {
                return _attributes;
            }

            set
            {
                _attributes = (value);
            }
        }

        private string _label = default(string);
        public string Label
        {
            get
            {
                return _label;
            }

            set
            {
                _label = (value);
            }
        }

        private string _type = default(string);
        public string Type
        {
            get
            {
                return _type;
            }

            set
            {
                _type = (value);
            }
        }

        private ulong _created = default(ulong);
        public ulong Created
        {
            get
            {
                return _created;
            }

            set
            {
                _created = (value);
            }
        }

        private ulong _modified = default(ulong);
        public ulong Modified
        {
            get
            {
                return _modified;
            }

            set
            {
                _modified = (value);
            }
        }
    }

    static class ItemExtensions
    {
        public static Task<bool> GetLockedAsync(this IItem o) => o.GetAsync<bool>("Locked");
        public static Task<IDictionary<string, string>> GetAttributesAsync(this IItem o) => o.GetAsync<IDictionary<string, string>>("Attributes");
        public static Task<string> GetLabelAsync(this IItem o) => o.GetAsync<string>("Label");
        public static Task<string> GetTypeAsync(this IItem o) => o.GetAsync<string>("Type");
        public static Task<ulong> GetCreatedAsync(this IItem o) => o.GetAsync<ulong>("Created");
        public static Task<ulong> GetModifiedAsync(this IItem o) => o.GetAsync<ulong>("Modified");
        public static Task SetAttributesAsync(this IItem o, IDictionary<string, string> val) => o.SetAsync("Attributes", val);
        public static Task SetLabelAsync(this IItem o, string val) => o.SetAsync("Label", val);
        public static Task SetTypeAsync(this IItem o, string val) => o.SetAsync("Type", val);
    }

    [DBusInterface("org.freedesktop.Secret.Session")]
    interface ISession : IDBusObject
    {
        Task CloseAsync();
    }
}