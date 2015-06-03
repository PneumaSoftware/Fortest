<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/editing.Master" CodeBehind="WG0101.aspx.cs" Inherits="OrixMvc.WG0101" %>
<%@ MasterType VirtualPath="~/Pattern/editing.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxTime"  TagPrefix="ocxControl" Src="~/ocxControl/ocxTime.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
        
<asp:Content ContentPlaceHolderID="editingArea" ID="mySearchArea" runat="server">
<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlCOLL_MTHD" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'COLL_MTHD' "
    runat="server" >
</asp:sqldatasource>

    <span style="display:none">
    <asp:TextBox runat="server" ID="SeqNo" Text='<%# Eval("SeqNo") %>' />
    </span>
    <table >
        <tr>
            <th class="nonSpace">銀行代碼：</th><td><ocxControl:ocxDialog runat="server" ID="BANK_NO" width="100" Text='<%# Eval("BANK_NO") %>' ControlID="BANK_NAME" FieldName="BANK_NAME"  SourceName="ACC18" /></td>
            <th>銀行名稱：</th><td colspan="4"><asp:TextBox runat="server"  CssClass="display" ID="BANK_NAME" Text='<%# Eval("BANK_NAME") %>'  Width="300"  size="10"></asp:TextBox></td>   
        </tr>          
        <tr>            
            <th rowspan="2">銀行類別：</th><td rowspan="2">
                <asp:RadioButtonList runat="server" ID="BANK_TYPE" RepeatDirection="Vertical" OnPreRender='checkList' RepeatLayout="Flow" ToolTip='<%# Eval("BANK_TYPE") %>'>
                    <asp:ListItem Value="1">本國銀行</asp:ListItem>
                    <asp:ListItem Value="2">票券金融</asp:ListItem>
                    <asp:ListItem Value="3">外商銀行</asp:ListItem>
                    <asp:ListItem Value="4">其他</asp:ListItem>                    
                </asp:RadioButtonList>
            </td>  
            <th>長短借：</th>
            <td><asp:DropDownList ID="LONG_SHORT_LOAN"  runat="server" Width=70  OnPreRender='checkList' ToolTip='<%# Eval("LONG_SHORT_LOAN") %>' >
                <asp:ListItem Value="1">綜合</asp:ListItem>
                <asp:ListItem Value="2">長借</asp:ListItem>
                <asp:ListItem Value="3">短借</asp:ListItem>
                </asp:DropDownList>
            </td>                      
        </tr>   
        <tr>
            <th>是否循環：</th>
            <td><asp:DropDownList ID="ISCYCLE"  runat="server" Width=70  OnPreRender='checkList' ToolTip='<%# Eval("ISCYCLE") %>' >
                <asp:ListItem Value="Y">循環</asp:ListItem>
                <asp:ListItem Value="N">不循環</asp:ListItem>                
                </asp:DropDownList>
            </td>                      
        </tr>   
        <tr>
            <th class="nonSpace">授信額度：</th>
            <td><ocxControl:ocxNumber runat="server" ID="CRD_AMT" MASK="999,999,999" Text='<%# Eval("CRD_AMT") %>' /></td>   
            <th>已使用授信額度：</th>
            <td><asp:TextBox runat="server" ReadOnly="true" CssClass="display number" ID="USED_CREDIT"   Text='<%# Eval("USED_CREDIT", "{0:###,###,###,##0}") %>' Width="90" ></asp:TextBox></td>
            <th>剩餘額度：</th>
            <td><asp:TextBox runat="server" ReadOnly="true" CssClass="display number" ID="REST_CREDIT"   Text='<%# Eval("REST_CREDIT", "{0:###,###,###,##0}") %>' Width="90" ></asp:TextBox></td>
            <th>長借：</th>
            <td><asp:TextBox runat="server" ReadOnly="true" CssClass="display number" ID="LONG_LOAN"   Text='<%# Eval("LONG_LOAN", "{0:###,###,###,##0}") %>' Width="35" ></asp:TextBox>%</td>
            <th>短借：</th>
            <td><asp:TextBox runat="server" ReadOnly="true" CssClass="display number" ID="SHORT_LOAN"   Text='<%# Eval("SHORT_LOAN", "{0:###,###,###,##0}") %>' Width="35" ></asp:TextBox>%</td>
        </tr>
        <tr>
            <th class="nonSpace">擔保方式：</th>
            <td><asp:DropDownList ID="COLL_MTHD"  runat="server" Width="97" AutoPostBack="true" OnSelectedIndexChanged="COLL_Change"  OnPreRender='checkList' ToolTip='<%# Eval("COLL_MTHD") %>'                                                                          
                 DataSourceID="sqlCOLL_MTHD" DataValueField="TypeCode" DataTextField="TypeDesc" >
                </asp:DropDownList> </td>            
            <th>PDC擔保成數：</th>
            <td><asp:UpdatePanel runat="server" ID="upPDC" RenderMode="Inline" UpdateMode="Conditional">
                <ContentTemplate><ocxControl:ocxNumber runat="server" ID="PDC_PERCENT" MASK="999.9999" Text='<%# Eval("PDC_PERCENT") %>'  />%</ContentTemplate>
                <Triggers><asp:AsyncPostBackTrigger ControlID="COLL_MTHD" /> </Triggers>
            </asp:UpdatePanel>
            </td> 
        </tr>              
        <tr>
            <th>保證費率：</th>
            <td><ocxControl:ocxNumber runat="server" ID="BOND_RATE" MASK="999.9999" Text='<%# Eval("BOND_RATE") %>' />%</td> 
        </tr>
        <tr>
            <th class="nonSpace">授信到期日：</th><td><ocxControl:ocxDate runat="server" ID="CRD_DATE_TO"  Text='<%# Eval("CRD_DATE_TO") %>'/></td>
        </tr>
        <tr>
            <th>最後異動日：</th>
            <td><asp:TextBox runat="server" ReadOnly="true" CssClass="display" ID="LAST_CHG_DATE"   Text='<%# Eval("LAST_CHG_DATE") %>' Width="97" ></asp:TextBox></td>
        </tr>
        <tr>
            <th >資金成本加碼說明：</th>
            <td colspan="3" ><asp:TextBox runat="server" TextMode="MultiLine" Rows="5" ID="CAPT_CODE_DESC"   Text='<%# Eval("CAPT_CODE_DESC") %>' Width="280" ></asp:TextBox></td>
            <th>備註：</th>
            <td colspan="5"><asp:TextBox runat="server" TextMode="MultiLine" Rows="5" ID="REMARK"   Text='<%# Eval("REMARK") %>' Width="250" ></asp:TextBox></td>
        </tr>       
    </table>
    
    
                           
</asp:Content>


<asp:Content ID="myPopWindow" ContentPlaceHolderID="PopWindow" runat="server">
     

     
<script language="javascript" type="text/javascript">
    function openDetail(strDate, strType, strName) {

    
        window.parent.openPopUpWindow();
    }
</script>
</asp:Content> 
