<%@ Page Title="Lead Rejected" Language="C#" AutoEventWireup="true" CodeFile="NewLeadRejected.aspx.cs"
    Inherits="CRM_NewLeadRejected" Culture="en-GB" %>

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
    <form id="frmRejected" runat="server" class="formApproval">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="Space" style="text-align: center; padding-left: 5px">
            <asp:Label ID="lblMessage" runat="server" Font-Underline="true" Font-Bold="true" ForeColor="Red"></asp:Label>
        </div>
        <span>
            <asp:HiddenField ID="hdnLeadId" runat="server" Value="0" />
            <asp:HiddenField ID="hdnUserId" runat="server" Value="0" />
            <br />
            <span style="float: left; padding-top: 5px; padding-right: 25px" id="span_ProceedAhead" runat="server">
                <asp:TextBox ID="txtRemark" runat="server" TabIndex="1" required placeholder=" Enter remark..." Rows="2" Width="900px" TextMode="MultiLine" Style="border: 0.5px solid black; border-radius: 4px"></asp:TextBox>
            </span>
        </span>
        <div class="Space" style="padding-left: 5px">
            <asp:Button ID="btnYes" runat="server" OnClientClick="return ConfirmMessage();" OnClick="btnYes_OnClick" Text="REJECT" ForeColor="White" Font-Bold="true" CssClass="btn" BackColor="#80cc3b" />
        </div>
        <asp:Button ID="lblLead" runat="server" Text="LEAD INFO" Enabled="false" ForeColor="White" Font-Bold="true" CssClass="btn" BackColor="#80cc3b" />
        <hr style="border-bottom: 1px solid black" />
        <div>
            <asp:FormView ID="FormView1" runat="server" Width="100%" DataSourceID="FormviewSqlDataSource" DataKeyNames="lid" Style="border: none">
                <ItemTemplate>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <colgroup>
                            <col style="width: 34%" />
                            <col style="width: 33%" />
                            <col style="width: 33%" />
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
                            <td><b>Customer Reference : </b>
                                <asp:Label ID="lblCustRef" runat="server" Text='<%#Eval("CustRef") %>'></asp:Label></td>
                            <td><b>Tunrover : </b>
                                <asp:Label ID="lblTurnover" runat="server" Text='<%#Eval("Turnover") %>'></asp:Label></td>
                            <td><b>Employee Count : </b>
                                <asp:Label ID="lblEmployeeCount" runat="server" Text='<%#Eval("EmployeeCount") %>'></asp:Label></td>
                        </tr>
                        <tr>
                            <td><b>Years In Service : </b>
                                <asp:Label ID="lblYearsInService" runat="server" Text='<%#Eval("YearsInService") %>'></asp:Label></td>
                            <td><b>Lead Owner : </b>
                                <asp:Label ID="lblCreatedBy" runat="server" Text='<%#Eval("CreatedBy") %>'></asp:Label></td>
                            <td><b>Lead Created On : </b>
                                <asp:Label ID="lblCreatedDate" runat="server" Text='<%#Eval("CreatedDate","{0:dd/MM/yyyy}") %>'></asp:Label></td>
                            <td></td>
                        </tr>
                    </table>
                    <div class="Space"></div>
                    <asp:Button ID="lblContact" runat="server" Text="CONTACT INFO" Enabled="false" ForeColor="White" Font-Bold="true" CssClass="btn" BackColor="#80cc3b" />
                    <hr style="border-bottom: 1px solid black" />
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <colgroup>
                            <col style="width: 34%" />
                            <col style="width: 33%" />
                            <col style="width: 33%" />
                        </colgroup>
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
                    </table>

                    <div class="Space"></div>
                    <asp:Button ID="lblAddress" runat="server" Text="ADDRESS INFO" Enabled="false" ForeColor="White" Font-Bold="true" CssClass="btn" BackColor="#80cc3b" />
                    <hr style="border-bottom: 1px solid black" />
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
                    </table>
                    <div class="Space"></div>
                    <asp:Button ID="lblDescriptionTab" runat="server" Text="DESCRIPTION INFO" Enabled="false" ForeColor="White" Font-Bold="true" CssClass="btn" BackColor="#80cc3b" />
                    <hr style="border-bottom: 1px solid black" />
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td colspan="3"><b>Description : </b>
                                <asp:Label ID="lblDescription" runat="server" Width="1020px" Text='<%#Eval("Description") %>'></asp:Label>
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

    </form>
</body>
</html>

