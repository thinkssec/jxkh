<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KhZbLr2.aspx.cs" Inherits="Enterprise.UI.Web.Module.KhX.KhZbLr2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>考核指标录入</title>
    <link rel="stylesheet" type="text/css" href="/Resources/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/themes/icon.css" />
    <script type="text/javascript" src="/Resources/Scripts/jquery-1.4.2.min.js"></script>
    <script type="text/javascript" src="/Resources/Scripts/jquery.easyui.min.js"></script>
    <script type="text/javascript">
        var KhfsArray = [{ fs: "日常检查" }, { fs: "现场检查" }, { fs: "资料检查" }, { fs: "" }];
        var mergeColumn = "XM1,XM2,NR,BZ";
        var typeJs = null, tempTypeJs = null;
        $(function () {
            $("#dg").height($(document).height() - 100);
            $("#selectMbList").combobox({
                url: 'data/KhHandler.ashx?type=getKhmbList&bm=<%=bm%>&mbid=<%=id%>',
                valueField: 'ID',
                textField: 'NAME'
            });
            $("#dg").datagrid({
                url: 'data/KhHandler.ashx?type=getKhzb&mbid=<%=id%>',
                singleSelect: true,
                nowrap: false,
                rownumbers: true,
                idField: "ID",
                treeField: "ID",
                columns: [[
                    { field: 'ID', title: 'ID', hidden: true },
                    { field: 'XM1', title: '考核项目1', width: 100, align: "center", editor: "text" },
                    { field: 'XM2', title: '考核项目2', width: 80, align: "center", editor: "text" },
                    { field: 'NR', title: '考核内容', width: 140, align: "center", editor: "text" },
                    { field: 'BZ', title: '考核标准', width: 350, align: "center", editor: "text" },
                    { field: 'ZQ', title: '考核周期', width: 70, align: "center", editor: "text" },
                    {
                        field: 'FS', title: '考核方式', width: 70, align: "center",
                        editor: { type: "combobox", options: { data: KhfsArray, valueField: "fs", textField: "fs" } }
                    },
                    { field: 'BZF', title: '标准分', width: 50, align: "center", editor: "numberbox" },
                    { field: 'ORD', title: '排序', width: 50, align: "center", editor: "numberbox" }
                ]],
                toolbar: '#tb',
                onLoadSuccess: function (data) {
                    if (data.rows.length > 0) {
                        //设置评分方式选中值
                        $("#selectType").combobox('select', data.rows[0].TYPE);
                        typeJs = $("#selectType").combobox("getValue");
                        //调用mergeCellsByField()合并单元格
                        mergeCellsByField("dg", mergeColumn);
                    }
                    selectBindEvent();
                }
            });
            $("#btnAdd").bind("click", function () {
                // if (confirm("确定要在当前的评分方式下新增？")) {
                <%-- location.href = "KhzbEdit.aspx?mbid=<%=id%>&type=" + $("#selectType").combobox("getValue");--%>
                // }
                location.href = "KhzbEdit.aspx?mbid=<%=id%>&type=" + $("#selectType").combobox("getValue")
                    + "&khdzbid=<%=khdzbid%>&bm=<%=bm%>";
            });
            $("#btnEdit").bind("click", function () {
                var selectedRow = $("#dg").datagrid("getSelected");
                if (selectedRow != null) {
                    location.href = "KhzbEdit.aspx?mbid=<%=id%>&zbid=" + selectedRow.ID + "&khdzbid=<%=khdzbid%>&bm=<%=bm%>";
                }
                else {
                    alert("请选择要编辑的数据！");
                }
            });
            $("#btnDelete").bind("click", function () {
                var selectedRow = $("#dg").datagrid("getSelected");
                if (selectedRow != null) {
                    if (confirm("确认删除？")) {
                        $.ajax({
                            url: "data/KhHandler.ashx",
                            type: "post",
                            data: { type: "deleteZb", id: selectedRow.ID },
                            success: function (result) {
                                if (result != "") {
                                    alert(result);
                                }
                                else $("#dg").datagrid("reload");
                            }
                        });
                    }
                }
                else {
                    alert("请选中要删除的数据行！");
                }
            });
            $("#btnSure").bind("click", function () {
                var mbid = $("#selectMbList").combobox("getValue");
                if (mbid == null) {
                    alert("选择要继承的模板!");
                    return;
                }
                if (confirm("确认要继承模板，该项目下已存在的考核内容将被清除？")) {
                    $.ajax({
                        url: "data/KhHandler.ashx",
                        type: "post",
                        data: { type: "saveKhzbWithMb", Fmbid: mbid, Tmbid: "<%=id%>" },
                        success: function (result) {
                            if (result != "") {
                                alert(result);
                            }
                            else $("#dg").datagrid("reload");
                        }
                    });
                }
            });
            $("#btnBack").bind("click", function () {

                location.href = "/Module/Kh/KhbzAudit2.aspx?Id=<%=khdzbid%>&bm=<%=bm%>&khid=<%=khid%>";
            });
        });
        /**
       * 评分方式下拉框是否绑定选择改变事件
       */
        function selectBindEvent() {
            $("#selectType").combobox({
                onSelect: function (record) {
                    if (typeJs != null) {
                        tempTypeJs = typeJs;
                        if (confirm("确定要切换评分方式？")) {
                            $.ajax({
                                url: "data/KhHandler.ashx",
                                type: "post",
                                data: { type: "saveZbType", mbid: "<%=id%>", pftype: record.value },
                                success: function (result) {
                                    if (result != "") {
                                        alert(result);
                                    }
                                    else {
                                        location.reload();
                                        //$("#dg").datagrid("reload");
                                    }
                                }
                            });
                        }
                        else {
                            typeJs = null;
                            $("#selectType").combobox('select', tempTypeJs);
                            typeJs = tempTypeJs;
                        }
                    }
                }
            });
        }
        /**
        * EasyUI DataGrid根据字段动态合并单元格
        * 参数 tableID 要合并table的id
        * 参数 colList 要合并的列,用逗号分隔(例如："name,department,office");
        */
        function mergeCellsByField(tableID, colList) {
            var ColArray = colList.split(",");
            var tTable = $("#" + tableID);
            var TableRowCnts = tTable.datagrid("getRows").length;
            var tmpA;
            var tmpB;
            var PerTxt = "";
            var CurTxt = "";
            var alertStr = "";
            for (j = ColArray.length - 1; j >= 0; j--) {
                PerTxt = "";
                tmpA = 1;
                tmpB = 0;

                for (i = 0; i <= TableRowCnts; i++) {
                    if (i == TableRowCnts) {
                        CurTxt = "";
                    }
                    else {
                        CurTxt = tTable.datagrid("getRows")[i][ColArray[j]];
                    }
                    if (PerTxt == CurTxt) {
                        tmpA += 1;
                    }
                    else {
                        tmpB += tmpA;

                        tTable.datagrid("mergeCells", {
                            index: i - tmpA,
                            field: ColArray[j],　　//合并字段
                            rowspan: tmpA,
                            colspan: null
                        });
                        tTable.datagrid("mergeCells", { //根据ColArray[j]进行合并
                            index: i - tmpA,
                            field: "Ideparture",
                            rowspan: tmpA,
                            colspan: null
                        });
                        var tmpType = $("#selectType").combobox("getValue");
                        if ((tmpType == "xm1" && ColArray[j] == "XM1") || (tmpType == "xm2" && ColArray[j] == "XM2") || (tmpType == "nr" && ColArray[j] == "NR")) {
                            tTable.datagrid("mergeCells", {
                                index: i - tmpA,
                                field: "BZF",
                                rowspan: tmpA,
                                colspan: null
                            });
                            //tTable.datagrid("mergeCells", { //根据ColArray[j]进行合并
                            //    index: i - tmpA,
                            //    field: "Ideparture",
                            //    rowspan: tmpA,
                            //    colspan: null
                            //});
                        }

                        tmpA = 1;
                    }
                    PerTxt = CurTxt;
                }
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="p" class="easyui-panel" title="考核指标" icon="icon-save" collapsible="true" style="padding: 10px; background: #fafafa;">
            打分方式：<select id="selectType" class="easyui-combobox" style="width: 150px;cursor:pointer;" editable="false" disabled="disabled">
                <option value="xm1">按照考核项目1打分</option>
                <option value="xm2">按照考核项目2打分</option>
                <option value="nr">按照考核内容打分</option>
            </select>
            <%--继承模板：<input id="selectMbList" editable="false" style="width: 200px;" />
            <a id="btnSure" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-save'">确定</a>--%>
            <table id="dg"></table>
            <div id="tb">
                <%--<a href="#" id="btnAdd" class="easyui-linkbutton" style="margin-top: 2px" data-options="iconCls:'icon-add',plain:true">新增</a>
                <a href="#" id="btnEdit" class="easyui-linkbutton" style="margin-top: 2px" data-options="iconCls:'icon-edit',plain:true">编辑</a>
                <a href="#" id="btnDelete" class="easyui-linkbutton" data-options="iconCls:'icon-delete',plain:true">删除</a>--%>
                <a href="#" id="btnBack" class="easyui-linkbutton" data-options="iconCls:'icon-delete',plain:true">返回</a>
            </div>
        </div>
    </form>
</body>
</html>

