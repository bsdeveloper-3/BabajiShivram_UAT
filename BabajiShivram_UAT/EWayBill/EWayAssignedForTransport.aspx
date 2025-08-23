<%@ Page Title="EWay Bill For Transport" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="EWayAssignedForTransport.aspx.cs" Inherits="EWayBill_EWayAssignedForTransport" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upDetail" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); opacity: .8;">
                    <img alt="progress" src="../Images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upDetail" runat="server">
        <ContentTemplate>
    <asp:Label ID="lblErrorMsg" runat="server" EnableViewState="false"></asp:Label>
    <fieldset><legend>E-Way Bill Assigned For Transport</legend>
    <table>
        <tbody>
            <tr>
                <td>
                </td>
                <td>
                    <asp:RadioButtonList ID="rbCategory" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbCategory_SelectedIndexChanged">
                        <asp:ListItem Value="GENDT" Text="By Date" Selected="True" tooltip="Get eway bill assigned to you(Requesting Gstin) for a transportation within state - particular date"></asp:ListItem>
                        <asp:ListItem Value="GENTR" Text="By GSTIN" ></asp:ListItem>
                        <asp:ListItem Value="GENST" Text="By STATE" ></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </tbody>
    </table>
    <br />    
    <div>
    <div style="float:left;">
    
    <asp:Panel ID="pnlGenDate" runat="server" Visible="true">
        <b>Bill Date </b> &nbsp;&nbsp;
        <asp:TextBox ID="txtDate" runat="server" Width="80px"></asp:TextBox>
        <asp:Image ID="imgDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />&nbsp;&nbsp;
        <cc1:CalendarExtender ID="CalExtDate" runat="server" Enabled="True" EnableViewState="False"
            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDate" PopupPosition="BottomRight"
            TargetControlID="txtDate">
        </cc1:CalendarExtender>
    </asp:Panel>
    <asp:Panel ID="pnlGenGSTN" runat="server" Visible="false">
        <br />
        <b>GSTIN </b> &nbsp;&nbsp;
        <asp:TextBox ID="txtGSTIN" runat="server" Width="120px"></asp:TextBox>
        <asp:RegularExpressionValidator ID="RegExGSTIN" runat="server" ControlToValidate="txtGSTIN"
            ValidationExpression="^[0-9]{2}[a-zA-Z]{5}[0-9]{4}[a-zA-Z]{1}[1-9A-Za-z]{1}[Z]{1}[0-9a-zA-Z]{1}"
            Text="Please Enter Valid GSTIN" Display="Dynamic"></asp:RegularExpressionValidator>
    </asp:Panel>
    <asp:Panel ID="pnlGenState" runat="server" Visible="false">
        <br />
        <b>STATE </b> &nbsp;&nbsp;
        <asp:DropDownList ID="ddState" runat="server"></asp:DropDownList>
    </asp:Panel> 
    </div>
    <div style="float:left;">
        <asp:DropDownList ID="ddTrasnporter" runat="server">
            <asp:ListItem Value="27AAACN1163G1ZR" Text="NAVBHARAT CLEARING AGENTS PVT.LTD." Selected="True"></asp:ListItem>
            <asp:ListItem Value="27AAFFN5296A1ZA" Text="NAV JEEVAN AGENCY"></asp:ListItem>
            <asp:ListItem Value="27AAACB0466A1ZB" Text="MAHARASHTRA"></asp:ListItem>
            <%--<asp:ListItem Value="05AAACG0904A1ZL" Text="Testing"></asp:ListItem>--%>
        </asp:DropDownList>   
        <asp:Button ID="btnShowBill" runat="server" Text="Show Eway Bill" OnClick="btnShowBill_Click" />
        &nbsp;&nbsp;&nbsp;
        <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
        <asp:Image ID="mgExport" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" ImageAlign="AbsMiddle" />
        </asp:LinkButton>
        <div class="clear"></div>
    </div>
    </div>
    </fieldset>    
    <div>
        <asp:GridView ID="gvEWayBill" runat="server" AutoGenerateColumns="false" CssClass="table"
            OnRowCommand="gvEWayBill_RowCommand" AllowSorting="true" OnSorting="gvEWayBill_Sorting">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Eway Bill">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEwayNo" runat="server" Text='<%#Eval("ewbNo") %>' CommandArgument='<%#Eval("ewbNo") %>' CommandName="print"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ewbNo" HeaderText="Eway Bill" SortExpression="ewbNo" Visible="false" />
                <asp:BoundField DataField="ewbDate" HeaderText="Bill Date" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" SortExpression="ewbDate" />
                <asp:BoundField DataField="genGstin" HeaderText="Generated By" SortExpression="genGstin" />
                <asp:BoundField DataField="docNo" HeaderText="Doc No" SortExpression="docNo" />
                <asp:BoundField DataField="docDate" HeaderText="Doc Date" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" SortExpression="docDate"/>
                <asp:BoundField DataField="delPlace" HeaderText="Delivery Place" SortExpression="delPlace" />
                <asp:BoundField DataField="delPinCode" HeaderText="Pin Code" SortExpression="delPinCode" />
                <asp:BoundField DataField="validUpto" HeaderText="Valid Upto" SortExpression="validUpto" />
                <asp:BoundField DataField="extendedTimes" HeaderText="Extended Times" SortExpression="extendedTimes" />
                <asp:BoundField DataField="status" HeaderText="Status" SortExpression="status" />
                <asp:BoundField DataField="rejectStatus" HeaderText="Reject Status" SortExpression="rejectStatus" />
            </Columns>
        </asp:GridView>
    </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>