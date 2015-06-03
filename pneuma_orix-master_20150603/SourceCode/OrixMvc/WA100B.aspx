<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/query.Master" CodeBehind="WA100B.aspx.cs" Inherits="OrixMvc.WA100B" %>
<%@ MasterType VirtualPath="~/Pattern/query.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
    
<asp:Content ContentPlaceHolderID="searchArea" ID="mySearchArea" runat="server">
   <table >
        <tr>
            <th class="nonSpace">申請書編號：</th><td><ocxControl:ocxDialog SourceName="OR_CASE_APLY_BASE" runat="server" ID="APLY_NO" width="140" /></td>                       
        </tr>                        
    </table>   

                                    
</asp:Content>

<asp:Content ContentPlaceHolderID="queryHead" ID="myQueryHead" runat="server">
    <!--QueryHead!-->    
    <th class="fixCol">案件/動態擔保</th>
    <th>動產品名</th>
    <th>保險種類代碼</th>    
    <th>保險種類名稱</th>    
    <th>金額</th>  
    <th>保險迄日</th>  
</asp:Content>

<asp:Content ContentPlaceHolderID="queryBody" ID="myQueryBody" runat="server">
    <td class="fixCol"><%# Eval("POLICY_SUBJECT")%></td>
    <td><%# Eval("PROD_NAME")%></td>
    <td><%# Eval("ASUR_TYPE_CODE")%><asp:HiddenField runat="server" ID="ASUR_TYPE_CODE" Value='<%# Eval("ASUR_TYPE_CODE")%>' /></td>
    <td><%# Eval("ASUR_TYPE_NAME")%></td>
    <td><%# Eval("AMOUNT")%></td>
    <td><%# Eval("ASUR_E_DATE")%></td>
</asp:Content>


