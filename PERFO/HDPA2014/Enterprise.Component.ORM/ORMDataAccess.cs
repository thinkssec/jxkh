using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;

using NHibernate;
using NHibernate.Criterion;
using NHibernate.Collection;
using NHibernate.Cfg;

using Enterprise.Component.Infrastructure;

namespace Enterprise.Component.ORM
{
    /// <summary>
    /// 文件名:  ORMDataAccess.cs
    /// 功能描述: 封装NHibernate数据交互方法
    /// 创建人：qw
    /// 创建日期: 2013.1.22
    /// </summary>
    public class ORMDataAccess<TData> : IDisposable
    {
        /// <summary>
        /// 数据连接对象
        /// </summary>
        private ISession session;

        /// <summary>
        /// 构造方法
        /// </summary>
        public ORMDataAccess()
        {
            session = ORMApplication.GetCurrentSession(typeof(TData));
        }

        /// <summary>
        /// 反向构造方法
        /// </summary>
        ~ORMDataAccess()
        {
            Dispose();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (session != null)
            {
                if (session.Transaction.IsActive)
                {
                    session.Transaction.Rollback();
                }

                if (session.IsOpen)
                {
                    session.Close();
                }
                session = null;
            }
        }


        #region 执行原生SQL

        /// <summary>  
        /// 执行sql语句，没有返回数据集，提供事务支持  
        /// </summary>  
        /// <param name="sql"></param>  
        /// <param name="paramenters"></param>  
        /// <returns></returns>  
        public bool ExecuteNonQuery(string sql, params IDbDataParameter[] paramenters)
        {
            bool bl = false;
            ITransaction transaction = null;
            try
            {
                transaction = session.BeginTransaction();
                IDbCommand command = session.Connection.CreateCommand();
                transaction.Enlist(command);//注意此处要把command添加到事务中  
                command.CommandText = sql;
                if (paramenters != null)
                {
                    foreach (IDataParameter parm in paramenters)
                    {
                        command.Parameters.Add(parm);
                    }
                }
                int i = command.ExecuteNonQuery();
                if (i > -1) 
                    bl = true;
                transaction.Commit();
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                throw ex;
            }
            finally
            {
                if (session != null)
                {
                    session.Close();
                }
            }
            return bl;
        }


