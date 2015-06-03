var screenH = 0;
var screenW = 0;
var frameTitle = "";
var popWidth = 630;
var popHeight = 520;
var popWords = "";



var bolPopWindow = false;
//開啟Popup Window
function openPopUpWindow() {
    bolPopWindow = true;


    if (document.getElementById('divWait')) {

        document.documentElement.scrollTop = 0;
        document.body.scrollTop = 0;

 
        var obj = document.getElementById('divWait');
        obj.style.visibility = "visible";
        obj.style.top = 0 + 'px';
        obj.style.left = 0 + 'px';
        obj.style.cursor = "wait";


       // var h = document.documentElement.clientHeight - 0 + ;
       // var w = document.documentElement.clientWidth - 0;
        
        var w,h;
        if (self.innerHeight) // all except Explorer
        {
            w = self.innerWidth;
            h = self.innerHeight;
        }
        else if (document.documentElement && document.documentElement.clientHeight)
        // Explorer 6 Strict Mode
        {
            w = document.documentElement.clientWidth;
            h = document.documentElement.clientHeight;
        }
        else if (document.body) // other Explorers
        {
            w = document.body.clientWidth;
            h = document.body.clientHeight;
        }
        
      //  w = w - 18;
        obj.style.width = "100%";  //w + 'px';
        obj.style.height = "100%";// h + 'px';

        obj = window.frames["frameContent"].document.getElementById("divWait");
        if (window.parent.document.getElementById("frameContent").style.display!="none") {
            obj.style.visibility = "visible";
            obj.style.top = 0 + 'px';
            obj.style.left = 0 + 'px';
            obj.style.width = "100%";  //w + 'px';
            obj.style.height = "100%"; // h + 'px';
            obj.style.cursor = "wait";
            var objInside = window.frames["frameContent"].document.getElementById("divPopWindow");

            var l_Top = Math.round((h / 2) - 10 - (objInside.style.height / 2));
            var l_Left = Math.round((w / 2) - 100 - (objInside.style.width / 2));


            objInside.style.visibility = "visible";
           // objInside.style.top = l_Top + 'px';
           // objInside.style.left = l_Left + 'px';
        }

        obj = window.frames["frameDetail"].document.getElementById("divWait");
        if (window.parent.document.getElementById("frameDetail").style.display!="none") {
            obj.style.visibility = "visible";
            obj.style.top = 0 + 'px';
            obj.style.left = 0 + 'px';
            obj.style.width = "100%";  //w + 'px';
            obj.style.height = "100%"; // h + 'px';
            obj.style.cursor = "wait";
            
            var objInside = window.frames["frameDetail"].document.getElementById("divPopWindow");
            var hh=parseInt(objInside.style.height.replace("px",""));
            var ww=parseInt(objInside.style.width.replace("px",""));
            var l_Top = Math.round((h / 2) - 10 - (hh / 2));
            var l_Left = Math.round((w / 2) - 100 - (ww / 2));
          
            objInside.style.visibility = "visible";
           // objInside.style.top = l_Top + 'px';
          //  objInside.style.left = l_Left + 'px';
        }
        
       

    }

}



//關閉Popup Window
function closePopUpWindow() {
    bolPopWindow = false;
    if (document.getElementById('divWait')) {
        var obj;
        obj = document.getElementById('divWait');
        obj.style.visibility = "hidden";
    }

    var obj = window.frames["frameContent"].document.getElementById("divWait");
    if (obj) {
        obj.style.visibility = "hidden";
    }

     obj = window.frames["frameDetail"].document.getElementById("divWait");
    if (obj) {
        obj.style.visibility = "hidden";
    }
    
    var objInside = window.frames["frameContent"].document.getElementById("divPopWindow");
    if (objInside) {
       
        objInside.style.top = '-500px';
        objInside.style.left = "-500px";
        objInside.style.visibility = "hidden";
    }

    objInside = window.frames["frameDetail"].document.getElementById("divPopWindow");
    if (objInside) {

        objInside.style.top = '-500px';
        objInside.style.left = "-500px";
        objInside.style.visibility = "hidden";
    }

}


//sys:draw processing window 
function drawProcessing() {

    var win = $.messager.progress({
        title: '請稍候',
        msg: '資料正在處理中...'
    });
    //setTimeout(function() {
    //    $.messager.progress('close');
    //}, 5000)
    /*
    var obj = document.getElementById("divProc");

    screenH = window.screen.availHeight - 25 + document.documentElement.scrollTop;
    screenW = window.screen.availWidth - 20;

    obj.style.width = screenW + "px";
    obj.style.height = screenH + "px";
    obj.style.visibility = "visible";

    obj = window.document.getElementById("divProcessing");

    pw = 148;
    ph = 260;

    var l = (screenW - parseInt(pw)) / 2;
    var t = ((window.screen.height - parseInt(ph)) / 2) + document.documentElement.scrollTop;
    obj.style.height = ph + 'px';
    obj.style.width = pw + 'px';
    obj.style.top = t + 'px';
    obj.style.left = l + 'px';

    obj.style.visibility = "visible";*/

}


//sys:close processing window
function closeProcessing() {

    $.messager.progress('close');
    /*
    document.getElementById("divProc").style.width = '0px';
    document.getElementById("divProc").style.height = '0px';
    document.getElementById("divProc").style.visibility = "hidden";

    document.getElementById("divProcessing").style.visibility = "hidden";
*/


}

