<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/empty.Master" CodeBehind="WA070.aspx.cs" Inherits="OrixMvc.WA070" %>
<%@ MasterType VirtualPath="~/Pattern/empty.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
        
<asp:Content ContentPlaceHolderID="editingArea" ID="mySearchArea" runat="server">

<style type="text/css">
    fieldset
    {
    	width:780px;
    	margin-left:15px;
    }
</style>
<div style="display:none">
<asp:Button runat="server" ID="btnQry" CssClass="button qry" Text="查詢" OnCommand="myCase_Change"  CommandName="Query"/>
</div>
<div class="searchArea">
    <table cellpadding="2" cellspacing="2" >
        <tr>
            <td>
                <table >
                    <tr>
                        <td><asp:CheckBox runat="server" ID="myCase" Checked="false" Text="只顯示我的案子" AutoPostBack="true" OnCheckedChanged="myCase_Change" /></td>                      
                    </tr>                        
                </table>                
            </td>           
        </tr>        
    </table>    
</div>                     

<fieldset>
<legend ><a href="javascript:showItem('1')"><b id="b1" style="color:darkblue; text-decoration:none">＋</b></a>額度申請（尚未簽核數：<b style="color:red"><%=this.rptEdit1.Items.Count %></b>）</legend>
<div id="query1" class="gridMain " style="display:none">
    <div style="padding:0;margin:0; overflow-y:scroll;width:750px;height:200px" id="editGrid">
        <table cellpadding="0" cellspacing="0"  style="width:740px" > 
        <thead>
            <tr> 
                <th style="width:40px" ></th>
                <th style="width:130px" >申請書編號</th>
                <th style="width:100px">總額度</th>
                <th style="width:100px">客戶代號</th>
                <th style="width:150px">客戶簡稱</th>                
                <th style="width:120px">授權名稱</th>
                <th style="width:100px">經辦</th>
            </tr> 
        </thead>    
            <asp:Repeater runat="server" ID="rptEdit1"> 
                <ItemTemplate>
                    <tr > 
                        <td style="width:40px"><asp:Button runat="server" ID="appove1" OnCommand="appove_Click" CssClass="button upd" Text="審核" CommandName='<%#Eval("QUOTA_APLY_NO")+","+Eval("CUR_STS").ToString().Trim()+","+Eval("CUR_STS_NAME").ToString().Trim() %>' /> </td>    
                        <td style="width:130px"><%#Eval("QUOTA_APLY_NO")%></td>
                        <td style="width:100px"><%#Eval("APLY_TOT_QUOTA")%></td>
                        <td style="width:100px"><%#Eval("CUST_NO")%></td>
                        <td style="width:150px"><%#Eval("CUST_SNAME")%></td>
                        <td  style="width:120px"><%#Eval("AUD_NAME")%></td>
                        <td style="width:100px"><%#Eval("EMP_NAME")%></td>                        
                    </tr>
                </ItemTemplate> 
            </asp:Repeater>        
        </table>
    </div>     
</div> 
</fieldset>
<br /><br />
<fieldset>
<legend><a href="javascript:showItem('2')"><b id="b2" style="color:darkblue; text-decoration:none">＋</b></a>一般案件申請（尚未簽核數：<b style="color:red"><%=this.rptEdit2.Items.Count %></b>)</legend>    
<div id="query2"  class="gridMain " style="display:none">
    <div style="padding:0;margin:0; overflow-x:scroll;width:750px;height:200px"  id="editGrid">
        <table cellpadding="0" cellspacing="0" style="width:740px" > 
        <thead>
            <tr>             
                <th style="width:40px" ></th>
                <th style="width:150px" >申請書編號</th>
                <th style="width:100px">客戶代號</th>
                <th style="width:200px">客戶簡稱</th>                
                <th style="width:120px">授權名稱</th>
                <th style="width:120px">經辦</th>
            </tr>          
       </thead>     
            <asp:Repeater runat="server" ID="rptEdit2"> 
                <ItemTemplate>
                    <tr >    
                        <td style="width:40px"><asp:Button runat="server" ID="appove2" OnCommand="appove_Click" CssClass="button upd" Text="審核" CommandName='<%#Eval("APLY_NO").ToString().Trim()+","+Eval("CUR_STS").ToString().Trim()+","+Eval("CUR_STS_NAME").ToString().Trim() %>' /></td>    
                        <td style="width:150px" ><%#Eval("APLY_NO") %></td>
                        <td style="width:100px"><%#Eval("CUST_NO")%></td>
                        <td style="width:200px"><%#Eval("CUST_SNAME")%></td>
                        <td style="width:120px"><%#Eval("AUD_NAME")%></td>
                        <td style="width:120px"><%#Eval("EMP_NAME")%></td>
                    </tr>
                </ItemTemplate> 
            </asp:Repeater>        
        </table>
    </div>     
</div>     
</fieldset>
<br /><br />


