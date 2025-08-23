<%@ Page Title="" Language="C#" MasterPageFile="~/FAQ/FAQMaster.master" AutoEventWireup="true" CodeFile="FaqServiceDetails.aspx.cs" Inherits="FAQ_FaqServiceDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="wrapper col2">
        <div id="topbar">
            <div id="topnav">
                 <ul>
                    <li class="active"><a href="FAQ.aspx">HOME</a></li>
                    <li><a href="../Login.aspx">GO TO LOGIN PAGE</a></li>
                </ul>
                <%-- <asp:HyperLink ID="hlinkGoBack" runat="server" style="width:5px;height:3px" ImageUrl="~/Images/go-back.png" NavigateUrl="~/FAQ/FAQ.aspx" ></asp:HyperLink>--%>
            </div>
            <div id="search">
            </div>
            <br class="clear" />
        </div>
    </div>
    <div class="wrapper col3">
  <div id="breadcrumb">
    <ul>
        <li><a href="FAQ.aspx">Back</a></li>
        <li>&nbsp;&nbsp;</li>
        <li>&#187;</li>
        <li> <strong>Service:
        <asp:Label ID="lblservice" runat="server"></asp:Label></strong></li>
        </ul>
        </div>
        </div>
    <div class="wrapper col5">
        <div id="container">
        <div id="content">
        <table summary="Summary Here" style="width:200%" cellpadding="0" cellspacing="0">
        <thead>
          <tr>
            <th ><asp:Label ID="lblTitle" runat="server" Text="Services" Style="font-family: 'Tangerine', serif; font-size: 14px; white-space: normal"></asp:Label>
           <asp:Label ID="lblserviceId" runat="server" Visible="false"></asp:Label></th>
          </tr>
        </thead>
         </table>
                <p>
                    <asp:GridView ID="GrvDiscriptiopn" runat="server" AutoGenerateColumns="False"
                        Width="200%" PagerStyle-CssClass="pgr" Style="white-space: normal; font-family: Calibri (Body);padding:12px"
                        CellPadding="4" 
                        AllowPaging="True" AllowSorting="True" PageSize="20"  >
                        <Columns>

                            <asp:TemplateField  >
                                <ItemTemplate>
                                    <asp:Label ID="lblTitle" runat="server" Style="font-family: 'Tangerine', serif; font-size: 16px"
                                        Text='<%#Eval("Description") %>'></asp:Label>
                                </ItemTemplate>

                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </p>
            </div>

            <br class="clear" />
             <h1>Contact Person :</h1>
              
             <asp:GridView ID="GrvfaqContactPerson" Style="white-space: normal; font-family: Calibri (Body)" runat="server" AutoGenerateColumns="False"
                            Width="100%" PagerStyle-CssClass="pgr"
                            CssClass="table" CellPadding="4"
                            AllowPaging="True" AllowSorting="false" PageSize="20">
                            <Columns>
                                <asp:BoundField DataField="ContactPerName" HeaderText="Contact Person Name"  HeaderStyle-ForeColor="#2684B7" SortExpression="ContactPerName" />
                                <asp:BoundField DataField="BranchName" HeaderText="Branch" HeaderStyle-ForeColor="#2684B7" SortExpression="BranchName" />
                                <asp:BoundField DataField="ContactPerEmailId" HeaderText="Email" HeaderStyle-ForeColor="#2684B7" SortExpression="ContactPerEmailId" />
                                <asp:BoundField DataField="ContactPerPhoneNo" HeaderText="Phone No" HeaderStyle-ForeColor="#2684B7" SortExpression="ContactPerPhoneNo" />
                            </Columns>
                        </asp:GridView>
            <br />

            <h1>Download Documents :</h1>
             
             <asp:GridView ID="GridFaqDocDownload" runat="server" AutoGenerateColumns="false"
            OnRowDataBound="GridFaqDocDownload_RowDataBound" style="text-align:center"
            OnRowCommand="GridFaqDocDownload_RowCommand" CellPadding="4"
            AllowPaging="True" AllowSorting="True" Width="100%" PageSize="20">
            <Columns>
              
            <asp:TemplateField HeaderText="Download Document">
            <ItemTemplate>
                   
                <asp:LinkButton ID="lnkDownload" runat="server"  Text="Document" CommandName="download"
                    CommandArgument='<%#Eval("DocPath") %>' Visible="true" ToolTip="Download Document"> </asp:LinkButton>

            </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Download Form">
            <ItemTemplate>

                   
                <asp:LinkButton ID="lnkDownload1" runat="server"   Text="Form" CommandName="download1"
                    CommandArgument='<%#Eval("DocFormPath") %>' Visible="true" ToolTip="Download Form"></asp:LinkButton>

            </ItemTemplate>
            </asp:TemplateField>
       
            </Columns>
            </asp:GridView>
               
            
               
                
           <%-- <h1>Download Form :</h1>
            <br class="clear" />--%>

               <%--<asp:GridView ID="GrvDocumentForm" runat="server" AutoGenerateColumns="false"
            OnRowCommand="GrvDocumentForm_RowCommand"  CellPadding="4" style="text-align:center"
            AllowPaging="True" AllowSorting="True" Width="282%" PageSize="20" OnRowDataBound="GrvDocumentForm_RowDataBound">
            <Columns>
            <asp:TemplateField  HeaderText="Download">
            <ItemTemplate>

                <asp:LinkButton ID="lnkDownload" runat="server" Text="Download Document Form" CommandName="download"
                    CommandArgument='<%#Eval("DocFormPath1") %>' Visible="true" ToolTip="Download Form"></asp:LinkButton>

            </ItemTemplate>
            </asp:TemplateField>
            <%--<asp:TemplateField HeaderText="Download">
            <ItemTemplate>

                <asp:LinkButton ID="lnkDownload1" runat="server" Text="Download Doc Form 2" CommandName="download1"
                    CommandArgument='<%#Eval("DocFormPath2") %>' Visible="true" ToolTip="Download"></asp:LinkButton>

            </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Download">
            <ItemTemplate>

                <asp:LinkButton ID="lnkDownload2" runat="server" Text="Download Doc Form 3" CommandName="downloadExcise"
                    CommandArgument='<%#Eval("DocFormPath3") %>' ToolTip="Download"></asp:LinkButton>
            </ItemTemplate>
            </asp:TemplateField>--%>
           <%-- </Columns>
            </asp:GridView>--%>
                </div>
               
                
             <br class="clear" />
             <br class="clear" />
           
            </div>

         <div class="wrapper col6" style="background-color:white;height:20px">
            <div id="footer">
                 
           
                <br class="clear" />
               
            </div>
        </div>
    </div>
   



</asp:Content>

