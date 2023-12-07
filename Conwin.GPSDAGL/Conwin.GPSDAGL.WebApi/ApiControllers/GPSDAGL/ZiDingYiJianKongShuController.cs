using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.Log4net;
using Conwin.Framework.ServiceAgent.Attributes;
using Conwin.Framework.ServiceAgent.BaseClasses;
using Conwin.Framework.ServiceAgent.Utilities;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.DtosExt.Dtos;
using Conwin.GPSDAGL.Services.Interfaces;
using Conwin.GPSDAGL.Services.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Conwin.GPSDAGL.WebApi.ApiControllers.GPSDAGL
{
    [ApiPrefix(typeof(ZiDingYiJianKongShuController))]
    public class ZiDingYiJianKongShuController : BaseApiController
    {

        private readonly IZiDingYiJiangKongShuService _ziDingYiJiangKongShuService;
        public ZiDingYiJianKongShuController(IZiDingYiJiangKongShuService _ziDingYiJiangKongShuService)
        {
            this._ziDingYiJiangKongShuService = _ziDingYiJiangKongShuService;
        }

        /// <summary>
        /// 003300300410
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddTree")]
        public object AddTree([FromBody]string requestString)
        {
            CustomMonitorTreeDto dto;
            try
            {
                if (UserInfo == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "用户信息过期" };
                }
                dto = CWRequestParam.GetBody<CustomMonitorTreeDto>();
                return _ziDingYiJiangKongShuService.AddTree(dto, UserInfo);
                //dto = CWRequestParam.GetBody<CustomMonitorTreeDto>();
                //return _ziDingYiJiangKongShuService.AddTree(dto, new Conwin.Framework.ServiceAgent.Dtos.UserInfoDto() { ExtUserId = "3bc01061-6f7b-4b8b-8273-0a204b5577fd" });
            }
            catch(Exception ex)
            {
                LogHelper.Error("ZiDingYiJianKongShuController.AddTree ",ex);
                return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// 003300300411
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetTreeList")]
        public object GetTreeList([FromBody]string requestString)
        {
            QueryData dto;
            try
            {
                if (UserInfo == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "用户信息过期" };
                }
                dto = CWRequestParam.GetBody<QueryData>();
                return _ziDingYiJiangKongShuService.GetTreeList(dto, UserInfo);
            }
            catch (Exception ex)
            {
                LogHelper.Error("ZiDingYiJianKongShuController.GetTreeList ", ex);
                return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// 003300300413
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EnabledTree")]
        public object EnabledTree([FromBody]string requestString)
        {
            string dto;
            try
            {
                if (UserInfo == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "用户信息过期" };
                }
                dto = CWRequestParam.body.ToString();
                return _ziDingYiJiangKongShuService.EnabledTree(dto, UserInfo);
            }
            catch (Exception ex)
            {
                LogHelper.Error("ZiDingYiJianKongShuController.EnabledTree ", ex);
                return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// 003300300412
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteTree")]
        public object DeleteTree([FromBody]string requestString)
        {
            List<string> dto;
            try
            {
                if (UserInfo == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "用户信息过期" };
                }
                dto = CWRequestParam.GetBody<List<string>>();
                return _ziDingYiJiangKongShuService.DeleteTree(dto, UserInfo);
            }
            catch (Exception ex)
            {
                LogHelper.Error("ZiDingYiJianKongShuController.DeleteTree ", ex);
                return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// 003300300414
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetTree")]
        public object GetTree([FromBody]string requestString)
        {
            string dto;
            try
            {
                if (UserInfo == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "用户信息过期" };
                }
                dto = CWRequestParam.body.ToString();
                return _ziDingYiJiangKongShuService.GetTree(dto);
            }
            catch (Exception ex)
            {
                LogHelper.Error("ZiDingYiJianKongShuController.GetTree ", ex);
                return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// 003300300415
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateTree")]
        public object UpdateTree([FromBody]string requestString)
        {
            CustomMonitorTreeDto dto;
            try
            {
                if (UserInfo == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "用户信息过期" };
                }
                dto = CWRequestParam.GetBody<CustomMonitorTreeDto>();
                return _ziDingYiJiangKongShuService.UpdateTree(dto, UserInfo);
            }
            catch (Exception ex)
            {
                LogHelper.Error("ZiDingYiJianKongShuController.GetTree ", ex);
                return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = ex.Message };
            }
        }

        [HttpPost]
        [Route("GetEnabledTreeByUser")]
        public object GetEnabledTreeByUser([FromBody]string requestString)
        {
            CustomMonitorTreeDto dto;
            try
            {
                if (UserInfo == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "用户信息过期" };
                }
                dto = CWRequestParam.GetBody<CustomMonitorTreeDto>();
                return _ziDingYiJiangKongShuService.GetEnabledTreeByUser(UserInfo);

                //return _ziDingYiJiangKongShuService.GetEnabledTreeByUser(new Conwin.Framework.ServiceAgent.Dtos.UserInfoDto() { ExtUserId = "3bc01061-6f7b-4b8b-8273-0a204b5577fd" });
            }
            catch (Exception ex)
            {
                LogHelper.Error("ZiDingYiJianKongShuController.GetEnabledTreeByUser ", ex);
                return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = ex.Message };
            }
        }



        //[HttpPost]
        //[Route("GetCarByOrgCodes")]
        //public object GetCarByOrgCodes([FromBody]string requestString)
        //{
        //    QueryData dto;
        //    try
        //    {
        //        //if (UserInfo == null)
        //        //{
        //        //    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "用户信息过期" };
        //        //}
        //        dto = CWRequestParam.GetBody<QueryData>();
        //        return _GPSCheLiangJianKongService.QueryXiaJiCheLiangByOrgCodes(dto);

        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Error("ZiDingYiJianKongShuController.GetCarByOrgCodes ", ex);
        //        return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = ex.Message };
        //    }
        //}

    }
}