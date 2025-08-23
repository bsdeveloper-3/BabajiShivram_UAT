<%@ Page Title="Pending Form 13" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PendingForm13.aspx.cs"
    Inherits="ExportCHA_PendingForm13" EnableEventValidation="false" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingForm13" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPendingForm13" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="vsAddFilingDetail" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" />
            </div>
            <div class="clear">
            </div>
            <fieldset class="fieldset-AutoWidth">
                <legend>Pending Form 13</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:DataFilter ID="DataFilter1" runat="server" />
                        </div>
                        <div class="fleft" style="margin-left: 2px; padding-top: 3px;">
                            <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                                <asp:Image ID="Image1" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                            </asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>
                <div class="clear">
                </div>
                <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr"
                    DataKeyNames="JobId" OnRowCommand="gvJobDetail_RowCommand" AllowPaging="True" AllowSorting="True" CssClass="table" PageSize="20"
                    PagerSettings-Position="TopAndBottom" DataSourceID="PendingNotingSqlDataSource" OnRowDataBound="gvJobDetail_RowDataBound"
                    OnPreRender="gvJobDetail_PreRender" OnRowEditing="gvJobDetail_RowEditing" OnRowCancelingEdit="gvJobDetail_RowCancelingEdit"
                    OnRowUpdating="gvJobDetail_RowUpdating">
                    <Columns>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" CommandName="Edit" runat="server" Text="Edit"
                                    ToolTip="Click To Update Form 13 Details."></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update Form 13 Detail" runat="server"
                                    Text="Update" ValidationGroup="Required"></asp:LinkButton>
                                <asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel Form 13 Detail Updating." CausesValidation="false"
                                    runat="server" Text="Cancel"></asp:LinkButton>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="BS Job No" SortExpression="JobRefNo">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkJobRefNo" ToolTip="Show Job Details." CommandName="select" runat="server" Text='<%#Eval("JobRefNo") %>'
                                    CommandArgument='<%#Eval("JobId")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" Visible="false" />
                        <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" ReadOnly="true" />
                        <asp:BoundField DataField="CustRefNo" HeaderText="Cust Ref No" SortExpression="CustRefNo" ReadOnly="true" />
                        <asp:BoundField DataField="Shipper" HeaderText="Shipper" SortExpression="Shipper" ReadOnly="true" />
                        <asp:BoundField DataField="Consignee" HeaderText="Consignee" SortExpression="Consignee" ReadOnly="true" />
                        <asp:BoundField DataField="PortOfDischarge" HeaderText="Port Of Discharge" SortExpression="PortOfDischarge" ReadOnly="true" />
                        <asp:BoundField DataField="SBNo" HeaderText="SB No" SortExpression="SBNo" ReadOnly="true" />
                        <asp:BoundField DataField="SBDate" HeaderText="SB Date" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                        <asp:BoundField DataField="LEODate" HeaderText="Supretendent LEO Date" SortExpression="Supretendent LEO Date" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                        <asp:TemplateField HeaderText="Form 13 Date" SortExpression="Form13Date">
                            <ItemTemplate>
                                <asp:Label ID="lblForm13Date" Text='<%# Bind("Form13Date","{0:dd/MM/yyyy}")%>' runat="server"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtForm13Date" runat="server" Width="80px" MaxLength="10" Text='<%# Bind("Form13Date","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                <cc1:CalendarExtender ID="calForm13Date" runat="server" Enabled="True"
                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgForm13Date" PopupPosition="BottomRight"
                                    TargetControlID="txtForm13Date">
                                </cc1:CalendarExtender>
                                <asp:Image ID="imgForm13Date" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                <cc1:MaskedEditExtender ID="MskExtForm13Date" TargetControlID="txtForm13Date" Mask="99/99/9999" MessageValidatorTip="true"
                                    MaskType="Date" AutoComplete="false" runat="server">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditValidator ID="MskValForm13Date" ControlExtender="MskExtForm13Date" ControlToValidate="txtForm13Date" IsValidEmpty="true"
                                    EmptyValueMessage="Please Enter Form 13 Date." EmptyValueBlurredText="*" InvalidValueMessage="Form 13 Date is invalid"
                                    InvalidValueBlurredMessage="Invalid Date" MinimumValueMessage="Invalid Form 13 Date" MaximumValueMessage="Invalid Form 13 Date"
                                    MinimumValue="01/07/2014" MaximumValue="31/12/2025"
                                    runat="Server" SetFocusOnError="true" ValidationGroup="Required"></cc1:MaskedEditValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Transporter Hand Over Date" SortExpression="TransHandOverDate">
                            <ItemTemplate>
                                <asp:Label ID="lblTransHandOverDate" Text='<%# Bind("TransHandOverDate","{0:dd/MM/yyyy}")%>' runat="server"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtTransHandOverDate" runat="server" Width="80px" MaxLength="10" Text='<%# Bind("TransHandOverDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                <cc1:CalendarExtender ID="calTransHandOverDate" runat="server" Enabled="True"
                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgTransHandOverDate" PopupPosition="BottomRight"
                                    TargetControlID="txtTransHandOverDate">
                                </cc1:CalendarExtender>
                                <asp:Image ID="imgTransHandOverDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                <cc1:MaskedEditExtender ID="MskExtTransHandOverDate" TargetControlID="txtTransHandOverDate" Mask="99/99/9999" MessageValidatorTip="true"
                                    MaskType="Date" AutoComplete="false" runat="server">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditValidator ID="MskValTransHandOverDate" ControlExtender="MskExtTransHandOverDate" ControlToValidate="txtTransHandOverDate" IsValidEmpty="true"
                                    EmptyValueMessage="Please Enter Transporter Hand Over Date." EmptyValueBlurredText="*" InvalidValueMessage="Transporter Hand Over Date is invalid"
                                    InvalidValueBlurredMessage="Invalid Date" MinimumValueMessage="Invalid Transporter Hand Over Date" MaximumValueMessage="Invalid Transporter Hand Over Date"
                                    MinimumValue="01/07/2014" MaximumValue="31/12/2025"
                                    runat="Server" SetFocusOnError="true" ValidationGroup="Required"></cc1:MaskedEditValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Container Get In Date" SortExpression="ContainerGetInDate">
                            <ItemTemplate>
                                <asp:Label ID="lblContainerGetInDate" Text='<%# Bind("ContainerGetInDate","{0:dd/MM/yyyy}")%>' runat="server"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtContainerGetInDate" runat="server" Width="80px" MaxLength="10" Text='<%# Bind("ContainerGetInDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                <cc1:CalendarExtender ID="calContGetInDate" runat="server" Enabled="True"
                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgContainerGetInDate" PopupPosition="BottomRight"
                                    TargetControlID="txtContainerGetInDate">
                                </cc1:CalendarExtender>
                                <asp:Image ID="imgContainerGetInDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                <cc1:MaskedEditExtender ID="MskExtContainerGetInDate" TargetControlID="txtContainerGetInDate" Mask="99/99/9999" MessageValidatorTip="true"
                                    MaskType="Date" AutoComplete="false" runat="server">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditValidator ID="MskValContainerGetInDate" ControlExtender="MskExtContainerGetInDate" ControlToValidate="txtContainerGetInDate" IsValidEmpty="true"
                                    EmptyValueMessage="Please Enter Container Get In Date." EmptyValueBlurredText="*" InvalidValueMessage="Container Get In Date is invalid"
                                    InvalidValueBlurredMessage="Invalid Date" MinimumValueMessage="Invalid Container Get In Date" MaximumValueMessage="Invalid Container Get In Date"
                                    MinimumValue="01/07/2014" MaximumValue="31/12/2025"
                                    runat="Server" SetFocusOnError="true" ValidationGroup="Required"></cc1:MaskedEditValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="PendingNotingSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="EX_GetPendingForm13" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

