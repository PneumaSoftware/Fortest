<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/editing.Master" CodeBehind="WC0201.aspx.cs" Inherits="OrixMvc.WC0201" %>
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
    id="sqlOR3_FRC_DEP" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="select FRC_DEP_CODE,FRC_DEP_NAME from OR3_FRC_DEP where FRC_CODE=(case when @FRC_CODE!='xxx' then @FRC_CODE else FRC_CODE end) "
    runat="server" >
   <SelectParameters>
    <asp:ControlParameter ControlID="FRC_CODE" PropertyName="Text" Name="FRC_CODE" DefaultValue='xxx'  />
   </SelectParameters>
</asp:sqldatasource>

<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlJOB_NAME" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'JOB_NAME' "
    runat="server" >
</asp:sqldatasource> 


    
    <span style="display:none">
    <asp:TextBox runat="server" ID="SEQ_NO" Text='<%# Eval("SEQ_NO") %>' ></asp:TextBox>
    </span>
     <table style="margin-top:15px">
        <tr>
             <th class="nonSpace">供應商代號：</th><td><ocxControl:ocxDialog runat="server" ID="FRC_CODE" width="100" Text='<%# Eval("FRC_CODE") %>' ControlID="FRC_SNAME" FieldName="FRC_SNAME" SourceName="OR_FRC" /></td>
            <td colspan="2"><asp:TextBox runat="server" ID="FRC_SNAME" CssClass="display"  size="20" Text='<%# Eval("FRC_SNAME") %>'></asp:TextBox></td>                        
            <th  class="nonSpace">營業單位：</th><td>
            <asp:HiddenField runat="server" Value='<%# Eval("FRC_DEP_CODE") %>' ID="hidden_FRC_DEP_CODE" />
            
            
            <asp:UpdatePanel runat="server" ID="upDEPT" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>
            
                <asp:DropDownList ID="FRC_DEPT_CODE"  runat="server" AutoPostBack=true  OnSelectedIndexChanged="FRC_DEP_CODE_changed"    Width="150" DataSourceID="sqlOR3_FRC_DEP" OnPreRender='checkList' ToolTip='<%# Eval("FRC_DEP_CODE") %>' DataValueField="FRC_DEP_CODE" DataTextField="FRC_DEP_NAME" ></asp:DropDownList>    
            <span style="display:none">
            <asp:Button runat="server" ID="btnDEPT" OnClick="FrcDept_Click"  />
            </span>
            
            <asp:Button runat="server" ID="btnMaintain" cssClass="button func" Text="營業單位維護" OnCommand="GridFunc_Click"  />
            </ContentTemplate>
                <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnDEPT" />
                </Triggers>
            </asp:UpdatePanel>
            </td>
       </tr>
       <tr>
            <th class="nonSpace">姓名：</th>
            <td>
                <asp:TextBox runat="server" ID="FRC_SALES_NAME" Text='<%# Eval("FRC_SALES_NAME") %>'  Width="100"  MaxLength="50" ></asp:TextBox>    
            </td>
            <th>職稱：</th>
            <td>
            <asp:DropDownList ID="JOB_NAME"  runat="server"    Width="150" DataSourceID="sqlJOB_NAME" OnPreRender='checkList' ToolTip='<%# Eval("JOB_NAME") %>' DataValueField="TypeCode" DataTextField="TypeDesc" ></asp:DropDownList>    
            </td>
       </tr>
        <tr>
            <th>電話：</th>
            <td ><asp:TextBox runat="server" ID="TEL" Text='<%# Eval("TEL") %>'  Width="120"  CssClass="display" ReadOnly="true"  ></asp:TextBox></td>
            <th>分機：</th>
            <td ><asp:TextBox runat="server" ID="EXT" Text='<%# Eval("EXT") %>'  Width="60"   MaxLength=10  ></asp:TextBox></td>
            <th>地址：</th>
            <td ><asp:TextBox runat="server" ID="ADDRESS" Text='<%# Eval("ADDRESS") %>'  Width="300"  CssClass="display" ReadOnly="true"  ></asp:TextBox></td>
       </tr> 
        <tr>
            <th>專線：</th>
            <td ><asp:TextBox runat="server" ID="LINE" Text='<%# Eval("LINE") %>'  Width="120"   ></asp:TextBox></td>
            <th>傳真：</th>
            <td ><asp:TextBox runat="server" ID="FACSIMILE" Text='<%# Eval("FACSIMILE") %>'  Width="120"  ></asp:TextBox></td>
        </tr>   
        <tr>
            <th>手機：</th>
            <td ><asp:TextBox runat="server" ID="MOBILE" Text='<%# Eval("MOBILE") %>'  Width="120"   ></asp:TextBox></td>
            <th>手機2：</th>
            <td ><asp:TextBox runat="server" ID="CELL2" Text='<%# Eval("CELL2") %>'  Width="120"  ></asp:TextBox></td>
        </tr>   
        <tr>
            <th>EMail：</th>
            <td colspan="3" ><asp:TextBox runat="server" ID="EMAIL" Text='<%# Eval("EMAIL") %>'  Width="250"   ></asp:TextBox></td>
        </tr> 
        <tr>
            <th>備註：</th>
            <td colspan="3" ><asp:TextBox runat="server" ID="REMARK" Text='<%# Eval("REMARK") %>'  Width="300"   ></asp:TextBox></td>
        </tr> 
    </table>
    
          
          
  
    <script language="javascript" type="text/javascript">

   
    
    var FRC_CODE = "";

    function dialogChange(sid, val)
    {
        if (FRC_CODE != val && val != "") {
            FRC_CODE = val;
            document.getElementById("<%=this.btnDEPT.ClientID %>").click();
        }
    }
    
 
  
 
