<%@ Page Language="C#" EnableEventValidation = "false" AutoEventWireup="true" CodeBehind="Welcome.aspx.cs" Inherits="OrixMvc.Error" %>

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
	<!-- MAIN CONTENT -->
	<form id="loginform" runat="server"  >
	
	
		<asp:ScriptManager ID="myScriptManager" runat="server" EnablePageMethods="true" />
		
	
			<br/><div id="warning" class="information-box round">歡迎使用台灣歐力士新租賃管理系統</div>
            
				
		</form>
</body>
</html>
