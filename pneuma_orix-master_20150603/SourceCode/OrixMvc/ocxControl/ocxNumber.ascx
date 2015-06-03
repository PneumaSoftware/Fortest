<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ocxNumber.ascx.cs" Inherits="OrixMvc.ocxControl.ocxNumber" %>
<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>  
    
 <asp:TextBox ID="myNumber" runat="server" Width="130px"   style="text-align:right;margin:0px" onfocusin='this.value=this.value.replace(/,/g,"");' onfocusout='this.value=parseMoney(this.value);'   />
 <ajaxToolkit:FilteredTextBoxExtender ID="filter_myNumber" runat="server" TargetControlID="myNumber" FilterType="Custom" ValidChars="0123456789,.-" />
 
            
    