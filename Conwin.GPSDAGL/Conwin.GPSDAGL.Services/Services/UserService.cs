using Conwin.EntityFramework.Extensions;
using Conwin.Framework.CommunicationProtocol;
using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Entities.Enums;
using Conwin.GPSDAGL.Entities.Repositories;
using Conwin.GPSDAGL.Services.Common;
using Conwin.GPSDAGL.Services.DtosExt.User;
using Conwin.GPSDAGL.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Services
{
    public partial class UserService : IUserService
    {
        private readonly IOrgBaseInfoRepository _orgBaseInfoRepository;

        public UserService(
            IOrgBaseInfoRepository orgBaseInfoRepository)
        {
            _orgBaseInfoRepository = orgBaseInfoRepository;
        }

        public ServiceResult<BindAccountDto> GetUserWechatOpenIdByOrgCode(OrgInfoDto orgInfoDto)
        {
            var result = new ServiceResult<BindAccountDto>();
            if (orgInfoDto == null)
            {
                result.ErrorMessage = "参数必填";
                result.StatusCode = 2;
            }
            if (string.IsNullOrWhiteSpace(orgInfoDto.SysId))
            {
                result.ErrorMessage = $"参数{nameof(orgInfoDto.SysId)}必填";
                result.StatusCode = 2;
            }

            if (orgInfoDto.WechatSysIds == null || orgInfoDto.WechatSysIds.Count < 0)
            {
                result.ErrorMessage = $"参数{nameof(orgInfoDto.WechatSysIds)}必填";
                result.StatusCode = 2;
            }

            if (orgInfoDto.OrgCodes == null || orgInfoDto.OrgCodes.Count < 0)
            {
                result.ErrorMessage = $"参数{nameof(orgInfoDto.OrgCodes)}必填";
                result.StatusCode = 2;
            }

            if (result.StatusCode != 0)
            {
                return result;
            }

            var bindAccountResult = CommonHelper.HttpResponseModel<BindAccountDto>(orgInfoDto.SysId, "00000030045", "1.0", new { SysId = orgInfoDto.SysId, OrgCode = orgInfoDto.OrgCodes, TargetSysId = orgInfoDto.WechatSysIds });

            return bindAccountResult;
        }

        public ServiceResult<BindAccountDto> GetUserWechatOpenIdByYeHu(QueryWechatOpenIdByYeHuDto yeHuDto)
        {
            var result = new ServiceResult<BindAccountDto>();
            if (yeHuDto == null)
            {
                result.ErrorMessage = "参数必填";
                result.StatusCode = 2;
            }
            if (string.IsNullOrWhiteSpace(yeHuDto.SysId))
            {
                result.ErrorMessage = $"参数{nameof(yeHuDto.SysId)}必填";
                result.StatusCode = 2;
            }

            if (yeHuDto.WechatSysIds == null || yeHuDto.WechatSysIds.Count < 0)
            {
                result.ErrorMessage = $"参数{nameof(yeHuDto.WechatSysIds)}必填";
                result.StatusCode = 2;
            }

            if (string.IsNullOrEmpty(yeHuDto.YeHuDaiMa))
            {
                result.ErrorMessage = $"参数{nameof(yeHuDto.YeHuDaiMa)}必填";
                result.StatusCode = 2;
            }

            if (result.StatusCode != 0)
            {
                return result;
            }

            //根据业户，找到其所在辖区市、县主管部门组织代码

            var yeHuModel = _orgBaseInfoRepository.FindOne(s => s.OrgCode == yeHuDto.YeHuDaiMa.Trim() && s.OrgType == (int)OrganizationType.企业 && s.SYS_XiTongZhuangTai == 0);
            if (yeHuModel == null)
            {
                result.ErrorMessage = $"业户不存在";
                result.StatusCode = 2;
                return result;
            }

            Expression<Func<OrgBaseInfo, bool>> OrgBaseExp = q => q.SYS_XiTongZhuangTai == 0;

            Expression<Func<OrgBaseInfo, bool>> OrgBaseExp2 = null;

            if (!string.IsNullOrWhiteSpace(yeHuModel.XiaQuShi))
            {
                OrgBaseExp2 = u => u.OrgType == (int)OrganizationType.市政府 && u.XiaQuShi == yeHuModel.XiaQuShi.Trim();
            }

            if (!string.IsNullOrWhiteSpace(yeHuModel.XiaQuXian))
            {
                if (OrgBaseExp2 != null)
                {
                    OrgBaseExp2 = OrgBaseExp2.Or(u => u.OrgType == (int)OrganizationType.县政府 && u.XiaQuXian == yeHuModel.XiaQuXian.Trim());
                }
                else
                {
                    OrgBaseExp2 = u => u.OrgType == (int)OrganizationType.县政府 && u.XiaQuXian == yeHuModel.XiaQuXian.Trim();
                }
            }

            if (OrgBaseExp2 != null)
            {
                OrgBaseExp = OrgBaseExp.And(OrgBaseExp2);
            }

            var query = _orgBaseInfoRepository.GetQuery(OrgBaseExp);

            var orgCodeList = query.Select(s => s.OrgCode).ToList();
            orgCodeList.Add(yeHuModel.OrgCode);

            OrgInfoDto orgInfoDto = new OrgInfoDto
            {
                SysId = yeHuDto.SysId,
                WechatSysIds = yeHuDto.WechatSysIds,
                OrgCodes = orgCodeList
            };

            return this.GetUserWechatOpenIdByOrgCode(orgInfoDto);
        }


        public void Dispose()
        {

        }
    }
}
