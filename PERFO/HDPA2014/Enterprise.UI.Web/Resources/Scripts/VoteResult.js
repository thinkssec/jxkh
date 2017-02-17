function loadchart() {
    var title = "投票结果";
    var xml = "<chart palette='2' caption='"+title+"' xAxisName='选项' yAxisName='票数' showValues='0' decimals='0' formatNumberScale='0'>>";
    var rows = $("#Result tr").length; //表格行数
    var cols = $("#Result tr:eq(0) th").length; //表格列数
    //alert(cols);
    //xml += "<categories>      <category label='Jan' />      <category label='Feb' />      <category label='Mar' />      <category label='Apr' />      <category label='May' />      <category label='Jun' />      <category label='Jul' />      <category label='Aug' />      <category label='Sep' />      <category label='Oct' />      <category label='Nov' />      <category label='Dec' />   </categories>";
    for (var i = 0; i < cols; i++) {
        var caption = $("#Result tr:eq(0) th").eq(i).text(); //表头

        xml += "<set label='" + caption + "'";
        for (var j = 1; j < rows; j++) {
            var val = $("#Result tr:eq(" + j + ") td").eq(i).text(); //数值
            xml += " value='" + val + "' />";
        }
    }
    xml += "</chart>";
    //alert(xml);
    chart(xml, "/Resources/FCF/Column2D.swf", 400, 260, "chartContainer2");
    hidegrid();
}

function hidegrid() {
    $("#Result").hide();
}

function chart(xml, swf, width, height, chartdiv) {
    var chartObj = new FusionCharts(swf, "chartId", width, height, "0", "1");
    chartObj.setDataXML(xml);
    //alert(xml);
    chartObj.configure("ChartNoDataText", "no data to display");
    chartObj.render(chartdiv);
}