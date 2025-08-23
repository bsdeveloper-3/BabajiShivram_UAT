<%@ Page Title="View Labour Expense" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="LabourExpenseView.aspx.cs" Inherits="Service_LabourExpenseView" Culture="en-GB" %>
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
        
        <div align="center">
        <asp:Label ID="lblMessage" runat="server" CssClass="errorMsg" EnableViewState="false"></asp:Label>
        </div>
        <fieldset Class="fieldset-AutoWidth"><legend>Search</legend>
            <div>
            <b>Expense Date From</b> &nbsp;
            <asp:TextBox ID="txtStatusDateFrom" ValidationGroup="Required" AutoPostBack="true" runat="server" MaxLength="100" Width="80px"></asp:TextBox>
            <cc1:CalendarExtender ID="calStatusDateFrom" runat="server" EnableViewState="False"
                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtStatusDateFrom"></cc1:CalendarExtender>
                <b> To Date</b> &nbsp;<asp:TextBox ID="txtStatusDateTo" AutoPostBack="true" runat="server" MaxLength="100" Width="80px"></asp:TextBox>
                <cc1:MaskedEditExtender ID="MskEdtFrom" TargetControlID="txtStatusDateFrom" Mask="99/99/9999" MessageValidatorTip="true"
                    MaskType="Date" AutoComplete="false" runat="server">
                </cc1:MaskedEditExtender>
                
                <cc1:MaskedEditValidator ID="MskEdtValFromDate" ControlExtender="MskEdtFrom" ControlToValidate="txtStatusDateFrom" IsValidEmpty="false"
                    InvalidValueMessage="Invalid From Date" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true"
                    MinimumValueMessage="Invalid From Date" MaximumValueMessage="Invalid From Date" 
                    MinimumValueBlurredText="Invalid From Date" MaximumValueBlurredMessage="Invalid From Date"
                    ValidationGroup="Required" EmptyValueMessage="Date Required" EmptyValueBlurredText="Required" runat="Server"></cc1:MaskedEditValidator>
            
                <cc1:CalendarExtender ID="calStatusDateTo" runat="server" EnableViewState="False"
                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtStatusDateTo"></cc1:CalendarExtender>
           <br />
            <div>
            <b> Branch </b>
            <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="true">
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
                <asp:ListItem Text="SEA" Value="2"></asp:ListItem>
                <asp:ListItem Text="AIR" Value="1"></asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="btnView" runat="server" Text="View A Labour Expense" ValidationGroup="Required"/> 
                &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click" ValidationGroup="Required">
                <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
            </asp:LinkButton>
        </div>
        </fieldset>
        <fieldset><legend runat="server">Expense Detail - A Labour</legend>
        <div class="m clear"></div>
        <asp:GridView ID="gvDetail" runat="server" AutoGenerateColumns="true" CssClass="table" DataKeyNames="JobRefNO"
            AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataSourceID="ExpenseSqlDataSource" 
            AllowPaging="True" AllowSorting="True" PageSize="1000" PagerSettings-Position="TopAndBottom"
            ShowFooter="true" Width="100%">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex +1 %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            
        </asp:GridView>
        </fieldset>
            
        <div>
            <asp:SqlDataSource ID="ExpenseSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="GetDailyALabourExpView" SelectCommandType="StoredProcedure">
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


