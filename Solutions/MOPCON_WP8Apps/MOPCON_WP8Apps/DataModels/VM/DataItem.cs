using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using MOPCON_WP8Apps.Helpers;

namespace MOPCON_WP8Apps.DataModels
{
    public class DataItem : DataCommon
    {
        #region 文章建立時間
        private DateTime _time = DateTime.MinValue;
        /// <summary>
        /// 文章建立時間
        /// </summary>
        public DateTime Time
        {
            get { return this._time; }
            set { this.SetProperty(ref this._time, value); }
        }

        /// <summary>
        /// 將文章建立時間，轉換成客戶指定的日期格式字串
        /// </summary>
        public string TimeString
        {
            get
            {
                string ret = this._time.ToString("yyyy.MMM.dd",
                                   new CultureInfo("en-US"));
                return ret;
            }
        }

        private string m_StringDatetime = "";
        /// <summary>
        /// 文章建立時間
        /// </summary>
        public string StringDatetime
        {
            get { return this.m_StringDatetime; }
            set { this.SetProperty(ref this.m_StringDatetime, value); }
        }

        #endregion

        #region URL
        private string _url = "";
        /// <summary>
        /// 文章連結
        /// </summary>
        public string Url
        {
            get
            {
                if (string.IsNullOrEmpty(this._url))
                {
                    return "";
                }
                else
                {
                    return this._url;
                }
            }
            set { this.SetProperty(ref this._url, value); }
        }

        private string _urlTitle = "";
        /// <summary>
        /// 文章連結
        /// </summary>
        public string UrlTitle
        {
            get
            {
                if (string.IsNullOrEmpty(this._urlTitle))
                {
                    return "";
                }
                else
                {
                    return this._urlTitle;
                }
            }
            set { this.SetProperty(ref this._urlTitle, value); }
        }


        #endregion

        #region Content
        private string m_Content = string.Empty;
        /// <summary>
        /// 文章內容
        /// </summary>
        public string Content
        {
            //get { return this._content; }

            // 底下是有使用 RichText的模式
            get
            {
                return this.m_Content;
            }
            set { this.SetProperty(ref this.m_Content, value); }
        }

        #endregion

        #region images

        #region Image setting
        /// <summary>
        /// 將圖片所指定的URL轉換成一個唯一名稱，作為儲存本機端的檔案名稱
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string LocalImageMD5(string name)
        {
            return string.Format("{0}.png", MD5Core.GetHashString(name));
        }

        /// <summary>
        /// Icon圖片要儲存在本機端的時候，檔案名稱
        /// </summary>
        public Uri IconUri
        {
            get
            {
                return new Uri(string.Format("ms-appdata:///local/icon/{0}", LocalImageMD5(this._iconPath)));
            }
        }
        #endregion

        #region Icon
        private String _iconPath = null;
        public string IconPath
        {
            set
            {
                this._icon = null;
                this._iconPath = value;
                this.OnPropertyChanged("Icon");
            }
            get { return this._iconPath; }
        }

        private ImageSource _icon = null;
        /// <summary>
        /// 一般圖片：GridView or List用的顯示項目圖片 175x175
        /// </summary>
        public ImageSource Icon
        {
            get
            {
                if (this._icon == null && string.IsNullOrEmpty(this._iconPath) == false)
                {
                    BitmapImage timage;
                    if (this._iconPath.IndexOf("http") >= 0)
                    {
                        timage = new BitmapImage(new Uri(this._iconPath));
                    }
                    else
                    {
                        timage = new BitmapImage(new Uri(DataCommon.BaseUri, this._iconPath));
                    }

                    timage.ImageOpened += async (sender, e) =>
                    {

                        try
                        {
                            if (this._iconPath.IndexOf("http") >= 0)
                            {
                                var md5String = MD5Core.GetHashString(this._iconPath);
                                string fileName = string.Format("{0}.png", md5String);
                                await this.GetLocalImageAsync(this._iconPath, "icon", fileName);
                            }
                        }
                        catch
                        {
                        }
                    };

                    this._icon = timage;

                }
                return this._icon;
            }
            set
            {
                this.SetProperty(ref this._icon, value);
            }
        }
        #endregion

