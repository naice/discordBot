using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteImageMaker
{
    static class DesignTimeHelper
    {
        public static void CreateMe<T>(object me, string json)
        {
            CopyProperties(Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json), me);
        }
        public static void CopyProperties(object from, object to)
        {
            var destType = to.GetType();
            var origType = from.GetType();
            var origTypeProperties = origType.GetProperties();

            destType.GetProperties().ToList()
                .ForEach((destProperty) => 
                {
                    var origProperty = origTypeProperties.FirstOrDefault((prop) => prop.Name == destProperty.Name);
                    if (origProperty != null)
                    {
                        destProperty.SetValue(to, origProperty.GetValue(from));
                    }
                });
        }
    }
}
