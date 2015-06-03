<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/editing.Master" CodeBehind="WC0101.aspx.cs" Inherits="OrixMvc.WC0101" %>
<%@ MasterType VirtualPath="~/Pattern/editing.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxYM"  TagPrefix="ocxControl" Src="~/ocxControl/ocxYM.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxTime"  TagPrefix="ocxControl" Src="~/ocxControl/ocxTime.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
        
<asp:Content ContentPlaceHolderID="editingArea" ID="mySearchArea" runat="server">
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

<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlINDUSTRY_TYPE" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'INDUSTRY_TYPE' "
    runat="server" >
</asp:sqldatasource>

<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlCOUNTRY_CODE" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'COUNTRY_CODE' "
    runat="server" >
</asp:sqldatasource>

<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlBUY_WAY" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'BUY_WAY' "
    runat="server" >
</asp:sqldatasource>

<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlFRC_VER" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="select VER,STS=FRC_STS,FRC_STS=(case FRC_STS when '0' then '待確認' else '已確認'  end),Eff_Date=dbo.f_DateAddSlash(Eff_Date),Exp_Date=dbo.f_DateAddSlash(Exp_Date),QUOTA_AMT,ttlCount=isnull((select count(*) from OR3_FRC_VER where FRC_CODE=@FRC_CODE),0) ,sureCount=isnull((select count(*) from OR3_FRC_VER where FRC_CODE=@FRC_CODE and FRC_STS='1'),0) from OR3_FRC_VER where FRC_CODE=@FRC_CODE"
    runat="server" >
    <SelectParameters>
        <asp:ControlParameter ControlID="FRC_CODE" Name="FRC_CODE" PropertyName="Text" />
    </SelectParameters>
</asp:sqldatasource>

<div  style="width: 798px; height: auto;">
		<div class="tabs-header" style="width: 780px;"><div class="tabs-scroller-left" style="display: none;"></div>
		<div class="tabs-scroller-right" style="display: none;"></div><div class="tabs-wrap" style="margin-left: 0px; margin-right: 0px; width: 780px;">
			<ul class="tabs">
				<li class="tabs-selected" id="tab1" onclick="tab_select(1,3)"><a href="javascript:void(0)" class="tabs-inner"><span class="tabs-title">基本資料</span><span class="tabs-icon"></span></a></li>
				<li class=""  id="tab2" onclick="tab_select(2,3)"><a href="javascript:void(0)" class="tabs-inner"><span class="tabs-title">經辦意見</span><span class="tabs-icon"></span></a></li>
				<li class=""  id="tab3" onclick="tab_select(3,3)"><a href="javascript:void(0)" class="tabs-inner"><span class="tabs-title">標的物</span><span class="tabs-icon"></span></a></li>
			</ul>
		</div>
	</div>
