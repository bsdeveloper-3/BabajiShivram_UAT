<%@ Page Title="E-Way BoE Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
        CodeFile="EWayBoEReport.aspx.cs" Inherits="Reports_EWayBoEReport" %>
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
    <script type="text/javascript">
    function OnJobSelected(source, eventArgs) {
        var results = eval('(' + eventArgs.get_value() + ')');
        $get('ctl00_ContentPlaceHolder1_hdnJobId').value = results.JobId;
    }
    </script>
    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <asp:HiddenField ID="hdnJobId" runat="server" Value="0" />
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <fieldset><legend>Babaji Job No</legend>
            <div class="m clear"></div>
            <div>
                <asp:TextBox ID="txtJobNumber" Width="160px" runat="server" ToolTip="Enter Job/BOE Number." CssClass="SearchTextbox" 
                    placeholder="Search Job" TabIndex="1" AutoPostBack="true" OnTextChanged="txtJobNumber_TextChanged"></asp:TextBox>
                    <cc1:AutoCompleteExtender ID="JobDetailExtender" runat="server" TargetControlID="txtJobNumber"
                    CompletionListElementID="divwidthJob" ServicePath="~/WebService/JobNumberAutoComplete.asmx"
                    ServiceMethod="GetJobListForDelivery" MinimumPrefixLength="2" BehaviorID="divwidthJob"
                    ContextKey="1" UseContextKey="True" OnClientItemSelected="OnJobSelected" 
                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                </cc1:AutoCompleteExtender>
                <asp:Button ID="btnShowJob" Text="Show Invoice Detail" runat="server" OnClick="btnShowJob_Click" />
            </div> 
            </fieldset>
            <div class="clear"></div>
            <fieldset><legend>E-Way Bill BoE Product Report</legend>
            <div>
                <asp:LinkButton ID="lnkReportXls" runat="server"  OnClick="lnkReportXls_Click">
                    <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>
            </div>
            <div class="clear"></div>
                <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="false" CssClass="table"
                    ShowFooter="false" DataSourceID="DataSourceReport">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="JobRefNo" HeaderText="Job No" SortExpression="JobRefNo" />
                        <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" SortExpression="InvoiceNo" />
                        <asp:BoundField DataField="BOENo" HeaderText="BOE No" SortExpression="BOENo" />
                        <asp:BoundField DataField="BOEDate" HeaderText="BOE Date" SortExpression="BOEDate" DataFormatString="{0:dd/MM/yyyy}"/>
                        <asp:BoundField DataField="CTHNo" HeaderText="HSN Code" SortExpression="CTHNo" />
                        <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                        <asp:BoundField DataField="Quantity" HeaderText="Qty" SortExpression="Quantity" />
                        <asp:BoundField DataField="ItemAssessableValue" HeaderText="Assessable Value" SortExpression="ItemAssessableValue" />
                        <asp:BoundField DataField="BasicDutyRate" HeaderText="BCD %" SortExpression="BasicDutyRate" />
                        <asp:BoundField DataField="BasicDutyAmount" HeaderText="BCD" SortExpression="BasicDutyAmount" />
                        <asp:BoundField DataField="EduCessRate" HeaderText="Cess %" SortExpression="EduCessRate" />
                        <asp:BoundField DataField="EduCessAmount" HeaderText="Cess" SortExpression="EduCessAmount" />
                        <asp:BoundField DataField="IGST Assessable Value" HeaderText="Total IGST Assessable Value" SortExpression="IGST Assessable Value" />
                        <asp:BoundField DataField="GSTDutyRate" HeaderText="IGST Rate" SortExpression="GSTDutyRate" />
                        <asp:BoundField DataField="GSTDutyAmount" HeaderText="IGST Duty Amount" SortExpression="GSTDutyAmount" />
                        <asp:BoundField DataField="Transporter GST No" HeaderText="Transporter GST No" SortExpression="Transporter GST No" />
                        <asp:BoundField DataField="LR NO" HeaderText="LR NO" SortExpression="LR NO" />
                        <asp:BoundField DataField="Vehicle No" HeaderText="Vehicle No" SortExpression="Vehicle No" />
                        
                    </Columns>
                </asp:GridView>
            </fieldset>  
            <div>
                <asp:SqlDataSource ID="DataSourceReport" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                 SelectCommand="rptEWayInvoiceDetail" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:ControlParameter Name="JobID" ControlID="hdnJobId" PropertyName="Value" Type="Int32"/>
                    </SelectParameters>
            </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

