<%@ Page Title="Job Detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MiscJobDetail.aspx.cs" Inherits="Service_MiscJobDetail" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="GVPager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .Tab .ajax__tab_header {
            white-space: nowrap !important;
        }
    </style>
    <AjaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <%--<asp:ScriptManager runat="server" ID="ScriptManager1" />--%>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upJobDetail" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                    <img alt="progress" src="images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
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
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                 <asp:ValidationSummary ID="VSJobCancel" runat="server" ShowMessageBox="True"
                     ShowSummary="False" ValidationGroup="RequiredJobCancel" CssClass="errorMsg"
                     EnableViewState="false" />
                <asp:HiddenField ID="hdnJobRefNo" runat="server" />
                <asp:HiddenField ID="hdnPreAlertId" runat="server" />
                <asp:HiddenField ID="hdnCustId" runat="server" />
                <asp:HiddenField ID="hdnMode" runat="server" />
                <asp:HiddenField ID="hdnModuleID" runat="server" />
                <asp:HiddenField ID="hdnUploadPath" runat="server" />
            </div>
            <div class="clear"></div>
            <AjaxToolkit:TabContainer runat="server" ID="Tabs" ActiveTabIndex="0" CssClass="Tab"
                Width="100%" OnClientActiveTabChanged="ActiveTabChanged12" AutoPostBack="false">
                <AjaxToolkit:TabPanel runat="server" ID="TabPanelJobDetail" HeaderText="Job Detail">
                    <ContentTemplate>
                        <%-- Job Detail --%>
                        <fieldset>
                            <legend>Job Detail</legend>
                            <asp:FormView ID="FVJobDetail" HeaderStyle-Font-Bold="true" runat="server" DataKeyNames="JobId"
                                Width="100%" OnDataBound="FVJobDetail_DataBound">
                                <ItemTemplate>
                                    <div class="m clear">
                                        <asp:Button ID="btnCancelJob" runat="server" OnClick="btnCancelJob_Click" Text="Cancel Job" ValidationGroup="RequiredJobCancel" />
                                          <%-- OnClientClick="return confirm('Sure to Cancel Job Detail? All Job related detail will be removed from system.');"--%>   
                                        <asp:Button ID="btnBackButton" runat="server" OnClick="btnBackButton_Click" Text="Back" />
                                    </div>
                                    <%--Remark for Job Cancelation--%>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                 <tr>
                                    <td>Remark
                                        <asp:RequiredFieldValidator ID="RFVRemark" runat="server" ErrorMessage="Please Enter Remark." Text="*" SetFocusOnError="true"
                                        ControlToValidate="txtRemark" Display="Dynamic" ValidationGroup="RequiredJobCancel"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemark" runat="server"  Width="550px" TextMode="MultiLine" ></asp:TextBox>
                                    </td>
                                </tr>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                         <%--<br/>--%>
                                        <tr>
                                            <td>Job No.
                                            </td>
                                            <td>
                                                <%# Eval("JobRefNo") %>
                                            </td>
                                            <td>Service
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("ModuleName") %>
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
                                            <td>Consignee GSTN
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("ConsigneeGSTIN")%>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Customer Division
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDivision" runat="server" Text='<%#Eval("DivisionName") %>'></asp:Label>
                                            </td>
                                            <td>Customer Plant
                                            </td>
                                            <td>
                                                <asp:Label ID="lblPlant" runat="server" Text='<%#Eval("PlantName") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Mode
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("Mode")%>
                                                </span>
                                            </td>
                                            <td>Babaji Branch
                                            </td>
                                            <td>
                                                <span>
                                                    <asp:Label ID="lblBabajiBranch" runat="server" Text='<%#Eval("BranchName") %>'></asp:Label>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                
                                            </td>
                                            <td>Job Created On
                                            </td>
                                            <td>
                                                <%# Eval("JobDate", "{0:dd/MM/yyyy  hh:mm tt}")%>
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td>Job Remark
                                            </td>
                                            <td colspan="3">
                                                <span>
                                                    <%# Eval("Remark")%>
                                                </span>
                                            </td>
                                        </tr>
                                        
                                    </table>
                                </ItemTemplate>
                            </asp:FormView>
                        </fieldset>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>

                <AjaxToolkit:TabPanel ID="TabPanelStatus" runat="server" HeaderText="Status">
                    <ContentTemplate>
                        <fieldset class="fieldset-AutoWidth">
                            <legend>Change Job Status</legend>
                            <div>
                                Status For
                            <asp:DropDownList ID="ddChangeStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddChangeStatus_SelectedIndexChanged">
                                <asp:ListItem Text="-Select-" Value=""></asp:ListItem>
                                <asp:ListItem Text="Cleared" Value="16"></asp:ListItem>
                                <asp:ListItem Text="File Sent to Billing" Value="11"></asp:ListItem>
                              </asp:DropDownList>

                                &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnUpdateStatusAdHoc" runat="server" OnClick="btnUpdateStatusAdHoc_Click" Text="Update Status" />
                            </div>
                        </fieldset>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
                <AjaxToolkit:TabPanel ID="TabDocument" runat="server" HeaderText="Document">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Upload Document</legend>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td>Document Type
                                        <asp:DropDownList ID="ddDocument" runat="server" CssClass="DropDownBox">
                                        </asp:DropDownList></td>
                                    <td>
                                        <asp:FileUpload ID="fuDocument" runat="server" /> &nbsp;&nbsp;
                                        <asp:Button ID="btnUpload" runat="server"
                                          OnClick="btnUpload_Click"  Text="Upload Document"  />
                                    </td>
                                    </tr>
                            </table>
                        </fieldset>
                        <%--<div class="m clear">
                        </div>--%>
                        <fieldset>
                            <legend>Download/Remove Document</legend>
                            <asp:GridView ID="GridViewDocument" runat="server" AutoGenerateColumns="False" CssClass="table"
                                Width="100%" PagerStyle-CssClass="pgr" DataKeyNames="lid" DataSourceID="DocumentSqlDataSource"
                                OnRowCommand="gvDocument_RowCommand" CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20">
                                <Columns>
                                    <asp:TemplateField HeaderText="SI">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DocumentName" HeaderText="Document" />
                                    <asp:BoundField DataField="sName" HeaderText="Uploaded By" />
                                    <asp:TemplateField HeaderText="Download">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                                                CommandArgument='<%#Bind("lid") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <%--<asp:TemplateField HeaderText="Remove">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkRemove" runat="server" Text="Remove" CommandName="Remove"
                                                CommandArgument='<%#Eval("lid") %>' OnClientClick="return confirm('Are you sure wants to Remove ?');"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                </Columns>
                            </asp:GridView>
                        </fieldset>
                        <div>
                            <asp:SqlDataSource ID="DocumentSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="MS_GetJobDocument" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="MiscJobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
                <AjaxToolkit:TabPanel ID="TabSeaContainer" runat="server" HeaderText="Container"
                    Visible="false">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Add Container</legend>
                            <asp:Label ID="lblContainerMessage" runat="server" CssClass="errorMsg"></asp:Label>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td>Container No
                                        <asp:RegularExpressionValidator ID="REVContainer" runat="server" ControlToValidate="txtContainerNo"
                                            ValidationGroup="valContainer" SetFocusOnError="true" Text="*" ErrorMessage="Please Enter 11 digit Container No."
                                            Display="Dynamic" ValidationExpression="^[a-zA-Z0-9]{11}$"></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="RFVContainer" runat="server" ControlToValidate="txtContainerNo"
                                            ValidationGroup="valContainer" SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Container No"
                                            Display="Dynamic">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtContainerNo" runat="server" MaxLength="11"></asp:TextBox>
                                    </td>
                                    <td>Container Type
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddContainerType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddContainerType_SelectedIndexChanged">
                                            <asp:ListItem Text="FCL" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="LCL" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Container Size
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddContainerSize" runat="server">
                                            <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="20" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="40" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="45" Value="3"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td></td>
                                    <td>
                                        <asp:Button ID="btnAddContainer" Text="Add Container" 
                                            ValidationGroup="valContainer" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <fieldset>
                            <legend>Container Detail</legend>
                            <div>
                                <asp:GridView ID="gvContainer" runat="server" AllowPaging="true" CssClass="table"
                                    PagerStyle-CssClass="pgr" AutoGenerateColumns="false" DataKeyNames="lid" Width="100%"
                                    PageSize="40" 
                                    AllowSorting="true">
                                    <Columns>
                                        <asp:TemplateField HeaderText="SI">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex +1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Container No" SortExpression="ContainerNo">
                                            <ItemTemplate>
                                                <asp:Label ID="lblContainerNo" runat="server" Text='<%#Eval("ContainerNo") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtEditContainerNo" runat="server" Text='<%#Eval("ContainerNo") %>'
                                                    MaxLength="11" Width="100px"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="REVGridContainer" runat="server" ControlToValidate="txtEditContainerNo"
                                                    ValidationGroup="valGridContainer" SetFocusOnError="true" ErrorMessage="Enter 11 Digit Container No."
                                                    Display="Dynamic" ValidationExpression="^[a-zA-Z0-9]{11}$"></asp:RegularExpressionValidator>
                                                <asp:RequiredFieldValidator ID="RFVGridContainer" runat="server" ControlToValidate="txtEditContainerNo"
                                                    ValidationGroup="valGridContainer" SetFocusOnError="true" ErrorMessage="*" Display="Dynamic">
                                                </asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblType" runat="server" Text='<%#Eval("ContainerTypeName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddEditContainerType" runat="server" SelectedValue='<%#Eval("ContainerType") %>'
                                                    Width="80px">
                                                    <asp:ListItem Text="FCL" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="LCL" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Size">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSize" runat="server" Text='<%#Eval("ContainerSizeName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddEditContainerSize" runat="server" SelectedValue='<%#Eval("ContainerSize") %>'
                                                    Width="80px">
                                                    <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="20" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="40" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="45" Value="3"></asp:ListItem>
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="User">
                                            <ItemTemplate>
                                                <asp:Label ID="lblContrUser" runat="server" Text='<%#Eval("UserName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblContrDate" runat="server" Text='<%#Eval("updDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:CommandField ButtonType="Link" ShowEditButton="true" ShowDeleteButton="true"
                                            ValidationGroup="valGridContainer" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                        <div>
                            <asp:SqlDataSource ID="DataSourceContainer" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="GetContainerDetail" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
                <!-- Tab Invoice -->
                
                <!-- Job Activity -->
                <AjaxToolkit:TabPanel runat="server" ID="TabPanelDailyActivity" HeaderText="Activity">
                    <ContentTemplate>
                        <div>
                        </div>
                        <fieldset>
                            <legend>Add Job Activity</legend>
                            <div class="m clear">
                                <asp:Button ID="btnSaveDailyActivity" runat="server" ValidationGroup="RequiredActivity" OnClick="btnSaveDailyActivity_Click"
                                    Text="Save" />
                                <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="false" OnClick="btnCancel_OnClick" TabIndex="6" Visible="false" />
                                <asp:HiddenField ID="hdnActivityID" runat="server" Value="0" />
                            </div>
                            <table border="0" cellpadding="0" cellspacing="0" width="90%" bgcolor="white">
                                <tr>
                                    <td>Current Status
                                         <asp:RequiredFieldValidator ID="RFVStatus" runat="server" ErrorMessage="Please Select Current Status." Text="*" SetFocusOnError="true"
                                             ControlToValidate="ddActivityStatus" Display="Dynamic" InitialValue="0" ValidationGroup="RequiredActivity"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddActivityStatus" runat="server" DataSourceID="SqlDataSourceStatusMS"
                                            DataTextField="StatusName" DataValueField="lid" AppendDataBoundItems="true">
                                            <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Daily Progress
                                        <asp:RequiredFieldValidator ID="RFVProgreee" runat="server" ErrorMessage="Please Enter Progress Detail." Text="*" SetFocusOnError="true"
                                            ControlToValidate="txtProgressDetail" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtProgressDetail" runat="server" TextMode="MultiLine" MaxLength="4000" Rows="5" TabIndex="3" Width="60%"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                             </fieldset>
                        <fieldset>
                             <legend>Job Activity</legend>
                            <asp:GridView ID="gvDailyJob" runat="server" AutoGenerateColumns="False" CssClass="table"
                                AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lid"
                                Width="99%" PageSize="20" AllowPaging="true" PagerSettings-Position="TopAndBottom"
                                AllowSorting="true" OnRowCommand="gvDailyJob_RowCommand" DataSourceID="sqlmiscJobdailyActivity"
                                OnPreRender="gvDailyJob_PreRender" OnRowDataBound="gvDailyJob_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex +1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="dtDate" HeaderText="Date & Time" SortExpression="dtDate"
                                        DataFormatString="{0:dd/MM/yyyy hh:mm tt}" ReadOnly="true" />
                                    <%--<asp:BoundField DataField="StatusName" HeaderText="Current Status" SortExpression="StatusName"
                                        ReadOnly="true" />--%>
                                    <asp:TemplateField HeaderText="Current Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatusName" runat="server" Text='<%#Eval("StatusName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddStatusId" runat="server" DataSourceID="SqlDataSourceStatusMS"
                                                DataTextField="StatusName" DataValueField="lid" SelectedValue='<%#Eval("StatusId")%>' AppendDataBoundItems="true">
                                                <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Progress Detail">
                                        <ItemTemplate>
                                        <div style="word-wrap: break-word; width: 220px; white-space:normal;">
                                            <asp:Label ID="lblProgress" runat="server" Text='<%#Eval("DailyProgress") %>'></asp:Label>
                                        </div>
                                            <asp:HyperLink ID="lnkMoreProgress" NavigateUrl="#" Text="...More" runat="server" Visible="false"></asp:HyperLink>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtProgressDetail" runat="server" Text='<%#Eval("DailyProgress") %>'
                                                TextMode="MultiLine"></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                  <%--  <asp:TemplateField HeaderText="Visible To Customer">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomerVisible" Text='<%#(Boolean.Parse(Eval("IsCustomerVisible").ToString())? "Yes" : "No") %>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:RadioButtonList ID="RDLCustomer" runat="server" RepeatDirection="Horizontal"
                                                SelectedValue='<%#Eval("IsCustomerVisible") %>'>
                                                <asp:ListItem Text="Yes" Value="True" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="False"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Document" Visible="false">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkActivtDownload" runat="server" Text="Download" CommandArgument='<%#Eval("DocumentPath") %>'
                                                CommandName="download"></asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName"
                                        ReadOnly="true" />
                                    <asp:TemplateField HeaderText="Remove">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkActEdit" runat="server" Text="Edit" CommandName="Edit" Visible="false" ></asp:LinkButton> <%----%>
                                            <asp:LinkButton ID="lnkDelete" Text="Remove" CommandName="ActivityDelete" CommandArgument='<%#Eval("lId") %>' OnClientClick="return confirm('Sure to delete?');"
                                                runat="server"></asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="lnkActUpdate" runat="server" Text="Update" CommandName="UpdateAA"></asp:LinkButton>
                                            &nbsp;&nbsp;<asp:LinkButton ID="lnkActCancel" runat="server" Text="Cancel" CommandName="Cancel"></asp:LinkButton>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerTemplate>
                                    <GVPager:GridViewPager runat="server" />
                                </PagerTemplate>
                            </asp:GridView>
                        </fieldset>
                        <asp:SqlDataSource ID="sqlmiscJobdailyActivity" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="MS_GetJobActivityDetailById" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="MiscJobId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="SqlDataSourceStatusMS" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="MS_GetJobStatusMS" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="ModuleID" SessionField="ModuleID" />
                                 
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
                
                <AjaxToolkit:TabPanel runat="server" ID="TabPanelBilling" HeaderText="Billing">
                    <ContentTemplate>
                         <div>
                        </div>
                        <fieldset>
                <legend>Billing Instruction</legend>
                <%--<div id="dvResult" runat="server" style="max-height: 550px; overflow: auto; text-align: center;">--%>
                <br />
                <%--<div align="center">--%>
                <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" align="center" style="text-align: left;">
                    <tr>
                        <td><b>Allied Service</b></td>
                        <td>
                            <div style="word-wrap: break-word; width: 200px; white-space: normal;">
                                <asp:Label ID="lblAlliedAgencyService" runat="server" Text='<%# Bind("AlliedAgencyService") %>'></asp:Label>
                            </div>
                        </td>
                        <td><b>Allied Remark</b></td>
                        <td>
                            <div style="word-wrap: break-word; width: 200px; white-space: normal;">
                                <asp:Label ID="lblAlliedAgencyRemark" runat="server" Text='<%# Bind("AlliedAgencyRemark") %>'></asp:Label>
                            </div>
                        </td>
                        <td>
                            <b>Read By & Date</b>
                        </td>
                        <td>
                            <asp:Label ID="lblAlliedReadBy" runat="server"></asp:Label>
                        </td>
                        <td><b>Charge By & Date</b></td>
                        <td>
                            <asp:Label ID="lblAlliedChargeBy" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Other Service</b></td>
                        <td>
                            <div style="word-wrap: break-word; width: 200px; white-space: normal;">
                                <asp:Label ID="lblOtherService" runat="server"></asp:Label>
                            </div>
                        </td>
                        <td><b>Other Service remark</b></td>
                        <td>
                            <div style="word-wrap: break-word; width: 200px; white-space: normal;">
                                <asp:Label ID="lblOtherServiceRemark" runat="server"></asp:Label>
                            </div>
                        </td>
                        <td>
                            <b>Read By & Date</b>
                        </td>
                        <td>
                            <asp:Label ID="lblOtherServiceReadBy" runat="server"></asp:Label>
                        </td>
                        <td><b>Charge By & Date</b></td>
                        <td>
                            <asp:Label ID="lblOtherServiceChargeBy" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Other Instruction</b></td>
                        <td>
                            <div style="word-wrap: break-word; width: 200px; white-space: normal;">
                                <asp:Label ID="lblInstruction" runat="server"></asp:Label>
                            </div>
                        </td>
                        <td><b>Instruction Copy</b></td>
                        <td>
                            <div style="word-wrap: break-word; width: 200px; white-space: normal;">
                                <asp:LinkButton ID="lnkInstructionCopy" runat="server" OnClick="lnkInstructionCopy_Click"></asp:LinkButton>
                                <asp:HiddenField ID="hdnInstructionCopy" runat="server" />
                            </div>
                        </td>
                        <td>
                            <b>Read By & Date</b>
                        </td>
                        <td>
                            <asp:Label ID="lblReadBy1" runat="server"></asp:Label>
                        </td>
                        <td><b>Charge By & Date</b></td>
                        <td>
                            <asp:Label ID="lblChargeBy1" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Other Instruction</b></td>
                        <td>
                            <div style="word-wrap: break-word; width: 200px; white-space: normal;">
                                <asp:Label ID="lblInstruction1" runat="server"></asp:Label>
                        </td>

                        <td><b>Instruction Copy</b></td>
                        <td>
                            <div style="word-wrap: break-word; width: 200px; white-space: normal;">
                                <asp:LinkButton ID="lnkInstructionCopy1" runat="server" OnClick="lnkInstructionCopy1_Click"></asp:LinkButton>
                                <asp:HiddenField ID="hdnInstructionCopy1" runat="server" />
                            </div>
                        </td>
                        <td>
                            <b>Read By & Date</b>
                        </td>
                        <td>
                            <asp:Label ID="lblReadBy2" runat="server"></asp:Label>
                        </td>
                        <td><b>Charge By & Date</b></td>
                        <td>
                            <asp:Label ID="lblChargeBy2" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Other Instruction</b></td>
                        <td>
                            <div style="word-wrap: break-word; width: 200px; white-space: normal;">
                                <asp:Label ID="lblInstruction2" runat="server"></asp:Label>
                            </div>
                        </td>
                        <td><b>Instruction Copy</b></td>
                        <td>
                            <div style="word-wrap: break-word; width: 200px; white-space: normal;">
                                <asp:LinkButton ID="lnkInstructionCopy2" runat="server" OnClick="lnkInstructionCopy2_Click"></asp:LinkButton>
                                <asp:HiddenField ID="hdnInstructionCopy2" runat="server" />
                            </div>
                        </td>
                        <td>
                            <b>Read By & Date</b>
                        </td>
                        <td>
                            <asp:Label ID="lblReadBy3" runat="server"></asp:Label>
                        </td>
                        <td><b>Charge By & Date</b></td>
                        <td>
                            <asp:Label ID="lblChargeBy3" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Other Instruction</b></td>
                        <td>
                            <div style="word-wrap: break-word; width: 200px; white-space: normal;">
                                <asp:Label ID="lblInstruction3" runat="server"></asp:Label>
                            </div>
                        </td>
                        <td><b>Instruction Copy</b></td>
                        <td>
                            <div style="word-wrap: break-word; width: 200px; white-space: normal;">
                                <asp:LinkButton ID="lnkInstructionCopy3" runat="server" OnClick="lnkInstructionCopy3_Click"></asp:LinkButton>
                                <asp:HiddenField ID="hdnInstructionCopy3" runat="server" />
                            </div>
                        </td>
                        <td>
                            <b>Read By & Date</b>
                        </td>
                        <td>
                            <asp:Label ID="lblReadBy4" runat="server"></asp:Label>
                        </td>
                        <td><b>Charge By & Date</b></td>
                        <td>
                            <asp:Label ID="lblChargeBy4" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>User Name</b></td>
                        <td>
                            <asp:Label ID="lblUserId" runat="server"></asp:Label>
                        </td>
                        <td><b>User Date</b></td>
                        <td>
                            <asp:Label ID="lblUserDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <%--</div>--%>
                <%--</div>--%>
            </fieldset>

            <fieldset>
                <legend>Billing Scrutiny</legend>
                <asp:GridView ID="gvbillingscrutiny" runat="server" AutoGenerateColumns="False"
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
                    </Columns>
                </asp:GridView>
            </fieldset>

            <fieldset>
                <legend>Draft Invoice</legend>
                <asp:GridView ID="gvDraftInvoice" runat="server" AutoGenerateColumns="False"
                    CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceDraftinvoice"
                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
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
                    </Columns>
                </asp:GridView>
            </fieldset>

            <fieldset>
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

            <fieldset>
                <legend>Final Invoice Typing</legend>
                <asp:GridView ID="gvFinaltyping" runat="server" AutoGenerateColumns="False"
                    CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceFinalTyping"
                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
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
                    </Columns>
                </asp:GridView>
            </fieldset>

            <fieldset>
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

            <fieldset>
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

            <fieldset>
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
                        <asp:SessionParameter Name="JobId" SessionField="MiscJobId" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="DataSourceDraftinvoice" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetDraftInvoiceById" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="JobId" SessionField="MiscJobId" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="DataSourceDraftCheck" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetDraftCheckById" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="JobId" SessionField="MiscJobId" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="DataSourceFinalTyping" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetFinalTypingById" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="JobId" SessionField="MiscJobId" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="DataSourceFinalCheck" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetFinalCheckById" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="JobId" SessionField="MiscJobId" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="DataSourceBillDispatch" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetBillDispatchById" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="JobId" SessionField="MiscJobId" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="DataSourceBillRejection" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetBillRejectionById" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="JobId" SessionField="MiscJobId" />
                    </SelectParameters>
                </asp:SqlDataSource>

            </div>

                    </ContentTemplate>
                </AjaxToolkit:TabPanel>

            </AjaxToolkit:TabContainer>

            
            <div>
                <asp:LinkButton ID="lnkDocumentPupup" runat="server" Enabled="false"></asp:LinkButton>
            </div>
            
        </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Content>
