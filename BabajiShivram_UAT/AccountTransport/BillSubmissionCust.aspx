<%@ Page Title="Billing Advice" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="BillSubmissionCust.aspx.cs" Inherits="AccountTransport_BillSubmissionCust" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div align="center">
        <asp:Label ID="lberror" Text="" runat="server" EnableViewState="false"></asp:Label>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="Required" />
        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="vgJobNo" />
    </div>
    <fieldset>
        <fieldset>
            <legend>Job Detail</legend>
            <table border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
                <tr>
                    <td>BS Job No
                    </td>
                    <td>
                        <asp:Label ID="lblJobNo" runat="server"></asp:Label>
                    </td>
                    <td>Customer
                    </td>
                    <td>
                        <asp:Label ID="lblCustName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Bill To
                    </td>
                    <td>
                        <asp:Label ID="lblTransportBillTo" runat="server" Text=""></asp:Label>
                    </td>
                    <td>Destination
                    </td>
                    <td>
                        <asp:Label ID="lblDestination" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset>
        <legend>Transporter Billing Detail</legend>
        <div>
            <asp:HiddenField ID="hdnPageValid" runat="server" Value="0" />
            <asp:HiddenField ID="hdnJobId" runat="server" Value="0" />
            <asp:HiddenField ID="hdnFreightAmt" runat="server" Value="0" />
            <asp:HiddenField ID="hdnDetentionAmt" runat="server" Value="0" />
            <asp:HiddenField ID="hdnVaraiAmt" runat="server" Value="0" />
            <asp:HiddenField ID="hdnEmptyContReturnAmt" runat="server" Value="0" />
            <asp:HiddenField ID="hdnTollCharges" runat="server" Value="0" />
            <asp:HiddenField ID="hdnOtherCharges" runat="server" Value="0" />
            <asp:HiddenField ID="hdnAdvanceAmt" runat="server" Value="0" />
            <asp:HiddenField ID="hdnTotalAmount" runat="server" Value="0" />
            <asp:HiddenField ID="hdnSavingAmt" runat="server" Value="0" />
            <asp:HiddenField ID="hdnMarketRate" runat="server" Value="0" />
        </div>
        <div align="center">
            <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary3" runat="server" ShowMessageBox="True"
                ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" />
        </div>
        <div class="m clear">
            <asp:GridView ID="GridViewBillDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                Width="100%" DataKeyNames="lId" DataSourceID="DataSourceBillDetail" CellPadding="4">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:BoundField HeaderText="Ref No" DataField="TRRefNo" />
                    <asp:BoundField HeaderText="Job No" DataField="JobRefNo" />--%>
                    <asp:BoundField HeaderText="Transporter" DataField="Transporter" />
                    <asp:BoundField HeaderText="Bill Number" DataField="BillNumber" />
                    <%--<asp:BoundField HeaderText="Bill Submit Date" DataField="BillSubmitDate" DataFormatString="{0:dd/MM/yyyy}" />--%>
                    <asp:BoundField HeaderText="Bill Date" DataField="BillDate" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField HeaderText="Freight" DataField="BillAmount" />
                    <asp:BoundField HeaderText="Detention" DataField="DetentionAmount" />
                    <asp:BoundField HeaderText="Varai" DataField="VaraiAmount" />
                    <asp:BoundField HeaderText="Empty" DataField="EmptyContRcptCharges" />
                    <asp:BoundField HeaderText="Toll" DataField="TollCharges" />
                    <asp:BoundField HeaderText="Union" DataField="OtherCharges" />
                    <asp:BoundField HeaderText="Total" DataField="TotalAmount" />
                    <asp:BoundField HeaderText="Remark" DataField="Justification" />
                    <asp:BoundField HeaderText="Date" DataField="dtDate" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField HeaderText="User" DataField="UserName"/>
                    <%--<asp:BoundField HeaderText="Billing Person" DataField="BillPersonName" />--%>
                </Columns>
            </asp:GridView>
        </div>
        </fieldset>
        <fieldset>
        <legend>Customer Bill Detail</legend>
        <div class="m clear">
            <asp:Button ID="btnBillSubmit" Text="Save Bill Detail" runat="server" OnClientClick="return ConfirmMessage();"
                OnClick="btnBillSubmit_Click" />
            <asp:Button ID="btnCancelBill" runat="server" Text="Cancel" OnClick="btnCancelBill_Click" />
        </div>
        <div>
            <asp:GridView ID="GridViewVehicle" runat="server" AutoGenerateColumns="False" CssClass="table"
                Width="99%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="RateId"
                DataSourceID="DataSourceRate" CellPadding="4" AllowPaging="True" ShowFooter="true"
                PageSize="200" OnRowDataBound="GridViewVehicle_RowDataBound" 
                FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Vehicle No">
                        <ItemTemplate>
                            <asp:Label ID="lblVehicleNo" runat="server" Text='<%# Bind("VehicleNo") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="TransporterName" HeaderText="Transporter" ReadOnly="true" />
                    <asp:TemplateField HeaderText="Freight">
                        <ItemTemplate>
                            <asp:TextBox ID="lblFreightAmount" runat="server" Text='<%# Bind("FreightAmount") %>' Width="48px" MaxLength="10"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Detention">
                        <ItemTemplate>
                            <asp:TextBox ID="txtDetention" runat="server" Text='<%# Bind("DetentionAmount") %>' Width="48px" MaxLength="10"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Varai">
                        <ItemTemplate>
                            <asp:TextBox ID="txtVarai" runat="server" Text='<%# Bind("VaraiExpense") %>' Width="48px" MaxLength="10"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Empty">
                        <ItemTemplate>
                            <asp:TextBox ID="txtEmptyContRecpt" runat="server" Text='<%# Bind("EmptyContRecptCharges") %>' Width="48px" MaxLength="10"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Toll">
                        <ItemTemplate>
                            <asp:TextBox ID="txtToll" runat="server" Text='<%# Bind("TollCharges") %>' Width="48px" MaxLength="10"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Union">
                        <ItemTemplate>
                            <asp:TextBox ID="txtUnion" runat="server" Text='<%# Bind("UnionCharges") %>' Width="48px" MaxLength="10"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Challan No">
                        <ItemTemplate>
                            <asp:TextBox ID="txtChallanNo" runat="server" Text='<%# Bind("FreightChallanNo") %>' TextMode="MultiLine" Columns="3"  MaxLength="50"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Challan Date">
                        <ItemTemplate>
                            <asp:TextBox ID="txtChallanDate" runat="server" Text='<%# Bind("FreightChallanDate") %>' MaxLength="10" Width="60px"></asp:TextBox>
                            <cc1:CalendarExtender ID="calChallanDate" runat="server" Enabled="True" 
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgChallanDate" PopupPosition="BottomRight"
                                TargetControlID="txtChallanDate">
                            </cc1:CalendarExtender>
                            <asp:Image ID="imgChallanDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            <cc1:MaskedEditExtender ID="MskExtChallanDate" TargetControlID="txtChallanDate" Mask="99/99/9999" MessageValidatorTip="true" 
                                MaskType="Date" AutoComplete="false" runat="server"></cc1:MaskedEditExtender>
                            <cc1:MaskedEditValidator ID="MskValChallanDate" ControlExtender="MskExtChallanDate" ControlToValidate="txtChallanDate" IsValidEmpty="false" 
                                EmptyValueMessage="Please Enter Challan Date" EmptyValueBlurredText="*" InvalidValueMessage="Challan Date is invalid" 
                                InvalidValueBlurredMessage="Invalid Date" MinimumValueMessage="Invalid Challan Date" MaximumValueMessage="Invalid Date" 
                                MaximumValue='<%#DateTime.Now.ToString("dd/MM/yyyy") %>'
                                Runat="Server" SetFocusOnError="true" ValidationGroup="Required"></cc1:MaskedEditValidator>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Dest From">
                        <ItemTemplate>
                            <asp:TextBox ID="txtFrom" runat="server" Text='<%# Bind("Dest_From") %>' TextMode="MultiLine" Columns="2" MaxLength="100"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Dest To">
                        <ItemTemplate>
                            <asp:TextBox ID="txtTo" runat="server" Text='<%# Bind("Dest_To") %>' TextMode="MultiLine" Columns="2" MaxLength="100"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Remark">
                        <ItemTemplate>
                            <asp:TextBox ID="txtRemark" runat="server" Text='<%# Bind("Remark") %>' TextMode="MultiLine" Columns="2"  MaxLength="200"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sacn LR">
                        <ItemTemplate>
                            <asp:FileUpload ID="updSacnLRCopy" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%--<asp:TemplateField HeaderText="Toll">
                        <ItemTemplate>
                            <asp:TextBox ID="txtToll" runat="server" Text='<%# Bind("TollCharges") %>' Width="48px" MaxLength="10"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Other Charges">
                        <ItemTemplate>
                            <asp:TextBox ID="txtOtherCharges" runat="server" Text='<%# Bind("OtherCharges") %>' Width="48px" MaxLength="10"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                </Columns>
            </asp:GridView>
        </div>
    </fieldset>
    </fieldset>
    <div id="divDatasource">
    <asp:SqlDataSource ID="DataSourceBillDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
        SelectCommand="TR_GetTransBillByJobid" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:SessionParameter Name="TransReqID" SessionField="TransReqId" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="DataSourceRate" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
        SelectCommand="TRS_GetTransBillByRequestId" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:SessionParameter Name="TransReqID" SessionField="TransReqId" />
        </SelectParameters>
    </asp:SqlDataSource>
    </div>
</asp:Content>

