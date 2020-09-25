using System.Collections.Generic;
using Fhi.HelseId.Common.Identity;
using Fhi.HelseId.Web.Services;

namespace dotnet_new_angular.HelseId
{
    public class WhitelistConfiguration
    {
        public Whitelist Whitelist { get; set; } = new Whitelist();

        public IEnumerable<White> List => Whitelist;
    }
}