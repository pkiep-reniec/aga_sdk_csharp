using Newtonsoft.Json;
using System;

/**
 * @author Alexander Llacho
 */
namespace aga_sdk_csharp.dto
{
    public class Config
    {
        [JsonProperty("client_id")]
        public String clientId { get; set; }
        [JsonProperty("client_secret")]
        public String clientSecret { get; set; }
        [JsonProperty("security_uri")]
        public String securityUri { get; set; }
        [JsonProperty("eaddress_service_uri")]
        public String eaddressServiceUri { get; set; }
        [JsonProperty("doc_type")]
        public String docType { get; set; }
        [JsonProperty("doc")]
        public String doc { get; set; }
        [JsonProperty("name")]
        public String name { get; set; }
        [JsonProperty("app_name")]
        public String appName { get; set; }
    }
}
