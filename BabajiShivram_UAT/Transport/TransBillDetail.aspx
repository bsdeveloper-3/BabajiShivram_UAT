<%@ Page Title="Transport Bill Detail" Language="C#" MasterPageFile="~/MasterPage.master" 
    AutoEventWireup="true" CodeFile="TransBillDetail.aspx.cs"  Inherits="Transport_TransBillDetail" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <div align="center">
        <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" />
    </div>
        <div>
        <cc1:CalendarExtender ID="calBillSubmitDate" runat="server" Enabled="True" EnableViewState="False"
            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgBillSubmitDate"
            PopupPosition="BottomRight" TargetControlID="txtBillSubmitDate">
        </cc1:CalendarExtender>
        <cc1:CalendarExtender ID="calBillDate" runat="server" Enabled="True" EnableViewState="False"
            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgBillDate"
            PopupPosition="BottomRight" TargetControlID="txtBillDate">
        </cc1:CalendarExtender>
    
    </div>
    <div>
        <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" />
    </div>
    <div>
        <fieldset><legend>Transport Detail</legend>
        <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
            <tr>
                <td>
                    Ref No.
                </td>
                <td>
                    <asp:Label ID="lblTRRefNo" runat="server"></asp:Label>
                </td>
                <td>
                   
                </td>
                <td>
                   
                </td>
            </tr>
            <tr>
                <td>
                    Customer Name
                </td>
                <td>
                    <asp:Label ID="lblCustName" runat="server"></asp:Label>
                </td>
                <td>
                    Job No.
                </td>
                <td>
                    <asp:Label ID="lblJobNo" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Location
                </td>
                <td>
                    <asp:Label ID="lblLocationFrom" runat="server"></asp:Label>
                </td>
                <td>
                    Destination
                </td>
                <td>
                    <asp:Label ID="lblDestination" runat="server"></asp:Label>
                </td>
                
            </tr>
            <tr>
                <td>
                    Packages    
                </td>
                <td>
                    <asp:Label ID="lblNoOfPackages" runat="server"></asp:Label>
                    <%--&nbsp;<asp:Label ID="lblPackageType" runat="server"></asp:Label>--%>
                </td>
                <td>
                    Gross Weight (Kgs)
                </td>
                <td>
                    <asp:Label ID="lblGrossWeight" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Con 20
                </td>
                <td>
                    <asp:Label ID="lblCon20" runat="server"></asp:Label>
                </td>
                <td>
                    Con 40
                </td>
                <td>
                    <asp:Label ID="lblCon40" runat="server"></asp:Label>
                </td>
            </tr>
       </table> 
    </fieldset>
    </div>
    <fieldset><legend>Rate Detail</legend>
        <div>
        <asp:GridView ID="GridViewVehicle" runat="server" AutoGenerateColumns="False" CssClass="table"
            Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
            DataSourceID="DataSourceRate" CellPadding="4" AllowPaging="True" AllowSorting="True"
            PageSize="20" OnRowCommand="GridViewVehicle_RowCommand" OnRowDataBound="GridViewVehicle_RowDataBound"
            OnRowEditing="GridViewVehicle_RowEditing" OnRowUpdating="GridViewVehicle_RowUpdating" OnRowCancelingEdit="GridViewVehicle_RowCancelingEdit" >
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Approved Rate" DataField="ApprovedAmount" ReadOnly="true" />
                <asp:BoundField HeaderText="Pkgs" DataField="Packages" ReadOnly="true" />
                <%--<asp:BoundField HeaderText="Vehicle No" DataField="VehicleNo" ReadOnly="true" />--%>
                <asp:TemplateField HeaderText="Vehicle No">
                    <ItemTemplate>
                        <asp:Label ID="lblVehicleNo" runat="server" Text='<%#BIND("VehicleNo") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Type" DataField="VehicleType" ReadOnly="true" />
                <asp:BoundField HeaderText="Delivery From" DataField="DeliveryFrom" ReadOnly="true" />
                <asp:BoundField HeaderText="Delivery Point" DataField="DeliveryTo" ReadOnly="true" />
                <asp:BoundField HeaderText="Transporter" DataField="Transporter" ReadOnly="true" />
                <asp:BoundField HeaderText="Dispatch Date" DataField="DispatchDate" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                <asp:BoundField HeaderText="Unloading Date" DataField="UnloadingDate" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                <%--<asp:BoundField HeaderText="Status" DataField="UserName" />--%>
                <%--<asp:BoundField HeaderText="User" DataField="UserName" />--%>
            </Columns>
        </asp:GridView>
        </div>
    </fieldset>
        <fieldset><legend>Billing Detail</legend>
        <div class="m clear">
            <asp:Button ID="btnBillSubmit" OnClick="btnBillSubmit_Click" Text="Approve Bill" runat="server" ValidationGroup="BillRequired" />
        </div>
        <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white" runat="server">
            <tr>
                <td>
                    Transporter
                    <asp:RequiredFieldValidator ID="RFVTransporter" runat="server" ControlToValidate="ddTransporter" SetFocusOnError="true"   
                       InitialValue="0" Text="*" ErrorMessage="Please Select Transporter" Display="Dynamic" ValidationGroup="BillRequired"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:DropDownList ID="ddTransporter" runat="server" DataSourceID="DSTransporterPlaced" 
                        DataTextField="TransporterName" DataValueField="TransporterID" AppendDataBoundItems="true"> 
                        <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    Bill Submit Date
                   <cc1:MaskedEditExtender ID="MEditBillSubmitDate" TargetControlID="txtBillSubmitDate" Mask="99/99/9999" MessageValidatorTip="true" 
                        MaskType="Date" AutoComplete="false" runat="server"></cc1:MaskedEditExtender>
                    <cc1:MaskedEditValidator ID="MEditValBillSubmitDate" ControlExtender="MEditBillSubmitDate" ControlToValidate="txtBillSubmitDate" IsValidEmpty="false"
                        InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Bill Submit Date is invalid" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" 
                        EmptyValueBlurredText="Required"
                        MinimumValue="01/01/2016" SetFocusOnError="true" Runat="Server" ValidationGroup="BillRequired"></cc1:MaskedEditValidator>
                </td>
                <td>
                   <asp:TextBox ID="txtBillSubmitDate" runat="server" Width="100px" TabIndex="8" placeholder="dd/mm/yyyy"></asp:TextBox>
                    <asp:Image ID="imgBillSubmitDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Bill No
                    <asp:RequiredFieldValidator ID="RFVBillNo" runat="server" ControlToValidate="txtBillNo" SetFocusOnError="true"   
                       InitialValue="" Text="*" ErrorMessage="Please Enter Bill No" Display="Dynamic" ValidationGroup="BillRequired"></asp:RequiredFieldValidator>
                </td>
                <td>
                   <asp:TextBox ID="txtBillNo" runat="server" Width="100px" TabIndex="8"></asp:TextBox>
                </td>
                <td>
                    Bill Date
                    <cc1:MaskedEditExtender ID="MEditBillDate" TargetControlID="txtBillDate" Mask="99/99/9999" MessageValidatorTip="true" 
                        MaskType="Date" AutoComplete="false" runat="server"></cc1:MaskedEditExtender>
                    <cc1:MaskedEditValidator ID="MEditValBillDate" ControlExtender="MEditBillDate" ControlToValidate="txtBillDate" IsValidEmpty="false"
                        InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Bill Date is invalid" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" 
                        EmptyValueBlurredText="Required"
                        MinimumValue="01/01/2016" SetFocusOnError="true" Runat="Server" ValidationGroup="BillRequired"></cc1:MaskedEditValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtBillDate" runat="server" Width="100px" TabIndex="8" placeholder="dd/mm/yyyy"></asp:TextBox>
                    <asp:Image ID="imgBillDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Bill Amount
                    <asp:RequiredFieldValidator ID="RFVBillAmount" runat="server" ControlToValidate="txtBillAmount" SetFocusOnError="true"   
                       InitialValue="" Text="*" ErrorMessage="Please Enter Bill Amount" Display="Dynamic" ValidationGroup="BillRequired"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtBillAmount" runat="server" Width="100px" TabIndex="8"></asp:TextBox>
                </td>
                <td>
                    Detention Amount
                </td>
                <td>
                    <asp:TextBox ID="txtDetentionAmount" runat="server" Width="100px" TabIndex="8"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Varai Exp
                </td>
                <td>
                    <asp:TextBox ID="txtVaraiExp" runat="server" Width="100px" TabIndex="8"></asp:TextBox>
                </td>
                <td>
                    Empty Cont Rcpt Charges
                </td>
                <td>
                    <asp:TextBox ID="txtEmptyContCharges" runat="server" Width="100px" TabIndex="8"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Total Amount
                    <asp:RequiredFieldValidator ID="RFVTotalAmount" runat="server" ControlToValidate="txtTotalAmount" SetFocusOnError="true"   
                       InitialValue="" Text="*" ErrorMessage="Please Enter Total Amount" Display="Dynamic" ValidationGroup="BillRequired"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtTotalAmount" runat="server" Width="100px" TabIndex="8"></asp:TextBox>
                </td>
                <td>
                    Bill A/C Person Name
                </td>
                <td>
                    <asp:TextBox ID="txtBillingEmpoyee" runat="server" Width="100px" TabIndex="8"></asp:TextBox>
                </td>
            </tr>
       </table> 
       <div class="m clear">
       <div>
        <asp:GridView ID="GridViewBillDetail" runat="server" AutoGenerateColumns="True" CssClass="table"
            Width="100%" DataKeyNames="lId" DataSourceID="DataSourceBillDetail" CellPadding="4">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                
            </Columns>
        </asp:GridView>
        </div>
       </div>
    </fieldset>
        <div id="divDataSource">
        <asp:SqlDataSource ID="DataSourceRate" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetTransDeliveryDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqID" SessionField="TransReqId" />
                <asp:SessionParameter Name="TransporterID" SessionField="TransporterID" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DSTransporterPlaced" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetTransporterPlaced" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransRequestId" SessionField="TransReqId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceBillDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetBillDetailForTransporter" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqID" SessionField="TransReqId" />
                <asp:SessionParameter Name="TransporterID" SessionField="TransporterID" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
</asp:Content>
