<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/empty.Master" CodeBehind="WA090.aspx.cs" Inherits="OrixMvc.WA090" %>
<%@ MasterType VirtualPath="~/Pattern/empty.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
        
<asp:Content ContentPlaceHolderID="editingArea" ID="mySearchArea" runat="server">



<div class="searchArea">
    <table cellpadding="2" cellspacing="2" >
        <tr>
            <td>
                <table >
                    <tr>
                        <th class="nonSpace">申請書編號：</th><td><ocxControl:ocxDialog SourceName="OR_CASE_APLY_BASE" runat="server" ID="APLY_NO" width="140" /></td>                       
                    </tr>                        
                </table>
                
            </td>
            <td style="text-align:right;">
                
                <div class="divButton">
                <asp:Button runat="server" ID="btnQry" CssClass="button qry" Text="查詢" OnCommand="Status_Click" CommandName="Query"/>
                </div>            
                          
            </td>
        </tr>        
    </table>    
</div>                     


<div id="query" class="gridMain ">
    <div style="padding:0;margin:0; overflow-y:scroll; position:relative;width:800px;height:434px" runat="server" id="divGrid">
        <table cellpadding="0" cellspacing="0"  id="tbImage" style="width:720px" > 
        <thead>
            <tr>                
                <th style="width:60px" >編輯</th>
                <th style="width:40px" >序號</th>
                <th style="width:100px" class="nonSpace" >附件代碼</th>
                <th style="width:150px" >附件名稱</th>                
                <th style="width:250px">備註</th>
                <th style="width:120px" class="nonSpace" >影像</th>
            </tr> 
        </thead>    
            <asp:Repeater runat="server" ID="rptEdit"> 
                <ItemTemplate>
                    <tr style="display:<%# Eval("Status").ToString()=="D"?"none":"" %>">     
                        <td style="width:60px">
                           <asp:Button ID="btnDel" runat="server"  cssclass="button del"  Text="刪除" OnCommand="GridFunc_Click" CommandName='<%# Eval("SEQ_NO") %>' />
                           <asp:HiddenField runat="server" ID="Status"  Value='<%# Eval("Status") %>' />
                        </td>               
                        <td style="width:40px"><asp:TextBox ID="SEQ_NO" runat="server" Text='<%# Eval("SEQ_NO") %>' CssClass="display" ReadOnly="true"></asp:TextBox></td>                        
                        <td style="width:100px"><ocxControl:ocxDialog runat="server" ID="ATCH_CODE" width="60"   SourceName="OR_ATCH_CODE" Text='<%# Eval("ATCH_CODE") %>'  FieldName="ATCH_NAME"  /></td>
                        <td style="width:150px"><asp:TextBox ID="ATCH_NAME"   CssClass="display" runat="server"  Text='<%# Eval("ATCH_NAME") %>' ></asp:TextBox></td>                   
                        <td style="width:250px"><asp:TextBox ID="REMARK"  runat="server" Text='<%# Eval("REMARK") %>'></asp:TextBox></td>                   
                        <td style="width:120px"><ocxControl:ocxUpload ID="FILE_SEQ" runat="server" Seq='<%# Eval("FILE_SEQ") %>' MIME='<%# Eval("MIME") %>' ExtName='<%# Eval("ExtName") %>' bImage=<%# Eval("bImage") %>></ocxControl:ocxUpload></td>                        
                    </tr>
                </ItemTemplate> 
            </asp:Repeater>
            <tr style='display:<%= this.APLY_NO.Text.Trim()==""?"none":""%>'>
                <td style="width:60px">
                   <asp:Button ID="btnAdd" runat="server"  cssclass="button dadd"  Text="新增" OnCommand="GridFunc_Click" CommandName="Add" />                   
                </td>
                 <td style="width:40px"></td>                
                <td style="width:100px">
                    <ocxControl:ocxDialog runat="server" ID="addATCH_CODE" width="60"   SourceName="OR_ATCH_CODE" FieldName="ATCH_NAME" ControlID="addATCH_NAME" />
                </td>
                <td style="width:150px">
                    <asp:TextBox ID="addATCH_NAME"   CssClass="display" runat="server"  ></asp:TextBox>
                </td>
                <td style="width:250px">
                     <asp:TextBox ID="addREMARK" runat="server" ></asp:TextBox>
                </td>
                <td style="width:120px">
                     <ocxControl:ocxUpload ID="addFILE_SEQ" runat="server" />
                </td>
            </tr>
        </table>
    </div>     
</div> 
    
                            
</asp:Content>

