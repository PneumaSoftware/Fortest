<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/query.Master" CodeBehind="WC020.aspx.cs" Inherits="OrixMvc.WC020" %>
<%@ MasterType VirtualPath="~/Pattern/query.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
    
<asp:Content ContentPlaceHolderID="searchArea" ID="mySearchArea" runat="server">
<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlOR3_FRC_DEP" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="select FRC_DEP_CODE,FRC_DEP_NAME from OR3_FRC_DEP where FRC_CODE=@FRC_CODE "
    runat="server" >
   <SelectParameters>
    <asp:ControlParameter ControlID="FRC_CODE" PropertyName="Text" Name="FRC_CODE" />
   </SelectParameters>
</asp:sqldatasource>



    
    <table >
        <tr>
            <th>供應商代號：</th><td><ocxControl:ocxDialog runat="server" ID="FRC_CODE" width="100" ControlID="FRC_SNAME" FieldName="FRC_SNAME" SourceName="OR_FRC" /></td>
            <td colspan="2"><asp:TextBox runat="server" ID="FRC_SNAME" CssClass="display"  size="20"></asp:TextBox></td>                        
            <th>營業單位：</th><td>
            <asp:UpdatePanel runat="server" ID="upDEPT" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>
            
            <asp:DropDownList ID="FRC_DEPT_CODE"  runat="server"  onfocusin="dialogChange()"    Width="150"  ></asp:DropDownList>    
                </ContentTemplate>
                <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnDEPT"  />            
                </Triggers>
            </asp:UpdatePanel>
            <span style="display:none">
            <asp:Button runat="server" ID="btnDEPT" OnClick="FrcDept_Click"  />
            </span>
            </td>
                            
        </tr> 
        <tr>
            <th>業務員姓名：</th><td><asp:TextBox runat="server" ID="FRC_SALES_NAME" width="100"></asp:TextBox></td>       
            <th>分機：</th><td><asp:TextBox runat="server" ID="EXT" size="20"></asp:TextBox></td>                       
            <th>手機：</th><td><asp:TextBox runat="server" ID="MOBILE" size="20"></asp:TextBox></td>                        
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

<asp:Content ContentPlaceHolderID="queryHead" ID="myQueryHead" runat="server">
    <!--QueryHead!-->    
    <th class="fixCol">供應商代號</th>
    <th class="fixCol">供應商簡稱</th>
    <th class="fixCol">營業單位</th>
    <th class="fixCol">簡稱</th>  
    <th>姓名</th>    
    <th>職稱</th>    
    <th>電話</th>      
    <th>分機</th>      
    <th>專線</th>      
    <th>傳真</th> 
    <th>手機</th>      
    <th>手機2</th>      
    <th>EMail</th>      
    <th>地址</th>      
</asp:Content>
<asp:Content ContentPlaceHolderID="queryBody" ID="myQueryBody" runat="server">
    <td class="fixCol"><%# Eval("FRC_CODE")%><asp:HiddenField runat="server" ID="hiddenSALES" Value='<%# Eval("FRC_CODE").ToString().Trim()+","+Eval("FRC_DEP_CODE").ToString().Trim()+","+Eval("SEQ_NO").ToString().Trim()%>' /> </td>
    <td class="fixCol"><%# Eval("FRC_SNAME")%></td>
    <td class="fixCol"><%# Eval("FRC_DEP_CODE")%></td>
    <td class="fixCol"><%# Eval("FRC_DEP_SNAME")%></td>
    <td><%# Eval("FRC_SALES_NAME")%></td>
    <td><%# Eval("JOB_NAME")%></td>
    <td><%# Eval("TEL")%></td>    
    <td><%# Eval("EXT")%></td>
    <td><%# Eval("LINE")%></td>
    <td><%# Eval("FACSIMILE")%></td>
    <td><%# Eval("MOBILE")%></td>
    <td><%# Eval("CELL2")%></td>
    <td><%# Eval("EMAIL")%></td>
    <td><%# Eval("ADDRESS")%></td>
</asp:Content>
