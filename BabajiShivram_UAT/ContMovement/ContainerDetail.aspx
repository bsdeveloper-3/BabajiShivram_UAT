<%@ Page Title="Container Detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ContainerDetail.aspx.cs"
    Inherits="ContMovement_ContainerDetail" ValidateRequest="false" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .heading {
            line-height: 20px;
        }

        .modalBackground {
            background-color: black;
            filter: alpha(opacity=90);
            opacity: 0.5;
        }

        .modalPopup {
            border-radius: 5px;
            background-color: #FFFFFF;
            border-width: 1px;
            border-style: solid;
            border-color: black;
            padding-top: 5px;
            padding-left: 3px;
            width: 300px;
            height: 140px;
        }

        .modalPopup1 {
            border-radius: 5px;
            background-color: #FFFFFF;
            border-width: 1px;
            border-style: solid;
            border-color: black;
            padding-top: 5px;
            padding-left: 3px;
            width: 600px;
            height: 300px;
        }
    </style>

    <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
    </asp:ScriptManager>

    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlContainerDetail" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../Images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upnlContainerDetail" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div class="clear">
                <asp:Button ID="btnCancelContainer" runat="server" OnClick="btnCancelContainer_Click" Text="Back" />
            </div>
            <fieldset class="fieldset-AutoWidth">
                <asp:FormView ID="fvJobDetail" HeaderStyle-Font-Bold="true" runat="server" DataSourceID="SqlDataSourceJobDetail"
                    DataKeyNames="JobId" Width="100%">
                    <ItemTemplate>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>Job Ref No
                                </td>
                                <td>
                                    <b>
                                        <span>
                                            <%# Eval("JobRefNo")%>
                                        </span>
                                    </b>
                                </td>
                                <td>Branch
                                </td>
                                <td>
                                    <span>
                                        <%# Eval("BranchName")%>
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td>Customer
                                </td>
                                <td>
                                    <span>
                                        <%# Eval("CustomerName")%>
                                    </span>
                                </td>
                                <td>Consignee</td>
                                <td>
                                    <span>
                                        <%# Eval("ConsigneeName")%>
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td>ETA Date</td>
                                <td>
                                    <span>
                                        <%# Eval("ETADate","{0:dd/MM/yyyy}")%>
                                    </span>
                                </td>
                                <td>Dispatch Date</td>
                                <td>
                                    <span>
                                        <%# Eval("LastDispatchDate","{0:dd/MM/yyyy}")%>
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td>Sum Of 20
                                </td>
                                <td>
                                    <span>
                                        <%# Eval("SumOf20")%>
                                    </span>
                                </td>
                                <td>Sum Of 20</td>
                                <td>
                                    <span>
                                        <%# Eval("SumOf40")%>
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td>Container Type</td>
                                <td>
                                    <span>
                                        <%# Eval("ContainerType")%>
                                    </span>
                                </td>
                                <td>Created Date</td>
                                <td>
                                    <span>
                                        <%# Eval("JobCreationDate", "{0:dd/MM/yyyy}")%>
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td>Created By</td>
                                <td>
                                    <span>
                                        <%# Eval("JobCreatedBy")%>
                                    </span>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>CFS Name</td>
                                <td>
                                    <span>
                                        <%# Eval("CFSName")%>
                                    </span>
                                </td>
                                <td>Nominated CFS Name</td>
                                <td>
                                    <span>
                                        <%# Eval("NominatedCFSName")%>
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td>Shipping Line Date</td>
                                <td>
                                    <span>
                                        <%# Eval("ShippingLineDate", "{0:dd/MM/yyyy}")%>
                                    </span>
                                </td>
                                <td>Confirmed By Line Date</td>
                                <td>
                                    <span>
                                        <%# Eval("ConfirmedByLineDate", "{0:dd/MM/yyyy}")%>
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td>Movement Complete Date</td>
                                <td>
                                    <span>
                                        <%# Eval("MovementCompDate", "{0:dd/MM/yyyy}")%>
                                    </span>
                                </td>
                                <td>Container Received At CFS Date</td>
                                <td>
                                    <span>
                                        <%# Eval("EmptyContReturnDate", "{0:dd/MM/yyyy}")%>
                                    </span>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:FormView>
            </fieldset>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="vsContainerDetail" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="vgContainerDetail" CssClass="errorMsg" />
            </div>
            <fieldset>
                <legend>Delivery Detail</legend>
                <div class="clear">
                </div>
                <div>
                    <asp:GridView ID="gvDeliveryDetails" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr"
                        DataKeyNames="lid" CssClass="table" PageSize="20" DataSourceID="DataSourceDeliveryDetails" OnPreRender="gvDeliveryDetails_PreRender">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" SortExpression="VehicleNo" ReadOnly="true" />
                            <asp:BoundField DataField="TransporterName" HeaderText="Transporter Name" SortExpression="TransporterName" ReadOnly="true" />
                            <asp:BoundField DataField="LRNo" HeaderText="LR No" SortExpression="LRNo" ReadOnly="true" />
                            <asp:BoundField DataField="LRDate" HeaderText="LR Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LRDate" ReadOnly="true" />
                            <asp:BoundField DataField="BabajiChallanNo" HeaderText="Challan No" SortExpression="BabajiChallanNo" ReadOnly="true" />
                            <asp:BoundField DataField="BabajiChallanDate" HeaderText="Challan Date" SortExpression="BabajiChallanDate" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                            <asp:BoundField DataField="DispatchDate" HeaderText="Dispatch Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="DispatchDate" ReadOnly="true" />
                        </Columns>
                    </asp:GridView>
                </div>
            </fieldset>
            <fieldset class="fieldset-AutoWidth">
                <legend>Empty Container Received at Yard Detail</legend>
                <div class="clear">
                </div>
                <div>
                    <asp:GridView ID="gvContRecdCFS" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr"
                        DataKeyNames="lid" OnRowCommand="gvContRecdCFS_RowCommand" AllowPaging="True" AllowSorting="True" CssClass="table" PageSize="20"
                        PagerSettings-Position="TopAndBottom" OnRowDataBound="gvContRecdCFS_RowDataBound" DataSourceID="DataSourceContReceived"
                        OnPreRender="gvContRecdCFS_PreRender">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:ButtonField CommandName="AddCFS" Text="Add Date" ButtonType="Button" />
                            <asp:BoundField DataField="ContainerNo" HeaderText="Container No" SortExpression="ContainerNo" ReadOnly="true" />
                            <asp:BoundField DataField="ContainerSize" HeaderText="Container Size" SortExpression="ContainerSize" ReadOnly="true" />
                            <asp:BoundField DataField="ContainerType" HeaderText="Container Type" SortExpression="ContainerType" ReadOnly="true" />
                            <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" SortExpression="VehicleNo" ReadOnly="true" Visible="false" />
                            <asp:BoundField DataField="TransporterName" HeaderText="Transporter Name" SortExpression="TransporterName" ReadOnly="true" Visible="false" />
                            <asp:BoundField DataField="LRNo" HeaderText="LR No" SortExpression="LRNo" ReadOnly="true" Visible="false" />
                            <asp:BoundField DataField="LRDate" HeaderText="LR Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LRDate" ReadOnly="true" Visible="false" />
                            <asp:BoundField DataField="BabajiChallanNo" HeaderText="Challan No" SortExpression="BabajiChallanNo" ReadOnly="true" Visible="false" />
                            <asp:BoundField DataField="EmptyValidityDate" HeaderText="Empty Validity Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="EmptyValidityDate" ReadOnly="true" />
                            <asp:TemplateField HeaderText="Empty Container Received at Yard Date" SortExpression="ContCFSReceivedDate">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnDispatchDate" runat="server" Value='<%#Eval("DispatchDate") %>' />
                                    <asp:HiddenField ID="hdnlid" runat="server" Value='<%#Eval("lid") %>' />
                                    <asp:TextBox ID="txtContRecdAtCFSDate" runat="server" Width="80px" MaxLength="10" Text='<%# Bind("ContRecdAtCFSDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                    <cc1:CalendarExtender ID="calContRecdAtCFSDate" runat="server" Enabled="True"
                                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgContRecdAtCFSDate" PopupPosition="BottomRight"
                                        TargetControlID="txtContRecdAtCFSDate">
                                    </cc1:CalendarExtender>
                                    <asp:Image ID="imgContRecdAtCFSDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                    <cc1:MaskedEditExtender ID="meeContRecdAtCFSDate" TargetControlID="txtContRecdAtCFSDate" Mask="99/99/9999" MessageValidatorTip="true"
                                        MaskType="Date" AutoComplete="false" runat="server">
                                    </cc1:MaskedEditExtender>
                                    <cc1:MaskedEditValidator ID="mevContRecdAtCFSDate" ControlExtender="meeContRecdAtCFSDate" ControlToValidate="txtContRecdAtCFSDate" IsValidEmpty="true"
                                        EmptyValueMessage="Please Enter Container Received at CFS Date." EmptyValueBlurredText="*" InvalidValueMessage="Cont Received at CFS Date is invalid"
                                        InvalidValueBlurredMessage="Invalid Date" MinimumValueMessage="Invalid Cont Received at CFS Date" MaximumValueMessage="Invalid Cont Received at CFS Date"
                                        runat="Server" SetFocusOnError="true" ValidationGroup="vgRequired"></cc1:MaskedEditValidator>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerTemplate>
                            <asp:GridViewPager runat="server" />
                        </PagerTemplate>
                    </asp:GridView>
                </div>
            </fieldset>
            <asp:SqlDataSource ID="DataSourceDeliveryDetails" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="CM_GetContDeliveryDetails" SelectCommandType="StoredProcedure" DataSourceMode="DataSet" EnableCaching="true"
                CacheDuration="300" CacheKeyDependency="MyCacheDependency">
                <SelectParameters>
                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourceJobDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="CM_GetMovementDetailByJobId" SelectCommandType="StoredProcedure" DataSourceMode="DataSet" EnableCaching="true"
                CacheDuration="300" CacheKeyDependency="MyCacheDependency">
                <SelectParameters>
                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="DataSourceContReceived" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="CM_GetContainerDetails" SelectCommandType="StoredProcedure" InsertCommand="CM_insContRecdCFSDetail"
                InsertCommandType="StoredProcedure" OnInserted="DataSourceContReceived_Inserted" OnInserting="DataSourceContReceived_Inserting">
                <SelectParameters>
                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                </SelectParameters>
                <UpdateParameters>
                    <asp:Parameter Name="ContainerDetailId" Type="Int32" />
                    <asp:Parameter Name="ContRecdAtCFSDate" DbType="Date" />
                    <asp:SessionParameter Name="lUser" SessionField="UserId" />
                    <asp:Parameter Name="OutPut" Type="Int32" Direction="Output" Size="4" />
                </UpdateParameters>
            </asp:SqlDataSource>
            <%-- Popup for Delivery Detail --%>
            <asp:LinkButton ID="lnkbtnContRecdCFSDate" runat="server" Text=""></asp:LinkButton>
            <cc1:ModalPopupExtender ID="mpeContRecdCFSDate" runat="server" TargetControlID="lnkbtnContRecdCFSDate" CancelControlID="btnCancel_ContRecdCFSDate"
                PopupControlID="pnlContRecdCFSDate" BackgroundCssClass="modalBackground" CacheDynamicResults="false">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pnlContRecdCFSDate" runat="server" CssClass="ModalPopupPanel" Style="border-radius: 5px; padding: 5px">
                <div class="header">
                    <div class="fleft">
                        Delivery Detail
                    </div>
                    <div class="fright">
                        <asp:Button ID="btnCancel_ContRecdCFSDate" runat="server" ToolTip="Close" Text="Close" Style="background: white; color: black" />
                    </div>
                </div>
                <div id="dvContCFSDate" runat="server" style="height: 650px; width: 800px; overflow: auto; padding: 5px">
                    <fieldset>
                        <div>
                        </div>
                    </fieldset>
                </div>
            </asp:Panel>
            <%-- Popup for Delivery Detail --%>

            <div>
                <asp:SqlDataSource ID="SqlDataSourceCustomer" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetCustomerMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSourceBranch" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetBabajiBranch" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

