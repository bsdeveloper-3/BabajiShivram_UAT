<%@ Page Title="EWay Bill Message" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EwaySuccess.aspx.cs" Inherits="EWayBill_EwaySuccess" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <asp:Label ID="lblMessage" runat="server" CssClass="errorMsg"></asp:Label>

    <asp:GridView ID="gvEwaySuccess" runat="server" AutoGenerateColumns="true" CssClass="table" Width="90%">
        <Columns>
            <asp:TemplateField HeaderText="Sl">
                <ItemTemplate>
                    <%#Container.DataItemIndex + 1 %>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>        

    </asp:GridView>
    <br />
    <asp:GridView ID="gvEwayError" runat="server" AutoGenerateColumns="true" CssClass="table" Width="90%">
        <Columns>
            <asp:TemplateField HeaderText="Sl">
                <ItemTemplate>
                    <%#Container.DataItemIndex + 1 %>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>        
    </asp:GridView>

    <asp:Button ID="btnExit" runat="server" Text="Exit" OnClick="btnExit_Click" />
</asp:Content>

