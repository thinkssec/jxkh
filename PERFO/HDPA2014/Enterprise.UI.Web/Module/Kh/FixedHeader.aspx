<%@ Page Title="" Language="C#" MasterPageFile="../Project.Master" AutoEventWireup="true"
    ValidateRequest="false" EnableEventValidation="false"
    CodeBehind="FixedHeader.aspx.cs" Inherits="Enterprise.UI.Web.Kh.FixedHeader" %>

<%@ Import Namespace="Enterprise.Model.Perfo.Kh" %>
<%@ Import Namespace="Enterprise.Component.Infrastructure" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/Resources/FixedHeader/javascripts/superTables.js"></script>
    <link href="/Resources/FixedHeader/stylesheets/superTables_Default.css" rel="Stylesheet" type="text/css" />
    <%--    <script type="text/javascript" src="/Resources/FixedHeader/javascripts/jquery.resizableColumns.js"></script>--%>
    <style type="text/css">
        .gridcell {
            padding: 5px;
        }

        .fakeContainer {
            float: left;
            margin: 5px;
            border: solid 1px #ccc;
            width: 630px;
            height: 250px;
            /*background-color: #ffffff;*/
            overflow: hidden;
        }
    </style>
    <script type="text/javascript">
        function checkForm() {
            return $("#form1").form('validate');
        }
        function showInfo(tt, msg) {
            $.messager.show({
                title: tt,
                msg: msg,
                height: '200',
                width: '400',
                showType: 'show',
                timeout: 30000,
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ProjectPH" runat="server">
    <div data-options="region:'center'">
        <div id="Div1" class="ssec-form">
            <div class="ssec-group ssec-group-hasicon">
                <div class="icon-form"></div>
                <span>固定表头和冻结列</span>
            </div>
        </div>
        <div class="main-gridview">
            <asp:GridView ID="GridView1" Width="100%" runat="server" CssClass="GridViewStyle" OnRowCreated="GridView1_RowCreated">
                <HeaderStyle CssClass="GridViewHeaderStyle" />
                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                <RowStyle CssClass="GridViewRowStyle" />
            </asp:GridView>
        </div>
    </div>
    <script type="text/javascript">
        //<![CDATA[
        var grid = document.getElementById("ProjectPH_GridView1");
        if (grid != null && grid != undefined) {
            grid.parentNode.className = "fakeContainer";

            (function () {
                var start = new Date();
                superTable("ProjectPH_GridView1", {
                    cssSkin: "Default",
                    // fixedColsdd  设置要固定的列数，这里设置固定3列
                    fixedCols: 3,
                    onFinish: function () {
                        // Basic row selecting for a superTable with/without fixed columns
                        if (this.fixedCols == 0) {
                            for (var i = 0, j = this.sDataTable.tBodies[0].rows.length; i < j; i++) {
                                this.sDataTable.tBodies[0].rows[i].onclick = function (i) {
                                    var clicked = false;
                                    var dataRow = this.sDataTable.tBodies[0].rows[i];
                                    return function () {
                                        if (clicked) {
                                            //dataRow.style.backgroundColor = "#ffffff";
                                            clicked = false;
                                        }
                                        else {
                                            //dataRow.style.backgroundColor = "#eeeeee";
                                            clicked = true;
                                        }
                                    }
                                }.call(this, i);
                            }
                        }
                        else {
                            for (var i = 0, j = this.sDataTable.tBodies[0].rows.length; i < j; i++) {
                                //xugang  begin
                                if (i % 2 == 0) {
                                    this.sDataTable.tBodies[0].rows[i].style.backgroundColor = "#f5ffef";
                                }
                                if (i >= (j - 2)) {
                                    this.sDataTable.tBodies[0].rows[i].style.backgroundColor = "#eeeeee"; //"#ffffd2";
                                }
                                //xugang  end

                                this.sDataTable.tBodies[0].rows[i].onclick = this.sFDataTable.tBodies[0].rows[i].onclick = function (i) {
                                    var clicked = false;
                                    var dataRow = this.sDataTable.tBodies[0].rows[i];
                                    var fixedRow = this.sFDataTable.tBodies[0].rows[i];

                                    //var dataRow_old_Color = dataRow.style.backgroundColor;
                                    //var fixedRow_old_Color = fixedRow.style.backgroundColor;
                                    return function () {
                                        if (clicked) {
                                            //dataRow.style.backgroundColor = fixedRow_old_Color;//"#ffffff";
                                            //fixedRow.style.backgroundColor = fixedRow_old_Color;//"#eeeeee";
                                            clicked = false;
                                        }
                                        else {
                                            //dataRow.style.backgroundColor = "#ffffd2";
                                            //fixedRow.style.backgroundColor = "#adadad";
                                            clicked = true;
                                        }
                                    }
                                }.call(this, i);
                            }
                        }
                        return this;
                    }
                });

                //$("#ProjectPH_GridView1").resizableColumns();

            })();
        }
        //]]>
    </script>
</asp:Content>
