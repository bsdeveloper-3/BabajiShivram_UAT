<%@ Page Title="Mumbai/Chennai A Labour" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="MumbaiExpense.aspx.cs" Inherits="Service_MumbaiExpense" EnableEventValidation="false" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />    
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upFillDetails" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                    <img alt="progress" src="../images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>    
        <asp:UpdatePanel ID="upFillDetails" runat="server" UpdateMode="Conditional">
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
        <asp:Label ID="lblMessage" runat="server" CssClass="errorMsg" EnableViewState="false"></asp:Label>
            <asp:HiddenField ID="hdnInvoiceId" runat="server" />
            <asp:HiddenField ID="hdnOperationType" runat="server" />
        </div>
        <fieldset Class="fieldset-AutoWidth"><legend>Search</legend>
            <div>
            <b>Expense Date From</b> &nbsp;<asp:TextBox ID="txtStatusDateFrom" AutoPostBack="true" runat="server" MaxLength="100" Width="80px"></asp:TextBox>
            <cc1:CalendarExtender ID="calStatusDateFrom" runat="server" EnableViewState="False"
                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtStatusDateFrom"></cc1:CalendarExtender>
                <b> To Date</b> &nbsp;<asp:TextBox ID="txtStatusDateTo" AutoPostBack="true" runat="server" MaxLength="100" Width="80px"></asp:TextBox>
            <cc1:CalendarExtender ID="calStatusDateTo" runat="server" EnableViewState="False"
                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtStatusDateTo"></cc1:CalendarExtender>
           <br />
            <div>
            <b> Branch </b>
            <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="true">
                <asp:ListItem Text="MUMBAI - SAKINAKA" Value="3"></asp:ListItem>
                <asp:ListItem Text="Chennai - SEA" Value="6"></asp:ListItem>
            </asp:DropDownList>
            <b></b>
            <asp:DropDownList ID="ddModule" runat="server" AutoPostBack="true">
                <asp:ListItem Text="IMPORT" Value="1"></asp:ListItem>
            </asp:DropDownList>
            <b>Mode</b>
            <asp:DropDownList ID="ddlMode" runat="server" AutoPostBack="true">
                <asp:ListItem Text="SEA" Value="2"></asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="btnView" runat="server" Text="View A Labour Expense" />
        </div>
        </fieldset>
        <fieldset><legend>Mumbai/Chennai Expense Posting - A Labour</legend>
        <div>
            <asp:Button ID="btnPostAll" runat="server" Text="Mumbai/Chennai - Post All Expense" OnClick="btnPostAll_Click" />
            <asp:Panel ID="pnlFilter" runat="server">
                <div class="fleft">
                    <uc1:DataFilter ID="DataFilter1" runat="server" />
                </div>
                <div class="fleft" style="margin-left:20px; padding-top:3px;">
                    <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                        <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                    </asp:LinkButton>
                </div>
                <%--<div class="fleft" style="margin-left:10px; padding-top:3px;">
                    <asp:LinkButton ID="lnkSummaryExcel" runat="server" Text="Export To Summary Excel" OnClick="lnkSummaryExcel_Click">
                    </asp:LinkButton>
                </div>--%>
            </asp:Panel>
        </div>
        <div class="m clear"></div>
        <asp:GridView ID="gvDetail" runat="server" AutoGenerateColumns="false" CssClass="table" DataKeyNames="JobId"
            AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataSourceID="ExpenseSqlDataSource" 
            AllowPaging="True" AllowSorting="True" PageSize="1000" PagerSettings-Position="TopAndBottom"
            ShowFooter="true" Width="100%" OnRowCommand="gvDetail_RowCommand" OnRowDataBound="gvDetail_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
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
                <asp:TemplateField HeaderText="Post JB" ItemStyle-Width="8%">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgbtnApproveJob" runat="server" ImageUrl="~/Images/success.png" ToolTip="Post Expense"
                            CommandArgument='<%#Eval("JobID") %>' CommandName="ApproveJob" Width="17px" Height="17px" 
                            Style="padding-right: 1px; margin-right: 0px; padding-left: 1px"></asp:ImageButton>
                        <%--<asp:ImageButton ID="imgbtnHoldJob" runat="server" ImageUrl="~/Images/hold.png"
                            CommandArgument='<%#Eval("JobID") %>' CommandName="Hold" Width="18px" Height="18px" ToolTip="Hold Expense"
                            Style="padding-right: 1px; margin-right: 0px; padding-left: 1px"></asp:ImageButton>--%>
                        </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="View BJV" SortExpression="FARefNo">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBJVNo" CommandName="ViewBJV" runat="server" Text="View BJV"
                            CommandArgument='<%#Eval("JobRefNO") %>' ToolTip="Check BJV Detail" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Job No" SortExpression="JobRefNO">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkJobRefNo" CommandName="select" runat="server" Text='<%#Eval("JobRefNO") %>'
                            CommandArgument='<%#Eval("JobId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="JobRefNO" HeaderText="Job No" Visible="False" />
                <asp:TemplateField HeaderText="A Labour Charges" SortExpression="PaidCharges">
                    <ItemTemplate>
                        <asp:Label ID="lblCharges" runat="server" Text='<%#Eval("PaidCharges")%>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblTotal" runat="server" Font-Bold="true" BackColor="Yellow" Font-Size="14" ></asp:Label>
                    </FooterTemplate>
                </asp:TemplateField>
                <%--<asp:BoundField DataField="PaidCharges" HeaderText="A Labour Charges" SortExpression="PaidCharges"  />--%>
                <asp:BoundField DataField="BranchName" HeaderText="Branch" SortExpression="BranchName" />
                <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                <asp:BoundField DataField="CFS" HeaderText="CFS" SortExpression="CFS" />
                <asp:BoundField DataField="CFSUserName" HeaderText="CFS User Name" SortExpression="CFSUserName" />
                <asp:BoundField DataField="BETypeName" HeaderText="BE Type" SortExpression="BETypeName" />
                <asp:BoundField DataField="RMSNonRMS" HeaderText="RMS/NonRMS" SortExpression="RMSNonRMS" />
                <asp:BoundField DataField="Count20" HeaderText="Count20" SortExpression="Count20" />
                <asp:BoundField DataField="Count40" HeaderText="Count40" SortExpression="Count40" />
                <asp:BoundField DataField="CountLCL" HeaderText="CountLCL" SortExpression="CountLCL" />
                <asp:TemplateField HeaderText="Date" SortExpression="ExpenseDate">
                    <ItemTemplate>
                        <asp:Label ID="lblExpenseDate" runat="server" Text='<%#Eval("ExpenseDate","{0:dd/MM/yyyy}") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            
            </Columns>
            
        </asp:GridView>
        </fieldset>
        
        <%--  START : MODAL POP-UP FOR JB Booking- 1/Hold-2 EXPENSE  --%>

            <div>
                <asp:LinkButton ID="lnkRejectExp" runat="server"></asp:LinkButton>
            </div>

            <div id="divOpExpense">
                <cc1:ModalPopupExtender ID="RejectModalPopupExtender" runat="server" CacheDynamicResults="false" PopupControlID="panRejectJob" 
                    CancelControlID="imgbtnRejectExp" TargetControlID="lnkRejectExp" BackgroundCssClass="modalBackground" DropShadow="true">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="panRejectJob" runat="server" CssClass="ModalPopupPanel" Width="600px">
                    <div class="header">
                        <div class="fleft">
                            <asp:Label ID="lblPopupName" runat="server"></asp:Label>
                        </div>
                        <div class="fright">
                            <asp:ImageButton ID="imgbtnRejectExp" ImageUrl="../Images/delete.gif" runat="server"
                                ToolTip="Close" />
                        </div>
                    </div>
                    <div id="Div2" runat="server" style="max-height: 250px; overflow: auto; padding: 5px">
                        <div>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <div class="m">
                                    <center>
                                        <asp:Label ID="lblError_RejectExp" runat="server"></asp:Label>
                                    </center>
                                </div>
                                <tr>
                                    <td>Job Number</td>
                                    <td>
                                        <asp:Label ID="lblJobNumber" runat="server"></asp:Label>
                                    </td>
                                    <td>Branch</td>
                                    <td>
                                        <asp:Label ID="lblBranch1" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>Expense Date</td>
                                    <td>
                                        <asp:Label ID="lblExpenseDate1" runat="server"></asp:Label>
                                    </td>
                                    <td>Mode</td>
                                    <td>
                                        <asp:Label ID="lblMode1" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Amount</td>
                                    <td>
                                        <asp:Label ID="lblAmount1" runat="server"></asp:Label>
                                    </td>
                                    <td>Remark</td>
                                    <td>
                                        <asp:TextBox ID="lblRemark1" runat="server" TextMode="MultiLine" Width="200px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                
                                <tr>
                                    <td colspan="2">
                                        <asp:Button ID="btnApproveJob" runat="server" Text="Post Expense To FA System" OnClick="btnApproveJob_OnClick"  Visible="false" ValidationGroup="ValidateExpense" />
                                        <asp:Button ID="btnRejectJob" runat="server" Text="Reject"  Visible="false" ValidationGroup="ValidateExpense" />
                                        <asp:Button ID="btnHoldJob" runat="server" Text="Hold Expense" Visible="false" ValidationGroup="ValidateExpense" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            
            <div id="BJV_Popup">
            <asp:LinkButton ID="lnkModelPopup21" runat="server" />
            <cc1:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="lnkModelPopup21" PopupControlID="Panel21">
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
            <asp:SqlDataSource ID="ExpenseSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="GetDailyALabourExp" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:ControlParameter Name="ExpenseDateFrom" ControlID="txtStatusDateFrom" PropertyName="Text" Type="datetime" />
                    <asp:ControlParameter Name="ExpenseDateTo" ControlID="txtStatusDateTo" PropertyName="Text" Type="datetime" />
                    <asp:ControlParameter Name="ModuleID" ControlID="ddModule" PropertyName="SelectedValue" Type="Int32" />
                    <asp:ControlParameter Name="BranchID" ControlID="ddlBranch" PropertyName="SelectedValue" Type="Int32" />
                    <asp:ControlParameter Name="TransMode" ControlID="ddlMode" PropertyName="SelectedValue" Type="Int32" />
                    <asp:SessionParameter Name="UserId" SessionField="UserId" />
                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>



