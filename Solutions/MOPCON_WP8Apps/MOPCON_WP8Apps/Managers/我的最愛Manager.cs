using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOPCON_WP8Apps.DataModels;
using MOPCON_WP8Apps.Helpers;

namespace MOPCON_WP8Apps.Managers
{
    public class 我的最愛Manager
    {
        public List<Session> Items = new List<Session>();
        private SessionJSONData data;
        string DataFileName = MainHelp.FilenameMySession;
        string DataFolderName = MainHelp.APIFolder;
        public string account = "";
        public string password = "";

        /// <summary>
        /// 將物件資料從檔案中讀取出來
        /// </summary>
        public async Task ReadFromFileAsync()
        {
            data = await IsolatedStorageJSON<SessionJSONData>.LoadFromFileAsync(this.DataFolderName, this.DataFileName);
            if (data != null)
            {
                Items = data.sessions;
            }
        }

        /// <summary>
        /// 將物件資料寫入到檔案中
        /// </summary>
        public async Task WriteToFileAsync()
        {
            await IsolatedStorageJSON<SessionJSONData>.SaveToFileAsync(this.DataFolderName, this.DataFileName, this.data);
        }
    }
}
