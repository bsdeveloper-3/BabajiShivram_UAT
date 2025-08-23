<%@ Page Title="Freight Rate Sea" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
        CodeFile="FreightRateSea.aspx.cs" Inherits="Freight_FreightRateSea" EnableEventValidation="false" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <AjaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <script type="text/javascript">
        function onCalendarHidden() {
            var cal = $find("calStatusDate");

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

            var cal = $find("calStatusDate");

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
                    var cal = $find("calStatusDate");
                    cal._visibleDate = target.date;
                    cal.set_selectedDate(target.date);
                    //cal._switchMonth(target.date);
                    cal._blur.post(true);
                    cal.raiseDateSelectionChanged();
                    break;
            }
        }
    </script>
    
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upExpense" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upExpense" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
        <fieldset><legend>ADD SEA RATE DETAIL</legend>
            <div style="text-align:center;">
                <asp:Label ID="lblError" runat="server"></asp:Label>
            </div>
        <asp:Button ID="btnSubmitSEA" runat="Server" Text="Save Sea Rate" OnClick="btnSubmitSEA_Click" ValidationGroup="Required" />
        <b>Validity Month</b> &nbsp;
        <asp:TextBox ID="txtStatusDate" AutoPostBack="true" runat="server" MaxLength="100" Width="80px"></asp:TextBox>
        <AjaxToolkit:CalendarExtender ID="calStatusDate" runat="server" DefaultView="Months" ClientIDMode="Static"
            Format="MMM/yyyy" PopupPosition="BottomRight" TargetControlID="txtStatusDate"
            OnClientShown="onCalendarShown" OnClientHidden="onCalendarHidden">
        </AjaxToolkit:CalendarExtender>
        <AjaxToolkit:CalendarExtender ID="calValidityDate" runat="server" 
            Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtValidityDateSEA">
        </AjaxToolkit:CalendarExtender>
        <br /><br />    
    <table class="table">
    <th>COUNTRY</th><th>POL</th><th>POD</th><th>Shipping Line</th><th>Agent</th><th>Transit Days</th>
        <tr>
            <td>
                <asp:RequiredFieldValidator ID="RDVCountry" runat="server" ControlToValidate="txtCountrySea" ErrorMessage="*"
                    ValidationGroup="Required" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:TextBox ID="txtCountrySea" runat="server"></asp:TextBox>
                <div id="divwidthCountry"></div>
                <AjaxToolkit:AutoCompleteExtender ID="CountryExtender" runat="server" TargetControlID="txtCountrySea"
                    CompletionListElementID="divwidthCountry" ServicePath="../WebService/CountryAutoComplete.asmx"
                    ServiceMethod="GetCountryCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCountry"
                    ContextKey="4244" UseContextKey="True" 
                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                </AjaxToolkit:AutoCompleteExtender>
            </td>
            <td>
                <asp:TextBox ID="txtPOLSEA" runat="server"></asp:TextBox>
                <div id="divwidthLoadingPort"></div>
                <AjaxToolkit:AutoCompleteExtender ID="AutoCompletePortLoading" runat="server" TargetControlID="txtPOLSEA"
                    CompletionListElementID="divwidthLoadingPort" ServicePath="../WebService/PortOfLoadingAutoComplete.asmx"
                    ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthLoadingPort"
                    ContextKey="1267" UseContextKey="True" 
                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                </AjaxToolkit:AutoCompleteExtender>
            </td>
            <td>
                <asp:DropDownList ID="ddPODSEA" runat="server">
                    <asp:ListItem Text="Nhava Sheva" Value="Nhava Sheva"></asp:ListItem>
                    <asp:ListItem Text="Chennai" Value="Chennai"></asp:ListItem>
                    <asp:ListItem Text="Cochin" Value="Cochin"></asp:ListItem>
                    <asp:ListItem Text="Haldia" Value="Haldia"></asp:ListItem>
                    <asp:ListItem Text="Kandla" Value="Kandla"></asp:ListItem>
                    <asp:ListItem Text="Kolkata" Value="Kolkata"></asp:ListItem>
                    <asp:ListItem Text="Mundra" Value="Mundra"></asp:ListItem>
                    <asp:ListItem Text="Vizag" Value="Vizag"></asp:ListItem>
                    <asp:ListItem Text="Katupalli" Value="Katupalli"></asp:ListItem>
                    <asp:ListItem Text="Pipava" Value="Pipava"></asp:ListItem>
                    <asp:ListItem Text="Tuticorin" Value="Tuticorin"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:TextBox ID="txtShippingLineSEA" runat="server"></asp:TextBox>
                <div id="divwidthAgent"></div>
            </td>
            <td>
                <asp:TextBox ID="txtAgentSEA" runat="server"></asp:TextBox>
                <AjaxToolkit:AutoCompleteExtender ID="CustomerExtender" runat="server" TargetControlID="txtAgentSEA"
                    CompletionListElementID="divwidthAgent" ServicePath="../WebService/CompanyAutoComplete.asmx"
                    ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthAgent"
                    ContextKey="5" UseContextKey="True" 
                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight" FirstRowSelected="true">
                </AjaxToolkit:AutoCompleteExtender>
            </td>
            <td>
                <asp:TextBox ID="txtTransitDaysSEA" runat="server" Width="50px"></asp:TextBox>
            </td>
        </tr>
        <th>20 GP</th> <th>40 GP HQ</th><th>Currency</th>
        <th>Validity Date</th><th colspan="2">Remark</th>
        <tr>
        <td>
            <asp:TextBox ID="txt20GPSEA" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txt40GPSEA" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:RequiredFieldValidator ID="RFVCurrency" runat="server" ControlToValidate="txtCurrencySEA" ErrorMessage="*"
                ValidationGroup="Required" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:TextBox ID="txtCurrencySEA" runat="server"></asp:TextBox>
        </td>
        
        <td>
            <asp:RequiredFieldValidator ID="RFVValidityDate" runat="server" ControlToValidate="txtValidityDateSEA" ErrorMessage="*"
                ValidationGroup="Required" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:TextBox ID="txtValidityDateSEA" runat="server"></asp:TextBox>
        </td>
        <td colspan="2">
            <asp:TextBox ID="txtRemarkSEA" runat="server" MaxLength="200" Width="90%"></asp:TextBox>
        </td>
    </tr>
    </table>
    </fieldset>
    <fieldset><legend>SEA RATE DETAIL</legend>
        <div class="clear">
        <asp:Panel ID="pnlFilter" runat="server">
            <div class="fleft">
                <uc1:DataFilter ID="DataFilter1" runat="server" />
            </div>
            <div class="fleft" style="margin-left:40px;">
                <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                    <asp:Image ID="imgExport" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>
            </div>
        </asp:Panel>
        </div>
        <div>
        <asp:GridView ID="GridViewSEARate" runat="server" AutoGenerateColumns="false" CssClass="table" 
            AllowSorting="true" AllowPaging="true" DataKeyNames="lid" 
            DataSourceID="SqlDataSourceSEA" OnRowCommand="GridViewSEARate_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex +1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="COUNTRY" HeaderText="COUNTRY" SortExpression="COUNTRY" />
                <asp:BoundField DataField="POL" HeaderText="POL" SortExpression="POL" />
                <asp:BoundField DataField="POD" HeaderText="POD" SortExpression="POD" />
                <asp:BoundField DataField="LINE" HeaderText="line" SortExpression="LINE" />
                <asp:BoundField DataField="TransitDays" HeaderText="T/T" SortExpression="TransitDays" />
                <asp:BoundField DataField="20GP" HeaderText="20GP" SortExpression="20GP" />
                <asp:BoundField DataField="40GPHQ" HeaderText="40GP HQ" SortExpression="40GPHQ" />
                <asp:BoundField DataField="Currency" HeaderText="CURRENCY" SortExpression="Currency" />
                <asp:BoundField DataField="Agent" HeaderText="AGENT" SortExpression="AGENT" />
                <asp:BoundField DataField="RateValidityDate" HeaderText="VALIDITY DATE" DataFormatString="{0:dd/MM/yyyy}" SortExpression="RateValidityDate" />
                <asp:BoundField DataField="Remark" HeaderText="REMARK" SortExpression="Remark" />
                <asp:TemplateField HeaderText="Delete">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="Remove" CommandArgument='<%#Eval("lid") %>'
                           OnClientClick="return confirm('Are you sure to delete?');"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerTemplate>
            <asp:GridViewPager runat="server" />
        </PagerTemplate>
        </asp:GridView>
        </div>
        <div>
            <asp:SqlDataSource ID="SqlDataSourceSEA" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="FR_GetFreightRateByMonth" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter Name="TransMode" DefaultValue="2" />
                    <asp:ControlParameter Name="ValidityMonth" ControlID="txtStatusDate" PropertyName="Text"/>
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </fieldset>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

