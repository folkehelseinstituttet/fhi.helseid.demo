using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;

namespace dotnet_new_angular
{
    public static class DataProtectionExtensions { 
        public static void ConfigureDataProtection(this IServiceCollection services)
        {
            // When running on several nodes, you need to persist the keys protecting your cookies.
            // Microsoft provides several mechanisms to support the secure datastores you need to use
            // in your application. 
            // Microsoft also provides mechanisms to encrypt keys before persisting them.
            // This is described here: https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/configuration/overview?view=aspnetcore-3.1

            // Out of the box, for storage you get providers for:
            // - Azure Blob Storage
            // - Redis
            // - DB using Entity Framework Core
            // - Custom XML store (e.g. SQL Server)
            // - File system (make sure to encrypt your keys with some private key)
            // See https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/implementation/key-storage-providers?view=aspnetcore-3.1&tabs=visual-studio
            
            // For protecting the persisted keys ("keys at rest") your out-of-the box options include:
            // - Azure Key Vault
            // - Microsoft DPAPI
            // - Certificates
            // See https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.dataprotection.dataprotectionbuilderextensions?view=aspnetcore-3.1

            ////////////////////////////////////////////////////////////////////////////////////////////
            // The following sample code shows how to store (unencrypted) keys using a custom store.
            // 
            ////////////////////////////////////////////////////////////////////////////////////////////
            
            // Add a custom store (just a local cache in this demo)
            services.AddSingleton<IXmlRepository, PersistedDataProtectionKeys>();

            // Make sure the custom store is available
            var built = services.BuildServiceProvider();

            // Add dataprotection with a unique name for your application
            services.AddDataProtection(opt => opt.ApplicationDiscriminator = "fhi.helseid.demo")
            // Indicate that the custom store should be used 
            .AddKeyManagementOptions(opt => opt.XmlRepository = built.GetService<IXmlRepository>());
        }
    }

    /// <summary>
    /// A simple implementation of a store / repository for persisting dataprotections keys.
    /// NOTE: This sample just stores the keys in a local dictionary, and won't work across a web farm.
    /// </summary>
    public class PersistedDataProtectionKeys : IXmlRepository
    {
        private readonly IDictionary<string, XElement> _store;

        public PersistedDataProtectionKeys()
        {
            _store = new ConcurrentDictionary<string, XElement>();
        }
        public IReadOnlyCollection<XElement> GetAllElements()
        {
            return new ReadOnlyCollection<XElement>(_store.Values.ToList());
        }

        public void StoreElement(XElement element, string friendlyName)
        {
            if(_store.ContainsKey(friendlyName))
            {
                _store[friendlyName] = element;
            } 
            else
            {
                _store.Add(friendlyName, element);
            }
        }
    }
}
