<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ocxYear.ascx.cs" Inherits="OrixMvc.ocxControl.ocxYear" %>
<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>
    
 
<asp:TextBox ID="txtYear" CssClass="YearMonth" runat="server" Width="40px"  MaxLength="4"    />
<ajaxToolkit:FilteredTextBoxExtender ID="filter_myYear" runat="server" TargetControlID="txtYear" FilterType="Custom" ValidChars="0123456789/" />





