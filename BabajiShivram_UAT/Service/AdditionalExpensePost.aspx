<%@ Page Title="Post Additional Expense" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="AdditionalExpensePost.aspx.cs" Inherits="Service_AdditionalExpensePost" EnableEventValidation="false" Culture="en-GB"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingPost" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPendingPost" runat="server" UpdateMode="Conditional" RenderMode="Inline">
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
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false" ></asp:Label>
            </div>
            <div class="clear"></div>   
            <fieldset>
            <legend>Additional Expense</legend>    
            <div class="fleft">
                <table>
                    <tr>
                        <td valign="baseline">
                            <asp:Button ID="btnPost" runat="server" Text="Post/Book Expense To FA System" OnClick="btnPost_Click" OnClientClick="return confirm('Sure to Post Selected Expense?');" />
                        </td>
                        <td>
                            <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                                <asp:Image ID="Image1" runat="server" ImageAlign="AbsMiddle" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                            </asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="clear">
                    <uc1:DataFilter ID="DataFilter1" runat="server" />
                </div>
            <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                DataKeyNames="ExpenseID" PageSize="20" AllowPaging="True" AllowSorting="True"
                    PagerSettings-Position="TopAndBottom" PagerStyle-CssClass="pgr" OnRowCommand="gvJobDetail_RowCommand" 
                OnPreRender="gvJobDetail_PreRender" DataSourceID="PendingExpenseSqlDataSource" >
                <Columns>
                <asp:TemplateField HeaderText="Sl" >
                    <ItemTemplate>
                        <%#Container.DataItemIndex +1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkboxSelectAll" align="center" ToolTip="Check All" runat="server" onclick="GridSelectAllColumn(this);" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk1" runat="server" ToolTip="Check"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:TemplateField HeaderText="JB Booking" ItemStyle-Width="8%">
                <ItemTemplate>
                    <asp:ImageButton ID="imgbtnApproveJob" runat="server" ImageUrl="~/Images/success.png" ToolTip="Post Expense"
                        CommandArgument='<%#Eval("JobID") %>' CommandName="ApproveJob" Width="17px" Height="17px" 
                        Style="padding-right: 1px; margin-right: 0px; padding-left: 1px"></asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="View BJV" SortExpression="FARefNo">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBJVNo" CommandName="ViewBJV" runat="server" Text="View BJV"
                            CommandArgument='<%#Eval("JobRefNO") %>' ToolTip="Check BJV Detail" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Job No" SortExpression="JobRefNO">
                <ItemTemplate>
                <asp:LinkButton ID="lnkJobRefNo" CommandName="select" runat="server" Text='<%#Eval("JobRefNO") %>'
                    CommandArgument='<%#Eval("JobID")+";"+ Eval("ExpenseId") %>' />
                </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" Visible="false" />
                <asp:BoundField DataField="Consignee" HeaderText="Consignee" SortExpression="Consignee" />
                <asp:BoundField DataField="BranchName" HeaderText="Branch" SortExpression="Branch" />
                <asp:BoundField DataField="Mode" HeaderText="Mode" SortExpression="Mode" />
                <asp:BoundField DataField="Billable" HeaderText="Billable ?" SortExpression="Billable" />
                <asp:BoundField DataField="PaidTo" HeaderText="PaidTo" SortExpression="PaidTo" />
                <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />
                <asp:BoundField DataField="MgmtApprovalName" HeaderText="Approved By" SortExpression="MgmtApprovalName" />
                <asp:BoundField DataField="ApprovalDate" HeaderText="Approval Date" DataFormatString="{0:dd/MM/yyyy}"
                    SortExpression="ApprovalDate" />
                <asp:BoundField DataField="ExpenseName" HeaderText="Type" SortExpression="ExpenseName" />
                <asp:BoundField DataField="ApprovalRemark" HeaderText="Approval Remark" SortExpression="ApprovalRemark" />
                <asp:BoundField DataField="ExpenseDate" HeaderText="Expense Date" DataFormatString="{0:dd/MM/yyyy}"
                    SortExpression="ExpenseDate" />
                <asp:TemplateField HeaderText="User Remark" SortExpression="ExpenseRemark">
                    <ItemTemplate>
                        <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                        <asp:Label ID="lblExpenseRemark" runat="server" 
                            Text='<%# Eval("ExpenseRemark")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
                <PagerTemplate>
                    <asp:GridViewPager runat="server" />
                </PagerTemplate>
            </asp:GridView>
            </fieldset>
            <div id="BJV_Popup">
                <asp:LinkButton ID="lnkModelPopup21" runat="server" />
                    <cc1:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="lnkModelPopup21"
                        PopupControlID="Panel21">
                    </cc1:ModalPopupExtender>
                    <asp:Panel ID="Panel21" Style="display: none" runat="server">
                        <fieldset>
                            <legend>BJV DETAIL</legend>
                            <div class="header">
                                <div class="fright">
                                    <asp:ImageButton ID="ImageButton1" ImageUrl="~/Images/delete.gif" runat="server"
                                        OnClick="btnCancelBJVPopup_Click" ToolTip="Close" />
                                </div>
                            </div>
                            <div class="clear"></div>
                            <div id="Div3" runat="server" style="max-height: 560px; overflow: auto;">
                                <asp:GridView ID="gvBJVDetail" runat="server" CssClass="table" AutoGenerateColumns="false" AllowPaging="false"
                                    OnPreRender="gvBJVDetail_PreRender">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex +1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Ref" HeaderText="Job No" />
                                        <asp:BoundField DataField="Type" HeaderText="Type" />
                                        <asp:BoundField DataField="BookName" HeaderText="Book Name" />
                                        <asp:BoundField DataField="billno" HeaderText="Bill No" />
                                        <asp:BoundField DataField="billdate" HeaderText="Bill Date" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="par_code" HeaderText="Party Code" />
                                        <asp:BoundField DataField="Debit" HeaderText="Debit" />
                                        <asp:BoundField DataField="Credit" HeaderText="Credit" />
                                        <asp:TemplateField HeaderText="Narration">
                                        <ItemTemplate>
                                            <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                                            <asp:Label ID="lblNarration" runat="server" 
                                                Text='<%# Eval("narration")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                    </asp:Panel>
            </div>
            <div>
                <asp:SqlDataSource ID="PendingExpenseSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetJobDetailForExpensePost" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


