<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/empty.Master" CodeBehind="WZ010.aspx.cs" Inherits="OrixMvc.WZ010" %>
<%@ MasterType VirtualPath="~/Pattern/empty.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
        
<asp:Content ContentPlaceHolderID="editingArea" ID="mySearchArea" runat="server">
<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlMenu" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand=
    " select a.FUNC_ID,FUNC_NAME=''+a.FUNC_ID+' '+a.FUNC_NAME  from OR3_FUNCTION  a left join OR3_MENU_SET b
      on a.FUNC_ID=b.FUNC_ID 
      where FUNC_TYPE='S' order by ATCH_SEQ
    "
    runat="server" >   
   
</asp:sqldatasource>

<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlFunction" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand=
    " select a.FUNC_ID,FUNC_NAME,b.PARENT_ID,b.ATCH_SEQ,a.FUNC_TYPE  from OR3_FUNCTION a left join OR3_MENU_SET b
      on a.FUNC_ID=b.FUNC_ID 
      order by (case when a.FUNC_TYPE='S' then 0 else 1 end), a.FUNC_ID  
    "
    runat="server" >  
   
</asp:sqldatasource>

                            
<div id="query" class="gridMain ">
    <div style="padding:0;margin:0; overflow-y:scroll;width:100%;height:494px" runat="server" id="divGrid">
        <table cellpadding="0" cellspacing="0" style="width:95%" id="tbFunction"> 
        <thead>
            <tr>
                <th style="width:25%">系統名稱</th>
                <th style="width:15%">功能代碼</th>
                <th style="width:40%">功能名稱</th>                
                <th style="width:15%">顯示順序</th>
            </tr> 
        </thead>    
            <asp:Repeater runat="server" ID="rptFunction" DataSourceID="sqlFunction">
                <ItemTemplate>
                    <tr >
                         <td style="width:25%" >          
                            <asp:DropDownList  runat="server" ID="PARENT_ID" OnPreRender='checkList' ToolTip='<%# Eval("PARENT_ID") %>'
                            Visible='<%#Eval("FUNC_TYPE").ToString().Trim()=="S"?false:true %>'                                 
                              DataValueField="FUNC_ID" DataSourceID="sqlMenu" DataTextField="FUNC_NAME" ></asp:DropDownList>                              
                                         
                        </td>
                        <td style="width:15%"><%# Eval("Func_ID") %></td>
                        <td style="width:40%"><%# Eval("FUNC_NAME") %></td>                   
                        <td style="width:15%"><ocxControl:ocxNumber MASK="99" ID="ATCH_SEQ" Text ='<%# Eval("ATCH_SEQ") %>' runat="server" /> <asp:HiddenField runat="server" ID="FUNC_TYPE" Value='<%# Eval("FUNC_TYPE") %>' /><asp:HiddenField runat="server" ID="FUNC_ID" Value='<%# Eval("Func_ID") %>' /></td>
                        
                    </tr>
                </ItemTemplate> 
            </asp:Repeater>
        </table>
    </div>     
</div> 

                            
</asp:Content>

