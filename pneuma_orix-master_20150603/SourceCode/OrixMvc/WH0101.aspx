<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/editing.Master" CodeBehind="WH0101.aspx.cs" Inherits="OrixMvc.WH0101" %>
<%@ MasterType VirtualPath="~/Pattern/editing.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxYM"  TagPrefix="ocxControl" Src="~/ocxControl/ocxYM.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxTime"  TagPrefix="ocxControl" Src="~/ocxControl/ocxTime.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
        
<asp:Content ContentPlaceHolderID="editingArea" ID="mySearchArea" runat="server">

<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlINDUSTRY_TYPE" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'INDUSTRY_TYPE' "
    runat="server" >
</asp:sqldatasource>


        
       
<table cellpadding="0"  cellspacing="0">
    <tr><td valign="top">
     <table style="margin-top:15px">
        <tr>
            <th class="nonSpace">業務員代號：</th>
            <td colspan="2"><asp:TextBox runat="server" ID="SALES" Text='<%# Eval("SALES") %>'  Width="150"  MaxLength="50" ></asp:TextBox></td>
       </tr>
       <tr>
            <th class="nonSpace">業務員姓名：</th>
            <td colspan="2">
    <asp:UpdatePanel runat="server" ID="upSALES1"  UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
                <asp:TextBox runat="server" ID="SALES1" Text='<%# Eval("SALES1") %>'  Width="150"  MaxLength="50" ></asp:TextBox>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="CORP_ACCT"  />
        </Triggers>
    </asp:UpdatePanel>    
            </td>
       </tr>
       <tr>
            <th>EMAIL：</th>
            <td colspan="2"><asp:TextBox runat="server" ID="EMAIL" Text='<%# Eval("EMAIL") %>'  Width="150"  MaxLength="50"  ></asp:TextBox></td>
       </tr> 
        <tr>
            <th>社內帳號：</th>
            <td colspan="2"><asp:TextBox runat="server" ID="CORP_ACCT" Text='<%# Eval("CORP_ACCT") %>'  Width="150"  MaxLength="10" AutoPostBack="true" OnTextChanged='CORP_ACCT_changed' ></asp:TextBox></td>
       </tr> 
        <tr> 
            <td></td>           
            <td><asp:CheckBox runat="server" ID="ENABLE" ToolTip='<%# Eval("ENABLE") %>' OnPreRender='checkList' Text='可用' /></td> 
            <td><asp:CheckBox runat="server" ID="SP_ENABLE" ToolTip='<%# Eval("SP_ENABLE") %>' OnPreRender='checkList' Text='SP可用' /></td> 
        </tr>   
        <tr><th>日報表群組：</th><td><ocxControl:ocxNumber runat="server" ID="DAY_REPORT_GROUP" MASK="999" Text='<%# Eval("DAY_REPORT_GROUP") %>' /></td></tr>       
    </table>
    </td>
    <td valign="top">
    <asp:UpdatePanel runat="server" ID="upDetailEditing" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
        
        <div  class="gridMain " style="width:545px">
        <div style="padding:0;margin:0; overflow-y:scroll;width:540px;height:300px;position:relative; " id="editGrid" runat="server">
            <table cellpadding="0" cellspacing="0"  style="width:480px" > 
            <thead>
                <tr>      
                    <th>編輯</th>          
                    <th class="nonSpace">生效日</th>
                    <th class="nonSpace">業務員部門代號</th>
                    <th>業務員部門名稱</th>
                    <th>業務員組別</th>                            
                </tr> 
            </thead>    
                <asp:Repeater runat="server" ID="rptEdit"  > 
                    <ItemTemplate>
                        <tr style="display:<%# Eval("Status").ToString()=="D"?"none":"" %>" > 
                            <td>
                            <% if (!this.bolQuery)
                               {%>
                            <asp:Button ID="btnDel" runat="server"  cssClass="button del"  Text="刪除"  OnCommand="GridFunc_Click"  Visible='<%# Eval("EFF_DATE").ToString().CompareTo(this.EFFDate)>=0 && (Eval("Status").ToString()=="A" || Eval("Status").ToString()=="")?true:false %>' CommandName='<%# Eval("EFF_DATE") %>' />
                            <%} %>
                            <asp:HiddenField runat="server" ID="Status"  Value='<%# Eval("Status") %>' /></td>
                            <td><ocxControl:ocxDate runat="server" ID="EFF_DATE" Text='<%# Eval("EFF_DATE") %>' /></td>
                            <td><ocxControl:ocxDialog runat="server" ID="DEP_NO" width="80" ControlID="DEPT_NAME" FieldName="DEPT_NAME"  SourceName="OR_DEPT" Text='<%# Eval("DEP_NO") %>' /></td>
                            <td><asp:TextBox runat="server" ID="DEP" Text='<%# Eval("DEP") %>' MaxLength="20" CssClass="display"    ></asp:TextBox></td>                            
                            <td><asp:TextBox runat="server" ID="DEP_NO2" Text='<%# Eval("DEP_NO2") %>' MaxLength="20"  ></asp:TextBox></td>
                        </tr>
                    </ItemTemplate> 
                </asp:Repeater>                       
                <tr style='display:<%=  this.bolQuery ?"none":""%>'>
                    <td><asp:Button ID="btnAdd" runat="server"  cssClass="button dadd"  Text="新增" OnCommand="GridFunc_Click" CommandName="Add" /> </td>
                    <td><ocxControl:ocxDate runat="server" ID="addEFF_DATE"  /></td>
                    <td><ocxControl:ocxDialog runat="server" ID="addDEP_NO" width="80" ControlID="addDEP" FieldName="DEPT_NAME"  SourceName="OR_DEPT"  /></td>
                    <td><asp:TextBox runat="server" ID="addDEP"  MaxLength="20" CssClass="display"   ></asp:TextBox></td>
                    <td><asp:TextBox runat="server" ID="addDEP_NO2"  MaxLength="20"  ></asp:TextBox></td>                       
                </tr>
            </table>
        </div>     
    </div> 
    </ContentTemplate>
     <Triggers>
                <asp:PostBackTrigger ControlID="btnAdd" /> 
            </Triggers>
    </asp:UpdatePanel>
    </td>
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
