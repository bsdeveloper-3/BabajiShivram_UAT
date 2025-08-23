<%@ Page Title="Billing Stage Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="BillingChartStage2.aspx.cs" Inherits="Reports_BillingChartStage2" EnableEventValidation="false" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upBillReport" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>  
    <asp:UpdatePanel ID="upBillReport" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
    <div>
        <div class="clear" style="text-align:center;">
            <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
        </div>
        <div class="clear">
        </div>
        <div>
            <b>Report Month</b> &nbsp;<asp:TextBox ID="txtReportDate" AutoPostBack="true" runat="server" MaxLength="100" Width="100px"></asp:TextBox>
            <AjaxToolkit:CalendarExtender ID="calReportDate" runat="server" DefaultView="Months" ClientIDMode="Static"
                    Format="MMM/yyyy" PopupPosition="BottomRight" TargetControlID="txtReportDate"
                    OnClientShown="onCalendarShown" OnClientHidden="onCalendarHidden">
            </AjaxToolkit:CalendarExtender>
            <asp:Button ID="btnView" runat="Server" Text="View Report" />
        </div><br />
    <div class="m"></div>
        <fieldset><legend>Billing Stage KPI</legend>
            <asp:GridView ID="gvKPIReport" runat="server" AutoGenerateColumns="False" CssClass="table"
                Width="100%" PagerStyle-CssClass="pgr" CellPadding="4" AllowPaging="True" 
                AllowSorting="True" PageSize="20">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="BillingStage" HeaderText="Billing Stage"/>
                    <asp:BoundField DataField="TotalJob" HeaderText="Total Job"/>
                    <asp:BoundField DataField="AvgHour" HeaderText="Average Hour"/>
                    <asp:BoundField DataField="RejectCount" HeaderText="Total Reject"/>
                    <asp:BoundField DataField="RejectOtherDept" HeaderText="Rejected By Other Dept"/>
                    <asp:BoundField DataField="AvgPending" HeaderText="Pending Job (More Than 7 Days)"/>
                    <asp:BoundField DataField="ErrorRate" HeaderText="Rejection Error %"/>
                </Columns>
            </asp:GridView>
        </fieldset>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