        #region Image1
        private String _image1Path = null;
        public string Image1Path
        {
            set
            {
                this._image1 = null;
                this._image1Path = value;
                this.OnPropertyChanged("Image1");
            }
            get { return this._image1Path; }
        }

        private ImageSource _image1 = null;
        /// <summary>
        /// 大項目圖片：GridView or List用的顯示項目圖片，或者是Detail Page內的最右方的四個圖片之第1個   600x360
        /// </summary>
        public ImageSource Image1
        {
            get
            {
                if (this._image1 == null && string.IsNullOrEmpty(this._image1Path) == false)
                {
                    BitmapImage timage;
                    if (this._image1Path.IndexOf("http") >= 0)
                    {
                        timage = new BitmapImage(new Uri(this._image1Path));
                    }
                    else
                    {
                        timage = new BitmapImage(new Uri(DataCommon.BaseUri, this._image1Path));
                    }

                    timage.ImageOpened += async (sender, e) =>
                    {
                        try
                        {
                            if (this._image1Path.IndexOf("http") >= 0)
                            {
                                var md5String = MD5Core.GetHashString(this._image1Path);
                                string fileName = string.Format("{0}.png", md5String);
                                await this.GetLocalImageAsync(this._image1Path, "images", fileName);
                            }
                        }
                        catch
                        {
                        }
                    };

                    this._image1 = timage;

                }
                return this._image1;
            }
            set
            {
                this.SetProperty(ref this._image1, value);
            }
        }
        #endregion

        #endregion

        #region Method
        /// <summary>
        /// Copies an image from the internet (http protocol) locally to the AppData LocalFolder.  This is used by some methods 
        /// (like the SecondaryTile constructor) that do not support referencing images over http but can reference them using 
        /// the ms-appdata protocol.  
        /// </summary>
        /// <param name="internetUri">The path (URI) to the image on the internet</param>
        /// <param name="uniqueName">A unique name for the local file</param>
        /// <returns>Path to the image that has been copied locally</returns>
        private async Task GetLocalImageAsync(string internetUri, string folderName, string uniqueName)
        {
            string fullFilename = "";
            if (string.IsNullOrEmpty(internetUri))
            {
                return;
            }

            using (var response = await HttpWebRequest.CreateHttp(internetUri).GetResponseAsync())
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        if (isolatedStorageFile.DirectoryExists(folderName) == false)
                        {
                            isolatedStorageFile.CreateDirectory(folderName);
                            fullFilename = string.Format("{0}/{1}", folderName, uniqueName);
                        }
                        else
                        {
                            fullFilename = string.Format("{0}", uniqueName);
                        }

                        if (isolatedStorageFile.FileExists(fullFilename))
                        {
                            isolatedStorageFile.DeleteFile(fullFilename);
                        }

                        using (IsolatedStorageFileStream fileStream = new IsolatedStorageFileStream(fullFilename, FileMode.Create, isolatedStorageFile))
                        {
                            using (BinaryWriter writer = new BinaryWriter(fileStream))
                            {
                                long length = stream.Length;
                                byte[] buffer = new byte[32];
                                int readCount = 0;
                                using (BinaryReader reader = new BinaryReader(stream))
                                {
                                    // read file in chunks in order to reduce memory consumption and increase performance
                                    while (readCount < length)
                                    {
                                        int actual = reader.Read(buffer, 0, buffer.Length);
                                        readCount += actual;
                                        writer.Write(buffer, 0, actual);
                                    }
                                    writer.Flush();
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            if (Icon != null)
            {
                (Icon as BitmapImage).UriSource = null;
                Icon = null;
            }
            if (Image1 != null)
            {
                (Image1 as BitmapImage).UriSource = null;
                Image1 = null;
            }
        }

        #endregion
    }
}
