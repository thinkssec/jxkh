using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Component.Cache;
using Enterprise.Data.Perfo.Sys;
using Enterprise.Model.Perfo.Sys;
using System.Web.UI.WebControls;

namespace Enterprise.Service.Perfo.Sys
{

    /// <summary>
    /// �ļ���:  SysBmjgService.cs
    /// ��������: ҵ���߼���-�����������ݴ���
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:45
    /// </summary>
    public class SysBmjgService
    {
        #region ����������

        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly ISysBmjgData dal = new SysBmjgData();

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SysBmjgModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<SysBmjgModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<SysBmjgModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<SysBmjgModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(SysBmjgModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region �Զ��巽��

        /// <summary>
        /// ��ȡ���Ż������������м���
        /// </summary>
        /// <param name="isAll">�Ƿ�ȫ����ʾ</param>
        /// <returns></returns>
        public IList<SysBmjgModel> GetBmjgTreeLst(bool isAll)
        {
            List<SysBmjgModel> bmjgList = null;
            if (WebKeys.EnableCaching)
            {
                bmjgList = (List<SysBmjgModel>)CacheHelper.GetCache(SysBmjgData.CacheClassKey + "_GetBmjgTreeLst_" + isAll);
            }
            if (bmjgList == null || bmjgList.Count == 0)
            {
                string hql = "from SysBmjgModel p ";
                if (!isAll)
                {
                    hql += " where p.SFKH='1'";
                }
                bmjgList = GetListByHQL(hql).OrderBy(p => p.XSXH).ToList();
                foreach (var q in bmjgList)
                {
                    switch (q.XSXH.Length)
                    {
                        case 2:
                            break;
                        case 4:
                            q.JGMC = CommonTool.GenerateBlankSpace(1) + q.JGMC;
                            break;
                        default:
                            string jgmc = CommonTool.GenerateBlankSpace(q.XSXH.Length / 2 - 1);
                            q.JGMC = jgmc + q.JGMC;
                            break;
                    }
                }
            }
            if (WebKeys.EnableCaching)
            {
                //���ݴ��뻺��ϵͳ
                CacheHelper.Add(SysBmjgData.CacheClassKey + "_GetBmjgTreeLst_" + isAll, bmjgList);
            }
            return bmjgList;
        }

        /// <summary>
        /// ��ȡ��Ӧ�Ļ�������
        /// </summary>
        /// <param name="jgbm">��������</param>
        /// <returns></returns>
        public static string GetBmjgName(int jgbm)
        {
            var m = dal.GetList().FirstOrDefault(p => p.JGBM == jgbm);
            return ((m != null) ? m.JGMC : "");
        }

  
        /// <summary>
        /// ��ȡ��Ӧ�Ļ�������
        /// </summary>
        /// <param name="xsxh">��ʾ���</param>
        /// <param name="len">��ʾ��ų���</param>
        /// <returns></returns>
        public static string GetBmjgNameByXsxh(string xsxh, int len)
        {
            if (len > 0 && xsxh.Length >= len)
            {
                xsxh = xsxh.Substring(0, len);
            }
            var m = dal.GetList().FirstOrDefault(p => p.XSXH == xsxh);
            return ((m != null) ? m.JGMC : "");
        }

        /// <summary>
        /// ��ȡ��Ӧ�Ļ�������
        /// </summary>
        /// <param name="xsxh">��ʾ���</param>
        /// <param name="len">��ʾ��ų���</param>
        /// <returns></returns>
        public SysBmjgModel GetModelByXsxh(string xsxh, int len)
        {
            if (len > 0 && xsxh.Length >= len)
            {
                xsxh = xsxh.Substring(0, len);
            }
            var m = dal.GetList().FirstOrDefault(p => p.XSXH == xsxh);
            return m;
        }

        /// <summary>
        /// ��ȡָ�������������¼�����
        /// </summary>
        /// <param name="jgbm"></param>
        /// <returns></returns>
        public string GetJgbmsForSQL(string jgbm)
        {
            string jgbms = string.Empty;
            var bmjg = GetSingle(jgbm);
            if (bmjg != null)
            {
                var list = GetList().Where(p => p.XSXH.StartsWith(bmjg.XSXH)).OrderBy(p => p.XSXH);
                foreach (var q in list)
                {
                    jgbms += "'" + q.JGBM + "',";
                }
            }
            return jgbms.TrimEnd(',');
        }

        /// <summary>
        /// ��ȡͬһ�㼶�����е�λ������
        /// </summary>
        /// <param name="xhLength"></param>
        /// <returns></returns>
        public IList<SysBmjgModel> GetSameLevelBmjg(int xhLength)
        {
            List<SysBmjgModel> bmjgList = null;
            if (WebKeys.EnableCaching)
            {
                bmjgList = (List<SysBmjgModel>)CacheHelper.GetCache(SysBmjgData.CacheClassKey + "_GetSameLevelBmjg_" + xhLength);
            }
            if (bmjgList == null || bmjgList.Count == 0)
            {
                string hql = "from SysBmjgModel p where length(p.XSXH)='" + xhLength + "' order by p.JGLX,p.BZ";
                bmjgList = GetListByHQL(hql).ToList();
            }
            if (WebKeys.EnableCaching)
            {
                //���ݴ��뻺��ϵͳ
                CacheHelper.Add(SysBmjgData.CacheClassKey + "_GetSameLevelBmjg_" + xhLength, bmjgList);
            }
            return bmjgList;
        }

        /// <summary>
        /// ��ȡָ�����������µ������¼���������
        /// </summary>
        /// <param name="jgbm">��������</param>
        /// <returns></returns>
        public IList<SysBmjgModel> GetBmjgLisByParentJgbm(string jgbm)
        {
            List<SysBmjgModel> bmList = null;
            var model = GetSingle(jgbm);
            if (model != null)
            {
                bmList = GetBmjgTreeLst(false).
                    Where(p => p.XSXH.StartsWith(model.XSXH) && p.XSXH.Length >= model.XSXH.Length).OrderBy(p=>p.XSXH).ToList();

            }
            return bmList;
        }

        /// <summary>
        /// �󶨴����鹦�ܵ�ָ����λ�������ؼ�
        /// </summary>
        /// <param name="ddl">�����ؼ�</param>
        /// <param name="bmjgLst">���Ż���</param>
        /// <param name="parentJgbms">��������</param>
        public void BindSSECDropDownListForBmjg(SSECDropDownList ddl, IList<SysBmjgModel> bmjgLst, int[] parentJgbms)
        {
            ddl.Items.Clear();
            string jglx = string.Empty;
            string xsxh = string.Empty;
            foreach (var q in bmjgLst)
            {
                if (parentJgbms.Contains(q.JGBM))
                {
                    ddl.Items.Add(new ListItem(q.JGMC, "optgroup"));
                    continue;
                }
                if (string.IsNullOrEmpty(jglx))
                {
                    ddl.Items.Add(new ListItem(q.JGLX, "optgroup"));
                    ddl.Items.Add(new ListItem(q.JGMC, q.JGBM.ToString()));
                    jglx = q.JGLX;
                    xsxh = q.XSXH.Substring(0, 4);
                }
                else if (q.JGLX == jglx && q.XSXH.StartsWith(xsxh))
                {
                    ddl.Items.Add(new ListItem(q.JGMC, q.JGBM.ToString()));
                }
                else
                {
                    ddl.Items.Add(new ListItem(q.JGLX, "optgroup"));
                    ddl.Items.Add(new ListItem(q.JGMC, q.JGBM.ToString()));
                    jglx = q.JGLX;
                    xsxh = q.XSXH.Substring(0, 4);
                }
            }
            ddl.Items.Insert(0, new ListItem("����������", ""));
        }

        /// <summary>
        /// �󶨴����鹦�ܵĵ�λ�����ؼ�
        /// </summary>
        /// <param name="ddl">�����ؼ�</param>
        /// <param name="bmjgLst">���Ż���</param>
        public void BindSSECDropDownListForBmjg(SSECDropDownList ddl, IList<SysBmjgModel> bmjgLst)
        {
            ddl.Items.Clear();
            string jglx = string.Empty;
            foreach (var q in bmjgLst)
            {
                if (string.IsNullOrEmpty(jglx))
                {
                    ddl.Items.Add(new ListItem(q.JGLX, "optgroup"));
                    ddl.Items.Add(new ListItem(q.JGMC, q.JGBM.ToString()));
                    jglx = q.JGLX;
                }
                else if (q.JGLX == jglx)
                {
                    ddl.Items.Add(new ListItem(q.JGMC, q.JGBM.ToString()));
                }
                else
                {
                    ddl.Items.Add(new ListItem(q.JGLX, "optgroup"));
                    ddl.Items.Add(new ListItem(q.JGMC, q.JGBM.ToString()));
                    jglx = q.JGLX;
                }
            }
            ddl.Items.Insert(0, new ListItem("����������", ""));
        }
                

        #endregion
    }

}
