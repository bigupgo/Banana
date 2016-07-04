
/*  
*     创建 者 ： 吴优
*     创建时间： 2016-03-18
*     备    注： 
*/

//####################################### 功能介绍 ##################################################
//#
//#                                 自定义标签选择器功能
//#
//###################################################################################################

var findHasExist = null;

//触发样式：选中或取消
function ClickForChoicedToggled($nodeLable, selectedCss, noSelectCss) {
    //console.log($nodeLable);
    //console.log("*节点需要变换操作!");
    //关闭span 标记
    var closeSpanChildren = $nodeLable.find(".SpanCloseParent").first();

    $nodeLable.click(function () {
        $nodeLable.toggleClass(noSelectCss);
        $nodeLable.toggleClass(selectedCss);
        //当前元素是否被选中的属性
        var isSelectedElement = $nodeLable.attr("isChecked");
        ////// isSelectedElement = isSelectedElement ? false : true;
        switch (isSelectedElement) {
            case "true":  //// $(this).append("<span id='close" + _PKID  + "'>×</span>");
                $nodeLable.attr("isChecked", "false")
                closeSpanChildren.hide(200);
                break;
            case "false":
                $nodeLable.attr("isChecked", "true")
                closeSpanChildren.show(200);
                break;
            default:
        }
    });
}
//触发样式：选中则删除
function ClickForChoicedNodeDelete($nodeLable, selectedCss, noSelectCss) {
    $nodeLable.click(function () {
        $nodeLable.hide(100);
        setInterval(function () {
            $nodeLable.remove();
        }, 200)
    });

}


//指定一个$区域对象，将控件填充到里面
function RendToLabelSelectorToDiv(UniquePKID, nameTitle, $toRendContainerDiv, mustCheckExist, callBack, _selectedCss, _noSelectCss) {
    UniquePKID = $.trim(UniquePKID);
    nameTitle = $.trim(nameTitle);
    //强制要求检查是否重复
    if (mustCheckExist) {
        findHasExist = $toRendContainerDiv.find("label[pkid=\"" + UniquePKID + "\"]");
        //发现重复对象并提示
        if (findHasExist.length > 0) { return false; }
    }
    var _$spanStr = null;
    var _$labelStr = null;
    _$labelStr = $("<label class=\"SelectorDefault " + _selectedCss + "\" pkid=\"" + UniquePKID + '" pname="' + nameTitle + "\" ischecked=\"true\">" + nameTitle + "</label>");
   
    _$labelStr.appendTo($toRendContainerDiv);
    // console.log("=======在指定区域找刚生成的节点对象 ============");
    findHasExist = $toRendContainerDiv.find("label[pkid=\"" + UniquePKID + "\"]");
    //找到后，如果参数调用回发 callBack 为指定的2个动作：变换，节点删除
    if (findHasExist.length > 0 && callBack) {
        callBack(findHasExist, _selectedCss, _noSelectCss)
    }
    return true;
}