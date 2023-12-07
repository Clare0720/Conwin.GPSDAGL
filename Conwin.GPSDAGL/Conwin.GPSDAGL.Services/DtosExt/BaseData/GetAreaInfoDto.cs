using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.BaseData
{
    public class GetAreaInfoDto
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public string ProvinceName { get; set; }
        public string ProvinceID { get; set; }
        public string CityName { get; set; }
        public string CityID { get; set; }
    }

    public class QueryDistrictsDto
    {
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
    }
}
