<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/query.Master" CodeBehind="WH020.aspx.cs" Inherits="OrixMvc.WH020" %>
<%@ MasterType VirtualPath="~/Pattern/query.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
    
<asp:Content ContentPlaceHolderID="searchArea" ID="mySearchArea" runat="server">

    <table >
        <tr>
            <th>契約編號：</th><td><asp:TextBox runat="server" ID="NUM"   size="14"></asp:TextBox></td> 
        </tr>
        <tr>    
            <th class="nonSpace">日期：</th><td colspan="2"><ocxControl:ocxDate runat="server" ID="DATE_S" />~<ocxControl:ocxDate runat="server" ID="DATE_E" /></td>           
        </tr>  
        <tr>
            <th>營業人員：</th>
            <td><ocxControl:ocxDialog runat="server" ID="EMP_CODE"  width="60" ControlID="EMP_NAME" FieldName="EMP_NAME" SourceName="OR_EMP" /></td>            
            <td><asp:TextBox runat="server" ID="EMP_NAME" CssClass="display" ReadOnly="true"   size="14"></asp:TextBox></td>
            
        </tr>   
    </table>
                     
</asp:Content>
<asp:Content runat="server" ID="functionArea" ContentPlaceHolderID="functionBar">
<script language="javascript" type="text/javascript">
 function openDetail() {


        var obj = document.getElementById("divPopWindow");
        obj.style.width = "340px";
        obj.style.height = "100px";
       

        obj.style.top = "150px";
        obj.style.left = "150px";

        window.parent.openPopUpWindow();
    }
</script>

     <div class="divButton">
        <input type="button" value="匯入" class="button add" onclick="openDetail();" />            
                    </div>
              
</asp:Content>

<asp:Content ID="myPopWindow" ContentPlaceHolderID="PopWindow" runat="server">
    <script type="text/javascript" language="javascript">
        function chkFNUM(){
            if (document.getElementById('<%=this.F_NUM.ClientID %>').value==""){
                alert("契約編號必須輸入!!");
                return false;
            }

            window.parent.closePopUpWindow();
            return true;
            
            
        }
        
        
    
    
    </script>
    <table >
        <tr>
            <th>契約編號：</th><td><ocxControl:ocxDialog runat="server" ID="F_NUM" width="140"  SourceName="OR_APLY_WH020" /></td>    
            <td>
                 <div class="divButton">
                <asp:Button runat="server" ID="btnImport" CssClass="button qry" Text="確定"  CommandName="Import" OnClientClick="return chkFNUM();"  />
                </div>
            </td>
        </tr>          
    </table>
</asp:Content>

<asp:Content ContentPlaceHolderID="queryHead" ID="myQueryHead" runat="server">
    <!--QueryHead!-->    
    
    <th class="fixCol">契約編號</th>
    <th>日期</th>
    <th>客戶名稱</th>    
    <th>期數</th>    
    <th>營業人員</th>  
    <th>營業人員姓名</th>  
    <th>案件類別</th>  
    <th>期買額</th>  
    <th>契約額</th>
</asp:Content>
<asp:Content ContentPlaceHolderID="gridButton" runat="server" ID="myGridButton">
    <asp:Button runat="server" ID="btnBack" CssClass="button upd" Text='特殊修改'  CommandName="Upd"  />    
</asp:Content>
<asp:Content ContentPlaceHolderID="queryBody" ID="myQueryBody" runat="server">

    <td class="fixCol"><%# Eval("NUM")%><asp:HiddenField runat="server" ID="hiddenNUM" Value='<%# Eval("NUM")%>' /> </td>
    <td><%# Eval("DAY")%></td>
    <td><%# Eval("CUSTOMER")%></td>
    <td><%# Eval("TERM")%></td>
    <td><%# Eval("SALES")%></td>
    <td><%# Eval("EMP_NAME")%></td>
    <td><%# Eval("ST")%></td>
    <td class="number"><%# Eval("COST", "{0:###,###,###,##0}")%></td>
    <td class="number"><%# Eval("TOTAL", "{0:###,###,###,##0}")%></td>
</asp:Content>
