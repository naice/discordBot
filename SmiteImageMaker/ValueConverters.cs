using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace SmiteImageMaker
{
    static class Helper
    {
        /// <summary>
         /// Load a resource WPF-BitmapImage (png, bmp, ...) from embedded resource defined as 'Resource' not as 'Embedded resource'.
         /// </summary>
         /// <param name="pathInApplication">Path without starting slash</param>
         /// <param name="assembly">Usually 'Assembly.GetExecutingAssembly()'. If not mentionned, I will use the calling assembly</param>
         /// <returns></returns>
        public static BitmapImage LoadBitmapFromResource(string pathInApplication, Assembly assembly = null)
        {
            if (assembly == null)
            {
                assembly = Assembly.GetCallingAssembly();
            }

            if (pathInApplication[0] == '/')
            {
                pathInApplication = pathInApplication.Substring(1);
            }
            return new BitmapImage(new Uri(@"pack://application:,,,/" + assembly.GetName().Name + ";component/" + pathInApplication, UriKind.Absolute));
        }
    }
    public class PentaKillValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int v)
            {
                switch (v)
                {
                    case 2: return Helper.LoadBitmapFromResource("Assets/Double_Kill.png");
                    case 3: return Helper.LoadBitmapFromResource("Assets/Triple_Kill.png");
                    case 4: return Helper.LoadBitmapFromResource("Assets/Quadra_Kill.png");
                    case 5: return Helper.LoadBitmapFromResource("Assets/Penta_Kill.png");
                    default: return null;
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MatchMinutesValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int v)
            {
                var lastMatchDuration = TimeSpan.FromMinutes(v);
                return string.Format("{0:00}:{1:00}", lastMatchDuration.Hours, lastMatchDuration.Minutes);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class GoldPerMinuteValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SmiteAPI.Model.MatchHistory model)
            {
                if (model.Minutes == 0) return 0;
                return model.Gold / model.Minutes;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MatchToItemsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SmiteAPI.Model.SmiteImageMaker<SmiteAPI.Model.MatchHistory> model)
            {
                var arr = new List<object>();
                AddIfNotNull(arr, model.ItemCache.FirstOrDefault((A) => A.ItemId == model.Data.ItemId1));
                AddIfNotNull(arr, model.ItemCache.FirstOrDefault((A) => A.ItemId == model.Data.ItemId2));
                AddIfNotNull(arr, model.ItemCache.FirstOrDefault((A) => A.ItemId == model.Data.ItemId3));
                AddIfNotNull(arr, model.ItemCache.FirstOrDefault((A) => A.ItemId == model.Data.ItemId4));
                AddIfNotNull(arr, model.ItemCache.FirstOrDefault((A) => A.ItemId == model.Data.ItemId5));
                AddIfNotNull(arr, model.ItemCache.FirstOrDefault((A) => A.ItemId == model.Data.ItemId6));
                return arr;
            }

            return value;
        }

        private static void AddIfNotNull(List<object> list, object item)
        {
            if (item != null)
                list.Add(item);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MatchToRelicsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SmiteAPI.Model.SmiteImageMaker<SmiteAPI.Model.MatchHistory> model)
            {
                var arr = new List<object>();
                AddIfNotNull(arr, model.ItemCache.FirstOrDefault((A) => A.ItemId == model.Data.ActiveId1));
                AddIfNotNull(arr, model.ItemCache.FirstOrDefault((A) => A.ItemId == model.Data.ActiveId2));
                return arr;
            }

            return value;
        }

        private static void AddIfNotNull(List<object> list, object item)
        {
            if (item != null)
                list.Add(item);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MatchToGodPropertyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SmiteAPI.Model.SmiteImageMaker<SmiteAPI.Model.MatchHistory> model)
            {
                var god = model.GodCache.FirstOrDefault((A) => A.id == model.Data.GodId);
                if (god != null)
                {
                    if (parameter is string propertyName)
                    {
                        return god.GetType().GetRuntimeProperty(propertyName).GetValue(god);
                    }

                    return god;
                }

            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class IfNullThenParameter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ?? parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class PercentageValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length > 1)
            {
                if (values.All((v)=> v is int))
                {
                    var prozentwert1 = (int)values[0];
                    var grundwert = values.Select((v)=>(int)v).Sum();
                    if (parameter != null)
                        grundwert -= prozentwert1; 
                    return Math.Round(((double)prozentwert1 / (double)grundwert)*100d, 2).ToString();
                }
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
