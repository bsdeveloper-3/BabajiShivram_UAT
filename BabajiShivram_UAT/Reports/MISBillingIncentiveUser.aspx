<%@ Page Title="Report User" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="MISBillingIncentiveUser.aspx.cs" Inherits="Reports_MISBillingIncentiveUser" %>

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
            <%--<asp:DropDownList ID="ddBabajiBranch" runat="server" AutoPostBack="true"></asp:DropDownList>--%>
            <asp:Button ID="btnView" runat="Server" Text="View Report" />
            <asp:LinkButton ID="lnkKPICriteria" runat="Server" Text="View Incentive Criteria" OnClick="lnkKPICriteria_Click" />
            &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
            </asp:LinkButton>
            
        </div><br />
    <div class="m"></div>
        <fieldset><legend>Billing Stage KPI</legend>
            <asp:GridView ID="gvKPIReport" runat="server" AutoGenerateColumns="False" CssClass="table"
                Width="100%" PagerStyle-CssClass="pgr" CellPadding="4" AllowPaging="True" 
                AllowSorting="True" PageSize="200">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="BillingStage" HeaderText="Billing Stage"/>
                    <asp:BoundField DataField="UserName" HeaderText="User"/>
                    <asp:BoundField DataField="TotalJob" HeaderText="Total Job"/>
                    <asp:BoundField DataField="KPISameDay" HeaderText="KPI Same Day %"/>
                    <asp:BoundField DataField="KPIOneDay" HeaderText="KPI One Day %"/>
                    <asp:BoundField DataField="KPITwoDay" HeaderText="KPI Two Days %"/>
                    <asp:BoundField DataField="KPIAbove3Day" HeaderText="Above 3 Days %"/>
                </Columns>
            </asp:GridView>
        </fieldset>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

