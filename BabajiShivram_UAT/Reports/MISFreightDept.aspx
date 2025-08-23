<%@ Page Title="Freight MIS" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="MISFreightDept.aspx.cs" Inherits="Reports_MISFreightDept" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <asp:ScriptManager runat="server" ID="ScriptManager1" />
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
                            <asp:DropDownList ID="ddBabajiBranch" runat="server" AutoPostBack="true" Visible="false"></asp:DropDownList>
                            &nbsp;&nbsp; 
                            <asp:TextBox ID="txtKPIDays" runat="server" Text="0" AutoPostBack="true" MaxLength="6" Width="50px" Visible="false"></asp:TextBox>
                            <asp:Button ID="btnView" runat="Server" Text="View Report" />
                        </div>
                    </div>
                    <br />
                    <div class="m"></div>
                    <div style="float: left;">
                        <div id="divEnquiry">
                            <asp:Chart ID="ChartEnquiry" runat="server" Height="350px" Width="500px" BackColor="WhiteSmoke" BorderWidth="2" BackGradientStyle="TopBottom" BackSecondaryColor="White" Palette="BrightPastel" BorderlineDashStyle="Solid" BorderColor="26, 59, 105">
                                <Titles>
                                    <asp:Title  Text="Enquiry" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" Name="Title2" ForeColor="26, 59, 105"></asp:Title>
                                </Titles>
                                <Legends>
                                    <asp:Legend Alignment="Far" Docking="Top" Enabled="True" IsTextAutoFit="False" Name="Default" BackColor="Transparent" Font="Trebuchet MS, 9.25pt, style=Bold"></asp:Legend>
                                </Legends>
                                <BorderSkin SkinStyle="Emboss"></BorderSkin>
                                <Series>
                                    <asp:Series XValueType="Auto" Name="Series2" LegendText="2016" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                                    <asp:Series XValueType="Auto" Name="Series3" LegendText="2017" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                                    <asp:Series XValueType="Auto" Name="Series4" LegendText="2018" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                                </Series>
                                <ChartAreas>
                                    <asp:ChartArea Name="AreaEnquiry" BorderColor="64, 64, 64, 64" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
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

                        <div id="divQuote">
                            <asp:Chart ID="ChartQuote" runat="server" Height="350px" Width="500px" BackColor="WhiteSmoke" BorderWidth="2" BackGradientStyle="TopBottom" BackSecondaryColor="White" Palette="BrightPastel" BorderlineDashStyle="Solid" BorderColor="26, 59, 105">
                                <Titles>
                                    <asp:Title  Text="Quote" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" Name="Title2" ForeColor="26, 59, 105"></asp:Title>
                                </Titles>
                                <Legends>
                                    <asp:Legend Alignment="Far" Docking="Top" Enabled="True" IsTextAutoFit="False" Name="Default" BackColor="Transparent" Font="Trebuchet MS, 9.25pt, style=Bold"></asp:Legend>
                                </Legends>
                                <BorderSkin SkinStyle="Emboss"></BorderSkin>
                                <Series>
                                    <asp:Series XValueType="Auto" Name="Series2" LegendText="2016" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                                    <asp:Series XValueType="Auto" Name="Series3" LegendText="2017" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                                    <asp:Series XValueType="Auto" Name="Series4" LegendText="2018" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                                </Series>
                                <ChartAreas>
                                    <asp:ChartArea Name="AreaQuote" BorderColor="64, 64, 64, 64" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
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
                        <div id="divAward">
                            <asp:Chart ID="ChartAward" runat="server" Height="350px" Width="500px" BackColor="WhiteSmoke" BorderWidth="2" BackGradientStyle="TopBottom" BackSecondaryColor="White" Palette="BrightPastel" BorderlineDashStyle="Solid" BorderColor="26, 59, 105">
                                <Titles>
                                    <asp:Title  Text="Award" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" Name="Title2" ForeColor="26, 59, 105"></asp:Title>
                                </Titles>
                                <Legends>
                                    <asp:Legend Alignment="Far" Docking="Top" Enabled="True" IsTextAutoFit="False" Name="Default" BackColor="Transparent" Font="Trebuchet MS, 9.25pt, style=Bold"></asp:Legend>
                                </Legends>
                                <BorderSkin SkinStyle="Emboss"></BorderSkin>
                                <Series>
                                    <asp:Series XValueType="Auto" Name="Series2" LegendText="2016" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                                    <asp:Series XValueType="Auto" Name="Series3" LegendText="2017" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                                    <asp:Series XValueType="Auto" Name="Series4" LegendText="2018" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                                </Series>
                                <ChartAreas>
                                    <asp:ChartArea Name="AreaAward" BorderColor="64, 64, 64, 64" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
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
                        <div id="divLost">
                            <asp:Chart ID="ChartLost" runat="server" Height="350px" Width="500px" BackColor="WhiteSmoke" BorderWidth="2" BackGradientStyle="TopBottom" BackSecondaryColor="White" Palette="BrightPastel" BorderlineDashStyle="Solid" BorderColor="26, 59, 105">
                                <Titles>
                                    <asp:Title  Text="Lost" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" Name="Title2" ForeColor="26, 59, 105"></asp:Title>
                                </Titles>
                                <Legends>
                                    <asp:Legend Alignment="Far" Docking="Top" Enabled="True" IsTextAutoFit="False" Name="Default" BackColor="Transparent" Font="Trebuchet MS, 9.25pt, style=Bold"></asp:Legend>
                                </Legends>
                                <BorderSkin SkinStyle="Emboss"></BorderSkin>
                                <Series>
                                    <asp:Series XValueType="Auto" Name="Series2" LegendText="2016" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                                    <asp:Series XValueType="Auto" Name="Series3" LegendText="2017" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                                    <asp:Series XValueType="Auto" Name="Series4" LegendText="2018" ChartType="Line" BorderColor="180, 26, 59, 105" YValueType="Auto"></asp:Series>
                                </Series>
                                <ChartAreas>
                                    <asp:ChartArea Name="AreaLost" BorderColor="64, 64, 64, 64" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
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
                        

                    </div>
                    <div style="float: left;">
                        
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Content>

