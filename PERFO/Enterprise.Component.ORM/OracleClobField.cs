using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.OracleClient;
using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace Enterprise.Component.ORM
{
    /// <summary>
    /// 文件名:  OracleClobField.cs
    /// 功能描述: NHibernate的自定类字段类型，用于Oracle的Clob字段处理
    /// 创建人：qw
    /// 创建日期: 2013.2.22
    /// </summary>
    public class OracleClobField : PatchForOracleLobField
    {
        public override void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            if (cmd is OracleCommand)
            {
                //CLob、NClob类型的字段，存入中文时参数的OracleDbType必须设置为OracleDbType.Clob
                //否则会变成乱码（Oracle 10g client环境）
                OracleParameter param = cmd.Parameters[index] as OracleParameter;
                if (param != null)
                {
                    param.OracleType = OracleType.Clob;// 关键就这里啦
                    param.IsNullable = true;
                }
            }
            NHibernate.NHibernateUtil.StringClob.NullSafeSet(cmd, value, index);
        }
    }

    /// <summary>
    /// 用户自定义类型
    /// </summary>
    public abstract class PatchForOracleLobField : IUserType
    {
        public PatchForOracleLobField()
        {
        }
        public bool IsMutable
        {
            get { return true; }
        }
        public System.Type ReturnedType
        {
            get { return typeof(String); }
        }
        public SqlType[] SqlTypes
        {
            get
            {
                return new SqlType[] { NHibernateUtil.String.SqlType };
            }
        }
        public object DeepCopy(object value)
        {
            return value;
        }
        public new bool Equals(object x, object y)
        {
            return x == y;
        }
        public int GetHashCode(object x)
        {
            return x.GetHashCode();
        }
        public object Assemble(object cached, object owner)
        {
            return DeepCopy(cached);
        }
        public object Disassemble(object value)
        {
            return DeepCopy(value);
        }
        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            return NHibernate.NHibernateUtil.StringClob.NullSafeGet(rs, names[0]);
        }
        public abstract void NullSafeSet(IDbCommand cmd, object value, int index);
        public object Replace(object original, object target, object owner)
        {
            return original;
        }
    }
}
