function show_win(title, _url, _width, _height, _modal) {
    $('#win').empty();
    $('#win').dialog({
        title: title,
        width: _width,
        height: _height,
        modal: _modal,
        href:_url,
        shadow: false,
        closed: false
    });
}

function show_win_inframe(_title, _url, _width, _height, _modal) {
    $('#win').empty();
    $('#win').window({
        title: _title,
        width: _width,
        height: _height,
        modal: _modal,
        shadow: false,
        closed: false
    });
    $('#win').window('open');
    $('#win').append("<iframe src='" + _url + "' frameborder='0' scrolling='no' border='0' width='100%' height='100%'></iframe>");
}

function close_win(rtn_value,refresh) {
    parent.$('#win').window('close');    
    if (refresh=="True") {
        parent.location.reload();
    }
    //parent.$('#rtn_value').attr("value", rtn_value);
    parent.window.poprtn(rtn_value);
}