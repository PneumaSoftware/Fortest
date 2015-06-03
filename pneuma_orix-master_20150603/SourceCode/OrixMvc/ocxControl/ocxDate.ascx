<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ocxDate.ascx.cs" Inherits="OrixMvc.ocxControl.ocxDate" %>
<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>  
<span class="spandate">
<span><asp:TextBox ID="txtDate" CssClass="date" runat="server" Width="73"  MaxLength="10"  onfocusin='this.value=this.value.replace(/\//g, "");' /></span><span runat="server"  class="date" id="btnDate"></span> 
</span> 

<ajaxToolkit:FilteredTextBoxExtender ID="filter_myDate" runat="server" TargetControlID="txtDate" FilterType="Custom" ValidChars="0123456789/" />
 <ajaxToolkit:CalendarExtender ID="CalendarForm" CssClass="cal_Theme1"   runat="server"  TargetControlID="txtDate"  PopupButtonID="btnDate"   Format="yyyy/MM/dd" />



<script language="javascript" type="text/javascript">

    document.getElementById("<%=this.txtDate.ClientID %>").onblur = function(evt) {
        var rtn = chkDate(document.getElementById("<%=this.txtDate.ClientID %>"));
        if (rtn) {
            try {
               
                isDate(document.getElementById("<%=this.txtDate.ClientID %>"));

                DateDo(document.getElementById("<%=this.txtDate.ClientID %>"), document.getElementById("<%=this.txtDate.ClientID %>").value);
            }
            catch (err) {
               
            }

        }
    };
</script>