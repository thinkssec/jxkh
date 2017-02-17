//开发生产月度曲线
function loadIndexChartForMonth() {
    chart(monthXml, "/Resources/FCF/MSColumn2DLineDY.swf", 570, 320, "chartContainer2");
    hidegrid();
}

//全年累计,计划与完成对比
function loadMonthTotalChart() {
    chart(monthTotalXml, "/Resources/FCF/MSColumn2DLineDY.swf", 570, 320, "chartContainer2");
    hidegrid();
}

//开发生产日度曲线
function loadIndexChartForDay() {
    chart(dayXml, "/Resources/FCF/MSLine.swf", 570, 320, "chartContainer2");
    hidegrid();
}

//原油销售柱图
function loadXiaoshouWX() {
    chart(xiaoshouXml, "/Resources/FCF/MSColumn2D.swf", 570, 320, "chartContainer2");
    hidegrid();
}

//原油合计柱图
function loadXiaoshouNX() {
    //MSColumn2D.swf
    chart(xiaoshouHjXml, "/Resources/FCF/MSColumn2DLineDY.swf", 570, 320, "chartContainer2");
    hidegrid();
}

//原油销售价格曲线图
function loadXiaoshouJG() {
    chart(xiaoshouJgXml, "/Resources/FCF/MSLine.swf", 570, 320, "chartContainer2");
    hidegrid();
}

//钻井月度进尺曲线
function loadZuanjingChart() {
    //alert(zuanjingXml);
    chart(zuanjingXml, "/Resources/FCF/MSColumn2DLineDY.swf", 570, 320, "chartContainer2");
    hidegrid();
}

//AFE统计
function loadAFEChart() {
    chart(afeXml, "/Resources/FCF/MSColumn2D.swf", 570, 320, "chartContainer2");
    hidegrid();
}

//差旅统计
function loadBusitravelChart() {
    //alert(busitravelXml);
    chart(busitravelXml, "/Resources/FCF/MSColumn2D.swf", 570, 320, "chartContainer2");
    hidegrid();
}

//查询全员绩效指定状态下的人员信息
function showStaffInfoByZT(zt) {
    $.ajax({
        type: "post",
        url: "/Component/BF/BFLoadHandler.ashx?TypeId=5&khzt=" + zt,
        datatype: "text",
        async: true,
        success: function (result) {
            $('#showSpan').html(result);
            $('#showResultWindow').window('open');
        }
    });
}

//全员绩效考核
function loadStaffChart() {
    chart(staffXml, "/Resources/FCF/Pie3D.swf", 570, 320, "chartContainer2");
    hidegrid();
}

function hidegrid() {
    $("#gv").hide();
    //$("#chanliang").hide();
}

function chart(xml, swf, width, height, chartdiv) {
    var chartObj = new FusionCharts(swf, "chartId", width, height, "0", "1");
    chartObj.setDataXML(xml);
    //alert(xml);
    chartObj.configure("ChartNoDataText", "no data to display");
    chartObj.render(chartdiv);
}

//***********************************************************************************************
//保存用户图表设置
function SaveUserCharts(loginName) {
    var userChartValue = 0;
    var userChartIndexs = "";
    var optLst = $("#MainPH_SelMyChart option");
    $.each(optLst, function (i, opt) {
        userChartValue += parseInt(opt.value);
        userChartIndexs += opt.value + "|";
    });
    //alert("len==" + optLst.length + "==" + userChartValue + "==" + userChartIndexs);
    $('#w').window('close');
    if (userChartValue > 0) {
        $.ajax({
            type: "post",
            url: "/Component/BF/BFLoadHandler.ashx?TypeId=4&user=" + loginName + "&uchartv=" + userChartValue + "&uchartIds=" + userChartIndexs,
            datatype: "text",
            async: true,
            success: function (result) {
                window.location.reload(true);
            }
        });
    }
}

//添加选项到我的图表列表内
function chartInsert() {
    var optLst = $("#MainPH_SelChart").find("option:selected");
    $.each(optLst, function (i, opt) {
        //在SelMyChart中查找是否已存在，如果不存在才能添加
        var isExist = false;
        var myOptLst = $("#MainPH_SelMyChart option");
        $.each(myOptLst, function (j, myOpt) {
            if (myOpt.value == opt.value) {
                isExist = true;
            }
        });
        if (!isExist) {
            $("#MainPH_SelMyChart").append("<option value='" + opt.value + "'>" + opt.text + "</option>"); //添加一项option
        }
    });
}

//移除我的图表列表内的选项
function chartRemove() {
    var optLst = $("#MainPH_SelMyChart").find("option:selected");
    $.each(optLst, function (i, opt) {
        $("#MainPH_SelMyChart option[value='" + opt.value + "']").remove();        
    });
}

//调整图表顺序--上升
function chartUp() {
    var myOptLst = $("#MainPH_SelMyChart option");
    $.each(myOptLst, function (j, myOpt) {
        if (myOpt.selected && j > 0) {
            //和上一个元素互换
            var prevOpt = myOptLst[j - 1];
            var t = prevOpt.text;
            var v = prevOpt.value;
            prevOpt.text = myOpt.text;
            prevOpt.value = myOpt.value;
            myOpt.text = t;
            myOpt.value = v;
            $("#MainPH_SelMyChart option[value='" + prevOpt.value + "']").attr("selected", true); 
            return;
        }
    });
}

//调整图表顺序--下降
function chartDown() {
    var myOptLst = $("#MainPH_SelMyChart option");
    $.each(myOptLst, function (j, myOpt) {
        if (myOpt.selected && j < myOptLst.length - 1) {
            //和上一个元素互换
            var nextOpt = myOptLst[j + 1];
            var t = nextOpt.text;
            var v = nextOpt.value;
            nextOpt.text = myOpt.text;
            nextOpt.value = myOpt.value;
            myOpt.text = t;
            myOpt.value = v;
            //$("#MainPH_SelMyChart option[value='" + nextOpt.value + "']").attr("selected", true);
            return;
        }
    });
}