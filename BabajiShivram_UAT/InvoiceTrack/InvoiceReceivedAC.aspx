<%@ Page Title="Invoice Received" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="InvoiceReceivedAC.aspx.cs" Inherits="InvoiceTrack_InvoiceReceivedAC" Culture="en-GB" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter1" TagPrefix="uc1" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter2" TagPrefix="uc2" %>
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
            <style type="text/css">
                .hidden
                {
                    display: none;
                }
            </style>
            <script type="text/javascript">
                function GridSelectAllColumn(spanChk) {
                    var oItem = spanChk.children;
                    var theBox = (spanChk.type == "checkbox") ? spanChk : spanChk.children.item[0]; xState = theBox.checked;
                    elm = theBox.form.elements;

                    for (i = 0; i < elm.length; i++) {
                        if (elm[i].type === 'checkbox' && elm[i].checked != xState)
                            elm[i].click();
                    }
                }
            </script>
            <div align="center">
                <asp:Label ID="lblerror" runat="server"></asp:Label>
                <asp:Label ID="lblMsgforReceived" runat="server"></asp:Label>
            </div>
            <div class="clear"></div>
            <cc1:TabContainer ID="TabJobDetail" runat="server" CssClass="Tab" CssTheme="None"
                AutoPostBack="True" ActiveTabIndex="0" OnActiveTabChanged="TabJobDetail_ActiveTabChanged">
                <cc1:TabPanel ID="TabPCDBilling" runat="server" TabIndex="0" HeaderText="Non Receive">
                    <ContentTemplate>
                        <div align="center">
                            <asp:Label ID="lblreceivemsg" runat="server"></asp:Label>
                            <asp:Label ID="lblMsgforNonReceived" runat="server"></asp:Label>
                        </div>
                        <div class="m">
                        </div>
                        <asp:Panel ID="pnlFilter1" runat="server">
                            <div class="fleft">
                                <table>
                                    <tr>
                                        <td>
                                            <uc1:DataFilter1 ID="DataFilter1" runat="server" />
                                        </td>
                                        <td valign="top">
                                            <asp:Button ID="btnReceive" runat="server" Text="Receive" CssClass="buttons" OnClick="btnReceive_Click"
                                                ToolTip="Receive Vendor Invoice" />
                                        </td>
                                        <td valign="top">
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                        <div style="overflow:scroll; width:100%">
                        <asp:GridView ID="gvNonRecievedJobDetail" runat="server" AutoGenerateColumns="False"
                            CssClass="table" DataKeyNames="lid" AllowPaging="True" AllowSorting="True" Width="100%" PageSize="20"
                            DataSourceID="PCDNonReceivedSqlDataSource" >
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkboxSelectAll" align="center" ToolTip="Check All" runat="server"
                                            onclick="GridSelectAllColumn(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk1" runat="server" ToolTip="Check"></asp:CheckBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Token No" SortExpression="TokanNo">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkTokanNo" runat="server" Text='<%#Eval("TokanNo") %>' CommandName="select"
                                            CommandArgument='<%#Eval("lid") %>' Enabled="false"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="Status"
                                    ReadOnly="true" />
                                <asp:BoundField DataField="VendorName" HeaderText="Vendor" SortExpression="VendorName"
                                    ReadOnly="true" />    
                                <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" SortExpression="InvoiceNo"
                                    ReadOnly="true" />
                                <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" DataFormatString="{0:dd/MM/yyyy}"
                                    SortExpression="InvoiceDate" ReadOnly="true" />
                                <asp:TemplateField HeaderText="GST No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGSTNo" Text='<%# Eval("GSTNo")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotalAmount" Text='<%# Eval("TotalAmount")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTaxAmount" Text='<%# Eval("TaxAmount")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CGST" HeaderText="CGST" SortExpression="CGST" ReadOnly="true" />
                                <asp:BoundField DataField="SGST" HeaderText="SGST" SortExpression="SGST" ReadOnly="true" />
                                <asp:BoundField DataField="IGST" HeaderText="IGST" SortExpression="IGST" ReadOnly="true" />
                                <asp:BoundField DataField="BillTypeName" HeaderText="Invoice Type" SortExpression="BillTypeName" ReadOnly="true" />
                                <asp:BoundField DataField="BranchName" HeaderText="Branch" SortExpression="BranchName" ReadOnly="true" />
                                <asp:BoundField DataField="UserName" HeaderText="User" SortExpression="UserName" ReadOnly="true" />
                            </Columns>
                        </asp:GridView>
                        </div>
                        <asp:SqlDataSource ID="PCDNonReceivedSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="INV_GetInvoiceByStatus" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                <asp:Parameter Name="StatusID" DefaultValue="2" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="TabPCDBilling1" runat="server" TabIndex="1" HeaderText="Receive">
                    <ContentTemplate>
                        <asp:ValidationSummary ID="ValSummary" runat="server" ShowMessageBox="True" ShowSummary="False"
                            ValidationGroup="validateform" CssClass="errorMsg" EnableViewState="false" />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                            ShowSummary="true" ValidationGroup="Required" CssClass="errorMsg" />
                        <div class="m"></div>
                        <div style="overflow:scroll; width:100%">
                        <asp:GridView ID="gvRecievedJobDetail" runat="server" AutoGenerateColumns="False"
                            CssClass="table" PagerStyle-CssClass="pgr" DataKeyNames="lid" AllowPaging="True"
                            AllowSorting="True" Width="100%" PageSize="60" PagerSettings-Position="TopAndBottom"
                            DataSourceID="PCDReceivedSqlDataSource">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="Status"
                                    ReadOnly="true" />
                                <asp:BoundField DataField="TokanNo" HeaderText="Tokan No" SortExpression="TokanNo"
                                    ReadOnly="true" />
                                <asp:BoundField DataField="VendorName" HeaderText="Vendor" SortExpression="VendorName"
                                    ReadOnly="true" />    
                                <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" SortExpression="InvoiceNo"
                                    ReadOnly="true" />
                                <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" DataFormatString="{0:dd/MM/yyyy}"
                                    SortExpression="InvoiceDate" ReadOnly="true" />
                                <asp:TemplateField HeaderText="GST No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGSTNo" Text='<%# Eval("GSTNo")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotalAmount" Text='<%# Eval("TotalAmount")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTaxAmount" Text='<%# Eval("TaxAmount")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:BoundField DataField="CGST" HeaderText="CGST" SortExpression="CGST" ReadOnly="true" />
                                <asp:BoundField DataField="SGST" HeaderText="SGST" SortExpression="SGST" ReadOnly="true" />
                                <asp:BoundField DataField="IGST" HeaderText="IGST" SortExpression="IGST" ReadOnly="true" />--%>
                                <asp:BoundField DataField="BillTypeName" HeaderText="Invoice Type" SortExpression="BillTypeName" ReadOnly="true" />
                                <asp:BoundField DataField="BranchName" HeaderText="Branch" SortExpression="BranchName" ReadOnly="true" />
                                <asp:BoundField DataField="UserName" HeaderText="User" SortExpression="UserName" ReadOnly="true" />
                            </Columns>
                        </asp:GridView>
                        </div>
                        <asp:SqlDataSource ID="PCDReceivedSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="INV_GetInvoiceByStatus" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                <asp:Parameter Name="StatusID" DefaultValue="3" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </ContentTemplate>
                </cc1:TabPanel>
            </cc1:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


