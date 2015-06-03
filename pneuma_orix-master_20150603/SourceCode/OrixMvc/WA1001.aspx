<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/editing.Master" CodeBehind="WA1001.aspx.cs" Inherits="OrixMvc.WA1001" %>
<%@ MasterType VirtualPath="~/Pattern/editing.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
        
<asp:Content ContentPlaceHolderID="editingArea" ID="myEditingArea" runat="server">

<asp:HiddenField runat="server" ID="APLY_NO" Value='<%# Eval("APLY_NO") %>'  />
<asp:HiddenField runat="server" ID="POLICY_SUBJECT" Value='<%# Eval("POLICY_SUBJECT") %>' />
<asp:HiddenField runat="server" ID="ASUR_TYPE_CODE" Value='<%# Eval("ASUR_TYPE_CODE") %>' />


<!--START Grid for editing!-->
<asp:UpdatePanel runat="server" ID="upGrid" RenderMode="Inline" UpdateMode="Conditional" ChildrenAsTriggers="false">
    <ContentTemplate>    
    
<div class="gridMain " >
    <div style="padding:0;margin:0; overflow-y:scroll;width:800px;height:494px" runat="server" id="divGrid">
        
        <table style="width:500px" id="tbImage"> 
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
                    <tr style="display:<%# Eval("Status").ToString()=="D"?"none":"" %>">     
                        <td>
                           <asp:Button ID="btnDel" runat="server"  cssclass="button func"  Text="刪除" OnCommand="GridFunc_Click" CommandName='<%# Eval("POLICY_NO") %>' />
                           <asp:HiddenField runat="server" ID="Status"  Value='<%# Eval("Status") %>' />
                        </td>             
                        <td><asp:TextBox ID="POLICY_NO"  Text='<%# Eval("POLICY_NO") %>' runat="server" ></asp:TextBox></td>  
                        <td><ocxControl:ocxDate runat="server" ID="ASUR_S_DATE" Text='<%# Eval("ASUR_S_DATE") %>'  /> </td>                         
                        <td><ocxControl:ocxDate runat="server" ID="ASUR_E_DATE" Text='<%# Eval("ASUR_E_DATE") %>'  /> </td> 
                    </tr>
                </ItemTemplate> 
            </asp:Repeater>
            <tr>
                <td>
                   <asp:Button ID="btnAdd" runat="server"  cssclass="button func"  Text="新增" OnCommand="GridFunc_Click" CommandName="Add" />                   
                </td>              
                <td>
                    <asp:TextBox ID="addPOLICY_NO" runat="server" ></asp:TextBox>
                </td>
                <td>
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
<!--START Grid for editing!-->
 
</asp:Content> 
