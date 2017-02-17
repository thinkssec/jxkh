using Enterprise.Model.Perfo.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// 机关部门考核得分表
    /// 创建人:代码生成器
    /// 创建时间:2014/11/28 16:45:02
    /// </summary>
    [Serializable]
    public class KhJgbmkhdfModel : PerfoSuperModel
    {
        #region 代码生成器

        /// <summary>
        ///得分表ID
        /// </summary>
        public virtual string DFID
        {
            get;
            set;
        }

        /// <summary>
        ///机构编码
        /// </summary>
        public virtual int JGBM
        {
            get;
            set;
        }

        /// <summary>
        ///考核ID
        /// </summary>
        public virtual int KHID
        {
            get;
            set;
        }

        /// <summary>
        ///考核项名称
        /// </summary>
        public virtual string KHXMC
        {
            get;
            set;
        }

        /// <summary>
        ///考核得分
        /// </summary>
        public virtual decimal? KHDF
        {
            get;
            set;
        }

        /// <summary>
        ///考核类型
        /// </summary>
        public virtual string KHLX
        {
            get;
            set;
        }

        /// <summary>
        ///统计时间
        /// </summary>
        public virtual DateTime? TJSJ
        {
            get;
            set;
        }

        /// <summary>
        ///操作人
        /// </summary>
        public virtual string CZR
        {
            get;
            set;
        }

        /// <summary>
        ///考核项序号
        /// </summary>
        public virtual string XMXH
        {
            get;
            set;
        }

        /// <summary>
        ///考核单位数量
        /// </summary>
        public virtual int? KHDWSL
        {
            get;
            set;
        }

        /// <summary>
        ///部门总得分
        /// </summary>
        public virtual decimal? BMZDF
        {
            get;
            set;
        }

        /// <summary>
        ///部门平均分
        /// </summary>
        public virtual decimal? BMPJF
        {
            get;
            set;
        }

        /// <summary>
        ///考核排名
        /// </summary>
        public virtual int? BMPM
        {
            get;
            set;
        }

        /// <summary>
        ///部门兑现倍数
        /// </summary>
        public virtual decimal? BMDXBS
        {
            get;
            set;
        }

        /// <summary>
        ///备注说明
        /// </summary>
        public virtual string BZSM
        {
            get;
            set;
        }

        /// <summary>
        ///负责人总得分
        /// </summary>
        public virtual decimal? FZRZDF
        {
            get;
            set;
        }

        /// <summary>
        ///负责人平均分
        /// </summary>
        public virtual decimal? FZRPJF
        {
            get;
            set;
        }

        /// <summary>
        ///负责人兑现倍数
        /// </summary>
        public virtual decimal? FZRDXBS
        {
            get;
            set;
        }

        /// <summary>
        ///负责人考核排名
        /// </summary>
        public virtual int? FZRPM
        {
            get;
            set;
        }

        /// <summary>
        ///汇总标志
        /// </summary>
        public virtual string HZBZ
        {
            get;
            set;
        }

        #endregion

        #region 自定义关联项

        /// <summary>
        /// 部门机构对象
        /// </summary>
        public virtual SysBmjgModel Bmjg { get; set; }

        #endregion
    }

}
