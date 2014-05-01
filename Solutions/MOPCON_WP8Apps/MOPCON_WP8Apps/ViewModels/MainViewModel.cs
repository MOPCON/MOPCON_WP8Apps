using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;


namespace MOPCON_WP8Apps
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
        }

        /// <summary>
        /// ItemViewModel 物件的集合。
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }

        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// 範例 ViewModel 屬性; 這個屬性是用在檢視中，以使用繫結來顯示其值
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// 建立並加入一些 ItemViewModel 物件到 Items 集合。
        /// </summary>
        public void LoadData()
        {
            // 範例資料; 以真實資料取代
            this.Items.Add(new ItemViewModel() {
                LineOne = "小貓咪飼養介紹",
                LineTwo = "愛貓人士",
                LineThree = "403 會議室"
            });
            this.Items.Add(new ItemViewModel()
            {
                LineOne = "小狗狗飼養介紹",
                LineTwo = "愛狗人士",
                LineThree = "404 會議室"
            });
            this.Items.Add(new ItemViewModel()
            {
                LineOne = "Open is the new norm.",
                LineTwo = "程世嘉 Sega Cheng",
                LineThree = "404 會議室"
            });
            this.Items.Add(new ItemViewModel()
            {
                LineOne = "Mikimoto假面的告白",
                LineTwo = "Mikimoto",
                LineThree = "企業號儲藏室"
            });
            this.Items.Add(new ItemViewModel()
            {
                LineOne = "Mikimotoo與娜美",
                LineTwo = "Mikimoto",
                LineThree = "企業號艦橋"
            });
            this.Items.Add(new ItemViewModel()
            {
                LineOne = "Mikimotoo與涼宮春日的浪漫夏天",
                LineTwo = "Mikimoto",
                LineThree = "企業號輪機室"
            });
            this.IsDataLoaded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}