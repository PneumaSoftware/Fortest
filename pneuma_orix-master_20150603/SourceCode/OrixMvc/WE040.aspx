<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/query.Master" CodeBehind="WE040.aspx.cs" Inherits="OrixMvc.WE040" %>
<%@ MasterType VirtualPath="~/Pattern/query.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
    
<asp:Content ContentPlaceHolderID="searchArea" ID="mySearchArea" runat="server">

   <table >
        <tr>
            <th >客戶代號：</th><td><ocxControl:ocxDialog runat="server" ID="CUST_NO" width="140" SourceName="OR_CUSTOM" /></td> 
            <td><asp:TextBox runat="server" ID="CUST_NAME"  size="30"></asp:TextBox></td>                       
            <th >狀態：</th>
            <td><asp:DropDownList runat="server" ID="SRV_REC_STS">
                    <asp:ListItem Value="">全部</asp:ListItem>
                    <asp:ListItem Value="1">追蹤</asp:ListItem>
                    <asp:ListItem Value="9">結案</asp:ListItem>
                </asp:DropDownList>
            </td> 
        </tr>                         
        <tr>
            <th >歸屬申請書編號：</th><td><ocxControl:ocxDialog SourceName="OR_CASE_APLY_BASE" width="140"  runat="server" ID="APLY_NO"></ocxControl:ocxDialog></td>                       
            <td><asp:CheckBox runat="server" ID="chkAPLY"  /><font class="green" id="caseTitle">顯示無歸屬申請書紀錄</font></td>
        </tr> 
        <tr>
            <th >記錄標題：</th><td colspan="2"><asp:TextBox runat="server" ID="REC_TITLE"  size="30"></asp:TextBox></td>             
            <th >輸入人員：</th><td><ocxControl:ocxDialog runat="server" ID="KEY_USER" width="100" FieldName="EMP_NAME" ControlID="EMP_NAME"  SourceName="OR_EMP" /></td><td><asp:TextBox runat="server" ID="EMP_NAME"  CssClass="display"   size="10"></asp:TextBox></td>
            
        </tr>                        
    </table>  

                                   
</asp:Content>

<asp:Content ContentPlaceHolderID="queryHead" ID="myQueryHead" runat="server">
    <!--QueryHead!-->    
    <th class="fixCol">狀態</th>
    <th>客戶代號</th>
    <th>客戶名稱</th>    
    <th>申請書編號</th>    
    <th>記錄日期</th>  
    <th>記錄時間</th>  
    <th>輸入人員</th>  
    <th>記錄標題</th>  
</asp:Content>

<asp:Content ContentPlaceHolderID="queryBody" ID="myQueryBody" runat="server">
    <td class="fixCol"><%# Eval("STS")%><asp:HiddenField runat="server" ID="hiddenCUST_NO" Value='<%# Eval("CUST_NO").ToString()+","+ Eval("PHONE_DATE").ToString()+","+ Eval("PHONE_TIME").ToString()%>' /></td>
    <td><%# Eval("CUST_NO")%></td>
    <td><%# Eval("CUST_NAME")%></td>
    <td><%# Eval("APLY_NO")%></td>
    <td><%# Eval("PHONE_DATE")%></td>
    <td><%# Eval("PHONE_TIME")%></td>
    <td><%# Eval("KEY_NAME")%></td>    
    <td><%# Eval("REC_TITLE")%></td>
</asp:Content>


<asp:Content ContentPlaceHolderID="functionBar" runat="server">
<% if (this.bolWE020)
   {%>
 <div class="divButton">
    <input type="button" ID="btnExit" class="button exit" value="返回" OnClick="contentChange('frameContent');" />    
</div>   
<% }%>
</asp:Content>
