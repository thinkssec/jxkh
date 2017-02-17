//回到顶部
function scrollTop() {
    $("html,body").animate({ scrollTop: 0 }, 500);
    return false;
}

function MenuOnMouseOver(obj) {
    //alert(obj);
    obj.className = 'menubar_button';
}

function MenuOnMouseOut(obj) {
    //alert(obj);
    obj.className = 'menubar_button_on';
}

var js = new function () {
    if (!objbeforeItem) { var objbeforeItem = null; var objbeforeItembackgroundColor = null; }
    this.ItemOver = function (obj) {
        if (objbeforeItem) { objbeforeItem.style.backgroundColor = objbeforeItembackgroundColor; }
        objbeforeItembackgroundColor = obj.style.backgroundColor;
        objbeforeItem = obj;
        obj.style.backgroundColor = "#FFFFE1";
    }
} 
