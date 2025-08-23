<%@ Page Title="A-Labour Posting" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="LabourExpenseTrack.aspx.cs" Inherits="Service_LabourExpenseTrack" EnableEventValidation="false" Culture="en-GB"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
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
        <div align="center">
        <asp:Label ID="lblMessage" runat="server" CssClass="errorMsg" EnableViewState="false"></asp:Label>
        </div>
        <fieldset Class="fieldset-AutoWidth"><legend>Search</legend>
            <div>
            <b>Posting From Date</b> &nbsp;<asp:TextBox ID="txtStatusDateFrom" AutoPostBack="true" runat="server" MaxLength="100" Width="80px"></asp:TextBox>
            <cc1:CalendarExtender ID="calStatusDateFrom" runat="server" EnableViewState="False"
                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtStatusDateFrom"></cc1:CalendarExtender>
                <b> To Date</b> &nbsp;<asp:TextBox ID="txtStatusDateTo" AutoPostBack="true" runat="server" MaxLength="100" Width="80px"></asp:TextBox>
            <cc1:CalendarExtender ID="calStatusDateTo" runat="server" EnableViewState="False"
                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtStatusDateTo"></cc1:CalendarExtender>
           <br />
            <b> Branch </b>
            <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="true">
                <asp:ListItem Text="--All--" Value="0"></asp:ListItem>
                 <asp:ListItem Text="MUMBAI - SAKINAKA" Value="3"></asp:ListItem>
                <asp:ListItem Text="MUMBAI - CARGO COMPLEX" Value="2"></asp:ListItem>
                <asp:ListItem Text="DELHI" Value="5"></asp:ListItem>
                <asp:ListItem Text="CHENNAI" Value="6"></asp:ListItem>
                <asp:ListItem Text="KOLKATA" Value="7"></asp:ListItem>
                <asp:ListItem Text="MUNDRA" Value="8"></asp:ListItem>
                <asp:ListItem Text="BANGALORE" Value="20"></asp:ListItem>
                <asp:ListItem Text="JAIPUR" Value="15"></asp:ListItem>
                <asp:ListItem Text="HYDERABAD" Value="23"></asp:ListItem>
                <asp:ListItem Text="AHMEDABAD" Value="13"></asp:ListItem>
                <asp:ListItem Text="ANKLESHWAR" Value="16"></asp:ListItem>
                <asp:ListItem Text="PUNJAB" Value="28"></asp:ListItem>      
                <asp:ListItem Text="VIZAG" Value="14"></asp:ListItem>
                <asp:ListItem Text="PIPAVAV" Value="17"></asp:ListItem>
            </asp:DropDownList>
            <b></b>
            <asp:DropDownList ID="ddModule" runat="server" AutoPostBack="true">
                <asp:ListItem Text="IMPORT" Value="1"></asp:ListItem>
                <asp:ListItem Text="EXPORT" Value="2"></asp:ListItem>
            </asp:DropDownList>
            <b>Mode</b>
            <asp:DropDownList ID="ddlMode" runat="server" AutoPostBack="true">
                <asp:ListItem Text="-All-" Value="0"></asp:ListItem>
                <asp:ListItem Text="SEA" Value="2"></asp:ListItem>
                <asp:ListItem Text="AIR" Value="1"></asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="btnView" runat="server" Text="View A-Labour Posting" />
        </div>
        </fieldset>
        <fieldset><legend runat="server">Track Expense Posting - A Labour
            <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                <asp:Image ID="Image1" runat="server" ImageAlign="AbsMiddle" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
            </asp:LinkButton>
        </legend>
        <asp:GridView ID="gvDetail" runat="server" AutoGenerateColumns="false" CssClass="table" DataKeyNames="JobId"
            AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataSourceID="ExpenseSqlDataSource" 
            AllowPaging="True" AllowSorting="True" PageSize="80" PagerSettings-Position="TopAndBottom"
            Width="100%" OnRowCommand="gvDetail_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex +1 %>
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
                <asp:BoundField DataField="PSSVCHNo" HeaderText="JB No" SortExpression="PSSVCHNo" />
                <asp:BoundField DataField="PaidCharges" HeaderText="Charges" SortExpression="PaidCharges"  />
                <asp:BoundField DataField="BranchName" HeaderText="Branch" SortExpression="BranchName" />
                <asp:BoundField DataField="CustName" HeaderText="Customer" SortExpression="CustName" />
                <asp:BoundField DataField="CFS" HeaderText="CFS" SortExpression="CFS" />
                <asp:BoundField DataField="CFSUserName" HeaderText="CFS User Name"  />
                <asp:BoundField DataField="BETypeName" HeaderText="BE Type" SortExpression="BETypeName" />
                <asp:BoundField DataField="JobTypeName" HeaderText="Job Type" SortExpression="JobTypeName" />
                <asp:BoundField DataField="DeliveryType" HeaderText="Delivery Type" SortExpression="DeliveryType" />
                <asp:BoundField DataField="RMSNonRMS" HeaderText="R/N" SortExpression="RMSNonRMS" />
                <asp:BoundField DataField="Count20" HeaderText="20" SortExpression="Count20" />
                <asp:BoundField DataField="Count40" HeaderText="40" SortExpression="Count40" />
                <asp:BoundField DataField="CountLCL" HeaderText="LCL" SortExpression="CountLCL" />
                <asp:BoundField DataField="PostingDate" HeaderText="Posting Date" SortExpression="PostingDate" DataFormatString="{0:dd/MM/yyyy}"  />
                <%--<asp:BoundField DataField="ExpenseDate" HeaderText="Date" SortExpression="ExpenseDate" DataFormatString="{0:dd/MM/yyyy}"  />--%>
            </Columns>
            <PagerTemplate>
                <asp:GridViewPager runat="server" />
            </PagerTemplate>
        </asp:GridView>
        </fieldset>
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
                SelectCommand="GetFAPostingALabour" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:ControlParameter Name="PostingDateFrom" ControlID="txtStatusDateFrom" PropertyName="Text" Type="datetime" />
                    <asp:ControlParameter Name="PostingDateTo" ControlID="txtStatusDateTo" PropertyName="Text" Type="datetime" />
                    <asp:ControlParameter Name="ModuleID" ControlID="ddModule" PropertyName="SelectedValue" Type="Int32" />
                    <asp:ControlParameter Name="BranchID" ControlID="ddlBranch" PropertyName="SelectedValue" Type="Int32" />
                    <asp:ControlParameter Name="TransMode" ControlID="ddlMode" PropertyName="SelectedValue" Type="Int32" />                    
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


