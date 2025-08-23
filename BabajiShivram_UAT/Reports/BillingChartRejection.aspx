<%@ Page Title="Billing Rejection" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="BillingChartRejection.aspx.cs" Inherits="Reports_BillingChartRejection" 
     EnableEventValidation="false" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <div class="clear" style="text-align:center;">
            <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
        </div>
        <div class="clear">
        </div>
        <div>
            <b>Report Year</b> &nbsp;
            <asp:DropDownList ID="ddYear" runat="server" Width="100px" AutoPostBack="true">
                <asp:ListItem Text="2016" Value="2016"></asp:ListItem>
                <asp:ListItem Text="2017" Value="2017"></asp:ListItem>
                <asp:ListItem Text="2018" Value="2018"></asp:ListItem>
                <asp:ListItem Text="2019" Value="2019" Selected="True"></asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="btnView" runat="Server" Text="View Report" />
        </div><br />
    <div class="m"></div>
    <div style="float:left;">
        <div id="divRejectMonth">
        <asp:Chart ID="ChartRejectMonth" runat="server" Height="400px" Width="500px" Palette="Excel"
            BackColor="#F3DFC1" BorderlineDashStyle="DashDot" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="181, 64, 1">
            <Titles>
                <asp:Title Text="Billing Reject (Avg Job/Month)" Name="RejectMonth" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" ForeColor="26, 59, 105"></asp:Title>
            </Titles>
            <%--<Legends>
                <asp:Legend Alignment="Center" Docking="Top" IsTextAutoFit="true" Name="LegendStage" LegendStyle="Row"></asp:Legend>
            </Legends>--%>
            <Series>
                <asp:Series Name="Billing Reject Month" BackSecondaryColor="WindowFrame"></asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="AreaReject" BorderWidth="0"></asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
        
        </div>

        <div id="divChartRejectDay">
        <asp:Chart ID="ChartRejectDay" runat="server" Height="400px" Width="500px" Palette="Excel"
            BackColor="#F3DFC1" BorderlineDashStyle="DashDot" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="181, 64, 1">
            <Titles>
                <asp:Title Text="Billing Reject (Avg Job/Day)" Name="RejectMonth" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" ForeColor="26, 59, 105"></asp:Title>
            </Titles>
            <%--<Legends>
                <asp:Legend Alignment="Center" Docking="Top" IsTextAutoFit="true" Name="LegendDay" LegendStyle="Row"></asp:Legend>
            </Legends>--%>
            <Series>
                <asp:Series Name="Billing Reject Day" BackSecondaryColor="WindowFrame"></asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="DayStage" BorderWidth="0"></asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
        </div>
    </div>
    <div style="float:left;">
        <div id="divReasonMonth">
        <asp:Chart ID="ChartReasonMonth" runat="server" Height="400px" Width="500px" Palette="Excel"
            BackColor="#D3DFF0" BorderlineDashStyle="DashDot" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="181, 64, 1">
            <Titles>
                <%--<asp:Title Name="Billing Reject Reason Aging"></asp:Title>--%>
                <asp:Title Text="Reject Reason (Avg Job/Month)" Name="RejectMonth" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" ForeColor="26, 59, 105"></asp:Title>
            </Titles>
            <%--<Legends>
                <asp:Legend Alignment="Center" Docking="Top" IsTextAutoFit="true" Name="LegendMonthReason" LegendStyle="Row"></asp:Legend>
            </Legends>--%>
            <Series>
                <asp:Series />
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="AreaReason" BorderWidth="0"></asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
        </div>

        <div id="divReasonDay">
        <asp:Chart ID="ChartReasonDay" runat="server" Height="400px" Width="500px" Palette="Excel"
            BackColor="#D3DFF0" BorderlineDashStyle="DashDot" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="181, 64, 1">
            <Titles>
                <%--<asp:Title Name="Billing Reject Reason Aging"></asp:Title>--%>
                <asp:Title Text="Reject Reason (Avg Job/Day)" Name="RejectMonth" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" ForeColor="26, 59, 105"></asp:Title>
            </Titles>
            <%--<Legends>
                <asp:Legend Alignment="Center" Docking="Top" IsTextAutoFit="true" Name="LegendDayReason" LegendStyle="Row"></asp:Legend>
            </Legends>--%>
            <Series>
                <asp:Series Name="Reject Reason (Avg Day)" BackSecondaryColor="WindowFrame"></asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="DayReason" BorderWidth="0"></asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
        </div>
    </div>

    <div style="float:left;">
        <div id="divRejectCustomerMonth">
        <asp:Chart ID="ChartCustomerMonth" runat="server" Height="400px" Width="500px" Palette="Excel"
            BackColor="#F3DFC1" BorderlineDashStyle="DashDot" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="181, 64, 1">
            <Titles>
                <asp:Title Text="Top Customer Reject (Avg Job/Month)" Name="RejectMonth" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" ForeColor="26, 59, 105"></asp:Title>
            </Titles>
            <%--<Legends>
                <asp:Legend Alignment="Center" Docking="Top" IsTextAutoFit="true" Name="LegendStage" LegendStyle="Row"></asp:Legend>
            </Legends>--%>
            <Series>
                <asp:Series Name="Top Customer Reject Month" BackSecondaryColor="WindowFrame"></asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="AreaCustomer" BorderWidth="0"></asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
        </div>
    </div>
        
    </div>
</asp:Content>