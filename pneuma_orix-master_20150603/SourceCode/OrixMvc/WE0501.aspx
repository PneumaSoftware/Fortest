<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/editing.Master" CodeBehind="WE0501.aspx.cs" Inherits="OrixMvc.WE0501" %>
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
    id="sqlCHEQ_KIND" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'CHEQ_KIND' "
    runat="server" >
</asp:sqldatasource> 

    <asp:HiddenField runat="server" ID="CUST_NO" Value='<%# Eval("CUST_NO") %>'/> 
    <table>
        <tr>
            <th>申請書編號：</th>
            <td><asp:TextBox runat="server" ID="APLY_NO" Text='<%# Eval("APLY_NO") %>' CssClass="display" ReadOnly="true"  /></td>
            <td>
            <div class="divFunction" >
                <asp:Button runat="server" ID="btnGet" CssClass="button trn" Text="取得其他契約票據" OnCommand="Function_Click" CommandName="GetPaper"  />
            </div>
            <div class="divFunction">
                <asp:Button runat="server" ID="btnFunction2" CssClass="button trn" Text="業管課簽收" OnCommand="Function_Click" CommandName="Sign1"  />
            </div>
            <div class="divFunction">
                <asp:Button runat="server" ID="btnFunction3" CssClass="button trn" Text="業管課退回"  OnCommand="Function_Click" CommandName="Sign1Back"  />
            </div>
            <div class="divFunction">
                <asp:Button runat="server" ID="btnFunction4" CssClass="button trn" Text="資金課簽收"  OnCommand="Function_Click" CommandName="Sign2"  />
            </div>   
            <div class="divFunction">
                <asp:Button runat="server" ID="btnFunction5" CssClass="button trn" Text="資金課退回" OnCommand="Function_Click" CommandName="Sign2Back"  />
            </div>      
            </td>
        </tr>
    </table>
        
     
    <div  class="gridMain " style="width:790px">
        <div id="editGrid" style=" position:relative;padding:0;margin:0; overflow-y:scroll;width:790px;height:450px" >
            <table cellpadding="0" cellspacing="0"  style="width:770px" > 
                <tr>      
                    <th class="fixCol">編輯</th>          
                    <th class="fixCol nonSpace">收票日期</th>
                    <th class="fixCol nonSpace">票據號碼</th>
                    <th class="nonSpace">票據金額</th>
                    <th class="nonSpace">到期日期</th>
                    <th class="nonSpace">預估兌現日</th>
                    <th class="nonSpace">票據種類</th>                    
                    <th>銀行代碼</th>
                    <th>銀行名</th>
                    <th>發票人</th>
                    <th>帳號</th>
                    <th>背書人</th>
                    <th>備註</th>                    
                    <th>業管課簽收</th>  
                    <th>業管課簽收日</th>  
                    <th>資金課簽收</th>  
                    <th>資金課簽收日</th>  
                </tr>   
                <asp:Repeater runat="server" ID="rptEdit_N"  > 
                    <ItemTemplate>
                        <tr > 
                            <td class="fixCol">&nbsp</td>
                            <td class="fixCol"><%# Eval("REC_DATE") %></td> 
                            <td class="fixCol"><%# Eval("PAPER_NBR") %></td>
                            <td class="number"><%# Eval("PAPER_AMT", "{0:###,###,###,##0}")%></td>
                            <td><%# Eval("DUE_DAT") %></td> 
                            <td><%# Eval("PRJ_CASH_DATE") %></td> 
                            <td><%# Eval("KIND_NAME") %></td>
                            <td><%# Eval("BANK_NO") %></td> 
                            <td><%# Eval("BANK_NAME") %></td>
                            <td><%# Eval("INV_NAME") %></td>
                            <td><%# Eval("ACCOUNT") %></td>
                            <td><%# Eval("ENDORSOR") %></td>
                            <td><%# Eval("REMARK") %></td>    
                            <td><%# Eval("SIGN1") %></td>    
                            <td><%# Eval("SIGN1_DATE") %></td>    
                            <td><%# Eval("SIGN2") %></td>    
                            <td><%# Eval("SIGN2_DATE") %></td>    
                        </tr>
                    </ItemTemplate> 
                </asp:Repeater>     
                <asp:Repeater runat="server" ID="rptEdit"  > 
                    <ItemTemplate>
                        <tr style="display:<%# Eval("Status").ToString()=="D"?"none":"" %>" > 
                            <td class="fixCol">
                                <asp:Button ID="btnDel" runat="server"  cssClass="button del"  Text="刪除"  OnCommand="GridFunc_Click"   CommandName='<%# Eval("PAPER_NBR") %>' />                            
                                <asp:HiddenField runat="server" ID="Status"  Value='<%# Eval("Status") %>' />
                                <asp:HiddenField runat="server" ID="oldPAPER_NBR"  Value='<%# Eval("oldPAPER_NBR") %>' /></td>
                            <td class="fixCol"><ocxControl:ocxDate runat="server" ID="REC_DATE" Text='<%# Eval("REC_DATE") %>'  /> </td> 
                            <td class="fixCol"><asp:TextBox runat="server" ID="PAPER_NBR" Text='<%# Eval("PAPER_NBR") %>' MaxLength="20" Width="80"   ></asp:TextBox></td>
                            <td><ocxControl:ocxNumber runat="server" ID="PAPER_AMT" MASK="9,999,999" Text='<%# Eval("PAPER_AMT") %>' /></td>
                            <td><ocxControl:ocxDate runat="server" ID="DUE_DAT" Text='<%# Eval("DUE_DAT") %>'  /> </td> 
                            <td><ocxControl:ocxDate runat="server" ID="PRJ_CASH_DATE" Text='<%# Eval("PRJ_CASH_DATE") %>'  /></td> 
                            <td>
                                <asp:DropDownList ID="CHEQ_KIND" runat="server"  Width="70" OnPreRender='checkList' ToolTip='<%# Eval("CHEQ_KIND") %>' >
                                    <asp:ListItem Value="1">1.支票</asp:ListItem>
                                    <asp:ListItem Value="2">2.匯票</asp:ListItem>
                                    <asp:ListItem Value="3">3.客票</asp:ListItem>
                                </asp:DropDownList>    
                            </td>
                            <td><ocxControl:ocxDialog runat="server" ID="BANK_NO" width="100" Text='<%# Eval("BANK_NO") %>' ControlID="BANK_NAME" FieldName="BANK_NAME"  SourceName="ACC18" /></td> 
                            <td><asp:TextBox runat="server" ID="BANK_NAME" Text='<%# Eval("BANK_NAME") %>' Width="150"  CssClass="display" ></asp:TextBox></td>
                            <td><asp:TextBox runat="server" ID="INV_NAME" Text='<%# Eval("INV_NAME") %>'  Width="150"></asp:TextBox></td>
                            <td><asp:TextBox runat="server" ID="ACCOUNT" Text='<%# Eval("ACCOUNT") %>'  Width="150" ></asp:TextBox></td>
                            <td><asp:TextBox runat="server" ID="ENDORSOR" Text='<%# Eval("ENDORSOR") %>'  Width="80"   ></asp:TextBox></td>
                            <td><asp:TextBox runat="server" ID="REMARK" Text='<%# Eval("REMARK") %>' Width="150"   ></asp:TextBox></td>
                            <td><%# Eval("SIGN1") %></td>    
                            <td><%# Eval("SIGN1_DATE") %></td>    
                            <td><%# Eval("SIGN2") %></td>    
                            <td><%# Eval("SIGN2_DATE") %></td>    
                        </tr>
                    </ItemTemplate> 
                </asp:Repeater>                       
                <tr >
                    <td class="fixCol"><asp:Button ID="btnAdd" runat="server"  cssClass="button dadd"  Text="新增" OnCommand="GridFunc_Click" CommandName="Add" /> </td>
                    <td class="fixCol"><ocxControl:ocxDate runat="server" ID="addREC_DATE"   /> </td> 
                            <td class="fixCol"><asp:TextBox runat="server" ID="addPAPER_NBR"  MaxLength="20" Width="80"  ></asp:TextBox></td>
                            <td><ocxControl:ocxNumber runat="server" ID="addPAPER_AMT" MASK="9,999,999" /></td>
                            <td><ocxControl:ocxDate runat="server" ID="addDUE_DAT"   /> </td> 
                            <td><ocxControl:ocxDate runat="server" ID="addPRJ_CASH_DATE"  /></td> 
                            <td>
                                <asp:DropDownList ID="addCHEQ_KIND" runat="server"  Width="70" >
                                    <asp:ListItem Value="1">1.支票</asp:ListItem>
                                    <asp:ListItem Value="2">2.匯票</asp:ListItem>
                                    <asp:ListItem Value="3">3.客票</asp:ListItem>
                                </asp:DropDownList>    
                            </td>
                            <td><ocxControl:ocxDialog runat="server" ID="addBANK_NO" width="100"  ControlID="addBANK_NAME" FieldName="BANK_NAME"  SourceName="ACC18" /></td> 
                            <td><asp:TextBox runat="server" ID="addBANK_NAME" Width="150" CssClass="display" ></asp:TextBox></td>
                            <td><asp:TextBox runat="server" ID="addINV_NAME" Width="150" ></asp:TextBox></td>
                            <td><asp:TextBox runat="server" ID="addACCOUNT" Width="150"   ></asp:TextBox></td>
                            <td><asp:TextBox runat="server" ID="addENDORSOR"  Width="80" ></asp:TextBox></td>
                            <td><asp:TextBox runat="server" ID="addREMARK"  Width=150   ></asp:TextBox></td>
                            <td></td>          
                            <td></td>          
                            <td></td>          
                            <td></td>          
                </tr>
            </table>
        </div>     
    </div> 
    
  <script language="javascript" type="text/javascript">
  Date.prototype.addDays = function (num) {
    var value = this.valueOf();
    value += 86400000 * num;
    return new Date(value);
}

  function DateDo(obj,val)
  {
   
   
    if (obj.id.indexOf("DUE_DAT")!=-1){
        var dt = val.replace(/\//g, "").replace(/_/g, "");

        var y = dt.substr(0, 4);
        var m = dt.substr(4, 2);
        var d = dt.substr(6, 2);

        var due =  new Date(y + "/" + m + "/" + d);
        var prj = new Date();

        prj=due.addDays(10);

        document.getElementById(obj.id.toString().replace("DUE_DAT","PRJ_CASH_DATE")).value=dateToYMD(prj);
        
    } 
  }
  </script>
</asp:Content> 

<asp:Content ContentPlaceHolderID="PopWindow" runat="server" ID="myPopWindow">


    
<table border="0" cellpadding="2" cellspacing="1"  class="tbInside" style="width:760px">      
     <tr>
        <td class="PopupTitle" >        
            取得契約票據                             
        </td>
    </tr>
</table>
<div class="searchArea">
    <table cellpadding="2" cellspacing="2" style="width:760px" >
        <tr>
            <td>
                <table >
                    <tr>
                        <th class="nonSpace">申請書編號：</th><td><ocxControl:ocxDialog SourceName="OR_RECV_PAPER" runat="server" ID="GAPLY_NO" width="140" /></td>                       
                    </tr>                        
                </table>
                
            </td>
            <td style="text-align:right;">
                
                <div class="divButton">
                <asp:Button runat="server" ID="btnQry" CssClass="button qry" Text="查詢" OnCommand="Function_Click" CommandName="GetDetail"/>
                </div>            
                          
            </td>
        </tr>        
    </table>    
</div> 
<asp:UpdatePanel runat="server" ID="upGetDetail" RenderMode="Inline" UpdateMode="Conditional">
    <ContentTemplate>
    
    
         <div class="editingArea" >
        <div id="query" class="gridMain ">
            <div id="divGrid" style="padding:0;margin:0; position:relative; overflow-y:scroll;width:760px;height:300px" runat="server">
            
            <table cellpadding="0" cellspacing="0"  style="width:740px" >            
                <tr>      
                    <th style="z-index:1500">選取</th>                              
                    <th>收票日期</th>
                    <th>票據號碼</th>  
                    <th>票據金額</th>  
                    <th>到期日期</th>
                    <th>預估兌現日</th>
                    <th>票據種類</th>                    
                    <th>銀行代碼</th>
                    <th>銀行名</th>
                    <th>發票人</th>
                    <th>帳號</th>                   
                    <th>背書人</th> 
                    <th>備註</th>                           
                </tr> 
                                <asp:Repeater runat="server" ID="rptDialog"  > 
                    <ItemTemplate>
                        <tr class="<%#Container.ItemIndex%2==0?"srow":"" %>"> 
                        <td><input type="checkbox" name="chkPAPER" value='<%# Eval("PAPER_NBR") %>' /></td>
                            <td><%# Eval("REC_DATE") %></td> 
                            <td><%# Eval("PAPER_NBR") %></td>
                            <td class="number"><%# Eval("PAPER_AMT", "{0:###,###,###,##0}")%></td>
                            <td><%# Eval("DUE_DAT") %></td> 
                            <td><%# Eval("PRJ_CASH_DATE") %></td> 
                            <td><%# Eval("KIND_NAME") %></td>
                            <td><%# Eval("BANK_NO") %></td> 
                            <td><%# Eval("BANK_NAME") %></td>
                            <td><%# Eval("INV_NAME") %></td>
                            <td><%# Eval("ACCOUNT") %></td>
                            <td><%# Eval("ENDORSOR")%></td>
                            <td><%# Eval("REMARK") %></td>                                    
                        </tr>
                    </ItemTemplate> 
                </asp:Repeater>
            </table>
        </div>     
    </div> 
    </div>
    
    
    </ContentTemplate>
</asp:UpdatePanel>


<div class="divButton" style="float:right;">
    <asp:Button runat="server" ID="btnReady" CssClass="button exit" Text="確認" OnCommand="Function_Click" CommandName="Ready" />
</div>  


<script language="javascript" type="text/javascript">
    var obj=document.getElementById("divPopWindow");
    obj.style.width="780px";
    obj.style.height="500px";
    obj.style.top="10px";
    obj.style.left="5px";
    
</script>
</asp:Content>
