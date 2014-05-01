using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOPCON_WP8Apps.JSONConverter;
using Newtonsoft.Json;

namespace MOPCON_WP8Apps.DataModels
{
    public class News
    {
        public string id { get; set; }

        public string title { get; set; }

        public string content { get; set; }

        public string publishe { get; set; }
        
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime pub_time { get; set; }

    }

    public class NewsJSONData
    {
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime last_update { get; set; }

        public List<News> news { get; set; }

        public NewsJSONData()
        {
            news = new List<News>();
        }
    }
}
