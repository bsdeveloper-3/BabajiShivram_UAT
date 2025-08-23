<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TransDashboard.aspx.cs" Inherits="Transport_TransDashboard" 
 MasterPageFile="~/MasterPage.master" Title="BSCCPL Transport" EnableEventValidation="false" Culture="en-GB"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
 <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div style="float:left; width:70%;">
        <!--Transport Month Summary -->
        <div id="divCategory">
            <fieldset><legend>Summary</legend>
            <div>
                <asp:LinkButton ID="lnkCategoryXls" runat="server" OnClick="lnkCategoryXls_Click" data-tooltip="&nbsp;&nbsp;&nbsp; Export To Excel">
                <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" />
                </asp:LinkButton>
                <asp:GridView ID="gvCategoryMonth" runat="server" CssClass="table"  
                    Width="99%" AutoGenerateColumns="true">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="DataSourceCategory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TR_DSExpenseSummaryMonth" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
            <!--Category Month Summary -->
            </div>
        <!--Vehicle Month Summary -->
        <div id="div1">
        <fieldset><legend>Summary</legend>
        <asp:LinkButton ID="lnkVehicleXls" runat="server" OnClick="lnkVehicleXls_Click" data-tooltip="&nbsp;&nbsp;&nbsp;&nbsp; Export To Excel">
            <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" />
            </asp:LinkButton>
        <div style="overflow:scroll; ">
            <asp:GridView ID="gvVehicleMonth" runat="server" CssClass="table" Width="99%" AutoGenerateColumns="true">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex +1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        </fieldset>
        <div>
            <asp:SqlDataSource ID="DataSourceVehicle" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="TR_DSVehicleSummaryMonth" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="UserId" SessionField="UserId" />
                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
        <!--Transport Month Summary -->
        </div>
    </div>
    
    <%--<div style="float:left; width:70%;">
    <fieldset>
    <legend>Transport Rate Approval Request </legend>
    <div>
    <asp:Button ID="btnSaveInvoice" runat="Server" Text="Approve Request" />
    <asp:Button ID="btnCancel" runat="Server" Text="Cancel" />
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
                <asp:TemplateField HeaderText="Check">
                <ItemTemplate>
                    <asp:CheckBox ID="chkSelect" runat="server" CausesValidation="false"/>
                </ItemTemplate>
            </asp:TemplateField>
                <asp:BoundField HeaderText="Job No" DataField="JobRefNo" ReadOnly="true" />
                <asp:BoundField HeaderText="Request Rate" DataField="TransportAmount" ReadOnly="true" />
                <asp:TemplateField HeaderText="Approved Rate">
                    <ItemTemplate>
                        <asp:TextBox ID="txtApprovedRate" runat="server" Text='<%#BIND("ApprovedAmount") %>' MaxLength="10" CausesValidation="true"></asp:TextBox>
                        <asp:CompareValidator ID="CompValRate" runat="server" ControlToValidate="txtApprovedRate" Operator="DataTypeCheck" SetFocusOnError="true" 
                            Type="Integer" Text="Invalid Rate" ErrorMessage="Invalid Transport Rate" Display="Dynamic" ValidationGroup="RateRequired"></asp:CompareValidator>
                        <asp:RequiredFieldValidator ID="RFVRate" runat="server" ControlToValidate="txtApprovedRate" SetFocusOnError="true"   
                            Text="*" ErrorMessage="Please Enter Approved Rate" Display="Dynamic" ValidationGroup="RateRequired"></asp:RequiredFieldValidator>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Packages" DataField="NoOfPackages" ReadOnly="true" />
                <asp:BoundField HeaderText="Vehicle No" DataField="VehicleNo" ReadOnly="true" />
                <asp:BoundField HeaderText="Type" DataField="VehicleType" ReadOnly="true" />
                <asp:BoundField HeaderText="Delivery Point" DataField="DeliveryPoint" ReadOnly="true" />
                <asp:BoundField HeaderText="Transporter" DataField="TransporterName" ReadOnly="true" />
                <asp:BoundField HeaderText="Dispatch Date" DataField="DispatchDate" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                <asp:BoundField HeaderText="Requested By" DataField="UserName" ReadOnly="true" />
                <asp:BoundField HeaderText="RequestDate" DataField="RequestDate" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
            </Columns>
        </asp:GridView>
    </div>
    </fieldset>
    <div>
        <asp:SqlDataSource ID="DataSourceVehicle" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetJobDeliveryDetail" SelectCommandType="StoredProcedure">
        </asp:SqlDataSource>
    </div>
    
    </div>--%>
 </asp:Content>