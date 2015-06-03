<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main.aspx.cs" Inherits="OrixMvc.main" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>台灣歐力士新租賃管理系統</title>
    
    <link href="Style/main.css" type="text/css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="jquery/style/easyui.css" />
    <link rel="stylesheet" type="text/css" href="jquery/style/icon.css" />
    <script type="text/javascript" src="jquery/jquery-1.8.0.min.js"></script>
    <script type="text/javascript" src="jquery/jquery.easyui.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <!--#include VIRTUAL="Objects/PopUp.html" -->
    
    <script language="javascript" type="text/javascript">
        function showDetailContent() {
                if (document.getElementById("frameContent").src.toLowerCase().indexOf("wa060") == -1 && document.getElementById("frameContent").src.toLowerCase().indexOf("wb060") == -1) {
                // if (document.getElementById("frameDetail").src.toLowerCase().indexOf("wa0601") == -1 && document.getElementById("frameDetail").src.toLowerCase().indexOf("wb0601") == -1) {
                if (document.getElementById("frameContent").style.display == "none")
                    document.getElementById("frameDetail").style.display = ""
            }

        }

        function showFunctionContent() {
            if (document.getElementById("frameFunction").style.display == "none")
                document.getElementById("frameFunction").style.display = ""
        }
            
    </script>
    <asp:ScriptManager ID="myScriptManager" runat="server" EnablePageMethods="true" />
    <asp:HiddenField runat="server" ID="UserId" />
    <asp:SqlDataSource
          id="sqlMenu"
          runat="server"
          DataSourceMode="DataReader"
          ConnectionString="<%$ ConnectionStrings:myConnectionString%>"
        SelectCommand="exec s_GetMenu @UserId">
        <SelectParameters>
            <asp:ControlParameter ControlID="UserId" PropertyName="Value" Name="UserId" />
        </SelectParameters>
    </asp:SqlDataSource>
      
    <div style="width: 100%;" align="center">
        <div class="page">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td rowspan="2">
                        <div style="HEIGHT: 100%; overflow-x:hidden;overflow-y:auto;">
                            <div id="leftMenu" class="left">
                                <div class="logo"><img src="images/OrixLogo-s.jpg" /></div>
                                <div class="progID" id="dprogID"><%=this.ProgramId %></div>
                                <div class="progNM" id="dprogNM"><%=this.ProgramName %></div>
                                <div class="empNM">
                                    <div style="float:left">
                                        <%=this.EmployeeName %> 
                                    </div>
                                    <%--增加登出功能--%>
                                    <div style="text-align:right">
                                        <asp:Button ID="btnLO" runat="server" onclick="btnLogout_Click" CssClass="Sout ui-button" Text="登出"
                                            style="background-color:transparent;color:#FFFF66;border-color:transparent; margin:0px" />
                                    </div>
                                </div>
                                <div id="menu">
                                    <ul>
                                        <asp:Repeater runat="server" ID="rptMenu" DataSourceID="sqlMenu">
                                            <ItemTemplate>
                                                <%# Container.ItemIndex != 0 && Eval("Atch_seq").ToString() == "-1" ?"</ul></li>":""%>
                                                <%--增加mvc選單--%>
                                                <%# Eval("Atch_seq").ToString() == "-1" ? "<li onclick='menuShow(this)'><a href='#'><div class='arrow'></div>" + Eval("FUNC_NAME").ToString() + "</a><ul onclick='stopMenu()'>" : "<li><a onclick='menuSet(this,\"" + Eval("Func_ID").ToString() + "\",\"" + Eval("Func_NAME").ToString() + "\",\"" + Eval("MVC_FLAG").ToString() + "\")'  href='#'>" + Eval("Func_Name").ToString() + "</a></li>" %>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </td>
                    <td>
                        <div class="head" id="head">
                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                <tr>
                                    <td style="width: 20px; background-color: #150643;">
                                        <div class="showMenu" onclick="shMenu(this)">
                                        </div>
                                    </td>
                                    <td style="width: 790px">
                                        <div class="message" id="divMessage">
                                            訊息列！</div>
                                    </td>
                                    <td style="background-color: #150643" align="center">
                                        <a style="border: none; display: none" target="_blank" id="helpLink">
                                            <img id="helpImage" src="images/help.png" alt="程式說明連結" /></a><img style="display: none"
                                                id="helpImage_no" src="images/nohelp.png" alt="無程式說明" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top">
                        <div class="content" id="content">
                            <div id="divPopWindow" style="display: none;">
                                <div id="close" onclick="document.getElementById('divPopWindow').style.display='none';">
                                    <img title="關閉" src="Images/close.png" alt="關閉" /></div>
                                <iframe id="iframeFile" name="iframeFile" frameborder="0" scrolling="yes" width="800"
                                    height="580"></iframe>
                            </div>
                            <iframe id="frameContent" name="frameContent" frameborder="0" scrolling="no" src="<%=this.ProgramId %>.aspx"
                                style="position: absolute; z-index: 50"></iframe>
                            <%--增加mvc iframe--%>
                            <iframe id="aicFrame" name="aicFrame" frameborder="0" scrolling="no" src = ''
                                style="display: none; position: absolute; z-index: 50; height: 700px;width:843px"></iframe>
                            <iframe style="display: none" id="frameDetail" onload="showDetailContent()" name="frameDetail"
                                frameborder="0" scrolling="no"></iframe>
                            <iframe style="display: none" id="frameFunction" onload="showFunctionContent()" name="frameFunction"
                                frameborder="0" scrolling="no"></iframe>
                        </div>
                    </td>
                </tr>
            </table>
            <span style="display: none">
                <asp:UpdatePanel runat="server" ID="upOpen" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <asp:TextBox runat="server" ID="progID"></asp:TextBox>
                        <asp:TextBox runat="server" ID="progNM"></asp:TextBox>
                        <asp:Button runat="server" ID="btnOpen" OnClick="Open_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </span>
            <div id="w" class="easyui-window" title="錯誤訊息" data-options="modal:true,closed:true,iconCls:'icon-save',minimizable:false,maximizable:false"
                style="width: 500px; height: 200px; padding: 10px;">
            </div>
        </div>
        <script language="javascript" type="text/javascript">
            //Reload MVC模式的open
            if ("<%=this.MVC %>" == "True")
            {
                if (document.getElementById("frameContent") != null) document.getElementById("frameContent").style.display = "none";
                if (document.getElementById("frameContent") != null) document.getElementById("aicFrame").src = "";
                if (document.getElementById("frameDetail") != null) document.getElementById("frameDetail").style.display = "none";
                if (document.getElementById("frameFunction") != null) document.getElementById("frameFunction").style.display = "none";
                if (document.getElementById("aicFrame") != null) document.getElementById("aicFrame").style.display = "";
                if (document.getElementById("aicFrame") != null) document.getElementById("aicFrame").src = "<%=this.ProgramId %>";
            }

            window.onbeforeunload = function () {
                $(".Sout").trigger("click"); 
            };
             
            function setHELP(helpName) {
                if (helpName != "") {
                    document.getElementById("helpLink").style.display = ""; // = "helps/" + helpName;
                    document.getElementById("helpImage_no").style.display = "none";
                    document.getElementById("helpLink").href = "helps/" + helpName;
                }
                else {
                    document.getElementById("helpLink").style.display = "none";
                    document.getElementById("helpImage_no").style.display = "";
                }
            }

            //sys:draw processing window 
            function Loading() {

                var win = $.messager.progress({
                    title: '請稍候',
                    msg: '資料正在處理中...'
                });
            }


            //sys:close processing window
            function closeLoading() {

                $.messager.progress('close');

            }
            function openMessage() {
                $('#w').window('open');
            }
            function setHeight() {

                var intHeight = document.body.offsetHeight + 20;

                if (intHeight < 640)
                    intHeight = 640;

                //增加不等於null的條件
                if (document.getElementById('frameContent') != null && document.getElementById('frameContent').style.display == "")
                    document.getElementById('frameContent').height = intHeight;
                else if (document.getElementById('frameDetail') != null && document.getElementById('frameDetail').style.display == "")
                    document.getElementById('frameDetail').height = intHeight;
                else
                    document.getElementById('frameFunction').height = intHeight;

            }
            function contentChange(sID) {
                //增加不等於null的條件
                if (sID == "frameContent") {
                    if (document.getElementById("frameContent") != null) document.getElementById("frameContent").style.display = "";
                    if (document.getElementById("frameDetail") != null) document.getElementById("frameDetail").style.display = "none";
                    if (document.getElementById("frameFunction") != null) document.getElementById("frameFunction").style.display = "none";
                } else if (sID == "frameDetail") {

                    if (document.getElementById("frameDetail") != null) document.getElementById("frameDetail").src = pageDetail;
                    if (document.getElementById("frameContent") != null) document.getElementById("frameContent").style.display = "";
                    if (document.getElementById("frameContent") != null) document.getElementById("frameContent").style.display = "none";
                    if (document.getElementById("frameFunction") != null) document.getElementById("frameFunction").style.display = "none";
                } else if (sID == "frameFunction") {
                    if (document.getElementById("frameFunction") != null) document.getElementById("frameFunction").src = pageDetail;
                    if (document.getElementById("frameContent") != null) document.getElementById("frameContent").style.display = "none";
                    if (document.getElementById("frameDetail") != null) document.getElementById("frameDetail").style.display = "none";
                    if (document.getElementById("frameFunction") != null) document.getElementById("frameFunction").style.display = "";
                }

                //  window.setTimeout(setHeight(),500);

            }
            var objMenu;

            function shMenu(obj) {

                //  var ifrmaeC = document.getElementById('frameContent');

                if (obj.className == "showMenu") {
                    obj.className = "hideMenu";
                    obj.title = "顯示選單";
                    document.getElementById("leftMenu").style.display = "none";
                    document.getElementById("content").style.marginLeft = "3px";
                    document.getElementById("head").style.marginLeft = "3px";
                    // if (ifrmaeC.getElementById("query"))
                    //     ifrmaeC.getElementById("query").className = "gridMain gridMainMax";                        
                }
                else {
                    obj.className = "showMenu";
                    obj.title = "隱藏選單";
                    document.getElementById("leftMenu").style.display = "";
                    document.getElementById("content").style.marginLeft = "3px";
                    //document.getElementById("head").style.marginLeft = "150px";
                    //  if (ifrmaeC.getElementById("query"))
                    //     ifrmaeC.getElementById("query").className = "gridMain gridMainMin";                        
                }
            }

            var mObj
            //增加是否為mvc的flag
            function menuSet(obj, id, nm, flag) {

                var aicFlag = (flag != 1) ? false : true;

                if (!aicFlag) {
                    //不是mvc
                    if (document.getElementById("aicFrame") != null) {
                        document.getElementById("aicFrame").style.display = "none";
                        document.getElementById("frameFunction").src = "";
                    }


                    if (mObj) {
                        mObj.className = "";
                    }
                    obj.className = "selected";
                    mObj = obj;
                    document.getElementById("<%=this.progID.ClientID %>").value = id;
                    document.getElementById("<%=this.progNM.ClientID %>").value = nm;
                    document.getElementById("dprogID").innerHTML = id;
                    document.getElementById("dprogNM").innerHTML = nm;
                    document.getElementById("<%=this.btnOpen.ClientID %>").click();
                }
                else {
                    //mvc
                    if (mObj) {
                        mObj.className = "";
                    }
                    obj.className = "selected";
                    mObj = obj;

                    document.getElementById("<%=this.progID.ClientID %>").value = id;
                    document.getElementById("<%=this.progNM.ClientID %>").value = nm;
                    document.getElementById("dprogID").innerHTML = id;
                    document.getElementById("dprogNM").innerHTML = nm;

                    if (document.getElementById("frameContent") != null) document.getElementById("frameContent").style.display = "none";
                    if (document.getElementById("frameDetail") != null) document.getElementById("frameDetail").style.display = "none";
                    if (document.getElementById("frameFunction") != null) document.getElementById("frameFunction").style.display = "none";
                    if (document.getElementById("aicFrame") != null) document.getElementById("aicFrame").style.display = "";

					//設定Session
                    $.ajax({
                        type: "POST",
                        url: "main.aspx/SetSession",
                        data: "{ ProgramId: '" + id + "', ProgramName: '" + nm + "' }",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                        }
                    });

                    window.open(id, 'aicFrame');
                    // aic flow
                }
                //   contentChange("frameContent");
                //frameContent
            }


            function stopMenu() {
                window.event.cancelBubble = true;
            }
            function menuShow(obj) {
                var css = obj.className;

                if (objMenu)
                    objMenu.className = "";

                if (css == "")
                    obj.className = "show";
                else
                    obj.className = "";



                objMenu = obj;
            }

            function errorMessage(title, msg) {
                //$.messager.alert(title, msg, 'error');
                alert(msg);
            }

            function slideMessage(title, msg) {
                $.messager.show({
                    title: title,
                    msg: msg,
                    timeout: 4000,
                    showType: 'slide'
                });


            }
        </script>
    </div>
    </form>
</body>
</html>
