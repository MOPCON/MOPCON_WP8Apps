using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOPCON_WP8Apps.DataModels
{
    public class TaskPageState : DataCommon
    {
        private string _Address = "";
        #region Define

        #endregion

        #region Member

        #endregion

        #region Prop
        /// <summary>
        /// 群組通訊清單ID
        /// </summary>
        public string Address
        {
            get { return _Address; }
            set
            {
                if (_Address != value)
                {
                    _Address = value;
                    NotifyPropertyChanged("Address");
                }
            }
        }

        #endregion

        #region constructor & destructor
        public TaskPageState()
        {
        }

        ~TaskPageState()
        {
        }
        #endregion

        #region Method
        #endregion
    }
}
