<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AwaitingAdvice.aspx.cs"
    Inherits="FreightOperation_AwaitingAdvice" Title="Awaiting Billing Advice" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="updateOperation" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="updateOperation" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </div>
            <fieldset>
                <legend>Awaiting Billing Advice</legend>
                <div class="m clear">
                    <div class="fleft">
                        <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                            <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                        </asp:LinkButton>
                        &nbsp;
                    </div>
                    <div class="fleft">
                        <uc1:datafilter id="DataFilter1" runat="server" />
                    </div>
                    <div class="fleft">
                        &nbsp;&nbsp;<span style="color: Teal;">** Aging: DO Date to Today</span>
                    </div>
                </div>
                <div class="clear"></div>
                <div>
                    <asp:GridView ID="gvFreight" runat="server" DataSourceID="GridViewSqlDataSource" DataKeyNames="EnqId" Width="100%"
                        AllowPaging="True" PageSize="20" AllowSorting="true" PagerStyle-CssClass="pgr" PagerSettings-Position="TopAndBottom"
                        CssClass="table" AutoGenerateColumns="False" OnRowCommand="gvFreight_RowCommand" OnPreRender="gvFreight_PreRender"
                        OnRowDataBound="gvFreight_RowDataBound"
                        >
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1 %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Hold">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgbtnHoldJob" runat="server" ImageUrl="~/Images/UnlockImg.png"
                                        CommandArgument='<%#Eval("EnqId") + ";" + Eval("Amount") + ";" + Eval("FRJobNo")%>' CommandName="Hold" Width="18px" Height="18px" ToolTip="Hold Job."
                                        Style="padding-right: 1px; margin-right: 0px; padding-left: 1px"></asp:ImageButton>
                                    <asp:ImageButton ID="imgbtnUnholdJob" runat="server" ImageUrl="~/Images/LockImg.png"
                                        CommandArgument='<%#Eval("EnqId") + ";" + Eval("Amount") + ";" + Eval("FRJobNo")%>' CommandName="Unhold" Width="18px" Height="18px" ToolTip="Unhold Job."
                                        Style="padding-right: 1px; margin-right: 0px; padding-left: 1px"></asp:ImageButton>
                                    <asp:HiddenField ID="hdnJobId" runat="server" Value='<%#Eval("EnqId")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Job No" SortExpression="FRJobNo">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkViewFreight" runat="Server" Text='<%#Eval("FRJobNo") %>' CommandName="navigate"
                                        CommandArgument='<%#Eval("EnqId")+";"+ Eval("TypeName")+";"+ Eval("ModeName") %>' CausesValidation="false"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FRJobNo" HeaderText="Job No" Visible="false" />
                            <asp:BoundField DataField="HoldRemark" HeaderText="Hold Remark" SortExpression="HoldRemark" />
                            <asp:BoundField DataField="ReasonforPendency" HeaderText="Reason for Pendency" SortExpression="ReasonforPendency" />
                            <asp:BoundField DataField="TypeName" HeaderText="Type" SortExpression="TypeName"/>
                            <asp:BoundField DataField="ENQRefNo" HeaderText="Enquiry No" SortExpression="ENQRefNo" />
                            <asp:BoundField DataField="EnquiryUser" HeaderText="Freight SPC" SortExpression="EnquiryUser" />
                            <asp:BoundField DataField="ETA" HeaderText="ETA" SortExpression="ETA" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="Aging" HeaderText="Aging" SortExpression="Aging" />
                            <asp:BoundField DataField="AgentName" HeaderText="Agent" SortExpression="AgentName" />
                            <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                            <asp:BoundField DataField="Consignee" HeaderText="Consignee" SortExpression="Consignee" />
                            <asp:BoundField DataField="ModeName" HeaderText="Mode" SortExpression="ModeName" />
                            <asp:BoundField DataField="CountOf20" HeaderText="20" SortExpression="CountOf20" />
                            <asp:BoundField DataField="CountOf40" HeaderText="40" SortExpression="CountOf40" />
                            <asp:BoundField DataField="LCLVolume" HeaderText="LCL(CBM)" SortExpression="LCLVolume" />
                            <asp:BoundField DataField="NoOfPackages" HeaderText="Pkgs" SortExpression="NoOfPackages" />
                            <asp:BoundField DataField="GrossWeight" HeaderText="Gross WT" SortExpression="GrossWeight" />
                            <asp:BoundField DataField="TermsName" HeaderText="Terms" SortExpression="TermsName" />
                            <asp:BoundField DataField="TypeName" HeaderText="Type" SortExpression="TypeName" Visible="false" />
                            <asp:BoundField DataField="CountryName" HeaderText="Country" SortExpression="CountryName" />
                            <asp:BoundField DataField="LoadingPortName" HeaderText="Loading Port" SortExpression="LoadingPortName" />
                            <asp:BoundField DataField="PortOfDischargedName" HeaderText="Port of Discharged" SortExpression="PortOfDischargedName" />
                        </Columns>
                        <PagerTemplate>
                            <asp:GridViewPager ID="GridViewPager1" runat="server" />
                        </PagerTemplate>
                    </asp:GridView>
                    <asp:SqlDataSource ID="GridViewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="FOP_GetAwaitingAdvice" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:SessionParameter Name="UserId" SessionField="UserId" />
                            <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
            </fieldset>

            <%--  START : MODAL POP-UP FOR HOLD EXPENSE  --%>

            <div id="divHoldExpense">
                <cc1:ModalPopupExtender ID="mpeHoldExpense" runat="server" CacheDynamicResults="false"
                    PopupControlID="pnlHoldExpense" CancelControlID="imgbtnHoldExp" TargetControlID="hdnHoldExp" BackgroundCssClass="modalBackground" DropShadow="true">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pnlHoldExpense" runat="server" CssClass="ModalPopupPanel" Width="600px" Height="350px">
                    <div class="header">
                        <div class="fleft">
                            <asp:Label ID="lblHoldPopupName" runat="server"></asp:Label>
                        </div>
                        <div class="fright">
                            <asp:ImageButton ID="imgbtnHoldExp" ImageUrl="../Images/delete.gif" runat="server"
                                ToolTip="Close" />
                        </div>
                    </div>
                    <div class="m">
                        <asp:HiddenField ID="hdnJobRefNo" runat="server" />
                        <asp:HiddenField ID="hdnJobId_hold" runat="server" />
                        <asp:HiddenField ID="hdnJobId" runat="server" />
                    </div>
                    <!-- Lists Of All Documents -->
                    <div id="Div3" runat="server" style="max-height: 300px; overflow: auto; padding: 5px">
                        <center>
                            <asp:Label ID="lblError_HoldExp" runat="server" EnableViewState="false"></asp:Label>
                            <div style="width:100%">
                                <asp:FormView ID="fvHoldJobDetail" runat="server" DataSourceID="DataSourceHoldJob" DataKeyNames="JobId">
                                    <ItemTemplate>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                            <tr>
                                                <td>BS Job Number</td>
                                                <td>
                                                    <asp:Label ID="lblBSJobNo" runat="server" Text='<%#Eval("JobRefNo") %>'></asp:Label>
                                                </td>
                                                <td>Customer</td>
                                                <td>
                                                    <asp:Label ID="lblCustomer" runat="server" Text='<%#Eval("Customer") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Consignee</td>
                                                <td>
                                                    <asp:Label ID="lblConsignee" runat="server" Text='<%#Eval("Consignee") %>'></asp:Label>
                                                </td>
                                                <td>Amount</td>
                                                <td>
                                                    <asp:Label ID="lblAmount" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Out of Charge Date</td>
                                                <td colspan="3">
                                                    <asp:Label ID="lblOutOfChargeDt" runat="server" Text='<%#Eval("OutOfChargeDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                </td>
                                           
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblReasonHold" runat="server" Text="Rejection Type" ForeColor="Black" Font-Size="9"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <asp:DropDownList ID="ddlReasonHold" runat="server" DataSourceID="DsReasonforpendency" ForeColor="Black" Font-Size="9" DataTextField="ReasonforPendency" DataValueField="Id">
                                                        <asp:ListItem Value="0" Text="-Select-" Selected="True"></asp:ListItem>
                                                    </asp:DropDownList>

                                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlReasonHold"
                                                        ErrorMessage="Please Select Rejection Type" InitialValue="0" ValidationGroup="vgAddHoldExpense"></asp:RequiredFieldValidator>

                                                     <asp:SqlDataSource ID="DsReasonforpendency" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                                            SelectCommand="GetpendencyReason" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                                                </td>
                                          
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:FormView>
                            </div>
                            <br />
                            &nbsp;
                            <asp:RequiredFieldValidator ID="rfvReason" runat="server" ControlToValidate="txtReason" SetFocusOnError="true" Display="Dynamic"
                                ForeColor="Red" ErrorMessage="* Enter Reason" ValidationGroup="vgAddHoldExpense" Font-Bold="true"></asp:RequiredFieldValidator>
                            <div>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                    <tr>
                                        <td>Reason                                      
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" Rows="4" Width="500px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Button ID="btnHoldJob" runat="server" ValidationGroup="vgAddHoldExpense" Text=""
                                                OnClientClick="return confirm('Are you sure to hold this job?');" OnClick="btnHoldJob_OnClick" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </center>
                    </div>
                </asp:Panel>
            </div>
            <div>
                <asp:LinkButton ID="hdnHoldExp" runat="server"></asp:LinkButton>
            </div>

            <%--  END   : MODAL POP-UP FOR HOLD EXPENSE  --%>

            <asp:SqlDataSource ID="DataSourceHoldJob" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="Get_FRJobDetailById" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:ControlParameter ControlID="hdnJobId" Name="JobId" PropertyName="Value" />
                </SelectParameters>
            </asp:SqlDataSource>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

