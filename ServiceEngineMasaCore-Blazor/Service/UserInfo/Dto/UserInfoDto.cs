namespace ServiceEngineMasaCore.Blazor.Service.UserInfo.Dto
{
    public class UserInfoDto
    {
        /// <summary>
        /// 账号名称
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 个人简介
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 电子签名
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// 机构Id
        /// </summary>
        public long OrgId { get; set; }

        /// <summary>
        /// 机构名称
        /// </summary>
        public string OrgName { get; set; }

        /// <summary>
        /// 职位名称
        /// </summary>
        public string PosName { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public List<string> Roles { get; set; }
        /// <summary>
        /// 按钮权限集合
        /// </summary>
        public List<string> AuthBtnList { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public string LoginTime { get; set; }
    }
}
