using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KapGel.Models
{
    using Newtonsoft.Json;

    public class CaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}