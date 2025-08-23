<%@ Page Title="Agent Contract" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="AgentContract.aspx.cs" Inherits="Freight_AgentContract" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
    </asp:ScriptManager>

    <fieldset>
        <div align="center">
            <asp:HiddenField ID="hdnCustFilePath" runat="server" Value="" />
            <asp:Label ID="lberror" runat="server" Text="" CssClass="errorMsg" EnableViewState="false"></asp:Label>
        </div>

        <legend>Agent Contract</legend>
        <div style="text-align: center;">
            <asp:Label ID="lblError" runat="server"></asp:Label>
        </div>
        <asp:Button ID="btnAgentContract" ValidationGroup="Agent" Text="Add Agent Contract" runat="server"
            OnClick="btnAgentContractAdd_Click" />
        <br />
        <br />
        <table class="table">
            <th>Agent 
    <asp:RequiredFieldValidator ID="RFVAgent" runat="server" ErrorMessage="Please Select Agent" Text="*" SetFocusOnError="true" ControlToValidate="ddAgent"
        ValidationGroup="Agent" InitialValue="0"></asp:RequiredFieldValidator>
            </th>
            <th>Contract Note
                <asp:RequiredFieldValidator ID="RFVNote" runat="server" ErrorMessage="Please Select Document" Text="*" SetFocusOnError="true"
                    ControlToValidate="txtNotes" ValidationGroup="Agent">
                </asp:RequiredFieldValidator>
            </th>
            <th>Upload Contract 
                <asp:RequiredFieldValidator ID="RFVAgentDoc" runat="server" ErrorMessage="Please Select Document" Text="*" SetFocusOnError="true"
                    ControlToValidate="fuNotesDoc" ValidationGroup="Agent">
                </asp:RequiredFieldValidator>
            </th>
            <th>Created Date 
                <asp:RequiredFieldValidator ID="RFVCreatedDate" runat="server" ErrorMessage="Please Enter Created Date" Text="*" SetFocusOnError="true"
                    ControlToValidate="txtStartDate" ValidationGroup="Agent">
                </asp:RequiredFieldValidator>
            </th>
            <th>Valid Till Date </th>

            <tr>
                <td>
                    <asp:DropDownList ID="ddAgent" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:TextBox ID="txtNotes" runat="server" Width="300px"></asp:TextBox>
                </td>
                <td>
                    <asp:FileUpload ID="fuNotesDoc" runat="server" />
                </td>
                <td>
                    <AjaxToolkit:CalendarExtender ID="CalExtStartDate" runat="server" Enabled="True" EnableViewState="False"
                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDateStart" PopupPosition="BottomRight"
                        TargetControlID="txtStartDate"></AjaxToolkit:CalendarExtender>
                    <asp:TextBox ID="txtStartDate" runat="server" Width="80px"></asp:TextBox>
                    <asp:Image ID="imgDateStart" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif" runat="server" />
                </td>
                <td>
                    <AjaxToolkit:CalendarExtender ID="CalExtValidDate" runat="server" Enabled="True" EnableViewState="False"
                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgValidDate" PopupPosition="BottomRight"
                        TargetControlID="txtValidDate"></AjaxToolkit:CalendarExtender>
                    <asp:TextBox ID="txtValidDate" runat="server" Width="80px"></asp:TextBox>
                    <asp:Image ID="imgValidDate" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif" runat="server" />
                </td>
            </tr>
        </table>
    </fieldset>

    <fieldset>
        <legend>Agent Contract Detail</legend>
        <asp:GridView ID="gvAgentContract" runat="server" DataKeyNames="lid" Width="99%" AllowPaging="True"
            PageSize="10" AutoGenerateColumns="False" CssClass="table" DataSourceID="DataSourceAgentContract"
            OnRowCommand="gvAgentContract_RowCommand" Style="white-space: normal" AlternatingRowStyle-CssClass="alt"
            PagerStyle-CssClass="pgr" AllowSorting="true" PagerSettings-Position="TopAndBottom" OnPreRender="gvAgentContract_PreRender"
            AutoGenerateEditButton="true" OnRowUpdating="gvAgentContract_RowUpdating">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex +1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="AgentName" HeaderText="Agent Name" SortExpression="AgentName" ReadOnly="true" />
                <asp:TemplateField HeaderText="Notes" SortExpression="Notes">
                    <ItemTemplate>
                        <asp:Label id="lblNotes" runat="server" Text='<%#Eval("Notes") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox id="txtNotes" runat="server" TextMode="MultiLine" Text='<%#Bind("Notes") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <%--<asp:BoundField DataField="Notes" HeaderText="Contract Note" SortExpression="Notes" />--%>
                <asp:TemplateField HeaderText="Created Date" SortExpression="StartDate">
                    <ItemTemplate>
                        <asp:Label id="lblStartDate" runat="server" Text='<%#Eval("StartDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox id="txtEdtStartDate" runat="server" Text='<%#Bind("StartDate","{0:dd/MM/yyyy}") %>' Width="80px"></asp:TextBox>
                        <AjaxToolkit:CalendarExtender ID="CalExtStartDate1" runat="server" Enabled="True" EnableViewState="False"
                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" 
                        TargetControlID="txtEdtStartDate"></AjaxToolkit:CalendarExtender>
                    </EditItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Valid Till" SortExpression="ValidTillDate">
                    <ItemTemplate>
                        <asp:Label id="lblValidTill" runat="server" Text='<%#Eval("ValidTillDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox id="txtEdtValidTill" runat="server" Text='<%#Bind("ValidTillDate","{0:dd/MM/yyyy}") %>' Width="80px"></asp:TextBox>
                        <AjaxToolkit:CalendarExtender ID="CalExtValidDate1" runat="server" Enabled="True" EnableViewState="False"
                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" 
                        TargetControlID="txtEdtValidTill"></AjaxToolkit:CalendarExtender>
                    </EditItemTemplate>
                </asp:TemplateField>

<%--                <asp:BoundField DataField="StartDate" HeaderText="Created Date" SortExpression="StartDate"
                    DataFormatString="{0:dd/MM/yyyy}" />--%>
                <%--<asp:BoundField DataField="ValidTillDate" HeaderText="Valid Till Date" SortExpression="ValidTillDate"
                    DataFormatString="{0:dd/MM/yyyy}" />--%>

                <asp:TemplateField HeaderText="Document">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkNotesDownload" Text="Download" CommandName="Download" runat="server"
                            CommandArgument='<%#Eval("FilePath") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="UserName" HeaderText="User" SortExpression="UserName" ReadOnly="true" />
            </Columns>
            <PagerTemplate>
                <asp:GridViewPager runat="server" />
            </PagerTemplate>
        </asp:GridView>

        <asp:SqlDataSource ID="DataSourceAgentContract" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="FR_GetAgentContract" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:Parameter Name="CategoryID" DefaultValue="5" />
            </SelectParameters>

        </asp:SqlDataSource>

    </fieldset>

</asp:Content>

