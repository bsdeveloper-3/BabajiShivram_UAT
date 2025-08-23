<%@ Page Title="Import Performance" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="MISImportDept.aspx.cs" Inherits="Reports_MISImportDept" EnableEventValidation="false" Culture="en-GB" %>
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
                        <asp:Button ID="btnView" runat="Server" Text="View Report" />
                        <div class="tooltip">Import Criteria ?&nbsp;
                            <span class="tooltiptext">A. % Job Opening - Same Day<br />
                              B. % IGM Filing on or Before ETA<br />
                              C. % Checklist Preparation One Day<br />
                              D. % Checklist Audit in 2 days<br />
                              E. % Noting - 1. ETA To BE Date. <br />
                                &nbsp;FCL – 1 day, LCL - 3 days,  <br />
                                &nbsp;Exbond – 2 Days/Job Date to BE Date
                            </span>
                        </div>
                    </div>
                </div>
                <br />
                <div class="m"></div>
                <div style="float: left;">
                    <div id="divJobOpen">
                        <asp:Chart ID="ChartJobOpen" runat="server" Height="350px" Width="500px" BackColor="WhiteSmoke" BorderWidth="2" BackGradientStyle="TopBottom" BackSecondaryColor="White" Palette="BrightPastel" BorderlineDashStyle="Solid" BorderColor="26, 59, 105">
                            <Titles>
                                <asp:Title  Text="Job Opening Dept % (Same Day)" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" Name="Title2" ForeColor="26, 59, 105"></asp:Title>
                            </Titles>
                            <Legends>
                                <asp:Legend Alignment="Far" Docking="Top" Enabled="True" IsTextAutoFit="False" Name="Default" BackColor="Transparent" Font="Trebuchet MS, 9.25pt, style=Bold"></asp:Legend>
                            </Legends>
                            <BorderSkin SkinStyle="Emboss"></BorderSkin>
                            <Series>
                                <asp:Series XValueType="Auto" Name="Series1" LegendText="2016" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                                <asp:Series XValueType="Auto" Name="Series2" LegendText="2017" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                                <asp:Series XValueType="Auto" Name="Series3" LegendText="2018" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
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

                    <div id="divChecklistPrepare">
                        <asp:Chart ID="ChartChecklistPrepare" runat="server" Height="350px" Width="500px" BackColor="WhiteSmoke" BorderWidth="2" BackGradientStyle="TopBottom" BackSecondaryColor="White" Palette="BrightPastel" BorderlineDashStyle="Solid" BorderColor="26, 59, 105">
                            <Titles>
                                <asp:Title  Text="Checklist Dept % (Prepared within a Day)" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" Name="Title2" ForeColor="26, 59, 105"></asp:Title>
                            </Titles>
                            <Legends>
                                <asp:Legend Alignment="Far" Docking="Top" Enabled="True" IsTextAutoFit="False" Name="Default" BackColor="Transparent" Font="Trebuchet MS, 9.25pt, style=Bold"></asp:Legend>
                            </Legends>
                            <BorderSkin SkinStyle="Emboss"></BorderSkin>
                            <Series>
                                <asp:Series XValueType="Auto" Name="Series1" LegendText="2016" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                                <asp:Series XValueType="Auto" Name="Series2" LegendText="2017" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                                <asp:Series XValueType="Auto" Name="Series3" LegendText="2018" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="AreaChecklistPrepare" BorderColor="64, 64, 64, 64" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
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
                    <div id="divJobIGM">
                        <asp:Chart ID="ChartIGM" runat="server" Height="350px" Width="500px" BackColor="WhiteSmoke" BorderWidth="2" BackGradientStyle="TopBottom" BackSecondaryColor="White" Palette="BrightPastel" BorderlineDashStyle="Solid" BorderColor="26, 59, 105">
                            <Titles>
                                <asp:Title  Text="IGM Dept % (Filing Before ETA)" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" Name="Title2" ForeColor="26, 59, 105"></asp:Title>
                            </Titles>
                            <Legends>
                                <asp:Legend Alignment="Far" Docking="Top" Enabled="True" IsTextAutoFit="False" Name="Default" BackColor="Transparent" Font="Trebuchet MS, 9.25pt, style=Bold"></asp:Legend>
                            </Legends>
                            <BorderSkin SkinStyle="Emboss"></BorderSkin>
                            <Series>
                                <asp:Series XValueType="Auto" Name="Series1" LegendText="2016" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                                <asp:Series XValueType="Auto" Name="Series2" LegendText="2017" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                                <asp:Series XValueType="Auto" Name="Series3" LegendText="2018" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="AreaIGM" BorderColor="64, 64, 64, 64" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
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

                    <div id="divAudit">
                        <asp:Chart ID="ChartAudit" runat="server" Height="350px" Width="500px" BackColor="WhiteSmoke" BorderWidth="2" BackGradientStyle="TopBottom" BackSecondaryColor="White" Palette="BrightPastel" BorderlineDashStyle="Solid" BorderColor="26, 59, 105">
                            <Titles>
                                <asp:Title  Text="Checklist Audit % (Approval within 2 Days)" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" Name="Title2" ForeColor="26, 59, 105"></asp:Title>
                            </Titles>
                            <Legends>
                                <asp:Legend Alignment="Far" Docking="Top" Enabled="True" IsTextAutoFit="False" Name="Default" BackColor="Transparent" Font="Trebuchet MS, 9.25pt, style=Bold"></asp:Legend>
                            </Legends>
                            <BorderSkin SkinStyle="Emboss"></BorderSkin>
                            <Series>
                                <asp:Series XValueType="Auto" Name="Series1" LegendText="2016" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                                <asp:Series XValueType="Auto" Name="Series2" LegendText="2017" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                                <asp:Series XValueType="Auto" Name="Series3" LegendText="2018" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="AreaAudit" BorderColor="64, 64, 64, 64" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
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
                    <div id="divDO">
                        <asp:Chart ID="ChartDO" runat="server" Height="350px" Width="500px" BackColor="WhiteSmoke" BorderWidth="2" BackGradientStyle="TopBottom" BackSecondaryColor="White" Palette="BrightPastel" BorderlineDashStyle="Solid" BorderColor="26, 59, 105">
                            <Titles>
                                <asp:Title  Text="DO Collection % (FCL 2 Days, LCL 3 Days from ETA)" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" Name="Title2" ForeColor="26, 59, 105"></asp:Title>
                            </Titles>
                            <Legends>
                                <asp:Legend Alignment="Far" Docking="Top" Enabled="True" IsTextAutoFit="False" Name="Default" BackColor="Transparent" Font="Trebuchet MS, 9.25pt, style=Bold"></asp:Legend>
                            </Legends>
                            <BorderSkin SkinStyle="Emboss"></BorderSkin>
                            <Series>
                                <asp:Series XValueType="Auto" Name="Series1" LegendText="2016" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                                <asp:Series XValueType="Auto" Name="Series2" LegendText="2017" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                                <asp:Series XValueType="Auto" Name="Series3" LegendText="2018" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="AreaDO" BorderColor="64, 64, 64, 64" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
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
                    <div id="divNoting" style="float: left;">
                        <asp:Chart ID="ChartNoting" runat="server" Height="350px" Width="500px" BackColor="WhiteSmoke" BorderWidth="2" BackGradientStyle="TopBottom" BackSecondaryColor="White" Palette="BrightPastel" BorderlineDashStyle="Solid" BorderColor="26, 59, 105">
                            <Titles>
                                <asp:Title  Text="Noting Dept % (FCL 1 Day, LCL 3 Days from ETA, Exbond - 2 Days)" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" Name="Title2" ForeColor="26, 59, 105"></asp:Title>
                            </Titles>
                            <Legends>
                                <asp:Legend Alignment="Far" Docking="Top" Enabled="True" IsTextAutoFit="False" Name="Default" BackColor="Transparent" Font="Trebuchet MS, 9.25pt, style=Bold"></asp:Legend>
                            </Legends>
                            <BorderSkin SkinStyle="Emboss"></BorderSkin>
                            <Series>
                                <asp:Series XValueType="Auto" Name="Series1" LegendText="2016" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                                <asp:Series XValueType="Auto" Name="Series2" LegendText="2017" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                                <asp:Series XValueType="Auto" Name="Series3" LegendText="2018" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="AreaNoting" BorderColor="64, 64, 64, 64" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
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
