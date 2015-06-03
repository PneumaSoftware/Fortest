<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/editing.Master" CodeBehind="WE0101.aspx.cs" Inherits="OrixMvc.WE0101" %>
<%@ MasterType VirtualPath="~/Pattern/editing.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxTime"  TagPrefix="ocxControl" Src="~/ocxControl/ocxTime.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
        
<asp:Content ContentPlaceHolderID="editingArea" ID="mySearchArea" runat="server">
<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlCustKind" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="select ' ' CUST_FRC_TYPE_CODE,'請選擇..' CUST_FRC_TYPE_NAME union all select CUST_FRC_TYPE_CODE,CUST_FRC_TYPE_NAME from OR_CUST_KIND  where substring(CUST_FRC_TYPE_CODE,1,1)='0' "
    runat="server" >
</asp:sqldatasource>
<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlCUST_UNION" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="select ' ' CUST_FRC_TYPE_CODE,'請選擇..' CUST_FRC_TYPE_NAME union all select CUST_FRC_TYPE_CODE,CUST_FRC_TYPE_NAME from OR_CUST_KIND  where substring(CUST_FRC_TYPE_CODE,1,1)!='0' "
    runat="server" >
</asp:sqldatasource>

<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlNATIONALITY" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="select ' ' Nationality,'請選擇..' Nat_Name union all select Nationality=ltrim(rtrim(Nationality)),Nat_Name from OR_CUST_NAT "
    runat="server" >
</asp:sqldatasource>

<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlSPEC_COND" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'SPEC_COND' "
    runat="server" >
</asp:sqldatasource>    

<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlORG_TYPE" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'ORG_TYPE' "
    runat="server" >
</asp:sqldatasource>   

<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlCITY_CODE" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="select ' ' city_code,'請選擇..' City_name union all select city_code,City_name from or3_zip where city_code!='' group by city_code, city_name  order by city_code "
    runat="server" >
</asp:sqldatasource>   


<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlZIP_CODE" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="select ' ' ZIP_CODE,'請選擇..' ZIP_NAME union all select ZIP_CODE,ZIP_NAME=zone_name+' '+zip_code from or3_zip where city_code=@CITY_CODE AND ZIP_CODE!='' order by zip_code"
    runat="server" >
    <SelectParameters>
        <asp:ControlParameter ControlID="hiddenCityCode" PropertyName="Value" Name="CITY_CODE" />
    </SelectParameters>
</asp:sqldatasource>    

<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlRGT_CITY_CODE" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="select ' ' city_code,'請選擇..' City_name union all select city_code,City_name from or3_zip where city_code!='' group by city_code, city_name  order by city_code "
    runat="server" >
</asp:sqldatasource>   


<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlRGT_ZIP_CODE" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="select ' ' ZIP_CODE,'請選擇..' ZIP_NAME union all select ZIP_CODE,ZIP_NAME=zone_name+' '+zip_code from or3_zip where city_code=@RGT_CITY_CODE AND ZIP_CODE!='' order by zip_code"
    runat="server" >
    <SelectParameters>
        <asp:ControlParameter ControlID="hiddenRGT_CITY_CODE" PropertyName="Value" Name="RGT_CITY_CODE" />
    </SelectParameters>
