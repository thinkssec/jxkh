using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Enterprise.Component.Infrastructure
{
    /// <summary>
    /// 页面常量类
    /// </summary>
    public class WebKeys
    {

        #region 缓存相关项

        /// <summary>
        /// 缓存过期时间（分钟）
        /// </summary>
        public static readonly int CacheTimeout = 
            ((ConfigurationManager.AppSettings["CacheDuration"] != null) ?
            int.Parse(ConfigurationManager.AppSettings["CacheDuration"]) : 20);

        /// <summary>
        /// 是否启用缓存
        /// </summary>
        public static readonly bool EnableCaching =
            ((ConfigurationManager.AppSettings["EnableCaching"] != null) ?
            bool.Parse(ConfigurationManager.AppSettings["EnableCaching"]) : false);

        /// <summary>
        /// 数据表监控周期（秒）
        /// </summary>
        public static readonly int MonitorInterval =
            ((ConfigurationManager.AppSettings["MonitorInterval"] != null) ?
            int.Parse(ConfigurationManager.AppSettings["MonitorInterval"]) : 10);

        /// <summary>
        /// 缓存项名称--定时检测名称
        /// </summary>
        public static readonly string CacheItemKey = "SSEC_CacheItemKey_";


        #endregion


        #region 业务流设计器相关项

        /// <summary>
        /// 暂存数据用session名称
        /// </summary>
        public static readonly string SSEC_BF_SESSION_KEY = "ssec_bf_sessionKey";
        /// <summary>
        /// 业务流前缀
        /// </summary>
        public static readonly string BF_PREFIX = "BF";
        /// <summary>
        /// 业务流消息前缀
        /// </summary>
        public static readonly string BF_MESSAGE_PREFIX = "M";
        /// <summary>
        /// 业务流角色获取方法表前缀
        /// </summary>
        public static readonly string BF_CLSMETHOD_PREFIX = "MD";
        /// <summary>
        /// 业务流节点前缀
        /// </summary>
        public static readonly string BF_NODE_PREFIX = "N";
        /// <summary>
        /// 业务流实例前缀
        /// </summary>
        public static readonly string BF_INSTANCE_PERFIX = "P";
        /// <summary>
        /// 业务流节点运行前缀
        /// </summary>
        public static readonly string BF_RUN_PERFIX = "R";

        #endregion


        #region 错误相关项

        public static readonly string ErrorMessage = "ErrorMessage";

        public static readonly string Error = "Error";

        #endregion


        #region 关键项

        /// <summary>
        /// 语言名称标识
        /// </summary>
        public static readonly string LangName = "SSEC_Language";

        /// <summary>
        /// 语言转换词典缓存项名称
        /// </summary>
        public static readonly string LangCacheName = "SSEC_MyDictionary";

        /// <summary>
        /// 原始文本名称
        /// </summary>
        public static readonly string OriginalText = "SSEC_OriginalText";

        #endregion


        #region Cookie相关

        /// <summary>
        /// 缺省语言类型Cookie名称
        /// </summary>
        public static readonly string LangCookieName = "DefaultLangType";

        #endregion


        #region 动作标识

        /// <summary>
        /// 添加操作
        /// </summary>
        public static readonly string InsertAction = "Insert";
        /// <summary>
        /// 更新操作
        /// </summary>
        public static readonly string UpdateAction = "Update";
        /// <summary>
        /// 添加或更新操作
        /// </summary>
        public static readonly string InsertOrUpdateAction = "InsertOrUpdate";
        /// <summary>
        /// 删除操作
        /// </summary>
        public static readonly string DeleteAction = "Delete";
        /// <summary>
        /// 整体删除操作
        /// </summary>
        public static readonly string DeleteAllAction = "DeleteAll";
        /// <summary>
        /// 刷新操作
        /// </summary>
        public static readonly string RefreshAction = "Refresh";

        #endregion


        #region 项目专用标识

        /// <summary>
        /// 数据库用户名称
        /// </summary>
        public static readonly string DATABASE_USERNAME = "HUADONG";

        /// <summary>
        /// 项目前缀
        /// </summary>
        public static readonly string PROJ_PREFIX = "PT";

        /// <summary>
        /// 中心项目审核机构（生产办）
        /// </summary>
        public static readonly int ProjectAuditDeptId = 
            ((ConfigurationManager.AppSettings["ProjectAuditDeptId"] != null) ?
            int.Parse(ConfigurationManager.AppSettings["ProjectAuditDeptId"]) : 0);

        /// <summary>
        /// 报告导出路径
        /// </summary>
        public static readonly string ProjectReportPath = 
            ConfigurationManager.AppSettings["ProjectReportPath"].ToRequestString();

        #endregion

        #region 操作权限相关

        /// <summary>
        /// 用户菜单风络
        /// </summary>
        public enum MenuType
        {
            树型菜单 = 1,
            折叠菜单 = 2
        }

        /// <summary>
        /// 权限类型
        /// </summary>
        public enum PermissionType
        {
            /// <summary>
            /// 浏览
            /// </summary>
            View = 1,
            /// <summary>
            /// 添加
            /// </summary>
            Ins = 2,
            /// <summary>
            /// 编辑
            /// </summary>
            Upd = 3,
            /// <summary>
            /// 删除
            /// </summary>
            Del = 4,
            /// <summary>
            /// 报表导出
            /// </summary>
            Rpt = 5,
            /// <summary>
            /// 打印
            /// </summary>
            Prt = 6,
            /// <summary>
            /// 审核
            /// </summary>
            Audit = 7
        }

        #endregion

        #region 考核系统相关

        /// <summary>
        /// 考核对象类型
        /// </summary>
        public enum KaoheType
        {
            二级单位 = 1,
            领导班子 = 2,
            机关部门 = 3,
            部门负责人 = 4
        }

        /// <summary>
        /// 打分用户类型
        /// </summary>
        public enum DFUserType
        {
            上级领导 = 1,
            同级部门 = 2,
            同级二级单位 = 3
        }

        /// <summary>
        /// 二级单位考核节点
        /// </summary>
        public enum EjdwKhNode
        {
            绩效责任书,
            目标值确认,
            完成值录入,
            约束和加分指标打分,
            领导班子打分,
            完成值和得分审核,
            考核结果审定
        }

        /// <summary>
        /// 机关部门考核节点
        /// </summary>
        public enum JgbmKhNode
        {
            绩效责任书,
            自评打分完成,
            同级打分完成,
            上级打分完成,
            连带指标打分,
            考核结果审定
        }

        #endregion

    }
}