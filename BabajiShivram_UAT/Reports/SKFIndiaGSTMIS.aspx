<%@ Page Title="GST MIS" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="SKFIndiaGSTMIS.aspx.cs" Inherits="Reports_SKFIndiaGSTMIS" Culture="en-GB" %>

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
            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white" style="Width:80%; align-content:center;">
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
                        <asp:TextBox ID="txtToDate" runat="server" Width="100px" MaxLength="10" TabIndex="2" placeholder="dd/mm/yyyy" ></asp:TextBox>
                        <asp:Image ID="imgToDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                            runat="server"/>
                        <asp:CompareValidator ID="ComValToDate" runat="server" ControlToValidate="txtToDate" Display="Dynamic" ErrorMessage="Invalid To Date." Text="Invalid To Date." 
                            Type="Date" CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="Required" SetFocusOnError="true">
                        </asp:CompareValidator>    
                    </td>
                    <td>
                        <asp:DropDownList ID="ddClearedStatus" runat="server">
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
            <fieldset><legend>SKF India GST MIS</legend>
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
                        <%--<asp:BoundField HeaderText="JobDate" DataField="JobDate" />--%>

                        <asp:BoundField HeaderText="BS Job No" DataField="BS Job No" />

                        <asp:BoundField HeaderText="Control No" DataField="Control No" />
                        <asp:BoundField HeaderText="Payment LoT No" DataField="Payment LoT No" />
                        <asp:BoundField HeaderText="JV / Voucher No" DataField="JV / Voucher No" />
                        <asp:BoundField HeaderText="Supplier Name" DataField="Supplier Name" />
                        <asp:BoundField HeaderText="GSTIN of Service Provider/Supplier" DataField="GSTIN of Service Provider/Supplier" />
                        <asp:BoundField HeaderText="Nature of Service" DataField="Nature of Service( ex. Forwarding)" />
                        <asp:BoundField HeaderText="BSR Bill Invoice No" DataField="BSR Bill Invoice No" />
                        <asp:BoundField HeaderText="Supplier Invoice Date" DataField="Supplier Invoice Date" />
                        <asp:BoundField HeaderText="Total Supplier Invoice Amount/Value" DataField="Total Supplier Invoice Amount/Value" />
                        <asp:BoundField HeaderText="SAC Code" DataField="SAC Code" />
                        <asp:BoundField HeaderText="GST Rate on BSR Invoice" DataField="GST Rate on BSR Invoice" />
                        <asp:BoundField HeaderText="IGST Amount" DataField="IGST Amount" />
                        <asp:BoundField HeaderText="IGST Amount of Supplier Invoice" DataField="IGST Amount of Supplier Invoice" />
                        <asp:BoundField HeaderText="CGST Amount of Supplier Invoice" DataField="CGST Amount of Supplier Invoice" />
                        <asp:BoundField HeaderText="IGST Amount of Supplier Invoice" DataField="IGST Amount of Supplier Invoice" />
                        <asp:BoundField HeaderText="Cess Amount of Supplier Invoice" DataField="Cess Amount of Supplier Invoice" />
                        <asp:BoundField HeaderText="GSTIN MENTION IN BOE" DataField="GSTIN MENTION IN BOE" />
                        <asp:BoundField HeaderText="Remark" DataField="Remark" />

                        
                        <%--<asp:BoundField HeaderText="Date of Delivery to Warehouse" DataField="Date of Delivery to Warehouse" />--%>
                    </Columns>
                </asp:GridView>
            </fieldset>  
            <div>
                <asp:SqlDataSource ID="DataSourceReport" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                 SelectCommand="rptSKFGSTMIS" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="UserId" SessionField="UserId" />
                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                </SelectParameters>
            </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
