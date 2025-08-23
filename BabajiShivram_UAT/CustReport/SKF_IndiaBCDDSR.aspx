<%@ Page Title="SKF India BCD DSR" Language="C#" MasterPageFile="~/CustomerMaster.master"
    AutoEventWireup="true" CodeFile="SKF_IndiaBCDDSR.aspx.cs" Culture="en-GB"
    Inherits="CustReport_SKF_IndiaBCDDSR" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingchecklist" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white" style="Width:100%; align-content:center;">
                <tr>
                     <td>

                        Job Date From
                        <cc1:CalendarExtender ID="CalExtJobFromDate" runat="server" Enabled="True" EnableViewState="False"
                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgFromDate" PopupPosition="BottomRight"
                            TargetControlID="txtFromDate">
                        </cc1:CalendarExtender>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDate" runat="server" Width="100px" MaxLength="10" TabIndex="1" placeholder="dd/mm/yyyy" ></asp:TextBox>
                        <asp:Image ID="imgFromDate" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif"
                            runat="server"/>
                        <asp:CompareValidator ID="ComValFromDate" runat="server" ControlToValidate="txtFromDate" Display="Dynamic" Text="Invalid Date." Type="Date" 
                            CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="Required" SetFocusOnError="true" ErrorMessage="Invalid From Date">
                        </asp:CompareValidator>
                    </td>
                    <td>
                        Job Date To
                        <cc1:CalendarExtender ID="CalExtJobFromTo" runat="server" Enabled="True" EnableViewState="False"
                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgToDate" PopupPosition="BottomRight"
                            TargetControlID="txtToDate">
                        </cc1:CalendarExtender>
                    </td>
                    <td>
                        <asp:TextBox ID="txtToDate" runat="server" Width="100px"  TabIndex="2" placeholder="dd/mm/yyyy" ></asp:TextBox>
                        <asp:Image ID="imgToDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                            runat="server"/>
                        <asp:CompareValidator ID="ComValToDate" runat="server" ControlToValidate="txtToDate" Display="Dynamic" ErrorMessage="Invalid To Date." Text="Invalid To Date." 
                            Type="Date" CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="Required" SetFocusOnError="true">
                        </asp:CompareValidator>    
                    </td>
                    <td>
                        BS Job No
                    </td>
                    <td>
                        <asp:TextBox ID="txtJobNo" runat="server" Width="100px" MaxLength="10" placeholder="Job No" ></asp:TextBox>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddClearedStatus" runat="server"  >
                            <asp:ListItem Value="" Text="All Job" ></asp:ListItem>
                            <asp:ListItem Value="1" Text="Cleared" ></asp:ListItem>
                            <asp:ListItem Value="0" Text="Un-Cleared" ></asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;&nbsp;
                        <asp:Button ID="btnAddFilter" runat="server" Text="Add Filter"  OnClick="btnShowReport_Click" />
                        &nbsp;&nbsp;
                        <asp:Button ID="btnClearFilter" runat="server" Text="Clear Filter" OnClick="btnClearFilter_Click"  />
                    </td>
                </tr>
            </table>
            
            <div class="clear"></div>
            <div class="clear"></div>
            <fieldset><legend><asp:Label ID="lblLegend" runat="server"></asp:Label></legend>
            <div>
                <asp:LinkButton ID="lnkReportXls" runat="server"  OnClick="lnkReportXls_Click">
                    <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>
            </div>
            <div class="clear"></div>
                <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="False" CssClass="table"
                    ShowFooter="false" DataSourceID="DataSourceReport">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Job Created Date" DataField="Job Created Date" />
                        <asp:BoundField HeaderText="Customer Ref No" DataField="Customer Ref No" />
                        <asp:BoundField HeaderText="Country Of Origin" DataField="Country Of Origin" />
                        <asp:BoundField HeaderText="BS Job No" DataField="BS Job No" />
                        <asp:BoundField HeaderText="ITEM_ASSESSABLE_VALUE" DataField="ITEM_ASSESSABLE_VALUE" />
                        <asp:BoundField HeaderText="ITEM_BCD_RATE" DataField="ITEM_BCD_RATE" />
                        <asp:BoundField HeaderText="ITEM_BCD_AMOUNT" DataField="ITEM_BCD_AMOUNT" />
                        <asp:BoundField HeaderText="ITEM_SOCIAL_WELFARE_RATE" DataField="ITEM_SOCIAL_WELFARE_RATE" />
                        <asp:BoundField HeaderText="ITEM_SOCIAL_WELFARE_AMT" DataField="ITEM_SOCIAL_WELFARE_AMT" />
                        <asp:BoundField HeaderText="IGST_LEVY_RATE" DataField="IGST_LEVY_RATE" />
                        <asp:BoundField HeaderText="IGST_LEVY_TOTAL_AMT" DataField="IGST_LEVY_TOTAL_AMT" />
                        <asp:BoundField HeaderText="Total Duty" DataField="Total Duty" />
                        <asp:BoundField HeaderText="Recovarable (IGST) Amt" DataField="Recovarable (IGST) Amt" />
                        <asp:BoundField HeaderText="Non Recovarable Amt" DataField="Non Recovarable Amt" />
                        <asp:BoundField HeaderText="Recovarable (IGST) %" DataField="Recovarable (IGST) %" />
                        <asp:BoundField HeaderText="Non Recovarable %" DataField="Non Recovarable %" />
                  </Columns>
                </asp:GridView>
            </fieldset>  
            <div>
                <asp:SqlDataSource ID="DataSourceReport" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                 SelectCommand="rptSKFIndiaBCDDSR" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="UserId" SessionField="UserId"  ConvertEmptyStringToNull="true" DefaultValue="0"/>
                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    <asp:SessionParameter Name="CustomerId" SessionField="CustId" />
                </SelectParameters>

            </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
