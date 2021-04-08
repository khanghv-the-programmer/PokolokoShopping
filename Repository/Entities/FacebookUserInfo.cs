using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Entities
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class FacebookUserInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("birthday")]
        public string Birthday { get; set; }

        [JsonProperty("hometown")]
        public FacebookUserHometown Hometown { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class FacebookUserHometown
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
