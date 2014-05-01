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
    public partial class DetailInformation : PhoneApplicationPage
    {
        SystemSettingManager<Session> ssm = new SystemSettingManager<Session>("SelectData");
        SessionManager fm = new SessionManager();
        我的最愛Manager myFavoriter = new 我的最愛Manager();
        private string SecondaryTileUriSource = "ID={0}";

        #region Fields
        /// <summary>
        /// 判斷頁面 Life Cycle 狀態的 flag
        /// </summary>
        private bool IsPageActived = false;
        #endregion

        #region Member
        ApplicationBarIconButton _appbar_button_AddFavoriter;
        ApplicationBarIconButton _appbar_button_RemoveFavoriter;
        ApplicationBarIconButton _appbar_button_Pin;

        #endregion

        public DetailInformation()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(Page_Loaded);
        }
        
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await ssm.ReadFromFileAsync();
            this.LayoutRoot.DataContext = ssm.Item;

            await myFavoriter.ReadFromFileAsync();


            await fm.ReadFromFileAsync();


            InitBarRefresh();
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

        private void map1_ViewChangeEnd(object sender, Microsoft.Phone.Controls.Maps.MapEventArgs e)
        {
        }

        private void InitBarRefresh()
        {
            _appbar_button_AddFavoriter = new ApplicationBarIconButton();
            _appbar_button_AddFavoriter.Text = "儲存最愛";
            _appbar_button_AddFavoriter.IconUri = new Uri("/Assets/Icons/AddFav.png", UriKind.Relative);
            _appbar_button_AddFavoriter.Click += new EventHandler(appBtnAdd_Click);

            _appbar_button_RemoveFavoriter = new ApplicationBarIconButton();
            _appbar_button_RemoveFavoriter.Text = "移除最愛";
            _appbar_button_RemoveFavoriter.IconUri = new Uri("/Assets/Icons/DelFav.png", UriKind.Relative);
            _appbar_button_RemoveFavoriter.Click += new EventHandler(appBtnAdd_Click);

            _appbar_button_Pin = new ApplicationBarIconButton();
            _appbar_button_Pin.Text = "釘選";
            _appbar_button_Pin.IconUri = new Uri("/Assets/Icons/Pin.png", UriKind.Relative);
            _appbar_button_Pin.Click += new EventHandler(appBtnPin_Click);

            AppBarRefresh();
        }

        private void AppBarRefresh()
        {
            ApplicationBar.Buttons.Clear();
            var sr = myFavoriter.Items.Where(x => x.id == ssm.Item.id).ToList();
            if (sr.Count == 0)
            {
                ApplicationBar.Buttons.Add(_appbar_button_AddFavoriter);
            }
            else
            {
                ApplicationBar.Buttons.Add(_appbar_button_RemoveFavoriter);
            }
            ApplicationBar.Buttons.Add(_appbar_button_Pin);
        }

        private async void appBtnAdd_Click(object sender, EventArgs e)
        {
            var sr = myFavoriter.Items.Where(x => x.id == ssm.Item.id).ToList();
            if (sr.Count == 0)
            {
                myFavoriter.Items.Add(ssm.Item);
                MainHelp.toastMessage("通知", "該筆資料已經加入到我的最愛中");
            }
            else
            {
                myFavoriter.Items.Remove(sr[0]);
                MainHelp.toastMessage("通知", "該筆資料已經從我的最愛中移除");
            }
            await myFavoriter.WriteToFileAsync();

            AppBarRefresh();
        }

        private void appBtnPin_Click(object sender, EventArgs e)
        {
            updateCreateSecondaryTile("create");
        }

        private void updateCreateSecondaryTile(string action)
        {
            var sTileSource = SecondaryTileURL();
            ShellTile tile = this.FindTile(sTileSource);

            if (tile == null)
            {
                if (action == "create")
                {
                    StandardTileData tileData = this.GetSecondaryTileData();
                    string tileUri = string.Concat("/Pages/SplashScreenPage.xaml?", sTileSource);
                    try
                    {
                        ShellTile.Create(new Uri(tileUri, UriKind.Relative), tileData);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);

                    }
                }
                else if (action == "update")
                {
                    //deleteTileImages();
                }
            }
            else
            {
                if (action == "update")
                {
                    StandardTileData tileData = this.GetSecondaryTileData();
                    tile.Update(tileData);
                }
                else if (action == "delete")
                {
                    tile.Delete();
                }
            }
        }

        private string SecondaryTileURL()
        {
            return string.Format(SecondaryTileUriSource, ssm.Item.id);
        }

        private ShellTile FindTile(string partOfUri)
        {
            ShellTile shellTile = ShellTile.ActiveTiles.FirstOrDefault(
                tile => tile.NavigationUri.ToString().Contains(partOfUri));

            return shellTile;
        }

        private StandardTileData GetSecondaryTileData()
        {
            StandardTileData tileData = new StandardTileData
            {
                Title = ssm.Item.name,
                BackTitle = ssm.Item.loc,
                BackContent = ssm.Item.name,
                BackgroundImage = new Uri("/Assets/Images/logo.png", UriKind.RelativeOrAbsolute),
            };

            return tileData;
        }

    }
}