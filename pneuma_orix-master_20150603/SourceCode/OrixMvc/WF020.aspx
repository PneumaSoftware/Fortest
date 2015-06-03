<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/query.Master" CodeBehind="WF020.aspx.cs" Inherits="OrixMvc.WF020" %>
<%@ MasterType VirtualPath="~/Pattern/query.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
    
<asp:Content ContentPlaceHolderID="searchArea" ID="mySearchArea" runat="server">
 <asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlTargetSTS" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'TargetSTS' "
    runat="server" >
</asp:sqldatasource>

    <table >
        <tr>
            <th>業務：</th><td><ocxControl:ocxDialog runat="server" ID="PEMP_NO" width="100"  SourceName="OR_EMP" /></td>
            <th>狀態：</th><td><asp:DropDownList ID="PTRACK_STS"  runat="server"
                                                                          
                 DataSourceID="sqlTargetSTS" DataValueField="TypeCode" DataTextField="TypeDesc" >
                </asp:DropDownList> </td> 
            <th>新增日期：</th><td><ocxControl:ocxDate runat="server" ID="PADD_DATE_S" />~<ocxControl:ocxDate runat="server" ID="PADD_DATE_E" /></td>           
        </tr>  
        <tr>
            <th>供應商代號：</th><td><ocxControl:ocxDialog runat="server" ID="PSUPL_CODE" width="100"  SourceName="OR_FRC" /> </td><th>供應商名稱：</th><td colspan="3"><asp:TextBox runat="server" ID="PFRC_NAME" size="40" MaxLength="50" ></asp:TextBox></td> 
        </tr>
        <tr>
            <th>客戶代號：</th><td><ocxControl:ocxDialog runat="server" ID="PCUST_CODE" width="100" SourceName="OR_CUSTOM" /> </td><th>客戶名稱：</th><td colspan="3"><asp:TextBox runat="server" ID="PCUST_NAME"   size="40" MaxLength="50"></asp:TextBox></td> 
        </tr>   
    </table>
    

                         
</asp:Content>
<asp:Content ContentPlaceHolderID="queryHead" ID="myQueryHead" runat="server">
    <!--QueryHead!-->
    <th class="fixCol">新增日期</th>
    <th>業務</th>
    <th>供應商代號</th>    
    <th>供應商名稱</th>    
    <th>客戶代號</th>  
    <th>客戶名稱</th>  
    <th>狀態</th>      
    <th>預計申請金額</th>  
    <th>進度</th>  
    <th>備註</th>      
</asp:Content>
<asp:Content ContentPlaceHolderID="gridButton" runat="server" ID="myGridButton">
    <asp:Button runat="server" ID="btnSet" CssClass="button upd" Text='追蹤'  CommandName="Set" Visible='<%# Eval("TRACK_STS").ToString()=="9"?true:false %>'  />
    <asp:Button runat="server" ID="btnCancel" CssClass="button del" Text='作廢'  CommandName="Cancel" Visible='<%# Eval("TRACK_STS").ToString()=="1"?true:false %>'  />
</asp:Content>
<asp:Content ContentPlaceHolderID="queryBody" ID="myQueryBody" runat="server">
    <td class="fixCol"><%# Eval("ADD_DATE")%><asp:HiddenField runat="server" ID="hiddenSeqNo" Value='<%# Eval("SeqNo")%>' /> </td>
    <td><%# Eval("EMP_no")%></td>
    <td><%# Eval("SUPL_CODE")%></td>
    <td><%# Eval("FRC_SNAME")%></td>
    <td><%# Eval("CUST_NO")%></td>
    <td><%# Eval("CUST_SNAME")%></td>
    <td><%# Eval("TRACK_STS_DESC")%></td>
    <td class="number"><%# Eval("FORECAST_APLY_AMT","{0:###,###,###,##0}")%></td>
    <td><%# Eval("PROGRESS_DESC")%></td>
    <td><%# Eval("REMARK")%></td>    
</asp:Content>


