using Conwin.Framework.BusinessLogger;
using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.Log4net;
using Conwin.Framework.ServiceAgent.Dtos;
using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Entities.Enums;
using Conwin.GPSDAGL.Entities.Repositories;
using Conwin.GPSDAGL.Services.Common;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.DtosExt.Dtos;
using Conwin.GPSDAGL.Services.Interfaces;
using Conwin.GPSDAGL.Services.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services
{
    public class ZiDingYiJiangKongShuService : ApiServiceBase, IZiDingYiJiangKongShuService
    {
        /// <summary>
        /// 分钟
        /// </summary>
        private static double CheLiangDingWeiZaiXian = CommonHelper.CheLiangZaiXianExpireTime * -1;
        private readonly IZiDingYiCheLiangJianKongShuRepository _ziDingYiCheLiangJianKongShuRepository;
        private readonly IUserJianKongShuRepository _userJianKongShuRepository;

        private readonly ICheLiangRepository _cheLiangXinXiRepository;
        private readonly ICheLiangDingWeiXinXiRepository _cheLiangDingWeiXinXiRepository;


        public ZiDingYiJiangKongShuService(
             IBussinessLogger bussinessLogger,
            IZiDingYiCheLiangJianKongShuRepository ziDingYiCheLiangJianKongShuRepository,
            IUserJianKongShuRepository userJianKongShuRepository,

               ICheLiangRepository cheLiangXinXiRepository,
                     ICheLiangDingWeiXinXiRepository cheLiangDingWeiXinXiRepository


            ) : base(bussinessLogger)
        {
            _ziDingYiCheLiangJianKongShuRepository = ziDingYiCheLiangJianKongShuRepository;
            _userJianKongShuRepository = userJianKongShuRepository;

            _cheLiangXinXiRepository = cheLiangXinXiRepository;
            _cheLiangDingWeiXinXiRepository = cheLiangDingWeiXinXiRepository;

        }

        public override void Dispose() { }

        /// <summary>
        /// 根据ID查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ServiceResult<JSTreeDTO> GetTree(string id)
        {
            var result = new ServiceResult<JSTreeDTO>();
            List<ZiDingYiCheLiangJianKongShu> list = new List<ZiDingYiCheLiangJianKongShu>();
            try
            {
                Guid guid = new Guid(id);
                var resNode = _userJianKongShuRepository.GetQuery(u => u.SYS_XiTongZhuangTai == 0 && u.Id == guid).FirstOrDefault();
                if (resNode == null)
                {
                    result.ErrorMessage = "找不到对应的记录";
                    result.StatusCode = 2;
                    return result;
                }
                Guid nodeid = new Guid(resNode.NodeId);
                var root = _ziDingYiCheLiangJianKongShuRepository.GetQuery(u => u.SYS_XiTongZhuangTai == 0 && u.Id == nodeid).FirstOrDefault();
                list.Add(root);
                if (root != null)
                {
                    GetChild(root.Id, ref list);
                }
                var jstreedto = PraseToJsTreeDto(list);
                result.Data = jstreedto;
                result.StatusCode = 0;
            }
            catch (Exception ex)
            {
                LogHelper.Error("ZiDingYiJiangKongShuService.GetTree", ex);
                result.StatusCode = 2;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 监控树 展示
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public ServiceResult<JSTreeDTO> GetEnabledTreeByUser(UserInfoDto userInfo)
        {
            var result = new ServiceResult<JSTreeDTO>();
            List<ZiDingYiCheLiangJianKongShu> list = new List<ZiDingYiCheLiangJianKongShu>();
            try
            {
                var resNode = _userJianKongShuRepository.GetQuery(u => u.SYS_XiTongZhuangTai == 0 && u.SysUserId == userInfo.ExtUserId && u.Enabled == 1).FirstOrDefault();
                if (resNode == null)
                {
                    result.ErrorMessage = "找不到对应的记录";
                    result.StatusCode = 2;
                    return result;
                }
                Guid nodeid = new Guid(resNode.NodeId);
                var root = _ziDingYiCheLiangJianKongShuRepository.GetQuery(u => u.SYS_XiTongZhuangTai == 0 && u.Id == nodeid).FirstOrDefault();
                list.Add(root);
                if (root != null)
                {
                    GetChild(root.Id, ref list);
                }
                var jstreedto = PraseToJsTreeDto(list);

                AddOnlineCountOfTree(jstreedto);//统计在线数

                result.Data = jstreedto;
                result.StatusCode = 0;
            }
            catch (Exception ex)
            {
                LogHelper.Error("ZiDingYiJiangKongShuService.GetEnabledTreeByUser", ex);
                result.StatusCode = 2;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        //public ServiceResult<string> AddNode(CustomMonitorTreeDto dto)
        //{
        //    var result = new ServiceResult<string>();
        //    List<ZiDingYiCheLiangJianKongShu> list = new List<ZiDingYiCheLiangJianKongShu>();
        //    try
        //    {
        //        ZiDingYiCheLiangJianKongShu entity = new ZiDingYiCheLiangJianKongShu
        //        {
        //            Id = Guid.NewGuid(),
        //            Order = dto.NodeIndex,
        //            NodeName = dto.NodeName,
        //            ParentNodeId = new Guid(dto.ParentNodeId),
        //            IsEnabled = dto.Enabled,
        //            NodeType = 0,
        //            SYS_XiTongZhuangTai = 0,
        //            SYS_ChuangJianShiJian = DateTime.Now
        //        };
        //        int res = 0;
        //        using (var uow = _ziDingYiCheLiangJianKongShuRepository.UnitOfWork)
        //        {
        //            uow.BeginTransaction();
        //            _ziDingYiCheLiangJianKongShuRepository.Add(entity);
        //            res = uow.CommitTransaction();
        //        }
        //        if (res > 0)
        //        {
        //            result.Data = entity.Id.ToString();
        //            result.StatusCode = 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Error("ZiDingYiJiangKongShuService.AddNode", ex);
        //        result.StatusCode = 2;
        //        result.ErrorMessage = ex.Message;
        //    }
        //    return result;
        //}

        public ServiceResult<string> AddTree(CustomMonitorTreeDto dto, UserInfoDto userinfo)
        {
            var result = new ServiceResult<string>();

            try
            {
                List<ZiDingYiCheLiangJianKongShu> list;
                JSTreeDTO rootnode = new JSTreeDTO()
                {
                    text = dto.NodeName,
                    children = dto.TreeNodes
                };
                int index = 1;
                list = JsTreePrase(null, rootnode, userinfo, ref index);
                if (list.Count > 0)
                {
                    using (var uow = _ziDingYiCheLiangJianKongShuRepository.UnitOfWork)
                    {
                        uow.BeginTransaction();
                        _ziDingYiCheLiangJianKongShuRepository.BatchInsert(list.ToArray());
                        uow.CommitTransaction();
                    }

                    UserJianKongShu entity = new UserJianKongShu();
                    entity.Id = Guid.NewGuid();
                    entity.NodeId = list.FirstOrDefault().Id.ToString().ToLower();
                    entity.SysUserId = userinfo.ExtUserId;
                    entity.Enabled = 0; //1开启 0 关闭
                    entity.SYS_ChuangJianRen = userinfo.UserName;
                    entity.SYS_ChuangJianRenID = userinfo.ExtUserId;
                    entity.SYS_ChuangJianShiJian = DateTime.Now;
                    entity.SYS_XiTongZhuangTai = 0;
                    int uowresult = 0;
                    using (var uow = _userJianKongShuRepository.UnitOfWork)
                    {
                        uow.BeginTransaction();

                        _userJianKongShuRepository.Add(entity);
                        uowresult = uow.CommitTransaction();
                    }
                    if (uowresult > 0)
                    {
                        result.Data = entity.NodeId;
                        result.StatusCode = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("AddTree 异常", ex);
                result.ErrorMessage = ex.Message;
                result.StatusCode = 2;
            }

            return result;
        }


        public ServiceResult<QueryResult> GetTreeList(QueryData queryData, UserInfoDto userinfo)
        {
            var result = new ServiceResult<QueryResult>();


            QueryResult queryResult = new QueryResult();

            try
            {
                var userJianKongShuList = _userJianKongShuRepository.GetQuery(u => u.SYS_XiTongZhuangTai == 0 && u.SysUserId == userinfo.ExtUserId).ToList();
                if (userJianKongShuList.Count > 0)
                {
                    var ziDingYiShulist = _ziDingYiCheLiangJianKongShuRepository.GetQuery(u => u.SYS_XiTongZhuangTai == 0 && u.ParentNodeId == null).ToList();


                    var query = from u in userJianKongShuList
                                join z in ziDingYiShulist on u.NodeId equals z.Id.ToString().ToLower() into zz
                                from zidingyi in zz
                                select new CustomMonitorTreeDto
                                {
                                    Id = u.Id.ToString().ToLower(),
                                    NodeId = zidingyi.Id.ToString().ToLower(),
                                    Enabled = u.Enabled.HasValue ? u.Enabled.Value == 1 : false,
                                    NodeName = zidingyi.NodeName
                                };


                    CustomMonitorTreeDto dto = JsonConvert.DeserializeObject<CustomMonitorTreeDto>(queryData.data.ToString());
                    if (dto != null && !string.IsNullOrWhiteSpace(dto.NodeName))
                    {
                        query = query.Where(s => s.NodeName.Contains(dto.NodeName));
                    }
                    queryResult.totalcount = query.Count();
                    var lsit = query.OrderByDescending(s => s.Enabled).Skip((queryData.page - 1) * queryData.rows).Take(queryData.rows).ToList();
                    queryResult.items = lsit;
                    result.StatusCode = 0;
                    result.Data = queryResult;
                }
                else
                {
                    queryResult.totalcount = 0;
                    queryResult.items = new int[0];
                    result.StatusCode = 0;
                    result.Data = queryResult;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("GetTreeList 异常", ex);
                result.ErrorMessage = ex.Message;
                result.StatusCode = 2;
            }

            return result;
        }

        public ServiceResult<bool> EnabledTree(string id, UserInfoDto userinfo)
        {
            var result = new ServiceResult<bool>();
            try
            {
                Guid uguid = new Guid(id);
                var userJianKongShu = _userJianKongShuRepository.GetQuery(u => u.SYS_XiTongZhuangTai == 0 && u.Id == uguid).FirstOrDefault();
                if (userJianKongShu != null)
                {
                    int uowresult = 0;
                    using (var uow = _userJianKongShuRepository.UnitOfWork)
                    {
                        uow.BeginTransaction();

                        Func<UserJianKongShu, UserJianKongShu> func = entity =>
                        {
                            entity.Enabled = 0;
                            entity.SYS_ZuiJinXiuGaiRen = userinfo.UserName;
                            entity.SYS_ZuiJinXiuGaiRenID = userinfo.ExtUserId;
                            entity.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                            return entity;
                        };
                        var list = _userJianKongShuRepository.GetQuery(e => e.SysUserId == userinfo.ExtUserId && e.SYS_XiTongZhuangTai == 0).ToList();
                        foreach (var item in list)
                        {
                            _userJianKongShuRepository.Update(func(item));
                        }
                        userJianKongShu.Enabled = 1;
                        userJianKongShu.SYS_ZuiJinXiuGaiRen = userinfo.UserName;
                        userJianKongShu.SYS_ZuiJinXiuGaiRenID = userinfo.ExtUserId;
                        userJianKongShu.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        _userJianKongShuRepository.Update(userJianKongShu);
                        uowresult = uow.CommitTransaction();
                    }
                    if (uowresult > 0)
                    {
                        result.Data = true;
                        result.StatusCode = 0;
                    }
                }
                else
                {
                    result.Data = false;
                    result.StatusCode = 2;
                    result.ErrorMessage = "没有找到这条记录";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("GetTreeList 异常", ex);
                result.ErrorMessage = ex.Message;
                result.StatusCode = 2;
            }

            return result;
        }

        public ServiceResult<bool> DeleteTree(List<string> ids, UserInfoDto userinfo, string mark = null)
        {
            var result = new ServiceResult<bool>();
            try
            {
                List<Guid> uguid = new List<Guid>();

                foreach (var id in ids)
                {
                    uguid.Add(new Guid(id));
                }

                var list = _userJianKongShuRepository.GetQuery(u => u.SYS_XiTongZhuangTai == 0 && uguid.Contains(u.Id)).ToList();
                if (list.Count > 0)
                {
                    int uowresult = 0;
                    using (var uow = _userJianKongShuRepository.UnitOfWork)
                    {
                        uow.BeginTransaction();
                        foreach (var item in list)
                        {
                            item.SYS_ZuiJinXiuGaiRen = userinfo.UserName;
                            item.SYS_ZuiJinXiuGaiRenID = userinfo.ExtUserId;
                            item.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                            item.SYS_XiTongZhuangTai = 1;
                            item.SYS_XiTongBeiZhu = mark;
                            _userJianKongShuRepository.Update(item);
                        }
                        uowresult = uow.CommitTransaction();
                    }
                    if (uowresult > 0)
                    {
                        result.Data = true;
                        result.StatusCode = 0;
                    }
                }
                else
                {
                    result.Data = false;
                    result.StatusCode = 2;
                    result.ErrorMessage = "没有找到记录";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("DeleteTree 异常", ex);
                result.ErrorMessage = ex.Message;
                result.StatusCode = 2;
            }

            return result;
        }

        public ServiceResult<string> UpdateTree(CustomMonitorTreeDto dto, UserInfoDto userinfo)
        {
            var result = new ServiceResult<string>();
            try
            {
                var userjiankongentity = _userJianKongShuRepository.GetQuery(u => u.SYS_XiTongZhuangTai == 0 && u.NodeId == dto.NodeId).FirstOrDefault();
                if (userjiankongentity != null)
                {
                    List<ZiDingYiCheLiangJianKongShu> list;
                    JSTreeDTO rootnode = new JSTreeDTO()
                    {
                        text = dto.NodeName,
                        children = dto.TreeNodes,
                        data = dto.NodeData,
                    };
                    int index = 1;
                    list = JsTreePrase(null, rootnode, userinfo, ref index);
                    if (list.Count > 0)
                    {
                        DeleteTree(list.Select(s => s.Id.ToString()).ToList(), userinfo, "用户修改创建新树");
                        int uowresult = 0;
                        using (var uow = _ziDingYiCheLiangJianKongShuRepository.UnitOfWork)
                        {
                            uow.BeginTransaction();
                            _ziDingYiCheLiangJianKongShuRepository.BatchInsert(list.ToArray());
                            uow.CommitTransaction();
                        }

                        userjiankongentity.NodeId = list.FirstOrDefault().Id.ToString().ToLower();
                        userjiankongentity.SYS_ZuiJinXiuGaiRen = userinfo.UserName;
                        userjiankongentity.SYS_ZuiJinXiuGaiRenID = userinfo.ExtUserId;
                        userjiankongentity.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        using (var uow = _userJianKongShuRepository.UnitOfWork)
                        {
                            uow.BeginTransaction();
                            _userJianKongShuRepository.Update(userjiankongentity);
                            uowresult = uow.CommitTransaction();
                        }
                        if (uowresult > 0)
                        {
                            result.Data = userjiankongentity.NodeId;
                            result.StatusCode = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("UpdateTree 异常", ex);
                result.ErrorMessage = ex.Message;
                result.StatusCode = 2;
            }

            return result;
        }


        private void GetOnlineCount(JSTreeNodeData nodedata, UserInfoDto userInfo = null)
        {
            //var query = _RYFPJKCLSSOrgJiLuRepository.GetQuery(s=>s.OrgCode == nodedata.OrgCode && s.SYS_XiTongZhuangTai ==0 && s.SysUserId == null);

            //var onlineQuery = _RYFPJKCLSSOrgJiLuRepository.GetQuery(s => s.OrgCode == nodedata.OrgCode && s.SYS_XiTongZhuangTai == 0 && s.SysUserId == null && s.ShiFouZaiXian==1);


            DateTime compareTime = DateTime.Now.AddMinutes(CheLiangDingWeiZaiXian);
            var query = from a in _cheLiangXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && p.YeHuOrgType == (int)OrganizationType.企业 && p.YeHuOrgCode == nodedata.OrgCode)
                        join b in _cheLiangDingWeiXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                        on new { a.ChePaiHao, a.ChePaiYanSe } equals new { ChePaiHao = b.RegistrationNo, ChePaiYanSe = b.RegistrationNoColor } into c
                        from d in c.DefaultIfEmpty()
                        select new
                        {
                            ShiFouZaiXian = (d.LatestGpsTime ?? DateTime.MinValue) >= compareTime
                        };


            nodedata.CarCount = query.Count();
            nodedata.CarOnlineCount = query.Count(l => l.ShiFouZaiXian == true);
        }

        private void AddOnlineCountOfTree(JSTreeDTO treenode)
        {
            JSTreeNodeData data;
            if (treenode.data != null)
            {
                data = JsonConvert.DeserializeObject<JSTreeNodeData>(treenode.data.ToString());
            }
            else
            {
                data = new JSTreeNodeData();
            }
            if (data != null && !string.IsNullOrWhiteSpace(data.OrgCode))
            {
                GetOnlineCount(data);
            }
            if (treenode.children != null && treenode.children.Count > 0)
            {
                foreach (var item in treenode.children)
                {
                    AddOnlineCountOfTree(item);
                    if (item.data != null)
                    {
                        JSTreeNodeData ndata = item.data as JSTreeNodeData;
                        if (ndata != null)
                        {
                            data.CarCount += ndata.CarCount;
                            data.CarOnlineCount += ndata.CarOnlineCount;
                        }
                    }
                }
            }
            treenode.text += $"({data.CarOnlineCount}/{data.CarCount})";
            treenode.data = data;
        }



        #region 
        private List<ZiDingYiCheLiangJianKongShu> JsTreePrase(Guid? parentID, JSTreeDTO dto, UserInfoDto userinfo, ref int order)
        {
            List<ZiDingYiCheLiangJianKongShu> list = new List<ZiDingYiCheLiangJianKongShu>();

            var entity = new ZiDingYiCheLiangJianKongShu()
            {
                Id = Guid.NewGuid(),
                NodeName = dto.text,
                NodeType = 0,
                ParentNodeId = parentID,
                IsEnabled = true,
                ChuangJianRenOrgCode = userinfo.OrganizationCode,
                Order = order,
                SYS_ChuangJianRen = userinfo.UserName,
                SYS_ChuangJianRenID = userinfo.ExtUserId,
                SYS_ChuangJianShiJian = DateTime.Now,
                SYS_XiTongZhuangTai = 0
            };
            if (dto.data != null)
            {
                entity.NodeData = JsonConvert.SerializeObject(dto.data);
                entity.NodeType = 1;
            }


            list.Add(entity);
            if (dto.children != null && dto.children.Count > 0)
            {
                int n_order = 1;
                foreach (var item in dto.children)
                {
                    var temp = JsTreePrase(entity.Id, item, userinfo, ref n_order);
                    list.AddRange(temp);
                }
            }
            order++;
            return list;
        }


        private JSTreeDTO PraseToJsTreeDto(List<ZiDingYiCheLiangJianKongShu> scrList)
        {
            var rootdto = scrList.First();
            JSTreeDTO root = new JSTreeDTO
            {
                id = rootdto.Id.ToString().ToLower(),
                text = rootdto.NodeName,
            };
            SetJsTreeDtoChild(scrList, root);
            return root;
        }

        private void SetJsTreeDtoChild(List<ZiDingYiCheLiangJianKongShu> scrList, JSTreeDTO dto)
        {
            Guid id = new Guid(dto.id);
            dto.children = scrList.Where(s => s.ParentNodeId == id).OrderBy(s => s.Order).Select(s => new JSTreeDTO
            {
                id = s.Id.ToString().ToLower(),
                text = s.NodeName,
                data = s.NodeData,
                children = new List<JSTreeDTO>()
            }).ToList();
            foreach (var item in dto.children)
            {
                if (item.data != null)
                {
                    item.data = JsonConvert.DeserializeObject(item.data);
                }
            }
            if (dto.children != null && dto.children.Count > 0)
            {
                foreach (var child in dto.children)
                {
                    SetJsTreeDtoChild(scrList, child);
                }
            }
        }

        private void GetChild(Guid parentid, ref List<ZiDingYiCheLiangJianKongShu> list)
        {
            var templist = _ziDingYiCheLiangJianKongShuRepository.GetQuery(u => u.SYS_XiTongZhuangTai == 0 && u.ParentNodeId == parentid).ToList();
            if (templist != null)
            {
                foreach (var item in templist)
                {
                    list.Add(item);
                    GetChild(item.Id, ref list);
                }
            }
        }

        #endregion
    }
}
