using Coding4Fun.Toolkit.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MOPCON_WP8Apps.Helpers
{
    public class MainHelp
    {
        public static Regex re_Return = new Regex(@"\r\n");
        public static Regex re_n = new Regex(@"\n");
        public static Regex re_s = new Regex(@"\s");
        public static Regex re_tab = new Regex(@"\t");

        #region 連結 API
        public static string APIFolder = "MOPCON";

        public static string APIUrlSession = "http://mopcon.org/2013/api/session.php";

        public static string APIUrlNews = "http://mopcon.org/2013/api/news.php";
        #endregion

        #region Page URL

        internal const string PageNameMapLocation = @"/Pages/MapLocation.xaml";
        internal const string PageNameDetailInformation = @"/Pages/DetailInformation.xaml";
        internal const string PageNameNewsDetailInformation = @"/Pages/NewsDetailInformation.xaml";
        internal const string PageNameMainPage = @"/Pages/MainPage.xaml";
        #endregion

        #region 系統用到的檔案名稱
        public static string Filename搜尋關鍵字 = "搜尋關鍵字";

        public static string FilenameSession = "Session";
        public static string FilenameNews = "News";
        public static string FilenameMySession = "MySession";
        #endregion

        #region 系統用到的訊息字串
        public static string APIInternalError = "系統發生錯誤 Exception = null, Result = null";
        #endregion

        /// <summary>
        /// 利用 Toast 方式，顯示訊息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="Content"></param>
        public static void toastMessage(string title, string Content)
        {
            #region 顯示 Toast訊息
            ToastPrompt toast = new ToastPrompt();
            toast.Title = title;
            toast.Message = Content;
            toast.ImageSource = new BitmapImage(new Uri("/Assets/images/logo.png", UriKind.Relative));
            toast.TextOrientation = System.Windows.Controls.Orientation.Vertical;
            toast.TextWrapping = TextWrapping.Wrap;
            toast.FontSize = 25;
            //toast.Completed += toast_Completed;
            toast.Show();
            #endregion 顯示 Toast訊息
        }

    }
}
