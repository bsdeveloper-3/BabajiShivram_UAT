<%@ Page Title="MIS Import User" MasterPageFile="~/MasterPage.master" Language="C#" AutoEventWireup="true"
    CodeFile="MISImportUser.aspx.cs" Inherits="Reports_MISImportUser" EnableEventValidation="false" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager2" />
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
                <div class="clear" style="text-align: center;">
                    <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                </div>
                <div class="clear">
                </div>
                <div>
                    <b>Report Month</b> &nbsp;<asp:TextBox ID="txtReportDate" AutoPostBack="true" runat="server" MaxLength="100" Width="100px"></asp:TextBox>
                    <AjaxToolkit:CalendarExtender ID="calReportDate" runat="server" DefaultView="Months" ClientIDMode="Static"
                        Format="MMM/yyyy" PopupPosition="BottomRight" TargetControlID="txtReportDate"
                        OnClientShown="onCalendarShown" OnClientHidden="onCalendarHidden"></AjaxToolkit:CalendarExtender>
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
                <br />
                <div class="m"></div>
                <div id="divJobUserMonth">
                    <asp:Chart ID="ChartJobUser" runat="server" Height="400px" Width="1000px" Palette="Excel"
                        BackColor="#D3DFF0" BorderlineDashStyle="DashDot" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="181, 64, 1">
                        <Titles>
                            <asp:Title Text="Job Opening User % (Same Day)" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" Name="Title2" ForeColor="26, 59, 105"></asp:Title>
                            <%--<asp:Title Name="Job Opening - User"></asp:Title>--%>
                        </Titles>
                        <%--<Legends>
                                <asp:Legend Alignment="Center" Docking="Top" IsTextAutoFit="true" Name="LegendStage" LegendStyle="Row"></asp:Legend>
                            </Legends>--%>
                        <Series>
                            <asp:Series Name="Job Opening User (Same Day %)" BackSecondaryColor="WindowFrame"></asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="AreaUser" BorderColor="64, 64, 64, 64" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                                <AxisY LineColor="64, 64, 64, 64">
                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                </AxisY>
                                <AxisX LineColor="64, 64, 64, 64">
                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                </AxisX>
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>

                </div>

                <div id="divIGMUserMonth">
                    <asp:Chart ID="ChartIGMUser" runat="server" Height="400px" Width="1000px" Palette="Excel"
                        BackColor="#D3DFF0" BorderlineDashStyle="DashDot" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="181, 64, 1">
                        <Titles>
                            <asp:Title Text="IGM Filing % (Before ETA)" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" Name="Title2" ForeColor="26, 59, 105"></asp:Title>
                        </Titles>
                        <Series>
                            <asp:Series Name="IGM User (KPI %)" BackSecondaryColor="WindowFrame"></asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="AreaIGM" BorderColor="64, 64, 64, 64" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                                <AxisY LineColor="64, 64, 64, 64">
                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                </AxisY>
                                <AxisX LineColor="64, 64, 64, 64">
                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                </AxisX>
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </div>

                <div id="divChecklistUser">
                    <asp:Chart ID="ChartChecklist" runat="server" Height="400px" Width="1000px" Palette="Excel"
                        BackColor="#D3DFF0" BorderlineDashStyle="DashDot" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="181, 64, 1">
                        <Titles>
                            <asp:Title Text="Checklist Prepare % (Within a day)" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" Name="Title2" ForeColor="26, 59, 105"></asp:Title>
                        </Titles>
                        <Series>
                            <asp:Series Name="Checklist User (KPI %)" BackSecondaryColor="WindowFrame"></asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="AreaChecklist" BorderColor="64, 64, 64, 64" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                                <AxisY LineColor="64, 64, 64, 64">
                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                </AxisY>
                                <AxisX LineColor="64, 64, 64, 64">
                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                </AxisX>
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </div>

                <div id="divAudit">
                    <asp:Chart ID="ChartAudit" runat="server" Height="400px" Width="1000px" Palette="Excel"
                        BackColor="#D3DFF0" BorderlineDashStyle="DashDot" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="181, 64, 1">
                        <Titles>
                            <asp:Title Text="Checklist Audit % (2 days)" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" Name="Title2" ForeColor="26, 59, 105"></asp:Title>
                        </Titles>
                        <Series>
                            <asp:Series Name="Audit User (KPI %)" BackSecondaryColor="WindowFrame"></asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="AreaAudit" BorderColor="64, 64, 64, 64" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                                <AxisY LineColor="64, 64, 64, 64">
                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                </AxisY>
                                <AxisX LineColor="64, 64, 64, 64">
                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                </AxisX>
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </div>
                
                
                <div id="divDO">
                    <asp:Chart ID="ChartDO" runat="server" Height="400px" Width="1000px" Palette="Excel"
                        BackColor="#D3DFF0" BorderlineDashStyle="DashDot" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="181, 64, 1">
                        <Titles>
                            <asp:Title Text="DO (ETA To Final DO Date, FCL 2 & LCL 3 Days)" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" Name="Title2" ForeColor="26, 59, 105"></asp:Title>
                        </Titles>
                        <Series>
                            <asp:Series Name="DO User (KPI %)" BackSecondaryColor="WindowFrame"></asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="AreaDO" BorderColor="64, 64, 64, 64" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                                <AxisY LineColor="64, 64, 64, 64">
                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                </AxisY>
                                <AxisX LineColor="64, 64, 64, 64">
                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                </AxisX>
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </div>

                <div id="divNoting">
                    <asp:Chart ID="ChartNoting" runat="server" Height="400px" Width="1000px" Palette="Excel"
                        BackColor="#D3DFF0" BorderlineDashStyle="DashDot" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="181, 64, 1">
                        <Titles>
                            <asp:Title Text="Noting (ETA To BE Date, FCL 1, LCL 3 & Exbond 2 Days)" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" Name="Title2" ForeColor="26, 59, 105"></asp:Title>
                        </Titles>
                        <Series>
                            <asp:Series Name="Noting User (KPI %)" BackSecondaryColor="WindowFrame"></asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="AreaNoting" BorderColor="64, 64, 64, 64" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                                <AxisY LineColor="64, 64, 64, 64">
                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                </AxisY>
                                <AxisX LineColor="64, 64, 64, 64">
                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                </AxisX>
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </div>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
