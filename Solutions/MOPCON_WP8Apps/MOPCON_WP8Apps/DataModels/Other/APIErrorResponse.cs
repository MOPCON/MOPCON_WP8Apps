using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOPCON_WP8Apps.DataModels
{
    public class APIErrorResponse
    {
        public int error_id { get; set; }
        public string error_text { get; set; }
        public string success_text { get; set; }
        public string debug { get; set; }
    }
}
