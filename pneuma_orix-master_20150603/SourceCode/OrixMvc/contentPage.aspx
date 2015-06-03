<%@ Page Language="C#"  AutoEventWireup="true" CodeBehind="contentPage.aspx.cs" Inherits="OrixMvc.contentPage" %>



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
               
            </td>
            <td style="text-align:right; vertical-align:bottom">
                <asp:Button runat="server" ID="btnAdd" CssClass="button add" Text=" + 新增"  /><br />
                <asp:Button runat="server" ID="btnQry" CssClass="button qry" Text="查詢"  />
            </td>
        </tr>        
    </table>
    
    </div>
    <style type="text/css">

    </style>

        
       
       <div id="editGrid" class="gridMain" style="width:300px; height:150px; overflow:scroll; position:relative; padding:0">
        <table cellpadding="0" cellspacing="0" style="width:100%" >            
            <tr>
                <th class="seq fixCol">No</th>
                <th  class="fixCol" >編輯</th>
                <th >功能代碼</th>
                <th>功能名稱</th>
                <th >類型區分</th>
            </tr>
            <tr>
                <td class="seq fixCol">1</td>
                <td class="fixCol">
                    <input type="button" class="button upd" value="修改" id="upd"  />                            
                        <asp:Button runat="server" ID="btnDel1" CssClass="button del" Text="刪除" OnClientClick="return window.confirm('是否確定刪除此筆資料?')"   />
                </td>
                <td>GroupA A</td>
                <td>GroupA A Name</td>
                <td>子系統</td>
            </tr>
            <tr class="srow">
                <td class="seq fixCol">2</td>
                <td class="fixCol">
                    <input type="button" class="button upd" value="修改" id="Button1"  />                            
                        <asp:Button runat="server" ID="Button21" CssClass="button del" Text="刪除" OnClientClick="return window.confirm('是否確定刪除此筆資料?')"   />
                </td>
                <td>GroupA A</td>
                <td>GroupA A Name nGroupA A NameGroupA A A Name nGroupA A NameGroupA A </td>
                <td>子系統</td>
            </tr>
            <tr>
                 <td class="seq fixCol">3</td>
                <td class="fixCol">
                    <input type="button" class="button upd" value="修改" id="Button2"  />                            
                        <asp:Button runat="server" ID="Button3" CssClass="button del" Text="刪除" OnClientClick="return window.confirm('是否確定刪除此筆資料?')"   />
                </td>
                <td>GroupA A</td>
                <td>GroupA A Name GroupA A Name GroupA A Name</td>
                <td>子系統</td>
            </tr>
             <tr>
                 <td class="seq fixCol">3</td>
                <td class="fixCol">
                    <input type="button" class="button upd" value="修改" id="Button6"  />                            
                        <asp:Button runat="server" ID="Button7" CssClass="button del" Text="刪除" OnClientClick="return window.confirm('是否確定刪除此筆資料?')"   />
                </td>
                <td>GroupA A</td>
                <td>GroupA A Name GroupA A Name GroupA A Name</td>
                <td>子系統</td>
            </tr>
             <tr>
                 <td class="seq fixCol">3</td>
                <td class="fixCol">
                    <input type="button" class="button upd" value="修改" id="Button8"  />                            
                        <asp:Button runat="server" ID="Button9" CssClass="button del" Text="刪除" OnClientClick="return window.confirm('是否確定刪除此筆資料?')"   />
                </td>
                <td>GroupA A</td>
                <td>GroupA A Name GroupA A Name GroupA A Name</td>
                <td>子系統</td>
            </tr>
             <tr>
                 <td class="seq fixCol">3</td>
                <td class="fixCol">
                    <input type="button" class="button upd" value="修改" id="Button10"  />                            
                        <asp:Button runat="server" ID="Button11" CssClass="button del" Text="刪除" OnClientClick="return window.confirm('是否確定刪除此筆資料?')"   />
                </td>
                <td>GroupA A</td>
                <td>GroupA A Name GroupA A Name GroupA A Name</td>
                <td>子系統</td>
            </tr>
            <tr class="srow">
                 <td class="seq fixCol">4</td>
                <td class="fixCol">
                    <input type="button" class="button upd" value="修改" id="Button4"  />                            
                        <asp:Button runat="server" ID="Button5" CssClass="button del" Text="刪除" OnClientClick="return window.confirm('是否確定刪除此筆資料?')"   />
                </td>
                <td>GroupA A</td>
                <td>GroupA A Name</td>
                <td>子系統 子系統 子系統子系統</td>
            </tr>
        </table>
        </div>


    
    </form>
</body>
</html>

