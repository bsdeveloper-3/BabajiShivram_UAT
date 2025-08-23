<%@ Page Title="Vendor Payment Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CustomerExpenseReport.aspx.cs" 
        Inherits="AccountExpense_CustomerExpenseReport" EnableEventValidation="false" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:HiddenField ID="hdnCustomerId" runat="server" Value='<%#Eval("CustomerId") %>' />
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <script type="text/javascript">
        function OnCustomerSelected(source, eventArgs) {
            // alert(eventArgs.get_value());
            var results = eval('(' + eventArgs.get_value() + ')');
           
            $get('ctl00_ContentPlaceHolder1_hdnCustomerId').value = results.ClientId;
        }
      
    </script>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upInvoiceReport" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upInvoiceReport" runat="server" UpdateMode="Conditional"
        RenderMode="Inline">
        <ContentTemplate>
            
            <div align="center" style="vertical-align: top">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <div class="clear"></div>
            <fieldset><legend>Search Customer</legend>    
            <div>
                <div class="fleft">
                <asp:TextBox ID="txtCustomer" runat="server" ToolTip="Enter Customer Name." 
                    CssClass="SearchTextbox" placeholder="Search" style="width:300px;"></asp:TextBox>
                <div id="divwidthCust" runat="server">
                </div>
                <cc1:AutoCompleteExtender ID="CustomerExtender" runat="server" TargetControlID="txtCustomer"
                    CompletionListElementID="divwidthCust" ServicePath="~/WebService/CustomerAutoComplete.asmx"
                    ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCust"
                    ContextKey="1284" UseContextKey="True" OnClientItemSelected="OnCustomerSelected"
                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                </cc1:AutoCompleteExtender>
            </div>
                <div class="fleft" style="margin-left:30px;">
                    <asp:Button ID="btnShowReport" Text="Show Report" OnClick="btnShowReport_OnClick" runat="server" ValidationGroup="Required" />
                </div>
                <div class="fleft" style="margin-left:30px;">
                <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click" ValidationGroup="Required">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>
            </div>
            </div>
            <div class="m clear"></div>
            
            </fieldset>
            <fieldset><legend>Vendor Payment Report</legend>
                <asp:GridView ID="gvPaymentReport" runat="server" AutoGenerateColumns="False" CssClass="table" DataSourceID="DataSourcePayment">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="FARefNo" HeaderText="Job No"/>
                        <asp:BoundField DataField="ExpenseTypeName" HeaderText="Type" />
                        <asp:BoundField DataField="PaymentType" HeaderText="Payment Type" SortExpression="PaymentType" />
                        <%--<asp:BoundField DataField="BankName" HeaderText="Bank Name" SortExpression="BankName" />--%>
                        <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" />
                        <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                        <asp:BoundField DataField="BillToParty" HeaderText="Bill To Party" SortExpression="BillToParty" />
                        <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" SortExpression="VendorName" />
                        <asp:BoundField DataField="PaidAmount" HeaderText="Paid Amount" SortExpression="PaidAmount" />
                        <asp:BoundField DataField="AdvanceAmount" HeaderText="Advance" SortExpression="AdvanceAmount" />
                        <asp:BoundField DataField="InstrumentNo" HeaderText="UTR No" SortExpression="InstrumentNo" />
                        <asp:BoundField DataField="InstrumentDate" HeaderText="UTR Date" SortExpression="InstrumentDate" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="VendorBankName" HeaderText="Bank Name" SortExpression="VendorBankName" />
                        <asp:BoundField DataField="VendorBankAccount" HeaderText="A/C No" SortExpression="VendorBankAccount" />
                        <asp:BoundField DataField="VendorBankIFSC" HeaderText="IFSC" SortExpression="VendorBankIFSC" />
                        <asp:BoundField DataField="VendorGST" HeaderText="GST No" SortExpression="VendorGST" />
                        <asp:BoundField DataField="VendorPAN" HeaderText="PAN" SortExpression="VendorPAN" />
                    </Columns>
                </asp:GridView>
            </fieldset>
        <asp:SqlDataSource ID="DataSourcePayment" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommandType="StoredProcedure" SelectCommand="AC_rptCustomerExpense" DataSourceMode="DataSet">
            <SelectParameters>
                <asp:ControlParameter Name="CustomerID" ControlID="hdnCustomerId" PropertyName="Value" Type="Int32" />
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
            </SelectParameters>
            </asp:SqlDataSource> 
        </ContentTemplate> 
    </asp:UpdatePanel>
</asp:Content>
