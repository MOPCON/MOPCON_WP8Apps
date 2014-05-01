using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using MOPCON_WP8Apps;
using MOPCON_WP8Apps.DataModels;
using MOPCON_WP8Apps.Helpers;
using MOPCON_WP8Apps.Managers;
using System.Threading.Tasks;
using Microsoft.Phone.Controls.Maps;

namespace MOPCON_WP8Apps
{
    public partial class MainPage : PhoneApplicationPage
    {
        private bool IsPageActived = false;
        private DateTime SelectDate = new DateTime(2013,10,26);
        SessionManager fm = new SessionManager();
        NewsManager nm = new NewsManager();
        我的最愛Manager myFavoriter = new 我的最愛Manager();
        MainViewModel viewModelFavoriter = new MainViewModel();
        MainViewModel viewModelNews = new MainViewModel();

        // 建構函式
        public MainPage()
        {
            InitializeComponent();

            // 將清單方塊控制項的資料內容設為範例資料
            DataContext = App.ViewModel;
            this.NewsPI.DataContext = viewModelNews;
            this.我的最愛PI.DataContext = viewModelFavoriter;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {

            base.OnNavigatedTo(e);

            int numOfHistory = NavigationService.BackStack.Count();

            for (int i = 0; i < numOfHistory; i++)
            {
                NavigationService.RemoveBackEntry();
            }

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

        private async Task Init()
        {
            PerfProgBar.IsIndeterminate = true;
            await fm.ReadFromFileAsync();
            await myFavoriter.ReadFromFileAsync();
            await nm.ReadFromFileAsync();
            RefreshData();
            PerfProgBar.IsIndeterminate = false;

            if (this.NavigationContext.QueryString.ContainsKey("ID"))
            {
                string ID = this.NavigationContext.QueryString["ID"].ToString();
                this.NavigationContext.QueryString.Remove("ID");

                var ld = fm.Items.Where(x => x.id == ID).ToList();
                if (ld.Count > 0)
                {
                    SystemSettingManager<Session> ssm = new SystemSettingManager<Session>("SelectData");
                    ssm.Item = ld[0];
                    await ssm.WriteToFileAsync();

                    NavigationService.Navigate(new Uri(MainHelp.PageNameDetailInformation, UriKind.Relative));
                }
            }
        }

        // 載入 ViewModel 項目的資料
        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            InitMap();

            if (!App.ViewModel.IsDataLoaded)
            {
                //App.ViewModel.LoadData();
            }

            await Init();
        }

        private void InitMap()
        {
            map1.Center = new System.Device.Location.GeoCoordinate(22.626197, 120.285605);
            map1.ZoomLevel = 16;

            Pushpin pp = new Pushpin();
            pp.Location = new GeoCoordinate(22.626197, 120.285605);
            pp.Background = new SolidColorBrush(Colors.Green);
            pp.Content = string.Format("{0}\r\n{1}", "高雄國際會議中心ICCK", "高雄市鹽埕區中正四路274號");
            map1.Children.Add(pp);
        }

        private async void appbar_button_AddTask_Click(object sender, EventArgs e)
        {
            await UpdateData();
        }

        private async Task UpdateData()
        {
            PerfProgBar.IsIndeterminate = true;
            await fm.getSessionAsync();
            await nm.getNewsAsync();
            if (string.IsNullOrEmpty(fm.lastErrorMessage) == false)
            {
                MainHelp.toastMessage("錯誤訊息", fm.lastErrorMessage);
            }
            else
            {
                MainHelp.toastMessage("通知訊息", "資料已經更新完成");
            }
            RefreshData();
            PerfProgBar.IsIndeterminate = false;
        }

        private void RefreshData()
        {
            if (fm.Items.Count > 0)
            {
                App.ViewModel.Items.Clear();
                List<Session> ims = fm.Items.Where(x => x.start_time.ToString("yyyy/MM/dd") == SelectDate.ToString("yyyy/MM/dd")).ToList();
                foreach (var item in ims.Select(x => x.start_time).Distinct().OrderBy(x => x))
                {
                    ItemViewModel ivm = new ItemViewModel()
                    {
                        DocID = "time",
                        LineOne = item.ToString("yyyy/MM/dd HH:mm:ss"),
                    };

                    foreach (var session in ims.Where(x => x.start_time == item))
                    {
                        ItemViewModel children = new ItemViewModel()
                        {
                            DocID = session.id,
                            LineOne = session.name,
                            LineTwo = session.speaker,
                            LineThree = session.loc,
                            LineFour = "/Assets/Images/" + session.catalog.ToString("D2") + ".png"
                        };
                        ivm.Children.Add(children);
                    }
                    App.ViewModel.Items.Add(ivm);
                }
            }
            if (nm.Items.Count > 0)
            {
                viewModelNews.Items.Clear();
                foreach (var item in nm.Items.OrderByDescending(x => x.pub_time))
                {
                    ItemViewModel ivm = new ItemViewModel()
                    {
                        DocID = item.id,
                        LineOne = item.title,
                        LineTwo = item.pub_time.ToString("yyyy/MM/dd HH:mm:ss"),
                        LineThree = ""
                    };
                    viewModelNews.Items.Add(ivm);
                }
            }
            if (myFavoriter.Items.Count > 0)
            {
                viewModelFavoriter.Items.Clear();
                foreach (var item in myFavoriter.Items)
                {
                    ItemViewModel ivm = new ItemViewModel()
                    {
                        DocID = item.id,
                        LineOne = item.name,
                        LineTwo = item.speaker,
                        LineThree = item.loc
                    };
                    viewModelFavoriter.Items.Add(ivm);
                }
            }
        }

        private async void StackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var ee = sender as StackPanel;
            ItemViewModel ivm = ee.Tag as ItemViewModel;

            if (ivm.DocID == "time")
            {
                return;
            }

            var ld = fm.Items.Where(x => x.id == ivm.DocID).ToList();

            if (ld.Count == 0)
            {
                MainHelp.toastMessage("錯誤訊息", string.Format("沒有發現該筆資料:{0}", ivm.DocID));
                return;
            }

            SystemSettingManager<Session> ssm = new SystemSettingManager<Session>("SelectData");
            ssm.Item = ld[0];
            await ssm.WriteToFileAsync();

            NavigationService.Navigate(new Uri(MainHelp.PageNameDetailInformation, UriKind.Relative));
        }

        private async void NewsStackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var ee = sender as StackPanel;
            ItemViewModel ivm = ee.Tag as ItemViewModel;

            var ld = nm.Items.Where(x => x.id == ivm.DocID).ToList();

            if (ld.Count == 0)
            {
                MainHelp.toastMessage("錯誤訊息", string.Format("沒有發現該筆資料:{0}", ivm.DocID));
                return;
            }

            SystemSettingManager<News> ssm = new SystemSettingManager<News>("SelectNewsData");
            ssm.Item = ld[0];
            await ssm.WriteToFileAsync();

            NavigationService.Navigate(new Uri(MainHelp.PageNameNewsDetailInformation, UriKind.Relative));
        }

        private void Date26_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            btn26.Background = new SolidColorBrush(Colors.Blue);
            btn27.Background = new SolidColorBrush(Colors.Transparent);

            SelectDate = new DateTime(2013, 10, 26);
            RefreshData();
        }

        private void Date27_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            btn26.Background = new SolidColorBrush(Colors.Transparent);
            btn27.Background = new SolidColorBrush(Colors.Blue);

            SelectDate = new DateTime(2013, 10, 27);
            RefreshData();
        }
    }
}