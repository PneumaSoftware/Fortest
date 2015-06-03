<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/query.Master" CodeBehind="WF030.aspx.cs" Inherits="OrixMvc.WF030" %>
<%@ MasterType VirtualPath="~/Pattern/query.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
    
<asp:Content ContentPlaceHolderID="searchArea" ID="mySearchArea" runat="server">
    
    <table >
        <tr>
            <th class="nonSpace">員工代號：</th><td><ocxControl:ocxDialog runat="server" ID="EMP_CODE" width="100"  SourceName="OR_EMP" /></td>
            <th>員工姓名：</th><td><asp:TextBox runat="server" ID="EMP_NAME"    size="10"></asp:TextBox></td>            
        </tr>     
    </table>
    

                                   
</asp:Content>

<asp:Content ContentPlaceHolderID="queryHead" ID="myQueryHead" runat="server">
    <!--QueryHead!-->    
    <th class="fixCol">申請書編號</th>
    <th>供應商名稱</th>
    <th>客戶名稱</th>    
    <th>申請金額</th>      
    <th>實際TR</th>  
    <th>預計起租日</th>  
    <th>進度</th>  
    <th>預計本月起租金額</th>  
    <th>預計次月起租金額</th>  
    <th>預計本季起租金額</th>  
    <th>預計次季起租金額</th>  
    <th>備註</th>  
</asp:Content>
<asp:Content ContentPlaceHolderID="queryBody" ID="myQueryBody" runat="server">
    <td class="fixCol"><%# Eval("APLY_NO")%><asp:HiddenField runat="server" ID="hiddenAPLY" Value='<%# Eval("APLY_NO")%>' /></td>
     <td><%# Eval("FRC_SNAME")%></td>
    <td><%# Eval("CUST_SNAME")%></td>
    <td class="number"><%# Eval("APRV_BUY_AMT", "{0:###,###,###,##0}")%></td> 
    <td class="number"><%# Eval("APRV_REAL_TR", "{0:###,###,###.###0}")%></td>
    <td><%# Eval("PREDICT_LEASE_DATE")%></td>
    <td><%# Eval("PROGRESS")%></td>
    <td class="number"><%# Eval("AMT1", "{0:###,###,###,##0}")%></td>
    <td class="number"><%# Eval("AMT2", "{0:###,###,###,##0}")%></td>
    <td class="number"><%# Eval("AMT3", "{0:###,###,###,##0}")%></td>
    <td class="number"><%# Eval("AMT4","{0:###,###,###,##0}")%></td>
    <td><%# Eval("REMARK")%></td>
</asp:Content>


