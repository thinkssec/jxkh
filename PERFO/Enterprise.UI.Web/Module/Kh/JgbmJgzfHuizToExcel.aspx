<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JgbmJgzfHuizToExcel.aspx.cs"
    Inherits="Enterprise.UI.Web.Module.Kh.JgbmJgzfHuizToExcel" %>

<html xmlns:v="urn:schemas-microsoft-com:vml" xmlns:o="urn:schemas-microsoft-com:office:office"
xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40">
<head>
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name></x:Name><x:WorksheetOptions><x:Selected/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]-->
    <style type="text/css">
        .gdtjContainer .tb tr {
            text-align: center;
            vertical-align: middle;
        }

        .gdtjContainer .tb th {
            text-align: center;
            /*font-weight: normal;
                border-left: 0.5pt solid #000;
            border-bottom: 0.5pt solid #000;
                */
            font-size: 11pt;
            height: 35px;
        }

        .td-bold {
            font-weight: bold;
        }
    </style>
</head>
<body>
    <div class="gdtjContainer">
        <%=GetJgbmTable() %>
        <%--<table class="tb" cellspacing="0" cellpadding="0" border="0" width="2184px">
            <colgroup>
                <col class="td" />
                <col class="td" />
                <col class="td" />
                <col class="td" />
                <col class="td" />
                <col class="td" />
                <col class="td" />
                <col class="td" />
                <col class="td" />
                <col class="td" />
                <col class="td" />
                <col class="td" />
                <col class="td" />
                <col class="td" />
                <col class="td" />
                <col class="td" />
                <col class="td" />
                <col class="td" />
                <col class="td" />
                <col class="td" />
                <col class="td" />
                <col class="td" />
                <col class="td" />
                <col class="td" />
                <col class="td" />
                <col class="td" />
            </colgroup>
            <tr style="height: 40px">
                <th style="font-size: 20pt; font-family: 宋体; border: none;" colspan="26">
                    2011年增城市单位土地使用权出让情况登记表（统计时间从2011-06-29至2011-06-30）
                </th>
            </tr>
            <tr>
                <th colspan="23" style="border-left: none;">
                    &nbsp;
                </th>
                <th style="text-align: left; font-size: 12pt; border-left: none;" colspan="3">
                    单位：万元、平方米
                </th>
            </tr>
            <tr class="header">
                <th rowspan="2">
                    合同编号
                </th>
                <th colspan="2" rowspan="2">
                    用地单位
                </th>
                <th colspan="2" rowspan="2">
                    土地座落
                </th>
                <th rowspan="2">
                    供地面积
                </th>
                <th>
                    &nbsp;
                </th>
                <th>
                    &nbsp;
                </th>
                <th rowspan="2">
                    用途
                </th>
                <th colspan="3" rowspan="1">
                    出让金
                </th>
                <th rowspan="2">
                    容积率
                </th>
                <th rowspan="2">
                    建筑密度
                </th>
                <th rowspan="2">
                    绿地率
                </th>
                <th rowspan="2">
                    规划建筑面积
                </th>
                <th rowspan="2">
                    出让方式
                </th>
                <th rowspan="2">
                    审批日期
                </th>
                <th rowspan="2">
                    合同签订日期
                </th>
                <th rowspan="2">
                    动工期限
                </th>
                <th rowspan="2">
                    竣工日期
                </th>
                <th rowspan="2">
                    批次情况
                </th>
                <th rowspan="2">
                    合同约定缴费期限
                </th>
                <th rowspan="2">
                    缴费情况
                </th>
                <th rowspan="2">
                    滞纳金
                </th>
                <th rowspan="2" class="rightborder">
                    备注
                </th>
            </tr>
            <tr style="height: 40px" class="header">
                <th>
                    新增面积
                </th>
                <th style="font-size: 10pt;">
                    保障性住房用地占用面积
                </th>
                <th>
                    应缴
                </th>
                <th>
                    已缴
                </th>
                <th>
                    未缴
                </th>
            </tr>
            <tr>
                <th class="auto-style1" >
                    440183-2011-
                </th>
                <th colspan="2" class="auto-style1"> &nbsp;
                </th>
                <th colspan="2" class="auto-style1">
                    &nbsp;
                </th>
                <th class="auto-style1">&nbsp;
                </th>
                <th class="auto-style1">
                    &nbsp;
                </th>
                <th class="auto-style1">
                    &nbsp;
                </th>
                <th class="auto-style1">
                    &nbsp;
                </th>
                <th class="auto-style1">&nbsp;
                </th>
                <th class="auto-style1">
                    &nbsp;
                </th>
                <th class="auto-style1">
                    &nbsp;
                </th>
                <th class="auto-style1">
                    &nbsp;
                </th>
                <th class="auto-style1">
                    &nbsp;
                </th>
                <th class="auto-style1">
                    &nbsp;
                </th>
                <th class="auto-style1">
                    &nbsp;
                </th>
                <th class="auto-style1">
                    拍卖出让
                </th>
                <th class="auto-style1">
                    &nbsp;
                </th>
                <th class="auto-style1">
                    2011-06-29
                </th>
                <th class="auto-style1">
                    &nbsp;
                </th>
                <th class="auto-style1">
                    &nbsp;
                </th>
                <th class="auto-style1">
                    &nbsp;
                </th>
                <th class="auto-style1">
                    &nbsp;
                </th>
                <th class="auto-style1">
                    &nbsp;
                </th>
                <th class="auto-style1">
                    &nbsp;
                </th>
                <th class="rightborder" style="height: 29px">
                    &nbsp;
                </th>
            </tr>
            <tr>
                <th>
                    &nbsp;
                </th>
                <th colspan="2">
                    合计
                </th>
                <th colspan="2">
                    &nbsp;
                </th>
                <th>&nbsp;
                </th>
                <th>
                    &nbsp;
                </th>
                <th>
                    &nbsp;
                </th>
                <th>
                    &nbsp;
                </th>
                <th>&nbsp;
                </th>
                <th>
                    &nbsp;
                </th>
                <th>
                    &nbsp;
                </th>
                <th>
                    &nbsp;
                </th>
                <th>
                    &nbsp;
                </th>
                <th>
                    &nbsp;
                </th>
                <th>
                    &nbsp;
                </th>
                <th>
                    &nbsp;
                </th>
                <th>
                    &nbsp;
                </th>
                <th>
                    &nbsp;
                </th>
                <th>
                    &nbsp;
                </th>
                <th>
                    &nbsp;
                </th>
                <th>
                    &nbsp;
                </th>
                <th>
                    &nbsp;
                </th>
                <th>
                    &nbsp;
                </th>
                <th>
                    &nbsp;
                </th>
                <th class="rightborder">
                    &nbsp;
                </th>
            </tr>
        </table>--%>
    </div>
</body>
</html>
