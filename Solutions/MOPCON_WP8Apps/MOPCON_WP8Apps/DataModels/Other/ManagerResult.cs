using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOPCON_WP8Apps.DataModels
{
    /// <summary>
    /// Manager 通用的回傳結果
    /// </summary>
    public class ManagerResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object WebResponse { get; set; }
        public object Result { get; set; }
        public object Exception { get; set; }

        public ManagerResult()
        {
            reset();
        }

        public void reset()
        {
            this.Success = false;
            this.Message = "";
            this.WebResponse = null;
            this.Result = null;
            this.Exception = null;
        }
    }
}
