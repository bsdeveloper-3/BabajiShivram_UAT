<%@ Page Title="Invoice Tracking" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="InvoiceTracking.aspx.cs" Inherits="InvoiceTrack_InvoiceTracking" Culture="en-GB" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter1" TagPrefix="uc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upJobDetail" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upJobDetail" runat="server">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server"></asp:Label>
            </div>
            <div class="clear"></div>
            <fieldset class="fieldset-AutoWidth"><legend>Vendor Invoice Tracking</legend>
            <div class="clear"></div>
             <asp:Panel ID="pnlFilter1" runat="server">
                <div class="fleft">
                    <table>
                        <tr>
                            <td>
                                <uc1:DataFilter1 ID="DataFilter1" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
           
             <asp:GridView ID="gvInvoiceDetail" runat="server" AutoGenerateColumns="False" CssClass="table" 
                    DataKeyNames="lid" AllowPaging="True" AllowSorting="True" Width="100%" PageSize="20"
                    PagerSettings-Position="TopAndBottom" DataSourceID="SqlDataSourceTracking" OnPreRender="gvInvoiceDetail_PreRender">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Token No" SortExpression="TokanNo">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkTokanNo" runat="server" Text='<%#Eval("TokanNo") %>' CommandName="select"
                                    CommandArgument='<%#Eval("lid") %>' Enabled="false"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="FARefNo" HeaderText="Ref No" SortExpression="FARefNo"
                            ReadOnly="true" />
                        <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName"
                            ReadOnly="true" />
                        <asp:BoundField DataField="VendorName" HeaderText="Vendor" SortExpression="VendorName"
                            ReadOnly="true" />    
                        <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" SortExpression="InvoiceNo"
                            ReadOnly="true" />
                        <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" DataFormatString="{0:dd/MM/yyyy}"
                            SortExpression="InvoiceDate" ReadOnly="true" />
                        <asp:BoundField DataField="GSTNo" HeaderText="GST No" SortExpression="GSTNo" ReadOnly="true" />
                        <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount" SortExpression="TotalAmount" ReadOnly="true" />
                        <asp:BoundField DataField="TaxAmount" HeaderText="TaxAmount" SortExpression="TaxAmount" ReadOnly="true" />
                        <asp:BoundField DataField="CGST" HeaderText="CGST" SortExpression="CGST" ReadOnly="true" />
                        <asp:BoundField DataField="SGST" HeaderText="SGST" SortExpression="SGST" ReadOnly="true" />
                        <asp:BoundField DataField="IGST" HeaderText="IGST" SortExpression="IGST" ReadOnly="true" />
                        <asp:BoundField DataField="BillTypeName" HeaderText="Invoice Type" SortExpression="BillTypeName" ReadOnly="true" />
                        <asp:BoundField DataField="BranchName" HeaderText="Branch" SortExpression="BranchName" ReadOnly="true" />
                        <asp:BoundField DataField="UserName" HeaderText="User" SortExpression="UserName" ReadOnly="true" />
                    </Columns>
                    <PagerTemplate>
                    <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
           
            </fieldset>
                <asp:SqlDataSource ID="SqlDataSourceTracking" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="INV_GetInvoiceByStatus" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                    </SelectParameters>
                </asp:SqlDataSource>
        </ContentTemplate>
             
    </asp:UpdatePanel>
</asp:Content>

