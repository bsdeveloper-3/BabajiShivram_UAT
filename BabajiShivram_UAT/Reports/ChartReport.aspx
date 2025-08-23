<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ChartReport.aspx.cs" 
    Inherits="Reports_ChartReport" Title="Performance Chart" EnableEventValidation="false" Culture="en-GB" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div><b>&nbsp;&nbsp;Mode</b>
        <asp:DropDownList ID="ddMode" runat="server" AutoPostBack="true" Width="100px">
            <asp:ListItem Text="-All-" Value="0"></asp:ListItem>
            <asp:ListItem Text="-SEA-" Value="2"></asp:ListItem>
            <asp:ListItem Text="-AIR-" Value="1"></asp:ListItem>
        </asp:DropDownList>
        &nbsp;&nbsp;<b>Report Duration</b>
        <asp:DropDownList ID="ddDuration" runat="server" AutoPostBack="true">
            <asp:ListItem Text="-Fin Year-" Value="0"></asp:ListItem>
            <asp:ListItem Text="First Quarter" Value="3"></asp:ListItem>
            <asp:ListItem Text="Second Quarter" Value="6"></asp:ListItem>
            <asp:ListItem Text="Third Quarter" Value="9"></asp:ListItem>
            <asp:ListItem Text="Fourth Quarter" Value="12"></asp:ListItem>
        </asp:DropDownList>&nbsp;&nbsp;
        <asp:LinkButton ID="lnkExport" runat="server" OnClick="btnExportPDF_Click">
            <asp:Image ID="imgExort" runat="server" ImageUrl="../images/pdf2.png" ToolTip="Export To PDF" ImageAlign="AbsMiddle"/>
        </asp:LinkButton>
        </div>
    </div>
    <div class="m"></div>
    <div style="float:left;">
        <div id="divSector">
        <asp:Chart ID="SectorChart" Height="300px" Width="590px" runat="server" BorderlineWidth="2" BorderlineColor="ActiveBorder" BorderlineDashStyle="DashDot" 
            BackSecondaryColor="White" BackGradientStyle="TopBottom" BorderWidth="2px" backcolor="211, 223, 240" BorderColor="#1A3B69">
	        <Series>
		        <asp:Series Name="Sector" YValueType="Int32" ChartArea="SectorArea" ChartType="Pie" Color="211, 223, 240"></asp:Series>
	        </Series>
            <Titles>
                <asp:Title Text="Top 10 Sector" font="Arial, 12pt, style=Bold, Italic" Name="Sector"></asp:Title>
            </Titles>
	        <ChartAreas>
		        <asp:ChartArea Name="SectorArea">
                </asp:ChartArea>
            </ChartAreas>
            <Legends>
                <asp:Legend Name="SectorLegends" BorderColor="Green" BorderDashStyle="Solid"></asp:Legend>
            </Legends>
        </asp:Chart>
        </div>
        <div id="divCustomer">
        <asp:Chart ID="CustomerChart" Height="300px" Width="590" runat="server" BorderlineWidth="2" BorderlineColor="ActiveBorder" BorderlineDashStyle="DashDot" 
            BackSecondaryColor="White" BackGradientStyle="TopBottom" BorderWidth="2px" backcolor="211, 223, 240" BorderColor="#1A3B69">
	        <Series>
		        <asp:Series Name="Customer" YValueType="Int32" ChartArea="CustomerArea" ChartType="Pie" Color="211, 223, 240"></asp:Series>
	        </Series>
            <Titles>
                <asp:Title Text="Top 10 Customer" font="Arial, 12pt, style=Bold, Italic" Name="Customer"></asp:Title>
            </Titles>
	        <ChartAreas>
		        <asp:ChartArea Name="CustomerArea">
                </asp:ChartArea>
            </ChartAreas>
            <Legends>
                <asp:Legend Name="CustomerLegends" BorderColor="Red" BorderDashStyle="Solid"></asp:Legend> 
            </Legends>
        </asp:Chart>
        </div>
    </div>

    <div style="float:left;">
        <div id="divBranch">
        <asp:Chart ID="BranchChart" Height="300px" Width="555px" runat="server" BorderlineWidth="2" BorderlineColor="ActiveBorder" BorderlineDashStyle="DashDot" 
            BackSecondaryColor="White" BackGradientStyle="TopBottom" BorderWidth="2px" backcolor="211, 223, 240" BorderColor="#1A3B69">
	        <Series>
		        <asp:Series Name="Branch" YValueType="Int32" ChartArea="BranchArea" ChartType="Pie" Color="211, 223, 240"></asp:Series>
	        </Series>
            <Titles>
                <asp:Title Text="Top 10 Branch" font="Arial, 12pt, style=Bold, Italic" Name="Branch"></asp:Title>
            </Titles>
	        <ChartAreas>
		        <asp:ChartArea Name="BranchArea">
                </asp:ChartArea>
            </ChartAreas>
            <Legends>
                <asp:Legend Name="BranchLegends" BorderColor="Blue" BorderDashStyle="Solid"></asp:Legend> 
            </Legends>
        </asp:Chart>
        </div>

        <div id="divPort">
        <asp:Chart ID="PortChart" Height="300px" Width="555" runat="server" BorderlineWidth="2" BorderlineColor="ActiveBorder" BorderlineDashStyle="DashDot" 
            BackSecondaryColor="White" BackGradientStyle="TopBottom" BorderWidth="2px" backcolor="211, 223, 240" BorderColor="#1A3B69">
	        <Series>
		        <asp:Series Name="Port" YValueType="Int32" ChartArea="PortArea" ChartType="Pie" Color="211, 223, 240"></asp:Series>
	        </Series>
            <Titles>
                <asp:Title Text="Top 10 Port - Job Wise" font="Arial, 12pt, style=Bold, Italic" Name="Port"></asp:Title>
            </Titles>
	        <ChartAreas>
		        <asp:ChartArea Name="PortArea">
                </asp:ChartArea>
            </ChartAreas>
            <Legends>
                <asp:Legend Name="PortLegends" BorderColor="DarkSeaGreen" BorderDashStyle="Solid"></asp:Legend> 
            </Legends>
        </asp:Chart>
        </div>
    </div>

    <div style="float:left;">
        <div id="divJobType">
        <asp:Chart ID="JobTypeChart" Height="300px" Width="590px" runat="server" BorderlineWidth="2" BorderlineColor="ActiveBorder" BorderlineDashStyle="DashDot" 
            BackSecondaryColor="White" BackGradientStyle="TopBottom" BorderWidth="2px" backcolor="211, 223, 240" BorderColor="#1A3B69">
	        <Series>
		        <asp:Series Name="JobType" YValueType="Int32" ChartArea="JobTypeArea" ChartType="Pie" Color="211, 223, 240"></asp:Series>
	        </Series>
            <Titles>
                <asp:Title Text="Top 10 Job Type" font="Arial, 12pt, style=Bold, Italic" Name="Branch"></asp:Title>
            </Titles>
	        <ChartAreas>
		        <asp:ChartArea Name="JobTypeArea">
                </asp:ChartArea>
            </ChartAreas>
            <Legends>
                <asp:Legend Name="JobTypeLegends" BorderColor="HotPink" BorderDashStyle="Solid"></asp:Legend>
            </Legends>
        </asp:Chart>
        </div>
    </div>
    <div style="float:left;">
        <div id="divTEU">
        <asp:Chart ID="TEUChart" Height="300px" Width="555" runat="server" BorderlineWidth="2" BorderlineColor="ActiveBorder" BorderlineDashStyle="DashDot" 
            BackSecondaryColor="White" BackGradientStyle="TopBottom" BorderWidth="2px" backcolor="211, 223, 240" BorderColor="#1A3B69">
	        <Series>
		        <asp:Series Name="TEU" YValueType="Int32" ChartArea="TEUArea" ChartType="Pie" Color="211, 223, 240"></asp:Series>
	        </Series>
            <Titles>
                <asp:Title Text="Top 10 TEU - Port" font="Arial, 12pt, style=Bold, Italic" Name="TEU"></asp:Title>
            </Titles>
	        <ChartAreas>
		        <asp:ChartArea Name="TEUArea">
                </asp:ChartArea>
            </ChartAreas>
            <Legends>
                <asp:Legend Name="TEULegends" BorderColor="DarkSeaGreen" BorderDashStyle="Solid"></asp:Legend> 
            </Legends>
        </asp:Chart>
        </div>
    </div>
</asp:Content>








