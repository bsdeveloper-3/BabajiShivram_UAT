<%@ Page Title="Billing User Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="MISBillingUser.aspx.cs" Inherits="Reports_MISBillingUser" EnableEventValidation="false" Culture="en-GB" %>
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
    <style>
    .tooltip {
        position: relative;
        display: inline-block;
        border-bottom: 1px dotted black;
        color:blue;
        cursor:help;
    }

    .tooltip .tooltiptext {
        visibility: hidden;
        width: 210px;
        background-color: black;
        color: #fff;
        text-align: left;
        border-radius: 6px;
        padding: 5px 0;

        /* Position the tooltip */
        position: absolute;
        z-index: 1;
    }

    .tooltip:hover .tooltiptext {
        visibility: visible;
    }
    </style>
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
            <b>KPI Days</b> <asp:TextBox ID="txtKPIDays" runat="server" Text="1" Width="30px" AutoPostBack="true" ></asp:TextBox>
            <asp:Button ID="btnView" runat="Server" Text="View Report" />
            &nbsp;&nbsp;<div class="tooltip">Report Criteria ?&nbsp;
            <span class="tooltiptext">A. KPI Days  - File Sent Date<br />
                &nbsp;To Completed Date<br />
                B. KPI % - % of Total File Sent to File &nbsp;Completed
            </span>
        </div>
        </div>
        
        
    <div class="m"></div>
    <div style="float:left;">
        <div id="divScrutiny">
        <asp:Chart ID="ChartScrutiny" runat="server" Height="400px" Width="500px" Palette="Excel"
            BackColor="#F3DFC1" BorderlineDashStyle="DashDot" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="181, 64, 1">
            <Titles>
                <asp:Title Text="Scrutiny User (KPI 1 Day)" Name="Title1" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" ForeColor="26, 59, 105"></asp:Title>
            </Titles>
            <%--<Legends>
                <asp:Legend Alignment="Center" Docking="Top" IsTextAutoFit="true" Name="LegendScrutiny" LegendStyle="Row"></asp:Legend>
            </Legends>--%>
            <Series>
                <asp:Series Name="Scrutiny User" BackSecondaryColor="WindowFrame"></asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="AreaScrutiny" BorderColor="64, 64, 64, 64" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom"></asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
        
        </div>
        <div id="divChartDraftCheck">
        <asp:Chart ID="ChartDraftCheck" runat="server" Height="400px" Width="500px" Palette="Excel"
            BackColor="#F3DFC1" BorderlineDashStyle="DashDot" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="181, 64, 1">
            <Titles>
                <asp:Title Text="Draft Check User (KPI 1 Day)" Name="Title1" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" ForeColor="26, 59, 105"></asp:Title>
            </Titles>
            <Series>
                <asp:Series Name="Draft Check User (KPI 1 Day)" BackSecondaryColor="WindowFrame"></asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="AreaDraftCheck" BorderWidth="0"></asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
        </div>
        
    </div>

    <div style="float:left;">
        <div id="divChartDraft">
        <asp:Chart ID="ChartDraft" runat="server" Height="400px" Width="500px" Palette="Excel"
            BackColor="#D3DFF0" BorderlineDashStyle="DashDot" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="181, 64, 1">
            <Titles>
                <asp:Title Text="Draft User (KPI 1 Day)" Name="Title1" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" ForeColor="26, 59, 105"></asp:Title>
            </Titles>
            <%--<Legends>
                <asp:Legend Alignment="Center" Docking="Top" IsTextAutoFit="true" Name="LegendDraft" LegendStyle="Row"></asp:Legend>
            </Legends>--%>
            <Series>
                <asp:Series Name="Draft User" BackSecondaryColor="WindowFrame"></asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="AreaDraft" BorderWidth="0"></asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
        </div>

        <div id="divChartFinalType">
        <asp:Chart ID="ChartFinalType" runat="server" Height="400px" Width="500px" Palette="Excel"
            BackColor="#D3DFF0" BorderlineDashStyle="DashDot" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="181, 64, 1">
            <Titles>
                <asp:Title Text="Final Type User (KPI 1 Day)" Name="Title1" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" ForeColor="26, 59, 105"></asp:Title>
            </Titles>
            <%--<Legends>
                <asp:Legend Alignment="Center" Docking="Top" IsTextAutoFit="true" Name="LegendFinalType" LegendStyle="Row"></asp:Legend>
            </Legends>--%>
            <Series>
                <asp:Series Name="Final Type" BackSecondaryColor="WindowFrame"></asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="AreaFinalType" BorderColor="64, 64, 64, 64" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom"></asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
        </div>
    </div>

    <div style="float:left;">
        <div id="divFinalCheck">
        <asp:Chart ID="ChartFinalCheck" runat="server" Height="400px" Width="500px" Palette="Excel"
            BackColor="#F3DFC1" BorderlineDashStyle="DashDot" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="181, 64, 1">
            <Titles>
                <asp:Title Text="Final Check User (KPI 1 Day)" Name="Title1" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" ForeColor="26, 59, 105"></asp:Title>
            </Titles>
            <%--<Legends>
                <asp:Legend Alignment="Center" Docking="Top" IsTextAutoFit="true" Name="LegendFinalType" LegendStyle="Row"></asp:Legend>
            </Legends>--%>
            <Series>
                <asp:Series Name="Final Check" BackSecondaryColor="WindowFrame"></asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="AreaFinalCheck" BorderColor="64, 64, 64, 64" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom"></asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
        </div>
        <div id="divDelay">
        
        </div>
        
    </div>
    
    <div style="float:left;">
         <div id="divDispatch">
        <asp:Chart ID="ChartDispatch" runat="server" Height="400px" Width="500px" Palette="Excel"
            BackColor="#D3DFF0" BorderlineDashStyle="DashDot" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="181, 64, 1">
            <Titles>
                <asp:Title Text="Dispatch User (KPI 1 Day)" Name="Title1" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" ForeColor="26, 59, 105"></asp:Title>
            </Titles>
            <%--<Legends>
                <asp:Legend Alignment="Center" Docking="Top" IsTextAutoFit="true" Name="LegendBillDispatch" LegendStyle="Row"></asp:Legend>
            </Legends>--%>
            <Series>
                <asp:Series Name="Bill Dispatch" BackSecondaryColor="WindowFrame"></asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="AreaBillDispatch" BorderColor="64, 64, 64, 64" BackSecondaryColor="White" BackColor="WhiteSmoke" ShadowColor="Transparent" BackGradientStyle="TopBottom"></asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
        </div>
        <div id="divErrorDrafCheck">
        
        </div>
    </div>    
    
    <div style="float:left;">
         <div id="divErrorFinalType">
        
        </div>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>