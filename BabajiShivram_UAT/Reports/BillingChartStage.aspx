<%@ Page Title="Billing Stage Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="BillingChartStage.aspx.cs" Inherits="Reports_BillingChartStage" EnableEventValidation="false" Culture="en-GB" %>
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
    <div style="float:left;">
        <div id="divBillingStage">
        <asp:Chart ID="ChartStage" runat="server" Height="400px" Width="500px" Palette="Excel"
            BackColor="#D3DFF0" BorderlineDashStyle="DashDot" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="181, 64, 1">
            <Titles>
                <asp:Title Name="Billing Stage Aging"></asp:Title>
            </Titles>
            <Legends>
                <asp:Legend Alignment="Center" Docking="Top" IsTextAutoFit="true" Name="LegendStage" LegendStyle="Row"></asp:Legend>
            </Legends>
            <Series>
                <asp:Series Name="Billing Stage (Avg Hours)" BackSecondaryColor="WindowFrame"></asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="AreaStage" BorderColor="64, 64, 64, 64" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                    <axisy LineColor="64, 64, 64, 64">
						<LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
						<MajorGrid LineColor="64, 64, 64, 64" />
					</axisy>
					<axisx LineColor="64, 64, 64, 64">
						<MajorGrid LineColor="64, 64, 64, 64" />
					</axisx>
                </asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
        
        </div>
        <div id="divDraft">
        <asp:Chart ID="ChartDraft" runat="server" Height="400px" Width="500px" Palette="Berry"
            BackColor="#D3DFF0" BorderlineDashStyle="DashDot" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="181, 64, 1">
            <Titles>
                <asp:Title ShadowOffset="1" Name="Draft"></asp:Title>
            </Titles>
            <Legends>
                <asp:Legend Alignment="Center" Docking="Top" IsTextAutoFit="true" Name="DraftStage" LegendStyle="Row"></asp:Legend>
            </Legends>
            <Series>
                <asp:Series Name="Draft Stage" BackSecondaryColor="WindowFrame"></asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="AreaDraft" BorderColor="64, 64, 64, 64" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom"></asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
        </div>
    </div>

    <div style="float:left;">
        <div id="divKPIScrutiny">
        <asp:Chart ID="ChartScrutiny" runat="server" Height="400px" Width="500px" Palette="Bright"
            BackColor="#F3DFC1" BorderlineDashStyle="DashDot" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="181, 64, 1">
            <Titles>
                <asp:Title ShadowOffset="1" Name="Scrutiny"></asp:Title>
            </Titles>
            <Legends>
                <asp:Legend Alignment="Center" Docking="Top" IsTextAutoFit="true" Name="ScrutinyStage" LegendStyle="Row"></asp:Legend>
            </Legends>
            <Series>
                <asp:Series Name="Scrutiny Stage" BackSecondaryColor="WindowFrame"></asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="AreaScrutiny" BorderWidth="0"></asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
        </div>
    
        <div id="divDraftCheck">
        <asp:Chart ID="ChartDraftCheck" runat="server" Height="400px" Width="500px" Palette="Chocolate"
            BackColor="#F3DFC1" BorderlineDashStyle="DashDot" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="181, 64, 1">
            <Titles>
                <asp:Title ShadowOffset="1" Name="Draft Check"></asp:Title>
            </Titles>
            <Legends>
                <asp:Legend Alignment="Center" Docking="Top" IsTextAutoFit="true" Name="DraftCheckStage" LegendStyle="Row"></asp:Legend>
            </Legends>
            <Series>
                <asp:Series Name="Draft Check Stage" BackSecondaryColor="WindowFrame"></asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="AreaDraftCheck" BorderWidth="0"></asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
        </div>
    
    </div>

    <div style="float:left;">
        <div id="divFinalType">
        <asp:Chart ID="ChartFinalType" runat="server" Height="400px" Width="500px" Palette="EarthTones"
            BackColor="#D3DFF0" BorderlineDashStyle="DashDot" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="181, 64, 1">
            <Titles>
                <asp:Title ShadowOffset="1" Name="Draft"></asp:Title>
            </Titles>
            <Legends>
                <asp:Legend Alignment="Center" Docking="Top" IsTextAutoFit="true" Name="FinalTypeStage" LegendStyle="Row"></asp:Legend>
            </Legends>
            <Series>
                <asp:Series Name="FinalType Stage" BackSecondaryColor="WindowFrame"></asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="AreaFinalType" BorderWidth="0"></asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
        </div>
        <div id="divDispatch">
        <asp:Chart ID="ChartDispatch" runat="server" Height="400px" Width="600px" Palette="None"
            BackColor="#D3DFF0" BorderlineDashStyle="DashDot" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="181, 64, 1">
            <Titles>
                <asp:Title ShadowOffset="1" Name="Dispatch"></asp:Title>
            </Titles>
            <Legends>
                <asp:Legend Alignment="Center" Docking="Top" IsTextAutoFit="true" Name="DispatchStage" LegendStyle="Row"></asp:Legend>
            </Legends>
            <Series>
                <asp:Series Name="Dispatch Stage" BackSecondaryColor="WindowFrame"></asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="AreaDispatch" BorderWidth="0"></asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
        </div>
    </div>    
    <div>
        <div id="divFinalCheck">
        <asp:Chart ID="ChartFinalCheck" runat="server" Height="400px" Width="500px" Palette="Fire"
            BackColor="#F3DFC1" BorderlineDashStyle="DashDot" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="181, 64, 1">
            <Titles>
                <asp:Title ShadowOffset="1" Name="Final Check"></asp:Title>
            </Titles>
            <Legends>
                <asp:Legend Alignment="Center" Docking="Top" IsTextAutoFit="true" Name="FinalCheckStage" LegendStyle="Row"></asp:Legend>
            </Legends>
            <Series>
                <asp:Series Name="Final Check Stage" BackSecondaryColor="WindowFrame"></asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="AreaFinalCheck" BorderWidth="0"></asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
        </div>
    </div>
        
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>