using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using MOPCON_WP8Apps.DataModels;
using MOPCON_WP8Apps.Helpers;

namespace MOPCON_WP8Apps
{
    public partial class SplashScreenPage : PhoneApplicationPage
    {
        #region Field

        /// <summary>
        /// Splash page 顯示時間計時 
        /// </summary>
        private readonly BackgroundWorker _backroungWorker;

        private bool _AllowNavigation = false;

        /// <summary>
        /// Object used to prevent multiple save/load operations
        /// </summary>
        private object threadLock = new object();

        /// <summary>
        /// Background thread object
        /// </summary>
        private BackgroundWorker worker = new BackgroundWorker();

        //美食資訊Manager fm = new 美食資訊Manager();
        #endregion

        #region Properites

        public bool AllowNavigation
        {
            get
            {
                return _AllowNavigation;
            }
            set
            {
                _AllowNavigation = value;
            }
        }

        /// <summary>
        /// Flag 用來判斷頁面是否從 Tomb Stone 被喚醒
        /// </summary>
        protected bool IsPageActivated { get; private set; }

        #endregion

        public SplashScreenPage()
        {
            InitializeComponent();

            _backroungWorker = new BackgroundWorker();

            this.LayoutUpdated += SplashScreenPage_LayoutUpdated;

            this.tbVer.Text = string.Format("Version:{0}", GetVersionNumber());

            //fm.ReadFromFileAsync();
        }
        private string GetVersionNumber()
        {
            var asm = Assembly.GetExecutingAssembly();
            var parts = asm.FullName.Split(',');
            return parts[1].Split('=')[1];
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);


            var asm = Assembly.GetExecutingAssembly();
            var parts = asm.FullName.Split(',');
            var curr1 = parts[1].Split('=');
            var curr2 = curr1[1].Split('.');
            var currVersion = string.Format("V{0}.{1}", curr2[0], curr2[1]);

            if (ApplicationState.ApplicationStartup == AppOpenState.Launching)
            {
                // 剛出生，要做一下人物設定             
                RunBackgroundWorker();

                this.AllowNavigation = false;

                return;
            }

            if (ApplicationState.ApplicationStartup == AppOpenState.Activated
                && !IsPageActivated)
            {
                // 睡醒，打一打就好
                ApplicationState.ApplicationStartup = AppOpenState.None;
                return;
            }

            this.AllowNavigation = false;
            RunBackgroundWorker();

            if (ApplicationState.ApplicationStartup == AppOpenState.None)
            {
                // 從其他頁面過來
                // 理論上在此頁不應該發生
                this.AllowNavigation = true;
                return;
            }

            if (ApplicationState.ApplicationStartup == AppOpenState.Activated)
            {
                if (IsPageActivated)
                {
                    if (!ApplicationState.RetrieveAppObjects())
                    {
                        // Slight possibility that we did not complete 1st time init because of 
                        // of a app deactivate immediately after start. Protect against this and
                        // perform application 1st time startup sequence.
                        this.DeferStartup();
                    }
                }
                else
                {
                    // We are returning to the application, but we were not tomb stoned.
                    ApplicationState.ApplicationStartup = AppOpenState.None;
                    return;
                }
            }

            this.IsPageActivated = true;


            ApplicationState.ApplicationStartup = AppOpenState.None;
        }

        #region Private Method

        private void SplashScreenPage_LayoutUpdated(object sender, EventArgs e)
        {
            this.LayoutUpdated -= SplashScreenPage_LayoutUpdated;
            if (ApplicationState.ApplicationStartup == AppOpenState.Launching)
            {
                this.worker.DoWork += new DoWorkEventHandler(DeferStartupWork);
                this.DeferStartup();
                ApplicationState.ApplicationStartup = AppOpenState.None;
            }

        }

        /// <summary>
        /// Background thread work for loading 
        /// The loading of the supported conversions from the Xap as well as the 
        /// favorites from isolated storage is done on a non UI thread to allow the 
        /// fastest possible page render
        /// </summary>
        /// <param name="sender">worker thread object</param>
        /// <param name="e">Action delegate for completion</param>
        private void DeferStartupWork(object sender, DoWorkEventArgs e)
        {
            Action completed = e.Argument as Action;

            lock (threadLock)
            {
                try
                {
                    ApplicationState.AppLaunchInitialization();

                    //ScheduledActionManager.ClearExpirationAlarms();
                }
                catch { }
                AllowNavigation = true;
            }

            if (completed != null)
            {
                completed();
            }
        }

        private void DeferStartup()
        {
            this.worker.RunWorkerAsync();
        }

        private void RunBackgroundWorker()
        {
            _backroungWorker.DoWork += ((s, args) =>
            {
                Thread.Sleep(1000);
                System.Diagnostics.Debug.WriteLine("Loading");

                while (!AllowNavigation)
                {
                    Thread.Sleep(500);
                    System.Diagnostics.Debug.WriteLine("Loading ...");
                }
            });

            _backroungWorker.RunWorkerCompleted += ((s, args) =>
            {
                // Adapter.Invoke(new System.Threading.SendOrPostCallback(o =>
                // {
                System.Diagnostics.Debug.WriteLine("1");
                if (AllowNavigation)
                {
                    string pagrUri = "";

                    Dispatcher.BeginInvoke(() =>
                    {
                        string queryString = "";
                        if (this.NavigationContext.QueryString.ContainsKey("ID"))
                        {
                            string ID = this.NavigationContext.QueryString["ID"].ToString();
                            queryString = string.Format("?ID={0}", ID);
                            this.NavigationContext.QueryString.Remove("ID");
                        }

                        pagrUri = string.Format("{0}{1}", MainHelp.PageNameMainPage, queryString);
                        NavigationService.Navigate(new Uri(pagrUri, UriKind.Relative));
                    });
                }

                //  }), null);
            });

            _backroungWorker.RunWorkerAsync();
        }

        #endregion
    }
}