<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PCABillingAdvice.aspx.cs" Inherits="PCA_PCABillingAdvice" 
    MasterPageFile="~/MasterPage.master" Title="PCA Billing Advice" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    
    <div align="center">
        <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" />
    </div>
    <div class="clear"></div>
        <cc1:TabContainer ID="TabJobDetail" runat="server" ActiveTabIndex="0" CssClass="Tab">
            <cc1:TabPanel ID="TabPCDDocument" runat="server" HeaderText="Document">
            <ContentTemplate>

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
    
    </ContentTemplate>
        </cc1:TabPanel>
        <%--start Expense Details--%>
            <cc1:TabPanel runat="server" ID="TabPanel1" HeaderText="Expense Details">
                <ContentTemplate>
                    <div style="overflow: scroll;">
                    <fieldset class="fieldset-AutoWidth">
			<legend>Expense Details</legend>
                        <asp:GridView ID="gvjobexpenseDetail" runat="server" AutoGenerateColumns="False" ShowFooter="true"
                                CssClass="table" Width="99%" PagerStyle-CssClass="pgr"  DataSourceID="DataSourceExpenseDetail"
                                CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40" onrowdatabound="gvjobexpenseDetail_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField HeaderText="Scrutiny Completed By" DataField="CREDITAMT"/>--%>
                                    <asp:BoundField HeaderText="VCHDATE" DataField="VCHDATE" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />                                        
                                    <asp:BoundField HeaderText="VCHNO" DataField="VCHNO" />
                                    <asp:BoundField HeaderText="CONTRANAME" DataField="CONTRANAME" />                                        
                                    <asp:BoundField HeaderText="CHQNO" DataField="CHQNO" />
                                    <asp:BoundField HeaderText="CHQDATE" DataField="CHQDATE" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    <asp:BoundField HeaderText="DEBITAMT" DataField="DEBITAMT" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField HeaderText="CREDITAMT" DataField="CREDITAMT" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField HeaderText="AMOUNT" DataField="AMOUNT" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField HeaderText="NARRATION" DataField="NARRATION"/>                                 
                                </Columns>
				<FooterStyle HorizontalAlign="Right" Font-Bold="true" />
                            </asp:GridView>                            
                        </fieldset>
                        <div id="div2">
                            <asp:SqlDataSource ID="DataSourceExpenseDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="GetBJVDetails" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="JobId" />                                
                                </SelectParameters>                                  
                            </asp:SqlDataSource>
                        </div>
                    </div>                         
                </ContentTemplate>
            </cc1:TabPanel>
            <%--end Expense Details--%>    
        </cc1:TabContainer>    
</asp:Content>
