<%@ Page Title="PaymentMIS" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="SKFIndiaPaymentMIS.aspx.cs" Inherits="Reports_SKFIndiaPaymentMIS_" Culture="en-GB" %>

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
                        <asp:Button ID="btnAddFilter" runat="server" Text="Add Filter"   />
                        &nbsp;&nbsp;
                        <asp:Button ID="btnClearFilter" runat="server" Text="Clear Filter"   />
                    </td>
                </tr>
            </table>
            
            <div class="clear"></div>
            <div class="clear"></div>
            <fieldset><legend>SKF India Payment MIS</legend>
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

                        <asp:BoundField HeaderText="GSTIN MENTION IN BOE" DataField="GSTIN MENTION IN BOE" />
                        <asp:BoundField HeaderText="BOE NO" DataField="BOE NO" />
                        <asp:BoundField HeaderText="BOE Date" DataField="BOE Date" />
                        <asp:BoundField HeaderText="Assessable Value" DataField="Assessable Value" />
                        <asp:BoundField HeaderText="Total Duty Paid" DataField="Total Duty Paid" />
                        <asp:BoundField HeaderText="Licence No" DataField="Licence No" />
                        <asp:BoundField HeaderText="Supplier" DataField="Supplier" />
                        <asp:BoundField HeaderText="Supplier Invoice No" DataField="Supplier Invoice No" />
                        <asp:BoundField HeaderText="Supplier Invoice Date" DataField="Supplier Invoice Date" />
                        <asp:BoundField HeaderText="Currency" DataField="Currency" />
                        <asp:BoundField HeaderText="Total Commercial Invoice Value" DataField="Total Commercial Invoice Value" />
                        <asp:BoundField HeaderText="Exchange Rate" DataField="Exchange Rate" />
                        <asp:BoundField HeaderText="Duty Interest Amount" DataField="Duty Interest Amount" />
                        <asp:BoundField HeaderText="W/H CODE" DataField="W/H CODE" />
                        <asp:BoundField HeaderText="BSR GST Code" DataField="BSR GST Code" />
                        <asp:BoundField HeaderText="Payment LoT No" DataField="Payment LoT No" />
                        <asp:BoundField HeaderText="SHIPMENT NO" DataField="SHIPMENT NO" />
                        <asp:BoundField HeaderText="BSR Bill Invoice No" DataField="BSR Bill Invoice No" />
                        <asp:BoundField HeaderText="BSR Total Invoice Value Amount INR" DataField="BSR Total Invoice Value Amount INR" />
                        <asp:BoundField HeaderText="DOM. FREIGHT BASIC AMT." DataField="DOM. FREIGHT BASIC AMT." />
                        <asp:BoundField HeaderText="INTL FREIGHT BASIC AMT." DataField="INTL FREIGHT BASIC AMT." />
                        <asp:BoundField HeaderText="INTL FREIGHT IGST" DataField="International Freight IGST Amount" />
                        <asp:BoundField HeaderText="INTL FREIGHT CGST" DataField="INTL FREIGHT CGST" />
                        <asp:BoundField HeaderText="INTL FREIGHT SGST" DataField="INTL FREIGHT SGST" />
                        <asp:BoundField HeaderText="COMMSS CHGS" DataField="COMMSS CHGS" />
                        <asp:BoundField HeaderText="COMMSS IGST" DataField="COMMSS IGST" />
                        <asp:BoundField HeaderText="COMMSS CGST" DataField="COMMSS CGST" />
                        <asp:BoundField HeaderText="COMMSS SGST" DataField="COMMSS SGST" />
                        <asp:BoundField HeaderText="REIMBURSEMENT EXPS" DataField="REIMBURSEMENT EXPS" />
                        <asp:BoundField HeaderText="REIMBURSEMENT IGST" DataField="REIMBURSEMENT IGST" />
                        <asp:BoundField HeaderText="REIMBURSEMENT CGST" DataField="REIMBURSEMENT CGST" />
                        <asp:BoundField HeaderText="REIMBURSEMENT SGST" DataField="REIMBURSEMENT SGST" />
                        <asp:BoundField HeaderText="% of Assessable value" DataField="% of Assessable value" />
                        <asp:BoundField HeaderText="RECOVERABLE DUTY" DataField="RECOVERABLE DUTY" />
                        <asp:BoundField HeaderText="RECOVERABLE  DUTY %" DataField="RECOVERABLE  DUTY %" />
                        <asp:BoundField HeaderText="NON-RECOVERABLE DUTY" DataField="NON-RECOVERABLE DUTY" />
                        <asp:BoundField HeaderText="NON-RECOVERABLE DUTY %" DataField="NON-RECOVERABLE DUTY %" />
                        <asp:BoundField HeaderText="Assessable Value % of Total  Invoice value BSR" DataField="Assessable Value % of Total  Invoice value BSR" />
                        <asp:BoundField HeaderText="Value (INR)" DataField="Value (INR)" />
                        <asp:BoundField HeaderText="Freight Amount" DataField="Freight Amount" />
                        <asp:BoundField HeaderText="Miscellaneous charges Other Than freight charges" DataField="Miscellaneous charges Other Than freight charges" />
                        <asp:BoundField HeaderText="REMARKS/ DEMURRAGE" DataField="REMARKS/ DEMURRAGE" />
                        <%--<asp:BoundField HeaderText="TAX CODE" DataField="TAX CODE" />--%>
                                       
                        
                        <%--<asp:BoundField HeaderText="Date of Delivery to Warehouse" DataField="Date of Delivery to Warehouse" />--%>
                    </Columns>
                </asp:GridView>
            </fieldset>  
            <div>
                <asp:SqlDataSource ID="DataSourceReport" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                 SelectCommand="rptSKFIndiaPaymentMIS" SelectCommandType="StoredProcedure" >
                <SelectParameters>
                  <asp:Parameter Name="UserId" DefaultValue="1" />
                    <asp:Parameter Name ="FinYearId" DefaultValue="6" />

                    <%--<asp:SessionParameter Name="UserId" SessionField="UserId"  ConvertEmptyStringToNull="true"/>
                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" ConvertEmptyStringToNull="true" />--%>
                </SelectParameters>
            </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>