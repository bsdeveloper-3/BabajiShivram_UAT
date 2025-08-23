<%@ Page Title="Freight AIR Rate" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="FreightRateAir.aspx.cs" Inherits="Freight_FreightRateAir" EnableEventValidation="false" Culture="en-GB"%>
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
        <fieldset><legend>ADD AIR RATE DETAIL</legend>
            <div style="text-align:center;">
                <asp:Label ID="lblError" runat="server"></asp:Label>
            </div>
        <asp:Button ID="btnSubmitAIR" runat="Server" Text="Save Air Rate" OnClick="btnSubmitAIR_Click"  ValidationGroup="Required" />
        <b>Validity Month</b> &nbsp;
        <asp:TextBox ID="txtStatusDate" AutoPostBack="true" runat="server" Width="80px"></asp:TextBox>
        <AjaxToolkit:CalendarExtender ID="calStatusDate" runat="server" DefaultView="Months" ClientIDMode="Static"
            Format="MMM/yyyy" PopupPosition="BottomRight" TargetControlID="txtStatusDate"
            OnClientShown="onCalendarShown" OnClientHidden="onCalendarHidden">
        </AjaxToolkit:CalendarExtender>
        <AjaxToolkit:CalendarExtender ID="calValidityDate" runat="server" 
            Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtValidityDateAIR">
        </AjaxToolkit:CalendarExtender>
        <br /><br />    
        <table class="table">
        <th>COUNTRY</th><th>POL</th><th>POD</th>
        <th><div style="float:left;">MIN</div><div style="float:left; margin-left:55px;">45kg</div>
            <div style="float:right; margin-right:40px;">100kg</div></th>
        <th><div style="float:left;">300kg</div><div style="float:left; margin-left:40px;">500kg</div>
            <div style="float:right; margin-right:30px;">1000kg</div>
        <th><div style="float:left;">FSC</div><div style="float:left; margin-left:50px;">SSC</div>
            <div style="float:right; margin-right:30px;">OTHER</div>
        </th>
        <tr>
            <td>
                <asp:RequiredFieldValidator ID="RDVCountry" runat="server" ControlToValidate="txtCountryAir" ErrorMessage="*"
                    ValidationGroup="Required" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:TextBox ID="txtCountryAir" runat="server" Width="120px"></asp:TextBox>
                <div id="divwidthCountry"></div>
                <AjaxToolkit:AutoCompleteExtender ID="CountryExtender" runat="server" TargetControlID="txtCountryAir"
                    CompletionListElementID="divwidthCountry" ServicePath="../WebService/CountryAutoComplete.asmx"
                    ServiceMethod="GetCountryCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCountry"
                    ContextKey="6233" UseContextKey="True" 
                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                </AjaxToolkit:AutoCompleteExtender>
            </td>
            <td>
                <asp:TextBox ID="txtPOLAir" runat="server" Width="120px"></asp:TextBox>
                <div id="divwidthLoadingPort"></div>
                <AjaxToolkit:AutoCompleteExtender ID="AutoCompletePortLoading" runat="server" TargetControlID="txtPOLAir"
                    CompletionListElementID="divwidthLoadingPort" ServicePath="../WebService/PortOfLoadingAutoComplete.asmx"
                    ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthLoadingPort"
                    ContextKey="56267" UseContextKey="True" 
                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                </AjaxToolkit:AutoCompleteExtender>
            </td>
            <td>
                <asp:DropDownList ID="ddPODAIR" runat="server" Width="120px">
                    <asp:ListItem Text="Mumbai" Value="Mumbai"></asp:ListItem>
                    <asp:ListItem Text="Delhi" Value="Delhi"></asp:ListItem>
                    <asp:ListItem Text="Chennai" Value="Chennai"></asp:ListItem>
                    <asp:ListItem Text="Kolkata" Value="Kolkata"></asp:ListItem>
                    <asp:ListItem Text="Hyderabad" Value="Hyderabad"></asp:ListItem>
                    <asp:ListItem Text="Ahmedabad" Value="Ahmedabad"></asp:ListItem>
                    <asp:ListItem Text="Bangalore" Value="Bangalore"></asp:ListItem>
                    <asp:ListItem Text="Cochin" Value="Cochin"></asp:ListItem>
                    <asp:ListItem Text="Vizag" Value="Vizag"></asp:ListItem>
                </asp:DropDownList>
            </td>
            
            <td>
                <asp:TextBox ID="txtMin" runat="server" Width="50px"></asp:TextBox>
                <asp:TextBox ID="txt45KG" runat="server" Width="50px"></asp:TextBox>
                <asp:TextBox ID="txt100KG" runat="server" Width="50px"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txt300KG" runat="server" Width="50px"></asp:TextBox>
                <asp:TextBox ID="txt500KG" runat="server" Width="50px"></asp:TextBox>
                <asp:TextBox ID="txt1000KG" runat="server" Width="50px"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtFSC" runat="server" Width="50px"></asp:TextBox>
                <asp:TextBox ID="txtSSC" runat="server" Width="50px"></asp:TextBox>
                <asp:TextBox ID="txtOther" runat="server" Width="50px"></asp:TextBox>    
            </td>
        </tr>
        <th>AIRLINE</th><th>AGENT</th><th>CURRENCY</th><th>VALIDITY</th><th colspan="2">REMARK</th>
        <tr>
            <td>
                <asp:TextBox ID="txtAirline" runat="server" Width="120px"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtAgentAIR" runat="server" Width="120px"></asp:TextBox>
                <div id="divwidthAgent"></div>
                <AjaxToolkit:AutoCompleteExtender ID="CustomerExtender" runat="server" TargetControlID="txtAgentAIR"
                    CompletionListElementID="divwidthAgent" ServicePath="../WebService/CompanyAutoComplete.asmx"
                    ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthAgent"
                    ContextKey="5" UseContextKey="True" 
                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight" FirstRowSelected="true">
                </AjaxToolkit:AutoCompleteExtender>
            </td>
        <td>
            <asp:RequiredFieldValidator ID="RFVValidityCurrency" runat="server" ControlToValidate="txtCurrencyAIR" ErrorMessage="*"
                ValidationGroup="Required" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:TextBox ID="txtCurrencyAIR" runat="server" Width="100px"></asp:TextBox>
        </td>
        
        <td>
            <asp:RequiredFieldValidator ID="RFVValidityDate" runat="server" ControlToValidate="txtValidityDateAIR" ErrorMessage="*"
                ValidationGroup="Required" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:TextBox ID="txtValidityDateAIR" runat="server" Width="100px"></asp:TextBox>
        </td>
        <td colspan="2">
            <asp:TextBox ID="txtRemarkAIR" runat="server" MaxLength="200" Width="90%"></asp:TextBox>
        </td>
    </tr>
    </table>
    </fieldset>
        
        <fieldset><legend>AIR RATE DETAIL</legend>
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
        
        <asp:GridView ID="GridViewAIRRate" runat="server" AutoGenerateColumns="false" CssClass="table" 
            AllowSorting="true" AllowPaging="true" DataKeyNames="lid" 
            DataSourceID="SqlDataSourceAIR" OnRowCommand="GridViewAIRRate_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex +1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="COUNTRY" HeaderText="COUNTRY" SortExpression="COUNTRY" />
                <asp:BoundField DataField="POL" HeaderText="POL" SortExpression="POL" />
                <asp:BoundField DataField="POD" HeaderText="POD" SortExpression="POD" />
                <asp:BoundField DataField="MINCharge" HeaderText="MIN" SortExpression="MINCharge" />
                <asp:BoundField DataField="45kg" HeaderText="45kg" SortExpression="45kg" />
                <asp:BoundField DataField="100kg" HeaderText="100kg" SortExpression="100kg" />
                <asp:BoundField DataField="300kg" HeaderText="300kg" SortExpression="300kg" />
                <asp:BoundField DataField="500kg" HeaderText="500kg" SortExpression="500kg" />
                <asp:BoundField DataField="1000kg" HeaderText="1000kg" SortExpression="1000kg" />
                <asp:BoundField DataField="FSCCharge" HeaderText="FSC" SortExpression="FSCCharge" />
                <asp:BoundField DataField="SSCCharge" HeaderText="SSC" SortExpression="SSCCharge" />
                <asp:BoundField DataField="OtherCharge" HeaderText="Other" SortExpression="OtherCharge" />
                <asp:BoundField DataField="Currency" HeaderText="CURRENCY" SortExpression="CURRENCY" />
                <asp:BoundField DataField="AIRLINE" HeaderText="AIRLINE" SortExpression="AIRLINE" />
                <asp:BoundField DataField="AGENT" HeaderText="AGENT" SortExpression="AGENT" />
                <asp:BoundField DataField="RateValidityDate" HeaderText="VALIDITY DATE" DataFormatString="{0:dd/MM/yyyy}" SortExpression="RateValidityDate" />
                <asp:BoundField DataField="Remark" HeaderText="REMARK" SortExpression="REMARK" />
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
            <asp:SqlDataSource ID="SqlDataSourceAIR" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="FR_GetFreightRateByMonth" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter Name="TransMode" DefaultValue="1" />
                    <asp:ControlParameter Name="ValidityMonth" ControlID="txtStatusDate" PropertyName="Text"/>
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </fieldset>
    
    </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Content>