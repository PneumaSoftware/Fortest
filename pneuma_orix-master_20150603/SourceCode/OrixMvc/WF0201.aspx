<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/editing.Master" CodeBehind="WF0201.aspx.cs" Inherits="OrixMvc.WF0201" %>
<%@ MasterType VirtualPath="~/Pattern/editing.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
        
<asp:Content ContentPlaceHolderID="editingArea" ID="mySearchArea" runat="server">
<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlProgress" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'TargetProcess' "
    runat="server" >
</asp:sqldatasource>

    <asp:HiddenField runat="server" ID="SeqNo" Value='<%# Eval("SeqNo") %>' />
    <table>
        <tr>            
            <th>供應商代號：</th><td colspan="2"><ocxControl:ocxDialog runat="server" ID="SUPL_CODE" width="103" ControlID="FRC_SNAME" FieldName="FRC_SNAME" Text='<%# Eval("SUPL_CODE") %>' SourceName="OR_FRC" /><asp:TextBox CssClass="display" Text='<%# Eval("FRC_SNAME") %>' runat="server" ID="FRC_SNAME" MaxLength="20" ></asp:TextBox></td>            
        </tr>  
        <tr>            
            <th>客戶代號：</th><td  colspan="2"><ocxControl:ocxDialog runat="server" ID="CUST_CODE" width="103" ControlID="CUST_SNAME" FieldName="CUST_SNAME" SourceName="OR_CUSTOM" Text='<%# Eval("CUST_NO") %>' /><asp:TextBox runat="server"  Text='<%# Eval("CUST_SNAME") %>' ID="CUST_SNAME"  CssClass="display"  size="20"></asp:TextBox></td>             
        </tr>   
        <tr>
            <th>預計申請金額：</th>
            <td><ocxControl:ocxNumber MASK="999,999,999" ID="FORECAST_APLY_AMT" runat="server" Text='<%# Eval("FORECAST_APLY_AMT") %>'  /></td>           
        </tr>
        <tr>
            <th class="nonSpace">案件進度：</th>
            <td><asp:DropDownList ID="PROGRESS"  runat="server"  OnPreRender='checkList' ToolTip='<%# Eval("PROGRESS") %>' Width="103"
                 DataSourceID="sqlProgress" DataValueField="TypeCode" DataTextField="TypeDesc" >
                </asp:DropDownList>  </td>                       
            <th class="nonSpace">備註：</th>   
           <td><asp:TextBox runat="server" ID="REMARK"  Text='<%# Eval("REMARK") %>' size="50" Rows="6"></asp:TextBox></td>                   
        </tr>              
        <tr>
            <th>申請書編號：</th>
            <td><ocxControl:ocxDialog SourceName="OR_CASE_APLY_BASE" width="140"  Text='<%# Eval("APLY_NO") %>' FieldName="CUR_STS" ControlID="APLY_STS"  runat="server" ID="APLY_NO"></ocxControl:ocxDialog></td>
            <th>目前狀況：</th>
            <td><asp:TextBox runat="server" ID="APLY_STS" CssClass="display"  Text='<%# Eval("APLY_STS") %>' size="10"></asp:TextBox></td>
        </tr>       
    </table>
    
    
                           
</asp:Content>


<asp:Content ID="myPopWindow" ContentPlaceHolderID="PopWindow" runat="server">
     

     
<script language="javascript" type="text/javascript">
    function openDetail(strDate, strType, strName) {

    
        window.parent.openPopUpWindow();
    }
</script>
</asp:Content> 
