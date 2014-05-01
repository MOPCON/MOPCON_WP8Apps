using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MOPCON_WP8Apps.DataModels;

namespace MOPCON_WP8Apps
{
    public class ItemViewModel : DataCommon
    {

        #region DocID
        private string m_DocID = string.Empty;
        /// <summary>
        /// 文章ID
        /// </summary>
        public string DocID
        {
            //get { return this._content; }

            // 底下是有使用 RichText的模式
            get
            {
                return this.m_DocID;
            }
            set { this.SetProperty(ref this.m_DocID, value); }
        }
        #endregion

        #region LineOne
        private string m_LineOne = string.Empty;
        /// <summary>
        /// 範例 ViewModel 屬性; 這個屬性是用在檢視中，以使用繫結來顯示其值。
        /// </summary>
        public string LineOne
        {
            //get { return this._content; }

            // 底下是有使用 RichText的模式
            get
            {
                return this.m_LineOne;
            }
            set { this.SetProperty(ref this.m_LineOne, value); }
        }
        #endregion

        #region LineTwo
        private string m_LineTwo = string.Empty;
        /// <summary>
        /// 範例 ViewModel 屬性; 這個屬性是用在檢視中，以使用繫結來顯示其值。
        /// </summary>
        public string LineTwo
        {
            //get { return this._content; }

            // 底下是有使用 RichText的模式
            get
            {
                return this.m_LineTwo;
            }
            set { this.SetProperty(ref this.m_LineTwo, value); }
        }
        #endregion

        #region LineThree
        private string m_LineThree = string.Empty;
        /// <summary>
        /// 範例 ViewModel 屬性; 這個屬性是用在檢視中，以使用繫結來顯示其值。
        /// </summary>
        public string LineThree
        {
            //get { return this._content; }

            // 底下是有使用 RichText的模式
            get
            {
                return this.m_LineThree;
            }
            set { this.SetProperty(ref this.m_LineThree, value); }
        }
        #endregion

        #region LineFour
        private string m_LineFour = string.Empty;
        /// <summary>
        /// 範例 ViewModel 屬性; 這個屬性是用在檢視中，以使用繫結來顯示其值。
        /// </summary>
        public string LineFour
        {
            //get { return this._content; }

            // 底下是有使用 RichText的模式
            get
            {
                return this.m_LineFour;
            }
            set { this.SetProperty(ref this.m_LineFour, value); }
        }
        #endregion

        #region LineFour
        private List<ItemViewModel> m_Children = new List<ItemViewModel>();
        /// <summary>
        /// 範例 ViewModel 屬性; 這個屬性是用在檢視中，以使用繫結來顯示其值。
        /// </summary>
        public List<ItemViewModel> Children
        {
            //get { return this._content; }

            // 底下是有使用 RichText的模式
            get
            {
                return this.m_Children;
            }
            set { this.SetProperty(ref this.m_Children, value); }
        }
        #endregion
    }
}