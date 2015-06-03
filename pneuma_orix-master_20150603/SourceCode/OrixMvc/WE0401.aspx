<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/editing.Master" CodeBehind="WE0401.aspx.cs" Inherits="OrixMvc.WE0401" %>
<%@ MasterType VirtualPath="~/Pattern/editing.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
        
<asp:Content ContentPlaceHolderID="editingArea" ID="myEditingArea" runat="server">
<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlSUPPORT_TYPE" 
    connectionstring="<%$ ConnectionStrings:orixConn%>"
    SelectCommand="select TypeCode=[報修類別代號],TypeDesc=[報修類別名稱] from ServiceType "
    runat="server" >
</asp:sqldatasource> 

 <table >
    <tr>
        <th class="nonSpace">客戶代號：</th>
        <td><ocxControl:ocxDialog runat="server" ID="CUST_NO" width="70" SourceName="OR_CUSTOM" Text='<%# Eval("CUST_NO") %>' ControlID="CUST_SNAME" FieldName="CUST_NAME" /></td>            
        <td><asp:TextBox runat="server" ID="CUST_SNAME"  CssClass="display" Text='<%# Eval("CUST_NAME") %>'  size="21"></asp:TextBox></td> 
        <th>結案日期：</th>
        <td><asp:TextBox runat="server" ID="CLOSED_DATE"  ReadOnly="true" CssClass="display" Text='<%# Eval("CLOSED_DATE") %>' size="10"></asp:TextBox></td>  
        <td rowspan="6">        
        <asp:CheckBox  style="margin-left:20px" runat="server" ID="chkSupport" Text="產生支援單" />
        <span style="margin-left:20px">支援類別：</span><span><asp:DropDownList  runat="server" ID="SupportType" Width="130" DataSourceID="sqlSUPPORT_TYPE"  DataValueField="TypeCode" DataTextField="TypeDesc" ></asp:DropDownList> </span><br />        
            <!--START Grid for editing!-->            
            <fieldset><legend><font class="memo">通知</font></legend>
<asp:UpdatePanel runat="server" ID="upDetailEditing" RenderMode="Inline" UpdateMode="Conditional" ChildrenAsTriggers="false">
    <ContentTemplate>    
                                     
<div class="gridMain " >
    <div style="padding:0;margin:0; overflow-y:scroll;width:270px;height:350px" runat="server" id="divGrid">        
        <table cellpadding="0" cellspacing="0" style="width:95%" id="tbImage">         
        <thead>
            <tr> 
                <th>編輯</th>               
                <th >員工代號</th>
                <th >員工姓名</th>                                     
            </tr> 
        </thead>     
            <asp:Repeater runat="server" ID="rptEdit"> 
                <ItemTemplate>
                    <tr  class="<%#Container.ItemIndex%2==0?"srow":"" %>">      
                        <td>
                           <asp:Button ID="btnDel" runat="server"  class="button del"  Text="刪除" OnCommand="GridFunc_Click" CommandName='<%# Eval("EMP_CODE") %>' />                           
                        </td>                                       
                        <td><asp:TextBox runat="server" ID="EMP_CODE" Text='<%# Eval("EMP_CODE") %>'   size="20" CssClass="display" ReadOnly="true" ></asp:TextBox></td> 
                        <td><asp:TextBox runat="server" ID="EMP_NAME" Text='<%# Eval("EMP_NAME") %>'   size="20" CssClass="display" ReadOnly="true" ></asp:TextBox></td> 
                    </tr>
                </ItemTemplate> 
            </asp:Repeater>
            <tr >
                <td><asp:Button ID="btnAdd" runat="server"  cssClass="button dadd"  Text="新增" OnCommand="GridFunc_Click" CommandName="Add" /> </td>
                <td ><ocxControl:ocxDialog runat="server" ID="addEMP_CODE" ControlID="addEMP_NAME" FieldName="EMP_NAME" width="100"  SourceName="OR_EMP" /></td>
                <td align="right"><asp:TextBox runat="server" ID="addEMP_NAME"   size="20" CssClass="display"  ></asp:TextBox></td>                
            </tr>  
        </table>
    </div>     
</div>     
        
    </ContentTemplate> 
</asp:UpdatePanel>
</fieldset>
<!--START Grid for editing!-->
    </td>
    </tr>
    <tr>
        <th>歸屬申請書：</th> 
        <td colspan="4" ><ocxControl:ocxDialog SourceName="OR_CASE_APLY_BASE" width="140" runat="server" Text='<%# Eval("APLY_NO") %>' ID="APLY_NO"></ocxControl:ocxDialog> <font class="memo">若不歸屬於任何申請書，請刪除申請書編號</font>            
        </td>
    </tr>        
    <tr>
        <th class="nonSpace">記錄標題：</th>
        <td colspan="3"><asp:TextBox ID="REC_TITLE" runat="server"   MaxLength="50" Width=200 Text='<%# Eval("REC_TITLE") %>'></asp:TextBox></td>        
        <td><asp:CheckBox runat="server" ID="chkSTS" Text="追蹤"  OnPreRender='checkList' ToolTip='<%# Eval("SRV_REC_STS") %>'/></td>        
    </tr>   
    <tr>
        <th >輸入人員：</th>
        <td><asp:TextBox ID="KEY_USER" runat="server"  ReadOnly="true" CssClass="display" size="10" Text='<%# Eval("KEY_USER") %>'></asp:TextBox></td>
    </tr> 
    <tr>
        <th >記錄日期：</th>
        <td colspan="2"><asp:TextBox ID="PHONE_DATE" runat="server"  ReadOnly="true" CssClass="display" Width="80" Text='<%# Eval("PHONE_DATE") %>'></asp:TextBox>
        <asp:TextBox ID="PHONE_TIME" runat="server"  ReadOnly="true" CssClass="display" Width="40" Text='<%# Eval("PHONE_TIME") %>'></asp:TextBox></td>
        <th >修改日期：</th>
        <td><asp:TextBox ID="RE_DATE" runat="server"  ReadOnly="true" CssClass="display" Width="80" Text='<%# Eval("RE_DATE") %>'></asp:TextBox><asp:TextBox ID="RE_TIME" runat="server"  ReadOnly="true" CssClass="display" Width="40" Text='<%# Eval("RE_TIME") %>'></asp:TextBox></td>
        
    </tr>  
    <tr>
        <th>記錄內容：</th>
        <td colspan="4">
            <asp:TextBox ID="REC_CONTENT" runat="server" TextMode="MultiLine"  width="350" Rows="20" Text='<%# Eval("REC_CONTENT") %>'></asp:TextBox>
        </td>
        
    </tr> 
</table>
</asp:Content> 

<asp:Content ContentPlaceHolderID="functionBar" runat="server" ID="myFunctionBar">
<span runat="server" id="spanClose">
<div class="divButton" style="float:right">
    <asp:Button runat="server" ID="btnClose" CssClass="button exit" Text="結案" OnClick="Closed_Click" />
</div>     
</span>
</asp:Content>