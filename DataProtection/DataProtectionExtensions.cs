using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace dotnet_new_angular.DataProtection
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
    // The following sample code shows how to store (unencrypted) keys using Sql Server
    // The code is based on this repo: https://github.com/adamrushad/AspNetCore.DataProtection.SqlServer
    ////////////////////////////////////////////////////////////////////////////////////////////
    public static class DataProtectionExtensions {

        /// <summary>
        /// Stores DataProtection subsystem keys in a Microsoft SQL database
        /// </summary>
        /// <param name="builder">DataProtection configuration builder. Automatically passed</param>
        /// <param name="connectionString">Connection string for desired SQL database</param>
        /// <param name="schema">Schema to use for the table. Defaults to "DataProtection" if not specified</param>
        /// <param name="table">Table name to use. Defaults to "Keys" if not specified</param>
        public static IDataProtectionBuilder PersistKeysToSqlServer(this IDataProtectionBuilder builder, string connectionString, string schema = "DataProtection", string table = "Keys")
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder
                .Services
                .Configure<KeyManagementOptions>(options => 
                {
                    options.XmlRepository = new SqlServerXmlRepository(connectionString, schema, table);
                })
;
            return builder;
        }
    }
  
}
