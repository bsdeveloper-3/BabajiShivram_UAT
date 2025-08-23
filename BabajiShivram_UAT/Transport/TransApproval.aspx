<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TransApproval.aspx.cs" 
    Inherits="Transport_TransApproval" Title="Transport Approval" Culture="en-GB" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <asp:ScriptManager runat="server" ID="ScriptManager1" />
 <script type="text/javascript">
 function CheckAll(oCheckbox) {

     
     var GridViewA = document.getElementById("<%=GridViewVehicle.ClientID %>");
     
     for(i = 1;i < GridViewA.rows.length; i++)
     {
        GridViewA.rows[i].cells[1].getElementsByTagName("INPUT")[0].checked = oCheckbox.checked;
     }
  }
  </script>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingchecklist" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
        <div align="center">
            <asp:Label ID="lblError" runat="server"></asp:Label>
        </div>
        <div class="m clear">
        <fieldset><legend>Request Received</legend>
        <div class="m clear">
            <asp:Panel ID="pnlFilter" runat="server">
                <div class="fleft">
                    <uc1:DataFilter ID="DataFilter1" runat="server" />
                </div>
            </asp:Panel>
        </div>
        <div class="clear">
        </div>
        <div>
            <asp:Button ID="btnApprove" runat="Server" Text="Approve Request" OnClick="btnApprove_Click" />
            <asp:Button ID="btnCancel" runat="Server" Text="Cancel" />
        </div>
            <asp:GridView ID="GridViewVehicle" runat="server" AutoGenerateColumns="False" CssClass="table"
                Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                DataSourceID="DataSourceVehicle" CellPadding="4" AllowPaging="True" AllowSorting="True"
                PageSize="20" >
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="All">
                        <HeaderTemplate>
                            <input id="Checkbox2" type="checkbox" onclick="CheckAll(this)" runat="server" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelect" runat="server" CausesValidation="false"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="RefNo" DataField="TRRefNo" ReadOnly="true" />
                    <asp:BoundField HeaderText="Job No" DataField="JobRefNo" ReadOnly="true" />
                    <asp:BoundField HeaderText="Request Rate" DataField="TransportAmount" ReadOnly="true" />
                    <asp:TemplateField HeaderText="Approved Rate">
                    <ItemTemplate>
                        <asp:TextBox ID="txtApprovedRate" runat="server" Text='<%#BIND("ApprovedAmount") %>' Width="70px" MaxLength="10" CausesValidation="true"></asp:TextBox>
                        <asp:CompareValidator ID="CompValRate" runat="server" ControlToValidate="txtApprovedRate" Operator="DataTypeCheck" SetFocusOnError="true" 
                            Type="Integer" Text="Invalid Rate" ErrorMessage="Invalid Transport Rate" Display="Dynamic" ValidationGroup="RateRequired"></asp:CompareValidator>
                        <asp:RequiredFieldValidator ID="RFVRate" runat="server" ControlToValidate="txtApprovedRate" SetFocusOnError="true"   
                            Text="*" ErrorMessage="Please Enter Approved Rate" Display="Dynamic" ValidationGroup="RateRequired"></asp:RequiredFieldValidator>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Pkg" DataField="Packages" ReadOnly="true" />
                    <asp:BoundField DataField="GrossWeight" HeaderText="Weight" SortExpression="GrossWeight" 
                        ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"/>
                    <asp:BoundField DataField="ContainerSize" HeaderText="Size" SortExpression="ContainerSize" />
                    <asp:BoundField DataField="DeliveryType" HeaderText="Delivery Type" SortExpression="DeliveryType" />
                    <asp:BoundField HeaderText="Vehicle No" DataField="VehicleNo" ReadOnly="true" />
                    <asp:BoundField HeaderText="Type" DataField="VehicleType" ReadOnly="true" />
                    <asp:BoundField HeaderText="Delivery Point" DataField="DeliveryTo" ReadOnly="true" />
                    <asp:BoundField HeaderText="Transporter" DataField="Transporter" ReadOnly="true" />
                    <asp:BoundField HeaderText="Dispatch Date" DataField="DispatchDate" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                    <asp:BoundField HeaderText="Requested By" DataField="UserName" ReadOnly="true" />
                    <asp:BoundField HeaderText="Request Date" DataField="RequestDate" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                </Columns>
            </asp:GridView>
        </fieldset>
        <div>
        <asp:SqlDataSource ID="DataSourceVehicle" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetTransDeliveryApproval" SelectCommandType="StoredProcedure">
        </asp:SqlDataSource>
    </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

