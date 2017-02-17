using Enterprise.Model.Perfo.Kh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.Infrastructure;
using Enterprise.Service.Perfo.Kh;
using Enterprise.Service.Perfo.Sys;
using Enterprise.Model.Perfo.Kh;
using Enterprise.Model.Perfo.Sys;
using Enterprise.Model.Perfo.Zbk;

namespace Enterprise.Service.Perfo.Zbk
{

    /// <summary>
    /// 文件名:  ZbkJsgzLogic.cs
    /// 功能描述: 指标计算规则的具体业务逻辑类
    /// 创建人：QW
    /// 创建时间 ：2014/11/20
    /// </summary>
    public sealed partial class ZbkJsgzLogic
    {

        #region 投入资本收入贡献系数

        /// <summary>
        /// 投入资本收入贡献系数
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public JsgzLogicResult M_TouRuZiBenGongXianXiShu(object e)
        {
            JsgzLogicResult result = new JsgzLogicResult();
            result.MethodName = "M_TouRuZiBenGongXianXiShu";
            result.ParamObj = e;

            KhDlzbmxModel model = e as KhDlzbmxModel;
            if (model != null)
            {
                //1==提取本次考核与本指标相同的所有考核单位的完成值
                List<KhDlzbmxModel> dlzbDanwei = new List<KhDlzbmxModel>();
                dlzbDanwei.Add(model);
                KhDlzbmxService dlzbmxSrv = new KhDlzbmxService();
                var list = dlzbmxSrv.GetListByKhidAndZbbm(model.KHID.ToString(), model.ZBBM);
                foreach (var q in list)
                {
                    if (!dlzbDanwei.Exists(p=>p.JGBM == q.JGBM))
                    {
                        //检测该机构的完成值是否已录入
                        if (q.WCZ != null)
                        {
                            dlzbDanwei.Add(q);
                        }                  
                    }
                }
                if (dlzbDanwei.Count != list.Count)
                {
                    result.Msg = "完成值审核过的单位只有" + dlzbDanwei.Count + "家!";
                    return result;
                }
                //2==进行循环处理-----------------------------------
                //以纳入生产经营总收入考核单位平均投入资本收入贡献系数为基础1，每超（降）10%，增加（减少）0.01的系数。
                //系数最高不超过1.2，最低不低于0.9。
                decimal pjs = dlzbDanwei.Average(p => p.WCZ.Value);
                decimal wcz = model.WCZ.ToDecimal();
                if (pjs > 0)
                {
                    result.ReturnValue = (1 + ((wcz - pjs) / 10) * 0.01M);
                    decimal maxV = 1.2M;
                    decimal minV = 0.9M;
                    if (result.ReturnValue.Value > maxV) result.ReturnValue = maxV;
                    if (result.ReturnValue.Value < minV) result.ReturnValue = minV;
                    result.Msg = "";
                }
            }

            return result;
        }

        #endregion

    }
}
