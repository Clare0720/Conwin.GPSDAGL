using System;
using System.Collections.Generic;
using System.IO;

namespace Conwin.GPSDAGL.Framework
{
    public static class PositionRolesHelper
    {
        public static PositionRoles PositionRoles { get; private set; }

        public static void Init()
        {
            var configString = GetJson("PositionRoles.json");
            PositionRoles = Newtonsoft.Json.JsonConvert.DeserializeObject<PositionRoles>(configString);
        }


        private static string GetJson(string filepath)
        {
            string json = string.Empty;
            using (FileStream fs = new FileStream(System.AppDomain.CurrentDomain.BaseDirectory + "Config/Position/" + filepath, FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    json = sr.ReadToEnd().ToString();
                }
            }
            return json;
        }

    }

    public class PositionRoles
    {
        public List<PositionItem> PositionTable { get; set; }
        public Dictionary<string, string> RolesMap { get; set; }
    }

    public class PositionItem
    {
        public string Organization { get; set; }
        public string OrganizationCode { get; set; }
        public List<PositionType> PositionType { get; set; }
    }

    public class PositionType
    {
        public string PositionName { get; set; }
        public string PositionCode { get; set; }
    }

}
