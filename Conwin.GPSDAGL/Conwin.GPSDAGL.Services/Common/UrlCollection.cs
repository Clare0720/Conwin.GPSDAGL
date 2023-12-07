using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Conwin.GPSDAGL.Services.Common
{
    public class UrlCollection
    {
        //文件上传地址
        public static readonly string UploadFileAddress = "/api/ServiceGateway/UploadFile";
        //获取文件地址
        public static readonly string GetFileAddress = "/api/ServiceGateway/GetFile";
        public static readonly string XingZhengWenShuFile = "/XingZhengWenShuFile";
        public static readonly string ImagesFile = "/Config/Images";
    }
}