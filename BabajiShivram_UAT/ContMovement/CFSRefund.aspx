<%@ Page Title="CFS Refund Follow-Up" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="CFSRefund.aspx.cs" Inherits="ContMovement_CFSRefund" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
    </asp:ScriptManager>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upFollowupStatus" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../Images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upFollowupStatus" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="vsMovementDetail" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="vgRequired" CssClass="errorMsg" />
            </div>
            <div class="clear">
            </div>
            <asp:Button ID="btnJobCheck" runat="server" Text="Follow Up" OnClick="btnJobCheck_Click" />
            <div class="clear"></div>
            <fieldset class="fieldset-AutoWidth">
                <legend>CFS Refund Status</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:DataFilter ID="DataFilter1" runat="server" />
                        </div>
                        <div class="fleft" style="margin-left: 2px;">
                            <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                                <asp:Image ID="Image1" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                            </asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>
                <div class="clear">
                </div>
                <asp:GridView ID="gvMovementDetail" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr"
                    DataKeyNames="JobId" AllowPaging="True" AllowSorting="True" CssClass="table" PageSize="20"
                    PagerSettings-Position="TopAndBottom" DataSourceID="DataSourceRefund">
                    <Columns>
                        <asp:TemplateField ShowHeader="false">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelectJob" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" SortExpression="JobRefNo" ReadOnly="true" />
                        <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" ReadOnly="true" />
                        <asp:BoundField DataField="CFS" HeaderText="CFS" SortExpression="CFS" ReadOnly="true" />
                        <asp:BoundField DataField="ShippingName" HeaderText="Shipping Line" SortExpression="ShippingName" ReadOnly="true" />
                        <asp:BoundField DataField="InwardDate" HeaderText="Inward Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="InwardDate" ReadOnly="true" />
                        <asp:BoundField DataField="IGMDate" HeaderText="IGM Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="IGMDate" ReadOnly="true" />
                        <asp:BoundField DataField="BOEDate" HeaderText="BOE Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="BOEDate" ReadOnly="true" />
                        <asp:BoundField DataField="BOENo" HeaderText="BOE No" SortExpression="BOENo" ReadOnly="true" />
                        <asp:BoundField DataField="Container" HeaderText="Container" SortExpression="Container" ReadOnly="true" />
                        <asp:BoundField DataField="Aging" HeaderText="Aging" SortExpression="Aging" ReadOnly="true" />
                        <asp:BoundField DataField="FollowUpRemark" HeaderText="FollowUp Remark" SortExpression="FollowUpRemark" ReadOnly="true" />
                        <asp:BoundField DataField="FollowUpDate" HeaderText="FollowUp Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="FollowUpDate" ReadOnly="true" />
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="DataSourceRefund" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CM_GetPendingCFSRefund" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="lUser" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
            <%-- START  : Pop-up for Documents --%>
            <div>
                <asp:HiddenField ID="hdnPopup" runat="server" />
                <cc1:ModalPopupExtender ID="mpeStatus" runat="server" TargetControlID="hdnPopup" BackgroundCssClass="modalBackground" CancelControlID="imgClose"
                    PopupControlID="pnlStatus" DropShadow="true">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pnlStatus" runat="server" CssClass="modalPopup1" BackColor="#F5F5DC" Width="670px" Height="200px" BorderStyle="Solid" BorderWidth="1px">
                    <div id="div2" runat="server">
                        <table width="100%">
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td align="center">Update Status -
                                    <asp:Label ID="lblJobRefNo" runat="server"></asp:Label>
                                    <span style="float: right">
                                        <asp:ImageButton ID="imgClose" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgClose_Click" ToolTip="Close" />
                                    </span>
                                </td>
                            </tr>
                        </table>
                        <div align="center">
                            <asp:Panel ID="pnlDownloadDocs" runat="server" Width="660px" Height="200px" ScrollBars="Auto" BorderStyle="Solid" BorderWidth="1px">
                                <table class="table">
                                    <tr>
                                        <td>
                                            Status
                                            <asp:RequiredFieldValidator ID="rfvStatus" runat="server" ControlToValidate="ddRefundStauts" Text="Required"
                                               InitialValue="0" ValidationGroup="RequiredStauts" ></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddRefundStauts" runat="server">
                                                <asp:ListItem Text="--Status--" Value="0" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Refund NA" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Refund In Process" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Refund Processed" Value="3"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            Follow Up Date
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="RFVDate" runat="server" ControlToValidate="txtFollowUpDate" Text="Required"
                                               InitialValue="" ValidationGroup="RequiredStauts" ></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtFollowUpDate" runat="server" Width="100px"></asp:TextBox>
                                            <asp:Image ID="imgFollowUpDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                            <cc1:CalendarExtender ID="CalFollowDate" runat="server" Enabled="True" EnableViewState="False"
                                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgFollowUpDate" PopupPosition="BottomRight"
                                                TargetControlID="txtFollowUpDate">
                                            </cc1:CalendarExtender>
                                            <cc1:MaskedEditExtender ID="MEditFollowUpDate" TargetControlID="txtFollowUpDate" Mask="99/99/9999" MessageValidatorTip="true" 
                                                MaskType="Date" AutoComplete="false" runat="server"></cc1:MaskedEditExtender>
                                            <cc1:MaskedEditValidator ID="MEditValFollowUpDate" ControlExtender="MEditFollowUpDate" ControlToValidate="txtFollowUpDate" 
                                                IsValidEmpty="false" SetFocusOnError="true" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date"
                                                MinimumValue="01/01/2019" MaximumValue='<%=DateTime.Now() %>' Runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RequiredFieldValidator ID="RFVRemark" runat="server" ControlToValidate="txtFollowUpRemark" Text="Required"
                                               InitialValue="" ValidationGroup="RequiredStauts" ></asp:RequiredFieldValidator>
                                            Follow Up Remark
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtFollowUpRemark" runat="server" TextMode="MultiLine" MaxLength="200"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Button ID="btnUpdateStatus" Text="Update Stauts" OnClick="btnUpdateStatus_Click" runat="server" ValidationGroup="RequiredStauts" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <%-- END    : Pop-up for Follow Up Status--%>
                        
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


