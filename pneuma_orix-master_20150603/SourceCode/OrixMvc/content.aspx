<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="content.aspx.cs" Inherits="OrixMvc.content" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link  href="Style/content.css" type="text/css" rel="stylesheet" />
    <title>未命名頁面</title>
</head>
<body>
    <form id="form1" runat="server">
    <script type='text/javascript' src='ClientSrc/resizeGrid.js'></script>

    <div class="searchArea">
    <table cellpadding="2" cellspacing="2">
        <tr>
            <td>
                <table>
                    <tr>
                        <th>功能代碼：</th><td><asp:TextBox runat="server" ID="txtProgID" size="10"></asp:TextBox></td>
                        <th>類型區分：</th><td><asp:TextBox runat="server" ID="txtType"  size="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>功能名稱：</th><td colspan="3" ><asp:TextBox runat="server" ID="TextBox1" size="30"></asp:TextBox></td>
                    </tr>
                </table>
            </td>
            <td style="text-align:right; vertical-align:bottom">
                <asp:Button runat="server" ID="btnAdd" CssClass="button add" Text=" + 新增"  /><br />
                <asp:Button runat="server" ID="btnQry" CssClass="button qry" Text="查詢"  />
            </td>
        </tr>        
    </table>
    
    </div>
    
    <div class="gridArea">
       <div class="gridMain">
       <table cellpadding="0" cellspacing="0" class="title" >
       <tr>
        <th style="text-align:left;padding-left:5px" width="50%"><img src="Images/excel.png" /></th>
        <th><img src="Images/first.png" /><img src="Images/prev.png" /></th>
        <th>Page</th><th> <asp:textbox runat="server" ID="goPage" CssClass="goPage" Width="27px"></asp:textbox></th><th> of 10</th>
        <th><img src="Images/next.png" /><img src="Images/last.png" /></th>
        <th>
            <asp:DropDownList runat="server" ID="cntPage" >
            <asp:ListItem Value=1>10</asp:ListItem>
            <asp:ListItem Value=1>20</asp:ListItem>
            <asp:ListItem Value=1>30</asp:ListItem>
            <asp:ListItem Value=1>40</asp:ListItem>
            </asp:DropDownList> 
        </th>
        <th style="padding-left:5px">View 1 - 10 of 105</th></tr>
       </table> 
        <table cellpadding="0" cellspacing="0" class="resizable">            
            <tr>
                <th width="20px" class="seq fixCol"></th>
                <th width="110px" >編輯</th>
                <th>功能代碼</th>
                <th>功能名稱</th>
                <th>類型區分</th>
            </tr>
            <tr>
                <td class="seq fixCol">1</td>
                <td>
                    <input type="button" class="button upd" value="修改" id="upd"  />                            
                        <asp:Button runat="server" ID="btnDel1" CssClass="button del" Text="刪除" OnClientClick="closeUpd();return window.confirm('是否確定刪除此筆資料?')"   />
                </td>
                <td>GroupA A</td>
                <td>GroupA A Name</td>
                <td>子系統</td>
            </tr>
            <tr class="srow">
                <td class="seq fixCol">2</td>
                <td>
                    <input type="button" class="button upd" value="修改" id="Button1"  />                            
                        <asp:Button runat="server" ID="Button21" CssClass="button del" Text="刪除" OnClientClick="closeUpd();return window.confirm('是否確定刪除此筆資料?')"   />
                </td>
                <td>GroupA A</td>
                <td>GroupA A Name</td>
                <td>子系統</td>
            </tr>
            <tr>
                 <td class="seq fixCol">3</td>
                <td>
                    <input type="button" class="button upd" value="修改" id="Button2"  />                            
                        <asp:Button runat="server" ID="Button3" CssClass="button del" Text="刪除" OnClientClick="closeUpd();return window.confirm('是否確定刪除此筆資料?')"   />
                </td>
                <td>GroupA A</td>
                <td>GroupA A Name</td>
                <td>子系統</td>
            </tr>
            <tr class="srow">
                 <td class="seq fixCol">4</td>
                <td>
                    <input type="button" class="button upd" value="修改" id="Button4"  />                            
                        <asp:Button runat="server" ID="Button5" CssClass="button del" Text="刪除" OnClientClick="closeUpd();return window.confirm('是否確定刪除此筆資料?')"   />
                </td>
                <td>GroupA A</td>
                <td>GroupA A Name</td>
                <td>子系統</td>
            </tr>
        </table>
        
       </div> 
        <!--
        <div class="gridMain" >
        <div class="grid" > 
            <div class="gridHead">
                <div class="hrow">
                    <div class="cell seq" style="width: 20px;"></div>
                    <div class="cell edit" style="width: 120px;">編輯 </div>                    
                    <div class="cell" style="width: 120px;">功能代碼
                        <div class="sort def" onclick="sorting(this,'Dept_Code')"><a></a></div>
                    </div>
                    <div class="cell" style="width: 250px;">功能名稱
                       <div class="sort def" onclick="sorting(this,'Dept_Name')"> <a></a></div> 
                    </div>         
                    <div class="cell" style="width: 120px;">類型區分
                       <div class="sort def" onclick="sorting(this,'Dept_Name')"> <a></a></div> 
                    </div>
                </div>            
            </div>
            <div class="grid gridBody"> 
                <div class='row'>
                    <div class="cell seq" style="width: 20px;">1</div>
                    <div class="cell edit"  style="width: 120px;">
                        <input type="button" class="button upd" value="修改" id="upd"  />                            
                        <asp:Button runat="server" ID="btnDel" CssClass="button del" Text="刪除" OnClientClick="closeUpd();return window.confirm('是否確定刪除此筆資料?')"   />
                    </div>                      
                    <div class="cell" style="width: 120px;">GroupA A</div>
                    <div class="cell" style="width: 250px;">GroupA A Name </div>
                    <div class="cell" style="width: 120px;">子系統</div>                             
                </div> 
                <div class='srow'>
                    <div class="cell seq" style="width: 20px;">2</div>
                    <div class="cell edit"  style="width: 120px;">
                        <input type="button" class="button upd" value="修改" id="Button1"  />                            
                        <asp:Button runat="server" ID="Button2" CssClass="button del" Text="刪除" OnClientClick="closeUpd();return window.confirm('是否確定刪除此筆資料?')"   />
                    </div> 
                    <div class="cell" style="width: 120px;">GroupA A</div>
                    <div class="cell" style="width: 250px;">GroupA A Name </div>
                    <div class="cell" style="width: 120px;">子系統</div>                             
                </div> 
                <div class='row'>
                    <div class="cell seq" style="width: 20px;">3</div>
                    <div class="cell edit"  style="width: 120px;">
                        <input type="button" class="button upd" value="修改" id="Button3"  />                            
                        <asp:Button runat="server" ID="Button4" CssClass="button del" Text="刪除" OnClientClick="closeUpd();return window.confirm('是否確定刪除此筆資料?')"   />
                    </div>                      
                    <div class="cell" style="width: 120px;">GroupA A</div>
                    <div class="cell" style="width: 250px;">GroupA A Name </div>
                    <div class="cell" style="width: 120px;">子系統</div>                             
                </div> 
                <div class='srow'>
                    <div class="cell seq" style="width: 20px;">4</div>
                    <div class="cell edit"  style="width: 120px;">
                        <input type="button" class="button upd" value="修改" id="Button5"  />                            
                        <asp:Button runat="server" ID="Button6" CssClass="button del" Text="刪除" OnClientClick="closeUpd();return window.confirm('是否確定刪除此筆資料?')"   />
                    </div> 
                    <div class="cell" style="width: 120px;">GroupA A</div>
                    <div class="cell" style="width: 250px;">GroupA A Name </div>
                    <div class="cell" style="width: 120px;">子系統</div>                             
                </div> 
            </div>       
            <div class="grid gridFooter"></div> 
        </div> 
    </div>!-->
    </div>
    
    </form>
</body>
</html>

