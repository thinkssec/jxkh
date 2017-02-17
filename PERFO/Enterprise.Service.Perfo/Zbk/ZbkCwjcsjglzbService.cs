using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Zbk;
using Enterprise.Model.Perfo.Zbk;

namespace Enterprise.Service.Perfo.Zbk
{

    /// <summary>
    /// �ļ���:  ZbkCwjcsjglzbService.cs
    /// ��������: ҵ���߼���-����ָ�������������ݶ�Ӧ�����ݴ���
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/12/14 9:19:20
    /// </summary>
    public class ZbkCwjcsjglzbService
    {
        #region ����������

        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly IZbkCwjcsjglzbData dal = new ZbkCwjcsjglzbData();

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ZbkCwjcsjglzbModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<ZbkCwjcsjglzbModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<ZbkCwjcsjglzbModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<ZbkCwjcsjglzbModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(ZbkCwjcsjglzbModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region �Զ��巽����

        /// <summary>
        /// ��ɲ���������������ָ���Ӧ��ϵ�ĳ�ʼ��
        /// </summary>
        /// <returns></returns>
        public bool InitCwjcsjGlzbData()
        {
            bool isOK = true;
            List<ZbkCwjcsjglzbModel> list = new List<ZbkCwjcsjglzbModel>();
            list.AddRange(GetList());//������������ȡ����
            ZbkLhzbService lhzbSrv = new ZbkLhzbService();
            var lhzbLst =lhzbSrv.GetList().Where(p => p.JSBDS.IndexOf('=') > 0).ToList();
            foreach (var lhzb in lhzbLst)
            {
                string bds = lhzb.JSBDS;
                bds = bds.Substring(bds.IndexOf('=') + 1);
                List<string> values = Utility.GetMatchValues(bds, @"\{.*?\}");
                foreach (var v in values)
                {
                    ZbkCwjcsjglzbModel model = new ZbkCwjcsjglzbModel();
                    model.DB_Option_Action = WebKeys.InsertAction;
                    model.ID = CommonTool.GetGuidKey(); //ID
                    model.ZBXMC = v.Trim('{', '}');
                    if (!list.Exists(p => p.ZBXMC == model.ZBXMC))
                        list.Add(model);
                }
            }
            foreach (var m in list)
            {
                isOK = Execute(m);
            }
            return isOK;
        }

        #endregion
    }

}
