//Column Chart
function Column2DChart(caption, table_id, label_column_index, value_column_index, data_row_index, chartdiv, width, height) {
    var title = caption;
    var xml = "<graph caption='" + title + "' yAxisName='投稿数' showValues='0' showExportDataMenuItem='1' numdivlines='1' baseFontSize='11'  showYAxisValues='1' formatNumberScale='0' decimalPrecision='0'>";
    var rows = $("#" + table_id + " tr").length; //表格行数 yaxismaxvalue='100' yaxisminvalue='0' 
    var cols = $("#" + table_id + " tr:eq(0) th").length; //表格列数                   
    for (var i = data_row_index ; i < rows; i++) {
        var label = $("#" + table_id + " tr:eq(" + i + ") td").eq(label_column_index).text(); //Label
        var val = $("#" + table_id + "  tr:eq(" + i + ") td").eq(value_column_index).text(); //Value
        xml += " <set label='" + label + "' value='" + val + "'/>";
    }
    xml += "</graph>";
    var chartObj = new FusionCharts("/Resources/FCF/Column2D.swf", "chartId", width, height, "0", "1");
    chartObj.setDataXML(xml);
    //alert(xml);
    chartObj.configure("ChartNoDataText", "no data to display");
    chartObj.render(chartdiv);
}