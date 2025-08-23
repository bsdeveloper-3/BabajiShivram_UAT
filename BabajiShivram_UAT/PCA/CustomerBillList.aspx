<%@ Page Title="Bill List" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="CustomerBillList.aspx.cs" Inherits="PCA_CustomerBillList"  EnableEventValidation="false" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release"/>

    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingchecklist" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional"
        RenderMode="Inline">
        <ContentTemplate>
            <script type="text/javascript">
                function GridSelectAllColumn(spanChk) {
                    var oItem = spanChk.children;
                    var theBox = (spanChk.type == "checkbox") ? spanChk : spanChk.children.item[0]; xState = theBox.checked;
                    elm = theBox.form.elements;

                    for (i = 0; i < elm.length; i++) {
                        if (elm[i].type === 'checkbox' && elm[i].checked != xState)
                            elm[i].click();
                    }
                }
            </script>
            <div align="center">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </div>
        <div class="clear"></div>
        <div align="center">
        
        <asp:Label ID="lblMsgforApproveReject" runat="server"></asp:Label>
        <asp:Label ID="lblMsgforReceived" runat="server"></asp:Label>
        <fieldset>
        <legend>Bill</legend>
            <div class="fleft">
                <table>
                    <tr>
                        <td>
                            <uc2:DataFilter ID="DataFilter2" runat="server" />
                        </td>
                        <td valign="baseline">
                            <asp:Button ID="btnAccept" runat="server" Text="Accept Bill" OnClick="btnAccept_Click" />
                        </td>
                        <td valign="baseline">
                            <asp:Button ID="btnProcessed" runat="server" Text="Bill Processed" OnClick="btnProcessed_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="clear">
            </div>
            <div>
            <asp:GridView ID="gvRecievedJobDetail" runat="server" AutoGenerateColumns="False"
                CssClass="table" PagerStyle-CssClass="pgr" DataKeyNames="Billid" AllowPaging="true"
                PagerSettings-Position="TopAndBottom" AllowSorting="True" Width="100%" PageSize="80" 
                OnRowCommand="gvRecievedJobDetail_RowCommand" OnRowDataBound="gvRecievedJobDetail_RowDataBound"
                OnPreRender="gvRecievedJobDetail_PreRender" DataSourceID="SqlDataSourceCustomer">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex +1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkboxSelectAll" align="center" ToolTip="Check All" runat="server" onclick="GridSelectAllColumn(this);"/>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkBillNo" runat="server" ToolTip="Check"></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="BS Job No" SortExpression="JobRefNo">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkJobNo" runat="server" Text='<%#Eval("JobRefNo") %>' CommandArgument='<%#Eval("JobId")%>' CommandName="JobSelect"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" Visible="false"/>
                    <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer"/>
                    <asp:BoundField DataField="BillNo" HeaderText="Bill No" SortExpression="BillNo"/>
                    <asp:BoundField DataField="BillDate" HeaderText="Bill Date" SortExpression="BillDate" DataFormatString="{0:dd/MM/yyyy}"/>
                    <asp:BoundField DataField="BillAmount" HeaderText="Bill Amount" SortExpression="BillAmount" />
                    <asp:BoundField DataField="EBillDate" HeaderText="E-Bill Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="EBillDate"/>
                    <asp:BoundField DataField="ClientPortalDate" HeaderText="Portal Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ClientPortalDate"/>
                    <asp:BoundField DataField="DocketNo" HeaderText="AWB" SortExpression="DocketNo"/>
                    <asp:BoundField DataField="CourierName" HeaderText="Courier" SortExpression="CourierName"/>
                    <asp:BoundField DataField="PCDDeliveryDate" HeaderText="Delivery Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="PCDDeliveryDate"/>
                    <asp:BoundField DataField="BillStatusName" HeaderText="Status" SortExpression="BillStatusName"/>
                    <asp:TemplateField HeaderText="Reject">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkReject" runat="server" Text="Reject" CommandName="Reject"
                                ToolTip="Reject" CommandArgument='<%#Eval("BillId") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="View Bill">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDocument" runat="server" Text="View"
                                CommandName="View" CommandArgument='<%#Eval("BillId")%>'>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerTemplate>
                    <asp:GridViewPager runat="server" />
                </PagerTemplate>
            </asp:GridView>
            </div>
        </fieldset>
        
        <div id="divDataSource">
            <asp:SqlDataSource ID="SqlDataSourceCustomer" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="BL_GetOpenBillList" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="UserId" SessionField="UserId" />
                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>

    </div>
        <%--  START : MODAL POP-UP FOR Bill Reject-  --%>

    <div>
        <asp:LinkButton ID="lnkDomBillReject10" runat="server"></asp:LinkButton>
    </div>
    <div id="divOpBillReject">
    <cc1:ModalPopupExtender ID="BillRejectModalPopup" runat="server" CacheDynamicResults="false" PopupControlID="panBillReject" 
        CancelControlID="imgbtnBillReject" TargetControlID="lnkDomBillReject10" BackgroundCssClass="modalBackground" DropShadow="true">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="panBillReject" runat="server" CssClass="ModalPopupPanel" Width="600px">
        <div class="header">
                <div class="fleft">
                    <asp:Label ID="lblPopupName" runat="server" Text="Bill Rejection"></asp:Label>
                    <asp:HiddenField ID="hdnBillId" Value="0" runat="server" />
                </div>
                <div class="fright">
                    <asp:ImageButton ID="imgbtnBillReject" ImageUrl="../Images/delete.gif" runat="server"
                        ToolTip="Close" />
                </div>
        </div>
            <div id="Div2" runat="server" style="max-height: 250px; overflow: auto; padding: 5px">
                <div>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>Job No</td>
                            <td colspan="3">
                                <asp:Label ID="lblBJVNumber" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Bill No</td>
                            <td>
                                <asp:Label ID="lblBJVBillNo" runat="server"></asp:Label>
                            </td>
                            <td>Bill Date</td>
                            <td>
                                <asp:Label ID="lblBJVBillDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Amount</td>
                            <td>
                                <asp:Label ID="lblBJVAmount" runat="server"></asp:Label>
                            </td>
                            <td></td>
                            <td>                                        
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:RequiredFieldValidator ID="RFVRejection" runat="server" ControlToValidate="txtRejectionRemark" SetFocusOnError="true" Display="Dynamic"
                    ForeColor="Red" ErrorMessage="Remark Required" ValidationGroup="ValidateReject" Font-Bold="true"></asp:RequiredFieldValidator>
                    <div>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>Rejection Remark
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRejectionRemark" runat="server" MaxLength="200" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Button ID="btnRejectBill" runat="server" Text="Reject Bill" OnClick="btnRejectBill_Click" ValidationGroup="ValidateReject" />
                                </td>
                            </tr>
                        </table>
                    </div>
            </div>
    </asp:Panel>
</div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


