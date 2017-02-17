using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo
{
    /// <summary>
    /// 超类
    /// </summary>
    [Serializable]
    public class PerfoSuperModel
    {

        /// <summary>
        /// 操作方法 新增、修改、删除
        /// </summary>
        public virtual string DB_Option_Action { get; set; }


        #region 使用复合ID(多字段)的实体类必须先覆写下面两个方法 Equals and GetHashCode

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
