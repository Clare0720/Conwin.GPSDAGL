using Conwin.GPSDAGL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Common
{
    public class OrgSNoHelper
    {
        public static int GetOrgSNoTypeValue(OrganizationType organizationType)
        {
           
            return (int)organizationType;
        }
    }
}
