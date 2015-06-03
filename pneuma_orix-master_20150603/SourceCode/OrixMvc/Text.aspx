<%@ Page Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/editing.Master" CodeBehind="Text.aspx.cs" Inherits="OrixMvc.Text" %>
<%@ MasterType VirtualPath="~/Pattern/editing.Master" %>

<asp:Content ContentPlaceHolderID="editingArea" ID="mySearchArea" runat="server">
<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlINTERVIEW_TOPIC" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="select * from OR3_COND_DEF WHERE TypeField = 'INTERVIEW_TOPIC' And (End_date is null or End_date > GETDATE()) Union select * from OR3_COND_DEF WHERE TypeField = 'INTERVIEW_TOPIC' AND TypeCode = '@TypeCode'"
    runat="server" >
    <SelectParameters>
       <asp:Parameter Name="TypeCode" Type="String" />
   </SelectParameters>
</asp:sqldatasource>

    <form id="form1" runat="server">
    <div>
        <asp:DropDownList ID="INTERVIEW_TOPIC"  runat="server" Width="94"     
         DataSourceID="sqlINTERVIEW_TOPIC" DataValueField="TypeCode" DataTextField="TypeDesc" >
        </asp:DropDownList> 
    </div>
    </form>             
</asp:Content>
