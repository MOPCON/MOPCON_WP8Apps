using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VulcanCustomMap
{
    public enum GoogleTileSourceType
    {
        Street,
        Hybrid,
        Satellite,
        Physical,
        PhysicalHybrid,
        StreetOverlay,
        WaterOverlay
    }

    public class GoogleTileSource : Microsoft.Phone.Controls.Maps.TileSource
    {
        // 使用 nokia
#if (GOOGLEMAPSOURCE)
        static string usingmMapSource = "Google";
        static string GoogleTileSourceBase = "http://mt{0}.google.com/vt/lyrs=m@107&hl=en&x={1}&y={2}&z={3}&s=Ga";
#else
        static string usingmMapSource = "Nokia";
        static string GoogleTileSourceBase = "http://{0}.maptile.lbs.ovi.com/maptiler/v2/maptile/newest/normal.day/{1}/{2}/{3}/256/png8?lg=CHI&token={4}&appId={5}";
#endif


        static int _tile_count = 0;

        static string token = "eJCNTz60hpN01hKHFdoO";
        static string appId = "1cZClBtrb3Kej6YA2KIg";

        //public NokiaMapsRoadTileSource()  
        //: base("http://{0}.maptile.lbs.ovi.com/maptiler/v2/maptile/newest/normal.day/{1}/{2}/{3}/256/png8?lg=CHI&token={4}&appId={5}") 

        // 使用 google
        //public GoogleTileSource()
        //    : base("http://mt{0}.google.com/vt/lyrs=m@107&hl=en&x={1}&y={2}&z={3}&s=Ga")


        public GoogleTileSource()
            : base(GoogleTileSource.GoogleTileSourceBase)
        {
            //UriFormat = @"http://mt{0}.google.com/vt/lyrs={1}&z={2}&x={3}&y={4}";
            TileSourceType = GoogleTileSourceType.Street;
        }

        // 使用 nokia
        //public override System.Uri GetUri(int x, int y, int zoomLevel)
        //{
        //    return new System.Uri(
        //        string.Format(UriFormat,(_tile_count++ % 4) + 1,zoomLevel,x,y,token,appId));
        //}  

        // 使用 google
        //public override Uri GetUri(int x, int y, int zoomLevel)
        //{
        //    int num = ((x & 1) << 1) | (y & 1);
        //    return new Uri(string.Format(this.UriFormat, num, x, y, zoomLevel));
        //}

        public override Uri GetUri(int x, int y, int zoomLevel)
        {
            if (GoogleTileSource.usingmMapSource != "Google")
            {
                return new System.Uri(string.Format(UriFormat, (_tile_count++ % 4) + 1, zoomLevel, x, y, token, appId));
            }
            else
            {
                int num = ((x & 1) << 1) | (y & 1);
                return new Uri(string.Format(this.UriFormat, num, x, y, zoomLevel));
            }
        }

        //public override Uri GetUri(int x, int y, int zoomLevel)
        //{
        //    {
        //        if (zoomLevel > 0)
        //        {
        //            var url = string.Format(UriFormat, Server, _mapMode, zoomLevel, x, y);
        //            return new Uri(url);
        //        }
        //    }
        //    return null;
        //}

        private int _servernr;
        private char _mapMode;

        private int Server
        {
            get
            {
                return _servernr = (_servernr + 1) % 4;
            }
        }

        private GoogleTileSourceType _tileSourceType;
        public GoogleTileSourceType TileSourceType
        {
            get { return _tileSourceType; }
            set
            {
                _tileSourceType = value;
                _mapMode = TypeToMapMode(value);
            }
        }

        private static char TypeToMapMode(GoogleTileSourceType tileSourceType)
        {
            switch (tileSourceType)
            {
                case GoogleTileSourceType.Hybrid:
                    return 'y';
                case GoogleTileSourceType.Satellite:
                    return 's';
                case GoogleTileSourceType.Street:
                    return 'm';
                case GoogleTileSourceType.Physical:
                    return 't';
                case GoogleTileSourceType.PhysicalHybrid:
                    return 'p';
                case GoogleTileSourceType.StreetOverlay:
                    return 'h';
                case GoogleTileSourceType.WaterOverlay:
                    return 'r';
            } return ' ';
        }
    }
}
