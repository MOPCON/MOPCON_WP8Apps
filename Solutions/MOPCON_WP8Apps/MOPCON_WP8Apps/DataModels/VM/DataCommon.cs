using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MOPCON_WP8Apps.DataModels
{
    public class DataCommon : BindableBase
    {
        #region Define

        #endregion

        #region Field
        /// <summary>
        /// 存取 Resource URI
        /// </summary>
        public static Uri BaseUri = new Uri("ms-appx:///");
        /// <summary>
        /// 存取 Local Data URI
        /// </summary>
        public static Uri LocalUri = new Uri("ms-appdata:///local/");
        #endregion

        #region Member
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        #endregion

        #region Prop

        private string m_UniqueId = "";
        /// <summary>
        /// 唯一鍵值
        /// </summary>
        public string UniqueId
        {
            get { return this.m_UniqueId; }
            set { this.SetProperty(ref this.m_UniqueId, value); }
        }

        private string m_Title = "";
        /// <summary>
        /// 主題
        /// </summary>
        public string Title
        {
            get { return this.m_Title; }
            set { this.SetProperty(ref this.m_Title, value); }
        }

        private string m_Subtitle = "";
        /// <summary>
        /// 次主題
        /// </summary>
        //[JsonIgnoreAttribute]
        public string Subtitle
        {
            get { return this.m_Subtitle; }
            set { this.SetProperty(ref this.m_Subtitle, value); }
        }

        private string m_Description = "";
        /// <summary>
        /// 詳細內容備註
        /// </summary>
        public string Description
        {
            get { return this.m_Description; }
            set { this.SetProperty(ref this.m_Description, value); }
        }

        private string m_MainCategory = "";
        /// <summary>
        /// 大分類
        /// </summary>
        public string MainCategory
        {
            get { return this.m_MainCategory; }
            set { this.SetProperty(ref this.m_MainCategory, value); }
        }

        private string m_SubCategory = "";
        /// <summary>
        /// 小分類
        /// </summary>
        public string SubCategory
        {
            get { return this.m_SubCategory; }
            set { this.SetProperty(ref this.m_SubCategory, value); }
        }

        #endregion

        #region constructor
        public DataCommon()
        {
        }

        #endregion constructor

        #region PropertyChanged Event
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region Method
        #endregion


    }
}
