using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOPCON_WP8Apps.DataModels;
using MOPCON_WP8Apps.Helpers;

namespace MOPCON_WP8Apps.Managers
{
    public class SearchKeywordsManager
    {
        public SearchKeywords Item = new SearchKeywords();
        string DataFileName = MainHelp.Filename搜尋關鍵字;
        string DataFolderName = MainHelp.APIFolder;

        /// <summary>
        /// 將物件資料從檔案中讀取出來
        /// </summary>
        public async Task ReadFromFileAsync()
        {
            Item = await IsolatedStorageJSON<SearchKeywords>.LoadFromFileAsync(this.DataFolderName, this.DataFileName);
        }

        /// <summary>
        /// 將物件資料寫入到檔案中
        /// </summary>
        public async Task WriteToFileAsync()
        {
            await IsolatedStorageJSON<SearchKeywords>.SaveToFileAsync(this.DataFolderName, this.DataFileName, this.Item);
        }
    }
}
