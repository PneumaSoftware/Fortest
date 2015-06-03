<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ocxTime.ascx.cs" Inherits="OrixMvc.ocxControl.ocxTime" %>
<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>
    
 
<asp:TextBox ID="txtTime" CssClass="Time" runat="server" Width="90"  MaxLength="5"  onfocusin="this.value=this.value.replace(':','');"   />
<ajaxToolkit:FilteredTextBoxExtender ID="filter_myTime" runat="server" TargetControlID="txtTime" FilterType="Custom" ValidChars="0123456789:" />



<script language="javascript" type="text/javascript">

    document.getElementById("<%=this.txtTime.ClientID %>").onblur = function(evt) {
    chkTime(document.getElementById("<%=this.txtTime.ClientID %>"));

    };
</script>