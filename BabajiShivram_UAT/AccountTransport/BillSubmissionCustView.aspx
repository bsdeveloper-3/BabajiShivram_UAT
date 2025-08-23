<%@ Page Title="Transport Draft Bill" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="BillSubmissionCustView.aspx.cs" Inherits="AccountTransport_BillSubmissionCustView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div align="center">
        <asp:Label ID="lberror" Text="" runat="server" EnableViewState="false"></asp:Label>
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
                    <asp:BoundField HeaderText="Bill Amount" DataField="BillAmount" />
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
        </div>
        <div>
            <asp:GridView ID="GridViewVehicle" runat="server" AutoGenerateColumns="False" CssClass="table"
                Width="99%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="RateId"
                DataSourceID="DataSourceRate" CellPadding="4" AllowPaging="True" ShowFooter="true"
                PageSize="200" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
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
                    <asp:TemplateField HeaderText="Freight Rate" >
                        <ItemTemplate>
                            <asp:Label ID="lblFreightAmount" runat="server" Text='<%# Bind("FreightAmount") %>' Width="48px" MaxLength="10"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Detention">
                        <ItemTemplate>
                            <asp:Label ID="txtDetention" runat="server" Text='<%# Bind("DetentionAmount") %>' Width="48px" MaxLength="10"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Varai">
                        <ItemTemplate>
                            <asp:Label ID="txtVarai" runat="server" Text='<%# Bind("VaraiExpense") %>' Width="48px" MaxLength="10"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Empty">
                        <ItemTemplate>
                            <asp:Label ID="txtEmptyContRecpt" runat="server" Text='<%# Bind("EmptyContRecptCharges") %>' Width="48px" MaxLength="10"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Toll">
                        <ItemTemplate>
                            <asp:Label ID="txtTollCharges" runat="server" Text='<%# Bind("TollCharges") %>' Width="48px" MaxLength="10"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="UNION">
                        <ItemTemplate>
                            <asp:Label ID="txtUNION" runat="server" Text='<%# Bind("UnionCharges") %>' Width="48px" MaxLength="10"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total">
                        <ItemTemplate>
                            <asp:Label ID="txtTotalAmount" runat="server" Text='<%# Bind("TotalAmount") %>' Width="48px" MaxLength="10"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
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

