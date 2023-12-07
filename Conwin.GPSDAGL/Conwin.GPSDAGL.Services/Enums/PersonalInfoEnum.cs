namespace Conwin.GPSDAGL.Services.Enums
{

    /// <summary>
    /// 1=待确认，
    /// 2=在职，
    /// 3=离职
    /// </summary>
    public enum PersonalInfoStatusType
    {
        WaitConfirm = 1,
        Work = 2,
        Leave = 3
    }

    /// <summary>
    /// 动态获取人员信息业务类型
    /// 0 = 驾驶员
    /// </summary>
    public enum PersonalInfoBusinessType
    {
        Driver = 0
    }

    public enum PersonalInfoUpdateType
    {
        Update = 0,
        Delete = 1,
        Dismiss = 2,
        Confirm = 3
    }
}
