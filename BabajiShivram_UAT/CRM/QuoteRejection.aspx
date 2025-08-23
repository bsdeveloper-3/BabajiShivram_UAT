<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QuoteRejection.aspx.cs" Inherits="Quotation_QuoteRejection" Culture="en-GB" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rejection</title>
    <link href="Approval.css" rel="stylesheet" />
    <style type="text/css">
        table.table th, table.table th a {
            background-color: white;
        }

        tr {
            border: none;
        }

        td {
            border: none;
        }
    </style>
</head>
<body>
    <form id="frmApproval" runat="server" class="formApproval">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="Space" style="text-align: center; padding-left: 5px">
            <asp:Label ID="lblMessage" runat="server" Font-Underline="true" Font-Bold="true" ForeColor="Red"></asp:Label>
            <asp:ValidationSummary ID="vsRequired" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="vgRequired" CssClass="errorMsg" />
        </div>
        <span>
            <asp:HiddenField ID="hdnLeadId" runat="server" Value="0" />
            <asp:HiddenField ID="hdnUserId" runat="server" Value="0" />
            <br />
            <span style="float: left; padding-top: 5px; padding-right: 25px" id="span_ProceedAhead" runat="server">
                <asp:TextBox ID="txtRemark" runat="server" TabIndex="1" placeholder=" Enter remark..." Rows="2" Width="900px" TextMode="MultiLine" Style="border: 0.5px solid black; border-radius: 4px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvRemark" runat="server" ControlToValidate="txtRemark" SetFocusOnError="true" Display="Dynamic"
                    ForeColor="Red" ErrorMessage="Please enter remark." Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
            </span>
        </span>
        <div class="Space" style="padding-left: 5px">
            <asp:Button ID="btnYes" runat="server" OnClick="btnYes_OnClick" Text="REJECT" ForeColor="White" Font-Bold="true" ValidationGroup="vgRequired" CssClass="btn" BackColor="#80cc3b" />
        </div>
        <asp:Button ID="lblLead" runat="server" Text="LEAD INFO" Enabled="false" ForeColor="White" Font-Bold="true" CssClass="btn" BackColor="#80cc3b" />
        <hr style="border-bottom: 1px solid black" />
        <div>
            <asp:FormView ID="FormView1" runat="server" Width="100%" DataSourceID="FormviewSqlDataSource" DataKeyNames="lid" Style="border: none">
                <ItemTemplate>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <colgroup>
                            <col style="width: 40%" />
                            <col style="width: 35%" />
                            <col style="width: 25%" />
                        </colgroup>
                        <tr>
                            <td><b>Lead Ref No : </b>
                                <asp:Label ID="lblLeadRefNo" runat="server" Text='<%#Eval("LeadRefNo") %>'></asp:Label></td>
                            <td><b>Company Name : </b>
                                <asp:Label ID="lblCompanyName" runat="server" Text='<%#Eval("CompanyName") %>'></asp:Label></td>
                            <td><b>Company Type : </b>
                                <asp:Label ID="lblCompanyType" runat="server" Text='<%#Eval("CompanyType") %>'></asp:Label></td>
                        </tr>
                        <tr>
                            <td><b>Lead Source : </b>
                                <asp:Label ID="lblLeadSource" runat="server" Text='<%#Eval("LeadSource") %>'></asp:Label>
                                <asp:Label ID="lblSourceDescription" runat="server" Text='<%# "(" + Eval("SourceDescription") + ")" %>'></asp:Label>
                            </td>
                            <td><b>Business Sector : </b>
                                <asp:Label ID="lblBusinessSector" runat="server" Text='<%#Eval("BusinessSector") %>'></asp:Label></td>
                            <td><b>Business Category : </b>
                                <asp:Label ID="lblBusinessCatg" runat="server" Text='<%#Eval("BusinessCategory") %>'></asp:Label></td>
                        </tr>
                        <tr>
                            <td><b>Contact Name : </b>
                                <asp:Label ID="lblContactName" runat="server" Text='<%#Eval("ContactName") %>'></asp:Label></td>
                            <td><b>Desgination : </b>
                                <asp:Label ID="lblDesignation" runat="server" Text='<%#Eval("Designation") %>'></asp:Label></td>
                            <td><b>Role : </b>
                                <asp:Label ID="lblRole" runat="server" Text='<%#Eval("Role") %>'></asp:Label></td>
                        </tr>
                        <tr>
                            <td><b>Email : </b>
                                <asp:Label ID="lblEmail" runat="server" Text='<%#Eval("Email") %>'></asp:Label></td>
                            <td><b>Mobile No : </b>
                                <asp:Label ID="lblMobileNo" runat="server" Text='<%#Eval("MobileNo") %>'></asp:Label></td>
                            <td><b>Contact No : </b>
                                <asp:Label ID="lblContactNo" runat="server" Text='<%#Eval("ContactNo") %>'></asp:Label></td>
                        </tr>
                        <tr>
                            <td><b>Created By : </b>
                                <asp:Label ID="lblCreatedBy" runat="server" Text='<%#Eval("CreatedBy") %>'></asp:Label></td>
                            <td><b>Created Date : </b>
                                <asp:Label ID="lblCreatedDate" runat="server" Text='<%#Eval("CreatedDate","{0:dd/MM/yyyy}") %>'></asp:Label></td>
                            <td></td>
                        </tr>
                    </table>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td colspan="3"><b>Office Location : </b>
                                <asp:Label ID="lblOfficeLocation" runat="server" Text='<%#Eval("OfficeLocation") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3"><b>Address Line 1: </b>
                                <asp:Label ID="lblAddress1" runat="server" Width="1020px" Text='<%#Eval("AddressLine1") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3"><b>Address Line 2: </b>
                                <asp:Label ID="lblAddress2" runat="server" Width="1020px" Text='<%#Eval("AddressLine2") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3"><b>Address Line 3: </b>
                                <asp:Label ID="lblAddress3" runat="server" Width="1020px" Text='<%#Eval("AddressLine3") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3"><b>Description : </b>
                                <asp:Label ID="lblDescription" runat="server" Width="1020px" Text='<%#Eval("Description") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3"><b>Additional Remark : </b>
                                <asp:Label ID="lblAdditionalRemark" runat="server" Width="1020px" Text='<%#Eval("StageRemark") %>'></asp:Label>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:FormView>
            <div id="divDataSource">
                <asp:SqlDataSource ID="FormviewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CRM_GetLeadByLid" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hdnLeadId" Name="lid" PropertyName="Value" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </div>
        <div class="Space"></div>

        <asp:Button ID="lblServicesOffered" runat="server" Text="SERVICES OFFERED" Enabled="false" ForeColor="White" Font-Bold="true" CssClass="btn" BackColor="#80cc3b" />
        <hr style="border-bottom: 1px solid black" />
        <asp:GridView ID="gvServicesOffered" runat="server" AutoGenerateColumns="False" Style="background-color: white" BorderWidth="2px"
            Width="70%" DataKeyNames="lid" DataSourceID="DataSourceServicesOffered" CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20">
            <Columns>
                <asp:TemplateField HeaderText="Sl" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#Container.DataItemIndex +1%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ServiceName" HeaderText="Service Offered" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderStyle="Solid" ItemStyle-BorderWidth="1px" />
                <asp:BoundField DataField="ServiceLocation" HeaderText="Service Location" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderStyle="Solid" ItemStyle-BorderWidth="1px" />
                <asp:BoundField DataField="VolumeExpected" HeaderText="Volume Expected" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderStyle="Solid" ItemStyle-BorderWidth="1px" />
                <asp:BoundField DataField="ExpectedCloseDate" HeaderText="Expected Close Date" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderStyle="Solid" ItemStyle-BorderWidth="1px" />
            </Columns>
        </asp:GridView>

        <div class="Space"></div>
        <asp:Button ID="lblEnquiry" runat="server" Text="ENQUIRY INFO" Enabled="false" ForeColor="White" Font-Bold="true" CssClass="btn" BackColor="#80cc3b" />
        <hr style="border-bottom: 1px solid black" />
        <div>
            <asp:FormView ID="FormViewEnquiry" runat="server" Width="100%" DataSourceID="DataSourceEnquiry" DataKeyNames="lid" Style="border: none"
                OnItemCommand="FormViewEnquiry_ItemCommand">
                <ItemTemplate>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <colgroup>
                            <col style="width: 40%" />
                            <col style="width: 35%" />
                            <col style="width: 25%" />
                        </colgroup>
                        <tr>
                            <td><b>Tunrover : </b>
                                <asp:Label ID="lblTurnover" runat="server" Text='<%#Eval("Turnover") %>'></asp:Label></td>
                            <td><b>Employee Count : </b>
                                <asp:Label ID="lblEmployeeCount" runat="server" Text='<%#Eval("EmployeeCount") %>'></asp:Label></td>
                            <td><b>Customer Reference : </b>
                                <asp:Label ID="lblCustRef" runat="server" Text='<%#Eval("CustRef") %>'></asp:Label></td>
                        </tr>
                        <tr>
                            <td><b>Payment Terms : </b>
                                <asp:Label ID="lblPaymentTerms" runat="server" Text='<%#Eval("PaymentTerms") %>'></asp:Label>
                            </td>
                            <td><b>Years In Service : </b>
                                <asp:Label ID="lblYearsInService" runat="server" Text='<%#Eval("YearsInService") %>'></asp:Label></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td><b>Notes : </b>
                                <asp:Label ID="lblNotes" runat="server" Text='<%#Eval("Notes") %>'></asp:Label></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td><b>Quote : </b>
                                <asp:Label ID="lblQuoteRefNo" runat="server" Text='<%#Eval("QuoteRefNo") %>' Font-Underline="true"></asp:Label>
                                &nbsp;
                                <asp:ImageButton ID="imgbtnDownloadQuote" runat="server" CausesValidation="false" CommandName="DownloadQuote"
                                    ImageAlign="AbsMiddle" ImageUrl="~/Images/pdf2.png" Width="16" Height="16" ToolTip="Download Quotation in PDF Format."
                                    CommandArgument='<%#Eval("QuotePath") %>' />
                            </td>
                            <td><b>Is RFQ Quote? </b>
                                <asp:Label ID="lblRFQQuote" runat="server" Text='<%#Eval("IsTenderQuote") %>'></asp:Label></td>
                            <td><b>Quote Created By : </b>
                                <asp:Label ID="lblQuoteCreatedBy" runat="server" Text='<%#Eval("QuoteCreatedBy") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td><b>Quote Created Date : </b>
                                <asp:Label ID="lblQuoteCreatedDate" runat="server" Text='<%#Eval("QuoteCreatedDate", "{0:dd/MM/yyyy}") %>'></asp:Label></td>
                            <td></td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:FormView>
            <div id="divDataSource_Enquiry">
                <asp:SqlDataSource ID="DataSourceEnquiry" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CRM_GetEnquiryByLeadId" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hdnLeadId" Name="LeadId" PropertyName="Value" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </div>

        <div class="Space"></div>
        <asp:Label ID="txtApprovedBy" runat="server" Width="250px" Enabled="false" Font-Bold="true" Visible="false"></asp:Label>
        <asp:SqlDataSource ID="DataSourceServicesOffered" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="CRM_GetServices" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="hdnLeadId" Name="LeadId" PropertyName="Value" />
            </SelectParameters>
        </asp:SqlDataSource>
    </form>
</body>
</html>
