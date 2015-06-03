<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/editing.Master" CodeBehind="WA0101.aspx.cs" Inherits="OrixMvc.WA0101" %>
<%@ MasterType VirtualPath="~/Pattern/editing.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxTime"  TagPrefix="ocxControl" Src="~/ocxControl/ocxTime.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
        
<asp:Content ContentPlaceHolderID="editingArea" ID="myEditingArea" runat="server">
  <asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlCNTR_DEPT" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'CNTR_DEPT' "
    runat="server" >
</asp:sqldatasource> 
   
    <table >
        <tr>
            <th class="nonSpace">範本代碼：</th><td><asp:TextBox runat="server" ID="TMP_CODE" Text='<%# Eval("TMP_CODE") %>'  Width="100"  size="10"></asp:TextBox></td> 
            <th class="nonSpace">範本說明：</th><td><asp:TextBox runat="server" ID="TMP_DESC" Text='<%# Eval("TMP_DESC") %>'  Width="300"  size="30"></asp:TextBox></td>
            <th class="nonSpace">部門：</th>
            <td><asp:DropDownList ID="DEPT"  runat="server" Width="100" DataSourceID="sqlCNTR_DEPT" DataValueField="TypeCode" DataTextField="TypeDesc" OnPreRender='checkList' ToolTip='<%# Eval("DEPT") %>'  >
                </asp:DropDownList> </td>       
        </tr>    
        <tr>
            <th class="nonSpace">合約條文：</th>
            <td colspan="3"><ocxControl:ocxUpload ID="CON_COND_FILE_SEQ" runat="server" Seq='<%# Eval("CON_COND_FILE_SEQ") %>' ></ocxControl:ocxUpload></td>                      
        </tr>   
        <tr>
            <th class="nonSpace">附表樣版：</th>
            <td colspan="3"><ocxControl:ocxUpload ID="CON_ATT_FILE_SEQ" runat="server" Seq='<%# Eval("CON_ATT_FILE_SEQ") %>' ></ocxControl:ocxUpload></td>                      
        </tr>   
          
    </table>
 </asp:Content>
 
 <asp:Content ContentPlaceHolderID="gridArea" runat="server" ID="myGridArea">   
 <asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlPARA_CODE" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="select TypeCode = '', TypeDesc='' union all select PARA_CODE as TypeCode,PARA_DESC as TypeDesc from OR3_CONTRACT_TEMP_PARA_LST "
    runat="server" >
</asp:sqldatasource> 
 <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline">
<ContentTemplate>

    <div class="gridMain " >
    <div style="padding:0;margin:0; overflow-y:scroll;width:700px;height:350px" runat="server" id="divGrid">    
        <table cellpadding="0" cellspacing="0" class="title"  style="width:670px">
       <tr>
        <th style="text-align:left;padding:5px; word-spacing:3px;color:Red; font-size:14px;background-color:darkblue" width="100%">
            附表參數設定                    
        </th>        
       </tr>
    </table> 
        <table style="width:500px" id="tbImage"> 
        <thead>
            <tr>    
                <th class="nonSpace" >書籤名稱</th>
                <th class="nonSpace" >參數名稱</th>                 
            </tr> 
        </thead>    
            <asp:Repeater runat="server" ID="rptEdit">                  
            <ItemTemplate>
                    <tr >      
                        <td><asp:TextBox ID="BOOK_MARK"  CssClass="display" Text='<%# Eval("BOOK_MARK") %>' runat="server" Width="300" ></asp:TextBox></td>  
                        <td>
                        <asp:DropDownList ID="PARA_CODE"  runat="server" Width="200" DataSourceID="sqlPARA_CODE" DataValueField="TypeCode" DataTextField="TypeDesc" OnPreRender='checkList' ToolTip='<%# Eval("PARA_CODE") %>'  >
                </asp:DropDownList>
                        </td>                          
                    </tr>
                </ItemTemplate> 
            </asp:Repeater>
          
        </table>
    </div>
</div>     
<div style="display:none">
<asp:TextBox runat="server" id="idWord"></asp:TextBox>  
<asp:Button runat="server" id="btnWord" OnClick="reloadWord" />
</div>
</ContentTemplate>
</asp:UpdatePanel>                           
</asp:Content>


<asp:Content ID="myPopWindow" ContentPlaceHolderID="PopWindow" runat="server">
     

     
<script language="javascript" type="text/javascript">

    function loadImage(strID) {
        if (strID.indexOf("CON_ATT_FILE_SEQ") == -1)
            return;
            
        document.getElementById("<%=this.idWord.ClientID %>").value=strID;
        document.getElementById("<%=this.btnWord.ClientID %>").click();
        window.setTimeout("window.parent.Loading();",1000);
    }
    
    function openDetail(strDate, strType, strName) {

    
        window.parent.openPopUpWindow();
    }
</script>
</asp:Content> 
