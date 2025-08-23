<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="LRDetailById.aspx.cs"
    Inherits="Master_LRDetailById" MaintainScrollPositionOnPostback="true" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <AjaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="updPanLRDetails" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                    <img alt="progress" src="images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="updPanLRDetails" runat="server">
        <ContentTemplate>
            <center>
                <asp:Label ID="lblError" runat="server"></asp:Label>
            </center>
            
            <fieldset>
                <legend>LR Detail</legend>
                <asp:Button ID="btnGeneratePDF" runat="server" Text="Generate pdf" OnClick="btnGeneratePDF_Click"/>
                <br />
                <br />

                <asp:FormView ID="FVJobDetail" HeaderStyle-Font-Bold="true" runat="server" DataKeyNames="lid"
                    Width="100%" DataSourceID="DataSourceDetails">
                    <%--OnDataBound="FVJobDetail_DataBound"--%>
                    <ItemTemplate>
                        <%--<div class="m clear">
                            <asp:Button ID="btnEditJob" runat="server" OnClick="btnEditJob_Click" CssClass="btn"
                                Text="Edit" />
                            <asp:Button ID="btnBackButton" runat="server" OnClick="btnBackButton_Click" UseSubmitBehavior="false"
                                Text="Back" CommandArgument="JobTracking.aspx" CausesValidation="false" />
                        </div>--%>
                         <asp:HiddenField ID="hdnlid" runat="server" Value='<%# Bind("lid") %>' />
                         <asp:HiddenField ID="hdnCompId" runat="server" Value='<%# Bind("CompId") %>' />
                         <asp:HiddenField ID="hdnCNNo" runat="server" Value='<%# Bind("CNNo") %>'/>

                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td><b>Transport By</b>
                                </td>
                                <td>
                                    <%# Eval("CompanyNm") %>                                                                               
                                </td>
                                <td><b>GSTIN/Unique Reg. No. of Person liable to pay</b>
                                </td>
                                <td>
                                    <%# Eval("LiablePayTo") %>                                                
                                </td>
                            </tr>
                            <tr>
                                <td><b>C.N.No</b>
                                </td>
                                <td>
                                    <%# Eval("CNNo") %>                                                                               
                                </td>
                                <td><b>C.N.Date</b>
                                </td>
                                <td>
                                    <%# Eval("CNDate", "{0:dd/MM/yyyy}") %>                                                
                                </td>
                            </tr>
                            <tr>
                                <td><b>Invoic eNo</b>
                                </td>
                                <td>
                                    <%# Eval("InvoiceNo") %>                                                                               
                                </td>
                                <td><b>Invoice Date</b>
                                </td>
                                <td>
                                    <%# Eval("InvoiceDate", "{0:dd/MM/yyyy}") %>                                                  
                                </td>
                            </tr>
                            <tr>
                                <td><b>From</b>
                                </td>
                                <td>
                                    <%# Eval("LRFrom") %>                                                                               
                                </td>
                                <td><b>To</b>
                                </td>
                                <td>
                                    <%# Eval("LRTo") %>                                                
                                </td>
                            </tr>
                            <tr>
                                <td><b>Consignor's Name & Address</b>
                                </td>
                                <td colspan="3">
                                    <%# Eval("ConsignorNm") %>                                                                               
                                </td>
                            </tr>
                            <tr>
                                <td><b>Address of Delivery Office</b>
                                </td>
                                <td colspan="3">
                                    <%# Eval("DeliveryAddr") %>                                                                               
                                </td>
                            </tr>

                            <tr>
                                <td><b>State</b>
                                </td>
                                <td>
                                    <%# Eval("State") %>                                                                               
                                </td>
                                <td><b>Tel. No.</b>
                                </td>
                                <td>
                                    <%# Eval("TelNo") %>                                                
                                </td>
                            </tr>
                            <tr>
                                <td><b>Vehicle No.</b>
                                </td>
                                <td>
                                    <%# Eval("VehicleNo") %>                                                                               
                                </td>
                                <td><b>Our Job No.</b>
                                </td>
                                <td>
                                    <%# Eval("OurJobNo") %>                                                
                                </td>
                            </tr>
                            <tr>
                                <td><b>Way Bill No.</b>
                                </td>
                                <td>
                                    <%# Eval("WayBillNo") %>                                                                               
                                </td>
                                <td><b>Vehicle Type</b>
                                </td>
                                <td>
                                    <%# Eval("VehicleType") %>                                                
                                </td>
                            </tr>
                            <tr>
                                <td><b>B/E No.</b>
                                </td>
                                <td>
                                    <%# Eval("BENo") %>                                                                               
                                </td>
                                <td><b>B/L No.</b>
                                </td>
                                <td>
                                    <%# Eval("BLNo") %>                                                
                                </td>
                            </tr>
                            <tr>
                                <td><b>Consignee Name & Address</b>
                                </td>
                                <td colspan="3">
                                    <%# Eval("ConsigneeNm") %>                                                                               
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:FormView>
            </fieldset>
            <div id="divDataSource">
                <asp:SqlDataSource ID="DataSourceDetails" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetLRDetailsById" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="lid" SessionField="lid" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>

            <fieldset>
                <%--<legend>Billing Activity</legend>--%>
                <asp:GridView ID="gvLROtherInfo" runat="server" AutoGenerateColumns="False" CssClass="table"
                    AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lid"
                    Width="100%" PageSize="20" PagerSettings-Position="TopAndBottom" AllowPaging="false"
                    AllowSorting="true" DataSourceID="SqlDataSourceOtherInfo"
                    OnPreRender="gvOtherInfo_PreRender">
                    <%--OnRowDataBound="gvBillingStatus_RowDataBound"--%>
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%# Container.DataItemIndex +1%>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <%--<asp:BoundField DataField="dtDate" HeaderText="Date & Time" SortExpression="dtDate"
                            DataFormatString="{0:dd/MM/yyyy hh:mm tt}" ReadOnly="true" />--%>

                        <asp:TemplateField HeaderText="Packages">
                            <ItemTemplate>
                                <asp:Label ID="lblPackages" runat="server" Text='<%#Eval("Packages") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("Description") %>'></asp:Label>
                                <%--<asp:HyperLink ID="lnkBillProgress" NavigateUrl="#" Text="...More" runat="server"></asp:HyperLink>--%>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Actual Weight">
                            <ItemTemplate>
                                <asp:Label ID="lblActualWt" runat="server" Text='<%#Eval("ActualWt") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Charged">
                            <ItemTemplate>
                                <asp:Label ID="lblcharged" runat="server" Text='<%#Eval("Charged") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField DataField="UserName" HeaderText="User" SortExpression="UserName" ReadOnly="true" />--%>
                    </Columns>
                </asp:GridView>
            </fieldset>
             <div id="divDataSource1">
                <asp:SqlDataSource ID="SqlDataSourceOtherInfo" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetLRPackageDetailsById" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="lid" SessionField="lid" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>

