<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/displayUP.Master" CodeBehind="WE020.aspx.cs" Inherits="OrixMvc.WE020" %>
<%@ MasterType VirtualPath="~/Pattern/displayUP.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
  
<asp:Content ContentPlaceHolderID="searchArea" ID="mySearchArea" runat="server">
<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlPCUR_STS" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'APLYSTS'"
    runat="server" >
</asp:sqldatasource> 

   <table style="display:<%=this.bolQuery?"":"none" %>">
        <tr>
            <th>申請書編號：</th><td><asp:TextBox  runat="server" ID="APLY_NO" MaxLength="17" Width="140" ></asp:TextBox> </td>
            <th >連絡人：</th><td><asp:TextBox runat="server" ID="PCONTACT"  size="12"></asp:TextBox></td>
        </tr>                        
        <tr>
            <th>客戶代號：</th><td><asp:TextBox  runat="server" ID="PCUST_NO" MaxLength="20" Width="100" ></asp:TextBox> 
                 <asp:CheckBox runat="server" ID="PIS_SEARCH" Text="搜尋共同承租人" />
            </td>
            <th>客戶名稱：</th><td><asp:TextBox  runat="server" ID="PCUST_NAME" MaxLength="80" Width="300" ></asp:TextBox> </td>            
          
        </tr> 
        <tr>
            <th >經銷商代碼：</th><td><asp:TextBox  runat="server" ID="FRC_CODE" MaxLength="20" Width="100" ></asp:TextBox> </td>
            <th >經銷商名稱：</th><td><asp:TextBox runat="server" ID="FRC_NAME"  size="20"></asp:TextBox></td>   
        </tr>                        
        <tr>
            <th >連絡電話：</th><td ><asp:TextBox runat="server" ID="PCTAC_TEL"  size="25"></asp:TextBox></td>            
            <th >收件人：</th><td><asp:TextBox runat="server" ID="PRECV_NAME"  size="12"></asp:TextBox></td> 
        </tr>                        
        <tr>
            <th >存放地址：</th><td colspan="3" ><asp:TextBox runat="server" ID="PSEND_ADDR"  size="40"></asp:TextBox></td>                        
        </tr>                        
        <tr>
            <th >請款地址：</th><td colspan="3" ><asp:TextBox runat="server" ID="PADDR"  size="40"></asp:TextBox></td>                        
        </tr>                        
        <tr>
            <th >品名：</th><td colspan="3" ><asp:TextBox runat="server" ID="PPROD_NAME"  size="30"></asp:TextBox></td>
        </tr>                        
        <tr>
            <th >機號：</th><td ><asp:TextBox runat="server" ID="PMAC_NO"  size="10"></asp:TextBox></td>            
            <th >申請日期：</th><td colspan="2"><ocxControl:ocxDate runat="server" ID="PAPLY_DATE_S" />~<ocxControl:ocxDate runat="server" ID="PAPLY_DATE_E" /></td> 
        </tr>                        
        <tr>
            <th >目前狀況：</th><td ><asp:DropDownList runat="server" ID="PCUR_STS"  DataSourceID="sqlPCUR_STS"  DataValueField="TypeCode" DataTextField="TypeDesc" ></asp:DropDownList></td>            
            <th >業務員：</th><td><asp:TextBox runat="server" ID="PEMP_CODE"  size="10"></asp:TextBox></td> 
        </tr>                        
        <tr>
            <th >案件來源：</th><td><asp:TextBox runat="server" ID="PCASE_SOUR"  size="5"></asp:TextBox>
                <asp:CheckBox runat="server" ID="PIS_DEL" Text="含作廢駁回" />
            </td>
            <th >部門代號：</th><td colspan="2"><asp:TextBox runat="server" ID="PDEPT_CODE"  size="10"></asp:TextBox></td>
        </tr> 
        <tr>
            <th >發票號碼：</th><td colspan="3" ><asp:TextBox runat="server" ID="PINV_NO"  size="10"></asp:TextBox></td>                        
        </tr>            
    </table>   
                                   
</asp:Content>




