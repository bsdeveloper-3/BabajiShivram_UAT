<%@ Page Title="Billing Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="BillingChartOverall.aspx.cs" Inherits="Reports_BillingChartOverall" EnableEventValidation="false" Culture="en-GB" %>
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
        </div>
    <div class="m"></div>
    <div style="float:left;">
        <div id="divBillingKPI">
        
        <asp:Chart ID="BillingKPIChart" Height="300px" Width="550px" runat="server" BorderlineWidth="2" BorderlineColor="ActiveBorder" BorderlineDashStyle="DashDot" 
            BackSecondaryColor="White" BackGradientStyle="TopBottom" BorderWidth="2px" backcolor="211, 223, 240" BorderColor="#1A3B69">
	        <Series>
		        <asp:Series Name="BillingKPISeries" YValueType="Int32" ChartArea="BillingKPIArea" ChartType="Pie" Color="211, 223, 240"></asp:Series>
	        </Series>
            <Titles>
                <asp:Title Text="Billing KPI Without Rejection" font="Arial, 12pt, style=Bold, Italic" Name="KPI Without Rejection"></asp:Title>
            </Titles>
	        <ChartAreas>
		        <asp:ChartArea Name="BillingKPIArea">
                </asp:ChartArea>
            </ChartAreas>
            <Legends>
                <asp:Legend Name="BillingKPILegends" BorderColor="Green" BorderDashStyle="Solid"></asp:Legend>
            </Legends>
        </asp:Chart>
        </div>

        <div id="divKPIWithRejection">
        <asp:Chart ID="KPIWithRejectChart" Height="300px" Width="550" runat="server" BorderlineWidth="2" BorderlineColor="ActiveBorder" BorderlineDashStyle="DashDot" 
            BackSecondaryColor="White" BackGradientStyle="TopBottom" BorderWidth="2px" backcolor="211, 223, 240" BorderColor="#1A3B69">
	        <Series>
		        <asp:Series Name="Customer" YValueType="Int32" ChartArea="RejectArea" ChartType="Pie" Color="211, 223, 240"></asp:Series>
	        </Series>
            <Titles>
                <asp:Title Text="Billing KPI With Rejection" font="Arial, 12pt, style=Bold, Italic" Name="Customer"></asp:Title>
            </Titles>
	        <ChartAreas>
		        <asp:ChartArea Name="RejectArea">
                </asp:ChartArea>
            </ChartAreas>
            <Legends>
                <asp:Legend Name="RejectLegends" BorderColor="Red" BorderDashStyle="Solid"></asp:Legend> 
            </Legends>
        </asp:Chart>
        </div>
    </div>

    <div style="float:left;">
        <div id="divClearance">
        <asp:Chart ID="ClearanceChart" Height="300px" Width="550px" runat="server" BorderlineWidth="2" BorderlineColor="ActiveBorder" BorderlineDashStyle="DashDot" 
            BackSecondaryColor="White" BackGradientStyle="TopBottom" BorderWidth="2px" backcolor="211, 223, 240" BorderColor="#1A3B69">
	        <Series>
		        <asp:Series Name="Billing KPI - Clearance Date" YValueType="Int32" ChartArea="ClearanceArea" ChartType="Pie" Color="211, 223, 240"></asp:Series>
	        </Series>
            <Titles>
                <asp:Title Text="Billing KPI - Clearance To Dispatch Date" font="Arial, 12pt, style=Bold, Italic" Name="Branch"></asp:Title>
            </Titles>
	        <ChartAreas>
		        <asp:ChartArea Name="ClearanceArea">
                </asp:ChartArea>
            </ChartAreas>
            <Legends>
                <asp:Legend Name="ClearanceLegends" BorderColor="Blue" BorderDashStyle="Solid"></asp:Legend> 
            </Legends>
        </asp:Chart>
        </div>

        <div id="divCustomer" >
        
        </div>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
