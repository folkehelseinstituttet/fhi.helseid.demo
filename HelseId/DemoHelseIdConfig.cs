using Fhi.HelseId.Web;

namespace dotnet_new_angular.HelseId
{
    /// <summary>
    /// Marker interface for HelseId configuration.
    /// Named in English for consistency in the code base, even though it inherits from a Norwegian named interface.
    /// </summary>
    public interface IHelseIdConfiguration : IHelseIdWebKonfigurasjon
    {

    }

    /// <summary>
    /// Class for strongly typed configuration of HelseId.
    /// </summary>
    public class DemoHelseIdConfig : HelseIdWebKonfigurasjon, IHelseIdConfiguration
    {
        
    }
}