<asp:Content ContentPlaceHolderID="editingArea" ID="myEditArea" runat="server" >
<script language="javascript" type="text/javascript">
function tab_select(intIndex,intLen)
{
    for (var i=1;i<=intLen;i++) 
    {
        document.getElementById("tab"+i.toString()).className="";
        document.getElementById("tabpanel"+i.toString()).style.display="none";
    }
    
    document.getElementById("tab"+intIndex.toString()).className="tabs-selected";
    document.getElementById("tabpanel"+intIndex.toString()).style.display="block";
    
}
</script>



<div  style="width: 800px; height: auto;display:<%=this.bolQuery?"none":"" %>">
	<div class="tabs-header" style="width: 798px;"><div class="tabs-scroller-left" style="display: none;"></div>
		<div class="tabs-scroller-right" style="display: none;"></div><div class="tabs-wrap" style="margin-left: 0px; margin-right: 0px; width: 798px;">
			<ul class="tabs">
				<li class="tabs-selected" id="tab1" onclick="tab_select(1,2)"><a href="javascript:void(0)" class="tabs-inner"><span class="tabs-title">申請書</span><span class="tabs-icon"></span></a></li>
				<li class=""  id="tab2" onclick="tab_select(2,2)"><a href="javascript:void(0)" class="tabs-inner"><span class="tabs-title">標的物</span><span class="tabs-icon"></span></a></li>
			</ul>
		</div>
	</div>
<div class="tabs-panels"  style="width: 800px;" >
	<div   class="panel" style="height: auto; width: 788px;display:block" id="tabpanel1">    
<div  class="gridMain ">
    <div style="padding:0;margin:0;overflow-x:scroll; overflow-y:scroll;width:770px;position:relative;height:360px" id="editGrid"  >

        <table cellpadding="0" cellspacing="0" style="width:95%" id="tbAPLY" >  
            <tr >
        <th style="text-align:left;padding-left:5px;border-right:none"  class="fixCol" >
            <asp:UpdatePanel runat="server" ID="upExcel" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
            <asp:Button runat="server" ID="A_excel" BackColor="Transparent" BorderStyle="None"  OnClick="toExcel" Width="20" Height="18" style="cursor:pointer;background-image:url(images/excel.png)" />       
            </ContentTemplate>
            </asp:UpdatePanel>                
        </th>
        </tr>        
            <tr id="topBorder">               
               
                <th class="fixCol">編輯</th>
                <th class="fixCol">申請書編號</th>
                <th>契約起日</th>
                <th>契約迄日</th>                
                <th>月數</th>
                <th>期數</th>
                <th>期租金</th>
                <th>狀況</th>
                <th>客戶代號</th>
                <th>客戶簡稱</th>
                <th>連絡人</th>
                <th>品名</th>
                <th>機號</th>
                <th>經銷商簡稱</th>
                <th>請款地址</th>
                <th>計張</th>
                <th>分開發票</th>
                <th>合併開立</th>
                <th>統一郵寄</th>
                <th>契約總額</th>
            </tr> 
            <asp:Repeater runat="server" ID="rptAPLY"  EnableViewState="false" > 
                <ItemTemplate>
                    <tr id='trA<%#Container.ItemIndex+1%>' class="<%#Container.ItemIndex%2==0?"srow":"" %>">          
                       
                        <td class="fixCol"><input type="button" id="btnAPLY" onclick="func_Click('<%# Eval("APLY_NO")%>','','trA<%#Container.ItemIndex+1%>')" style='display:<%# Eval("APLY_NO").ToString().Trim()==""?"none":""%>'  Class="button upd" value='選擇' />&nbsp</td>
                        <td class="fixCol"><%# Eval("APLY_NO").ToString()%>&nbsp</td>                         
                        <td><%# Eval("con_date_fr")%></td>
                        <td><%# Eval("con_date_to")%></td>                   
                        <td class="number"><%# Eval("APRV_DURN_M", "{0:###,###,###,##0}")%></td>                  
                        <td class="number"><%# Eval("APRV_PERD", "{0:###,###,###,##0}")%></td>                      
                        <td class="number"><%# Eval("APRV_HIRE", "{0:###,###,###,##0}")%></td>                        
                        <td><%# Eval("cursts")%></td>
                        <td><%# Eval("Cust_No")%></td>                   
                        <td><%# Eval("Cust_Sname")%></td>                   
                        <td><%# Eval("Contact")%></td>                        
                        <td><%# Eval("PROD_NAME")%></td>                                  
                        <td><%# Eval("MAC_NO")%></td>          
                        <td><%# Eval("FRC_SNAME")%></td>          
                        <td><%# Eval("REQ_PAY_ADDR")%></td>                   
                        <td><%# Eval("PAPER")%></td>                        
                        <td><%# Eval("DIVIDE")%></td>            
                        <td><%# Eval("MERGE_NO")%></td>                        
                        <td><%# Eval("MMail_NO")%></td>                        
                        <td class="number"><%# Eval("CUR_CON_AMT", "{0:###,###,###,##0}")%></td>                    
                    </tr>
                </ItemTemplate> 
            </asp:Repeater>            
        </table>
    </div>     
