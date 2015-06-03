<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/query.Master" CodeBehind="WA010.aspx.cs" Inherits="OrixMvc.WA010" %>
<%@ MasterType VirtualPath="~/Pattern/query.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
    
<asp:Content ContentPlaceHolderID="searchArea" ID="mySearchArea" runat="server">

<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlCNTR_DEPT" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'CNTR_DEPT' "
    runat="server" >
</asp:sqldatasource>   
    <table >
        <tr>
            <th>範本代碼：</th><td><asp:TextBox runat="server" ID="TMP_CODE"  Width="100"  size="10"></asp:TextBox></td> 
            <th>範本說明：</th><td><asp:TextBox runat="server" ID="TMP_DESC"  Width="300"  size="30"></asp:TextBox></td>             
        </tr>   
         <tr>
            <th>部門：</th>
            <td><asp:DropDownList ID="DEPT"  runat="server" Width="100" DataSourceID="sqlCNTR_DEPT" DataValueField="TypeCode" DataTextField="TypeDesc" >
                </asp:DropDownList> </td>       
        </tr>        
    </table>
                     
</asp:Content>

<asp:Content ContentPlaceHolderID="queryHead" ID="myQueryHead" runat="server">
    <!--QueryHead!-->    
    <th class="fixCol">範本代碼</th>
    <th class="fixCol">範本說明</th>
    <th>部門</th>    
    <th>合約條文</th>    
    <th>附表樣板</th>      
</asp:Content>
<asp:Content ContentPlaceHolderID="queryBody" ID="myQueryBody" runat="server">
    <td class="fixCol"><%# Eval("TMP_CODE")%><asp:HiddenField runat="server" ID="hiddenTMP_CODE" Value='<%# Eval("TMP_CODE")%>' /> </td>
    <td class="fixCol"><%# Eval("TMP_DESC")%></td>
    <td><%# Eval("DEPT")%></td>
    <td><input type="button" class="button upload" onclick="view('<%# Eval("CON_COND_FILE_SEQ") %>')" value="檢視" /></td>
    <td><input type="button" class="button upload" onclick="view('<%# Eval("CON_ATT_FILE_SEQ") %>')" value="檢視" /></td>
 
</asp:Content>

<asp:Content ContentPlaceHolderID="functionBar" runat="server" ID="myBar">
    <div style="display:none">
    <asp:TextBox runat="server" ID="seq"></asp:TextBox>
    <asp:Button ID="btnView" runat="server" OnClick="ViewFile" />
    <ocxControl:ocxUpload ID="FileView" runat="server"  bolUpload="false"    ></ocxControl:ocxUpload>
    </div>
    <script language="javascript" type="text/javascript">
        function view(iseq) {
            document.getElementById("<%=this.seq.ClientID %>").value = iseq;
            document.getElementById("<%=this.btnView.ClientID %>").click();
        }
    </script>
</asp:Content>