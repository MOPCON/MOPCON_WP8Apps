using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOPCON_WP8Apps.DataModels
{
    public class SystemSetting : DataCommon
    {
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

    }
}