</div>
	</div>
	<div  class="panel" style="height: auto; width: 788px;display:none"  id="tabpanel2">    
	<div class="gridMain ">    
    <div style="padding:0;margin:0;overflow-x:scroll; overflow-y:scroll;width:770px;position:relative;height:360px" id="editGrid">
        <table cellpadding="0" cellspacing="0" style="width:95%" id="tbOBJ" >  
            <tr >
        <th style="text-align:left;padding-left:5px;border-right:none"  class="fixCol" >
            <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
            <asp:Button runat="server" ID="O_excel" BackColor="Transparent" BorderStyle="None"  OnClick="toExcel" Width="20" Height="18" style="cursor:pointer;background-image:url(images/excel.png)" />       
            </ContentTemplate>
            </asp:UpdatePanel>                
        </th>
        </tr>        
            <tr id="topBorder">                
                <th  class="seq fixCol"></th>
                <th  class="fixCol">編輯</th>
                <th  class="fixCol">申請書編號</th>
                <th>契約起始日期</th>
                <th>契約終止日期</th>                
                <th>月數</th>
                <th>期數</th>
                <th>期租金</th>
                <th>狀況</th>
                <th>客戶代號</th>
                <th>客戶簡稱</th>
                <th>連絡人</th>
                <th>序號</th>
                <th>品名</th>
                <th>機號</th>
                <th>經銷商簡稱</th>
                <th>規格</th>
                <th>市價</th>
                <th>殘值</th>
                <th>標的物狀態</th>
                <th>標的物代號</th>
                <th>所在地址</th>
                <th>所在電話</th>
                <th>計張</th>
                <th>分開發票</th>
                <th>MERGE_NO</th>                
            </tr>  
            <asp:Repeater runat="server" ID="rptOBJ"  EnableViewState="false"> 
                <ItemTemplate>
                    <tr id='trO<%#Container.ItemIndex+1%>' class="<%#Container.ItemIndex%2==0?"srow":"" %>">          
                       <td class="seq fixCol"><%# Container.ItemIndex + 1%></td>
                       <td class="fixCol">
                        <input type="button" id="btnObj" onclick="func_Click('<%# Eval("APLY_NO")%>','<%# Eval("OBJ_CODE")%>','trO<%#Container.ItemIndex+1%>')"  Class="button upd" value='選擇' />
                        </td>
                        <td class="fixCol"><%# Eval("APLY_NO")%></td>                         
                        <td><%# Eval("con_date_fr")%></td>
                        <td><%# Eval("con_date_to")%></td>                   
                        <td class="number"><%# Eval("APRV_DURN_M", "{0:###,###,###,##0}")%></td>                  
                        <td class="number"><%# Eval("APRV_PERD", "{0:###,###,###,##0}")%></td>                      
                        <td class="number"><%# Eval("APRV_HIRE", "{0:###,###,###,##0}")%></td>                        
                        <td><%# Eval("cursts")%></td>
                        <td><%# Eval("Cust_No")%></td>                   
                        <td><%# Eval("Cust_Sname")%></td>                         
                        <td><%# Eval("Contact")%></td>  
                        <td><%# Eval("Row")%></td>                   
                        <td><%# Eval("PROD_NAME")%></td>                        
                        <td><%# Eval("MAC_NO")%></td> 
                        <td><%# Eval("FRC_SNAME")%></td> 
                        <td><%# Eval("SPEC")%></td>
                        <td class="number"><%# Eval("Market_price", "{0:###,###,###,##0}")%></td>     
                        <td class="number"><%# Eval("RV_AMT", "{0:###,###,###,##0}")%></td>                           
                        <td><%# Eval("OBJ_STS")%></td>            
                        <td><%# Eval("OBJ_CODE")%></td>                        
                        <td><%# Eval("OBJ_LOC_ADDR")%></td>                        
                        <td><%# Eval("OBJ_LOC_TEL")%></td>                        
                        <td><%# Eval("PAPER")%></td>                        
                        <td><%# Eval("DIVIDE")%></td>            
                        <td><%# Eval("MERGE_NO")%></td>                               
                    </tr>
                </ItemTemplate> 
            </asp:Repeater>            
        </table>
    </div>     
