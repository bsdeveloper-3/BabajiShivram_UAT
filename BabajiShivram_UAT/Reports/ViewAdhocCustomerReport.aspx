<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewAdhocCustomerReport.aspx.cs"
    Inherits="Reports_ViewAdhocCustomerReport" MasterPageFile="~/CustomerMaster.master"
    Title="View Customer Report" Culture="en-GB" EnableEventValidation="false" %>

<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upViewReport" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../Images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upViewReport" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                
                <asp:ValidationSummary ID="valSummary" runat="server" ShowSummary="false" ShowMessageBox="true"
                    ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
            </div>
            <div class="clear"></div>
                <fieldset>
                <legend>Generate Report</legend> 
                <p>
                    <asp:Table ID="CustomUITable" runat="server" CssClass="table">
                        <asp:TableRow>
                            <asp:TableCell>
                                Job Date From
                                <cc1:CalendarExtender ID="calFromDate" runat="server" Enabled="True" EnableViewState="False"
                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgFromDate" PopupPosition="BottomRight"
                                    TargetControlID="txtFromDate">
                                </cc1:CalendarExtender>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="txtFromDate" runat="server" MaxLength="10" Width="100px" TabIndex="1"></asp:TextBox>
                                <asp:Image ID="imgFromDate" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif"
                                    runat="server" TabIndex="2" />
                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" ControlToValidate="txtFromDate" SetFocusOnError="true"
                                    ForeColor="red" Text="*" ValidationGroup="Required" Display="Dynamic" ErrorMessage="Please Select Job Date from"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="ComValFromDate" runat="server" ControlToValidate="txtFromDate"
                                    Display="Dynamic" Text="Invalid Date." Type="Date" CultureInvariantValues="false"
                                    Operator="DataTypeCheck" SetFocusOnError="true" ValidationGroup="Required">
                                </asp:CompareValidator>    
                            </asp:TableCell>
                            <asp:TableCell>
                                Job Date To
                                <cc1:CalendarExtender ID="CalToDate" runat="server" Enabled="True" EnableViewState="False"
                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgToDate" PopupPosition="BottomRight"
                                    TargetControlID="txtToDate">
                                </cc1:CalendarExtender>
                                
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="txtToDate" runat="server" MaxLength="10" Width="100px" TabIndex="3"></asp:TextBox>
                                <asp:Image ID="imgToDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                                    runat="server" TabIndex="4"/>
                                <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtToDate" SetFocusOnError="true"
                                    Text="*" ValidationGroup="Required"  Display="Dynamic" ErrorMessage="Please Select Job Date To"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="ComValToDate" runat="server" ControlToValidate="txtToDate"
                                    Display="Dynamic" Text="Invalid Date." Type="Date" CultureInvariantValues="false"
                                    Operator="DataTypeCheck" SetFocusOnError="true" ValidationGroup="Required">
                                </asp:CompareValidator>    
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </p>
                <div class="m clear">
                    <p>
                        <div>
                            <asp:Button ID="btnShowReport" runat="server" Text="Generate Report" ValidationGroup="Required"
                                OnClick="btnShowReport_Click" TabIndex="5"/>
                        </div>
                    </p>
                </div>
               </fieldset> 
                <div class="clear">
                </div>
                <asp:GridView ID="gvViewReport" AutoGenerateColumns="true" AlternatingRowStyle-HorizontalAlign="Left"
                    RowStyle-HorizontalAlign="Left" runat="server" Width="100%" CellPadding="4" CssClass="table"
                    GridLines="Both" HeaderStyle-HorizontalAlign="Left">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div id="divSqlDataSource">
                    <asp:SqlDataSource ID="ViewReportSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="rptAdHocCustomerReport" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:SessionParameter SessionField="ReportId" Name="ReportId" />
                            <asp:SessionParameter SessionField="DateFrom" Name="DateFrom" />
                            <asp:SessionParameter SessionField="DateTo" Name="DateTo" />
                            <asp:SessionParameter SessionField="FinYearId" Name="FinYearId" />
                            <asp:SessionParameter SessionField="CustomerUserId" Name="CustomerUserId" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
