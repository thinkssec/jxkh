$('.htmlgrid tbody tr th:eq(0)').text("批复时间");

var cols = $('.htmlgrid tbody tr th').length;
var rows = $('.htmlgrid tbody tr').length;

for (var i = 1; i < cols; i++) {
    for (var j = 1; j < rows; j++) {
        year1 = $('.htmlgrid tbody tr:eq(' + j + ') td:eq(0)').text();
        year2 = year1;
//        var txt = "<a href=\"#\" onclick=\"alert('dfdfdf');return false;\">" + year1 + "</a>";
//        $('.htmlgrid tbody tr:eq(' + j + ') td:eq(0)').html(txt);
        var txt = $('.htmlgrid tbody tr:eq(' + j + ') td:eq(' + i + ')').text();
        txt = "<a href='/Modules/App/Jhb/JhbM01_JDBView.aspx?y1=" + year1 + "-01-01&y2=" + year2 + "-12-31&w1=&w2=' target='_parent'>" + txt + "</a>";
        $('.htmlgrid tbody tr:eq(' + j + ') td:eq(' + i + ')').html(txt)
    }
}

for(var i=1;i<$('.htmlgrid tbody tr th').length-1;i++) {
    header = $('.htmlgrid tbody tr th:eq(' + i + ')').text();
    header =header+"(金戈)";
    $('.htmlgrid tbody tr th:eq(' + i + ')').text(header);
}

//给年度增加链接，显示图表
for (var rIndex = 1; rIndex < rows; rIndex++) {
    var data = "";
    var year;
    for (var cIndex = 0; cIndex < cols -1; cIndex++) {
        year = $('.htmlgrid tbody tr:eq(' + rIndex + ') td:eq(0)').text();
        data = data + "|" + $('.htmlgrid tbody tr th:eq(' + cIndex + ')').text() + "," + $('.htmlgrid tbody tr:eq(' + rIndex + ') td:eq(' + cIndex + ')').text();
    }
    if (cols > 0) {
        var txt = "<a href=\"JhbChart.aspx?data=" + data + "\" target=\"_blank\">" + year + "</a>";
        $('.htmlgrid tbody tr:eq(' + rIndex + ') td:eq(0)').html(txt);
    }
}