</asp:sqldatasource>   
<ajax:FilteredTextBoxExtender ID="filter_myDate" runat="server" TargetControlID="CUST_NO" FilterType="Custom" ValidChars="0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ/" />
    <table cellpadding="1"  >        
        <tr>
            <th>集團代號：</th>
            <td><ocxControl:ocxDialog runat="server" ID="CUST_BLOC_CODE" Text='<%# Eval("CUST_BLOC_CODE") %>' width="65" ControlID="BLOC_SNAME" FieldName="BLOC_SNAME" SourceName="OR_BLOC" /> </td>
            <td colspan="7"><asp:TextBox runat="server" CssClass="display"   ID="BLOC_SNAME" Text='<%# Eval("BLOC_SNAME") %>' size="10"></asp:TextBox>行業別：<asp:DropDownList ID="CUST_TYPE_CODE"  runat="server"  OnPreRender='checkList' ToolTip='<%# Eval("CUST_TYPE_CODE") %>' Width="125"                                                                           
                 DataSourceID="sqlCustKind" DataValueField="CUST_FRC_TYPE_CODE" DataTextField="CUST_FRC_TYPE_NAME" >
                </asp:DropDownList> 租賃公會行業別：<asp:DropDownList ID="CUST_UNION"  runat="server"  OnPreRender='checkList' ToolTip='<%# Eval("CUST_UNION") %>' Width="135" 
                 DataSourceID="sqlCUST_UNION" DataValueField="CUST_FRC_TYPE_CODE" DataTextField="CUST_FRC_TYPE_NAME" >
                </asp:DropDownList><font color="red">*</font>國別
           <asp:DropDownList ID="NATIONALITY"  runat="server"  OnPreRender='checkList' ToolTip='<%# Eval("NATIONALITY") %>' Width="70"                                                                       
                 DataSourceID="sqlNATIONALITY" DataValueField="Nationality" DataTextField="Nat_Name" >
                </asp:DropDownList> </td>                                
        </tr>  
        <tr>            
            <th>客戶代號：</th>
            <td><asp:TextBox runat="server" ID="CUST_NO"  size="10"  MaxLength="10" Text='<%# Eval("CUST_NO") %>'></asp:TextBox></td>
            <th class="nonSpace">客戶名稱：</th>
            <td colspan="4"><asp:TextBox runat="server" ID="CUST_NAME" MaxLength="80" Width="230" Text='<%# Eval("CUST_NAME") %>'   size="20"></asp:TextBox><font class="memo" style='display:<%=this.bolProject?"":"none"%>'>專案額度客戶</font></td> 
            <th>統一編號：</th>
            <td><asp:TextBox runat="server" onblur="chkIDandGUI(this);setCUSTOM(this);" ID="UNIF_NO" Text='<%# Eval("UNIF_NO") %>' size="10"  MaxLength="10" ></asp:TextBox></td>
        </tr>  
         <tr>            
            <th>英文名稱：</th>
            <td colspan="2"><asp:TextBox runat="server" ID="EN_NAME"  size="20"  Text='<%# Eval("EN_NAME") %>'></asp:TextBox></td>
            <th>英文簡稱：</th>
            <td colspan="2"><asp:TextBox runat="server" ID="EN_SNAME" size="15"   Text='<%# Eval("EN_SNAME") %>' ></asp:TextBox></td> 
            <td></td>
            <th class="nonSpace">客戶簡稱：</th>
            <td><asp:TextBox runat="server" ID="CUST_SNAME" Text='<%# Eval("CUST_SNAME") %>' size="10"  MaxLength="40" ></asp:TextBox></td>            
        </tr>    
         <tr>            
            <th >聯絡人：</th>
            <td ><asp:TextBox runat="server" ID="CONTACT"  size="10"  MaxLength="12" Text='<%# Eval("Contact") %>'></asp:TextBox></td>
            <th>聯絡人職稱：</th>
            <td colspan="2"><asp:TextBox runat="server" ID="CTAC_TITLE" size="15"  MaxLength="20" Text='<%# Eval("CTAC_TITLE") %>' ></asp:TextBox></td>             
            <th>聯絡人分機：</th>
            <td><asp:TextBox runat="server" ID="CTAC_EXT" Text='<%# Eval("CTAC_EXT") %>' size="5"  MaxLength="5" ></asp:TextBox></td>
            <th>股票代號：</th>
            <td><asp:TextBox runat="server" ID="STOCKCODE" Text='<%# Eval("STOCKCODE") %>' size="4"  MaxLength="4" ></asp:TextBox></td>
        </tr>    
        <tr>            
            <th>聯絡人2：</th>
            <td><asp:TextBox runat="server" ID="CONTACT2"  size="10"  MaxLength="12" Text='<%# Eval("Contact2") %>'></asp:TextBox></td>
            <th>聯絡人職稱2：</th>
            <td colspan="2"><asp:TextBox runat="server" ID="CTAC_TITLE2" size="15"  MaxLength="20" Text='<%# Eval("CTAC_TITLE2") %>' ></asp:TextBox></td>             
            <th>聯絡人分機2：</th>
            <td><asp:TextBox runat="server" ID="CTAC_EXT2" Text='<%# Eval("CTAC_EXT2") %>' size="5"  MaxLength="5" ></asp:TextBox></td>
        </tr>   
        <tr>            
            <th>負責人：</th>
            <td colspan="4"><asp:TextBox runat="server" ID="TAKER"  size="10"  MaxLength="20" Text='<%# Eval("TAKER") %>'></asp:TextBox></td>
            <th>創業日期：</th>
            <td><ocxControl:ocxDate runat="server" ID="FLOT_DATE" Text='<%# Eval("FLOT_DATE") %>' /></td> 
            <th>設立日期：</th>
            <td><ocxControl:ocxDate runat="server" ID="BUILD_DATE" Text='<%# Eval("BUILD_DATE") %>' /></td> 
        </tr>   
        <tr>            
            <th>電話一：</th>
            <td ><asp:TextBox runat="server" ID="PHONE1"  size="10"  MaxLength="20" Text='<%# Eval("PHONE1") %>'></asp:TextBox></td>
            <th>電話二：</th>
            <td colspan="2"><asp:TextBox runat="server" ID="PHONE2" size="12"  MaxLength="20" Text='<%# Eval("PHONE2") %>' ></asp:TextBox></td>             
            <th>傳真：</th>
            <td><asp:TextBox runat="server" ID="FACSIMILE"   size="12"  MaxLength="20"  Text='<%# Eval("FACSIMILE") %>'  ></asp:TextBox></td>
            <th>員工人數：</th>
            <td><ocxControl:ocxNumber runat="server" ID="EMP_PSNS" MASK="999,999" Text='<%# Eval("EMP_PSNS") %>' /></td>            
        </tr>       
        <tr>            
            <th>營業登記地址：</th> 
            <td colspan="5" >           
           <asp:UpdatePanel runat="server" ID="upRGT_ZIP_CODE"  UpdateMode="Conditional" RenderMode="Inline"><ContentTemplate>
                     <asp:DropDownList ID="RGT_CITY_CODE"  AutoPostBack="true"  runat="server"  OnPreRender='checkList' ToolTip='<%# Eval("RGT_CITY_CODE") %>'               
                 DataSourceID="sqlRGT_CITY_CODE" DataValueField="CITY_CODE" DataTextField="CITY_NAME" OnSelectedIndexChanged="RGT_ZIP_CODE_LOAD" >
             </asp:DropDownList>                
             <asp:HiddenField runat="server" ID="hiddenRGT_CITY_CODE" Value='<%# Eval("RGT_CITY_CODE") %>' /> 
                    <asp:DropDownList ID="RGT_ZIP_CODE"  runat="server" AutoPostBack="true"  OnPreRender='checkList' OnSelectedIndexChanged='SetAddress' ToolTip='<%# Eval("RGT_ZIP_CODE") %>' 
                     DataSourceID="sqlRGT_ZIP_CODE" DataValueField="ZIP_CODE" DataTextField="ZIP_NAME" >
                    </asp:DropDownList>
                    <asp:TextBox runat="server" ID="SALES_RGT_ADDR"  size="50" MaxLength="80"  Width="220" Text='<%# Eval("SALES_RGT_ADDR") %>'></asp:TextBox>
                </ContentTemplate>
                <Triggers>                    
                    <asp:AsyncPostBackTrigger ControlID="RGT_CITY_CODE" />
                </Triggers>
           </asp:UpdatePanel>                
            
            </td>
            <th colspan="2">登記資本額(萬元)：</th>
            <td><ocxControl:ocxNumber runat="server" ID="RGT_CAPT_AMT" MASK="999,999,999" Text='<%# Eval("RGT_CAPT_AMT") %>' /></td>            
        </tr>       
        <tr>            
            <th>連絡地址：</th>
            <td colspan="5" >                        
           <asp:UpdatePanel runat="server" ID="upZIPCODE"   UpdateMode="Conditional" RenderMode="Inline"><ContentTemplate>
           <asp:DropDownList ID="CITY_CODE"  AutoPostBack="true"  runat="server"  OnPreRender='checkList' ToolTip='<%# Eval("CITY_CODE") %>'               
                 DataSourceID="sqlCITY_CODE" DataValueField="CITY_CODE" DataTextField="CITY_NAME" OnSelectedIndexChanged="ZIP_CODE_LOAD" >
             </asp:DropDownList>                
             <asp:HiddenField runat="server" ID="hiddenCityCode" Value='<%# Eval("CITY_CODE") %>' /> 
                    <asp:DropDownList ID="ZIP_CODE"  runat="server" AutoPostBack="true" OnPreRender='checkList'  OnSelectedIndexChanged='SetAddress' ToolTip='<%# Eval("ZIP_CODE") %>' 
                     DataSourceID="sqlZIP_CODE" DataValueField="ZIP_CODE" DataTextField="ZIP_NAME" >
                    </asp:DropDownList>
                    <asp:TextBox runat="server" ID="CTAC_ADDR"  size="31" Width="220" MaxLength="80" Text='<%# Eval("CTAC_ADDR") %>'></asp:TextBox></td>
                </ContentTemplate>
                <Triggers>                    
                    <asp:AsyncPostBackTrigger ControlID="CITY_CODE" />
                </Triggers>
           </asp:UpdatePanel>     </td>                        
            <th colspan="2">實收資本額(萬元)：</th>
            <td><ocxControl:ocxNumber runat="server" ID="REAL_CAPT_AMT" MASK="999,999,999" Text='<%# Eval("REAL_CAPT_AMT") %>' /></td>            
        </tr>   
        <tr>       
     
   
            <th>特殊客戶條件：</th>
            <td colspan="2"><asp:DropDownList ID="SPEC_COND"  runat="server"  OnPreRender='checkList' ToolTip='<%# Eval("SPEC_COND") %>' 
                 DataSourceID="sqlSPEC_COND" DataValueField="TypeCode" DataTextField="TypeDesc" >
                </asp:DropDownList> </td>             
            <td colspan="2"><asp:CheckBox runat="server" ID="IS_COND_AUTH" Text="有條件授權" OnPreRender='checkList' ToolTip='<%# Eval("IS_COND_AUTH") %>'/></td>
            <td colspan="2"><asp:CheckBox runat="server" ID="IS_BIZ_CUST" Text="營業用設備客戶" OnPreRender='checkList' ToolTip='<%# Eval("IS_BIZ_CUST") %>' /></td>
            <th><asp:CheckBox runat="server" ID="HONEST_AGREEMENT" Text="廉潔協定" OnPreRender='checkList' ToolTip='<%# Eval("HONEST_AGREEMENT") %>' /></th>
            <th><asp:CheckBox runat="server" ID="SECRET_PROMISE" Text="保密協定" OnPreRender='checkList' ToolTip='<%# Eval("SECRET_PROMISE") %>' /></th>
        </tr> 
         <tr>                        
            <th colspan="2"><asp:CheckBox runat="server" ID="IS_TRANSACTION" Text="是否成交客戶" Enabled="false" OnPreRender='checkList' ToolTip='<%# Eval("IS_TRANSACTION") %>'/></th>
             <th>組織型態：</th><td><asp:DropDownList ID="ORG_TYPE"  runat="server"  OnPreRender='checkList' ToolTip='<%# Eval("ORG_TYPE") %>' Width=110                                                                         
                 DataSourceID="sqlORG_TYPE" DataValueField="TypeCode" DataTextField="TypeDesc" >
                </asp:DropDownList> </td> 
             <th colspan="2">發票開立方式：</th><td><asp:DropDownList ID="INVOICE"  runat="server"  OnPreRender='checkList' ToolTip='<%# Eval("INVOICE") %>' >
                <asp:ListItem Value="2">二聯</asp:ListItem>
                <asp:ListItem Value="3">三聯</asp:ListItem>
                </asp:DropDownList> </td>             
        </tr>   
        <tr>            
            <th>主要營業項目：</th><td colspan="4" ><asp:TextBox runat="server" ID="MAIN_BUS_ITEM"  size="28"  MaxLength="40" Width="270" Text='<%# Eval("MAIN_BUS_ITEM") %>'></asp:TextBox></td>            
            <td><asp:CheckBox runat="server" ID="CUST_STS" Text="存為潛在客戶" OnPreRender='checkList' ToolTip='<%# Eval("CUST_STS") %>'/> </td>       
        </tr>     
        <tr>            
            <th>母公司名：</th><td colspan="4" ><asp:TextBox runat="server" ID="PARENT_COMP_NAME"  size="28"  MaxLength="80" Text='<%# Eval("PARENT_COMP_NAME") %>'></asp:TextBox></td>  
            <th>母公司股票代號：</th><td><asp:TextBox runat="server" ID="PARENT_COMP_STOCK_CODE" Text='<%# Eval("PARENT_COMP_STOCK_CODE") %>' size="4"  MaxLength="4" ></asp:TextBox></td>          
        </tr>
        
        <tr><th><h4>已核淮額度</h4></th><th>租賃分期案：</th><td colspan="2"><asp:TextBox runat="server" CssClass="display number" ReadOnly="true"  ID="CUST_GEN_QUOTA" Text='<%# Eval("GEN_CURR_QUOTA", "{0:###,###,###,##0}") %>' size="11"></asp:TextBox></td><th colspan="2"><h4>客戶背景資料</h4></th><td rowspan="3" colspan="4"><asp:TextBox runat="server" TextMode="MultiLine" Rows="4" size="30" Width="260" ID="BACKGROUND" Text='<%# Eval("BACKGROUND") %>' ></asp:TextBox></td></tr>
        <tr><th></th><th>VP專案：</th><td colspan="2"><asp:TextBox runat="server" CssClass="display number" ReadOnly="true"  ID="CUST_VP_QUOTA" Text='<%# Eval("VP_CURR_QUOTA", "{0:###,###,###,##0}") %>' size="11"></asp:TextBox></td></tr>
        <tr><th></th><th>帳款受讓案：</th><td colspan="2"><asp:TextBox runat="server" CssClass="display number" ReadOnly="true"  ID="CUST_AR_QUOTA" Text='<%# Eval("AR_CURR_QUOTA", "{0:###,###,###,##0}") %>' size="11"></asp:TextBox></td></tr>
    </table>
    
    <div style="display:none"><asp:TextBox runat="server" ID="chkExist"></asp:TextBox><asp:TextBox runat="server" ID="chkINVOICE"></asp:TextBox></div>
    
</asp:Content>


<asp:Content ID="myPopWindow" ContentPlaceHolderID="PopWindow" runat="server">
     

     
<script language="javascript" type="text/javascript">
  
  function setCUSTOM(obj){
    if (obj.vlaue!="")
        document.getElementById("<%=this.CUST_NO.ClientID %>").value=obj.value;
    
  }
  
  
    function openDetail(strDate, strType, strName) {
        window.parent.openPopUpWindow();
    }
    
    function checkFunction(chkType){
        switch(chkType)
        {
            case "1":
                if (window.confirm("統編證號未輸入，請確認？"))
                {
                   document.getElementById("<%=this.chkINVOICE.ClientID %>").value ="Y";
                   document.getElementById("ctl00_ctl00_body_btnEdit").click();
                   return;
                }
                break;
            case "2":
                if (window.confirm("此客戶名稱已存在,是否確定儲存？"))
                {
                   document.getElementById("<%=this.chkExist.ClientID %>").value ="Y";
                   document.getElementById("ctl00_ctl00_body_btnEdit").click();
                   return;
                }
                break;
        }
       
        
    }
</script>
</asp:Content> 
