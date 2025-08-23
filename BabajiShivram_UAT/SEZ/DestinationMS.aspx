<%@ Page Title="Destination" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DestinationMS.aspx.cs" 
    Inherits="SEZ_DestinationMS" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

     <div align="center">
        <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
    </div>        

      <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />

    
     <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upDestination" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    
        <asp:UpdatePanel ID="upDestination" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <div align="center">
                    <asp:Label ID="lblresult" runat="server" EnableViewState="false"></asp:Label>
                </div>
                <div class="clear"></div>
                <fieldset>
            <legend>Destination</legend>
                <asp:GridView ID="gvDestination" runat="server" AllowPaging="true" OnPageIndexChanging="gvDestination_PageIndexChanging"
                    CssClass="table" PagerStyle-CssClass="pgr" Width="100%" PagerSettings-Position="TopAndBottom"
                    PageSize="20" AllowSorting="true" ShowFooter="true" AutoGenerateColumns="false"
                    DataKeyNames="Did" OnRowCommand="gvDestination_RowCommand" OnRowUpdating="gvDestination_RowUpdating"
                    OnRowDeleting="gvDestination_RowDeleting" OnRowEditing="gvDestination_RowEditing"
                    OnRowCancelingEdit="gvDestination_RowCancelingEdit">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="lid" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblDid" runat="server" Text='<%#Eval("Did") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDid" runat="server" Text='<%#Eval("Did") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtDidfooter" runat="server" Text='<%#Eval("Did") %>'></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Destination">
                            <ItemTemplate>
                                <asp:Label ID="lblDName" runat="server" Text='<%#Eval("DName") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDName" runat="server" Text='<%#Eval("DName") %>'></asp:TextBox>
                                <span style="color: Red">*</span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtDNamefooter" runat="server" Text='<%#Eval("DName") %>' TabIndex="1"></asp:TextBox>
                                <span style="color: Red">*</span>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit/Delete">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit" Width="22" runat="server"
                                    Text="Edit" Font-Underline="true"></asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="lnkDelete" CommandName="Delete" ToolTip="Delete" Width="22" runat="server"
                                    Text="Delete" Font-Underline="true" OnClientClick="return confirm('Sure to delete?');"></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update" Width="39" runat="server"
                                    Text="Update" Font-Underline="true"></asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" Width="22" runat="server"
                                    Text="Cancel" Font-Underline="true"></asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lnkAdd" CommandName="Insert" ToolTip="Add" Width="22" runat="server"
                                    Text="Add" Font-Underline="true" TabIndex="2"></asp:LinkButton>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </fieldset>             
            </ContentTemplate>
        </asp:UpdatePanel>

</asp:Content>

