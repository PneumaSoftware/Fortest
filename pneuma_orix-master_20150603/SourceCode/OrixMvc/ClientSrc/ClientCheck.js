/*
document.onkeyup = function() {

    if (window.event.keyCode == 13) {
        var obj = window.event.srcElement;
        if (obj.className.indexOf("combo-text") != -1) {
            $(':input:enabled').addClass('enterIndex');
            // get only input tags with class data-entry
            textboxes = $('.enterIndex');
            // now we check to see which browser is being used
            if ($.browser.mozilla) {
                $(textboxes).bind('keypress', CheckForEnter);
            } else {
                $(textboxes).bind('keydown', CheckForEnter);
            }
        }
        return false;
    }

}*/

function validateNumber(evt) {
    var theEvent = evt || window.event;
    var key = theEvent.keyCode || theEvent.which;
    key = String.fromCharCode(key);
    var regex = /[0-9]|\./;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
}



var btnQuery = "";

window.onload=window_load;

//sys:設定tabIndex資訊 & 選取查詢明細
function window_load() {

    var objs = window.document.getElementsByTagName("Input");
    var obj;
    for (var i = 0; i < objs.length; i++) {
        obj = objs.item(i);
        if (obj.className.indexOf("slock") != -1 || obj.className.indexOf("display") != -1 || (obj.type.toUpperCase() != "TEXT" && obj.id.indexOf("btnQuery") == -1 && obj.id.indexOf("btnSave") == -1 && obj.id.indexOf("btnPrint") == -1)) 
        {
            obj.tabIndex = "9999";
        }

        

   } 
}

/*easy-ui*/

