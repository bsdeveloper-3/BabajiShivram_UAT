<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="VehicleMaster.aspx.cs" Inherits="VehicleMaster" Title="Vehicle Type Setup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingchecklist"
            runat="server">
            <progresstemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </progresstemplate>
        </asp:UpdateProgress>
    </div>
    
        <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional"
            RenderMode="Inline">
            <contenttemplate>
                  <div align="center">
                <asp:Label ID="lblresult" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <div class="clear"></div>
        <fieldset>
        <legend>Vehicle Type Setup</legend>
            <div>          
            <asp:GridView ID="gvVehicleMaster" runat="server" CssClass="table" ShowFooter="true"
                PagerStyle-CssClass="pgr" AllowSorting="true" AutoGenerateColumns="false"
                AllowPaging="true" OnPageIndexChanging="gvVehicleMaster_PageIndexChanging" PageSize="20" 
                PagerSettings-Position="TopAndBottom" DataKeyNames="lid" OnRowCommand="gvVehicleMaster_RowCommand" 
                OnRowUpdating="gvVehicleMaster_RowUpdating"  OnRowDeleting="gvVehicleMaster_RowDeleting" 
                OnRowEditing="gvVehicleMaster_RowEditing" Width="100%"
                OnRowCancelingEdit="gvVehicleMaster_RowCancelingEdit">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="lid" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lbllid" runat="server" Text='<%#Eval("lid") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtlid" runat="server" Text='<%#Eval("lid") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtlidfooter" runat="server" Text='<%#Eval("lid") %>'></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Vehicle Name">
                        <ItemTemplate>
                            <asp:Label ID="lblVehicle_Name" runat="server" Text='<%#Eval("SName") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtVehicle_Name" runat="server" Text='<%#Eval("SName") %>' MaxLength="50"></asp:TextBox>
                            <span style="color:Red">*</span>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtVehicle_Namefooter" runat="server" Text='<%#Eval("SName") %>' MaxLength="50" TabIndex="1" ></asp:TextBox>
                            <span style="color:Red">*</span>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Vehicle Remark">
                        <ItemTemplate>
                            <asp:Label ID="lblVehicle_Remark" runat="server" Text='<%#Eval("sRemarks") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtVehicle_Remark" runat="server" Text='<%#Eval("sRemarks") %>' MaxLength="50"></asp:TextBox>
                            <span style="color:Red">*</span>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtVehicle_Remarkfooter" runat="server" Text='<%#Eval("sRemarks") %>' MaxLength="50" TabIndex="2"></asp:TextBox>
                            <span style="color:Red">*</span>
                        </FooterTemplate>
                    </asp:TemplateField>                  
                    <asp:TemplateField HeaderText="Edit/Delete">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit" runat="server" Text="Edit" Font-Underline="true"></asp:LinkButton>
                          &nbsp;  <asp:LinkButton ID="lnkDelete" CommandName="Delete" ToolTip="Delete" Width="22" OnClientClick="return confirm('Sure to delete?');" runat="server" Text="Delete" Font-Underline="true"></asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                             <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update" runat="server" Text="Update" Font-Underline="true"></asp:LinkButton>
                           &nbsp;  <asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" runat="server" Text="Cancel" Font-Underline="true"></asp:LinkButton>
                        </EditItemTemplate>
                        <FooterTemplate>
                                <asp:LinkButton ID="lnkAdd" CommandName="Insert" ToolTip="Add" runat="server" Text="Add" Font-Underline="true" TabIndex="3"></asp:LinkButton>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </div>
        </fieldset>
        </contenttemplate>
        </asp:UpdatePanel>
    
</asp:Content>
