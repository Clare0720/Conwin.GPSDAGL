using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.Log4net;
using Conwin.Framework.ServiceAgent.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Framework
{
    public class CWHelper
    {
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


        public static dynamic HttpResponseModel(string sysId, string serviceCode, string serviceVersion, object bodyContent, out string errorMessage)
        {
            var request = GenerateRequest(sysId, serviceCode, serviceVersion, bodyContent);
            errorMessage = "";
            try
            {
                var responseString = ServiceAgentUtility.Send(request);
                CWResponse response = ServiceAgentUtility.DeserializeResponse(responseString);
                if (response.publicresponse.statuscode != 0 || response.body == null)
                {
                    errorMessage = serviceCode + "返回失败。\r\n请求报文：" + JsonConvert.SerializeObject(request);
                    errorMessage += "\r\n响应报文：" + responseString;
                }

                return response.body;
            }
            catch (Exception ex)
            {
                errorMessage = serviceCode + "接口请求异常：" + ex;
                errorMessage += "\r\n请求报文：" + JsonConvert.SerializeObject(request);
                return null;
            }
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

        public static CWResponse HttpResponse(string sysId, string serviceCode, string serviceVersion, object bodyContent)
        {
            var request = GenerateRequest(sysId, serviceCode, serviceVersion, bodyContent);
            try
            {
                var responseString = ServiceAgentUtility.Send(request);
                CWResponse response = ServiceAgentUtility.DeserializeResponse(responseString);

                return response;
            }
            catch (Exception ex)
            {
                return new CWResponse
                {
                    publicresponse = new CWPublicResponse()
                    {
                        sysid = sysId,
                        servicecode = serviceCode,
                        servicever = serviceVersion,
                        statuscode = 2,
                        message = $"请求失败：{ex.Message}"
                    }
                };
            }
        }

        /// <summary>
        /// 异常重试机制
        /// </summary>
        /// <param name="action">需要重试的方法</param>
        /// <param name="maxTryCount">重试次数</param>
        /// <param name="interval">间隔时长，单位秒</param>
        public static void Retry(Action action, int maxTryCount = 1, int interval = 1 * 1000)
        {
            bool isSuccess = false;
            var tryCount = 0;
            var outException = default(Exception);

            while (tryCount < maxTryCount)
            {
                try
                {
                    action();
                    isSuccess = true;
                    break;
                }
                catch (Exception exception)
                {
                    outException = exception;
                    tryCount++;
                    Thread.Sleep(interval);
                }
            }

            if (!isSuccess)
            {
                if (outException != null)
                {
                    throw outException;
                }
                else
                {
                    throw new Exception("尝试执行失败！");
                }
            }
        }

        /// <summary>
        /// 异常重试机制
        /// </summary>
        /// <typeparam name="T">需要重试的方法的返回值类型</typeparam>
        /// <param name="func">需要重试的方法</param>
        /// <param name="maxTryCount">重试次数</param>
        /// <param name="interval">间隔时长，单位秒</param>
        /// <returns>需要重试的方法的返回值</returns>
        public static TResult Retry<TResult>(Func<TResult> func, int maxTryCount = 3, int interval = 1 * 1000)
        {
            TResult result = default(TResult);

            bool isSuccess = false;
            var tryCount = 0;
            var outException = default(Exception);

            while (tryCount < maxTryCount)
            {
                try
                {
                    result = func();
                    isSuccess = true;
                    if (isSuccess)
                    {
                        break;
                    }
                    else
                    {
                        tryCount++;
                        Thread.Sleep(interval);
                    }

                }
                catch (Exception exception)
                {
                    outException = exception;
                    tryCount++;
                    Thread.Sleep(interval);
                }
            }
            if (!isSuccess)
            {
                if (outException != null)
                {
                    throw outException;
                }
                else
                {
                    throw new Exception("尝试执行失败！");
                }
            }

            return result;
        }

        public static TResult Retry<T, TResult>(Func<T, TResult> func, T funcArgs, int maxTryCount = 3, int interval = 1 * 1000)
        {
            TResult result = default(TResult);

            bool isSuccess = false;
            var tryCount = 0;
            var outException = default(Exception);

            while (tryCount < maxTryCount)
            {
                try
                {
                    result = func(funcArgs);
                    isSuccess = true;
                    if (isSuccess)
                    {
                        break;
                    }
                    else
                    {
                        tryCount++;
                        Thread.Sleep(interval);
                    }

                }
                catch (Exception exception)
                {
                    outException = exception;
                    tryCount++;
                    Thread.Sleep(interval);
                }
            }

            if (!isSuccess)
            {
                if (outException != null)
                {
                    throw outException;
                }
                else
                {
                    throw new Exception("尝试执行失败！");
                }
            }

            return result;
        }
        /// <summary>
        /// 获取字符串锁
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        public static string GetStringLock(params string[] strs)
        {
            var key = string.Join("_", strs.ToList());
            return string.Intern("StrLock:" + key.ToLower());
        }
    }
}
