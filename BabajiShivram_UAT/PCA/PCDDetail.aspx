<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PCDDetail.aspx.cs" Inherits="PCDDetail"
 MasterPageFile="~/MasterPage.master" Title="PCA To Customer Detail" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    
    
    <div align="center">
        <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" />
    </div>
    
    <cc1:TabContainer ID="TabJobDetail" runat="server" ActiveTabIndex="0" CssClass="Tab">
            <cc1:TabPanel ID="TabPCDBilling" runat="server" HeaderText="Billing Detail" Visible="false">
                <ContentTemplate>
               
          <div class="m clear">
            <asp:Button ID="btnSubmit" CssClass="btn" Text="Save" runat="server" ValidationGroup="Required" />
            <asp:Button ID="btnCancel" Text="Cancel" CssClass="btn" CausesValidation="false" runat="server" OnClick="btnCancel_Click" />
        </div>          
        <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
            <tr>
                <td>
                    BS Job No.
                </td>
                <td>
                    <asp:Label ID="lblJobRefNo" runat="server"></asp:Label>
                </td>
                <td>
                    Customer Name
                </td>
                <td>
                    <asp:Label ID="lblCustName" runat="server"></asp:Label>
                </td>
            </tr>
            
       </table> 
        </ContentTemplate>
        </cc1:TabPanel>
            <cc1:TabPanel ID="TabPCDDocument" runat="server" HeaderText="Document">
            <ContentTemplate>
                <div class="m clear"></div>
                <fieldset>
        <legend>Back Office Document</legend>
                    <asp:GridView ID="gvPCDDocument" runat="server" AutoGenerateColumns="False" CssClass="table"
                        Width="100%" PagerStyle-CssClass="pgr" DataKeyNames="lid" CellPadding="4" AllowPaging="True" 
                        AllowSorting="True" PageSize="20" DataSourceID="PCDDocumentSqlDataSource" 
                        OnRowDataBound="gvPCDDocument_RowDataBound" OnRowCommand="gvPCDDocument_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="DocumentName" HeaderText="PCA Document" SortExpression="DocumentName" />
                            <asp:BoundField DataField="IsCopy" HeaderText="Copy" SortExpression="IsCopy" />
                            <asp:BoundField DataField="IsOriginal" HeaderText="Original" SortExpression="IsOriginal" />
                            <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy" />
                            <asp:BoundField DataField="UploadedBy" HeaderText="Uploaded By" SortExpression="UploadedBy" />
                            <asp:BoundField DataField="UploadedDate" HeaderText="Uploaded Date" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" />
                            <asp:TemplateField HeaderText="Download">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                                        CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </fieldset>
                <div>
                    <asp:SqlDataSource ID="PCDDocumentSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="GetPCDDocumentByWorkFlow" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:SessionParameter Name="JobId" SessionField="JobId" />
                            <asp:Parameter Name="DocumentForType" DefaultValue="1" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
    </div>
    </ContentTemplate>
        </cc1:TabPanel>
            
        </cc1:TabContainer>
    
</asp:Content>
