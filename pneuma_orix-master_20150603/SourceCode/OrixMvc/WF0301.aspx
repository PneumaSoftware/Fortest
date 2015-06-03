<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/editing.Master" CodeBehind="WF0301.aspx.cs" Inherits="OrixMvc.WF0301" %>
<%@ MasterType VirtualPath="~/Pattern/editing.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
        
<asp:Content ContentPlaceHolderID="editingArea" ID="mySearchArea" runat="server">
<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlProgress" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'APRVCASEPROC' "
    runat="server" >
</asp:sqldatasource>


    
    <table>
        <tr>
            <th>申請書編號：</th>
            <td><asp:TextBox runat="server" ID="APLY_NO" CssClass="display" ReadOnly="true" Text='<%# Eval("APLY_NO") %>'  size="15"></asp:TextBox></td>            
            <th>客戶名稱：</th>
            <td><asp:TextBox runat="server" ID="CUST_SNAME" CssClass="display" ReadOnly="true"  Text='<%# Eval("CUST_SNAME") %>' size="30"></asp:TextBox></td>            
        </tr>
        <tr>
            <th>購買額：</th>
            <td><asp:TextBox runat="server" ID="APRV_BUY_AMT" CssClass="display number" ReadOnly="true" Text='<%# Eval("APRV_BUY_AMT","{0:###,###,###,##0}") %>'  size="15"></asp:TextBox></td>            
            <th>供應商名稱：</th>
            <td><asp:TextBox runat="server" ID="FRC_SNAME" CssClass="display" ReadOnly="true"  Text='<%# Eval("FRC_SNAME") %>' size="30"></asp:TextBox></td>            
        </tr>        
        <tr>
            <th class="nonSpace">進度：</th>
            <td><asp:DropDownList ID="PROGRESS"  runat="server"  OnPreRender='checkList' ToolTip='<%# Eval("PROGRESS_Code") %>'
                                                                          
                 DataSourceID="sqlProgress" DataValueField="TypeCode" DataTextField="TypeDesc" >
                </asp:DropDownList>
            </td>
            <th class="nonSpace">預計起租日：</th>
            <td><ocxControl:ocxDate runat="server" ID="PREDICT_LEASE_DATE"  Text='<%# Eval("PREDICT_LEASE_DATE") %>' /></td>            
        </tr>   
        <tr>
            <th class="nonSpace">備註：</th>
            <td colspan="3"><asp:TextBox runat="server" ID="REMARK"  TextMode="MultiLine"  Text='<%# Eval("REMARK") %>' Width="550" Rows="6"></asp:TextBox></td>
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
