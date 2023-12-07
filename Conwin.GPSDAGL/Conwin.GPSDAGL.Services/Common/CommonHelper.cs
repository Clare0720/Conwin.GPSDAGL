using Conwin.FileModule.ServiceAgent;
using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.FileAgent;
using Conwin.Framework.Log4net;
using Conwin.Framework.Log4net.Extendsions;
using Conwin.Framework.ServiceAgent.Dtos;
using Conwin.Framework.ServiceAgent.Utilities;
using Conwin.GPSDAGL.Services.DtosExt;
using Conwin.GPSDAGL.Services.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Conwin.GPSDAGL.Services.Common
{
    public static class CommonHelper
    {
        private static string WEBAPISYSID = System.Configuration.ConfigurationManager.AppSettings["WEBAPISYSID"] != null ? System.Configuration.ConfigurationManager.AppSettings["WEBAPISYSID"].ToString() : "";
        private static string WEBAPIAPPID = System.Configuration.ConfigurationManager.AppSettings["WEBAPIAPPID"] != null ? System.Configuration.ConfigurationManager.AppSettings["WEBAPIAPPID"].ToString() : "";
        private static string FileServiceUploadFileAddress = System.Configuration.ConfigurationManager.AppSettings["ApiGateWay"].ToString() + UrlCollection.UploadFileAddress;

        /// <summary>
        /// 车辆在线过期时间，单位分钟
        /// </summary>
        public static uint CheLiangZaiXianExpireTime
        {
            get
            {
                uint value = 120;
                try
                {
                    var rawValue = System.Configuration.ConfigurationManager.AppSettings["CheLiangZaiXianExpireTime"];
                    if (!string.IsNullOrEmpty(rawValue))
                    {
                        value = Convert.ToUInt32(rawValue);
                    }

                }
                catch { value = 120; }
                return value;
            }
        }

        public static T Send<T>(string serviceCode, string serviceVersion, object body)
        {
            var newCwRequest = new CWRequest()
            {
                publicrequest = new CWPublicRequest()
                {
                    protover = ConfigurationManager.AppSettings["Protover"],
                    reqid = Guid.NewGuid().ToString(),
                    requesttime = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                    servicecode = serviceCode,
                    servicever = serviceVersion,
                    sysid = ConfigurationManager.AppSettings["WEBAPISYSID"]
                },
                body = body
            };
            string resp = ServiceAgentUtility.Send(newCwRequest);
            CWResponse response = ServiceAgentUtility.DeserializeResponse(resp);
            if (response.publicresponse.statuscode != 0)
            {
                LogHelper.Error(response.publicresponse.servicecode + ":" + response.publicresponse.message);
            }
            return response.publicresponse.statuscode == 0 ? JsonConvert.DeserializeObject<T>(response.body.ToString()) : default(T);
        }

        public static string UploadExcel(string name, Action<XExcel> action, UserInfoDto user)
        {
            var xls = new XExcel(name);
            return xls.Create().Write(action).Upload(user);
        }

        public static string UploadFile(UserInfoDto userInfo, string filePath)
        {
            string FileId = string.Empty;
            FileDto ResFile = FileAgentUtility.UploadFile(new FileDTO()
            {
                AppId = ConfigurationManager.AppSettings["WEBAPIAPPID"],
                AppName = string.Empty,
                BusinessId = new Guid().ToString(),
                BusinessType = "",
                CreatorId = userInfo == null ? "" : userInfo.Id,
                CreatorName = userInfo == null ? "" : userInfo.UserName,
                DisplayName = Path.GetFileNameWithoutExtension(filePath),
                FileName = Path.GetFileName(filePath),
                Data = File.ReadAllBytes(filePath),
                Remark = string.Empty,
                SystemId = ConfigurationManager.AppSettings["WEBAPISYSID"]
            });
            FileId = ResFile.FileId.ToString();
            return FileId;
        }

        /// <summary>
        /// 上传文件到文件服务器
        /// （上传成功返回文件ID）
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <param name="fileName">文件名（例如："test.jpg"）</param>
        /// <param name="file">文件数组</param>
        /// <returns>返回文件ID</returns>
        public static string UploadFile(UserInfoDto userInfo, string fileName, byte[] file)
        {
            string FileId = string.Empty;
            FileDto ResFile = FileAgentUtility.UploadFile(new FileDTO()
            {
                AppId = ConfigurationManager.AppSettings["WEBAPIAPPID"],
                AppName = string.Empty,
                BusinessId = new Guid().ToString(),
                BusinessType = "",
                CreatorId = userInfo == null ? "" : userInfo.Id,
                CreatorName = userInfo == null ? "" : userInfo.UserName,
                DisplayName = Path.GetFileNameWithoutExtension(fileName),
                FileName = fileName,
                Data = file,
                Remark = string.Empty,
                SystemId = ConfigurationManager.AppSettings["WEBAPISYSID"]
            });
            FileId = ResFile.FileId.ToString();
            return FileId;
        }

        /// <summary>
        /// 上传文件到文件服务器
        /// （上传成功返回文件ID）
        /// </summary>
        /// <param name="userInfoNew">用户信息</param>
        /// <param name="fileName">文件名（例如："test.jpg"）</param>
        /// <param name="file">文件数组</param>
        /// <returns>返回文件ID</returns>
        public static string UploadFile(UserInfoDtoNew userInfoNew, string fileName, byte[] file)
        {
            string FileId = string.Empty;
            FileDto ResFile = FileAgentUtility.UploadFile(new FileDTO()
            {
                AppId = ConfigurationManager.AppSettings["WEBAPIAPPID"],
                AppName = string.Empty,
                BusinessId = new Guid().ToString(),
                BusinessType = "",
                CreatorId = userInfoNew == null ? "" : userInfoNew.Id,
                CreatorName = userInfoNew == null ? "" : userInfoNew.UserName,
                DisplayName = Path.GetFileNameWithoutExtension(fileName),
                FileName = fileName,
                Data = file,
                Remark = string.Empty,
                SystemId = ConfigurationManager.AppSettings["WEBAPISYSID"]
            });
            FileId = ResFile.FileId.ToString();
            return FileId;
        }

        #region 生成pdf、word文件

        //生成Pdf文书并上传
        public static object CreatePdfDoc(string htmlContent, string fileName)
        {
            object result = null;
            PDFHelper pdfHelper = new PDFHelper();
            var pdfByteArray = pdfHelper.ConvertHtmlTextToPDF(htmlContent);
            if (pdfByteArray == null || pdfByteArray.Length == 0)
            {
                result = new { success = false, msg = "生成PDF文档失败！" };
                return result;
            }
            try
            {

                byte[] bb = pdfByteArray;
                fileName = string.Format("{0}.pdf", fileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff"));

                HttpContent AllowedContentTypeContent = new StringContent("png|jpg|pdf");
                HttpContent MaxSizeContent = new StringContent("");
                HttpContent SystemIdContent = new StringContent(WEBAPISYSID);
                HttpContent AppIdContent = new StringContent(WEBAPIAPPID);
                HttpContent CreatorIdContent = new StringContent("");
                HttpContent CreatorNameContent = new StringContent("");
                HttpContent DisplayNameContent = new StringContent(fileName);
                HttpContent fileStreamContent = new StreamContent(new MemoryStream(bb));

                using (var client = new HttpClient())
                {
                    var token = HttpContext.Current.Request.Headers["Token"];
                    client.DefaultRequestHeaders.Add("token", token);
                    using (var formData = new MultipartFormDataContent())
                    {
                        formData.Add(AllowedContentTypeContent, "AllowedContentType");
                        formData.Add(MaxSizeContent, "MaxSize");
                        formData.Add(SystemIdContent, "SystemId");
                        formData.Add(AppIdContent, "AppId");
                        formData.Add(CreatorIdContent, "CreatorId");
                        formData.Add(CreatorNameContent, "CreatorName");
                        formData.Add(DisplayNameContent, "DisplayName");
                        formData.Add(fileStreamContent, "File", fileName);
                        var response = client.PostAsync(FileServiceUploadFileAddress, formData).Result;
                        if (!response.IsSuccessStatusCode)
                        {
                            result = new { success = false, msg = "上传文件失败！" };
                            return result;
                        }

                        var re = response.Content.ReadAsStreamAsync().Result;
                        StreamReader reader = new StreamReader(re);
                        string resp = reader.ReadToEnd();
                        FileUploadResponse frsp = JsonConvert.DeserializeObject<FileUploadResponse>(resp.ToString());
                        if (frsp != null && frsp.success)
                        {
                            result = new { success = true, fileId = frsp.data.FileId };
                        }
                        else
                        {
                            result = new { success = false, msg = "上传文件失败！" };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = new { success = false, msg = "服务错误，请联系管理员！", error = ex.Message };
            }
            return result;
        }
        public static object CreatePdfAndUploadPdfDoc(string htmlContent, string fileName, UserInfoDto userInfo, string customizeStyle = "")
        {
            object result = null;
            PDFHelper pdfHelper = new PDFHelper();
            string filePath = null;
            try
            {
                var pdfByteArray = pdfHelper.ConvertHtmlTextToPDFStrByWkHtmlToPdfReturnFile(htmlContent, fileName, out filePath, customizeStyle);
                if (pdfByteArray == null || pdfByteArray.Length == 0)
                {
                    return new { success = false, msg = "生成PDF文档失败！" };
                }
                FileDTO fileDto = new FileDTO()
                {
                    SystemId = ConfigurationManager.AppSettings["WEBAPISYSID"].ToString(),
                    AppId = ConfigurationManager.AppSettings["WEBAPIAPPID"].ToString(),
                    AppName = "",
                    CreatorId = userInfo.Id,
                    CreatorName = userInfo.UserName,
                    BusinessType = "",
                    BusinessId = "",
                    FileName = fileName ,
                    FileExtension = "pdf",
                    DisplayName = fileName,
                    Remark = ""
                };
                using (MemoryStream ms = new MemoryStream())
                {
                    fileDto.Data = pdfByteArray.ToArray();
                }
                FileDto fileDtoResult = FileAgentUtility.UploadFile(fileDto);
                if (fileDtoResult != null)
                {
                    result = new { success = true, FileId = fileDtoResult.FileId };
                }
            }
            catch (Exception ex)
            {
                result = new { success = false, msg = "服务错误，请联系管理员！", error = ex.Message };
            }
            return result;

        }
        //生成Pdf文书并上传
        public static object CreatePdfAndUploadPdfDoc(string htmlContent, string fileName)
        {
            object result = null;
            PDFHelper pdfHelper = new PDFHelper();
            string filePath = null;
            var pdfByteArray = pdfHelper.ConvertHtmlTextToPDFStrByWkHtmlToPdfReturnFile(htmlContent, fileName, out filePath);
            if (pdfByteArray == null || pdfByteArray.Length == 0)
            {
                result = new { success = false, msg = "生成PDF文档失败！" };
                return result;
            }
            return UploadFile(filePath);

        }


        public static byte[] GetFileContent(string filename)
        {
            byte[] array = null;
            string path = ("~" + UrlCollection.XingZhengWenShuFile).MapPath() + "\\" + filename;
            FileStream fileStream = new FileStream(path, FileMode.Open);
            try
            {
                array = new byte[fileStream.Length];
                fileStream.Read(array, 0, (int)fileStream.Length);
            }
            finally
            {
                if (fileStream != null)
                {
                    ((IDisposable)fileStream).Dispose();
                }
            }
            return array;
        }

        /// <summary>
        /// 车辆是否在线
        /// </summary>
        /// <param name="latestGpsTime"></param>
        /// <returns></returns>
        public static bool? CheLiangShiFouZaiXian(DateTime? latestGpsTime)
        {
            if (latestGpsTime.HasValue)
            {
                //是否在线判断规则：
                //车辆最后一次定位时间在 2 小时以内
                DateTime compareTime = DateTime.Now;
                return Math.Abs((compareTime - latestGpsTime.Value).TotalHours) <= 2;
            }
            else
            {
                return false;
            }
        }

        //定义文件上传返回参数
        private class FileUploadResponse
        {
            public bool success { get; set; }
            public string msg { get; set; }
            public FileUploadResponseData data { get; set; }
        }

        //定义文件上传返回data参数
        private class FileUploadResponseData
        {
            public string ElementId { get; set; }
            public string FileId { get; set; }
            public string SystemId { get; set; }
            public string AppId { get; set; }
            public string AppName { get; set; }
            public string BusinessType { get; set; }
            public string BusinessId { get; set; }
            public string OpType { get; set; }
            public string AttachName { get; set; }
            public string AttachDisName { get; set; }
            public string AttachPath { get; set; }
            public string URL { get; set; }
            public string AttachSize { get; set; }
            public string FileName { get; set; }
            public string Extension { get; set; }
            public string SessionId { get; set; }
            public string CreateId { get; set; }
            public string Creator { get; set; }
        }

        #endregion


        #region 上传文件到文件服务器

        public static object UploadFile(string filePath)
        {
            object result = null;
            FileStream fs = new FileStream(filePath, FileMode.Open);
            //获取文件大小
            long size = fs.Length;
            byte[] fileByteArray = new byte[size];
            fs.Read(fileByteArray, 0, fileByteArray.Length);
            fs.Close();
            if (fileByteArray == null || fileByteArray.Length == 0)
            {
                result = new { success = false, msg = "生成文件失败！" };
                return result;
            }
            try
            {

                byte[] bb = fileByteArray;
                //var fileName =  Path.GetFileNameWithoutExtension(filePath);
                var fileName = Path.GetFileName(filePath);
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);

                HttpContent AllowedContentTypeContent = new StringContent("png|jpg|jpeg|tif|gif|pdf|doc|docs|xls|xlsx|ppt|pptx|txt|rar|zip");
                HttpContent MaxSizeContent = new StringContent("");
                HttpContent SystemIdContent = new StringContent(WEBAPISYSID);
                HttpContent AppIdContent = new StringContent(WEBAPIAPPID);
                HttpContent CreatorIdContent = new StringContent("");
                HttpContent CreatorNameContent = new StringContent("");
                HttpContent DisplayNameContent = new StringContent(fileNameWithoutExtension, Encoding.UTF8);
                HttpContent fileStreamContent = new StreamContent(new MemoryStream(bb));

                using (var client = new HttpClient())
                {
                    var token = HttpContext.Current.Request.Headers["Token"];
                    client.DefaultRequestHeaders.Add("token", token);

                    using (var formData = new MultipartFormDataContent())
                    {
                        formData.Add(AllowedContentTypeContent, "AllowedContentType");
                        formData.Add(MaxSizeContent, "MaxSize");
                        formData.Add(SystemIdContent, "SystemId");
                        formData.Add(AppIdContent, "AppId");
                        formData.Add(CreatorIdContent, "CreatorId");
                        formData.Add(CreatorNameContent, "CreatorName");
                        formData.Add(DisplayNameContent, "DisplayName");
                        formData.Add(fileStreamContent, "File", WebUtility.UrlEncode(fileName));

                        var response = client.PostAsync(FileServiceUploadFileAddress, formData).Result;
                        if (!response.IsSuccessStatusCode)
                        {
                            result = new { success = false, msg = "文件服务上传文件失败！" };
                            return result;
                        }

                        var re = response.Content.ReadAsStreamAsync().Result;
                        StreamReader reader = new StreamReader(re);
                        string resp = reader.ReadToEnd();
                        FileUploadResponse frsp = JsonConvert.DeserializeObject<FileUploadResponse>(resp.ToString());
                        if (frsp != null && frsp.success)
                        {
                            result = new { success = true, fileId = frsp.data.FileId };
                        }
                        else
                        {
                            result = new { success = false, msg = "文件服务上传文件失败！" };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = new { success = false, msg = "服务错误，请联系管理员！", error = ex.Message };
            }
            return result;
        }


        #endregion


        #region 实体对象互转

        /// <summary>
        /// 实体对象互转
        /// </summary>
        /// <typeparam name="D">目标对象</typeparam>
        /// <typeparam name="S">待转对象</typeparam>
        /// <param name="s">待转实体</param>
        /// <returns></returns>
        public static D Mapper<D, S>(this S s)
        {
            D d = Activator.CreateInstance<D>();
            try
            {
                var Types = s.GetType();//获得类型  
                var Typed = typeof(D);
                foreach (PropertyInfo sp in Types.GetProperties())//获得类型的属性字段  
                {
                    foreach (PropertyInfo dp in Typed.GetProperties())
                    {
                        if (dp.Name.ToLower() == sp.Name.ToLower())//判断属性名是否相同  
                        {
                            dp.SetValue(d, sp.GetValue(s, null), null);//获得s对象属性的值复制给d对象的属性  
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return d;
        }

        /// <summary>
        /// 实体对象互转
        /// </summary>
        /// <typeparam name="D">目标对象</typeparam>
        /// <typeparam name="S">待转对象</typeparam>
        /// <param name="s">待转实体</param>
        /// <returns></returns>
        public static IList<D> Mapper<D, S>(this IList<S> sList)
        {
            IList<D> dList = new List<D>();
            foreach (var s in sList)
            {
                D d = Mapper<D, S>(s);
                dList.Add(d);
            }
            return dList;
        }

        #endregion

        #region 类型转换

        /// <summary>
        /// Converts to byte datatype from an arbitrary object.
        /// </summary>
        /// <remarks>Returns 0 if conversion fails.</remarks>
        /// <param name="Value">An arbitrary object</param>
        /// <returns>byte value</returns>
        public static byte ToByte(object Value)
        {
            return ToByte(Value, 0);
        }

        /// <summary>
        /// Converts to byte datatype from an arbitrary object.
        /// </summary>
        /// <param name="Value">An arbitrary object</param>
        /// <param name="DefaultValue">Value to be returned if conversion fails or is null.</param>
        /// <returns>byte value</returns>
        public static byte ToByte(object Value, byte DefaultValue)
        {
            try
            {
                return (Value == DBNull.Value || Value == null ? DefaultValue : Convert.ToByte(Value));
            }
            catch
            {
                return DefaultValue;
            }
        }

        /// <summary>
        /// Converts to short (Int16) datatype from an arbitrary object.
        /// </summary>
        /// <remarks>Returns -1 if conversion fails.</remarks>
        /// <param name="Value">An arbitrary object</param>
        /// <returns>short value</returns>
        public static short ToShort(object Value)
        {
            return ToShort(Value, -1);
        }

        /// <summary>
        /// Converts to short (Int16) datatype from an arbitrary object.
        /// </summary>
        /// <param name="Value">An arbitrary object</param>
        /// <param name="DefaultValue">Value to be returned if conversion fails or is null.</param>
        /// <returns>short value</returns>
        public static short ToShort(object Value, short DefaultValue)
        {
            try
            {
                return (Value == DBNull.Value || Value == null ? DefaultValue : Convert.ToInt16(Value));
            }
            catch
            {
                return DefaultValue;
            }
        }

        /// <summary>
        /// Converts to int (Int32) datatype from an arbitrary object.
        /// </summary>
        /// <remarks>Returns -1 if conversion fails.</remarks>
        /// <param name="Value">An arbitrary object</param>
        /// <returns>Int value</returns>
        public static int ToInt(object Value)
        {
            return ToInt(Value, -1);
        }

        /// <summary>
        /// Converts to int (Int32) datatype from an arbitrary object.
        /// </summary>
        /// <param name="Value">An arbitrary object</param>
        /// <param name="DefaultValue">Value to be returned if conversion fails or is null.</param>
        /// <returns>Int value</returns>
        public static int ToInt(object Value, int DefaultValue)
        {
            try
            {
                return (Value == DBNull.Value || Value == null ? DefaultValue : Convert.ToInt32(Value));
            }
            catch
            {
                return DefaultValue;
            }
        }

        public static int? ToInt(object Value, int? DefaultValue)
        {
            try
            {
                return (Value == DBNull.Value || Value == null ? DefaultValue : Convert.ToInt32(Value));
            }
            catch
            {
                return DefaultValue;
            }
        }

        /// <summary>
        /// Converts to long (Int64) datatype from an arbitrary object.
        /// </summary>
        /// <remarks>Returns -1 if conversion fails.</remarks>
        /// <param name="Value">An arbitrary object</param>
        /// <returns>Long value</returns>
        public static long ToLong(object Value)
        {
            return ToLong(Value, -1);
        }

        /// <summary>
        /// Converts to long (Int64) datatype from an arbitrary object.
        /// </summary>
        /// <param name="Value">An arbitrary object</param>
        /// <param name="DefaultValue">Value to be returned if conversion fails or is null.</param>
        /// <returns>Long value</returns>
        public static long ToLong(object Value, long DefaultValue)
        {
            try
            {
                return (Value == DBNull.Value || Value == null ? DefaultValue : Convert.ToInt64(Value));
            }
            catch
            {
                return DefaultValue;
            }
        }

        public static long? ToLong(object Value, long? DefaultValue)
        {
            try
            {
                return (Value == DBNull.Value || Value == null ? DefaultValue : Convert.ToInt64(Value));
            }
            catch
            {
                return DefaultValue;
            }
        }

        /// <summary>
        /// Converts to unsigned short (UInt16) datatype from an arbitrary object.
        /// </summary>
        /// <remarks>Returns 0 if conversion fails.</remarks>
        /// <param name="Value">An arbitrary object</param>
        /// <returns>ushort value</returns>
        public static ushort ToUnsignedShort(object Value)
        {
            return ToUnsignedShort(Value, 0);
        }

        /// <summary>
        /// Converts to unsigned short (UInt16) datatype from an arbitrary object.
        /// </summary>
        /// <param name="Value">An arbitrary object</param>
        /// <param name="DefaultValue">Value to be returned if conversion fails or is null.</param>
        /// <returns>ushort value</returns>
        public static ushort ToUnsignedShort(object Value, ushort DefaultValue)
        {
            try
            {
                return (Value == DBNull.Value || Value == null ? DefaultValue : Convert.ToUInt16(Value));
            }
            catch
            {
                return DefaultValue;
            }
        }

        public static ushort? ToUnsignedShort(object Value, ushort? DefaultValue)
        {
            try
            {
                return (Value == DBNull.Value || Value == null ? DefaultValue : Convert.ToUInt16(Value));
            }
            catch
            {
                return DefaultValue;
            }
        }

        /// <summary>
        /// Converts to unsigned int (UInt32) datatype from an arbitrary object.
        /// </summary>
        /// <remarks>Returns 0 if conversion fails.</remarks>
        /// <param name="Value">An arbitrary object</param>
        /// <returns>UInt value</returns>
        public static uint ToUnsignedInt(object Value)
        {
            return ToUnsignedInt(Value, 0);
        }

        /// <summary>
        /// Converts to unsigned int (UInt32) datatype from an arbitrary object.
        /// </summary>
        /// <param name="Value">An arbitrary object</param>
        /// <param name="DefaultValue">Value to be returned if conversion fails or is null.</param>
        /// <returns>UInt value</returns>
        public static uint ToUnsignedInt(object Value, uint DefaultValue)
        {
            try
            {
                return (Value == DBNull.Value || Value == null ? DefaultValue : Convert.ToUInt32(Value));
            }
            catch
            {
                return DefaultValue;
            }
        }

        public static uint? ToUnsignedInt(object Value, uint? DefaultValue)
        {
            try
            {
                return (Value == DBNull.Value || Value == null ? DefaultValue : Convert.ToUInt32(Value));
            }
            catch
            {
                return DefaultValue;
            }
        }

        /// <summary>
        /// Converts to unsigned long (UInt64) datatype from an arbitrary object.
        /// </summary>
        /// <remarks>Returns 0 if conversion fails.</remarks>
        /// <param name="Value">An arbitrary object</param>
        /// <returns>ULong value</returns>
        public static ulong ToUnsignedLong(object Value)
        {
            return ToUnsignedLong(Value, 0);
        }

        /// <summary>
        /// Converts to long (Int64) datatype from an arbitrary object.
        /// </summary>
        /// <param name="Value">An arbitrary object</param>
        /// <param name="DefaultValue">Value to be returned if conversion fails or is null.</param>
        /// <returns>Long value</returns>
        public static ulong ToUnsignedLong(object Value, ulong DefaultValue)
        {
            try
            {
                return (Value == DBNull.Value || Value == null ? DefaultValue : Convert.ToUInt64(Value));
            }
            catch
            {
                return DefaultValue;
            }
        }

        public static ulong? ToUnsignedLong(object Value, ulong? DefaultValue)
        {
            try
            {
                return (Value == DBNull.Value || Value == null ? DefaultValue : Convert.ToUInt64(Value));
            }
            catch
            {
                return DefaultValue;
            }
        }

        /// <summary>
        /// Converts to decimal datatype from an arbitrary object.
        /// </summary>
        /// <remarks>Returns 0 if conversion fails.</remarks>
        /// <param name="Value">An arbitrary object</param>
        /// <returns>decimal value</returns>
        public static decimal ToDecimal(object Value)
        {
            return ToDecimal(Value, 0);
        }

        /// <summary>
        /// Converts to decimal datatype from an arbitrary object.
        /// </summary>
        /// <param name="Value">An arbitrary object</param>
        /// <param name="DefaultValue">Value to be returned if conversion fails or is null.</param>
        /// <returns>decimal value</returns>
        public static decimal ToDecimal(object Value, decimal DefaultValue)
        {
            try
            {
                return (Value == DBNull.Value || Value == null ? DefaultValue : Convert.ToDecimal(Value));
            }
            catch
            {
                return DefaultValue;
            }
        }

        public static decimal? ToDecimal(object Value, decimal? DefaultValue)
        {
            try
            {
                return (Value == DBNull.Value || Value == null ? DefaultValue : Convert.ToDecimal(Value));
            }
            catch
            {
                return DefaultValue;
            }
        }

        /// <summary>
        /// Converts to double datatype from an arbitrary object.
        /// </summary>
        /// <remarks>Returns 0 if conversion fails.</remarks>
        /// <param name="Value">An arbitrary object</param>
        /// <returns>double value</returns>
        public static double ToDouble(object Value)
        {
            return ToDouble(Value, 0);
        }

        /// <summary>
        /// Converts to decimal datatype from an arbitrary object.
        /// </summary>
        /// <param name="Value">An arbitrary object</param>
        /// <param name="DefaultValue">Value to be returned if conversion fails or is null.</param>
        /// <returns>double value</returns>
        public static double ToDouble(object Value, double DefaultValue)
        {
            try
            {
                return (Value == DBNull.Value || Value == null ? DefaultValue : Convert.ToDouble(Value));
            }
            catch
            {
                return DefaultValue;
            }
        }

        public static double? ToDouble(object Value, double? DefaultValue)
        {
            try
            {
                return (Value == DBNull.Value || Value == null ? DefaultValue : Convert.ToDouble(Value));
            }
            catch
            {
                return DefaultValue;
            }
        }

        /// <summary>
        /// Converts to single (float) datatype from an arbitrary object.
        /// </summary>
        /// <remarks>Returns 0 if conversion fails.</remarks>
        /// <param name="Value">An arbitrary object</param>
        /// <returns>single (float) value</returns>
        public static float ToSingle(object Value)
        {
            return ToSingle(Value, 0);
        }

        /// <summary>
        /// Converts to single (float) datatype from an arbitrary object.
        /// </summary>
        /// <param name="Value">An arbitrary object</param>
        /// <param name="DefaultValue">Value to be returned if conversion fails or is null.</param>
        /// <returns>single (float) value</returns>
        public static float ToSingle(object Value, float DefaultValue)
        {
            try
            {
                return (Value == DBNull.Value || Value == null ? DefaultValue : Convert.ToSingle(Value));
            }
            catch
            {
                return DefaultValue;
            }
        }

        public static float? ToSingle(object Value, float? DefaultValue)
        {
            try
            {
                return (Value == DBNull.Value || Value == null ? DefaultValue : Convert.ToSingle(Value));
            }
            catch
            {
                return DefaultValue;
            }
        }

        /// <summary>
        /// Converts to single (float) datatype from an arbitrary object.
        /// </summary>
        /// <param name="Value">An arbitrary object</param>
        /// <returns>single (float) value</returns>
        public static float ToFloat(object Value)
        {
            return ToSingle(Value);
        }

        /// <summary>
        /// Converts to single (float) datatype from an arbitrary object.
        /// </summary>
        /// <param name="Value">An arbitrary object</param>
        /// <param name="DefaultValue">Value to be returned if conversion fails or is null.</param>
        /// <returns>single (float) value</returns>
        public static float ToFloat(object Value, float DefaultValue)
        {
            return ToSingle(Value, DefaultValue);
        }

        /// <summary>
        /// Converts to boolean datatype from an arbitrary object.
        /// </summary>
        /// <remarks>Returns false if conversion fails.</remarks>
        /// <param name="Value">An arbitrary object</param>
        /// <returns>Boolean value</returns>
        public static bool ToBoolean(object Value)
        {
            try
            {
                return (Value == DBNull.Value || Value == null ? false : Boolean.Parse(Value.ToString()));
            }
            catch
            {
                return (Value.ToString() == "0" ? false : true);
            }
        }

        public static bool? ToBoolean(object Value, bool? DefaultValue)
        {
            try
            {
                return (Value == DBNull.Value || Value == null ? false : Boolean.Parse(Value.ToString()));
            }
            catch
            {
                return DefaultValue;
            }
        }

        /// <summary>
        /// Converts to string datatype from an arbitrary object.
        /// </summary>
        /// <remarks>Returns a blank string if conversion fails.</remarks>
        /// <param name="Value">An arbitrary object</param>
        /// <returns>string value</returns>
        public static string ToString(object Value)
        {
            return ToString(Value, "");
        }

        /// <summary>
        /// Converts to string datatype from an arbitrary object.
        /// </summary>
        /// <param name="Value">An arbitrary object</param>
        /// <param name="DefaultValue">Value to be returned if conversion fails or is null.</param>
        /// <returns>string value</returns>
        public static string ToString(object Value, string DefaultValue)
        {
            try
            {
                return (Value == DBNull.Value || Value == null ? DefaultValue : Value.ToString());
            }
            catch
            {
                return DefaultValue;
            }
        }

        /// <summary>
        /// Converts to DateTime datatype from an arbitrary object.
        /// </summary>
        /// <remarks>Returns DateTime.MinValue if conversion fails.</remarks>
        /// <param name="Value">An arbitrary object</param>
        /// <returns>DateTime value</returns>
        public static DateTime ToDateTime(object Value)
        {
            return ToDateTime(Value, DateTime.MinValue);
        }

        /// <summary>
        /// Converts to DateTime datatype from an arbitrary object.
        /// </summary>
        /// <param name="Value">An arbitrary object</param>
        /// <param name="DefaultValue">Value to be returned if conversion fails or is null.</param>
        /// <returns>DateTime value</returns>
        public static DateTime ToDateTime(object Value, DateTime DefaultValue)
        {
            try
            {
                return (Value == DBNull.Value || Value == null ? DefaultValue : DateTime.Parse(Value.ToString()));
            }
            catch
            {
                return DefaultValue;
            }
        }

        public static DateTime? ToDateTime(object Value, DateTime? DefaultValue)
        {
            try
            {
                return (Value == DBNull.Value || Value == null ? DefaultValue : DateTime.Parse(Value.ToString()));
            }
            catch
            {
                return DefaultValue;
            }
        }

        public static string StringToBase64(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;
            Encoding encode = Encoding.UTF8;
            var bytes = encode.GetBytes(input);
            return Convert.ToBase64String(bytes, 0, bytes.Length);
        }

        public static string Base64ToString(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;
            var bytes = Convert.FromBase64String(input);
            return Encoding.UTF8.GetString(bytes);
        }

        #endregion 类型转换

        #region 服务调用
        public static CWRequest GenerateRequest(string sysId, string serviceCode, string serviceVersion, object bodyContent)
        {
            CWRequest request = new CWRequest()
            {
                publicrequest = new CWPublicRequest()
                {
                    sysid = sysId,
                    reqid = Guid.NewGuid().ToString(),
                    protover = "1.0",
                    servicecode = serviceCode,
                    servicever = serviceVersion,
                    requesttime = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                    signdata = "",
                    reserve = "",
                },
                body = bodyContent
            };
            return request;
        }

        public static CWResponse GenerateResponse(CWRequest request, object bodyContent)
        {
            CWResponse response = new CWResponse()
            {
                publicresponse = new CWPublicResponse()
                {
                    sysid = request.publicrequest.sysid,
                    reqid = request.publicrequest.reqid,
                    protover = request.publicrequest.protover,
                    servicecode = request.publicrequest.servicecode,
                    servicever = request.publicrequest.servicever,
                    responsetime = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                    signdata = "",
                    statuscode = 0,
                    reserve = "",
                    message = ""
                },
                body = bodyContent
            };
            return response;
        }
        public static ServiceResult<T> HttpResponseModel<T>(string sysId, string serviceCode, string serviceVersion, object bodyContent) where T : class
        {
            var result = new ServiceResult<T>();
            var request = GenerateRequest(sysId, serviceCode, serviceVersion, bodyContent);
            var responseString = "";
            try
            {
                responseString = ServiceAgentUtility.Send(request);
            }
            catch (Exception ex)
            {
                result.StatusCode = 2;
                result.ErrorMessage = "接口请求异常：" + ex.Message;
                LogHelper.Error(serviceCode + "接口请求异常，请求报文：" + JsonConvert.SerializeObject(request), ex);
            }
            if (result.StatusCode == 0)
            {
                try
                {
                    CWResponse response = ServiceAgentUtility.DeserializeResponse(responseString);
                    if (response.publicresponse.statuscode == 0)
                    {
                        if (response.body != null)
                        {
                            result.Data = response.GetBody<T>();
                        }
                    }
                    else
                    {
                        result.StatusCode = 2;
                        result.ErrorMessage = response.publicresponse.message;
                        LogHelper.Error(serviceCode + "返回失败，报文：" + JsonConvert.SerializeObject(new { Request = request, Response = responseString }));
                    }
                }
                catch (Exception ex)
                {
                    result.StatusCode = 2;
                    result.ErrorMessage = serviceCode + "接口响应数据异常：" + ex.Message;
                    LogHelper.Error(serviceCode + "接口请求异常，报文：" + JsonConvert.SerializeObject(new { Request = request, Response = responseString }), ex);
                }
            }
            else
            {
                result.StatusCode = 2;
                result.ErrorMessage = serviceCode + "接口响应失败";
                LogHelper.Error(serviceCode + "接口响应失败，报文：" + JsonConvert.SerializeObject(new { Request = request, Response = responseString }));
            }

            return result;
        }

        #endregion

        public static T ReadConfigData<T>(string filepath)
        {
            var json = Readjson(filepath);
            if (string.IsNullOrEmpty(json))
                return default(T);
            return JsonConvert.DeserializeObject<T>(json);
        }
        public static string Readjson(string filepath)
        {
            string json = string.Empty;
            if (!File.Exists(filepath))
                return json;
            using (FileStream fs = new FileStream(filepath, FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    json = sr.ReadToEnd().ToString();
                }
            }
            return json;
        }
    }
}

public class XExcel
{
    public NopiExcel nopi;

    /// <summary>
    /// 服务器路径
    /// </summary>
    private readonly string CachePath = AppDomain.CurrentDomain.BaseDirectory + "\\App_Data\\ExcelFileCache";

    /// <summary>
    /// Excel路径
    /// </summary>
    public readonly string filePath = string.Empty;

    /// <summary>
    /// Excel名称
    /// </summary>
    public readonly string fileName;


    public XExcel(string name)
    {
        this.fileName = name;
        this.filePath = string.Format("{0}\\{1}.xls", this.CachePath, name);
    }

    /// <summary>
    /// 创建Excel
    /// </summary>
    /// <returns></returns>
    public XExcel Create()
    {
        nopi = new NopiExcel(1);
        if (!Directory.Exists(this.CachePath))
        {
            Directory.CreateDirectory(this.CachePath);
        }
        nopi.Save(this.filePath);
        return this;
    }

    /// <summary>
    /// 修改Excel
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public XExcel Write(Action<XExcel> action)
    {
        //this.nopi = new NopiExcel(this.filePath);
        action(this);
        nopi.Save(this.filePath);
        return this;
    }

    /// <summary>
    /// 上传Excel
    /// </summary>
    /// <param name="userInfo"></param>
    /// <returns></returns>
    public string Upload(UserInfoDto userInfo)
    {
        string FileId = string.Empty;
        FileDto ResFile = FileAgentUtility.UploadFile(new FileDTO()
        {
            AppId = ConfigurationManager.AppSettings["WEBAPIAPPID"],
            AppName = string.Empty,
            BusinessId = new Guid().ToString(),
            BusinessType = "",
            CreatorId = userInfo.Id,
            CreatorName = userInfo.UserName,
            DisplayName = Path.GetFileNameWithoutExtension(this.filePath),
            FileName = Path.GetFileName(this.filePath),
            Data = File.ReadAllBytes(this.filePath),
            Remark = string.Empty,
            SystemId = ConfigurationManager.AppSettings["WEBAPISYSID"]
        });
        FileId = ResFile.FileId.ToString();
        return FileId;
    }

}

