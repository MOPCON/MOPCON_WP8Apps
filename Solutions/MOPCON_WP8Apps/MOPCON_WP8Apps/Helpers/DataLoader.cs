using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOPCON_WP8Apps.Helpers
{
    public class DataLoader
    {
        #region field
        private static DataLoader instance = new DataLoader();
        public static DataLoader Instance { get { return instance; } }

        public static string filename = "";
        #endregion
    }
}
