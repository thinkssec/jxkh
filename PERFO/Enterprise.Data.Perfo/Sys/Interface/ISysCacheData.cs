using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Model.Perfo.Sys;

namespace Enterprise.Data.Perfo.Sys
{	

    /// <summary>
    /// �ļ���:  ISysCacheData.cs
    /// ��������: ���ݲ�-ϵͳ����������ݷ��ʽӿ�
	/// �����ˣ�����������
	/// ����ʱ�䣺2014/10/31 10:02:24
    /// </summary>
    public interface ISysCacheData : IDataPerfo<SysCacheModel>
    {
        #region ����������
        
        ///// <summary>
        ///// ����ԭ��SQL�Ĳ�ѯ�б�
        ///// </summary>
        ///// <param name="sql"></param>
        ///// <returns></returns>
        //IList<SysCacheModel> GetListBySQL(string sql);


        ///// <summary>
        ///// ���������������ݼ���
        ///// </summary>
        ///// <param name="hql">HQL</param>
        ///// <returns></returns>
        //IList<SysCacheModel> GetListByHQL(string hql);

        #endregion
    }

}