        /// <summary>  
        /// 获取原生SQL的数据集合
        /// </summary>  
        /// <param name="sql"></param>  
        /// <returns></returns>  
        public DataSet ExecuteDataset(string sql, params IDbDataParameter[] paramenters)
        {
            DataSet ds = new DataSet();
            try
            {
                IDbCommand command = session.Connection.CreateCommand();
                command.CommandText = sql;
                if (paramenters != null)
                {
                    foreach (IDataParameter parm in paramenters)
                    {
                        command.Parameters.Add(parm);
                    }
                }  
                IDataReader reader = command.ExecuteReader();
                DataTable result = new DataTable();
                //result.Load(reader);
                //此方法亦可  
                DataTable schemaTable = reader.GetSchemaTable();
                for (int i = 0; i < schemaTable.Rows.Count; i++)
                {
                    string columnName = schemaTable.Rows[i][0].ToString();
                    result.Columns.Add(columnName);
                }
                while (reader.Read())
                {
                    int fieldCount = reader.FieldCount;
                    object[] values = new Object[fieldCount];
                    for (int i = 0; i < fieldCount; i++)
                    {
                        values[i] = reader.GetValue(i);
                    }
                    result.Rows.Add(values);
                }
                ds.Tables.Add(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (session != null)
                {
                    session.Close();
                }
            }
            return ds;
        }

        #endregion

        #region Transaction

        public void Transaction(Action action)
        {
            ITransaction transaction = session.BeginTransaction();
            try
            {
                action();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public void Transaction(Action action, System.Data.IsolationLevel isolationLevel)
        {
            ITransaction transaction = session.BeginTransaction(isolationLevel);
            try
            {
                action();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        #endregion


        #region Insert or Update

        public void Insert(object obj)
        {
            session.Save(obj);
            session.Flush();
        }

        public void Update(object obj)
        {
            session.Update(obj);
            session.Flush();
        }

        public void InsertOrUpdate(object obj)
        {
            session.SaveOrUpdate(obj);
            session.Flush();
        }

        #endregion


        #region Delete

        /// <summary>
        /// 根据实体对象删除
        /// </summary>
        /// <param name="obj">实体对象</param>
        public void Delete(object obj)
        {
            session.Delete(obj);
            session.Flush();
        }

        /// <summary>
        /// 根据hql语句删除
        /// <example>
        /// hql="from 类名 where 属性名=值"
        /// </example>
        /// </summary>
        /// <param name="hql">hql语句</param>
        public void DeleteByQuery(string hql)
        {
            session.Delete(hql);
            session.Flush();
        }

        /// <summary>
        /// 根据Query进行删除
        /// <example>
        /// hql="delete from 类名 where 属性名=:参数名";
        /// </example>
        /// </summary>
        /// <param name="hql">hql语句</param>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        public void DeleteByQuery(string hql, string name, object value)
        {
            DeleteByQuery(hql, new string[] { name }, new object[] { value });
        }

        /// <summary>
        /// 根据Query进行删除
        /// <example>
        /// hql="delete from 类名 where 属性名=:参数名";
        /// </example>
        /// </summary>
        /// <param name="hql">hql语句</param>
        /// <param name="names">参数名称数组</param>
        /// <param name="values">参数值数组</param>
        public void DeleteByQuery(string hql, string[] names, object[] values)
        {
            IQuery query = session.CreateQuery(hql);
            for (int i = 0; i < names.Length; i++)
            {
                query.SetParameter(names[i], values[i]);
            }
            query.ExecuteUpdate();
            session.Flush();
        }

        /// <summary>
        /// 根据Query进行删除
        /// <example>
        /// hql="delete from 类名 where 属性名=? and 属性名=？";
        /// </example>
        /// </summary>
        /// <param name="hql">hql语句</param>
        /// <param name="values">参数值数组</param>
        public void DeleteByQuery(string hql, object[] values)
        {
            IQuery query = session.CreateQuery(hql);
            for (int i = 0; i < values.Length; i++)
            {
                query.SetParameter(i, values[i]);
            }
            query.ExecuteUpdate();
            session.Flush();
        }

        #endregion


        #region Count

        public int Count<T>() where T : class
        {
            ICriteria criteria = session.CreateCriteria<T>();
            return Convert.ToInt32(criteria.SetProjection(Projections.RowCount()).UniqueResult());
        }

        public int Count<T>(ICriterion expression) where T : class
        {
            ICriteria criteria = session.CreateCriteria<T>();
            if (expression != null)
            {
                criteria.Add(expression);
            }
            return Convert.ToInt32(criteria.SetProjection(Projections.RowCount()).UniqueResult());
        }

        #endregion


        #region Scalar

        public object Scalar(string hql)
        {
            return session.CreateQuery(hql).UniqueResult();
        }

        public object Scalar(string hql, params object[] values)
        {
            IQuery query = session.CreateQuery(hql);
            for (int i = 0; i < values.Length; i++)
            {
                query.SetParameter(i, values[i]);
            }
            return query.UniqueResult();
        }

        public object Scalar(string hql, string name, object value)
        {
            return Scalar(hql, new string[] { name }, new object[] { value });
        }

        public object Scalar(string hql, string[] names, object[] values)
        {
            IQuery query = session.CreateQuery(hql);
            for (int i = 0; i < names.Length; i++)
            {
                query.SetParameter(names[i], values[i]);
            }
            return query.UniqueResult();
        }


        public object Scalar(string hql, Action<IQuery> action)
        {
            IQuery query = session.CreateQuery(hql);
            action(query);
            return query.UniqueResult();
        }

        #endregion


        #region ScalarBySQL

        /// <summary>
        /// 执行原生SQL，并返回一条记录
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object ScalarBySQL(string sql)
        {
            return session.CreateSQLQuery(sql).UniqueResult();
        }

        public object ScalarBySQL(string sql, params object[] values)
        {
            ISQLQuery query = session.CreateSQLQuery(sql);
            for (int i = 0; i < values.Length; i++)
            {
                query.SetParameter(i, values[i]);
            }
            return query.UniqueResult();
        }

        public object ScalarBySQL(string sql, string name, object value)
        {
            return ScalarBySQL(sql, new string[] { name }, new object[] { value });
        }

        public object ScalarBySQL(string sql, string[] names, object[] values)
        {
            ISQLQuery query = session.CreateSQLQuery(sql);
            for (int i = 0; i < names.Length; i++)
            {
                query.SetParameter(names[i], values[i]);
            }
            return query.UniqueResult();
        }

        public object ScalarBySQL(string sql, Action<ISQLQuery> action)
        {
            ISQLQuery query = session.CreateSQLQuery(sql);
            action(query);
            return query.UniqueResult();
        }

        #endregion


        #region Page SQL Query

        public IList QueryBySQL(string sql, object[] values, int pageIndex, int pageSize)
        {
            ISQLQuery query = session.CreateSQLQuery(sql);
            for (int i = 0; i < values.Length; i++)
            {
                query.SetParameter(i, values[i]);
            }
            return query.SetFirstResult((pageIndex - 1) * pageSize).SetMaxResults(pageSize).List();
        }

        public IList QueryBySQL(string sql, string name, object value, int pageIndex, int pageSize)
        {
            return QueryBySQL(sql, new string[] { name }, new object[] { value }, pageIndex, pageSize);
        }

        public IList QueryBySQL(string sql, string[] names, object[] values, int pageIndex, int pageSize)
        {
            ISQLQuery query = session.CreateSQLQuery(sql);
            for (int i = 0; i < names.Length; i++)
            {
                query.SetParameter(names[i], values[i]);
            }
            return query.SetFirstResult((pageIndex - 1) * pageSize).SetMaxResults(pageSize).List();
        }

        public IList QueryBySQL(string sql, int pageIndex, int pageSize)
        {
            ISQLQuery query = session.CreateSQLQuery(sql);
            return query.SetFirstResult((pageIndex - 1) * pageSize).SetMaxResults(pageSize).List();
        }

        #endregion


        #region SQL Query

        /// <summary>
        /// 返回原生SQL的查询结果集
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<T> QueryBySQL<T>(string sql,Type classType)
        {
            IList<T> rLst = new List<T>();
            try
            {
                ISQLQuery query = session.CreateSQLQuery(sql).AddEntity(classType);
                rLst = query.List<T>();
            }
            catch (Exception ex)
            {
                Debuger.GetInstance().log(this, "执行QueryBySQL方法出错!", ex);
            }
            return rLst;
        }

        //public IList QueryBySQL(string sql, params object[] values)
        //{
        //    ISQLQuery query = session.CreateSQLQuery(sql);
        //    for (int i = 0; i < values.Length; i++)
        //    {
        //        query.SetParameter(i, values[i]);
        //    }
        //    return query.List();
        //}

        //public IList QueryBySQL(string sql, string name, object[] value)
        //{
        //    return QueryBySQL(sql, new string[] { name }, new object[] { value });
        //}

        /// <summary>
        /// 根据原生SQL和其传入参数返回查询结果集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="names"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public IList<T> QueryBySQL<T>(string sql, string[] names, object[] values, Type classType)
        {
            IList<T> rLst = new List<T>();
            try
            {
                ISQLQuery query = session.CreateSQLQuery(sql).AddEntity(classType);
                for (int i = 0; i < names.Length; i++)
                {
                    query.SetParameter(names[i], values[i]);
                }
                rLst = query.List<T>();
            }
            catch (Exception ex)
            {
                Debuger.GetInstance().log(this, "执行QueryBySQL方法出错!", ex);
            }
            return rLst;
        }

        //public IList QueryBySQL(string sql, Action<ISQLQuery> action)
        //{
        //    ISQLQuery query = session.CreateSQLQuery(sql);
        //    action(query);
        //    return query.List();
        //}

        #endregion


        #region Page Query

        public IList<T> Query<T>(int pageIndex, int pageSize, out int recordCount) where T : class
        {
            return Query<T>(null, null, pageIndex, pageSize, out recordCount);
        }

        public IList<T> Query<T>(ICriterion expression, int pageIndex, int pageSize, out int recordCount) where T : class
        {
            return Query<T>(expression, null, pageIndex, pageSize, out recordCount);
        }

        public IList<T> Query<T>(ICriterion expression, Order[] order,
            int pageIndex, int pageSize, out int recordCount) where T : class
        {
            IList<T> list = new List<T>();
            recordCount = 0;
            ICriteria query = session.CreateCriteria<T>();
            if (expression != null)
            {
                query.Add(expression);
            }
            ICriteria queryPage = CriteriaTransformer.Clone(query);
            //获取记录总数
            recordCount = Convert.ToInt32(query.SetProjection(Projections.RowCount()).UniqueResult());

            //设置排序
            if (order != null)
            {
                foreach (Order o in order)
                {
                    queryPage.AddOrder(o);
                }
            }
            queryPage.SetFirstResult((pageIndex - 1) * pageSize);
            queryPage.SetMaxResults(pageSize);
            list = queryPage.List<T>();

            return list;
        }

        public IList<T> Query<T>(string hql, object[] values, int pageIndex, int pageSize)
        {
            IList<T> rLst = new List<T>();
            try
            {
                IQuery query = session.CreateQuery(hql);
                for (int i = 0; i < values.Length; i++)
                {
                    query.SetParameter(i, values[i]);
                }
                rLst = query.SetFirstResult((pageIndex - 1) * pageSize).SetMaxResults(pageSize).List<T>();
            }
            catch (Exception ex)
            {
                Debuger.GetInstance().log(this, "执行Query方法出错!", ex);
            }
            return rLst;
        }

        public IList<T> Query<T>(string hql, string name, object value, int pageIndex, int pageSize)
        {
            return Query<T>(hql, new string[] { name }, new object[] { value }, pageIndex, pageSize);
        }

        /// <summary>
        /// 根据传入参数返回指定分页的数据集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hql"></param>
        /// <param name="names"></param>
        /// <param name="values"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IList<T> Query<T>(string hql, string[] names, object[] values, int pageIndex, int pageSize)
        {
            IList<T> rLst = new List<T>();
            try
            {
                IQuery query = session.CreateQuery(hql);
                for (int i = 0; i < names.Length; i++)
                {
                    query.SetParameter(names[i], values[i]);
                }
                rLst = query.SetFirstResult((pageIndex - 1) * pageSize).SetMaxResults(pageSize).List<T>();
            }
            catch (Exception ex)
            {
                Debuger.GetInstance().log(this, "执行Query方法出错!", ex);
            }
            return rLst;
        }

        /// <summary>
        /// 返回指定分页的数据集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IList<T> Query<T>(string hql, int pageIndex, int pageSize)
        {
            IList<T> rLst = new List<T>();
            try
            {
                IQuery query = session.CreateQuery(hql);
                rLst = query.SetFirstResult((pageIndex - 1) * pageSize).SetMaxResults(pageSize).List<T>();
            }
            catch (Exception ex)
            {
                Debuger.GetInstance().log(this, "执行Query方法出错!", ex);
            }
            return rLst;
        }

        #endregion


        #region Query

        public IList<T> Query<T>() where T : class
        {
            return session.CreateCriteria<T>().List<T>();
        }

        public IList<T> Query<T>(Action<ICriteria> action) where T : class
        {
            ICriteria criteria = session.CreateCriteria<T>();
            action(criteria);
            return criteria.List<T>();
        }

        public IList<T> Query<T>(ICriterion expression) where T : class
        {
            return Query<T>(expression, null);
        }

        public IList<T> Query<T>(ICriterion expression, Action<ICriteria> action) where T : class
        {
            ICriteria criteria = session.CreateCriteria<T>();
            if (expression != null)
            {
                criteria.Add(expression);
            }
            if (action != null)
            {
                action(criteria);
            }
            return criteria.List<T>();
        }

        /// <summary>
        /// 获取数据对象的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hql"></param>
        /// <returns></returns>
        public IList<T> Query<T>(string hql)
        {
            IList<T> rLst = new List<T>();
            try
            {
                rLst = session.CreateQuery(hql).List<T>();
            }
            catch (Exception ex)
            {
                Debuger.GetInstance().log(this, "执行Query方法出错!", ex);
            }
            return rLst;
        }

        /// <summary>
        /// 获取数据对象的集合--支持查询缓存
        /// 注意：官方也认为查询缓存用处不大 2013.1.24
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hql"></param>
        /// <param name="cacheRegion"></param>
        /// <returns></returns>
        public IList<T> QueryInCache<T>(string hql,string cacheRegion)
        {
            return session.CreateQuery(hql).SetCacheable(true).
                SetCacheRegion(cacheRegion).List<T>();
        }

        //public IList<T> Query<T>(string hql, params object[] values)
        //{
        //    IQuery query = session.CreateQuery(hql);
        //    for (int i = 0; i < values.Length; i++)
        //    {
        //        query.SetParameter(i, values[i]);
        //    }
        //    return query.List<T>();
        //}

        /// <summary>
        /// 调用存储过程并返回结果
        /// </summary>
        /// <typeparam name="T">对象实体</typeparam>
        /// <param name="procName">存储过程名称</param>
        /// <param name="names">参数名称</param>
        /// <param name="values">传值</param>
        /// <returns></returns>
        public IList<T> QueryForProc<T>(string procName, string[] names, object[] values)
        {
            IList<T> rLst = new List<T>();
            try
            {
                IQuery query = session.GetNamedQuery(procName);
                if (names != null)
                {
                    for (int i = 0; i < names.Length; i++)
                    {
                        query.SetParameter(names[i], values[i]);
                    }
                }
                rLst = query.List<T>();
            }
            catch (Exception ex)
            {
                Debuger.GetInstance().log(this, "执行QueryForProc方法出错!", ex);
            }
            return rLst;
        }

        public IList<T> Query<T>(string hql, string name, object value)
        {
            return Query<T>(hql, new string[] { name }, new object[] { value });
        }

        /// <summary>
        /// 根据传入参数返回数据集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hql"></param>
        /// <param name="names"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public IList<T> Query<T>(string hql, string[] names, object[] values)
        {
            IList<T> rLst = new List<T>();
            try
            {
                IQuery query = session.CreateQuery(hql);
                for (int i = 0; i < names.Length; i++)
                {
                    query.SetParameter(names[i], values[i]);
                }
                rLst = query.List<T>();
            }
            catch (Exception ex)
            {
                Debuger.GetInstance().log(this, "执行Query方法出错!", ex);
            }
            return rLst;
        }

        //public IList<T> Query<T>(string hql, Action<IQuery> action)
        //{
        //    IQuery query = session.CreateQuery(hql);
        //    action(query);
        //    return query.List<T>();
        //}

        public T Get<T>(object id)
        {
            return session.Get<T>(id);
        }

        public T Load<T>(object id)
        {
            return session.Load<T>(id);
        }

        #endregion


    }
}