</script>  
                           
</asp:Content>


<asp:Content ID="myPopWindow" ContentPlaceHolderID="PopWindow" runat="server">
     <asp:UpdatePanel runat="server" ID="upDetailEditing" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
        
       
       <div style="width:100%;height:19px;padding:4px;font-size:13px; font-weight:bold; background-color:#150643;color:White;font-family: Verdana, Tahoma, Arial, Helvetica Neue , Helvetica, Sans-Serif;">營業單位維護</div>
       <div class="editingArea">
        <div id="query" class="gridMain ">
            <div id="divGrid" style="padding:0;margin:0; position:relative; overflow-y:scroll;width:680px;height:300px" runat="server">
            <table cellpadding="1" cellspacing="0"  style="width:500px" > 
            <thead>
                <tr>      
                    <th>編輯</th>          
                    <th class="nonSpace">代號</th>
                    <th class="nonSpace">名稱</th>
                    <th class="nonSpace">簡稱</th>
                    <th>電話</th>
                    <th>地址</th>
                </tr> 
            </thead>    
                <asp:Repeater runat="server" ID="rptEdit"  > 
                    <ItemTemplate>
                        <tr style="display:<%# Eval("Status").ToString()=="D"?"none":"" %>" > 
                            <td>                            
                            <asp:Button ID="btnDel" runat="server"  cssClass="button del"  Text="刪除"  OnCommand="GridFunc_Click"   CommandName='<%# Eval("FRC_DEP_CODE") %>' />                            
                            <asp:HiddenField runat="server" ID="Status"  Value='<%# Eval("Status") %>' /></td>
                            <td><asp:TextBox runat="server" ID="FRC_DEP_CODE" Text='<%# Eval("FRC_DEP_CODE") %>' MaxLength="20" Width="100"  ></asp:TextBox></td>
                            <td><asp:TextBox runat="server" ID="FRC_DEP_NAME" Text='<%# Eval("FRC_DEP_NAME") %>' MaxLength="20" Width="180"  ></asp:TextBox></td>
                            <td><asp:TextBox runat="server" ID="FRC_DEP_SNAME" Text='<%# Eval("FRC_DEP_SNAME") %>' MaxLength="20" Width="120"  ></asp:TextBox></td>
                            <td><asp:TextBox runat="server" ID="TEL" Text='<%# Eval("TEL") %>' MaxLength="20" Width="120"  ></asp:TextBox></td>
                            <td><asp:TextBox runat="server" ID="ADDRESS" Text='<%# Eval("ADDRESS") %>' MaxLength="20" Width="250"  ></asp:TextBox></td>
                        </tr>
                    </ItemTemplate> 
                </asp:Repeater>                       
                <tr >
                    <td><asp:Button ID="btnAdd" runat="server"  cssClass="button dadd"  Text="新增" OnCommand="GridFunc_Click" CommandName="Add" /> </td>
                    <td><asp:TextBox runat="server" ID="addFRC_DEP_CODE"  MaxLength="20" Width="100"  ></asp:TextBox></td>
                    <td><asp:TextBox runat="server" ID="addFRC_DEP_NAME"  MaxLength="20" Width="180"  ></asp:TextBox></td>
                    <td><asp:TextBox runat="server" ID="addFRC_DEP_SNAME" MaxLength="20" Width="120"  ></asp:TextBox></td>
                    <td><asp:TextBox runat="server" ID="addTEL"  MaxLength="20" Width="120"  ></asp:TextBox></td>
                    <td><asp:TextBox runat="server" ID="addADDRESS"  MaxLength="50" Width="250"  ></asp:TextBox></td>                    
                </tr>
            </table>
            </div>     
        </div>
    </div>  
     <div class="divButton" style='float:right;'>
                        <asp:Button runat="server" ID="btnEdit" CssClass="button save" Text="儲存" OnCommand="GridFunc_Click" CommandName="Save"   />
                    </div> 
                    
    </ContentTemplate>    
    </asp:UpdatePanel>

     
<script language="javascript" type="text/javascript">
    function openDetail() {


        var obj = document.getElementById("divPopWindow");
        obj.style.width = "700px";
        obj.style.height = "500px";
       

        obj.style.top = "30px";
        obj.style.left = "30px";

        window.parent.openPopUpWindow();
    }
    
    window.parent.closePopUpWindow();
    
</script>
</asp:Content> 
