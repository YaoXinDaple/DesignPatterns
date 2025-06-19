using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuilderPatternRealWorldUsage
{
    /// <summary>
    /// 自定义 claim 的声明
    /// </summary>
    public static class CustomClaims
    {
        /// <summary>
        /// 创建企业的用户默认被赋予的角色【开票员】
        /// </summary>
        public const int IssuerRoleId = 10010;

        /// <summary>
        /// 角色【查看】的RoleId
        /// </summary>
        public const int ViewerRoleId = 10011;

        /// <summary>
        /// 首页
        /// </summary>
        public const string MainPage = "首页";

        /// <summary>
        /// 首页权限的Id
        /// </summary>
        public const int MainPagePermissionId = 90001;

        /// <summary>
        /// 开票管理
        /// </summary>
        public const string InvoiceIssueManagement = "开票管理";

        /// <summary>
        /// 红票管理
        /// </summary>
        public const string RedInvoiceManagement = "红票管理";

        /// <summary>
        /// 查询统计
        /// </summary>
        public const string Statistics = "查询统计";

        /// <summary>
        /// 客户信息
        /// </summary>
        public const string CustomerManagement = "客户信息";

        /// <summary>
        /// 项目管理
        /// </summary>
        public const string InvoiceItemManagement = "项目信息";

        /// <summary>
        /// 附加信息
        /// </summary>
        public const string AdditionalInformationManagement = "附加信息";

        /// <summary>
        /// 设置
        /// </summary>
        public const string SettingManagement = "设置";

        /// <summary>
        /// 管理员权限（不作为角色或权限，仅对应UserCompany中的IsAdministrator字段）
        /// </summary>
        public const string Administrator = "Administrator";

        /// <summary>
        /// 菜单项 key到菜单项的映射
        /// </summary>
        public static FrozenDictionary<string, string> MenuCodeToNameDic => new Dictionary<string, string>
        {
            { nameof(MainPage),"首页" },
            { nameof(InvoiceIssueManagement) , "开票管理" },
            { nameof(RedInvoiceManagement) , "红票管理" },
            { nameof(Statistics) , "查询统计" },
            { nameof(CustomerManagement) , "客户信息" },
            { nameof(InvoiceItemManagement) , "项目管理" },
            { nameof(AdditionalInformationManagement) , "附加信息" },
            { nameof(SettingManagement) , "设置" }
        }.ToFrozenDictionary();

        /// <summary>
        /// 权限Id到菜单项 key的映射
        /// </summary>
        public static FrozenDictionary<int, string> PermissionIdToMenuCodeDic => new Dictionary<int, string>
        {
            { MainPagePermissionId, nameof(MainPage) },
            { 90002, nameof(InvoiceIssueManagement)  },
            { 90003, nameof(RedInvoiceManagement)  },
            { 90004, nameof(Statistics) },
            { 90005, nameof(CustomerManagement) },
            { 90006, nameof(InvoiceItemManagement)  },
            { 90007, nameof(AdditionalInformationManagement) },
            { 90008, nameof(SettingManagement) }
        }.ToFrozenDictionary();

        /// <summary>
        /// 获取企业的初始角色及角色对应权限
        /// </summary>
        /// <returns></returns>
        public static InitRole[] GetInitRoles()
        {
            return [
                new InitRole
                {
                    Id = IssuerRoleId,
                    Name = "开票员",
                    Permissions = [
                        new InitPermission
                        {
                            Id = 90001,
                            Code = PermissionIdToMenuCodeDic[90001],
                            Name = MenuCodeToNameDic[PermissionIdToMenuCodeDic[90001]]
                        },
                        new InitPermission
                        {
                            Id = 90002,
                            Code = PermissionIdToMenuCodeDic[90002],
                            Name = MenuCodeToNameDic[PermissionIdToMenuCodeDic[90002]]
                        },
                        new InitPermission
                        {
                            Id = 90003,
                            Code = PermissionIdToMenuCodeDic[90003],
                            Name = MenuCodeToNameDic[PermissionIdToMenuCodeDic[90003]]
                        },
                        new InitPermission
                        {
                            Id = 90004,
                            Code = PermissionIdToMenuCodeDic[90004],
                            Name = MenuCodeToNameDic[PermissionIdToMenuCodeDic[90004]]
                        },
                        new InitPermission
                        {
                            Id = 90005,
                            Code = PermissionIdToMenuCodeDic[90005],
                            Name = MenuCodeToNameDic[PermissionIdToMenuCodeDic[90005]]
                        },
                        new InitPermission
                        {
                            Id = 90006,
                            Code = PermissionIdToMenuCodeDic[90006],
                            Name = MenuCodeToNameDic[PermissionIdToMenuCodeDic[90006]]
                        },
                        new InitPermission
                        {
                            Id = 90007,
                            Code = PermissionIdToMenuCodeDic[90007],
                            Name = MenuCodeToNameDic[PermissionIdToMenuCodeDic[90007]]
                        },
                        new InitPermission
                        {
                            Id = 90008,
                            Code = PermissionIdToMenuCodeDic[90008],
                            Name = MenuCodeToNameDic[PermissionIdToMenuCodeDic[90008]]
                        }
                    ]
                },
                new InitRole
                {
                    Id = ViewerRoleId,
                    Name = "查看",
                    Permissions = [
                        new InitPermission
                        {
                            Id = 90001,
                            Code = PermissionIdToMenuCodeDic[90001],
                            Name = MenuCodeToNameDic[PermissionIdToMenuCodeDic[90001]]
                        },
                        new InitPermission
                        {
                            Id = 90004,
                            Code = PermissionIdToMenuCodeDic[90004],
                            Name = MenuCodeToNameDic[PermissionIdToMenuCodeDic[90004]]
                        },
                        new InitPermission
                        {
                            Id = 90005,
                            Code = PermissionIdToMenuCodeDic[90005],
                            Name = MenuCodeToNameDic[PermissionIdToMenuCodeDic[90005]]
                        },
                        new InitPermission
                        {
                            Id = 90006,
                            Code = PermissionIdToMenuCodeDic[90006],
                            Name = MenuCodeToNameDic[PermissionIdToMenuCodeDic[90006]]
                        },
                        new InitPermission
                        {
                            Id = 90008,
                            Code = PermissionIdToMenuCodeDic[90008],
                            Name = MenuCodeToNameDic[PermissionIdToMenuCodeDic[90008]]
                        }
                    ]
                }
            ];
        }

        public static InitPermission[] GetInitPermissions()
        {
            return GetInitRoles().SelectMany(r => r.Permissions).Distinct().ToArray();
        }

        /// <summary>
        /// 初始角色
        /// </summary>
        public struct InitRole
        {
            public int Id;
            public string Name;
            public string Code;
            public InitPermission[] Permissions;
        }

        /// <summary>
        /// 初始权限
        /// </summary>
        public struct InitPermission
        {
            public int Id;
            public string Name;
            public string Code;
        }

    }
}