<div class="tabs-panels"  style="width: 798px;" >	
	<div class="panel"  style="height: auto; width: 770px;display:block" id="tabpanel1">   
        <table>
            <tr>            
            <th class="nonSpace">供應商代號：</th>
            <td><asp:TextBox runat="server" ID="FRC_CODE"  size="10"  MaxLength="10" Text='<%# Eval("FRC_CODE") %>'></asp:TextBox></td>
            <th>銀行代號：</th>
            <td><ocxControl:ocxDialog runat="server" ID="BANK_CODE" Value='<%# Eval("BANK_CODE") %>' width="80" ControlID="BANK_NAME" FieldName="BANK_NAME" SourceName="ACC18" /> </td>
            <td><asp:TextBox runat="server" CssClass="display"   ID="BANK_NAME" Text='<%# Eval("BANK_NAME") %>' size="10"></asp:TextBox></td>            
            <th>匯款帳號：</th>
            <td><asp:TextBox runat="server"  ID="BANK_ACCT" Text='<%# Eval("BANK_ACCT") %>' size="15"  MaxLength="15" ></asp:TextBox></td>
        </tr>  
        <tr>
            <th class="nonSpace">供應商名稱：</th>
            <td colspan="3"><asp:TextBox runat="server" ID="FRC_NAME" Text='<%# Eval("FRC_NAME") %>' MaxLength="80"  Width="270"></asp:TextBox></td>         
            <th class="nonSpace">供應商簡稱：</th>
            <td colspan="2" ><asp:TextBox runat="server" ID="FRC_SNAME" Text='<%# Eval("FRC_SNAME") %>' MaxLength="50"  size="30"></asp:TextBox></td> 
        </tr>
        <tr>
            <th>集團代號：</th>
            <td><ocxControl:ocxDialog runat="server" ID="BLOC_NO" Text='<%# Eval("BLOC_NO") %>' width="90" ControlID="BLOC_SNAME" FieldName="BLOC_SNAME" SourceName="OR_BLOC" /> </td>
            <td><asp:TextBox runat="server" CssClass="display"   ID="BLOC_SNAME" Text='<%# Eval("BLOC_SNAME") %>' size="10"></asp:TextBox></td>            
        </tr>
         <tr>            
            <th class="nonSpace">聯絡人：</th>
            <td ><asp:TextBox runat="server" ID="CONTACT"  size="10"  MaxLength="12" Text='<%# Eval("CONTACT") %>'></asp:TextBox></td>
            <th>統一編號：</th>
            <td><asp:TextBox runat="server" onblur="chkGUINo(this);" ID="UNIF_NO" Text='<%# Eval("UNIF_NO") %>' size="10"  MaxLength="10" ></asp:TextBox></td>
            <th>負責人：</th>
            <td colspan="3"><asp:TextBox runat="server" ID="TAKER"  size="20"  MaxLength="20" Text='<%# Eval("TAKER") %>'></asp:TextBox></td>
         </tr> 
         <tr>            
            <th>電話一：</th>
            <td ><asp:TextBox runat="server" ID="PHONE1"  size="11"  MaxLength="20" Text='<%# Eval("PHONE1") %>'></asp:TextBox></td>
            <th>電話二：</th>
            <td><asp:TextBox runat="server" ID="PHONE2" size="12"  MaxLength="20" Text='<%# Eval("PHONE2") %>' ></asp:TextBox></td>             
            <th>傳真：</th>
            <td><asp:TextBox runat="server" ID="FACSIMILE"   size="12"  MaxLength="20"  Text='<%# Eval("FACSIMILE") %>'  ></asp:TextBox></td>            
        </tr> 
        <tr>            
            <th>營業登記地址：</th>
            <td colspan="5" ><asp:TextBox runat="server" ID="SALES_RGT_ADDR"  size="50"  MaxLength="80" Width="380" Text='<%# Eval("SALES_RGT_ADDR") %>'></asp:TextBox></td>
        </tr> 
        <tr>            
            <th>連絡地址：</th>
            <td colspan="5" ><asp:TextBox runat="server" ID="CTAC_ADDR"  size="50" MaxLength="80"  Width="380" Text='<%# Eval("CTAC_ADDR") %>'></asp:TextBox></td>
        </tr> 
        <tr>
            <th>員工人數：</th>
            <td><ocxControl:ocxNumber runat="server" ID="EMP_PSNS" MASK="999,999" Text='<%# Eval("EMP_PSNS") %>' /></td>            
            <th>設立日期：</th>
            <td colspan="2" ><ocxControl:ocxDate runat="server" ID="BUILD_DATE" Text='<%# Eval("BUILD_DATE") %>' /></td> 
            <th>登記資本額(萬元)：</th>
            <td><ocxControl:ocxNumber runat="server" ID="RGT_CAPT_AMT" MASK="999,999,999" Text='<%# Eval("RGT_CAPT_AMT") %>' /></td>            
        </tr>   
         <tr>
            <th>E-Mail Addr：</th>
            <td colspan="4" ><asp:TextBox runat="server" ID="CUST_EMAIL_ADDR"  size="50"   Width="250" Text='<%# Eval("CUST_EMAIL_ADDR") %>'></asp:TextBox></td>
            <th>實收資本額(萬元)：</th>
            <td><ocxControl:ocxNumber runat="server" ID="REAL_CAPT_AMT" MASK="999,999,999" Text='<%# Eval("REAL_CAPT_AMT") %>' /></td>            
        </tr> 
         <tr>
            <th >營業額年月：</th>
            <td colspan="2">
                <ocxControl:ocxYM runat="server" ID="TURNOVER_YM_S" Text='<%# Eval("TURNOVER_YM_S") %>' />~
                <ocxControl:ocxYM runat="server" ID="TURNOVER_YM_E" Text='<%# Eval("TURNOVER_YM_E") %>' />            
            </td>
            <th>營業額(萬元)：</th>
            <td><ocxControl:ocxNumber runat="server" ID="TURNOVER" MASK="999,999,999" Text='<%# Eval("TURNOVER") %>' /></td>            
        </tr> 
        <tr>
            <th>配合標的：</th>
            <td colspan="7" ><asp:TextBox  runat="server" ID="COOR_SUBJECT"  size="50"  MaxLength="50" Width="380" Text='<%# Eval("COOR_SUBJECT") %>'></asp:TextBox></td>
        </tr>
        <tr>
            <th>主力商品說明：</th>
            <td colspan="2" ><asp:TextBox runat="server" ID="MAIN_PROD_DESC"    MaxLength="50" Width="170" Text='<%# Eval("MAIN_PROD_DESC") %>'></asp:TextBox></td>
            <th colspan="2">供應商背景及資力：</th>
            <td colspan="3" ><asp:TextBox  runat="server" ID="FRC_BACKGROUND"    MaxLength="50" Width="200" Text='<%# Eval("FRC_BACKGROUND") %>'></asp:TextBox></td>
        </tr>
        </table>
    </div>
    <div class="panel"  style="height: auto; width: 770px;display:none" id="tabpanel2">  
        <table >
            <tr>
                <th>經辦：</th>
                <td style="width:110px"><ocxControl:ocxDialog runat="server" ID="EMP_CODE" Text='<%# Eval("CorpAcct") %>'  width="100" ControlID="EMP_NAME" FieldName="EMP_NAME" SourceName="OR_EMP" /></td>
                <td style="width:150px"><asp:TextBox  runat="server" ID="EMP_NAME"  size="10"  MaxLength="10"  CssClass="display"  Text='<%# Eval("EMP_NAME") %>'></asp:TextBox></td>
                <td style="width:330px"> </td>
            </tr>
            <tr><th>經辦意見：</th><td colspan="3"><asp:TextBox runat="server" ID="HAND_OPINION" Rows="25"  Width="500" Text='<%# Eval("HAND_OPINION") %>' TextMode="MultiLine"></asp:TextBox></td></tr>
        </table>
    </div>
    <div class="panel"  style="height: auto; width: 770px;display:none" id="tabpanel3">  
        <asp:UpdatePanel ID="upDetailQuery" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
            <ContentTemplate>

            <div id="query" class="gridMain ">
                <div style="padding:0;margin:0; overflow-y:scroll;width:730px;height:175px" runat="server" id="divGrid">
                    <table cellpadding="0" cellspacing="0"  id="tbImage" style="width:710px" > 
                    <thead>
                        <tr>                
                            <th>編輯</th>
                            <th>版次</th>
                            <th>狀態</th>
                            <th>生效日</th>                
                            <th>失效日</th>   
                            <th>額度</th>                            
                        </tr> 
                    </thead>    
                        <asp:Repeater runat="server" ID="rptFRC_VER" DataSourceID="sqlFRC_VER" > 
                            <ItemTemplate>
                                <tr id='tr<%#Container.ItemIndex+1%>' >     
                                    <td >
                                       <asp:Button ID="btnQuery" runat="server"  cssclass="button dadd"  Text="查詢" OnCommand="GridFunc_Click" CommandName='<%# Eval("VER") %>' />
                                       <asp:Button ID="btnUpd_D" runat="server"  cssclass="button upd"  Text="修改" OnCommand="GridFunc_Click" Visible='<%# Eval("STS").ToString()=="0"?true:false %>' CommandName='<%# Eval("VER") %>' />                                       
                                       <asp:Button ID="btnSure_D" runat="server"  cssclass="button upd"  Text="版次確認" OnCommand="GridFunc_Click" Visible='<%# Eval("STS").ToString()=="0"?true:false %>' CommandName='<%# Eval("VER") %>' />
                                       <asp:Button ID="btnCSure_D" runat="server"  cssclass="button upd"  Text="取消版次確認" OnCommand="GridFunc_Click" Visible='<%# (Container.ItemIndex+1).ToString()==Eval("ttlCount").ToString() && Eval("STS").ToString()=="1"?true:false %>' CommandName='<%# Eval("VER") %>' />                                       
                                       <asp:Button ID="btnDel_D" runat="server"  cssclass="button del"  Text="刪除" OnCommand="GridFunc_Click" Visible='<%# Eval("STS").ToString()=="0"?true:false %>' OnClientClick='return window.confirm("是否確定刪除此筆版次??");' CommandName='<%# Eval("VER") %>' />
                                    </td>               
                                    <td><%# Eval("VER")%></td>
                                    <td><%# Eval("FRC_STS")%></td>
                                    <td><%# Eval("Eff_Date")%></td>
                                    <td><%# Eval("Exp_Date")%></td>
                                    <td align="right"><%# Eval("QUOTA_AMT", "{0:###,###,###,##0}")%></td>
                                </tr>
                                <tr style='display:<%# (Container.ItemIndex+1).ToString()==Eval("ttlCount").ToString() && Eval("STS").ToString()=="1"?"":"none" %>'><td colspan="6"><asp:Button ID="btnAdd_D" runat="server"  cssclass="button dadd"  Text="新增版次" OnCommand="GridFunc_Click" CommandName='0' /></td></tr>                         
                            </ItemTemplate> 
                        </asp:Repeater> 
                         <tr style='display:<%=this.rptFRC_VER.Items.Count==0?"":"none" %>'><td colspan="6"><asp:Button ID="btnAdd_D" runat="server"  cssclass="button dadd"  Text="新增版次" OnCommand="GridFunc_Click" CommandName='0' /></td></tr>                         
                    </table>
                </div>      
            </div> 
            </ContentTemplate>
        </asp:UpdatePanel>  
        
        <asp:UpdatePanel runat="server" ID="upDetailEditing"  UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
            <table cellpadding="0"  cellspacing="0">
            <tr><td valign="top">
             <table style="margin-top:15px">
                <tr>
                    <th class="nonSpace">額度：</th>
                    <td><ocxControl:ocxNumber runat="server" ID="QUOTA_AMT" MASK="999,999,999"  /></td>
               </tr>
               <tr>     
                    <th class="nonSpace">附買回方式：</th>
                    <td>
                        <asp:DropDownList ID="BUY_WAY"  runat="server" Width="100" AutoPostBack="true" DataSourceID="sqlBUY_WAY"  DataValueField="TypeCode" DataTextField="TypeDesc" OnSelectedIndexChanged="BUY_WAY_Changed">                              
                        </asp:DropDownList>
                    </td> 
                </tr>
                <tr>
                    <th>送審單：</th>
                    <td><ocxControl:ocxUpload ID="SEND_EXAM_IMAGE" runat="server"  ></ocxControl:ocxUpload></td>                      
                </tr>   
                <tr>
                    <th>債權：</th>
                    <td><ocxControl:ocxUpload ID="DEBT_IMAGE" runat="server"  ></ocxControl:ocxUpload></td>                      
                </tr>   
                <tr><th>期滿約定：</th><td colspan="2"><asp:TextBox runat="server" ID="EXPIRE_PROMISE"   Width="180"  ></asp:TextBox></td></tr>
                <tr><th>其他約定：</th><td colspan="2"><asp:TextBox runat="server" ID="OTH_PROMISE" Rows="6"  Width="180"  TextMode="MultiLine"></asp:TextBox></td></tr>
            </table>
            </td>
            <td valign="top">
                <div  class="gridMain " style="width:465px">
                <div style="padding:0;margin:0; overflow-y:scroll;width:455px;height:220px" runat="server" id="editGrid">
                    <table cellpadding="0" cellspacing="0"  style="width:420px" > 
                    <thead>
                        <tr>      
                            <th>編輯</th>          
                            <th class="nonSpace">取回期間-起(月數)</th>
                            <th class="nonSpace">取回期間-迄(月數)</th>
                            <th class="nonSpace">買回比率(%)</th>                            
                        </tr> 
                    </thead>    
                        <asp:Repeater runat="server" ID="rptEdit">     
                            <ItemTemplate>
                                <tr style="display:<%# Eval("Status").ToString()=="D"?"none":"" %>" > 
                                    <td>
                                    <% if (!this.bolQuery)
                                       {%>
                                    <asp:Button ID="btnDel" runat="server"  cssClass="button del"  Text="刪除"  OnCommand="GridFunc_Click" CommandName='<%# Eval("RETK_DURN_FR") %>' />
                                    <%} %>
                                    <asp:HiddenField runat="server" ID="Status"  Value='<%# Eval("Status") %>' /></td>
                                    <td align="right"><ocxControl:ocxNumber runat="server" ID="RETK_DURN_FR" MASK="999" bolEnabled="false" Text='<%# Eval("RETK_DURN_FR", "{0:###,###,###,##0}")%>'   /></td>
                                    <td align="right"><ocxControl:ocxNumber runat="server" ID="RETK_DURN_TO" MASK="999" Text='<%# Eval("RETK_DURN_TO", "{0:###,###,###,##0}")%>'   /></td>
                                    <td align="right"><ocxControl:ocxNumber runat="server" ID="DUE_BUY_RATE" MASK="999" Text='<%# Eval("DUE_BUY_RATE", "{0:###,###,###,##0}")%>'   /></td>
                                </tr>
                            </ItemTemplate> 
                        </asp:Repeater>                       
                        <tr style='display:<%= this.BUY_WAY.SelectedValue=="1" || this.BUY_WAY.SelectedValue=="" || this.bolQuery ?"none":""%>'>
                            <td><asp:Button ID="btnAdd" runat="server"  cssClass="button dadd"  Text="新增" OnCommand="GridFunc_Click" CommandName="Add" /> </td>
                            <td align="right"><ocxControl:ocxNumber runat="server" ID="addRETK_DURN_FR" MASK="999" bolEnabled="false"   /></td>
                            <td align="right"><ocxControl:ocxNumber runat="server" ID="addRETK_DURN_TO" MASK="999"  /></td>
                            <td align="right"><ocxControl:ocxNumber runat="server" ID="addDUE_BUY_RATE" MASK="999"  /></td>                           
                        </tr>
                    </table>
                </div>     
            </div> 
            </td>
            </tr>           
            </table>            
            <div class="divFunction80" style='float:right;display:none'>
                <asp:Button runat="server" ID="btnEdit" CssClass="button trn80" Text="明細儲存" OnCommand="GridFunc_Click" CommandName="Save"   />
            </div>   
            </ContentTemplate>
           
        </asp:UpdatePanel>
                          
    </div>
    </div> 
 </div>   
  
    
                           
</asp:Content>

<asp:Content ID="myfunctionBar" ContentPlaceHolderID="functionBar" runat="server">

 <div class="divFunction" style="float:right">
    <asp:Button runat="server" ID="btnSavePrint" class="button trn" Text="存檔並列印核備書" OnClick="SaveAndPrint" />    
</div>   

</asp:Content>

<asp:Content ID="myPopWindow" ContentPlaceHolderID="PopWindow" runat="server">
     

     
<script language="javascript" type="text/javascript">
    function openDetail(strDate, strType, strName) {

    
        window.parent.openPopUpWindow();
    }
</script>
</asp:Content> 
