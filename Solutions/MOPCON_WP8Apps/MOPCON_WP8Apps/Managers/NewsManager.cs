using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using MOPCON_WP8Apps.DataModels;
using System.Net.Http;
using MOPCON_WP8Apps.Helpers;
using Newtonsoft.Json;

namespace MOPCON_WP8Apps.Managers
{
    public class NewsManager
    {
        public List<News> Items = new List<News>();
        private NewsJSONData data;
        HttpClient webClient = new HttpClient();
        string DataFileName = MainHelp.FilenameNews;
        string DataFolderName = MainHelp.APIFolder;

        public string lastErrorMessage = "";

        /// <summary>
        /// 將物件資料從檔案中讀取出來
        /// </summary>
        public async Task ReadFromFileAsync()
        {
            data = await IsolatedStorageJSON<NewsJSONData>.LoadFromFileAsync(this.DataFolderName, this.DataFileName);
            if (data != null && data.news.Any())
            {
                Items = data.news;
            }
            else
            {
                await getNewsAsync();
            }
        }

        /// <summary>
        /// 將物件資料寫入到檔案中
        /// </summary>
        public async Task WriteToFileAsync()
        {
            await IsolatedStorageJSON<NewsJSONData>.SaveToFileAsync(this.DataFolderName, this.DataFileName, this.data);
        }

        public async Task getNewsAsync()
        {
            ManagerResult mr = new ManagerResult();

            webClient = new HttpClient();

            MultipartFormDataContent form = new MultipartFormDataContent();

            UriBuilder ub = new UriBuilder(MainHelp.APIUrlNews);
            try
            {
                HttpResponseMessage response = await webClient.GetAsync(ub.Uri);
                lastErrorMessage = "";
                if (response != null)
                {
                    String strResult = "";
                    strResult = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        APIErrorResponse apiER = JsonConvert.DeserializeObject<APIErrorResponse>(strResult);
                        mr.Success = false;
                        mr.WebResponse = apiER;
                        mr.Message = string.Format("錯誤代碼:{0}, 錯誤訊息:存取網路發生錯誤", response.StatusCode);
                        lastErrorMessage = mr.Message;
                    }
                    else if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var readData = JsonConvert.DeserializeObject<NewsJSONData>(strResult);
                        if (data != null)
                        {
                                data = readData;
                                Items = data.news;
                        }
                        await this.WriteToFileAsync();
                        mr.Success = true;
                        mr.Result = null;
                    }
                }
                else
                {
                    mr.Exception = new Exception(MainHelp.APIInternalError);
                    mr.Message = string.Format("系統內部發生錯誤:{0}", MainHelp.APIInternalError);
                    lastErrorMessage = mr.Message;
                }
            }
            catch (Exception ex)
            {
                mr.Exception = ex;
                mr.Message = string.Format("系統發生異常錯誤:{0}", ex.Message.ToString());
                lastErrorMessage = mr.Message;
            }
        }
    }
}
