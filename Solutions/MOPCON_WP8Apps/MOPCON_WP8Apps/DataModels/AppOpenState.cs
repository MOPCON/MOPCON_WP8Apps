using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOPCON_WP8Apps.DataModels
{
    /// <summary>
    /// Hint to assist page loading to optimize work
    /// </summary>
    public enum AppOpenState
    {
        /// <summary>
        /// Internal state. object state is reset to this after initial page load
        /// </summary>
        None,

        /// <summary>
        /// App is launching
        /// </summary>
        Launching,

        /// <summary>
        /// App is being activated
        /// </summary>
        Activated,

        /// <summary>
        /// App is being deactivated
        /// </summary>
        Deactivated,

        /// <summary>
        /// App is closing
        /// </summary>
        Closing
    }
}
