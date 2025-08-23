<%@ Page Title="Vehicle Daily Status" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="VehicleDailyStatus.aspx.cs" 
    Inherits="Transport_VehicleDailyStatus" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="content2" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upExpense" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <fieldset><legend>Vehicle Daily Status</legend>
        <asp:UpdatePanel ID="upExpense" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <div class="clear" style="text-align:center;">
                    <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                </div>
                <div class="clear">
                </div>
                <div>
                    <asp:Button ID="btnSubmit" runat="Server" Text="Save Vehicle Status" OnClick="btnSubmit_Click" />
                    <asp:Button ID="btnCancel" runat="Server" Text="Cancel" OnClick="btnCancel_Click"/>
                    <b>Closing Date</b> &nbsp;<asp:TextBox ID="txtStatusDate" AutoPostBack="true" runat="server" MaxLength="100" Width="100px"
                       OnTextChanged="txtStatusDate_TextChanged" ></asp:TextBox>
                    <AjaxToolkit:CalendarExtender ID="calStatusDate" runat="server" EnableViewState="False"
                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtStatusDate">
                    </AjaxToolkit:CalendarExtender>
                    <asp:LinkButton ID="lnkExportMonth" runat="server" Text="Month Driver Attendance" OnClick="lnkExportMonth_Click"> </asp:LinkButton>    
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="lnkExportStatus" runat="server" Text="Month Vehicle Status" OnClick="lnkExportStatus_Click"> </asp:LinkButton>    
                </div>
                <div>
                <asp:GridView ID="GridViewVehicle" runat="server" AutoGenerateColumns="false" CssClass="table" DataKeyNames="VehicleId" 
                    AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" PageSize="200" AllowPaging="False" AllowSorting="False" 
                    PagerSettings-Position="TopAndBottom" AutoGenerateEditButton="false" DataSourceID="SqlDataSourceExp">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" ReadOnly="true"/>
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddStatus" runat="server" SelectedValue='<%#BIND("StatusID") %>'>
                                    <asp:ListItem Text="-Status-" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="VARDI" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="BREAKDOWN / UNDER MAINTENANCE /NO DRIVER" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="DETAINED" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="ABSENT" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="IDLE" Value="5"></asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Remark">
                            <ItemTemplate>
                                <asp:TextBox ID="txtRemark" Text='<%#BIND("Remark") %>' TextMode="MultiLine" runat="server" MaxLength="400"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Driver Name">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddDriver" runat="server" SelectedValue='<%#Bind("DriverID") %>'
                                 DataSourceID="SqlDataSourceDriver" AppendDataBoundItems="true" DataTextField="UserName" DataValueField="UserId">
                                    <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Attendance">
                            <ItemTemplate>
                                <asp:RadioButtonList ID="rdlPresent" runat="server" RepeatDirection="Horizontal" SelectedValue='<%#Bind("DriverPresent")%>'>
                                    <asp:ListItem Text="A" Value="False" title="Absent"></asp:ListItem>
                                    <asp:ListItem Text="P" Value="True" title="Present"></asp:ListItem>
                                </asp:RadioButtonList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mobile No">
                            <ItemTemplate>
                                <asp:Label ID="lblDriverPhone" Text='<%#Eval("DriverMobile") %>' runat="server" Width="100px"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                </div>
                <div>
                    <asp:SqlDataSource ID="SqlDataSourceExp" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="TR_GetVehicleDailyStatus" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter Name="StatusDate" ControlID="txtStatusDate" PropertyName="Text" Type="datetime" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="SqlDataSourceDriver" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="GetUserByDivisionId" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:Parameter Name="DivisionID" DefaultValue="30" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>

                <!-- Month Attendance -->
                <div>
                <asp:GridView ID="gvExportMonth" runat="server" AutoGenerateColumns="false" CssClass="table" 
                    AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" Visible="false"
                    DataSourceID="SqlDataSourceMonth">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="DriverName" HeaderText="DriveName"/>
                        <asp:BoundField DataField="PresentDays" HeaderText="PresentDays"/>
                    </Columns>

                </asp:GridView>
                </div>
                <div>
                    <asp:SqlDataSource ID="SqlDataSourceMonth" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="TR_GetDriverMonthAttend" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter Name="ReportDate" ControlID="txtStatusDate" PropertyName="Text" Type="datetime" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
                <!-- Vehicle Status Report -->
                <div>
                <asp:GridView ID="gvStatusReport" runat="server" AutoGenerateColumns="true" CssClass="table" 
                     DataSourceID="SqlDataSourceStatus" Visible="false">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                </div>
                <div>
                    <asp:SqlDataSource ID="SqlDataSourceStatus" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="TR_rptVehicleMonthStatus" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter Name="ReportDate" ControlID="txtStatusDate" PropertyName="Text" Type="datetime" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
</asp:Content>