<%@ Page Title="Pending SB Filing" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PendingSBFiling.aspx.cs"
    Inherits="ExportCHA_PendingSBFiling" EnableEventValidation="false" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingFiling" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upPendingFiling" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="vsAddFilingDetail" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" />
            </div>
            <div class="clear">
            </div>
            <fieldset class="fieldset-AutoWidth">
                <legend>Pending For Filing</legend>
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
                                    ToolTip="Click To Change Filing Detail."></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update Filing Detail" runat="server"
                                    Text="Update" ValidationGroup="Required"></asp:LinkButton>
                                <asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel Filing Detail Update" CausesValidation="false"
                                    runat="server" Text="Cancel"></asp:LinkButton>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" SortExpression="JobRefNo" ReadOnly="true" />
                        <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" ReadOnly="true" />
                        <asp:BoundField DataField="CustRefNo" HeaderText="Cust Ref No" SortExpression="CustRefNo" ReadOnly="true" />
                        <asp:BoundField DataField="Shipper" HeaderText="Shipper" SortExpression="Shipper" ReadOnly="true" />
                        <asp:BoundField DataField="Consignee" HeaderText="Consignee" SortExpression="Consignee" ReadOnly="true" />
                        <asp:BoundField DataField="TransMode" HeaderText="Mode" SortExpression="TransMode" ReadOnly="true" />
                        <asp:BoundField DataField="PortOfLoading" HeaderText="Port Of Loading" SortExpression="PortOfLoading" ReadOnly="true" />
                        <asp:BoundField DataField="PortOfDischarge" HeaderText="Port Of Discharge" SortExpression="PortOfDischarge" ReadOnly="true" />
                        <asp:BoundField DataField="ChecklistDate" HeaderText="Checklist Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ChecklistDate" ReadOnly="true" />
                        <asp:TemplateField HeaderText="SB No" SortExpression="SBNo">
                            <ItemTemplate>
                                <asp:Label ID="lblSBNo" runat="server" Text='<%# Bind("SBNo")%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSBNo" runat="server" Width="70px" MaxLength="7" Text='<%# Bind("SBNo")%>'></asp:TextBox>
                                <asp:RegularExpressionValidator ID="REVSBNo" runat="server" ErrorMessage="Please Enter 7 digit SB Number."
                                    Text="*" ValidationExpression="^[0-9]{7}$" ControlToValidate="txtSBNo" Display="Dynamic"
                                    ValidationGroup="Required" SetFocusOnError="true"></asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="reqvSBNo" runat="server" Text="*" ControlToValidate="txtSBNo" SetFocusOnError="true"
                                    ErrorMessage="Please Enter SB No." ValidationGroup="Required" Display="Dynamic"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SB Date" SortExpression="SBDate">
                            <ItemTemplate>
                                <asp:Label ID="lblSBDate" runat="server" Text='<%# Bind("SBDate","{0:dd/MM/yyyy}")%>' ></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSBDate" runat="server" Width="80px" MaxLength="10" Text='<%# Bind("SBDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                <cc1:CalendarExtender ID="calSBDate" runat="server" Enabled="True"
                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgSBDate" PopupPosition="BottomRight"
                                    TargetControlID="txtSBDate">
                                </cc1:CalendarExtender>
                                <asp:Image ID="imgSBDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                <cc1:MaskedEditExtender ID="MskExtSBDate" TargetControlID="txtSBDate" Mask="99/99/9999" MessageValidatorTip="true"
                                    MaskType="Date" AutoComplete="false" runat="server">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditValidator ID="MskValSBDate" ControlExtender="MskExtSBDate" ControlToValidate="txtSBDate" IsValidEmpty="false"
                                    EmptyValueMessage="Please Enter SB Date." EmptyValueBlurredText="*" InvalidValueMessage="SB Date is invalid"
                                    InvalidValueBlurredMessage="Invalid Date" MinimumValueMessage="Invalid SB Date" MaximumValueMessage="Invalid SB Date"
                                    MinimumValue="01/07/2014" MaximumValue="31/12/2025"
                                    runat="Server" SetFocusOnError="true" ValidationGroup="Required"></cc1:MaskedEditValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Marked/Appraising">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddMarkAppraising" runat="server" Width="80px">
                                    <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Marked" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Appraising" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvMarkAppraising" runat="server" Text="*" ControlToValidate="ddMarkAppraising" SetFocusOnError="true"
                                    InitialValue="0" ErrorMessage="Please Select Marked/Appraising." ValidationGroup="Required" Display="Dynamic"></asp:RequiredFieldValidator>
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
                    SelectCommand="EX_GetJobDetailForSBFiling" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