<fieldset>
<legend><a href="javascript:showItem('3')"><b id="b3" style="color:darkblue; text-decoration:none">＋</b></a>主約申請（尚未簽核數：<b style="color:red"><%=this.rptEdit3.Items.Count %></b>）</legend>                    
<div id="query3" class="gridMain " style="display:none">
    <div style="padding:0;margin:0; overflow-y:scroll;width:750px;height:200px" id="editGrid">
        <table cellpadding="0" cellspacing="0"   style="width:740px" > 
        <thead>
            <tr> 
                <th style="width:40px" ></th>
                 <th style="width:130px" >申請書編號</th>
                <th style="width:100px">客戶代號</th>
                <th style="width:200px">客戶簡稱</th>   
                <th style="width:120px">經辦</th>
            </tr> 
        </thead>
            <asp:Repeater runat="server" ID="rptEdit3"> 
                <ItemTemplate>
                    <tr >   
                        <td style="width:100px">
                        <asp:Button runat="server" ID="appove3" OnCommand="appove_Click" CssClass="button upd" Text="核淮" CommandName='<%#Eval("MAST_CON_NO") %>' />
                        <asp:Button runat="server" ID="appove31" CssClass="button del" Text="作廢"  OnCommand="appove_Click"  CommandName='<%#Eval("MAST_CON_NO") %>'  Visible='<%# Eval("CUR_STS").ToString().Trim()=="1" || Eval("CUR_STS").ToString().Trim()=="2" ?true:false %>' />
                        </td> 
                        <td  style="width:110px"><%#Eval("MAST_CON_NO")%></td>
                        <td style="width:100px"><%#Eval("CUST_NO")%></td>
                        <td style="width:180px"><%#Eval("CUST_SNAME")%></td>
                        <td style="width:100px"><%#Eval("EMP_NAME")%></td>
                    </tr>
                </ItemTemplate> 
            </asp:Repeater>        
        </table>
    </div>     
</div>  
</fieldset>
<br /><br />

<fieldset>
<legend><a href="javascript:showItem('4')"><b id="b4" style="color:darkblue; text-decoration:none">＋</b></a>先行出合約申請（尚未簽核數：<b style="color:red"><%=this.rptEdit4.Items.Count %></b>）</legend>     
<div id="query4"  class="gridMain " style="display:none">
    <div style="padding:0;margin:0; overflow-y:scroll;width:750px;height:200px" id="editGrid">
        <table cellpadding="0" cellspacing="0"   style="width:740px" > 
        <thead>
            <tr> 
                <th style="width:40px" ></th>
                <th style="width:130px" >申請書編號</th>
                <th style="width:100px">客戶代號</th>
                <th style="width:200px">客戶簡稱</th> 
                <th style="width:120px">經辦</th>
            </tr>  
        </thead>
            <asp:Repeater runat="server" ID="rptEdit4"> 
                <ItemTemplate>
                    <tr >    
                        <td style="width:100px"><asp:Button runat="server" ID="appove4" OnCommand="appove_Click" Text="核淮" CssClass="button upd" CommandName='<%#Eval("APLY_NO").ToString().Trim()+","+Eval("FAST_STS").ToString().Trim()+","+Eval("FAST_STS_NAME").ToString().Trim() %>' />
                        <asp:Button runat="server" ID="appove41" CssClass="button del" Text="作廢"  OnCommand="appove_Click"  CommandName='<%#Eval("APLY_NO").ToString().Trim()+","+Eval("FAST_STS").ToString().Trim()+","+Eval("FAST_STS_NAME").ToString().Trim() %>'  Visible='<%# Eval("CUR_STS_CODE").ToString().Trim()=="1" || Eval("CUR_STS_CODE").ToString().Trim()=="2" ?true:false %>' />
                        </td> 
                        <td style="width:110px"><%#Eval("APLY_NO") %></td>
                        <td style="width:100px"><%#Eval("CUST_NO") %></td>
                        <td style="width:180px"><%#Eval("CUST_SNAME")%></td>
                        <td style="width:100px"><%#Eval("EMP_NAME")%></td>
                    </tr>
                </ItemTemplate> 
            </asp:Repeater>        
        </table>
    </div>     
</div>  
</fieldset>
<br /><br />

<fieldset>
<legend><a href="javascript:showItem('5')"><b id="b5" style="color:darkblue; text-decoration:none">＋</b></a>供應商審核（尚未簽核數：<b style="color:red"><%=this.rptEdit5.Items.Count %></b>）</legend>     
<div id="query5"  class="gridMain " style="display:none">
    <div style="padding:0;margin:0; overflow-y:scroll;width:750px;height:200px"  id="editGrid">
        <table cellpadding="0" cellspacing="0"   style="width:740px" >
            <tr> 
                <th style="width:40px" ></th>
                <th style="width:90px" >經銷商代號</th>
                <th style="width:160px">經銷商簡稱</th>
                <th style="width:90px">集團代號</th>                
                <th style="width:160px">集團簡稱</th>
                <th style="width:100px">經辦</th>
            </tr>  
            <asp:Repeater runat="server" ID="rptEdit5"> 
                <ItemTemplate>
                    <tr >    
                        <td style="width:40px"><asp:Button runat="server" ID="appove5" OnCommand="appove_Click" Text="審核" CssClass="button upd" CommandName='<%#Eval("FRC_CODE") %>' /></td> 
                        <td style="width:90px" ><%#Eval("FRC_CODE")%></td>
                        <td style="width:160px"><%#Eval("FRC_NAME")%></td>
                        <td style="width:90px"><%#Eval("BLOC_NO")%></td>
                        <td style="width:160px"><%#Eval("BLOC_NAME")%></td>
                        <td style="width:100px"><%#Eval("EMP_NAME")%></td>
                    </tr>
                </ItemTemplate> 
            </asp:Repeater>        
        </table>
    </div>     
</div>  
</fieldset>
<br /><br />



<script language="javascript">
    function showItem(strType){
        if (document.getElementById("b"+strType).innerHTML=="＋")
        {
            document.getElementById("b"+strType).innerHTML="—"
            document.getElementById("query"+strType).style.display="";
        }
        else{
            document.getElementById("b"+strType).innerHTML="＋"
            document.getElementById("query"+strType).style.display="none";
        }
    }
</script>       
</asp:Content>

