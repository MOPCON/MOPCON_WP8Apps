using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOPCON_WP8Apps.DataModels;
using MOPCON_WP8Apps.Helpers;

namespace MOPCON_WP8Apps.Managers
{
    public class SystemSettingManager<T>
    {
        public T Item = (T)typeof(T).GetConstructor(new Type[] { }).Invoke(new object[] { });
        string DataFileName = MainHelp.FilenameSession;
        string DataFolderName = "Setting";

        public SystemSettingManager(string filename)
        {
            this.DataFileName = filename;
        }

        public SystemSettingManager()
        {
            this.DataFileName = "temp";
        }

        /// <summary>
        /// 將物件資料從檔案中讀取出來
        /// </summary>
        public async Task ReadFromFileAsync()
        {
            Item = await IsolatedStorageJSON<T>.LoadFromFileAsync(this.DataFolderName, this.DataFileName);
            //if (it != null)
            //{
            //    Items = it;
            //}
            //string data = await IsolatedStorageJSON(this.DataFolderName, this.DataFileName);
            //this.ItemInvoiceEventCollection = JsonConvert.DeserializeObject<InvoiceEventCollection>(data);
        }

        /// <summary>
        /// 將物件資料寫入到檔案中
        /// </summary>
        public async Task WriteToFileAsync()
        {
            await IsolatedStorageJSON<T>.SaveToFileAsync(this.DataFolderName, this.DataFileName, this.Item);
        }
    }
}