</div> 
	</div>
</div>




<span style="display:none">
<asp:TextBox runat="server" ID="rowAPLY"></asp:TextBox>
<asp:TextBox runat="server" ID="rowOBJECT"></asp:TextBox>
</span>
<script language="javascript" type="text/javascript">

    var oldID = "";
    var oldCSS = "";
    function func_Click(aply,obj, trID) {

        if (oldID != "") {
            document.getElementById(oldID).className = oldCSS;
        }
        oldCSS = document.getElementById(trID).className;
        oldID = trID;
        
        document.getElementById(trID).className = "crow";
        document.getElementById("<%=this.rowAPLY.ClientID %>").value = aply;
        document.getElementById("<%=this.rowOBJECT.ClientID %>").value = obj;
        
        
    }
</script>
<asp:UpdatePanel runat="server" ID="upFunction" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>
    
<p >
    <div class="divFunction" >
        <asp:Button runat="server" ID="btnFunction1" CssClass="button trn" Text="客戶服務資料異動" OnCommand="Function_Click" CommandName="Function1"  />
    </div>
    <div class="divFunction">
        <asp:Button runat="server" ID="btnFunction2" CssClass="button trn" Text="票據明細維護" OnCommand="Function_Click" CommandName="Function2"  />
    </div>
    <div class="divFunction">
        <asp:Button runat="server" ID="btnFunction3" CssClass="button trn" Text="電話紀錄維護"  OnCommand="Function_Click" CommandName="Function3"  />
    </div>
    <div class="divFunction">
        <asp:Button runat="server" ID="btnFunction4" CssClass="button trn" Text="解約金試算"  OnCommand="Function_Click" CommandName="Function4"  />
    </div>   
    <div class="divFunction">
        <asp:Button runat="server" ID="btnFunction5" CssClass="button trn" Text="標的物查詢" OnCommand="Function_Click" CommandName="Function5"  />
    </div>      
    <div class="divFunction">
        <asp:Button runat="server" ID="btnFunction6" CssClass="button trn" Text="客戶歷史交易查詢" OnCommand="Function_Click" CommandName="Function6"  />
    </div>
    <div class="divFunction">
        <asp:Button runat="server" ID="btnFunction7" CssClass="button trn" Text="客戶付款記錄查詢" OnCommand="Function_Click" CommandName="Function7"  />
    </div>
    <div class="divFunction">
        <asp:Button runat="server" ID="btnFunction9" CssClass="button trn" Text="催收記錄查詢"  OnCommand="Function_Click" CommandName="Function9"  />
    </div>
    <div class="divFunction">
        <asp:Button runat="server" ID="btnFunction10" CssClass="button trn" Text="計張資訊查詢" OnCommand="Function_Click" CommandName="Function10"  />
    </div>
    <div class="divFunction">
        <asp:Button runat="server" ID="btnFunction12" CssClass="button trn" Text="申請書資料查詢"  OnCommand="Function_Click" CommandName="Function12"  />
    </div>
</p>   
    </ContentTemplate>
</asp:UpdatePanel>
</div>


<div class="divButton" style="float:right;display:<%=this.bolQuery?"none":"" %>">
    <asp:Button runat="server" ID="btnExit" CssClass="button exit" Text="返回" OnClick="Exit_Click" />
</div>                             



</asp:Content>