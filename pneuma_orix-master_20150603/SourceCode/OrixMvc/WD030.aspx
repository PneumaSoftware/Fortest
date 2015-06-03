<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/empty.Master" CodeBehind="WD030.aspx.cs" Inherits="OrixMvc.WD030" %>
<%@ MasterType VirtualPath="~/Pattern/empty.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxYM"  TagPrefix="ocxControl" Src="~/ocxControl/ocxYM.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
        
<asp:Content ContentPlaceHolderID="editingArea" ID="mySearchArea" runat="server">



<div class="searchArea" id="divImport">
    <table cellpadding="2" cellspacing="2" style="margin:40px" >
        <tr>
            <td>
                <table >
                    <tr>
                    <th class="nonSpace">年月：</th><td>
                           <ocxControl:ocxYM ID=yearMonth runat="server" />
                        </td>
                        </tr>
                        <tr>
                        <th class="nonSpace">路徑：</th><td>
                            <asp:FileUpload runat="server" ID="filePath" Width=350 />
                        </td>                       
                    </tr>                        
                </table>
                
            </td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td style="text-align:right;">
               
                    
                <div class="divButton">
                <asp:Button runat="server" ID="btnImport" CssClass="button export" Text="匯入" onclick="Import_Click" OnClientClick="return confirmImport();"/>
                </div>          
                          
            </td>
        </tr>        
    </table>    
</div>                     

<div class="searchArea" id="divResult" style="display:none;margin:40px">
    <fieldset><legend><font size="4" color="darkblue">資料匯入驗證報告</font></legend>
    <table cellpadding="2" cellspacing="2"  >
            <tr>
                <td>
        <asp:TextBox runat="server" TextMode="MultiLine"  ReadOnly="true" Rows="30" Width="710" Font-Size="Larger" Font-Bold="true" ID="Message">    
        </asp:TextBox>
                </td>
            </tr>
    </table>
    </fieldset> 
</div>
<script language="javascript" type="text/javascript">     
    function confirmImport(){
         window.setTimeout("window.parent.Loading()", "");
         if ("<%=this.IS_TRANSFER %>" == "N" && document.getElementById("<%=this.filePath.ClientID %>").value != "") {
             var bol = confirm("暫存檔仍有資料未轉, 確定重新匯入?");
             if (!bol) {
                 window.setTimeout("window.parent.closeLoading()", "500");
             }
             return bol;
         }
         else {
             window.setTimeout("window.parent.closeLoading()", "500");
             return true;
         }
    }
</script>

    
                            
</asp:Content>

