<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ApproveQuote.aspx.cs" Inherits="Quotation_ApproveQuote" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Babaji Shivram Clearing And Carriers Pvt Ltd</title>
    <link href="../CSS/babaji-shivram.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" ID="ScriptManager1" />
        <style type="text/css">
            .success {
                color: #0000ff;
            }

            table.table {
                white-space: normal;
            }
        </style>
        <script type="text/javascript">
            function Confirm() {

                if (confirm("Do you want to Approve this Quotation?"))
                    return true;
                else
                    return false;

            }

            function ConfirmReject() {

                if (confirm("Do you want to Reject this Quotation?"))
                    return true;
                else
                    return false;

            }
        </script>
        <div>
            <div id="header">
                <img src='<%=imgPath %>' width="592" height="131" alt="Babaji Shivram" />
                <div class="clear">
                </div>
            </div>
            <div class="heading">
                <div class="pageHeading">
                    Quotation Approval
                </div>
                <div class="clear">
                </div>
            </div>
            <div id="divUpdPanel">
                <div>
                    <div id="divCPH" class="CPH" onscroll="saveScrollPos();" runat="server">
                        <asp:UpdatePanel ID="upCustomerUser" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                            <ContentTemplate>
                                <div style="padding-bottom: 0px; padding-left: 12px; padding-top: 5px">
                                    <div class="m clear" style="padding-left: 300px; padding-bottom: 10px">
                                        <asp:Label ID="lblError" runat="server" EnableViewState="false" Font-Bold="true" Font-Underline="true"></asp:Label>
                                    </div>
                                    <asp:Button ID="btnApprove" runat="server" Text="Approve" OnClientClick="if (!Confirm()) return false;" OnClick="btnApprove_OnClick" />
                                    <asp:Button ID="btnReject" runat="server" Text="Reject" OnClientClick="if (!ConfirmReject()) return false;" OnClick="btnReject_OnClick" />
                                </div>
                                <div style="padding: 10px">
                                    <table border="0" runat="server" style="width: 60%" class="table">
                                        <tr>
                                            <td>Quotation Ref No</td>
                                            <td>
                                                <asp:Label ID="lblQuoteRefNo" runat="server" Width="80%"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td>Customer Name</td>
                                            <td>
                                                <asp:Label ID="lblCust" runat="server" Width="80%"></asp:Label>
                                                <asp:Label ID="lblLumpSumCode" runat="server" Width="80%" Visible="false"></asp:Label>
                                            </td>
                                        </tr>

                                    </table>
                                    <fieldset>
                                        <legend style="margin-left: 2px">Charges Applicable</legend>

                                        <asp:GridView ID="gvQuote" runat="server" AutoGenerateColumns="False" EnableViewState="true" DataKeyNames="lid" ShowFooter="true"
                                            Width="60%" CssClass="table" OnRowDataBound="gvQuote_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr.No.">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex +1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Particulars" HeaderText="Particulars" ReadOnly="true" />
                                                <asp:BoundField DataField="ChargesApplicable" HeaderText="Charges Applicable" ReadOnly="true" />
                                                <asp:BoundField DataField="LpChargesApplicable" HeaderText="Charges Applicable" ReadOnly="true" />
                                                <asp:BoundField DataField="MinChargesApplicable" HeaderText="Minimum Charges Applicable" ReadOnly="true" />
                                                <asp:BoundField DataField="LumpSumAmt" ReadOnly="true" Visible="false" />
                                                <asp:BoundField DataField="MinAmt" ReadOnly="true" Visible="false" />
                                                <asp:BoundField DataField="IsValidDraft" ReadOnly="true" Visible="false" />
                                            </Columns>
                                        </asp:GridView>
                                        <div id="tblTotal" runat="server">
                                            <div style="color: red; margin-top: 10px">
                                                <b><u>NOTE : Line charges marked with red color are below minimum range for given charge.
                                                </u></b>
                                            </div>
                                        </div>
                                        <div id="tbltotal_Lp" runat="server">
                                            <%--  <table border="0" runat="server" style="width: 15%" class="table">
                                                <tr>
                                                    <td><b>Total: </b></td>
                                                    <td>
                                                        <b>
                                                            <asp:Label ID="lbltotal" runat="server" Width="80%" ForeColor="Red" Font-Bold="true"></asp:Label></b></td>
                                                </tr>
                                                <tr>
                                                    <td><b>Minimum Total:</b></td>
                                                    <td>
                                                        <b>
                                                            <asp:Label ID="lblMintotal" runat="server" Width="80%"></asp:Label></b>
                                                    </td>
                                                </tr>
                                            </table>--%>
                                            <div style="color: red; margin-top: 10px">
                                                <b><u>NOTE : The total of charges applicable is less than the actual minimum total (i.e., 
                                                    <asp:Label ID="lblTotal2" runat="server"></asp:Label>
                                                    <
                                                    <asp:Label ID="lblMinTotal2" runat="server"></asp:Label>) </u></b>
                                            </div>
                                        </div>
                                        <div>
                                            <asp:SqlDataSource ID="DataSourceQuote" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                                SelectCommand="rpt_Quotation" SelectCommandType="StoredProcedure">
                                                <SelectParameters>
                                                    <asp:SessionParameter Name="QuotationId" SessionField="QuotationId" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                        </div>
                                    </fieldset>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
    </form>
</body>
</html>
