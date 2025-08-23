<%@ Page Title="Bill Dispatch - Later" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PendingBillDispatchLater.aspx.cs" 
    Inherits="PCA_PendingBillDispatchLater" EnableEventValidation="false" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
            <asp:HiddenField ID="hdnJobId" runat="server" />
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
        <legend>Bill Dispatch Later</legend>
            <div class="fleft">
                <table>
                    <tr>
                        <%--     start covering letter--%>
                        <td valign="baseline">
                            <asp:Button ID="btnApprove" runat="server" Text="Approve" OnClick="btnApprove_Click" Visible="false" />
                        </td>
                        <td valign="baseline">
                            <asp:Button ID="btnCoveringLetter" runat="server" Text="Covering Letter" OnClick="btnCoveringLetter_Click" />
                        </td>

                        <td valign="baseline">
                            <asp:Button ID="btnMyPaccoAWBGeneration" runat="server" Text="MyPacco Dispatch" OnClick="btnMyPaccoAWBGeneration_Click"  />
                        </td>
                        <td valign="baseline">
                            <asp:Button ID="btnEBill" runat="server" Text="E-Bill Dispatch" OnClick="btnEBill_Click" Visible="false"  />
                        </td>
                        <%-- end covering letter--%>

                        <td valign="baseline">
                            <asp:LinkButton ID="lnkreceive" runat="server" OnClick="lnkreceive_Click" data-tooltip="&nbsp; Export To Excel">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Excel.jpg" />
                            </asp:LinkButton></td>
                    </tr>
                </table>
            </div>
            <div class="clear">
            </div>
            <div class="fleft">
                <uc2:DataFilter ID="DataFilter2" runat="server" />
            </div>
            <div class="clear">
            </div>
            <div>
            <asp:GridView ID="gvRecievedJobDetail" runat="server" AutoGenerateColumns="False"
                CssClass="table" PagerStyle-CssClass="pgr" DataKeyNames="JobId,CustomerId" AllowPaging="false"
                PagerSettings-Position="TopAndBottom" AllowSorting="True" Width="100%" PageSize="40" 
                OnRowCommand="gvRecievedJobDetail_RowCommand" OnRowDataBound="gvRecievedJobDetail_RowDataBound"
                OnPreRender="gvNonRecievedJobDetail_PreRender" DataSourceID="SqlDataSourceCustomer">
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
                            <asp:CheckBox ID="chkjoball" runat="server" ToolTip="Check"></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="BS Job No" SortExpression="JobRefNo">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkJobNo" runat="server" Text='<%#Eval("JobRefNo") %>' CommandArgument='<%#Eval("JobId")%>' CommandName="JobSelect"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" Visible="false"/>
                    <asp:BoundField DataField="JobTypeName" HeaderText="Job Type" />
                    <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer"/>
                    <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount"/>
                    <asp:BoundField DataField="AgingFSB" HeaderText="Aging_FSB" SortExpression="AgingFSB" />
                    <asp:BoundField DataField="AgingFinalCheck" HeaderText="Aging_FinalCheck" SortExpression="AgingFinalCheck" />
                    <asp:BoundField DataField="Ebill" HeaderText="E-Bill"/>
                </Columns>
            </asp:GridView>
            </div>
        </fieldset>
        
        <div id="divDataSource">
            <asp:SqlDataSource ID="SqlDataSourceCustomer" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="BL_GetPendngBillDispatchLater" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="UserId" SessionField="UserId" />
                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>

    </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

