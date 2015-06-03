<%@ Page Language="C#" EnableEventValidation = "false" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="OrixMvc.Login" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>台灣歐力士新租賃管理系統</title>
<!-- Stylesheets -->
	<link href='http://fonts.googleapis.com/css?family=Droid+Sans:400,700' rel='stylesheet'>
	<link rel="stylesheet" href="style/login.css">

	<!-- Optimize for mobile devices -->
	<meta name="viewport" content="width=device-width, initial-scale=1.0"/> 
</head>
<body >
<!-- TOP BAR -->
	<div id="top-bar">
		
		<div class="page-full-width">
		
			<a href="#" class="round button dark ic-favorite image-left ">台灣歐力士新租賃管理系統</a>

		</div> <!-- end full-width -->	
	
	</div> <!-- end top-bar -->
	
	
	
	<!-- HEADER -->
	<div id="header">
		
		<div class="page-full-width cf">
	
			<div id="login-intro" class="fr">
			<span id="loginTitle">
				<h1>登入</h1>
				<h5>請登錄使用者代號及密碼</h5>
			</span> 
			<span id="confirmTitle" style="display:none">
				<h1>變更密碼</h1>
				<h5>請登錄欲變更的新密碼</h5>
			</span> 
			</div> <!-- login-intro -->
			
			<!-- Change this image to your own company's logo -->
			<!-- The logo will automatically be resized to 39px height. -->
			<a href="#" id="company-branding" class="fl"><img src="images/OrixLogo-s.jpg" alt="Orix" /></a>
			
		</div> <!-- end full-width -->	

	</div> <!-- end header -->
	
	
	
	<!-- MAIN CONTENT -->
	<div id="content">
	
	<form id="loginform" runat="server"  >
	
	
		<asp:ScriptManager ID="myScriptManager" runat="server" EnablePageMethods="true" />
		
	
   
        <fieldset id="loginf" style="visibility:hidden">
                <div id="login">
				    <p>
					    <label for="login-username">使用者 ID</label>
					    <asp:TextBox ID="USER_ID"  CssClass="round full-width-input" autofocus runat="server" ></asp:TextBox>
					</p>

				    <p>
					    <label for="login-password">使用者密碼</label>
					    <asp:TextBox ID="USER_PASS" runat="server" CssClass="round full-width-input"  TextMode="PASSWORD"></asp:TextBox>
				    </p>
				    <asp:Button  CssClass="button round blue  ic-right-arrow" runat="server" Width=80 ID="btnLogin" Text="登 入"  OnCommand="Login_Click" CommandName="login" />
				    <asp:Button  CssClass="button round red image-right ic-edit" runat="server" Width=110 ID="btnConfirm" Text="變更密碼"  OnCommand="Login_Click" CommandName="confirm" />
				</div>
				<div  id="confirm" >
				    <p>
					    <label for="login-username">新密碼</label>
					    <asp:TextBox ID="USER_PASSN" runat="server" CssClass="round full-width-input"  TextMode="PASSWORD" ></asp:TextBox>
				    </p>

				    <p>
					    <label for="login-password">確認新密碼</label>
					    <asp:TextBox ID="USER_PASSNA" runat="server" CssClass="round full-width-input"  TextMode="PASSWORD" ></asp:TextBox>  
				    </p>
				    <asp:Button runat="server" CssClass="button round blue image-right ic-settings" Width=80 ID="btnReset" Text="確 認"  OnCommand="Login_Click" CommandName="reset"  />
				    <asp:Button runat="server" CssClass="button round gray image-right ic-cancel" Width=80 ID="btnCancel" Text="取 消"  OnCommand="Login_Click" CommandName="cancel"  />				    
				    
				</div>
				
			
			</fieldset>

			<br/><div id="warning" class="warning-box round">訊息列！</div>

            <asp:UpdatePanel runat="server" ID="uplogin" RenderMode="Inline">
			    <ContentTemplate>
			        
			    </ContentTemplate> 	
			    <Triggers>
			        <asp:AsyncPostBackTrigger ControlID="btnLogin" />
			        <asp:AsyncPostBackTrigger ControlID="btnConfirm" />
			        <asp:AsyncPostBackTrigger ControlID="btnReset" />
			    </Triggers>
			</asp:UpdatePanel>
				
		</form>
		
	</div> <!-- end content -->
	
	
	
	<!-- FOOTER -->
	<div id="footer">

		<p>&copy; Copyright 2013 <a href="#">肯美資訊科技股份有限公司</a>. All rights reserved.</p>
		
	
	</div> <!-- end footer -->
	
	
	<script language="javascript" type="text/javascript" >
	    changeForm("login");
	    document.getElementById("loginf").style.visibility = "";
	    document.getElementById("USER_ID").focus();
	    
	    function changeForm(strType) {
	        document.getElementById("warning").innerHTML = "訊息列！";
	        document.getElementById("login").style.display = (strType == "login" ? "" : "none");
	        document.getElementById("loginTitle").style.display = (strType == "login" ? "" : "none");
	        document.getElementById("confirm").style.display = (strType == "login" ? "none" : "");
	        document.getElementById("confirmTitle").style.display = (strType == "login" ? "none" : "");
	        if (document.getElementById("loginf").style.visibility == "") {
	            if (strType == "login")
	                document.getElementById("USER_ID").focus();
	            else
	                document.getElementById("USER_PASSN").focus();
	        }
	    }

	    function setWarning(strMessage) {
	        document.getElementById("warning").innerHTML = strMessage;
	        document.getElementById("USER_PASS").value = "";
	        document.getElementById("USER_PASSN").value = "";
	        document.getElementById("USER_PASSNA").value = "";
	    }
	</script>
</body>
</html>
