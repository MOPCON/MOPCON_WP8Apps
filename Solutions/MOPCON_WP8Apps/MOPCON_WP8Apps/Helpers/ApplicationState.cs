using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOPCON_WP8Apps.DataModels;

namespace MOPCON_WP8Apps.Helpers
{
    /// <summary>
    /// 應用程式狀態
    /// 1. 全域變數 2.全域常數 3.各頁面狀態
    /// 4. Life Cycle 資料管理
    /// </summary>
    public class ApplicationState
    {
        /// <summary>
        /// 軟體安裝後是否第一次被執行
        /// </summary>
        internal static bool ApplicationFirstLaunch { get; set; }

        /// <summary>
        /// Gets or sets the application startup state.
        /// This is a hint for the page loading to use to optimize the work done.
        /// </summary>
        public static AppOpenState ApplicationStartup { get; set; }

        #region Key of Page State Properites

        /// <summary>
        /// Key for the Application first Launch flag
        /// </summary>
        internal const string ApplicationFirstLaunchKey = "ApplicationFirstLaunch";

        /// <summary>
        /// Key for the Main Page state storage
        /// </summary>
        internal const string TaskPageState = "TaskPageState";
        internal const string LatestPageState = "LatestPageState";

        #endregion

        #region Page State

        internal static TaskPageState TaskPageInformation { get; set; }

        /// <summary>
        /// Latest Page State Information
        /// </summary>
        internal static object LatestPageInformation { get; set; }

        #endregion

        #region 系統狀態 Life Cycle 資料管理
        /// <summary>
        /// Initial application initialization. Logic that is deferred related to 
        /// loading the supported conversions from a file in the xap to speed up
        /// 1st page render
        /// </summary>
        internal static void AppLaunchInitialization()
        {
            if (ApplicationState.TaskPageInformation == null)
            {
                ApplicationState.TaskPageInformation = new TaskPageState();
            }


            // UserData = AppSetting.Exist(UserDataKey) ? (UserData)AppSetting.Load(UserDataKey) : new UserData();

            //TrackerRecord = RouteModel.LoadFromFile();

        }

        /// <summary>
        /// Apps the activated initialization.
        /// </summary>
        internal static void AppActivatedInitialization()
        {
            // UserData = AppSetting.Exist(UserDataKey) ? (UserData)AppSetting.Load(UserDataKey) : new UserData();

            //TrackerRecord = RouteModel.LoadFromFile();
        }


        /// <summary>
        /// Adds the app state objects to the system object store
        /// </summary>
        /// <param name="categoryPage">If true, save the category page state</param>
        internal static void AddAppObjects()
        {
            AddObject(TaskPageState, TaskPageInformation);

            if (LatestPageInformation != null)
            {
                AddObject(LatestPageState, LatestPageInformation);
            }
        }


        /// <summary>
        /// Retrieves the app state objects from the system object store
        /// </summary>
        /// <param name="categoryPage">If true, retrieve the category page state</param>
        /// <returns>True if all objects are non null, false otherwise</returns>
        internal static bool RetrieveAppObjects()
        {
            bool allObjectsNonNull = false;

            TaskPageInformation = RetrieveObject<TaskPageState>(TaskPageState);

            LatestPageInformation = RetrieveObject<object>(LatestPageState);

            if (TaskPageInformation != null
                && TaskPageInformation != null)
            {
                AppActivatedInitialization();
                allObjectsNonNull = true;
            }
            return allObjectsNonNull;
        }


        /// <summary>
        /// Retrieves the specified object from the system object store
        /// </summary>
        /// <typeparam name="T">Data to retrieve</typeparam>
        /// <param name="key">The object key</param>
        /// <returns>object default, or deserialized object</returns>
        private static T RetrieveObject<T>(string key)
        {
            T data = default(T);
            if (PhoneApplicationService.Current.State.ContainsKey(key))
            {
                data = (T)PhoneApplicationService.Current.State[key];
            }
            return data;
        }

        /// <summary>
        /// Adds the specified data object to the system object store
        /// </summary>
        /// <param name="key">The object key.</param>
        /// <param name="data">The data to store.</param>
        private static void AddObject(string key, object data)
        {
            if (PhoneApplicationService.Current.State.ContainsKey(key))
            {
                PhoneApplicationService.Current.State.Remove(key);
            }
            PhoneApplicationService.Current.State.Add(key, data);
        }

        /// <summary>
        /// 獲得現在是否有任何網路連線可以使用
        /// </summary>
        /// <returns>網路能夠使用回傳 true，無任何可使用網路連線則回傳 false</returns>
        internal static bool GetIsNetworkAvailable()
        {
            // Get current network availalability and store the 
            // initial value of the online variable
            return DeviceNetworkInformation.IsNetworkAvailable;
        }
        #endregion 系統狀態的儲存與取回

    }
}
