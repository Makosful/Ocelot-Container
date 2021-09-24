using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ApiGateway.Entities
{
    // ReSharper disable once ClassNeverInstantiated.Global
    /// <summary>
    ///     Extending the <see cref="JwtBearerOptions" /> class
    /// </summary>
    public class JwtBearer : JwtBearerOptions
    {
        /// <summary>
        ///     A key for matching a Route to an Authentication rule set.
        /// </summary>
        public string AuthenticationProviderKey { get; set; }
    }
}