<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ocxUpload.ascx.cs" Inherits="OrixMvc.ocxControl.ocxUpload" %> 
    <span style="display:none"><asp:TextBox runat="server" ID="txtUpload"></asp:TextBox></span>
    
    <table cellpadding=0 cellspacing=0  >
        <% if (this.bolUpload)
           {%>
        <tr><td align="right"><iframe  src="ocxControl/upload.aspx?Id=<%=this.btnImage.ClientID %>" frameborder=0 scrolling=no style="cursor:pointer;width:54px;height:22px;padding:2px 0px 0px 0px;" ></iframe> </td>
        <%} %>
        <td align="left">
            <span id="<%=this.btnImage.ClientID %>spanSeq" style='display:<%=this.Seq!=""?"":"none"%>'>
                           
                    <asp:Button runat="server" ID="view" Text="檢視" CssClass="button upload" OnClick="view_Click"  style='margin-left:5px;'  />
           <asp:UpdatePanel runat="server" ID="upImage" RenderMode="Inline" >
                <ContentTemplate>                      
                    <div style="display:none">
                    <asp:Button runat="server" ID="btnImage" OnClick="SaveImage" />     
                    </div>     
                </ContentTemplate>                
            </asp:UpdatePanel>    
            </span>
          
        </td>
        </tr>
    </table>
    
    <script language="javascript" type="text/javascript">
        
        
        
    </script>
        
    
 