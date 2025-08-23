<%@ Page Title="Job Detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="JobDetail.aspx.cs"
    Inherits="ContMovement_JobDetail" MaintainScrollPositionOnPostback="true" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp1" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <style type="text/css">
        .Tab .ajax__tab_header {
            white-space: nowrap !important;
        }

        .hidden {
            display: none;
        }
    </style>
    <cc1:toolkitscriptmanager runat="server" id="ScriptManager1" scriptmode="Release" />

    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upJobDetail" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                    <img alt="progress" src="../images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <script type="text/javascript">

        function ActiveTabChanged12() {
            /* Clear Error Message on Tab change Event */
            document.getElementById('<%=lblError.ClientID%>').innerHTML = '';
            document.getElementById('<%=lblError.ClientID%>').className = '';
        }
    </script>

    <asp:UpdatePanel ID="upJobDetail" runat="server">
        <ContentTemplate>
            <div align="center">
                <asp:HiddenField ID="hdnConsolidateJobId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnMovementId" runat="server" Value="0" />
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="vsConsolidateRequired" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="ConsolidateRequired" CssClass="errorMsg" EnableViewState="false" />
            </div>
            <div class="clear"></div>
            <cc1:tabcontainer runat="server" id="Tabs" activetabindex="0" cssclass="Tab" width="100%"
                onclientactivetabchanged="ActiveTabChanged12" autopostback="false">
                <cc1:TabPanel runat="server" ID="TabPanelJobDetail" HeaderText="Job Detail">
                    <ContentTemplate>
                        <fieldset id="fsJobDetail" runat="server">
                            <legend>Job Detail</legend>
                            <asp:FormView ID="fvJobDetail" HeaderStyle-Font-Bold="true" runat="server" DataSourceID="SqlDataSourceJobDetail"
                                DataKeyNames="JobId" Width="100%" OnDataBound="fvJobDetail_DataBound">
                                <ItemTemplate>
                                    <div class="m clear">
                                        <asp:Button ID="btnDeleteJob" runat="server" Text="Delete Job"
                                            OnClientClick="return confirm('Sure to delete Job Detail? All Job related detail will be removed from system.');" />
                                        <asp:Button ID="btnEditJobDetail" runat="server" OnClick="btnEditJobDetail_Click" Text="Edit" />
                                        <asp:Button ID="btnCancelJobDetail" runat="server" OnClick="btnCancelJobDetail_Click" Text="Cancel" />
                                    </div>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                        <tr>
                                            <td>Job Ref No
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("JobRefNo")%>
                                                </span>
                                            </td>
                                            <td>Branch
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("Branch")%>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Customer
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("Customer")%>
                                                </span>
                                            </td>
                                            <td>Division</td>
                                            <td>
                                                <span>
                                                    <%# Eval("Division")%>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Plant</td>
                                            <td>
                                                <span>
                                                    <%# Eval("Plant")%>
                                                </span>
                                            </td>
                                            <td>Created By</td>
                                            <td>
                                                <span>
                                                    <%# Eval("JobCreatedBy")%>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Created Date</td>
                                            <td>
                                                <span>
                                                    <%# Eval("JobCreationDate", "{0:dd/MM/yyyy}")%>
                                                </span>
                                            </td>
                                            <td>Updated By</td>
                                            <td>
                                                <span>
                                                    <%# Eval("UpdatedBy")%>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Updated Date</td>
                                            <td>
                                                <span>
                                                    <%# Eval("UpdatedDate", "{0:dd/MM/yyyy}")%>
                                                </span>
                                            </td>
                                            <td>Remark</td>
                                            <td>
                                                <span>
                                                    <%# Eval("Remark")%>
                                                </span>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div class="m clear">
                                        <asp:Button ID="btnUpdateJobDetail" runat="server" ValidationGroup="ConsolidateRequired" OnClick="btnUpdateJobDetail_Click"
                                            CssClass="update" Text="Update" />
                                        <asp:Button ID="btnCancelJobDetail2" runat="server" OnClick="btnCancelJobDetail2_Click" CausesValidation="False"
                                            CssClass="cancel" Text="Cancel" />
                                    </div>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                        <tr>
                                            <td>Job Ref No
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("JobRefNo")%>
                                                </span>
                                            </td>
                                            <td>Branch</td>
                                            <td>
                                                <span>
                                                    <%# Eval("Branch")%>
                                                </span>
                                                <%--<asp:DropDownList ID="ddBranch" runat="server" AppendDataBoundItems="true" DataSourceID="SqlDataSourceBranch"
                                                    DataTextField="BranchName" DataValueField="lid">
                                                    <asp:ListItem Value="0" Selected="True" Text="-Select-"></asp:ListItem>
                                                </asp:DropDownList>--%>
                                                <asp:HiddenField ID="hdnBranchId" runat="server" Value='<%#Eval("BranchId") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Customer
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCustomerMS" runat="server" AppendDataBoundItems="true" DataSourceID="SqlDataSourceCustomer" AutoPostBack="true"
                                                    DataTextField="CustName" DataValueField="lid" OnSelectedIndexChanged="ddlCustomerMS_SelectedIndexChanged" Width="300px">
                                                    <asp:ListItem Value="0" Selected="True" Text="-Select-"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hdnCustId" runat="server" Value='<%#Eval("CustomerId") %>' />
                                            </td>
                                            <td>Division</td>
                                            <td>
                                                <asp:DropDownList ID="ddDivision" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddDivision_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hdnDivisionId" runat="server" Value='<%#Eval("DivisionId") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Plant</td>
                                            <td>
                                                <asp:DropDownList ID="ddPlant" runat="server"></asp:DropDownList>
                                                <asp:HiddenField ID="hdnPlantId" runat="server" Value='<%#Eval("PlantId") %>' />
                                            </td>
                                            <td>Created By</td>
                                            <td>
                                                <span>
                                                    <%# Eval("JobCreatedBy")%>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Created Date</td>
                                            <td>
                                                <span>
                                                    <%# Eval("JobCreationDate", "{0:dd/MM/yyyy}")%>
                                                </span>
                                            </td>
                                            <td>Updated By</td>
                                            <td>
                                                <span>
                                                    <%# Eval("UpdatedBy")%>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Updated Date</td>
                                            <td>
                                                <span>
                                                    <%# Eval("UpdatedDate", "{0:dd/MM/yyyy}")%>
                                                </span>
                                            </td>
                                            <td>Remark</td>
                                            <td>
                                                <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Rows="3" Width="290px" Text='<%#Eval("Remark") %>'></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </EditItemTemplate>
                            </asp:FormView>
                        </fieldset>
                        <fieldset>
                            <legend>Back Office Documents</legend>
                            <table border="0" cellpadding="0" cellspacing="0" width="60%" bgcolor="white">
                                <tr>
                                    <td>Type&nbsp;&nbsp; &nbsp;&nbsp;<asp:DropDownList ID="ddlBackOfficeDocType" runat="server"
                                        DataSourceID="DataSourceBODocType" DataTextField="sName" DataValueField="lid">
                                    </asp:DropDownList>
                                    </td>
                                    <td>Browse File&nbsp;&nbsp; &nbsp;&nbsp;<asp:FileUpload ID="fuBackOfficeDoc" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnUploadBackOfficeDoc" runat="server" Text="Upload" OnClick="btnUploadBackOfficeDoc_Click" />
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <asp:GridView ID="gvBackOfficeDocument" runat="server" AutoGenerateColumns="False" CssClass="table"
                                PagerStyle-CssClass="pgr" OnPreRender="gvDocuments_PreRender" OnRowCommand="gvBackOfficeDocument_RowCommand"
                                DataKeyNames="lid" DataSourceID="SqlDataSourceJobForBackOffceDocument" AllowPaging="True" AllowSorting="True"
                                PagerSettings-Position="TopAndBottom" PageSize="20" Width="100%">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Document Name">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnDocument" CommandName="download" runat="server" Text='<%#Eval("DocPath") %>'
                                                CommandArgument='<%#Eval("lid") + ";" + Eval("DocPath")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DocTypeName" HeaderText="Document Type" SortExpression="DocTypeName" ReadOnly="true" />
                                    <asp:BoundField DataField="UpdatedBy" HeaderText="Updated By" SortExpression="UpdatedBy" ReadOnly="true" />
                                    <asp:BoundField DataField="UpdatedDate" HeaderText="Updated Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="UpdatedDate" ReadOnly="true" />
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnDeleteDoc" CommandName="Remove" runat="server" Text="Remove"
                                                CommandArgument='<%#Eval("lid")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSourceJobForBackOffceDocument" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="CM_GetBackOfficeDocuments" SelectCommandType="StoredProcedure" DataSourceMode="DataSet" EnableCaching="true"
                                CacheDuration="300" CacheKeyDependency="MyCacheDependency">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:SqlDataSource ID="DataSourceBODocType" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="CM_GetBackOfficeDocType" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                        </fieldset>
                        <div>
                            <asp:SqlDataSource ID="SqlDataSourceCustomer" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="GetCustomerMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                            <asp:SqlDataSource ID="SqlDataSourceBranch" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="GetBabajiBranch" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                            <asp:SqlDataSource ID="SqlDataSourceJobDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="CM_GetJobDetail" SelectCommandType="StoredProcedure" DataSourceMode="DataSet" EnableCaching="true"
                                CacheDuration="300" CacheKeyDependency="MyCacheDependency">
                                <SelectParameters>
                                    <asp:SessionParameter Name="lid" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="CM_GetMovementDetailByLid" SelectCommandType="StoredProcedure" DataSourceMode="DataSet" EnableCaching="true"
                                CacheDuration="300" CacheKeyDependency="MyCacheDependency">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="hdnConsolidateJobId" Name="lid" PropertyName="Value" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel runat="server" ID="TabPanelConsolidateJobs" HeaderText="Consolidate Jobs">
                    <ContentTemplate>
                        <div style="overflow: scroll;">
                            <fieldset class="fieldset-AutoWidth">
                                <legend>Consolidate Job Detail</legend>
                                <asp:GridView ID="gvConsolidateJobs" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    PagerStyle-CssClass="pgr" OnPreRender="gvConsolidateJobs_PreRender" OnRowCommand="gvConsolidateJobs_RowCommand"
                                    DataKeyNames="JobId" DataSourceID="SqlDataSourceConsolidateJobs" AllowPaging="True" AllowSorting="True"
                                    PagerSettings-Position="TopAndBottom" PageSize="20" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex +1%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnShowDocuments" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Click to view documents."
                                                    CommandName="documentdoc" CommandArgument='<%#Eval("MovementId") + ";" + Eval("BabajiJobNo")+ ";" + Eval("Customer")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkbtnEditJob" CommandName="edit" runat="server" Text="Edit" CommandArgument='<%#Eval("JobId") + ";" + Eval("MovementId")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="BabajiJobNo" HeaderText="BS Job No" SortExpression="BabajiJobNo" ReadOnly="true" />
                                        <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" ReadOnly="true" />
                                        <asp:BoundField DataField="ConsigneeName" HeaderText="Consignee" SortExpression="ConsigneeName" ReadOnly="true" />
                                        <asp:BoundField DataField="ETADate" HeaderText="ETA Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ETADate" ReadOnly="true" />
                                        <asp:BoundField DataField="LastDispatchDate" HeaderText="Dispatch Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LastDispatchDate" ReadOnly="true" />
                                        <asp:BoundField DataField="BranchName" HeaderText="Branch" SortExpression="BranchName" ReadOnly="true" />
                                        <asp:BoundField DataField="SumOf20" HeaderText="Sum Of 20" SortExpression="SumOf20" ReadOnly="true" />
                                        <asp:BoundField DataField="SumOf40" HeaderText="Sum Of 40" SortExpression="SumOf40" ReadOnly="true" />
                                        <asp:BoundField DataField="ContainerType" HeaderText="Cont Type" SortExpression="ContainerType" ReadOnly="true" />
                                        <asp:BoundField DataField="JobCreatedDate" HeaderText="Job Creation" DataFormatString="{0:dd/MM/yyyy}" SortExpression="JobCreatedDate" ReadOnly="true" />
                                        <asp:BoundField DataField="DeliveryTypeName" HeaderText="Delivery Type" SortExpression="DeliveryTypeName" ReadOnly="true" />
                                        <asp:BoundField DataField="CFSName" HeaderText="Nominated CFS" SortExpression="CFSName" ReadOnly="true" />
                                        <asp:BoundField DataField="ShippingLineDate" HeaderText="Shipping Line Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ShippingLineDate" ReadOnly="true" />
                                        <asp:BoundField DataField="ConfirmedByLineDate" HeaderText="Movement Confirmed By Line Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ConfirmedByLineDate" ReadOnly="true" />
                                        <asp:BoundField DataField="MovementCompDate" HeaderText="Movement Complete Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="MovementCompDate" ReadOnly="true" />
                                        <asp:BoundField DataField="EmptyContReturnDate" HeaderText="Empty Cont Return Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="EmptyContReturnDate" ReadOnly="true" />
                                        <asp:BoundField DataField="ContCFSReceivedDate" HeaderText="Container Received At Yard Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="MovementCompDate" ReadOnly="true" Visible="false" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>
                        </div>
                        <fieldset id="fsConsolidateJob" runat="server">
                            <legend>Update Job</legend>
                            <asp:FormView ID="fvConsolidateJob" HeaderStyle-Font-Bold="true" runat="server" DataSourceID="SqlDataSourceFVConsolidate"
                                DataKeyNames="MovementId" OnDataBound="fvConsolidateJob_DataBound" Width="100%">
                                <ItemTemplate>
                                    <div class="m clear">
                                        <asp:Button ID="btnEditConsolidateFV" runat="server" OnClick="btnEditConsolidateFV_Click" Text="Edit" />
                                        <asp:Button ID="btnCancelConsolidateFV" runat="server" OnClick="btnCancelConsolidateFV_Click" Text="Cancel" />
                                    </div>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                        <tr>
                                            <td>BS Job No
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("BabajiJobNo")%>
                                                </span>
                                            </td>
                                            <td>Customer
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("Customer")%>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Consignee</td>
                                            <td>
                                                <span>
                                                    <%# Eval("ConsigneeName")%>
                                                </span>
                                            </td>
                                            <td>ETA Date</td>
                                            <td>
                                                <span>
                                                    <%# Eval("ETADate", "{0:dd/MM/yyyy}")%>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Dispatch Date</td>
                                            <td>
                                                <span>
                                                    <%# Eval("LastDispatchDate", "{0:dd/MM/yyyy}")%>
                                                </span>
                                            </td>
                                            <td>Branch Name</td>
                                            <td>
                                                <span>
                                                    <%# Eval("BranchName")%>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Sum Of 20</td>
                                            <td>
                                                <span>
                                                    <%# Eval("SumOf20")%>
                                                </span>
                                            </td>
                                            <td>Sum Of 40</td>
                                            <td>
                                                <span>
                                                    <%# Eval("SumOf40")%>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Container Type</td>
                                            <td>
                                                <span>
                                                    <%# Eval("ContainerType")%>
                                                </span>
                                            </td>
                                            <td>Job Created On</td>
                                            <td>
                                                <span>
                                                    <%# Eval("JobCreatedDate", "{0:dd/MM/yyyy}")%>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Job Created By</td>
                                            <td>
                                                <span>
                                                    <%# Eval("JobCreatedBy")%>
                                                </span>
                                            </td>
                                            <td>CFS Name</td>
                                            <td>
                                                <span>
                                                    <%# Eval("CFSName")%>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Nominated CFS Name</td>
                                            <td>
                                                <span>
                                                    <%# Eval("NominatedCFSName")%>
                                                </span>
                                            </td>
                                            <td>Shipping Line Date</td>
                                            <td>
                                                <span>
                                                    <%# Eval("ShippingLineDate", "{0:dd/MM/yyyy}")%>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Movement Confirmed By Line Date</td>
                                            <td>
                                                <span>
                                                    <%# Eval("ConfirmedByLineDate", "{0:dd/MM/yyyy}")%>
                                                </span>
                                            </td>
                                            <td>Movement Complete Date</td>
                                            <td>
                                                <span>
                                                    <%# Eval("MovementCompDate", "{0:dd/MM/yyyy}")%>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Empty Container Return Date</td>
                                            <td>
                                                <span>
                                                    <%# Eval("EmptyContReturnDate", "{0:dd/MM/yyyy}")%>
                                                </span>
                                            </td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div class="m clear">
                                        <asp:Button ID="btnUpdateConsolidate" runat="server" ValidationGroup="ConsolidateRequired" OnClick="btnUpdateConsolidate_Click" CssClass="update"
                                            Text="Update" />
                                        <asp:Button ID="btnCancelConsolidate" runat="server" OnClick="btnCancelConsolidate_Click" CausesValidation="False"
                                            CssClass="cancel" Text="Cancel" />
                                    </div>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                        <tr>
                                            <td>BS Job No
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("BabajiJobNo")%>
                                                </span>
                                            </td>
                                            <td>Customer
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("Customer")%>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Consignee</td>
                                            <td>
                                                <span>
                                                    <%# Eval("ConsigneeName")%>
                                                </span>
                                            </td>
                                            <td>ETA Date</td>
                                            <td>
                                                <span>
                                                    <%# Eval("ETADate", "{0:dd/MM/yyyy}")%>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Dispatch Date</td>
                                            <td>
                                                <span>
                                                    <%# Eval("LastDispatchDate", "{0:dd/MM/yyyy}")%>
                                                </span>
                                            </td>
                                            <td>Branch Name</td>
                                            <td>
                                                <span>
                                                    <%# Eval("BranchName")%>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Sum Of 20</td>
                                            <td>
                                                <span>
                                                    <%# Eval("SumOf20")%>
                                                </span>
                                            </td>
                                            <td>Sum Of 40</td>
                                            <td>
                                                <span>
                                                    <%# Eval("SumOf40")%>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Container Type</td>
                                            <td>
                                                <span>
                                                    <%# Eval("ContainerType")%>
                                                </span>
                                            </td>
                                            <td>Job Created On</td>
                                            <td>
                                                <span>
                                                    <%# Eval("JobCreatedDate", "{0:dd/MM/yyyy}")%>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Job Created By</td>
                                            <td>
                                                <span>
                                                    <%# Eval("JobCreatedBy")%>
                                                </span>
                                            </td>
                                            <td>CFS Name</td>
                                            <td>
                                                <span>
                                                    <%# Eval("CFSName")%>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Nominated CFS Name</td>
                                            <td>
                                                <asp:HiddenField ID="hdnBranchId" runat="server" Value='<%#Eval("BabajiBranchId") %>' />
                                                <asp:DropDownList ID="ddlCFSName" runat="server">
                                                    <asp:ListItem Text="--Select--" Value="0">
                                                    </asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hdnNominatedCFSId" runat="server" Value='<%#Eval("NominatedCFSId") %>' />
                                                <asp:HiddenField ID="hdnJobId" runat="server" Value='<%#Eval("JobId") %>' />
                                            </td>
                                            <td>Shipping Line Date
                                                <cc1:CalendarExtender ID="calShippingLineDate" runat="server" Enabled="True" EnableViewState="False"
                                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgShippingLineDate"
                                                    PopupPosition="BottomRight" TargetControlID="txtShippingLineDate">
                                                </cc1:CalendarExtender>
                                                <cc1:MaskedEditExtender ID="meeShippingLineDate" TargetControlID="txtShippingLineDate" Mask="99/99/9999"
                                                    MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                                                </cc1:MaskedEditExtender>
                                                <cc1:MaskedEditValidator ID="mevShippingLineDate" ControlExtender="meeShippingLineDate" ControlToValidate="txtShippingLineDate" IsValidEmpty="false"
                                                    EmptyValueMessage="Enter Shipping Line Date" EmptyValueBlurredText="*" InvalidValueMessage="Shipping Line Date is invalid" SetFocusOnError="true"
                                                    MinimumValueMessage="Invalid Shipping Line Date" MaximumValueMessage="Invalid Shipping Line Date" InvalidValueBlurredMessage="Invalid"
                                                    MinimumValue="01/01/2014" MaximumValue="31/12/2021" runat="Server" ValidationGroup="ConsolidateRequired"></cc1:MaskedEditValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtShippingLineDate" runat="server" Width="100px" MaxLength="10" TabIndex="2" placeholder="dd/mm/yyyy" Text='<%#Eval("ShippingLineDate","{0:dd/MM/yyyy}") %>'></asp:TextBox>
                                                <asp:Image ID="imgShippingLineDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Movement Confirmed By Line Date
                                                <cc1:CalendarExtender ID="calConfirmedByLineDate" runat="server" Enabled="True" EnableViewState="False"
                                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgConfirmedByLineDate"
                                                    PopupPosition="BottomRight" TargetControlID="txtConfirmedByLineDate">
                                                </cc1:CalendarExtender>
                                                <cc1:MaskedEditExtender ID="meeConfirmedByLineDate" TargetControlID="txtConfirmedByLineDate" Mask="99/99/9999"
                                                    MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                                                </cc1:MaskedEditExtender>
                                                <cc1:MaskedEditValidator ID="mevConfirmedByLineDate" ControlExtender="meeConfirmedByLineDate" ControlToValidate="txtConfirmedByLineDate" IsValidEmpty="false"
                                                    EmptyValueMessage="Enter Movement Confirmed By Line Date" EmptyValueBlurredText="*" InvalidValueMessage="Movement Confirmed By Line Date is invalid" SetFocusOnError="true"
                                                    MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid"
                                                    MinimumValue="01/01/2014" MaximumValue="31/12/2021" runat="Server" ValidationGroup="ConsolidateRequired"></cc1:MaskedEditValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtConfirmedByLineDate" runat="server" Width="100px" MaxLength="10" TabIndex="3" placeholder="dd/mm/yyyy"
                                                    Text='<%#Eval("ConfirmedByLineDate","{0:dd/MM/yyyy}") %>'></asp:TextBox>
                                                <asp:Image ID="imgConfirmedByLineDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                                <asp:CompareValidator ID="cvConfirmedByLineDate" runat="server" ControlToValidate="txtConfirmedByLineDate" ControlToCompare="txtShippingLineDate"
                                                    Display="Dynamic" ErrorMessage="Movement Confirmed By Line Date should be greater than Shipping Line Date." Text="*" Type="Date"
                                                    Operator="GreaterThanEqual" SetFocusOnError="true" ValidationGroup="ConsolidateRequired"></asp:CompareValidator>
                                            </td>
                                            <td>Movement Complete Date
                                                <cc1:CalendarExtender ID="calMovementComplete" runat="server" Enabled="True" EnableViewState="False"
                                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgCompleteDate"
                                                    PopupPosition="BottomRight" TargetControlID="txtCompleteDate">
                                                </cc1:CalendarExtender>
                                                <cc1:MaskedEditExtender ID="meeCompleteDate" TargetControlID="txtCompleteDate" Mask="99/99/9999"
                                                    MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                                                </cc1:MaskedEditExtender>
                                                <cc1:MaskedEditValidator ID="mevCompleteDate" ControlExtender="meeCompleteDate" ControlToValidate="txtCompleteDate" IsValidEmpty="false"
                                                    EmptyValueMessage="Enter Movement Complete Date" EmptyValueBlurredText="*" InvalidValueMessage="Movement Complete Date is invalid" SetFocusOnError="true"
                                                    MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid"
                                                    MinimumValue="01/01/2014" MaximumValue="31/12/2021" runat="Server" ValidationGroup="ConsolidateRequired"></cc1:MaskedEditValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCompleteDate" runat="server" Width="100px" MaxLength="10" TabIndex="4" placeholder="dd/mm/yyyy"
                                                    Text='<%#Eval("MovementCompDate","{0:dd/MM/yyyy}") %>'></asp:TextBox>
                                                <asp:Image ID="imgCompleteDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                                <asp:CompareValidator ID="cvCompleteDate" runat="server" ControlToValidate="txtCompleteDate" ControlToCompare="txtConfirmedByLineDate"
                                                    Display="Dynamic" ErrorMessage="Movement Complete Date should be greater than Confirmed By Line Date." Text="*" Type="Date"
                                                    Operator="GreaterThanEqual" SetFocusOnError="true" ValidationGroup="ConsolidateRequired"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Empty Container Return Date
                                                <cc1:CalendarExtender ID="calEmptyContReturnDate" runat="server" Enabled="True" EnableViewState="False"
                                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgEmptyContReturnDate"
                                                    PopupPosition="BottomRight" TargetControlID="txtEmptyContReturnDate">
                                                </cc1:CalendarExtender>
                                                <cc1:MaskedEditExtender ID="meeEmptyContReturnDate" TargetControlID="txtEmptyContReturnDate" Mask="99/99/9999"
                                                    MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                                                </cc1:MaskedEditExtender>
                                                <cc1:MaskedEditValidator ID="mevEmptyContReturnDate" ControlExtender="meeEmptyContReturnDate" ControlToValidate="txtEmptyContReturnDate" IsValidEmpty="true"
                                                    EmptyValueMessage="Enter Empty Container Return Date" EmptyValueBlurredText="*" InvalidValueMessage="Empty Container Return Date is invalid" SetFocusOnError="true"
                                                    MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid"
                                                    MinimumValue="01/01/2014" MaximumValue="31/12/2021" runat="Server" ValidationGroup="ConsolidateRequired"></cc1:MaskedEditValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEmptyContReturnDate" runat="server" Width="100px" MaxLength="10" TabIndex="5" placeholder="dd/mm/yyyy"
                                                    Text='<%#Eval("EmptyContReturnDate","{0:dd/MM/yyyy}") %>'></asp:TextBox>
                                                <asp:Image ID="imgEmptyContReturnDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                                <asp:CompareValidator ID="cvEmptyContReturnDate" runat="server" ControlToValidate="txtEmptyContReturnDate" ControlToCompare="txtCompleteDate"
                                                    Display="Dynamic" ErrorMessage="Empty Container Return Date should be greater than Movement Complete Date." Text="*" Type="Date"
                                                    Operator="GreaterThanEqual" SetFocusOnError="true" ValidationGroup="ConsolidateRequired"></asp:CompareValidator>
                                            </td>
                                            <td>
                                                <%--<cc1:CalendarExtender ID="calContRecdAtCFSDate" runat="server" Enabled="True"
                                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgContRecdAtCFSDate" PopupPosition="BottomRight"
                                                    TargetControlID="txtContRecdAtCFSDate">
                                                </cc1:CalendarExtender>
                                                <cc1:MaskedEditExtender ID="meeContRecdAtCFSDate" TargetControlID="txtContRecdAtCFSDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                    MaskType="Date" AutoComplete="false" runat="server">
                                                </cc1:MaskedEditExtender>
                                                <cc1:MaskedEditValidator ID="mevContRecdAtCFSDate" ControlExtender="meeContRecdAtCFSDate" ControlToValidate="txtContRecdAtCFSDate" IsValidEmpty="false"
                                                    EmptyValueMessage="Please Enter Container Received at CFS Date." EmptyValueBlurredText="*" InvalidValueMessage="Cont Received at CFS Date is invalid"
                                                    InvalidValueBlurredMessage="Invalid Date" MinimumValueMessage="Invalid Cont Received at CFS Date" MaximumValueMessage="Invalid Cont Received at CFS Date"
                                                    runat="Server" SetFocusOnError="true" ValidationGroup="ConsolidateRequired"></cc1:MaskedEditValidator>--%>
                                            </td>
                                            <td>
                                                <%-- <asp:TextBox ID="txtContRecdAtCFSDate" runat="server" Width="100px" MaxLength="10" Text='<%# Bind("ContCFSReceivedDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                                <asp:Image ID="imgContRecdAtCFSDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />--%>
                                            </td>
                                        </tr>
                                    </table>
                                </EditItemTemplate>
                            </asp:FormView>
                        </fieldset>
                        <div>
                            <asp:SqlDataSource ID="SqlDataSourceConsolidateJobs" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="CM_GetConsolidateJobs" SelectCommandType="StoredProcedure" DataSourceMode="DataSet" EnableCaching="true"
                                CacheDuration="300" CacheKeyDependency="MyCacheDependency">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:SqlDataSource ID="SqlDataSourceFVConsolidate" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="CM_GetMovementDetailByLid" SelectCommandType="StoredProcedure" DataSourceMode="DataSet" EnableCaching="true"
                                CacheDuration="300" CacheKeyDependency="MyCacheDependency">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="hdnConsolidateJobId" Name="lid" PropertyName="Value" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                        <br />
                        <fieldset>
                            <legend>Movement Detail</legend>
                            <asp:GridView ID="gvHS_MovementDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                                DataKeyNames="JobId" DataSourceID="HSDataSourceMovementDetail" AllowPaging="True" AllowSorting="True"
                                PagerSettings-Position="TopAndBottom" PageSize="20" Width="100%">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="BabajiJobNo" HeaderText="BS Job No" SortExpression="BabajiJobNo" ReadOnly="true" />
                                    <asp:BoundField DataField="JobCreatedBy" HeaderText="Job Created By" SortExpression="JobCreatedBy" ReadOnly="true" />
                                    <asp:BoundField DataField="JobCreatedDate" HeaderText="Job Created On" DataFormatString="{0:dd/MM/yyyy}" SortExpression="JobCreatedDate" ReadOnly="true" />
                                    <asp:BoundField DataField="ProcessedByName" HeaderText="Processed By" SortExpression="ProcessedByName" ReadOnly="true" />
                                    <asp:BoundField DataField="ProcessedOn" HeaderText="Processed On" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ProcessedOn" ReadOnly="true" />
                                </Columns>
                            </asp:GridView>
                        </fieldset>
                        <fieldset>
                            <legend>Movement Process</legend>
                            <asp:GridView ID="gvHS_MovementProcess" runat="server" AutoGenerateColumns="False" CssClass="table"
                                DataKeyNames="JobId" DataSourceID="HSDataSourceMovementProcess" AllowPaging="True" AllowSorting="True"
                                PagerSettings-Position="TopAndBottom" PageSize="20" Width="100%">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="BabajiJobNo" HeaderText="BS Job No" SortExpression="BabajiJobNo" ReadOnly="true" />
                                    <asp:BoundField DataField="CFSName" HeaderText="CFS Name" SortExpression="CFSName" ReadOnly="true" />
                                    <asp:BoundField DataField="ShippingLineDate" HeaderText="Shipping Line Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ShippingLineDate" ReadOnly="true" />
                                    <asp:BoundField DataField="ConfirmedByLineDate" HeaderText="Movement Confirmed By Line Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ConfirmedByLineDate" ReadOnly="true" />
                                    <asp:BoundField DataField="MovementCompDate" HeaderText="Movement Complete Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="MovementCompDate" ReadOnly="true" />
                                    <asp:BoundField DataField="EmptyContReturnDate" HeaderText="Empty Cont Return Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="EmptyContReturnDate" ReadOnly="true" />
                                    <asp:BoundField DataField="MovementCompletedBy" HeaderText="Movement Completed By" SortExpression="MovementCompletedBy" ReadOnly="true" />
                                    <asp:BoundField DataField="MovementCompletedOn" HeaderText="Movement Completed On" DataFormatString="{0:dd/MM/yyyy}" SortExpression="MovementCompletedOn" ReadOnly="true" />
                                </Columns>
                            </asp:GridView>
                        </fieldset>
                        <fieldset>
                            <legend>Container Received</legend>
                            <asp:GridView ID="gvContRecdCFS" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr"
                                DataKeyNames="lid" AllowPaging="True" AllowSorting="True" CssClass="table" PageSize="20"
                                PagerSettings-Position="TopAndBottom" DataSourceID="DataSourceContReceived" EmptyDataText="Not applicable for delivery type other than loaded.">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" CommandName="Edit" runat="server" Text="Edit"
                                                ToolTip="Click To Change Noting Detail"></asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update Container Received Detail" runat="server"
                                                Text="Update" ValidationGroup="vgRequired"></asp:LinkButton>
                                            <asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel Noting Detail Update" CausesValidation="false"
                                                runat="server" Text="Cancel"></asp:LinkButton>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ContainerNo" HeaderText="Container No" SortExpression="ContainerNo" ReadOnly="true" />
                                    <asp:BoundField DataField="ContainerSize" HeaderText="Container Size" SortExpression="ContainerSize" ReadOnly="true" />
                                    <asp:BoundField DataField="ContainerType" HeaderText="Container Type" SortExpression="ContainerType" ReadOnly="true" />
                                    <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" SortExpression="VehicleNo" ReadOnly="true" />
                                    <asp:BoundField DataField="TransporterName" HeaderText="Transporter Name" SortExpression="TransporterName" ReadOnly="true" />
                                    <asp:BoundField DataField="LRNo" HeaderText="LR No" SortExpression="LRNo" ReadOnly="true" />
                                    <asp:BoundField DataField="LRDate" HeaderText="LR Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LRDate" ReadOnly="true" />
                                    <asp:BoundField DataField="BabajiChallanNo" HeaderText="Challan No" SortExpression="BabajiChallanNo" ReadOnly="true" />
                                    <asp:BoundField DataField="DispatchDate" HeaderText="Dispatch Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="DispatchDate" ReadOnly="true" />
                                    <asp:TemplateField HeaderText="Cont Received At Yard Date" SortExpression="ContCFSReceivedDate">
                                        <ItemTemplate>
                                            <asp:Label ID="lblContRecdAtCFSDate" Text='<%# Bind("ContRecdAtCFSDate","{0:dd/MM/yyyy}")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:HiddenField ID="hdnLid" runat="server" Value='<%#Eval("lid") %>' />
                                            <asp:TextBox ID="txtContRecdAtCFSDate" runat="server" Width="80px" MaxLength="10" Text='<%# Bind("ContRecdAtCFSDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                            <cc1:CalendarExtender ID="calContRecdAtCFSDate" runat="server" Enabled="True"
                                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgContRecdAtCFSDate" PopupPosition="BottomRight"
                                                TargetControlID="txtContRecdAtCFSDate">
                                            </cc1:CalendarExtender>
                                            <asp:Image ID="imgContRecdAtCFSDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                            <cc1:MaskedEditExtender ID="meeContRecdAtCFSDate" TargetControlID="txtContRecdAtCFSDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                MaskType="Date" AutoComplete="false" runat="server">
                                            </cc1:MaskedEditExtender>
                                            <cc1:MaskedEditValidator ID="mevContRecdAtCFSDate" ControlExtender="meeContRecdAtCFSDate" ControlToValidate="txtContRecdAtCFSDate" IsValidEmpty="false"
                                                EmptyValueMessage="Please Enter Container Received at Yard Date." EmptyValueBlurredText="*" InvalidValueMessage="Cont Received at Yard Date is invalid"
                                                InvalidValueBlurredMessage="Invalid Date" MinimumValueMessage="Invalid Cont Received at CFS Date" MaximumValueMessage="Invalid Cont Received at Yard Date"
                                                runat="Server" SetFocusOnError="true" ValidationGroup="vgRequired"></cc1:MaskedEditValidator>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <div>
                                <asp:SqlDataSource ID="DataSourceContReceived" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="CM_GetContainerDetailByJobId" SelectCommandType="StoredProcedure" UpdateCommand="CM_updContRecdCFSDetail"
                                    UpdateCommandType="StoredProcedure" OnUpdated="DataSourceContReceived_Updated" OnUpdating="DataSourceContReceived_Updating">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                    <UpdateParameters>
                                        <asp:Parameter Name="lid" Type="Int32" />
                                        <asp:Parameter Name="ContRecdAtCFSDate" DbType="Date" />
                                        <asp:SessionParameter Name="lUser" SessionField="UserId" />
                                        <asp:Parameter Name="OutPut" Type="Int32" Direction="Output" Size="4" />
                                    </UpdateParameters>
                                </asp:SqlDataSource>
                            </div>
                        </fieldset>
                        <fieldset>
                            <legend>Movement Un-Process</legend>
                            <asp:GridView ID="gvUnProcess" runat="server" AutoGenerateColumns="False" CssClass="table"
                                DataKeyNames="JobId" DataSourceID="HSDataSourceContRecd" AllowPaging="True" AllowSorting="True"
                                PagerSettings-Position="TopAndBottom" PageSize="20" Width="100%">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" SortExpression="JobRefNo" ReadOnly="true" />
                                    <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark" ReadOnly="true" />
                                    <asp:BoundField DataField="UpdatedBy" HeaderText="Updated By" SortExpression="UpdatedBy" ReadOnly="true" />
                                    <asp:BoundField DataField="UpdatedDate" HeaderText="Updated Date" SortExpression="UpdatedDate" ReadOnly="true" />
                                    <%--<asp:BoundField DataField="ContCFSReceivedDate" HeaderText="Container Received CFS Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ContCFSReceivedDate" ReadOnly="true" />
                                    <asp:BoundField DataField="ContRecdCFSBy" HeaderText="Updated By" SortExpression="ContRecdCFSBy" ReadOnly="true" />
                                    <asp:BoundField DataField="updContCFSRecdOn" HeaderText="Updated On" DataFormatString="{0:dd/MM/yyyy}" SortExpression="updContCFSRecdOn" ReadOnly="true" />--%>
                                </Columns>
                            </asp:GridView>
                        </fieldset>
                        <div>
                            <asp:SqlDataSource ID="HSDataSourceMovementDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="CM_GetMovementJobHistory" SelectCommandType="StoredProcedure" DataSourceMode="DataSet" EnableCaching="true"
                                CacheDuration="300" CacheKeyDependency="MyCacheDependency">
                                <SelectParameters>
                                    <asp:SessionParameter Name="lid" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:SqlDataSource ID="HSDataSourceMovementProcess" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="CM_GetMovementJobHistory" SelectCommandType="StoredProcedure" DataSourceMode="DataSet" EnableCaching="true"
                                CacheDuration="300" CacheKeyDependency="MyCacheDependency">
                                <SelectParameters>
                                    <asp:SessionParameter Name="lid" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                             <asp:SqlDataSource ID="HSDataSourceContRecd" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="CM_GetUnProcessJobHistory" SelectCommandType="StoredProcedure" DataSourceMode="DataSet" EnableCaching="true"
                                CacheDuration="300" CacheKeyDependency="MyCacheDependency">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                          <%--  <asp:SqlDataSource ID="HSDataSourceContRecd" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="CM_GetMovementJobHistory" SelectCommandType="StoredProcedure" DataSourceMode="DataSet" EnableCaching="true"
                                CacheDuration="300" CacheKeyDependency="MyCacheDependency">
                                <SelectParameters>
                                    <asp:SessionParameter Name="lid" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>--%>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="CM_GetMovementJobHistory" SelectCommandType="StoredProcedure" DataSourceMode="DataSet" EnableCaching="true"
                                CacheDuration="300" CacheKeyDependency="MyCacheDependency">
                                <SelectParameters>
                                    <asp:SessionParameter Name="lid" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel runat="server" ID="TabPanelDocument" HeaderText="Documents">
                    <ContentTemplate>
                        <div>
                            <fieldset>
                                <legend>Upload Document</legend>
                                <table border="0" cellpadding="0" cellspacing="0" width="60%" bgcolor="white">
                                    <tr>
                                        <td>Job Ref No&nbsp;&nbsp; &nbsp;&nbsp;<asp:DropDownList ID="ddlJobForDocument" runat="server"
                                            DataSourceID="DataSourceJobForDocument" DataTextField="JobRefNo" DataValueField="JobId">
                                        </asp:DropDownList>
                                        </td>
                                        <td>Browse File&nbsp;&nbsp; &nbsp;&nbsp;<asp:FileUpload ID="fuDocument" runat="server" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <fieldset>
                                <legend>All Documents</legend>
                                <div class="fleft">
                                    <uc1:DataFilter ID="DataFilter1" runat="server" />
                                </div>
                                <div class="clear">
                                </div>
                                <asp:GridView ID="gvDocuments" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    PagerStyle-CssClass="pgr" OnPreRender="gvDocuments_PreRender" OnRowCommand="gvDocuments_RowCommand"
                                    DataKeyNames="JobId" DataSourceID="SqlDataSourceJobForDocument" AllowPaging="True" AllowSorting="True"
                                    PagerSettings-Position="TopAndBottom" PageSize="20" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex +1%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" SortExpression="JobRefNo" ReadOnly="true" />
                                        <asp:TemplateField HeaderText="Document Name">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkbtnDocument" CommandName="download" runat="server" Text='<%#Eval("DocPath") %>'
                                                    CommandArgument='<%#Eval("JobId") + ";" + Eval("DocPath")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy" ReadOnly="true" />
                                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="CreatedDate" ReadOnly="true" />
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkbtnDeleteDoc" CommandName="Remove" runat="server" Text="Remove"
                                                    CommandArgument='<%#Eval("lid")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </fieldset>
                        </div>
                        <div>
                            <asp:SqlDataSource ID="SqlDataSourceJobForDocument" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="CM_GetAllDocuments" SelectCommandType="StoredProcedure" DataSourceMode="DataSet" EnableCaching="true"
                                CacheDuration="300" CacheKeyDependency="MyCacheDependency">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:SqlDataSource ID="DataSourceJobForDocument" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="CM_GetJobsForDocument" SelectCommandType="StoredProcedure" DataSourceMode="DataSet" EnableCaching="true"
                                CacheDuration="300" CacheKeyDependency="MyCacheDependency">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel runat="server" ID="TabPanelBilling" HeaderText="Billing">
                    <ContentTemplate>
                        <fieldset id="BillingScrutiny" runat="server">
                            <legend>Billing Scrutiny</legend>
                            <asp:Label ID="lblfreight" runat="server"></asp:Label>
                            <asp:GridView ID="gvBillingScrutiny" runat="server" AutoGenerateColumns="False"
                                CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceBillingScrutiny"
                                CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Status" DataField="Status" />
                                    <asp:BoundField HeaderText="Sent By Billing Advice" DataField="SentBy" />
                                    <asp:BoundField HeaderText="Billing Advice Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    <asp:BoundField HeaderText="Received By Billing Scrutiny" DataField="ReceivedBy" />
                                    <asp:BoundField HeaderText="Billing Scrutiny Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    <asp:BoundField HeaderText="Scrutiny Completed Date" DataField="ScrutinyDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    <asp:BoundField HeaderText="Scrutiny Completed By" DataField="ScrutinyCompletedBy" />
                                    <asp:BoundField HeaderText="FreightJob" DataField="FreightjobNo" />
                                </Columns>
                            </asp:GridView>
                        </fieldset>
                        <fieldset id="DraftInvoice" runat="server">
                            <legend>Draft Invoice</legend>
                            <asp:Label ID="lblConsolidated" runat="server"></asp:Label>
                            <asp:GridView ID="gvDraftInvoice" runat="server" AutoGenerateColumns="False"
                                CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceDraftinvoice"
                                CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40" OnRowCommand="gvDraftInvoice_RowCommand" OnRowDataBound="gvDraftInvoice_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Status" DataField="Status" />
                                    <asp:BoundField HeaderText="Sent By Billing Scrutiny" DataField="SentBy" />
                                    <asp:BoundField HeaderText="Billing Scrutiny Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    <asp:BoundField HeaderText="Received By Draft Invoice" DataField="ReceivedBy" />
                                    <asp:BoundField HeaderText="Draft Invoice Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    <asp:BoundField HeaderText="Draft Invoice Completed Date" DataField="DraftInvoiceDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    <asp:BoundField HeaderText="Draft Invoice Completed By" DataField="FAUserName" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    <asp:BoundField HeaderText="Consolidated Job No" DataField="ConsolidatedjobNo" />
                                    <asp:ButtonField Text="Next" ButtonType="Button" CommandName="DraftInvoiceNext" />
                                </Columns>
                            </asp:GridView>
                        </fieldset>
                        <fieldset id="DraftCheck" runat="server">
                            <legend>Draft Check</legend>
                            <asp:GridView ID="gvDraftcheck" runat="server" AutoGenerateColumns="False"
                                CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceDraftCheck"
                                CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Status" DataField="Status" />
                                    <asp:BoundField HeaderText="Sent By Draft Invoice" DataField="SentBy" />
                                    <asp:BoundField HeaderText="Draft Invoice Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    <asp:BoundField HeaderText="Received By Draft Check" DataField="ReceivedBy" />
                                    <asp:BoundField HeaderText="Draft Check Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    <asp:BoundField HeaderText="Draft Check Completed Date" DataField="DraftCheckDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                </Columns>
                            </asp:GridView>
                        </fieldset>
                        <fieldset id="FinalInvoiceTyping" runat="server">
                            <legend>Final Invoice Typing</legend>
                            <asp:GridView ID="gvFinaltyping" runat="server" AutoGenerateColumns="False"
                                CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceFinalTyping"
                                CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40" OnRowCommand="gvFinaltyping_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Status" DataField="Status" />
                                    <asp:BoundField HeaderText="Sent By Draft Check" DataField="SentBy" />
                                    <asp:BoundField HeaderText="Draft Check Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    <asp:BoundField HeaderText="Received By Final Typing" DataField="ReceivedBy" />
                                    <asp:BoundField HeaderText="Final Typing Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    <asp:BoundField HeaderText="Final Typing Completed Date" DataField="FinalTypingDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    <asp:BoundField HeaderText="Final Typing Completed by" DataField="FAUserName" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    <asp:BoundField HeaderText="Comment" DataField="Comment" />
                                    <asp:ButtonField Text="Next" ButtonType="Button" CommandName="FinalTypingNext" />
                                </Columns>
                            </asp:GridView>
                        </fieldset>
                        <fieldset id="FinalInvoiceCheck" runat="server">
                            <legend>Final Invoice Check</legend>
                            <asp:GridView ID="gvfinalcheck" runat="server" AutoGenerateColumns="False"
                                CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceFinalCheck"
                                CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Status" DataField="Status" />
                                    <asp:BoundField HeaderText="Sent By Final Typing" DataField="SentBy" />
                                    <asp:BoundField HeaderText="Final Typing Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    <asp:BoundField HeaderText="Received By Final Check" DataField="ReceivedBy" />
                                    <asp:BoundField HeaderText="Final Check Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    <asp:BoundField HeaderText="Final Check Completed Date" DataField="FinalCheckDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                </Columns>
                            </asp:GridView>
                        </fieldset>
                        <fieldset id="Billdispatch" runat="server">
                            <legend>Bill Dispatch</legend>
                            <asp:GridView ID="gvbilldispatch" runat="server" AutoGenerateColumns="False" CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceBillDispatch"
                                CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Status" DataField="Status" />
                                    <asp:BoundField HeaderText="Sent By Final Check" DataField="SentBy" />
                                    <asp:BoundField HeaderText="Final Check Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    <asp:BoundField HeaderText="Received By Bill Dispatch" DataField="ReceivedBy" />
                                    <asp:BoundField HeaderText="Bill Dispatch Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    <asp:BoundField HeaderText="Bill Dispatch Completed Date" DataField="BillDispatchDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                </Columns>
                            </asp:GridView>
                        </fieldset>
                        <fieldset id="BillRejection" runat="server">
                            <legend>Bill Rejection</legend>
                            <asp:GridView ID="gvBillrejection" runat="server" AutoGenerateColumns="False" CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceBillRejection"
                                CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Stage" DataField="Stage" />
                                    <asp:BoundField HeaderText="Rejected by" DataField="RejectedBy" />
                                    <asp:BoundField HeaderText="Bill Rejection Date" DataField="RejectedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    <asp:BoundField HeaderText="Reason" DataField="Reason" />
                                    <asp:BoundField HeaderText="Remark" DataField="Remark" />
                                    <asp:BoundField HeaderText="Followup Date" DataField="FollowupDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    <asp:BoundField HeaderText="Followup Remark" DataField="FollowupRemark" />
                                </Columns>
                            </asp:GridView>
                        </fieldset>
                        <div id="div1">
                            <asp:SqlDataSource ID="DataSourceBillingScrutiny" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="GetBillingScrutinyById" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:SqlDataSource ID="DataSourceDraftinvoice" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="GetDraftInvoiceById" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:SqlDataSource ID="DataSourceDraftCheck" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="GetDraftCheckById" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:SqlDataSource ID="DataSourceFinalTyping" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="GetFinalTypingById" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:SqlDataSource ID="DataSourceFinalCheck" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="GetFinalCheckById" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:SqlDataSource ID="DataSourceBillDispatch" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="GetBillDispatchById" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:SqlDataSource ID="DataSourceBillRejection" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="GetBillRejectionById" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </ContentTemplate>
                </cc1:TabPanel>
            </cc1:tabcontainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

