//Enter Convert To Tab
function onkeydown(event) {
    if (event.keyCode == "13") {
        var idx = inputs.index(this);

        if (inputs[idx + 1]) {
            if (inputs[idx + 1].id == "")
                idx++;
        }
        try {

            document.getElementById(inputs[idx + 1].id).focus();
        }
        catch (err) {
            document.getElementById(inputs[idx].id).focus();
        }
    }
};
      

//取得Url參數
function GetURLParameter(sParam) {
    var sPageURL = window.location.search.substring(1);
    var sURLVariables = sPageURL.split('&');
    for (var i = 0; i < sURLVariables.length; i++) {
        var sParameterName = sURLVariables[i].split('=');
        if (sParameterName[0] == sParam) {
            return sParameterName[1];
        }
    }
}

//整數
function ValidateInt(e, pnumber) {
    if (!/^\d+$/.test(pnumber)) {
        $(e).val(/^\d+/.exec($(e).val()));
    }
    return false;
}


//小數1位
function ValidateFloat(e, pnumber) {
    if (!/^\d+[.]?[1-9]?$/.test(pnumber)) {
        e.value = /\d+[.]?[1-9]?/.exec(e.value);
    }
    return false;
}

//
function substrForChinese(obj, len) {
    var str = $(obj).val();

    if (!str || !len) { return ''; }
    //中文2字數，英文1字數
    var a = 0;      //累計字數
    var i = 0;
    var temp = ''
    for (i = 0; i < str.length; i++) 
    {
        if (str.charCodeAt(i) > 255) {             //中文           
            a += 2;
        }
        else {
            a++;
        }         //如果長度大於限定長度，就直接返回暫存字串
        if (a > len) {
            $(obj).val(temp); 
        }          //將目前内容加到暫存字串
        temp += str.charAt(i);
    }     
} 

/*Start Address*/
//變更CityCode
function ChangeCityCode(CityCodeID, ZipCode) {
    var selectedValue = $('#' + CityCodeID + ' option:selected').val();
    if ($.trim(selectedValue).length > 0) {
        GetRGT_ZIP_CODE(selectedValue, ZipCode);
    }
}

//取得ZipCode
function GetRGT_ZIP_CODE(CityCode, ZipCode) {
    $.ajax({
        url: '/Common/getZipCode',
        data: { CityCode: CityCode },
        type: 'post',
        cache: false,
        async: false,
        dataType: 'json',
        success: function (data) {
            if (data.Data.length > 0) {
                $('#' + ZipCode).empty();
                $('#' + ZipCode).append($('<option></option>').val('').text('請選擇..'));
                $.each(data.Data, function (i, item) {
                    $('#' + ZipCode).append($('<option></option>').val(item.Value).text(item.Text));
                });
            }
        },
        error: function (ex) {
            alert("ERROR!!" + ex)
        }
    });
}
/*end Address*/

/*Start Grid Button*/
function btnQry() {
    //hidded
    var index = 0;
    callAjax(index);
};

function btnFirst() {
    var index = 1;
    callAjax(index);
};

function btnPrev() {
    var index = ($("#txtgoPage").val() > 1) ? $("#txtgoPage").val() - 1 : 1;
    callAjax(index);
};

function txtgoPage() {
    var iTotal = parseInt($('#TotalPage').text());
    var index = ($("#txtgoPage").val() > iTotal) ? iTotal : $("#txtgoPage").val();
    callAjax(index);
};

function btnNext() {
    var iPage = parseInt($("#txtgoPage").val());
    var iTotal = parseInt($('#TotalPage').text());
    var index = (iPage + 1 <= iTotal) ? iPage + 1 : iTotal;
    callAjax(index);
};

function btnLast() {
    var index = parseInt($('#TotalPage').text());
    callAjax(index);
};

function ddlPageSize() {
    var index = 1;
    callAjax(index);
};
/*End Grid Button*/

/*strat Message*/
function setMessageBox(strMessage) {    
    try {
        window.parent.parent.errorMessage('錯誤訊息', strMessage);
    }
    catch (er) {
        window.parent.parent.errorMessage('錯誤訊息', er);
    }
    finally { }
}


function showProcessMessage(strMessage, Error) {
    if (Error) {
        try {
            window.parent.parent.errorMessage('錯誤訊息', strMessage);
        }
        catch (er) {
            window.parent.parent.errorMessage('錯誤訊息', strMessage);
        }
        finally { }
    }
    else {
        try {
            window.parent.parent.slideMessage('處理訊息', strMessage);
        }
        catch (er) {
            window.parent.parent.slideMessage('處理訊息', strMessage);
        }
        finally { }
    }
}
/*End Message*/