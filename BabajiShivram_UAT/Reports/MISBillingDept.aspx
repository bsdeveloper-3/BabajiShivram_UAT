<%@ Page Title="Billing Performance" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="MISBillingDept.aspx.cs" Inherits="Reports_MISBillingDept" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
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
                <div class="clear" style="text-align: center;">
                    <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                </div>
                <div class="clear">
                </div>
                <div>
                    <div>
                        <asp:DropDownList ID="ddBabajiBranch" runat="server" AutoPostBack="true"></asp:DropDownList>
                        &nbsp;&nbsp; 
                        <asp:TextBox ID="txtKPIDays" runat="server" Text="1" AutoPostBack="true" MaxLength="6" Width="50px"></asp:TextBox>
                        <asp:Button ID="btnView" runat="Server" Text="View Report" />
                        <div class="tooltip">Report Criteria ?&nbsp;
                            <span class="tooltiptext">A. % of File Compled on time - One Day.
                                B. % = Total File Sent to Total File Completed in Month
                            </span>
                        </div>
                    </div>
                </div>
                <br />
                <div class="m"></div>
                <div style="float: left;">
                    <div id="divScrutiny">
                        <asp:Chart ID="ChartScrutiny" runat="server" Height="350px" Width="500px" BackColor="WhiteSmoke" BorderWidth="2" BackGradientStyle="TopBottom" BackSecondaryColor="White" Palette="BrightPastel" BorderlineDashStyle="Solid" BorderColor="26, 59, 105">
                            <Titles>
                                <asp:Title  Text="Scrutiny % (Same Day)" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" Name="Title2" ForeColor="26, 59, 105"></asp:Title>
                            </Titles>
                            <Legends>
                                <asp:Legend Alignment="Far" Docking="Top" Enabled="True" IsTextAutoFit="False" Name="Default" BackColor="Transparent" Font="Trebuchet MS, 9.25pt, style=Bold"></asp:Legend>
                            </Legends>
                            <BorderSkin SkinStyle="Emboss"></BorderSkin>
                            <Series>
                                <%--<asp:Series XValueType="Auto" Name="Series1" LegendText="2016" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>--%>
                                <asp:Series XValueType="Auto" Name="Series2" LegendText="2017" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                                <asp:Series XValueType="Auto" Name="Series3" LegendText="2018" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="AreaScrutiny" BorderColor="64, 64, 64, 64" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                                    <%--<Position Y="2" Height="94" Width="94" X="2"></Position>--%>
                                    <AxisY LineColor="64, 64, 64, 64">
                                        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                        <MajorGrid LineColor="64, 64, 64, 64" />
                                    </AxisY>
                                    <AxisX LineColor="64, 64, 64, 64">
                                        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                        <MajorGrid LineColor="64, 64, 64, 64" />
                                    </AxisX>
                                </asp:ChartArea>
                            </ChartAreas>
                        </asp:Chart>

                    </div>

                    <div id="divDraftCheck">
                        <asp:Chart ID="ChartDraftCheck" runat="server" Height="350px" Width="500px" BackColor="WhiteSmoke" BorderWidth="2" BackGradientStyle="TopBottom" BackSecondaryColor="White" Palette="BrightPastel" BorderlineDashStyle="Solid" BorderColor="26, 59, 105">
                            <Titles>
                                <asp:Title  Text="Draft Check" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" Name="Title2" ForeColor="26, 59, 105"></asp:Title>
                            </Titles>
                            <Legends>
                                <asp:Legend Alignment="Far" Docking="Top" Enabled="True" IsTextAutoFit="False" Name="Default" BackColor="Transparent" Font="Trebuchet MS, 9.25pt, style=Bold"></asp:Legend>
                            </Legends>
                            <BorderSkin SkinStyle="Emboss"></BorderSkin>
                            <Series>
                                <%--<asp:Series XValueType="Auto" Name="Series1" LegendText="2016" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>--%>
                                <asp:Series XValueType="Auto" Name="Series2" LegendText="2017" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                                <asp:Series XValueType="Auto" Name="Series3" LegendText="2018" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="AreaDraftCheck" BorderColor="64, 64, 64, 64" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                                    <%--<Position Y="2" Height="94" Width="94" X="2"></Position>--%>
                                    <AxisY LineColor="64, 64, 64, 64">
                                        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                        <MajorGrid LineColor="64, 64, 64, 64" />
                                    </AxisY>
                                    <AxisX LineColor="64, 64, 64, 64">
                                        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                        <MajorGrid LineColor="64, 64, 64, 64" />
                                    </AxisX>
                                </asp:ChartArea>
                            </ChartAreas>
                        </asp:Chart>
                            

                    </div>
                </div>
                <div style="float: left;">
                    <div id="divDraft">
                        <asp:Chart ID="ChartDraft" runat="server" Height="350px" Width="500px" BackColor="WhiteSmoke" BorderWidth="2" BackGradientStyle="TopBottom" BackSecondaryColor="White" Palette="BrightPastel" BorderlineDashStyle="Solid" BorderColor="26, 59, 105">
                            <Titles>
                                <asp:Title  Text="Draft" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" Name="Title2" ForeColor="26, 59, 105"></asp:Title>
                            </Titles>
                            <Legends>
                                <asp:Legend Alignment="Far" Docking="Top" Enabled="True" IsTextAutoFit="False" Name="Default" BackColor="Transparent" Font="Trebuchet MS, 9.25pt, style=Bold"></asp:Legend>
                            </Legends>
                            <BorderSkin SkinStyle="Emboss"></BorderSkin>
                            <Series>
                                <%--<asp:Series XValueType="Auto" Name="Series1" LegendText="2016" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>--%>
                                <asp:Series XValueType="Auto" Name="Series2" LegendText="2017" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                                <asp:Series XValueType="Auto" Name="Series3" LegendText="2018" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="AreaDraft" BorderColor="64, 64, 64, 64" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                                    <%--<Position Y="2" Height="94" Width="94" X="2"></Position>--%>
                                    <AxisY LineColor="64, 64, 64, 64">
                                        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                        <MajorGrid LineColor="64, 64, 64, 64" />
                                    </AxisY>
                                    <AxisX LineColor="64, 64, 64, 64">
                                        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                        <MajorGrid LineColor="64, 64, 64, 64" />
                                    </AxisX>
                                </asp:ChartArea>
                            </ChartAreas>
                        </asp:Chart>

                    </div>
                    <div id="divFinalType">
                        <asp:Chart ID="ChartFinalType" runat="server" Height="350px" Width="500px" BackColor="WhiteSmoke" BorderWidth="2" BackGradientStyle="TopBottom" BackSecondaryColor="White" Palette="BrightPastel" BorderlineDashStyle="Solid" BorderColor="26, 59, 105">
                            <Titles>
                                <asp:Title  Text="Final Type" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" Name="Title2" ForeColor="26, 59, 105"></asp:Title>
                            </Titles>
                            <Legends>
                                <asp:Legend Alignment="Far" Docking="Top" Enabled="True" IsTextAutoFit="False" Name="Default" BackColor="Transparent" Font="Trebuchet MS, 9.25pt, style=Bold"></asp:Legend>
                            </Legends>
                            <BorderSkin SkinStyle="Emboss"></BorderSkin>
                            <Series>
                                <%--<asp:Series XValueType="Auto" Name="Series1" LegendText="2016" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>--%>
                                <asp:Series XValueType="Auto" Name="Series2" LegendText="2017" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                                <asp:Series XValueType="Auto" Name="Series3" LegendText="2018" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="AreaFinalType" BorderColor="64, 64, 64, 64" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                                    <%--<Position Y="2" Height="94" Width="94" X="2"></Position>--%>
                                    <AxisY LineColor="64, 64, 64, 64">
                                        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                        <MajorGrid LineColor="64, 64, 64, 64" />
                                    </AxisY>
                                    <AxisX LineColor="64, 64, 64, 64">
                                        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                        <MajorGrid LineColor="64, 64, 64, 64" />
                                    </AxisX>
                                </asp:ChartArea>
                            </ChartAreas>
                        </asp:Chart>
                    </div>
                </div>

                <div style="float: left;">
                    <div id="divFinalCheck">
                        <asp:Chart ID="ChartFinalCheck" runat="server" Height="350px" Width="500px" BackColor="WhiteSmoke" BorderWidth="2" BackGradientStyle="TopBottom" BackSecondaryColor="White" Palette="BrightPastel" BorderlineDashStyle="Solid" BorderColor="26, 59, 105">
                            <Titles>
                                <asp:Title  Text="Final Check" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" Name="Title2" ForeColor="26, 59, 105"></asp:Title>
                            </Titles>
                            <Legends>
                                <asp:Legend Alignment="Far" Docking="Top" Enabled="True" IsTextAutoFit="False" Name="Default" BackColor="Transparent" Font="Trebuchet MS, 9.25pt, style=Bold"></asp:Legend>
                            </Legends>
                            <BorderSkin SkinStyle="Emboss"></BorderSkin>
                            <Series>
                                <%--<asp:Series XValueType="Auto" Name="Series1" LegendText="2016" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>--%>
                                <asp:Series XValueType="Auto" Name="Series2" LegendText="2017" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                                <asp:Series XValueType="Auto" Name="Series3" LegendText="2018" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="AreaFinalCheck" BorderColor="64, 64, 64, 64" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                                    <%--<Position Y="2" Height="94" Width="94" X="2"></Position>--%>
                                    <AxisY LineColor="64, 64, 64, 64">
                                        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                        <MajorGrid LineColor="64, 64, 64, 64" />
                                    </AxisY>
                                    <AxisX LineColor="64, 64, 64, 64">
                                        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                        <MajorGrid LineColor="64, 64, 64, 64" />
                                    </AxisX>
                                </asp:ChartArea>
                            </ChartAreas>
                        </asp:Chart>
                    </div>

                        
                </div>
                <div style="float: left;">
                    <div id="divDispatch" style="float: left;">
                        <asp:Chart ID="ChartDispatch" runat="server" Height="350px" Width="500px" BackColor="WhiteSmoke" BorderWidth="2" BackGradientStyle="TopBottom" BackSecondaryColor="White" Palette="BrightPastel" BorderlineDashStyle="Solid" BorderColor="26, 59, 105">
                            <Titles>
                                <asp:Title  Text="Bill Dispatch" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" Name="Title2" ForeColor="26, 59, 105"></asp:Title>
                            </Titles>
                            <Legends>
                                <asp:Legend Alignment="Far" Docking="Top" Enabled="True" IsTextAutoFit="False" Name="Default" BackColor="Transparent" Font="Trebuchet MS, 9.25pt, style=Bold"></asp:Legend>
                            </Legends>
                            <BorderSkin SkinStyle="Emboss"></BorderSkin>
                            <Series>
                                <%--<asp:Series XValueType="Auto" Name="Series1" LegendText="2016" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>--%>
                                <asp:Series XValueType="Auto" Name="Series2" LegendText="2017" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                                <asp:Series XValueType="Auto" Name="Series3" LegendText="2018" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="AreaDispatch" BorderColor="64, 64, 64, 64" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                                    <%--<Position Y="2" Height="94" Width="94" X="2"></Position>--%>
                                    <AxisY LineColor="64, 64, 64, 64">
                                        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                        <MajorGrid LineColor="64, 64, 64, 64" />
                                    </AxisY>
                                    <AxisX LineColor="64, 64, 64, 64">
                                        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                        <MajorGrid LineColor="64, 64, 64, 64" />
                                    </AxisX>
                                </asp:ChartArea>
                            </ChartAreas>
                        </asp:Chart>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

