<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ocxYM.ascx.cs" Inherits="OrixMvc.ocxControl.ocxYM" %>
<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>
    
 
<asp:TextBox ID="txtYM" CssClass="YearMonth" runat="server" Width="60px"  MaxLength="7" onfocusin='this.value=this.value.replace(/\//g, "");' onfocusout='isYearMonth(this);'   />
<ajaxToolkit:FilteredTextBoxExtender ID="filter_myYM" runat="server" TargetControlID="txtYM" FilterType="Custom" ValidChars="0123456789/" />





