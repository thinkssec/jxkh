<%@ Page Title="" Language="C#" MasterPageFile="../Project.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="EjdwJcsjData.aspx.cs" Inherits="Enterprise.UI.Web.Kh.EjdwJcsjData" %>

<%@ Import Namespace="Enterprise.Component.Infrastructure" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/Resources/Scripts/comm.component.js"></script>
    <script type="text/javascript">
        var isRowPaste = false;//行粘贴模式
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
        //保存数据
        function saveData() {
            $('#table1').datagrid('acceptChanges');
            $('#table2').datagrid('acceptChanges');
            if (confirm("您确定要保存数据吗？")) {
                var thisYearData = "";
                var rows1 = $("#table1").datagrid("getRows");
                for (var i = 0; i < rows1.length; i++) {
                    //获取每一行的数据
                    thisYearData += rows1[i]["xh"] + ":" + rows1[i]["m1"] + ":" + rows1[i]["m2"] + ":" + rows1[i]["m3"] + ":"
                        + rows1[i]["m4"] + ":" + rows1[i]["m5"] + ":" + rows1[i]["m6"] + ":" + rows1[i]["m7"] + ":"
                        + rows1[i]["m8"] + ":" + rows1[i]["m9"] + ":" + rows1[i]["m10"] + ":" + rows1[i]["m11"] + ":"
                        + rows1[i]["m12"] + ":" + rows1[i]["TJZ"] + "|";
                }
                $('#<%=Hid_JCSJData1.ClientID%>').val(thisYearData);

                var prevYearData = "";
                var rows2 = $("#table2").datagrid("getRows");
                for (var i = 0; i < rows2.length; i++) {
                    //获取每一行的数据
                    prevYearData += rows2[i]["xh"] + ":" + rows2[i]["m1"] + ":" + rows2[i]["m2"] + ":" + rows2[i]["m3"] + ":"
                        + rows2[i]["m4"] + ":" + rows2[i]["m5"] + ":" + rows2[i]["m6"] + ":" + rows2[i]["m7"] + ":"
                        + rows2[i]["m8"] + ":" + rows2[i]["m9"] + ":" + rows2[i]["m10"] + ":" + rows2[i]["m11"] + ":"
                        + rows2[i]["m12"] + ":" + rows2[i]["TJZ"] + "|";
                }
                $('#<%=Hid_JCSJData2.ClientID%>').val(prevYearData);
                showLoading();
                return true;
            }
            return false;
        }
        function setRowMode(obj) {
            isRowPaste = obj.checked;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ProjectPH" runat="server">
    <div data-options="region:'center'">
        <div id="Div1" class="ssec-form">
            <div class="ssec-group ssec-group-hasicon">
                <div class="icon-form"></div>
                <span>基础数据表——单位：</span>
                【<Ent:SSECDropDownList ID="Ddl_Danwei" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_Danwei_SelectedIndexChanged">
                </Ent:SSECDropDownList>】
                <asp:HiddenField ID="Hid_KHID" runat="server" />
                <asp:HiddenField ID="Hid_JGBM" runat="server" />
                <asp:HiddenField ID="Hid_JCSJData1" runat="server" />
                <asp:HiddenField ID="Hid_JCSJData2" runat="server" />
                <asp:HiddenField ID="Hid_TabTitle" runat="server" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Lbl_Msg" runat="server" Text="" ForeColor="Red"></asp:Label>
            </div>
        </div>
        <div class="main-gridview">
            <div id="tt" class="easyui-tabs" style="width: auto;">
                <div id="div1" title="本年数据〖<%=ThisYear %>〗" data-options="iconCls:'icon-application'" style="padding: 2px;z-index:10000;">
                    <table id="table1" style="width: auto; height: <%=TableHeight%>"
                        data-options="singleSelect:true,idField:'xh'">
                        <thead>
                            <tr>
                                <th colspan="13">金额(万元)</th>
                            </tr>
                            <tr>
                                <th data-options="field:'m1',width:70,align:'right',editor:{type:'numberbox',options:{precision:2}}">1月</th>
                                <th data-options="field:'m2',width:70,align:'right',editor:{type:'numberbox',options:{precision:2}}">2月</th>
                                <th data-options="field:'m3',width:70,align:'right',editor:{type:'numberbox',options:{precision:2}}">3月</th>
                                <th data-options="field:'m4',width:70,align:'right',editor:{type:'numberbox',options:{precision:2}}">4月</th>
                                <th data-options="field:'m5',width:70,align:'right',editor:{type:'numberbox',options:{precision:2}}">5月</th>
                                <th data-options="field:'m6',width:70,align:'right',editor:{type:'numberbox',options:{precision:2}}">6月</th>
                                <th data-options="field:'m7',width:70,align:'right',editor:{type:'numberbox',options:{precision:2}}">7月</th>
                                <th data-options="field:'m8',width:70,align:'right',editor:{type:'numberbox',options:{precision:2}}">8月</th>
                                <th data-options="field:'m9',width:70,align:'right',editor:{type:'numberbox',options:{precision:2}}">9月</th>
                                <th data-options="field:'m10',width:70,align:'right',editor:{type:'numberbox',options:{precision:2}}">10月</th>
                                <th data-options="field:'m11',width:70,align:'right',editor:{type:'numberbox',options:{precision:2}}">11月</th>
                                <th data-options="field:'m12',width:70,align:'right',editor:{type:'numberbox',options:{precision:2}}">12月</th>
                                <th data-options="field:'TJZ',width:70,align:'center',editor:{type:'numberbox',options:{precision:2}}">数额</th>
                            </tr>
                        </thead>
                    </table>
                </div>
                <div id="div2" title="上年数据〖<%=PrevYear %>〗" data-options="iconCls:'icon-application'" style="padding: 2px;z-index:10000;">
                    <table id="table2" style="width: auto; height: <%=TableHeight%>"
                        data-options="singleSelect:true,idField:'xh'">
                        <thead>
                            <tr>
                                <th colspan="13">金额(万元)</th>
                            </tr>
                            <tr>
                                <th data-options="field:'m1',width:70,align:'right',editor:{type:'numberbox',options:{precision:2}}">1月</th>
                                <th data-options="field:'m2',width:70,align:'right',editor:{type:'numberbox',options:{precision:2}}">2月</th>
                                <th data-options="field:'m3',width:70,align:'right',editor:{type:'numberbox',options:{precision:2}}">3月</th>
                                <th data-options="field:'m4',width:70,align:'right',editor:{type:'numberbox',options:{precision:2}}">4月</th>
                                <th data-options="field:'m5',width:70,align:'right',editor:{type:'numberbox',options:{precision:2}}">5月</th>
                                <th data-options="field:'m6',width:70,align:'right',editor:{type:'numberbox',options:{precision:2}}">6月</th>
                                <th data-options="field:'m7',width:70,align:'right',editor:{type:'numberbox',options:{precision:2}}">7月</th>
                                <th data-options="field:'m8',width:70,align:'right',editor:{type:'numberbox',options:{precision:2}}">8月</th>
                                <th data-options="field:'m9',width:70,align:'right',editor:{type:'numberbox',options:{precision:2}}">9月</th>
                                <th data-options="field:'m10',width:70,align:'right',editor:{type:'numberbox',options:{precision:2}}">10月</th>
                                <th data-options="field:'m11',width:70,align:'right',editor:{type:'numberbox',options:{precision:2}}">11月</th>
                                <th data-options="field:'m12',width:70,align:'right',editor:{type:'numberbox',options:{precision:2}}">12月</th>
                                <th data-options="field:'TJZ',width:70,align:'center',editor:{type:'numberbox',options:{precision:2}}">数额</th>
                            </tr>
                        </thead>
                    </table>
                </div>
            </div>
        </div>
        <asp:Panel ID="Pnl_Edit" runat="server" Visible="false">
            <div style="background: #C9EDCC; padding: 4px; width: auto;">
                <asp:LinkButton ID="LnkBtn_Ins" runat="server" class="easyui-linkbutton" iconCls="icon-save"
                    OnClientClick="return saveData();" OnClick="LnkBtn_Ins_Click">保存数据</asp:LinkButton>
                <%--&nbsp;&nbsp;
                <a href="<%=BackUrl %>" class="easyui-linkbutton" iconcls="icon-back">返回</a>--%>
                &nbsp;&nbsp;
                <asp:CheckBox ID="Chk_Expand" runat="server" Text="展开"
                    OnCheckedChanged="Chk_Expand_CheckedChanged" AutoPostBack="true" />
                &nbsp;&nbsp;
            <input id="Chk_Excel" type="checkbox" onclick="setRowMode(this);" />启用行粘贴
            &nbsp;&nbsp;提示：启用行粘贴后，请选中某一行数据，可支持EXCEL行数据粘贴(无法单个修改)!
            </div>
        </asp:Panel>
    </div>
    <script type="text/javascript">
        $('#tt').tabs({
            onSelect: function (title, index) {
                $('#tt').tabs('getTab', index).show();
                $('#<%=Hid_TabTitle.ClientID%>').val(title);
            }
        });
        //本年数据相关粘贴-------------------------------------------------------
        var selectRowIndex1;
        var lastIndex1;
        function div1OnPaste(event) {
            if (isRowPaste) {
                event = EventUtil.getEvent(event);
                EventUtil.preventDefault(event);//先取消
                $('#table1').datagrid('acceptChanges');
                var clipStr = EventUtil.getClipboardText(event);
                if (clipStr) {
                    var rowStrs = clipStr.split("\n"); //获取行数
                    if (rowStrs && rowStrs.length > 1) {
                        var rowIndex1 = selectRowIndex1;
                        for (var i = 0; i < rowStrs.length - 1; i++) {
                            var colStrs = rowStrs[i].split("	");
                            //var selected = $('#table1').datagrid('getSelected');
                            var rows1 = $("#table1").datagrid("getRows");
                            if (rowIndex1 < rows1.length) {
                                for (var j = 0; j <= colStrs.length - 1; j++) {
                                    if (!isNaN(colStrs[j])) {
                                        rows1[rowIndex1]["m" + (j + 1)] = colStrs[j];
                                    }
                                }
                                $('#table1').datagrid('refreshRow', rowIndex1);
                                rowIndex1++;
                            }
                        }
                    }
                }
            }
        }
        //上年数据相关粘贴---------------------------------------------------------
        var selectRowIndex2;
        var lastIndex2;
        function div2OnPaste(event) {
            if (isRowPaste) {
                event = EventUtil.getEvent(event);
                EventUtil.preventDefault(event);//先取消
                $('#table2').datagrid('acceptChanges');
                var clipStr = EventUtil.getClipboardText(event);
                if (clipStr) {
                    var rowStrs = clipStr.split("\n"); //获取行数
                    if (rowStrs && rowStrs.length > 1) {
                        var rowIndex2 = selectRowIndex2;
                        for (var i = 0; i < rowStrs.length - 1; i++) {
                            var colStrs = rowStrs[i].split("	");
                            //var selected = $('#table1').datagrid('getSelected');
                            var rows2 = $("#table2").datagrid("getRows");
                            if (rowIndex2 < rows2.length) {
                                for (var j = 0; j <= colStrs.length - 1; j++) {
                                    if (!isNaN(colStrs[j])) {
                                        rows2[rowIndex2]["m" + (j + 1)] = colStrs[j];
                                    }
                                }
                                $('#table2').datagrid('refreshRow', rowIndex2);
                                rowIndex2++;
                            }
                        }
                    }
                }
            }
        }
        $(function () {
            $('#tt').tabs('select', '<%=TabTitle%>');
            //本年数据--------------------------------------------------------------------------
            var now = new Date();
            var number1 = now.getSeconds() % 100;
            var url1 = '/Module/Kh/KhJcsjDataHandler.ashx?BM=<%=Jgbm%>&NF=<%=ThisYear%>&LX=Select&Rnd=' + number1;
            $('#table1').datagrid({
                url: url1,
                singleSelect: true,
                frozenColumns: [[
	                { field: 'xh', title: '序号', width: 40 },
	                { field: 'zbmc', title: '指标名称', width: 170 }
                ]],
                rownumbers: false,
                method: 'get',
                onBeforeLoad: function () {
                    $(this).datagrid('rejectChanges');
                },
                onSelect: function (index, row) {
                    selectRowIndex1 = index;
                },
                onClickRow: function (index, row) {
                    if (!isRowPaste) {
                        $('#table1').datagrid('endEdit', lastIndex1);
                        $('#table1').datagrid('beginEdit', index);
                    }
                    lastIndex1 = index;
                }
            });
            var div1 = document.getElementById('div1');
            EventUtil.addHandler(div1, "paste", function (event) {
                //alert(GetClipboard(event));
                div1OnPaste(event);
            });
            EventUtil.addHandler(div1, "keyup", function (event) {
                //处理粘贴板数据,Ctrl+V
                if (event.ctrlKey && event.keyCode == 86) {
                    div1OnPaste(event);
                }
            });
            //上年数据--------------------------------------------------------------------------
            var number2 = now.getSeconds() % 100;
            var url2 = '/Module/Kh/KhJcsjDataHandler.ashx?BM=<%=Jgbm%>&NF=<%=PrevYear%>&LX=Select&Rnd=' + number2;
            $('#table2').datagrid({
                url: url2,
                singleSelect: true,
                frozenColumns: [[
	                { field: 'xh', title: '序号', width: 40 },
	                { field: 'zbmc', title: '指标名称', width: 170 }
                ]],
                rownumbers: false,
                method: 'get',
                onBeforeLoad: function () {
                    $(this).datagrid('rejectChanges');
                },
                onSelect: function (index, row) {
                    selectRowIndex2 = index;
                },
                onClickRow: function (index, row) {
                    if (!isRowPaste) {
                        $('#table2').datagrid('endEdit', lastIndex2);
                        $('#table2').datagrid('beginEdit', index);
                    }
                    lastIndex2 = index;
                }
            });
            var div2 = document.getElementById('div2');
            EventUtil.addHandler(div2, "paste", function (event) {
                //alert(GetClipboard(event));
                div2OnPaste(event);
            });
            EventUtil.addHandler(div2, "keyup", function (event) {
                //处理粘贴板数据,Ctrl+V
                if (event.ctrlKey && event.keyCode == 86) {
                    div2OnPaste(event);
                }
            });
        });
    </script>
</asp:Content>
