<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="KPIBOE.aspx.cs" 
    Inherits="Reports_KPIBOE" EnableEventValidation="false" Title="BOE KPI" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" ID="content1">

<cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingchecklist" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    
     <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional"
        RenderMode="Inline">
        <ContentTemplate>
            <div>
                <cc1:CalendarExtender ID="CalFromDate" runat="server" Enabled="True" EnableViewState="False"
                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDateFrom" PopupPosition="BottomRight"
                    TargetControlID="txtDateFrom">
                </cc1:CalendarExtender>
                <cc1:CalendarExtender ID="CalToDate" runat="server" Enabled="True" EnableViewState="False"
                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDateTo" PopupPosition="BottomRight"
                    TargetControlID="txtDateTo">
                </cc1:CalendarExtender>
            </div>
            <div align="center" style="vertical-align: top">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <div class="clear"></div>
            <fieldset><legend>BOE KPI</legend>    
            <div>
                <div class="fleft">
                    <asp:Button ID="btnShowReport" Text="Show Report" OnClick="btnShowReport_OnClick" runat="server" ValidationGroup="Required" />
                </div>
                <div class="fleft" style="margin-left:40px;">
                <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click" ValidationGroup="Required">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>
            </div>
            </div>
            <div class="m clear"></div>
            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                <tr>
                    <td> BOE Date From
                        <asp:RequiredFieldValidator ID="RFVFomDate" ValidationGroup="Required" runat="server"
                            Text="*" ControlToValidate="txtDateFrom" ErrorMessage="Please Enter From Date"
                            SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="ComValFromDate" runat="server" ControlToValidate="txtDateFrom" Display="Dynamic" ValidationGroup="Required"
                            ErrorMessage="Invalid From Date." Type="Date" Operator="DataTypeCheck"></asp:CompareValidator>     
                    </td>
                    <td>
                        <asp:TextBox ID="txtDateFrom" runat="server" Width="100px" Text="01/07/2017"></asp:TextBox>
                        <asp:Image ID="imgDateFrom" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                    </td>
                    <td> BOE Date To
                        <asp:RequiredFieldValidator ID="RFVDateTo" ValidationGroup="Required" runat="server"
                            Text="*" ControlToValidate="txtDateTo" ErrorMessage="Please Enter TO Date"
                            SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="ComValDateTo" runat="server" ControlToValidate="txtDateTo" Display="Dynamic" ValidationGroup="Required"
                            ErrorMessage="Invalid To Date." Type="Date" Operator="DataTypeCheck"></asp:CompareValidator>     
                    </td>
                    <td>
                        <asp:TextBox ID="txtDateTo" runat="server" Width="100px" Text="01/08/2017"></asp:TextBox>
                        <asp:Image ID="imgDateTo" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                    </td>
                </tr>    
                <tr>
                    <td>Port</td>
                    <td>
                        <asp:DropDownList ID="ddPort" runat="server">
                            <asp:ListItem Text="All Port" Value="0"></asp:ListItem>
                            <asp:ListItem Text="NHAVA SHEVA" Value="5"></asp:ListItem>
                            <asp:ListItem Text="Mumbai Air Cargo" Value="4"></asp:ListItem>
                            <asp:ListItem Text="Ankaleshwar" Value="17"></asp:ListItem>
                            <asp:ListItem Text="Jaipur ICD" Value="10"></asp:ListItem>
                            <asp:ListItem Text="Ahmedabad ICD" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Ahmedabad Air ACC" Value="18"></asp:ListItem>
                            <asp:ListItem Text="Bangalore ICD" Value="41"></asp:ListItem>
                            <asp:ListItem Text="Banglore Air Cargo" Value="3"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
            </table>
            </fieldset>
            <div class="clear"></div>
            <cc1:TabContainer runat="server" ID="Tabs" ActiveTabIndex="0" CssClass="Tab" Width="100%" AutoPostBack="false">
                <cc1:TabPanel runat="server" ID="TabPanelJobDetail" HeaderText="Detail Report">
                <ContentTemplate>
                <div>
                <asp:GridView ID="gvBOEKPIReport" runat="server" AutoGenerateColumns="True" CssClass="table">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                </div>
                </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="TabPanelChart" runat="server" HeaderText="Chart Report">
                    <ContentTemplate>
                    <div id="divPIE">
                        <asp:Chart ID="BOEKPIChartPIE" Height="300px" Width="800px" runat="server" BorderlineWidth="2" BorderlineColor="ActiveBorder" BorderlineDashStyle="DashDot" 
                            BackSecondaryColor="White" BackGradientStyle="TopBottom" BorderWidth="2px" backcolor="211, 223, 240" BorderColor="#1A3B69">
	                        <Series>
		                        <asp:Series Name="BOEKPI" YValueType="Int32" ChartArea="BOEKPIArea" ChartType="Pie" Color="211, 223, 240"></asp:Series>
	                        </Series>
                            <Titles>
                                <asp:Title Text="Total No of Product" font="Arial, 12pt, style=Bold, Italic" Name="Sector"></asp:Title>
                            </Titles>
	                        <ChartAreas>
		                        <asp:ChartArea Name="BOEKPIArea">
                                </asp:ChartArea>
                            </ChartAreas>
                            <Legends>
                                <asp:Legend Name="BOEKPILegends" BorderColor="Green" BorderDashStyle="Solid"></asp:Legend>
                            </Legends>
                        </asp:Chart>
                    </div>
                    <%--<div id="divBAR" style="overflow: scroll;">
		                <asp:Chart ID="cTestChart" runat="server" Height="400px" Width="800px">
			                <Series>
				                <asp:Series Name="Testing">
				                </asp:Series>
			                </Series>
			                <ChartAreas>
				                <asp:ChartArea Name="ChartArea1">
					                <Area3DStyle />
				                </asp:ChartArea>
			                </ChartAreas>
		                </asp:Chart>

                    </div>--%>
                    <div style="overflow:scroll;">
                    <cc1:BarChart ID="BarChart1" runat="server" ChartHeight="300" ChartWidth = "450"
                        ChartType="Column" ChartTitleColor="#0E426C"  
                        CategoryAxisLineColor="#D08AD9" ValueAxisLineColor="#D08AD9" BaseLineColor="#A156AB">
                    </cc1:BarChart>
                    </div>
                    <div style="overflow:scroll;">
                    <cc1:BarChart ID="BarChart2" runat="server" ChartHeight="300" ChartWidth = "450"
                        ChartType="Column" ChartTitleColor="#1A3B69"  
                        CategoryAxisLineColor="#1A3B69" ValueAxisLineColor="#1A3B69" BaseLineColor="#A156AB">
                    </cc1:BarChart>
                    </div>
                    </ContentTemplate>
                </cc1:TabPanel>
            </cc1:TabContainer>
       
        <asp:SqlDataSource ID="DataSourceBOEKPI" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommandType="StoredProcedure" SelectCommand="rptkpiBOE" DataSourceMode="DataSet">
            <SelectParameters>
                <asp:ControlParameter Name="DateFrom" ControlID="txtDateFrom" PropertyName="Text" Type="datetime" />
                <asp:ControlParameter Name="DateTo" ControlID="txtDateTo" PropertyName="Text" Type="datetime"/>
                <asp:ControlParameter Name="PortId" ControlID="ddPort" PropertyName="SelectedValue"/>
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
            </SelectParameters>
            </asp:SqlDataSource> 
        </ContentTemplate> 
    </asp:UpdatePanel> 
</asp:Content>