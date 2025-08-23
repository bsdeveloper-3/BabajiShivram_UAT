<%@ Page Title="Customer Billing Stage" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="BillingChartCustomerStage.aspx.cs" Inherits="Reports_BillingChartCustomerStage" EnableEventValidation="false" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
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
        <div id="divCustomer">
        <asp:Chart ID="ChartCustomer" runat="server" Height="400px" Width="500px" Palette="BrightPastel" 
            BackColor="#F3DFC1" BorderlineDashStyle="DashDot" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="181, 64, 1">
            <Titles>
                <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" Text="Top 10 Customer Avg Bill Days" Name="Title1" ForeColor="26, 59, 105"></asp:Title>
            </Titles>
            <%--<Legends>
                <asp:Legend Docking="Top" TitleFont="Microsoft Sans Serif, 8pt, style=Bold" BackColor="Transparent" Font="Trebuchet MS, 8.25pt, style=Bold" IsTextAutoFit="False" Enabled="False" Name="Default"></asp:Legend>
            </Legends>--%>
            <Series>
                <asp:Series Name="Customer"/>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="AreaCustomer" BorderColor="64, 64, 64, 64" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                    <axisy LineColor="64, 64, 64, 64"  LabelAutoFitMaxFontSize="8">
						<LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
						<MajorGrid LineColor="64, 64, 64, 64" />
					</axisy>
					<axisx LineColor="64, 64, 64, 64"  LabelAutoFitMaxFontSize="8">
						<LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" IsEndLabelVisible="False" />
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
                <asp:Title Text="Customer Draft Avg Days" Name="Draft" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" ForeColor="26, 59, 105"></asp:Title>
            </Titles>
            <%--<Legends>
                <asp:Legend Alignment="Center" Docking="Top" IsTextAutoFit="true" Name="DraftStage" LegendStyle="Row"></asp:Legend>
            </Legends>--%>
            <Series>
                <asp:Series Name="Draft Stage" BackSecondaryColor="WindowFrame"></asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="AreaDraft" BorderWidth="0"></asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
        </div>
    </div>

    <div style="float:left;">
        <div id="divKPIScrutiny">
        <asp:Chart ID="ChartScrutiny" runat="server" Height="400px" Width="500px" Palette="Bright" 
            BackColor="#D3DFF0" BorderlineDashStyle="DashDot" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="181, 64, 1">
            <Titles>
                <asp:Title Text="Customer Scrutiny Avg Days" Name="" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" ForeColor="26, 59, 105"></asp:Title>
            </Titles>
            <%--<Legends>
                <asp:Legend Alignment="Center" Docking="Top" IsTextAutoFit="true" Name="ScrutinyStage" LegendStyle="Row"></asp:Legend>
            </Legends>--%>
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
                <asp:Title Text="Customer Draft Check Avg Days" Name="DraftCheck" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" ForeColor="26, 59, 105"></asp:Title>
            </Titles>
            <%--<Legends>
                <asp:Legend Alignment="Center" Docking="Top" IsTextAutoFit="true" Name="DraftCheckStage" LegendStyle="Row"></asp:Legend>
            </Legends>--%>
            <Series>
                <asp:Series Name="Draft Check Stage" BackSecondaryColor="WindowFrame"></asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="AreaDraftCheck" BorderColor="64, 64, 64, 64" BackSecondaryColor="White" BackColor="WhiteSmoke" ShadowColor="Transparent" BackGradientStyle="TopBottom"></asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
        </div>
    
    </div>

    <div style="float:left;">
        <div id="divFinalType">
        <asp:Chart ID="ChartFinalType" runat="server" Height="400px" Width="500px" Palette="EarthTones"
            BackColor="#F3DFC1" BorderlineDashStyle="DashDot" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="181, 64, 1">
            <Titles>
                <asp:Title Text="Customer Final Type Avg Days" Name="FinalType" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" ForeColor="26, 59, 105"></asp:Title>
            </Titles>
            <%--<Legends>
                <asp:Legend Alignment="Center" Docking="Top" IsTextAutoFit="true" Name="FinalTypeStage" LegendStyle="Row"></asp:Legend>
            </Legends>--%>
            <Series>
                <asp:Series Name="FinalType Stage" BackSecondaryColor="WindowFrame"></asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="AreaFinalType" BorderColor="64, 64, 64, 64" BackSecondaryColor="White" BackColor="WhiteSmoke" ShadowColor="Transparent" BackGradientStyle="TopBottom"></asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
        </div>
        
        <div id="divDispatch">
        <asp:Chart ID="ChartDispatch" runat="server" Height="400px" Width="500px" Palette="None"
            BackColor="#D3DFF0" BorderlineDashStyle="DashDot" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="181, 64, 1">
            <Titles>
                <asp:Title Text="Customer Bill Dispatch Avg Days" Name="Dispatch" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" ForeColor="26, 59, 105"></asp:Title>
            </Titles>
            <%--<Legends>
                <asp:Legend Alignment="Center" Docking="Top" IsTextAutoFit="true" Name="DispatchStage" LegendStyle="Row"></asp:Legend>
            </Legends>--%>
            <Series>
                <asp:Series Name="Dispatch Stage" BackSecondaryColor="WindowFrame"></asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="AreaDispatch" BorderWidth="0"></asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
        </div>
            
    </div>    

    <div style="float:left;">
        <div id="divFinalCheck">
        <asp:Chart ID="ChartFinalCheck" runat="server" Height="400px" Width="500px" Palette="SemiTransparent"
            BackColor="#F3DFC1" BorderlineDashStyle="DashDot" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="181, 64, 1">
            <Titles>
                <asp:Title Text="Customer Final Check Avg Days" Name="FinalCheck" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" ForeColor="26, 59, 105"></asp:Title>
            </Titles>
            <%--<Legends>
                <asp:Legend Alignment="Center" Docking="Top" IsTextAutoFit="true" Name="FinalCheckStage" LegendStyle="Row"></asp:Legend>
            </Legends>--%>
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
