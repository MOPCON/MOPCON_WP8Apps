using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using MOPCON_WP8Apps.DataModels;
using MOPCON_WP8Apps.Helpers;
using MOPCON_WP8Apps.Managers;

namespace MOPCON_WP8Apps
{
    public partial class NewsDetailInformation : PhoneApplicationPage
    {
        SystemSettingManager<News> ssm = new SystemSettingManager<News>("SelectNewsData");
        #region Fields
        /// <summary>
        /// 判斷頁面 Life Cycle 狀態的 flag
        /// </summary>
        private bool IsPageActived = false;
        #endregion

        #region Member
        #endregion

        public NewsDetailInformation()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(Page_Loaded);
        }
        
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await ssm.ReadFromFileAsync();
            this.LayoutRoot.DataContext = ssm.Item;
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {

            base.OnNavigatedTo(e);

            if (ApplicationState.ApplicationStartup == AppOpenState.Launching)
            {
                ApplicationState.AppLaunchInitialization();
            }

            if (ApplicationState.ApplicationStartup == AppOpenState.Activated
                && IsPageActived)
            {
                if (ApplicationState.RetrieveAppObjects())
                {
                    RefreshFromAppState();
                }
                else
                {
                    ApplicationState.AppActivatedInitialization();

                }
            }

            ApplicationState.ApplicationStartup = AppOpenState.None;

            IsPageActived = true;

        }

        private void RefreshFromAppState()
        {
        }
    }
}