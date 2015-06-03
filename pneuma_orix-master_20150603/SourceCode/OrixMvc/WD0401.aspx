<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/editing.Master" CodeBehind="WD0401.aspx.cs" Inherits="OrixMvc.WD0401" %>
<%@ MasterType VirtualPath="~/Pattern/editing.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxYM"  TagPrefix="ocxControl" Src="~/ocxControl/ocxYM.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxTime"  TagPrefix="ocxControl" Src="~/ocxControl/ocxTime.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
        
<asp:Content ContentPlaceHolderID="editingArea" ID="myEditingArea" runat="server">



<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlUSE" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="
    select	APLY_NO,PERIOD,MAC_NO,MONTH_FEE,FREE_QTY_COLOR,FREE_QTY_MONO,
		FREE_QTY_A3_COLOR,FREE_QTY_A3_MONO,USE_QTY_COLOR,USE_QTY_MONO,
		USE_QTY_A3_COLOR,USE_QTY_A3_MONO,SUM_A4_COLOR,SUM_A4_COLOR_LAST,
		SUM_A4_MONO,SUM_A4_MONO_LAST,SUM_A3_COLOR,SUM_A3_COLOR_LAST,SUM_A3_MONO,SUM_A3_MONO_LAST,
		ONE_PRICE_COLOR,ONE_PRICE_MONO,	ONE_PRICE_A3_COLOR,ONE_PRICE_A3_MONO,SUBTOT_COLOR,SUBTOT_MONO,TOTAL,XEROX_CON_NO,MSG
    from	OR3_PAPER_USE_DTL_TMP   where APLY_NO=@APLY_NO and ORI_APLY_NO=@ORI_APLY_NO
     "
    runat="server" >
    <SelectParameters>
        <asp:ControlParameter ControlID="APLY_NO" Name="APLY_NO" PropertyName=Value />
         <asp:ControlParameter ControlID="ORI_APLY_NO" Name="ORI_APLY_NO" PropertyName=Value />
    </SelectParameters>
</asp:sqldatasource> 

<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlINV" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="
    select APLY_NO,period,dbo.f_DateAddSlash(INV_DATE) as INV_DATE,INV_NO,AMOUNT,TAX,SUPPLY_INV_AMT from OR3_PAPER_SUPPLY_INV_TEMP   where APLY_NO=@APLY_NO
     "
    runat="server" >
    <SelectParameters>
        <asp:ControlParameter ControlID="APLY_NO" Name="APLY_NO" PropertyName=Value />
    </SelectParameters>