/*start date*/
//驗證日期
function chkDate(obj) {

    if (obj.readOnly)
        return true;
        
    var newValue = obj.value;

    var dt = newValue.replace(/\//g, "").replace(/_/g, "");

    //alert(dt);
    if (dt == "")
        return true;

    if (dt.length < 8) {
         alert("日期格式錯誤！");
        obj.select(); ;
        return false;
        }

    var newDt = ""
    var target = event.target || event.srcElement;
    try {

        var y = dt.substr(0, 4);
        var m = dt.substr(4, 2);
        var d = dt.substr(6, 2);

        newDt = new Date(y + "/" + m + "/" + d);
        dt = y + "/" + m + "/" + d;
        if (parseInt(m) == 0) m = m.substr(1, 1);
        if (parseInt(d) == 0) d = d.substr(1, 1);

        if (newDt.getFullYear() != parseInt(y) || newDt.getDate() != parseInt(d) || newDt.getMonth() != parseInt(m) - 1) {
            alert("日期格式錯誤！");
            obj.select(); ;
            return false;
        }
    }
    catch (Error) {
        alert("日期格式錯誤！");
        obj.select(); ;
        return false;
    }
    finally { }


    //target.value = dt;

    return true; 

}
/*end date*/


/*start time*/
//驗證時間
function checkTime(val) {

    if (val=="")
        return "";

    var dt = val.replace(/:/g, "");
    if (dt == "")
        return "";

    if (dt.length == 3) {
        dt = "0" + dt;
    }

    if (dt.length != 4) {
        alert("時間格式錯誤！");
        
        return "";
    }

    try {
        var m = parseInt(dt.substr(0, 2));
        var s = parseInt(dt.substr(2, 2));

        if (m > 23 || m < 0 || s > 59 || s < 0) {
            alert("時間格式錯誤！");
        
            return "";
        }
    }
    catch (Error) {
        alert("時間格式錯誤！");
        
        return "";
    }
    finally { }

    
    return dt.substr(0, 2) + ":" + dt.substr(2, 2)

    //return true;    
    

}

/*start time*/
//驗證時間
function chkTime(obj) {

    if (obj.readOnly)
        return true;

    var dt = obj.value.replace(/:/g, "");
    if (dt == "")
        return true;

    if (dt.length == 3) {
        dt = "0" + dt;
    }

    if (dt.length != 4) {
        alert("時間格式錯誤！");
        obj.select(); ;
        return false;
    }

    try {
        var m = parseInt(dt.substr(0, 2));
        var s = parseInt(dt.substr(2, 2));

        if (m > 23 || m < 0 || s > 59 || s < 0) {
            alert("時間格式錯誤！");
            obj.select();
            return false;
        }
    }
    catch (Error) {
        alert("時間格式錯誤！");
        obj.select(); ;
        return false;
    }
    finally { }

    
    obj.value = dt.substr(0, 2) + ":" + dt.substr(2, 2)

    return true;    
    

}
/*end time*/


function dateToYMD(date) {
    var d = date.getDate();
    var m = date.getMonth() + 1;
    var y = date.getFullYear();
    return '' + y + '/' + (m<=9 ? '0' + m : m) + '/' + (d <= 9 ? '0' + d : d);
}

//驗證年月
function isDate(obj) {
    if (obj.value=="")
        return;
        
     var dt = obj.value.replace(/\//g, "").replace(/_/g, "");


        var y = dt.substr(0, 4);
        var m = dt.substr(4, 2);
        var d = dt.substr(6, 2);
       
        obj.value=y + "/" + m + "/" + d;
}

//驗證年月
function isYearMonth(obj) {
    var dt = obj.value.replace(/\//g, "");
    if (dt == "")
        return true;


    if (dt.length != 6) {
        alert("年月格式錯誤！");
        obj.select();
        return false;
    }

    var newDt = ""

    try {
        var y = dt.substr(0, 4);
        var m = dt.substr(4, 2);
        var d = "01";

        newDt = new Date(y + "/" + m + "/" + d);

        if (parseInt(m) == 0) m = m.substr(1, 1);
        if (parseInt(d) == 0) d = d.substr(1, 1);

        if (newDt.getFullYear() != parseInt(y) || newDt.getDate() != parseInt(d) || newDt.getMonth() != parseInt(m) - 1) {
            alert("年月格式錯誤！");
            obj.select();
            return false;
        }
    }
    catch (Error) {
        alert("年月格式錯誤！");
        obj.select();
        return false;
    }
    finally { }

    obj.value = dt.substr(0, 4) + "/" + dt.substr(4, 2);

    return true;
}

function chkIDandGUI(obj, name)
{
    if (!checkIDNo(obj, "", false)) {
        return chkGUINo(obj, name);
    }
    return true;
}

//驗證身份證號
function isIDNo(obj)
{
    idStr=trim(obj.value);
 
    if (idStr=="")// || idStr.substring(0,1)=="*")   
        return true;
    
     var acc = 0;
        d0 = idStr.charAt(0);
        d1 = idStr.charAt(1);
        d2 = idStr.charAt(2);
        d3 = idStr.charAt(3);
        d4 = idStr.charAt(4);
        d5 = idStr.charAt(5);
        d6 = idStr.charAt(6);
        d7 = idStr.charAt(7);
        d8 = idStr.charAt(8);
        d9 = idStr.charAt(9);
        if ((d0 == "A") || (d0 == "a")) { acc = 10; }
        else if ((d0 == "B") || (d0 == "b")) { acc = 11; }
        else if ((d0 == "C") || (d0 == "c")) { acc = 12; }
        else if ((d0 == "D") || (d0 == "d")) { acc = 13; }
        else if ((d0 == "E") || (d0 == "e")) { acc = 14; }
        else if ((d0 == "F") || (d0 == "f")) { acc = 15; }
        else if ((d0 == "G") || (d0 == "g")) { acc = 16; }
        else if ((d0 == "H") || (d0 == "h")) { acc = 17; }
        else if ((d0 == "J") || (d0 == "j")) { acc = 18; }
        else if ((d0 == "K") || (d0 == "k")) { acc = 19; }
        else if ((d0 == "L") || (d0 == "l")) { acc = 20; }
        else if ((d0 == "M") || (d0 == "m")) { acc = 21; }
        else if ((d0 == "N") || (d0 == "n")) { acc = 22; }
        else if ((d0 == "P") || (d0 == "p")) { acc = 23; }
        else if ((d0 == "Q") || (d0 == "q")) { acc = 24; }
        else if ((d0 == "R") || (d0 == "r")) { acc = 25; }
        else if ((d0 == "S") || (d0 == "s")) { acc = 26; }
        else if ((d0 == "T") || (d0 == "t")) { acc = 27; }
        else if ((d0 == "U") || (d0 == "u")) { acc = 28; }
        else if ((d0 == "V") || (d0 == "v")) { acc = 29; }       
        else if ((d0 == "X") || (d0 == "x")) { acc = 30; }
        else if ((d0 == "Y") || (d0 == "y")) { acc = 31; }
        else if ((d0 == "W") || (d0 == "w")) { acc = 32; }
        else if ((d0 == "Z") || (d0 == "z")) { acc = 33; }
        else if ((d0 == "I") || (d0 == "i")) { acc = 34; }
        else if ((d0 == "O") || (d0 == "o")) { acc = 35; }
        if (acc == 0) {
               // alert("請輸入『身份證號碼』的第一個英文字母！");
                return false;
        } else {
                accstr = new String(acc);
                acc_1 = (accstr).charAt(0);
                acc_2 = (accstr).charAt(1);
                certsum = 1*acc_1 + 9*acc_2 + 8*d1 + 7*d2 + 6*d3 + 5*d4 + 4*d5 + 3*d6 + 2*d7 + 1*d8;
                certsum_2 = parseInt(certsum%10);
                certsum_3 = 10 - certsum_2;
                if (certsum_3==10) 
                    certsum_3=0;
                    
                if (d9 != certsum_3) {
                       // alert("請檢查『身份證號碼』是否輸入錯誤！");
                        return false;
                }
        }

    return true;

}

//驗證統一編號
function chkGUINo(obj, name) {
    var No = trim(obj.value);



    if (No == "")// || No.substring(0,1)=="*")   
        return true;

    if (No.length != 8) {
        alert(name + "錯誤");
        obj.select(); ;
        return false;
    }
    if (!isNumber(No)) {
        alert(name + "錯誤");
        obj.select(); ;
        return false;
    }
    var n;
    var sum = 0;
    var c = new Array(1, 2, 1, 2, 1, 2, 4, 1);
    for (var i = 0; i < 8; i++) {
        n = parseInt(No.charAt(i)) * c[i];

        if (i == 0 || i == 2 || i == 4 || i == 7) {
            sum = parseInt(sum) + parseInt(n);
        }
        else {
            sum = parseInt(sum) + parseInt(n / 10) + parseInt(n % 10);
        }
    }

    if (sum % 10 != 0) {
        if (No.substr(6, 1) == "7") {
            //判斷 1: 第 7位數是否為 7
            sum = parseInt(sum) + 1; //令iSum=iSum+1再再除以10取得餘數		
            if (sum % 10 == 0) return true;
        }
        alert("統一編號錯誤");
        obj.select(); ;
        return false;
    }
    return true;
}

//驗證身份證號
function checkIDNo(obj, name, flag) {
    idStr = trim(obj.value);

    if (idStr == "")// || idStr.substring(0,1)=="*")   
        return true;

    var acc = 0;
    d0 = idStr.charAt(0);
    d1 = idStr.charAt(1);
    d2 = idStr.charAt(2);
    d3 = idStr.charAt(3);
    d4 = idStr.charAt(4);
    d5 = idStr.charAt(5);
    d6 = idStr.charAt(6);
    d7 = idStr.charAt(7);
    d8 = idStr.charAt(8);
    d9 = idStr.charAt(9);
    if ((d0 == "A") || (d0 == "a")) { acc = 10; }
    else if ((d0 == "B") || (d0 == "b")) { acc = 11; }
    else if ((d0 == "C") || (d0 == "c")) { acc = 12; }
    else if ((d0 == "D") || (d0 == "d")) { acc = 13; }
    else if ((d0 == "E") || (d0 == "e")) { acc = 14; }
    else if ((d0 == "F") || (d0 == "f")) { acc = 15; }
    else if ((d0 == "G") || (d0 == "g")) { acc = 16; }
    else if ((d0 == "H") || (d0 == "h")) { acc = 17; }
    else if ((d0 == "J") || (d0 == "j")) { acc = 18; }
    else if ((d0 == "K") || (d0 == "k")) { acc = 19; }
    else if ((d0 == "L") || (d0 == "l")) { acc = 20; }
    else if ((d0 == "M") || (d0 == "m")) { acc = 21; }
    else if ((d0 == "N") || (d0 == "n")) { acc = 22; }
    else if ((d0 == "P") || (d0 == "p")) { acc = 23; }
    else if ((d0 == "Q") || (d0 == "q")) { acc = 24; }
    else if ((d0 == "R") || (d0 == "r")) { acc = 25; }
    else if ((d0 == "S") || (d0 == "s")) { acc = 26; }
    else if ((d0 == "T") || (d0 == "t")) { acc = 27; }
    else if ((d0 == "U") || (d0 == "u")) { acc = 28; }
    else if ((d0 == "V") || (d0 == "v")) { acc = 29; }
    else if ((d0 == "X") || (d0 == "x")) { acc = 30; }
    else if ((d0 == "Y") || (d0 == "y")) { acc = 31; }
    else if ((d0 == "W") || (d0 == "w")) { acc = 32; }
    else if ((d0 == "Z") || (d0 == "z")) { acc = 33; }
    else if ((d0 == "I") || (d0 == "i")) { acc = 34; }
    else if ((d0 == "O") || (d0 == "o")) { acc = 35; }
    if (acc == 0) {
        if (flag) {
            alert("請輸入『" + name + "』的第一個英文字母！");
        }
        return false;
    } else {
        accstr = new String(acc);
        acc_1 = (accstr).charAt(0);
        acc_2 = (accstr).charAt(1);
        certsum = 1 * acc_1 + 9 * acc_2 + 8 * d1 + 7 * d2 + 6 * d3 + 5 * d4 + 4 * d5 + 3 * d6 + 2 * d7 + 1 * d8;
        certsum_2 = parseInt(certsum % 10);
        certsum_3 = 10 - certsum_2;
        if (certsum_3 == 10)
            certsum_3 = 0;

        if (d9 != certsum_3) {
            if (flag) {
                alert("請檢查『" + name + "』是否輸入錯誤！");
            }
            return false;
        }
    }

    return true;

}

//Author:Alinta

var intError=0;
var timefocus;
var ajaxControl;
var StepClick=false;
var bolPost=false;
var lastID="";
var pageDetail = "";






//驗證數字
function isNumber(sText)
{
   var ValidChars = "0123456789.";
   var bolNumber=true;
   var Char;

    
 
   for (i = 0; i < sText.length && bolNumber == true; i++) 
      { 
          Char = sText.charAt(i); 
          if (ValidChars.indexOf(Char) == -1) 
             {
                bolNumber = false;
             }
      }
   return bolNumber;
   
}

function setHeight() {
   
    var intHeight = document.body.offsetHeight + 20;

    if (intHeight < 700)
        intHeight = 700;

    if (parent.document.getElementById('iframeCustom'))
        return;
        
    if (parent.document.getElementById('iframeFrc'))
        return;     
        
    try{
        if (parent.document.getElementById('frameContent').style.display == "")
        {
            parent.document.getElementById('frameContent').height = intHeight;
      
         }
        else 
        {
            parent.document.getElementById('frameDetail').height = intHeight;
      
        }
    
    }
    catch(er)
    {
    alert(window.location.href);
    }
    finally{}

   
}

function contentChange(sID) {


    if (sID == "frameContent") {
        //if (btnQuery != "")
        //    document.getElementById("");

        if (window.parent.document.getElementById("frameContent").contentWindow.document.getElementById("ctl00_ctl00_body_btnQry") && window.parent.document.getElementById("frameContent").contentWindow.document.getElementById("tbGrid"))
            if (window.parent.document.getElementById("frameContent").contentWindow.document.getElementById("tbGrid").rows.length > 1)
                window.parent.document.getElementById("frameContent").contentWindow.document.getElementById("ctl00_ctl00_body_btnQry").click();
                
            if (window.parent.document.getElementById("frameContent").src.toString().toLowerCase().indexOf("wa070")!=-1)
                if (window.parent.document.getElementById("frameContent").contentWindow.document.getElementById("ctl00_ctl00_body_editingArea_btnQry"))
                    window.parent.document.getElementById("frameContent").contentWindow.document.getElementById("ctl00_ctl00_body_editingArea_btnQry").click();
                
        window.parent.document.getElementById("frameContent").style.display = "";
        window.parent.document.getElementById("frameDetail").style.display = "none";
        window.parent.document.getElementById("frameFunction").style.display = "none";

    } else if (sID == "frameDetail") {
    
        window.parent.document.getElementById("frameDetail").src = pageDetail;
        window.parent.document.getElementById("frameContent").style.display = "none"
        window.parent.document.getElementById("frameFunction").style.display = "none"
        //  window.parent.document.getElementById("frameDetail").style.display = "";
    } else {
        window.parent.document.getElementById("frameFunction").src = pageDetail;
        window.parent.document.getElementById("frameDetail").style.display = "none"
        window.parent.document.getElementById("frameFunction").style.display = "none"
        //  window.parent.document.getElementById("frameDetail").style.display = "";
    }

   // window.setTimeout(setHeight(),500);
}



//sys:trim掉空白
function trim(stringToTrim) {
    return stringToTrim.replace(/^\s+|\s+$/g, "");
}



//sys:去尾數
function formatnumber(value, num) {
    var a, b, c, i
    a = value.toString();
    b = a.indexOf('.');
    c = a.length;
    if (num == 0) {
        if (b != -1)
            a = a.substring(0, b);
    }
    else {
        if (b == -1) {
            a = a + ".";
            for (i = 1; i <= num; i++)
                a = a + "0";
        }
        else {
            a = a.substring(0, b + num + 1);
            for (i = c; i <= b + num; i++)
                a = a + "0";
        }
    }
    return a
}




//取得欄位值
function getTextValue(strID) {
    var obj = document.getElementById(strID);
    var strValue = "";

    if (obj) {
        switch (obj.tagName.toUpperCase()) {
            case "INPUT":
                if (obj.className.toUpperCase().indexOf("NUMBER") != -1)
                    strValue = obj.value.replace(/,/g, "")
                else
                    strValue = obj.value;




                break;
            case "TEXTAREA":
                strValue = obj.innerText;
                break;
            case "SELECT":
                strValue = obj.options[obj.selectedIndex].value;
                break;
        }
    }

    return trim(strValue)
}


//設定欄位值
function setTextValue(strID, strValue) {
    var obj = document.getElementById(strID);

    if (obj) {
        switch (obj.tagName.toUpperCase()) {
            case "INPUT":
                document.getElementById(strID).value = strValue;
                break;
            case "TEXTAREA":
                document.getElementById(strID).innerText = strValue;
                break;
            case "SELECT":
                var s = document.getElementById(strID);
                for (var i = 0; i < s.options.length; i++) {
                    if (s.options[i].value == strValue) {
                        s.selectedIndex = i;
                        break;
                    }
                }

                break;
        }
    }
}


//將數字加上common
function parseMoney(str) {
    var string = "";
    var Num = 0;
    var bolM=false;
    if (str.toString().indexOf("-")!=-1)
        bolM=true;
        
    str=str.toString().replace(/-/g, "")
    try {
        str = parseFloat(str.replace(/,/g, ""));
    }
    catch (Err) { }

    if (isNaN(str)) {
        str = 0;
    }

    for (var i = str.toString().length - 1; i >= 0; i--) {
        if (str.toString().charAt(i) == ".") {
            Num = -1;
            string = string.replace(/,/g, "");            
        }

        if (Num >= 3) {
            string = "," + string;
            Num = 0;
            i++;
        }
        else {
            string = str.toString().charAt(i) + string;
            Num++;
        }
    }
    if (bolM)
        string="-"+string;
    return string;
}

