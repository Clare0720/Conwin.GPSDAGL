using Conwin.Framework.BusinessLogger;
using Conwin.Framework.BusinessLogger.Dtos;
using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.Log4net;
using Conwin.Framework.Redis;
using Conwin.Framework.ServiceAgent.Dtos;
using Conwin.Framework.ServiceAgent.Utilities;
using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Entities.Enums;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.DtosExt;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace Conwin.GPSDAGL.Services
{
    /// <summary>
    ///  基础帮助类
    /// </summary>
    public abstract class ApiServiceBase : IDisposable
    {
        private readonly string APPCODE = ConfigurationManager.AppSettings["APPCODE"];
        protected readonly string SysId = ConfigurationManager.AppSettings["WEBAPISYSID"].ToString();
        /// <summary>
        /// 管理员组织编号
        /// </summary>
        protected readonly string AdminOrgCode = ConfigurationManager.AppSettings["AdminOrgCode"];
        private readonly IBussinessLogger _bussinessLogger;
        public ApiServiceBase(IBussinessLogger bussinessLogger)
        {
            _bussinessLogger = bussinessLogger;
        }

        protected void AddBussiness(BusinessLogDTO businessLogDTO, UserInfoDtoNew userInfo)
        {
            if (businessLogDTO != null && userInfo != null)
            {
                businessLogDTO.YeWuBanLiDanWeiID = Guid.Parse(userInfo.OrganId);
                businessLogDTO.YeWuBanLiDanWei = userInfo.OrganizationName;
                businessLogDTO.YeWuBanLiRenID = new Guid(userInfo.ExtUserId);
                businessLogDTO.YeWuBanLiRen = userInfo.UserName;
                businessLogDTO.XiTongID = Guid.Parse(ConfigurationManager.AppSettings["WEBAPISYSID"]);
                //BianGengHouShuJuBanBen = "",
                //BianGengQianShuJuBanBen = "",
                //MoKuaiID = Guid.NewGuid(),
                //YeWuLiuChengID = Guid.NewGuid(),
                //BeiZhu = ""
                businessLogDTO.YeWuShouLiHao = Guid.NewGuid().ToString();
                businessLogDTO.YeWuBanLiShiJian = DateTime.Now;
                businessLogDTO.MoKuaiBianHao = APPCODE;//必填
                businessLogDTO.ShuJuZhuangTaiBiaoZhi = "正常";

                _bussinessLogger.LogAsync(businessLogDTO);
            }
        }

        protected void AddBussiness(BusinessLogDTO businessLogDTO)
        {
            if (businessLogDTO != null)
            {
                //businessLogDTO.YeWuBanLiDanWeiID = Guid.Parse(userInfo.OrganId);
                //businessLogDTO.YeWuBanLiDanWei = userInfo.OrganizationName;
                //businessLogDTO.YeWuBanLiRenID = new Guid(userInfo.ExtUserId);
                //businessLogDTO.YeWuBanLiRen = userInfo.UserName;
                businessLogDTO.XiTongID = Guid.Parse(ConfigurationManager.AppSettings["WEBAPISYSID"]);
                //BianGengHouShuJuBanBen = "",
                //BianGengQianShuJuBanBen = "",
                //MoKuaiID = Guid.NewGuid(),
                //YeWuLiuChengID = Guid.NewGuid(),
                //BeiZhu = ""
                businessLogDTO.YeWuShouLiHao = Guid.NewGuid().ToString();
                businessLogDTO.YeWuBanLiShiJian = DateTime.Now;
                businessLogDTO.MoKuaiBianHao = APPCODE;//必填
                businessLogDTO.ShuJuZhuangTaiBiaoZhi = "正常";

                _bussinessLogger.LogAsync(businessLogDTO);
            }
        }

        protected dynamic GetRequestBody(string request)
        {
            var cw = ServiceAgentUtility.DeserializeRequest(request);
            var body = cw.body;
            return body;
        }
        protected T GetRequestBodyDto<T>(string request) where T : class
        {
            var cw = ServiceAgentUtility.DeserializeRequest(request);
            var body = JsonConvert.SerializeObject(cw.body);
            return JsonConvert.DeserializeObject<T>(body);
        }


        /// <summary>
        /// 捕捉错误
        /// </summary>
        /// <param name="request"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        protected ServiceResult<T> ExecuteCommandStruct<T>(Func<dynamic> command)
        {
            string response = string.Empty;
            try
            {
                var result = command.Invoke();
                return result;

            }
            catch (ApplicationException ae)
            {
                LogHelper.Error($"ApplicationException异常信息:{ae.Message}", ae);
                return new ServiceResult<T>() { Data = default(T), StatusCode = 2, ErrorMessage = ae.Message };
            }
            catch (Exception e)
            {
                LogHelper.Error($"Exception异常信息:{e.Message}", e);
                return new ServiceResult<T>() { Data = default(T), StatusCode = 2, ErrorMessage = e.Message };
            }
            finally
            {
                this.Dispose();
            }
        }

        /// <summary>
        /// 捕捉错误
        /// </summary>
        /// <param name="request"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        protected ServiceResult<T> ExecuteCommandClass<T>(Func<dynamic> command) where T : class
        {
            string response = string.Empty;
            try
            {
                var result = command.Invoke();
                return result;

            }
            catch (ApplicationException ae)
            {
                return new ServiceResult<T>() { Data = default(T), StatusCode = 2, ErrorMessage = ae.Message };
            }
            catch (Exception e)
            {
                return new ServiceResult<T>() { Data = default(T), StatusCode = 2, ErrorMessage = e.Message };
            }
            finally
            {
                this.Dispose();
            }
        }

        protected ServiceResult<T> ExecuteCommandClass<T>(Func<dynamic> command, Action<Exception> exceptionHandler) where T : class
        {
            string response = string.Empty;
            try
            {
                var result = command.Invoke();
                return result;

            }
            catch (ApplicationException ae)
            {
                exceptionHandler(ae);
                return new ServiceResult<T>() { Data = default(T), StatusCode = 2, ErrorMessage = ae.Message };
            }
            catch (Exception e)
            {
                exceptionHandler(e);
                return new ServiceResult<T>() { Data = default(T), StatusCode = 2, ErrorMessage = e.Message };
            }
            finally
            {
                this.Dispose();
            }
        }

        protected ServiceResult<T> ExecuteCommandClass<T>(string cache, Func<dynamic> command) where T : class
        {
            string response = string.Empty;
            try
            {
                var obj = OpeateCache(cache, new TimeSpan(1, 0, 0), command);
                var result = JsonConvert.DeserializeObject<ServiceResult<T>>(obj.ToString());
                return result;
            }
            catch (ApplicationException ae)
            {
                return new ServiceResult<T>() { Data = default(T), StatusCode = 2, ErrorMessage = ae.Message };
            }
            catch (Exception e)
            {
                return new ServiceResult<T>() { Data = default(T), StatusCode = 2, ErrorMessage = e.Message };
            }
            finally
            {
                this.Dispose();
            }
        }


        /// <summary>
        /// 包装缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        protected object OpeateCache(string key, TimeSpan timespan, Func<object> command)
        {
            var cache = RedisManager.Get(key);
            object result = null;
            if (!cache.HasValue)
            {
                result = command.Invoke();
                RedisManager.Set(key, result, timespan);
            }
            else
            {
                result = JsonConvert.DeserializeObject(cache.ToString());
            }
            return result;
        }

        protected static CWResponse GetInvokeRequest(string serviceCode, string serviceVersion, object body)
        {
            var token = ServiceAgentUtility.GetToken();
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
            string resp = ServiceAgentUtility.Send(newCwRequest, token);
            CWResponse response = ServiceAgentUtility.DeserializeResponse(resp);

            if (response == null || response.publicresponse == null || response.publicresponse.statuscode != 0)
            {
                LogHelper.Debug($"请求失败或返回错误。【请求】{JsonConvert.SerializeObject(newCwRequest)}【响应】{JsonConvert.SerializeObject(response)}");
            }

            return response;
        }

        protected static UserInfoDtoNew GetUserInfo()
        {
            var token = ServiceAgentUtility.GetToken();
            var userInfoResponse = GetInvokeRequest("00000030002", "1.0", token);
            UserInfoDtoNew userInfoDto = null;//new UserInfoDtoNew();
            if (userInfoResponse.publicresponse.statuscode == 0)
            {
                userInfoDto = JsonConvert.DeserializeObject<UserInfoDtoNew>(Convert.ToString(userInfoResponse.body));
            }
            return userInfoDto;
        }


        protected dynamic GetRequestBody(CWRequest request)
        {
            var body = request.body;
            return body;
        }
        /// <summary>
        /// 添加SYS处理
        /// </summary>
        protected static void SetCreateSYSInfo<T>(T entity, UserInfoDto currentUser, bool isAdd = true, string ShuJuLaiYuan = "手动录入") where T : EntityMetadata
        {
            if (entity != null && currentUser != null)
            {
                entity.SYS_ChuangJianRen = currentUser.UserName;
                entity.SYS_ChuangJianRenID = currentUser.Id;
                entity.SYS_ChuangJianShiJian = DateTime.Now;
                SetBaseSYSInfo(entity, currentUser);
                entity.SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.正常;
            }
        }

        protected static void SetCreateSYSInfo<T>(T entity, UserInfoDtoNew currentUser, bool isAdd = true, string ShuJuLaiYuan = "手动录入") where T : EntityMetadata
        {
            if (entity != null && currentUser != null)
            {
                entity.SYS_ChuangJianRen = currentUser.UserName;
                entity.SYS_ChuangJianRenID = currentUser.Id;
                entity.SYS_ChuangJianShiJian = DateTime.Now;
                SetBaseSYSInfo(entity, currentUser);
                entity.SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.正常;
            }
        }

        /// <summary>
        /// 删除SYS处理
        /// </summary>
        protected Expression<Func<T, T>> SetDelSYSInfo<T>(T entity, UserInfoDto userInfo) where T : EntityMetadata, new()
        {
            Expression<Func<T, T>> exp = q => new T()
            {
                SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.作废,
                SYS_ZuiJinXiuGaiRen = userInfo.UserName,
                SYS_ZuiJinXiuGaiRenID = userInfo.Id,
                SYS_ZuiJinXiuGaiShiJian = DateTime.Now
            };
            return exp;
        }
        /// <summary>
        /// 删除SYS处理
        /// </summary>
        protected Expression<Func<T, T>> SetDelSYSInfo<T>(T entity, UserInfoDtoNew userInfo) where T : EntityMetadata, new()
        {
            Expression<Func<T, T>> exp = q => new T()
            {
                SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.作废,
                SYS_ZuiJinXiuGaiRen = userInfo.UserName,
                SYS_ZuiJinXiuGaiRenID = userInfo.Id,
                SYS_ZuiJinXiuGaiShiJian = DateTime.Now
            };
            return exp;
        }
        /// <summary>
        /// 更新SYS处理
        /// </summary>
        protected static void SetUpdateSYSInfo<P, T>(P preEntity, T entity, UserInfoDto currentUser) where T : EntityMetadata, P where P : EntityMetadata
        {
            if (preEntity != null && entity != null && currentUser != null)
            {
                SetBaseSYSInfo(entity, currentUser);
                entity.SYS_ChuangJianRen = preEntity.SYS_ChuangJianRen;
                entity.SYS_ChuangJianRenID = preEntity.SYS_ChuangJianRenID;
                entity.SYS_ChuangJianShiJian = preEntity.SYS_ChuangJianShiJian;
                entity.SYS_XiTongZhuangTai = 0;
            }
        }
        private static void SetBaseSYSInfo<T>(T entity, UserInfoDto userInfo) where T : EntityMetadata
        {
            if (entity != null && userInfo != null)
            {
                entity.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                entity.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                entity.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
            }
        }
        protected static void SetUpdateSYSInfo<P, T>(P preEntity, T entity, UserInfoDtoNew currentUser) where T : EntityMetadata, P where P : EntityMetadata
        {
            if (preEntity != null && entity != null && currentUser != null)
            {
                SetBaseSYSInfo(entity, currentUser);
                entity.SYS_ChuangJianRen = preEntity.SYS_ChuangJianRen;
                entity.SYS_ChuangJianRenID = preEntity.SYS_ChuangJianRenID;
                entity.SYS_ChuangJianShiJian = preEntity.SYS_ChuangJianShiJian;
                entity.SYS_XiTongZhuangTai = 0;
            }
        }
        private static void SetBaseSYSInfo<T>(T entity, UserInfoDtoNew userInfo) where T : EntityMetadata
        {
            if (entity != null && userInfo != null)
            {
                entity.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                entity.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                entity.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
            }
        }
        protected static void SetDeleteSYSInfo<T>(T entity, UserInfoDto userInfo) where T : EntityMetadata
        {
            if (entity != null && userInfo != null)
            {
                entity.SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.作废;
                entity.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                entity.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                entity.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
            }
        }
        protected static void SetDeleteSYSInfo<T>(T entity, UserInfoDtoNew userInfo) where T : EntityMetadata
        {
            if (entity != null && userInfo != null)
            {
                entity.SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.作废;
                entity.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                entity.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                entity.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
            }
        }

        protected static bool VailEmpty<T>(T entity) where T : EntityMetadata
        {
            T model = Activator.CreateInstance<T>();
            var type = typeof(T);
            var modelPropertyInfos = type.GetProperties();//获取T的属性集合
            foreach (var pi in modelPropertyInfos)
            {
                if (pi.Name.Contains("SYS") == false)
                {
                    var value = pi.GetValue(model);
                    if (value != null && string.IsNullOrEmpty(value.ToString()) == true)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        protected static bool VaildRex(string regex, string value)
        {
            Regex reg = new Regex(@regex);
            return reg.IsMatch(value);
        }

        protected static bool IsGuanLiYuanRoleCode(string RoleCodes)
        {
            if (string.IsNullOrWhiteSpace(RoleCodes)) return false;
            //根据权限码来判断权限
            //根据权限达到的话，
            var guanLiYuanJueSe = new string[] {
                    string.Format("{0:000}", (int)ZuZhiJueSe.系统管理员),
                    string.Format("{0:000}", (int)ZuZhiJueSe.组织管理员)
                };
            if (guanLiYuanJueSe.Intersect(RoleCodes.Split(',')).ToList().Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public abstract void Dispose();
    }
}
