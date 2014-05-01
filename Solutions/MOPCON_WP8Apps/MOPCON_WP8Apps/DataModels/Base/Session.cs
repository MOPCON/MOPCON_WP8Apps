using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOPCON_WP8Apps.JSONConverter;
using Newtonsoft.Json;

namespace MOPCON_WP8Apps.DataModels
{
    public class Session
    {
        public string id { get; set; }

        public string name { get; set; }

        public string content { get; set; }

        public string speaker { get; set; }

        public string speaker_bio { get; set; }

        public string keyword { get; set; }

        public string loc { get; set; }

        public int catalog { get; set; }

        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime start_time { get; set; }

        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime end_time { get; set; }
    }

    public class SessionJSONData
    {
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime last_update { get; set; }

        public List<Session> sessions { get; set; }

        public SessionJSONData()
        {
            sessions = new List<Session>();
        }
    }
}
