<%@ Page Language="C#" EnableEventValidation = "false" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="OrixMvc.Error" %>

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
				<h1>錯誤</h1>
				<h5>網頁發生錯誤</h5>
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
		
	
			<br/><div id="warning" class="warning-box round">網頁發生錯誤</div>
            
				
		</form>
		
	</div> <!-- end content -->
	
	
	
	
	
	
</body>
</html>
