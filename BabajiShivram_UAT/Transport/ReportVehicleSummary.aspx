<%@ Page Title="Vehicle Summary Status" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ReportVehicleSummary.aspx.cs" 
    Inherits="Transport_ReportVehicleSummary" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="content2" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <script type="text/javascript">

        function onCalendarHidden() {
            var cal = $find("calReportDate");

            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.removeHandler(row.cells[j].firstChild, "click", call);
                    }
                }
            }
        }

        function onCalendarShown() {

            var cal = $find("calReportDate");

            cal._switchMode("months", true);

            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.addHandler(row.cells[j].firstChild, "click", call);
                    }
                }
            }
        }

        function call(eventElement) {
            var target = eventElement.target;
            switch (target.mode) {
                case "month":
                    var cal = $find("calReportDate");
                    cal._visibleDate = target.date;
                    cal.set_selectedDate(target.date);
                    //cal._switchMonth(target.date);
                    cal._blur.post(true);
                    cal.raiseDateSelectionChanged();
                    break;
            }
        }
</script>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upExpense" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <fieldset><legend>Vehicle Summary Report</legend>
        <asp:UpdatePanel ID="upExpense" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <div class="clear" style="text-align:center;">
                    <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                </div>
                <div class="clear">
                </div>
                <div>
                    <asp:Button ID="btnView" runat="Server" Text="View Report" />
                    
                    <b>Summary Month</b> &nbsp;<asp:TextBox ID="txtReportDate" AutoPostBack="true" runat="server" MaxLength="100" Width="100px"></asp:TextBox>
                    <AjaxToolkit:CalendarExtender ID="calReportDate" runat="server" DefaultView="Months" ClientIDMode="Static"
                         Format="MMM/yyyy" PopupPosition="BottomRight" TargetControlID="txtReportDate"
                         OnClientShown="onCalendarShown" OnClientHidden="onCalendarHidden">
                    </AjaxToolkit:CalendarExtender>
                    &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="lnkXls" runat="server" OnClick="lnkXls_Click" data-tooltip="&nbsp; Export To Excel">
                    <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" />
                    </asp:LinkButton>    
                </div>
                <div>
                <asp:GridView ID="GridViewVehicle" runat="server" AutoGenerateColumns="true" CssClass="table" 
                     DataSourceID="SqlDataSourceExp" AllowSorting="true">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                </div>
                <div>
                    <asp:SqlDataSource ID="SqlDataSourceExp" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="TR_rptVehicleStatusSummary" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter Name="ReportMonth" ControlID="txtReportDate" PropertyName="Text"/>
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
</asp:Content>
