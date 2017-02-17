using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Zbk
{
    /// <summary>
    /// 量化指标维护
    /// 创建人:代码生成器
    /// 创建时间:2014/11/1 0:35:46
    /// </summary>
    [Serializable]
    public class ZbkLhzbModel : PerfoSuperModel
    {
        #region 代码生成器

        /// <summary>
        ///量化指标编码
        /// </summary>
        public virtual string LHZBBM
        {
            get;
            set;
        }

        /// <summary>
        ///规则ID
        /// </summary>
        public virtual string GZID
        {
            get;
            set;
        }

        /// <summary>
        ///指标ID
        /// </summary>
        public virtual int ZBID
        {
            get;
            set;
        }

        /// <summary>
        ///版本名称
        /// </summary>
        public virtual string BBMC
        {
            get;
            set;
        }

        /// <summary>
        ///计算单位
        /// </summary>
        public virtual string JSDW
        {
            get;
            set;
        }

        /// <summary>
        ///本级权重
        /// </summary>
        public virtual decimal? BJQZ
        {
            get;
            set;
        }

        /// <summary>
        ///指标说明
        /// </summary>
        public virtual string ZBSM
        {
            get;
            set;
        }

        /// <summary>
        ///评分标准
        /// </summary>
        public virtual string PFBZ
        {
            get;
            set;
        }

        /// <summary>
        ///是否禁用
        /// </summary>
        public virtual string SFJY
        {
            get;
            set;
        }

        /// <summary>
        ///上级指标
        /// </summary>
        public virtual string PARENTZBBM
        {
            get;
            set;
        }

        /// <summary>
        ///辅助指标
        /// </summary>
        public virtual string FZZB
        {
            get;
            set;
        }

        /// <summary>
        ///计算表达式
        /// </summary>
        public virtual string JSBDS
        {
            get;
            set;
        }

        /// <summary>
        ///计算描述
        /// </summary>
        public virtual string JSMS
        {
            get;
            set;
        }

        /// <summary>
        ///基准分数
        /// </summary>
        public virtual decimal? JZFS
        {
            get;
            set;
        }

        /// <summary>
        ///指标代号
        /// </summary>
        public virtual string ZBDH
        {
            get;
            set;
        }

        /// <summary>
        /// 是否可取上年数据作目标值
        /// </summary>
        public virtual string ISMBZ
        {
            get;
            set;
        }

        /// <summary>
        /// 指标显示序号
        /// </summary>
        public virtual string ZBXH
        {
            get;
            set;
        }

        /// <summary>
        /// 旧表ID
        /// </summary>
        public virtual string OLDID
        {
            get;
            set;
        }

        #endregion

        #region 自定义属性

        /// <summary>
        /// 计算规则对象
        /// </summary>
        public virtual ZbkJsgzModel Jsgz { get; set; }
        /// <summary>
        /// 指标基础信息
        /// </summary>
        public virtual ZbkZbxxModel Zbxx { get; set; }
        /// <summary>
        /// 目标值填报
        /// </summary>
        public virtual IList<ZbkMbztbModel> MbztbLst { get; set; }
        /// <summary>
        /// 目标值审核
        /// </summary>
        public virtual IList<ZbkMbzshModel> MbzshLst { get; set; }
        /// <summary>
        /// 完成值填报
        /// </summary>
        public virtual IList<ZbkWcztbModel> WcztbLst { get; set; }
        /// <summary>
        /// 完成值审核
        /// </summary>
        public virtual IList<ZbkWczshdfModel> WczshdfLst { get; set; }
        /// <summary>
        /// 根据指标序号生成表示符号
        /// </summary>
        public virtual string GradeSymbol
        {
            get
            {
                string s = string.Empty;
                if (!string.IsNullOrEmpty(ZBXH) && ZBXH.Length > 5)
                {
                    int cou = Convert.ToInt32((ZBXH.Length - 5) * 1M / 3M);
                    for (int i = 0; i < cou; i++)
                    {
                        s += "﹄﹄";
                    }
                }
                return s;
            }
        }

        #endregion
    }

}
