<%@ page title="" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Thermax_Pre-Alert Report.aspx.cs" inherits="Reports_Thermax_Pre_Alert_Report" %>

<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:scriptmanager runat="server" id="ScriptManager1" />
    <div>
        <asp:updateprogress id="updProgress" associatedupdatepanelid="updTheramxPrealertReport" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:updateprogress>
    </div>

    <asp:updatepanel id="updTheramxPrealertReport" runat="server" updatemode="Conditional" rendermode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            </div>

            <table border="0" cellpadding="0" cellspacing="0" width="80%" bgcolor="white" style="Width:100%; align-content:center;">
                <tr>
                    <td>
                        Job Date From
                        <cc1:CalendarExtender ID="CalExtJobFromDate" runat="server" Enabled="True" EnableViewState="False"
                            FirstDayOfWeek="Sunday" Format="dd-MMM-yyyy" PopupButtonID="imgFromDate" PopupPosition="BottomRight"
                            TargetControlID="txtFromDate">
                        </cc1:CalendarExtender>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDate" runat="server" Width="100px" MaxLength="10" TabIndex="1" placeholder="dd/mm/yyyy"  ></asp:TextBox>
                        <asp:Image ID="imgFromDate" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif"
                            runat="server"/>
                        <%--<asp:CompareValidator ID="ComValFromDate" runat="server" ControlToValidate="txtFromDate" Display="Dynamic" Text="Invalid Date." Type="Date" 
                            CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="Required" SetFocusOnError="true" ErrorMessage="Invalid From Date">
                        </asp:CompareValidator>--%>
                    </td>
                    <td>
                        Job Date To
                        <cc1:CalendarExtender ID="CalExtJobFromTo" runat="server" Enabled="True" EnableViewState="False"
                            FirstDayOfWeek="Sunday" Format="dd-MMM-yyyy" PopupButtonID="imgToDate" PopupPosition="BottomRight"
                            TargetControlID="txtToDate">
                        </cc1:CalendarExtender>
                    </td>
                    <td>
                        <asp:TextBox ID="txtToDate" runat="server" Width="100px" MaxLength="10" TabIndex="2" placeholder="dd/mm/yyyy"  ></asp:TextBox>
                        <asp:Image ID="imgToDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                            runat="server"/>
                        <%--<asp:CompareValidator ID="ComValToDate" runat="server" ControlToValidate="txtToDate" Display="Dynamic" ErrorMessage="Invalid To Date." Text="Invalid To Date." 
                            Type="Date" CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="Required" SetFocusOnError="true">
                        </asp:CompareValidator>    --%>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddClearedStatus" runat="server">
                            <%--<asp:ListItem Value="" Text="All Job"></asp:ListItem>--%>
                            <asp:ListItem Value="1" Text="Cleared"></asp:ListItem>
                            <asp:ListItem Value="0" Text="Un-Cleared" Selected="True" ></asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;&nbsp;
                        <asp:Button ID="btnAddFilter" runat="server" Text="Add Filter" OnClick="btnAddFilter_Click" />
                        &nbsp;&nbsp;
                        <asp:Button ID="btnClearFilter" runat="server" Text="Clear Filter" OnClick="btnClearFilter_Click" />
                    </td>
                    
                </tr>
            </table>
            <div class="clear"></div>
            <div class="clear"></div>

            <fieldset><legend>Thermax Pre-Alert Report </legend>
            <div>
                <asp:LinkButton ID="lnkReportXls" runat="server" OnClick="lnkReportXls_Click" >  
                    <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>
            </div>
            <div class="clear"></div>

                <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="False" CssClass="table"
                    ShowFooter="true" Visible="true">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="HBL Number" DataField="HBL Number" />
                        <asp:BoundField HeaderText="HBL Date" DataField="HBL Date" />
                        <asp:BoundField HeaderText="Job Number" DataField="Job Number" />
                        <asp:BoundField HeaderText="Job Date" DataField="Job Date" />
                        <asp:BoundField HeaderText="Reference Number" DataField="Reference Number" />
                        <asp:BoundField HeaderText="CB Code" DataField="CB Code" />
                        <asp:BoundField HeaderText="CB Branch Code" DataField="CB Branch Code" />
                        <asp:BoundField HeaderText="Status-category" DataField="Status-category" />
                        <asp:BoundField HeaderText="Status-Sub Category" DataField="Status-Sub Category" />
                        <asp:BoundField HeaderText="FOC / Non FOC" DataField="FOC / Non FOC" />
                        <asp:BoundField HeaderText="Country Code of Shipment" DataField="Country Code of Shipment" />
                        <asp:BoundField HeaderText="Importer Name" DataField="Importer Name" />
                        <asp:BoundField HeaderText="Importer Branch Code" DataField="Importer Branch Code" />
                        <asp:BoundField HeaderText="BoE Type" DataField="BoE Type" />

                        <asp:BoundField HeaderText="Advance / Post Filing - BE" DataField="Advance / Post Filing - BE" />
                        <asp:BoundField HeaderText="Remarks 1" DataField="Remarks 1" />
                        <asp:BoundField HeaderText="Document Date / Prealert Received date" DataField="Document Date / Prealert Received date" />
                        <asp:BoundField HeaderText="Document Time / Prealert Received Time" DataField="Document Time / Prealert Received Time" />
                        <asp:BoundField HeaderText="Document correctness by Forwarder" DataField="Document correctness by Forwarder" />
                        <asp:BoundField HeaderText="Remark for Document Correctness" DataField="Remark for Document Correctness" />
                        <asp:BoundField HeaderText="Remarks 2" DataField="Remarks 2" />
                        
                    </Columns>
                </asp:GridView>

                <div>
                    <asp:SqlDataSource ID="DataSourceReport" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                     SelectCommand="GetThermaxPrealertReport" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="FINYEAR" SessionField="FinYearId" />
                        <asp:ControlParameter Name="DateFrom" ControlID="txtFromDate" PropertyName="Text" Type="datetime" />
                        <asp:ControlParameter Name="DateTo" ControlID="txtToDate" PropertyName="Text" Type="datetime"/>
                        <asp:ControlParameter ControlID="ddClearedStatus" Name="Status" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
                </div>
        </ContentTemplate>
    </asp:updatepanel>
</asp:Content>

