<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/display.Master" CodeBehind="WA100.aspx.cs" Inherits="OrixMvc.WA100" %>
<%@ MasterType VirtualPath="~/Pattern/display.Master" %>
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

<asp:Content ContentPlaceHolderID="editingArea" ID="myEditArea" runat="server">

                 
<asp:UpdatePanel runat="server" RenderMode="Inline" ID="upGrid1" UpdateMode="Conditional">
<ContentTemplate>

<div id="query" class="gridMain">
    <div style="padding:0;margin:0; overflow-y:scroll;position:relative;width:800px;height:210px" id="divGrid">
        <table cellpadding="0" cellspacing="0" style="width:500px" > 
        <thead>
            <tr>                
                <th></th>
                <th class="fixCol">案件/動態擔保</th>
                <th>動產品名</th>
                <th>保險種類代碼</th>    
                <th>保險種類名稱</th>    
                <th>金額</th>  
                <th>保險迄日</th>  
            </tr> 
        </thead>    
            <asp:Repeater runat="server" ID="rptQuery"> 
                <ItemTemplate>
                    <tr id='tr<%# Container.ItemIndex+1  %>' class='<%#Container.ItemIndex%2==0?"srow":"" %>' >     
                        <td><asp:UpdatePanel runat="server" ID="upDetail" RenderMode="Inline" UpdateMode="Conditional"> <ContentTemplate>
                           <asp:Button ID="btnDetail" runat="server"  cssclass="button func"  Text="明細" OnCommand="GridFunc_Click" CommandName='<%# Eval("POLICY_SUBJECT").ToString()+","+Eval("ASUR_TYPE_CODE").ToString() %>' />
                           </ContentTemplate></asp:UpdatePanel>
                        </td>               
                       <td><%# Eval("POLICY_SUBJECT")%></td>
                        <td><%# Eval("PROD_NAME")%></td>
                        <td><%# Eval("ASUR_TYPE_CODE")%></td>
                        <td><%# Eval("ASUR_TYPE_NAME")%></td>
                        <td><%# Eval("AMOUNT", "{0:###,###,###,##0}")%></td>
                        <td><%# Eval("ASUR_E_DATE")%></td>                       
                    </tr>
                </ItemTemplate> 
            </asp:Repeater>            
        </table>
    </div>     
</div> 
</ContentTemplate>
</asp:UpdatePanel>       
 

<asp:UpdatePanel runat="server" RenderMode="Inline" ID="upGrid2" UpdateMode="Conditional">
<ContentTemplate>
<div class="gridMain" >
    <div style="padding:0;margin:0; overflow-y:scroll;width:800px;height:200px" >
        
        <table style="width:400px"  class="edit"> 
        <thead>
            <tr>                
                <th >編輯</th>
                <th class="nonSpace" >保單號碼</th>
                <th class="nonSpace" >保險起日</th>
                <th class="nonSpace" >保險迄日</th>                                
            </tr> 
        </thead>    
            <asp:Repeater runat="server" ID="rptEdit"> 
                <ItemTemplate>
                    <tr class='<%#Container.ItemIndex%2==0?"srow":"" %>' style="display:<%# Eval("Status").ToString()=="D"?"none":"" %>">     
                        <td>
                           <asp:Button ID="btnDel" runat="server"  cssclass="button del"  Text="刪除" OnCommand="GridFunc_Click" CommandName='<%# Eval("POLICY_NO") %>' />
                           <asp:HiddenField runat="server" ID="Status"  Value='<%# Eval("Status") %>' />
                        </td>             
                        <td><asp:TextBox ID="POLICY_NO"  Text='<%# Eval("POLICY_NO") %>' runat="server" ></asp:TextBox></td>  
                        <td><ocxControl:ocxDate runat="server" ID="ASUR_S_DATE" Text='<%# Eval("ASUR_S_DATE") %>'  /> </td>                         
                        <td><ocxControl:ocxDate runat="server" ID="ASUR_E_DATE" Text='<%# Eval("ASUR_E_DATE") %>'  /> </td> 
                    </tr>
                </ItemTemplate> 
            </asp:Repeater>
            <tr style='display:<%=this.POLICY_SUBJECT==""?"none":""%>'>
                <td>
                   <asp:Button ID="btnAdd" runat="server"  cssclass="button dadd"  Text="新增" OnCommand="GridFunc_Click" CommandName="Add" />                   
                </td>              
                <td>
                    <asp:TextBox ID="addPOLICY_NO" runat="server" ></asp:TextBox>
                </td>
                <td id="td1">
                    <ocxControl:ocxDate runat="server" ID="addASUR_S_DATE"  />                    
                </td>
                <td>
                    <ocxControl:ocxDate runat="server" ID="addASUR_E_DATE" />
                </td>              
            </tr>
        </table>
    </div>
</div>  
 
</ContentTemplate>
</asp:UpdatePanel> 

<br />
<div class="divButton" style="float:right">
    <asp:Button runat="server" ID="btnExit" CssClass="button exit" Text="清除" OnClick="Clear_Click" />
</div>   
<div class="divButton" style='float:right;'>
    <asp:Button runat="server" ID="btnEdit" CssClass="button save" Text="儲存" OnCommand="Save_Click" CommandName="Save"   />
</div>                      

<script language="javascript" type="text/javascript">
    
</script>           
</asp:Content>

