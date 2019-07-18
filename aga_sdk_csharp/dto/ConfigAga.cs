using System;

/**
 * @author Alexander Llacho
 */
namespace aga_sdk_csharp.dto
{
    public class ConfigAga
    {
        public String agaUri { get; set; }
        public String timestamp { get; set; }
        public String certificateId { get; set; }
        public String secretPassword { get; set; }
    }
}
