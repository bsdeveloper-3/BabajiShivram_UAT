<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MistakeTracking.aspx.cs" Inherits="MistakeTracking" 
    MasterPageFile="~/MasterPage.master" Title="Mistake Tracking" Culture="en-GB"%>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPanel" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPanel" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center" style="vertical-align: top">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <div class="clear"></div>
            <div>
            <fieldset>
        <legend>Mistake Tracking</legend>    
            <div>
                <uc1:DataFilter ID="DataFilter1" runat="server" />
            </div>
            <div class="clear">
            </div>
                <div>
                <asp:GridView ID="gvMistakeDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                    PagerStyle-CssClass="pgr" OnPreRender="gvMistakeDetail_PreRender" DataKeyNames="lId" 
                    DataSourceID="MistakeDetailSqlDataSource" AllowPaging="True" AllowSorting="True" 
                    PagerSettings-Position="TopAndBottom" PageSize="20" Width="100%" OnRowCommand="gvMistakeDetail_RowCommand"
                    OnRowEditing="gvMistakeDetail_RowEditing" OnRowCancelingEdit="gvMistakeDetail_RowCancelingEdit"
                    OnRowUpdating="gvMistakeDetail_RowUpdating">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit" Width="22" runat="server"
                                    Text="Edit" Font-Underline="true"></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update" runat="server" Text="Update" Font-Underline="true" 
                                    ValidationGroup="Required" OnClientClick="return confirm('Are You Sure to Update Log?');"></asp:LinkButton>
                                <asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" CausesValidation="false"
                                    runat="server" Text="Cancel" Font-Underline="true"></asp:LinkButton>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Mistake By" DataField="MistakeByName" SortExpression="MistakeByName" ReadOnly="true"/>
                        <asp:TemplateField HeaderText="Mistake Date" SortExpression="MistakeDate">
                            <ItemTemplate>
                                <asp:Label ID="lblStatusDate" runat="server" Text='<%#Eval("MistakeDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtMisakeDate" runat="server" Text='<%#Eval("MistakeDate","{0:dd/MM/yyyy}") %>' Width="70"></asp:TextBox>
                                <cc1:CalendarExtender ID="calMistDate" runat="server" Enabled="True" EnableViewState="False"
                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="txtMisakeDate" PopupPosition="BottomRight"
                                    TargetControlID="txtMisakeDate">
                                </cc1:CalendarExtender>
                                <asp:RequiredFieldValidator ID="RFVmistDate" runat="server" ErrorMessage="Please Enter Mistake Date." SetFocusOnError="true"
                                    Text="*" Display="Dynamic" ValidationGroup="Required" ControlToValidate="txtMisakeDate"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount" SortExpression="Amount">
                            <ItemTemplate>
                                <asp:Label ID="lblAmount" runat="server" Text='<%#Eval("Amount") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtAmount" runat="server" Text='<%#Eval("Amount") %>' Width="70px" MaxLength="8"></asp:TextBox>
                                <asp:CompareValidator ID="CompValAmount" runat="server" ControlToValidate="txtAmount" Display="Dynamic" SetFocusOnError="true"
                                    Type="Integer" Operator="DataTypeCheck" ErrorMessage="Invalid Amount." ValidationGroup="Required"></asp:CompareValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Customer" SortExpression="CustomerName">
                            <ItemTemplate>
                                <asp:Label ID="lblCustomer" runat="server" Text='<%#Eval("CustomerName") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtCustomer" runat="server" Text='<%#Eval("CustomerName") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                                                
                        <asp:TemplateField HeaderText="Status" SortExpression="StatusName">
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("StatusName") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddStatus" runat="server" SelectedValue='<%#Bind("lStatus") %>' Width="120px">
                                    <asp:ListItem Text="Unresolved" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Resolved" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="N.A." Value="3"></asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Remarks">
                            <ItemTemplate>
                                <asp:Label ID="lblRemakrs" runat="server" Text='<%#Eval("MistakeRemarks") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtRemarks" runat="server" Text='<%#Eval("MistakeRemarks") %>' MaxLength="800" TextMode="MultiLine"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RFVRemarks" runat="server" ErrorMessage="Please Enter Remark." SetFocusOnError="true"
                                    Text="*" Display="Dynamic" ValidationGroup="Required" ControlToValidate="txtRemarks"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Logged By" DataField="LoggedByName" SortExpression="LoggedByName" ReadOnly="true"/>
                        </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </div>
        </fieldset>
            </div>
            <div>
                <asp:SqlDataSource ID="MistakeDetailSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetMistake" SelectCommandType="StoredProcedure">
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
