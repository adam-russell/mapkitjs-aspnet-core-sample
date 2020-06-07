using System;
namespace MapKitJSSample.Models
{
    public class MapKitSettings
    {
        public string PrivateKey { get; set; }
        public string KeyIdentifier { get; set; }
        public string TeamID { get; set; }
        public int TokenExpirationMinutes { get; set; }
        public string Origin { get; set; }
    }
}
