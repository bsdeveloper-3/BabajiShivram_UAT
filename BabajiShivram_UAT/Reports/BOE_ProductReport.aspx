<%@ Page Title="" Language="C#" MasterPageFile="~/CustomerMaster.master" AutoEventWireup="true" CodeFile="BOE_ProductReport.aspx.cs" Inherits="Reports_BOE_ProductReport" %>

<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upBOEProduct" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upBOEProduct" runat="server" UpdateMode="Conditional"
        RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblMessage" runat="server" CssClass="info" Visible="false"></asp:Label>
            </div>
            <div class="clear">
            </div>

            <fieldset>
                <legend>BOE Product Report</legend>
                <div>
                    
                    <div class="fleft">
                        <uc1:DataFilter ID="DataFilter1" runat="server"  />
                    </div>
                    <div class="fleft">
                        <asp:LinkButton ID="lnkCustomerXls" runat="server" OnClick="lnkCustomerXls_Click">
                            <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                        </asp:LinkButton>&nbsp;
                    </div>
                </div>
                <div class="clear"></div>
                <asp:GridView ID="gvCustomerWiseJob" runat="server" AutoGenerateColumns="False" 
                    CssClass="table" DataSourceID="SqlDataSourceProduct"
                    AllowSorting="true">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Job Ref No" DataField="JobRefNo"  />
                        <asp:BoundField HeaderText="Customer" DataField="Customer"  />
                        <asp:BoundField HeaderText="Consignee" DataField="Consignee" SortExpression="Type" />
                        <asp:BoundField HeaderText="Invoice No" DataField="InvoiceNo" SortExpression="InvoiceNo" />
                        <asp:BoundField HeaderText="Invoice Date" DataField="InvoiceDate" SortExpression="InvoiceDate" />
                        <asp:BoundField HeaderText="Invoice Value" DataField="InvoiceValue" SortExpression="InvoiceValue" />
                        <asp:BoundField HeaderText="Terms Of Invoice" DataField="TermsOfInvoice" SortExpression="TermsOfInvoice" />
                        <asp:BoundField HeaderText="Quantity" DataField="Quantity" SortExpression="Quantity" />
                        <asp:BoundField HeaderText="Unit" DataField="UnitOfProduct" SortExpression="UnitOfProduct" />
                        <asp:BoundField DataField="UnitPrice" HeaderText="Unit Price" />
                        <asp:BoundField DataField="ExchangeRate" HeaderText="Exchange Rate" />
                        <asp:BoundField DataField="Description" HeaderText="Description" />
                        <asp:BoundField DataField="DutyAmount" HeaderText="Duty Amount" />
                        <asp:BoundField DataField="AssessableValue1" HeaderText="Assessable Value" />
                        <asp:BoundField DataField="BOENo" HeaderText="BOE No" />
                        <asp:BoundField DataField="BOEDate" HeaderText="BOE Date" />
                        <asp:BoundField DataField="ItemAmount" HeaderText="Item Amount" />
                        <asp:BoundField DataField="ItemAssessableValue" HeaderText="Item Assessable Value" />
                        <asp:BoundField DataField="ItemTotalDuty" HeaderText="Item Total Duty" />
                        <asp:BoundField DataField="BasicDutyRate" HeaderText="Basic Duty Rate%" />
                        <asp:BoundField DataField="BasicDutyAmount" HeaderText="Basic Duty Amt" />
                        <asp:BoundField DataField="AssessableValue" HeaderText="GST Assess Value" />
                        <asp:BoundField DataField="GSTDutyRate" HeaderText="GST Duty Rate%" />
                        <asp:BoundField DataField="IGSTAmount" HeaderText="GST Duty Amt" />
                        <asp:BoundField DataField="SocialWelfareSurchargeRate" HeaderText="Social Welfare Surcharge Rate %" />
                        <asp:BoundField DataField="SocialWelfareSurchargeAmt" HeaderText="Social Welfare Surcharge Amt" />
                        <asp:BoundField DataField="EduCessRate" HeaderText="Edu Cess Rate%" />
                        <asp:BoundField DataField="EduCessAmount" HeaderText="Edu Cess Amt" />
                        <asp:BoundField DataField="CTHNo" HeaderText="CTH" />
                        <asp:BoundField DataField="BasicDutyNotn" HeaderText="Basic Duty Notn" />
                        <asp:BoundField DataField="BasicDutyNotnSlNo" HeaderText="Basic Duty Notn Sl No" />
                        <asp:BoundField DataField="LicenseNo" HeaderText="Lic No" />
                        <asp:BoundField DataField="LicenseAmount" HeaderText="Lic Amt" />
                        <asp:BoundField DataField="LicenseQtyDebited" HeaderText="Lic Debit Qty" />
                    </Columns>
                </asp:GridView>
            </fieldset>
            <div>
                <<asp:SqlDataSource ID="SqlDataSourceProduct" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="BOEProductReport" SelectCommandType="StoredProcedure">
                     <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="CustId" />
                         <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