</asp:sqldatasource> 

    <asp:HiddenField runat="server" ID="APLY_NO" Value='<%# Eval("APLY_NO") %>'/> 
    <asp:HiddenField runat="server" ID="ORI_APLY_NO" Value='<%# Eval("ORI_APLY_NO") %>'/>
        
     
    <div  class="gridMain " style="width:790px">
        <div id="editGrid" style=" position:relative;padding:0;margin:0; overflow-y:scroll;width:790px;height:200px" >
            <table cellpadding="0" cellspacing="0"  style="width:770px" > 
            
                <tr>
                    <th class="fixCol">申請書編號</th>
                    <th class="fixCol">期數</th>
                    <th class="fixCol">機號</th>
                    <th>機器分期<br />月費</th>
                    <th>免費彩色<br />A4張數</th>
                    <th>免費黑白<br />A4張數</th>                    
                    <th>免費彩色<br />A3張數</th>
                    <th>免費黑白<br />A3張數</th>
                    <th>實際彩色<br />A4<br />列印張數</th>
                    <th>實際黑白<br />A4<br />列印張數</th>                    
                    <th>實際彩色<br />A3<br />列印張數</th>
                    <th>實際黑白<br />A3<br />列印張數</th>                    
                    <th>彩色A4<br />本月積數</th>
                    <th>彩色A4<br />上月積數</th>
                    <th>黑白A4<br />本月積數</th>
                    <th>黑白A4<br />上月積數</th>
                    <th>彩色A3<br />本月積數</th>
                    <th>彩色A3<br />上月積數</th>
                    <th>黑白A3<br />本月積數</th>
                    <th>黑白A3<br />上月積數</th>
                    <th>彩色A4<br />單張金額</th>
                    <th>黑白A4<br />單張金額</th>
                    <th>彩色A3<br />單張金額</th>
                    <th>黑白A3<br />單張金額</th>
                    <th>彩色金額<br />小計</th>
                    <th>黑白金額<br />小計</th>
                    <th>合計</th>
                    <th>全錄合約編號</th>
                    <th>錯誤訊息</th>
                </tr>   
                <asp:Repeater runat="server" ID="rptUSE" DataSourceID="sqlUSE"  > 
                    <ItemTemplate>
                        <tr > 
                            <td class="fixCol"><asp:TextBox runat="server" Text='<%# Eval("APLY_NO")%>' ID="txtAPLY_NO" Width="120"  ></asp:TextBox> <asp:HiddenField runat="server" ID="APLY_NO" Value='<%# Eval("APLY_NO")%>' /></td>
                            <td class="fixCol number"><ocxControl:ocxNumber runat="server" ID="txtPERIOD" MASK="9,999" Text='<%# Eval("PERIOD")%>' /><asp:HiddenField runat="server" ID="PERIOD" Value='<%# Eval("PERIOD")%>' /></td> 
                            <td class="fixCol"><asp:TextBox runat="server" Text='<%# Eval("MAC_NO")%>' ID="txtMAC_NO"  Width="120" ></asp:TextBox><asp:HiddenField runat="server" ID="MAC_NO" Value='<%# Eval("MAC_NO")%>' /></td>
                            <td class="number"><ocxControl:ocxNumber runat="server" ID="MONTH_FEE" MASK="9,99,999" Text='<%# Eval("MONTH_FEE") %>' /></td>
                            <td class="number"><%# Eval("FREE_QTY_COLOR", "{0:###,###,###,##0}")%></td> 
                            <td class="number"><%# Eval("FREE_QTY_MONO", "{0:###,###,###,##0}")%></td> 
                            <td class="number"><%# Eval("FREE_QTY_A3_COLOR", "{0:###,###,###,##0}")%></td> 
                            <td class="number"><%# Eval("FREE_QTY_A3_MONO", "{0:###,###,###,##0}")%></td> 
                            <td class="number"><ocxControl:ocxNumber runat="server" ID="USE_QTY_COLOR" MASK="999,999" Text='<%# Eval("USE_QTY_COLOR") %>' /></td>
                            <td class="number"><ocxControl:ocxNumber runat="server" ID="USE_QTY_MONO" MASK="999,999" Text='<%# Eval("USE_QTY_MONO") %>' /></td>
                            <td class="number"><ocxControl:ocxNumber runat="server" ID="USE_QTY_A3_COLOR" MASK="999,999" Text='<%# Eval("USE_QTY_A3_COLOR") %>' /></td>
                            <td class="number"><ocxControl:ocxNumber runat="server" ID="USE_QTY_A3_MONO" MASK="999,999" Text='<%# Eval("USE_QTY_A3_MONO") %>' /></td>
                            <td class="number"><ocxControl:ocxNumber runat="server" ID="SUM_A4_COLOR" MASK="999,999" Text='<%# Eval("SUM_A4_COLOR") %>' /></td>
                            <td class="number"><ocxControl:ocxNumber runat="server" ID="SUM_A4_COLOR_LAST" MASK="999,999" Text='<%# Eval("SUM_A4_COLOR_LAST") %>' /></td>
                            <td class="number"><ocxControl:ocxNumber runat="server" ID="SUM_A4_MONO" MASK="999,999" Text='<%# Eval("SUM_A4_MONO") %>' /></td>
                            <td class="number"><ocxControl:ocxNumber runat="server" ID="SUM_A4_MONO_LAST" MASK="999,999" Text='<%# Eval("SUM_A4_MONO_LAST") %>' /></td>
                            <td class="number"><ocxControl:ocxNumber runat="server" ID="SUM_A3_COLOR" MASK="999,999" Text='<%# Eval("SUM_A3_COLOR") %>' /></td>
                            <td class="number"><ocxControl:ocxNumber runat="server" ID="SUM_A3_COLOR_LAST" MASK="999,999" Text='<%# Eval("SUM_A3_COLOR_LAST") %>' /></td>
                            <td class="number"><ocxControl:ocxNumber runat="server" ID="SUM_A3_MONO" MASK="999,999" Text='<%# Eval("SUM_A3_MONO") %>' /></td>
                            <td class="number"><ocxControl:ocxNumber runat="server" ID="SUM_A3_MONO_LAST" MASK="999,999" Text='<%# Eval("SUM_A3_MONO_LAST") %>' /></td>
                             <td class="number"><%# Eval("ONE_PRICE_COLOR", "{0:###,###,###,##0}")%></td> 
                            <td class="number"><%# Eval("ONE_PRICE_MONO", "{0:###,###,###,##0}")%></td> 
                            <td class="number"><%# Eval("ONE_PRICE_A3_COLOR", "{0:###,###,###,##0}")%></td> 
                            <td class="number"><%# Eval("ONE_PRICE_A3_MONO", "{0:###,###,###,##0}")%></td> 
                            <td class="number"><%# Eval("SUBTOT_COLOR", "{0:###,###,###,##0}")%></td> 
                            <td class="number"><%# Eval("SUBTOT_MONO", "{0:###,###,###,##0}")%></td> 
                            <td class="number"><%# Eval("TOTAL", "{0:###,###,###,##0}")%></td> 
                            <td ><%# Eval("XEROX_CON_NO")%></td>
                            <td ><%# Eval("MSG")%></td>
                        </tr>
                    </ItemTemplate> 
                </asp:Repeater>     
                
            </table>
        </div>     
    </div> 
     <div  class="gridMain " style="width:790px">
        <div id="editGrid" style=" position:relative;padding:0;margin:0; overflow-y:scroll;width:780px;height:270px" >
            <table cellpadding="0" cellspacing="0"  style="width:600px" > 
           
                <tr>
                    <th>申請書編號</th>
                    <th>期數</th>
                    <th>發票號碼</th>
                    <th>發票日期</th>
                    <th>發票金額</th>
                    <th>發票稅額</th>
                    <th>發票總金額</th>                    
                </tr> 
          
                <asp:Repeater runat="server" ID="rptINV" DataSourceID="sqlINV"  > 
                    <ItemTemplate>
                        <tr > 
                            <td class="fixCol"><%# Eval("APLY_NO")%><asp:HiddenField runat="server" ID="APLY_NO" Value='<%# Eval("APLY_NO")%>' /></td>
                            <td class="fixCol number"><%# Eval("PERIOD")%><asp:HiddenField runat="server" ID="PERIOD" Value='<%# Eval("PERIOD")%>' /></td> 
                            <td><asp:TextBox runat="server" ID="INV_NO" Text='<%# Eval("INV_NO")%>' Width=150></asp:TextBox><asp:HiddenField runat="server" ID="OLD_INV_NO" Value='<%# Eval("INV_NO")%>' /></td>
                            <td class="number"><ocxControl:ocxDate runat="server" ID="INV_DATE"  Text='<%# Eval("INV_DATE") %>'/></td>                            
                            <td class="number"><ocxControl:ocxNumber runat="server" ID="AMOUNT" MASK="9,999,999" Text='<%# Eval("AMOUNT") %>' /></td>
                            <td class="number"><ocxControl:ocxNumber runat="server" ID="TAX" MASK="99,999" Text='<%# Eval("TAX") %>' /></td>
                            <td class="number"><ocxControl:ocxNumber runat="server" ID="SUPPLY_INV_AMT" MASK="9,999,999" Text='<%# Eval("SUPPLY_INV_AMT") %>' /></td>
                        </tr>
                    </ItemTemplate> 
                </asp:Repeater>     
                
            </table>
        </div>     
    </div> 

</asp:Content> 

