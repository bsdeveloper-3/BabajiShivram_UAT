<%@ Page Title="Driver Vehicle Detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="VehicleDriver.aspx.cs" 
    Inherits="Transport_VehicleDriver" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="content2" runat="server">
    <AjaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upExpense" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <fieldset><legend>Vehicle Driver Status</legend>
        <asp:UpdatePanel ID="upExpense" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <div class="clear" style="text-align:center;">
                    <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                </div>
                <div class="clear">
                </div>
                <div>
                <asp:GridView ID="GridViewVehicle" runat="server" AutoGenerateColumns="false" CssClass="table" DataKeyNames="VehicleId" 
                    AllowPaging="False" AutoGenerateEditButton="false" DataSourceID="SqlDataSourceVehicle" OnRowEditing="GridViewVehicle_RowEditing" 
                    OnRowCancelingEdit="GridViewVehicle_RowCancelingEdit" OnRowUpdating="GridViewVehicle_RowUpdating">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" CommandName="Edit" runat="server" Text="Edit" 
                                    ToolTip="Click To Update Detail"></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update Driver Detail" runat="server"
                                    Text="Update" ValidationGroup="Required"></asp:LinkButton>
                                <asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel Update" CausesValidation="false"
                                    runat="server" Text="Cancel"></asp:LinkButton>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" ReadOnly="true"/>
                        <asp:TemplateField HeaderText="Driver">
                            <ItemTemplate>
                                <asp:Label ID="lblDriverName" runat="server" Text='<%#Bind("DriverName") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%--<asp:RequiredFieldValidator ID="RFVDriver" runat="server" Text="*" ControlToValidate="ddDriver" SetFocusOnError="true"
                                    InitialValue="0" ErrorMessage="Please Select Driver Name" ValidationGroup="Required" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                                <asp:DropDownList ID="ddDriver" runat="server" SelectedValue='<%#Bind("DriverID") %>'
                                 DataSourceID="SqlDataSourceDriver" AppendDataBoundItems="true" DataTextField="UserName" DataValueField="UserId">
                                    <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Remark">
                            <ItemTemplate>
                                <asp:Label ID="txtRemark" Text='<%#BIND("Remark") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtRemark" Text='<%#BIND("Remark") %>' TextMode="MultiLine" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                </div>
                <div>
                    <asp:SqlDataSource ID="SqlDataSourceVehicle" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="TR_GetVehicleDriver" SelectCommandType="StoredProcedure">
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="SqlDataSourceDriver" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="GetUserByDivisionId" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:Parameter Name="DivisionID" DefaultValue="30" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
</asp:Content>

