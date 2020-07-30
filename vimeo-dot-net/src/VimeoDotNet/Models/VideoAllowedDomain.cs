using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace VimeoDotNet.Models
{
    public class VideoAllowedDomain
    {

        [PublicAPI]
        [JsonProperty(PropertyName = "domain")]
        public string Domain { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "allow_hd")]
        public bool AllowHd { get; set; }

        [PublicAPI]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

    }
}
