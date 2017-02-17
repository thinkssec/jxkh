<%@ Page Title="" Language="C#" MasterPageFile="../Project.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="CxtjKhjgHistory.aspx.cs" Inherits="Enterprise.UI.Web.Kh.CxtjKhjgHistory" %>

<%@ Import Namespace="Enterprise.Component.Infrastructure" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/Resources/Scripts/comm.component.js"></script>
    <script type="text/javascript">
        window.onerror = function () {
            return true;
        };
        function showLoading() {
            var win = $.messager.progress({
                title: '请您稍侯',
                msg: '正在提交数据...'
            });
            setTimeout(function () {
                $.messager.progress('close');
            }, 10000)
        }
        function Save_Excel() {//导出Excel文件
            //getExcelXML有一个JSON对象的配置，配置项看了下只有title配置，为excel文档的标题
            var now = new Date();
            var number1 = now.getSeconds() % 100;
            var data = $('#table1').datagrid('getExcelXml', { title: 'datagrid import to excel' }); //获取datagrid数据对应的excel需要的xml格式的内容
            //用ajax发动到动态页动态写入xls文件中
            var url1 = '/Module/Kh/KhDataHandler.ashx?LX=XLS&Rnd=' + number1;
            $.ajax({
                url: url1, data: { data: data }, type: 'POST', dataType: 'text',
                success: function (fn) {
                    alert('导出excel成功！');
                    window.location = "/Module/Kh/" + fn; //执行下载操作
                },
                error: function (xhr) {
                    alert('动态页有问题\nstatus：' + xhr.status + '\nresponseText：' + xhr.responseText)
                }
            });
        }
        function searchData() {
            //历次考核结果数据
            var now = new Date();
            var number1 = now.getSeconds() % 100;
            var khqv = $('#Sel_Kaohe').combobox('getValues');
            var khqs = $('#Sel_Kaohe').combobox('getText').split(',');
            var cols = [[]];
            cols[0][cols[0].length] = { field: 'zblb', title: '指标类别', width: 80 };
            cols[0][cols[0].length] = { field: 'zbmc', title: '指标名称', width: 170 };
            cols[0][cols[0].length] = { field: 'zbbb', title: '指标版本', width: 110 };
            for (var i = 0; i < khqs.length; i++) {
                var o = { field: 'attr' + i + '', title: '' + khqs[i] + '', width: 200 };
                cols[0][cols[0].length] = o;
            }
            khqv += "|" + $('#ProjectPH_Ddl_Danwei').val();
            var url1 = '/Module/Kh/KhDataHandler.ashx?LX=CXJG&PV=' + khqv + '&Rnd=' + number1;
            //alert(url1);
            $('#table1').datagrid({
                url: url1,
                idField: "id",
                columns: cols,
                //frozenColumns: [[
                //    { field: 'zblb', title: '指标类别', width: 80 },
	            //    { field: 'zbmc', title: '指标名称', width: 170 },
                //    { field: 'zbbb', title: '指标版本', width: 110 }
                //]],
                rownumbers: true,
                method: 'get',
                onBeforeLoad: function () {
                    $(this).datagrid('rejectChanges');
                },
                onLoadSuccess: function () {
                    //var tableHeight = $('#table1').height();
                    //alert(tableHeight);
                    //tb.css("height", "379");
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ProjectPH" runat="server">
    <asp:HiddenField ID="Hid_KHID" runat="server" />
    <asp:HiddenField ID="Hid_JGBM" runat="server" />
    <asp:HiddenField ID="Hid_JCSJData1" runat="server" />
    <asp:HiddenField ID="Hid_JCSJData2" runat="server" />
    <asp:HiddenField ID="Hid_TabTitle" runat="server" />
    <div data-options="region:'center'">
        <%--<div id="Div1" class="ssec-form">
            <div class="ssec-group ssec-group-hasicon">
                <div class="icon-form"></div>
                <span>历次考核结果对比【按单位】</span>
            </div>
        </div>--%>
        <div class="main-gridview">
            <div class="main-gridview-title">
                二级单位：<Ent:SSECDropDownList ID="Ddl_Danwei" runat="server">
                </Ent:SSECDropDownList>
                &nbsp;&nbsp;考核期：
                <select id="Sel_Kaohe" name="Sel_Kaohe" style="width: 250px;" class="easyui-combobox">
                </select>
                &nbsp;&nbsp;
                <a href="#" class="easyui-linkbutton" plain="true" iconcls="icon-search" onclick="searchData();">查询</a>
                &nbsp;&nbsp;
                <a href="#" class="easyui-linkbutton" plain="true" iconcls="icon-xls" onclick="Save_Excel();">导出</a>
            </div>
            <div id="tt" class="easyui-tabs" style="width: auto;">
                <div id="div1" title="历次考核结果统计" data-options="iconCls:'icon-application'" style="padding: 2px; z-index: 10000;">
                    <table id="table1" style="width: auto; height: <%=TableHeight%>;" data-options="fitColumns:false,singleSelect:true">
                    </table>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $('#tt').tabs({
            onSelect: function (title, index) {
                $('#tt').tabs('getTab', index).show();
                $('#<%=Hid_TabTitle.ClientID%>').val(title);
            }
        });
        $(function () {
            $('#tt').tabs('select', '<%=TabTitle%>');
            var now = new Date();
            var number2 = now.getSeconds() % 100;
            //考核期下拉选择
            $('#Sel_Kaohe').combobox({
                url: '/Module/Kh/KhDataHandler.ashx?LX=KHQH&PV=LX2014A&Rnd=' + number2,
                valueField: 'id',
                textField: 'text',
                multiple: true
            });
        });
    </script>
</asp:Content>
