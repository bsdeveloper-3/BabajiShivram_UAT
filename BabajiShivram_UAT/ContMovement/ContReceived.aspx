<%@ Page Title="Container Received" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ContReceived.aspx.cs"
    Inherits="ContMovement_ContReceived" ValidateRequest="false" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .heading {
            line-height: 20px;
        }

        .modalBackground {
            background-color: black;
            filter: alpha(opacity=90);
            opacity: 0.5;
        }

        .modalPopup {
            border-radius: 5px;
            background-color: #FFFFFF;
            border-width: 1px;
            border-style: solid;
            border-color: black;
            padding-top: 5px;
            padding-left: 3px;
            width: 300px;
            height: 140px;
        }

        .modalPopup1 {
            border-radius: 5px;
            background-color: #FFFFFF;
            border-width: 1px;
            border-style: solid;
            border-color: black;
            padding-top: 5px;
            padding-left: 3px;
            width: 600px;
            height: 300px;
        }
    </style>

    <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
    </asp:ScriptManager>

    <div>
        <script type="text/javascript">
            function GridSelectAllColumn(spanChk) {
                var oItem = spanChk.children;
                var theBox = (spanChk.type == "checkbox") ? spanChk : spanChk.children.item[0]; xState = theBox.checked;
                elm = theBox.form.elements;

                for (i = 0; i < elm.length; i++) {
                    if (elm[i].type === 'checkbox' && elm[i].checked != xState)
                        elm[i].click();
                }
            }
        </script>
    </div>

    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlContRecd" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../Images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upnlContRecd" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="vsConsolidate" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="vgConsolidate2" CssClass="errorMsg" />
            </div>
            <div class="clear">
            </div>
            <fieldset class="fieldset-AutoWidth">
                <legend>Container Received Detail</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:DataFilter ID="DataFilter1" runat="server" />
                        </div>
                        <div class="fleft" style="margin-left: 2px; padding-top: 0px;">
                            <asp:Button ID="btnConsolidateJob" runat="server" OnClick="btnConsolidateJob_Click" Text="Consolidate Jobs" />
                            <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                                <asp:Image ID="Image1" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                            </asp:LinkButton>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtcolor1" runat="server" Style="background-color: #668cbbb0; border-radius: 4px" Width="20px" Height="10px" Enabled="false"></asp:TextBox>
                            <asp:Label ID="lblColorName1" runat="server" Text="PN Movement in our scope" Font-Bold="true"></asp:Label>
                        </div>
                    </asp:Panel>
                </div>
                <div class="clear">
                </div>
                <asp:GridView ID="gvContReceived" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr"
                    DataKeyNames="lid" OnRowCommand="gvContReceived_RowCommand" AllowPaging="True" AllowSorting="True" CssClass="table" PageSize="20"
                    PagerSettings-Position="TopAndBottom" OnRowDataBound="gvContReceived_RowDataBound" DataSourceID="DataSourceContReceived"
                    OnPreRender="gvContReceived_PreRender">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="true">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkboxSelectAll" align="center" ToolTip="Check All" runat="server" onclick="GridSelectAllColumn(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelectJob" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CFS Date" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnContRecdCFSDate" CommandName="container" runat="server" Font-Bold="true"
                                    CommandArgument='<%#Eval("JobId")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Document" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnShowDocuments" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Click to view documents."
                                    CommandName="documentdoc" CommandArgument='<%#Eval("lid") + ";" + Eval("BabajiJobNo") + ";" + Eval("Customer") + ";" + Eval("NominatedCFSName") + ";" + Eval("NominatedCFSId")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="BabajiJobNo" HeaderText="BS Job No" SortExpression="JobRefNo" ReadOnly="true" />
                        <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" ReadOnly="true" />
                        <asp:BoundField DataField="ConsigneeName" HeaderText="Consignee" SortExpression="ConsigneeName" ReadOnly="true" />
                        <asp:BoundField DataField="ETADate" HeaderText="ETA Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ETADate" ReadOnly="true" />
                        <asp:BoundField DataField="LastDispatchDate" HeaderText="Dispatch Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LastDispatchDate" ReadOnly="true" />
                        <asp:BoundField DataField="DeliveryTypeName" HeaderText="Delivery Type" SortExpression="DeliveryTypeName" ReadOnly="true" />
                        <asp:BoundField DataField="BranchName" HeaderText="Branch" SortExpression="BranchName" ReadOnly="true" />
                        <asp:BoundField DataField="SumOf20" HeaderText="Sum Of 20" SortExpression="SumOf20" ReadOnly="true" />
                        <asp:BoundField DataField="SumOf40" HeaderText="Sum Of 40" SortExpression="SumOf40" ReadOnly="true" />
                        <asp:BoundField DataField="ContainerType" HeaderText="Cont Type" SortExpression="ContainerType" ReadOnly="true" />
                        <asp:BoundField DataField="JobCreationDate" HeaderText="Job Creation" DataFormatString="{0:dd/MM/yyyy}" SortExpression="JobCreationDate" ReadOnly="true" />
                        <asp:BoundField DataField="CFSName" HeaderText="CFS Name" SortExpression="CFSName" ReadOnly="true" />
                        <asp:BoundField DataField="NominatedCFSName" HeaderText="Nominated CFS Name" SortExpression="NominatedCFSName" ReadOnly="true" />
                        <asp:BoundField DataField="MovementCompDate" HeaderText="Movement Complete" DataFormatString="{0:dd/MM/yyyy}" SortExpression="MovementCompDate" ReadOnly="true" />
                        <asp:BoundField DataField="EmptyContReturnDate" HeaderText="Empty Cont Received at CFS Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="EmptyContReturnDate" ReadOnly="true" />
                        <asp:BoundField DataField="ContainerNos" HeaderText="Container No" ReadOnly="true" />
                        <%--<asp:BoundField DataField="ContCFSReceivedDate" HeaderText="Container Received at CFS Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="MovementCompDate" ReadOnly="true" Visible="false" />--%>
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </fieldset>
            <%-- Popup for Consolidate Jobs --%>
            <div>
                <asp:LinkButton ID="lnkDummy" runat="server" Text=""></asp:LinkButton>
                <cc1:ModalPopupExtender ID="mpeConsolidateJobs" runat="server" TargetControlID="lnkDummy" CancelControlID="btnCancel_Popup"
                    PopupControlID="pnlConsolidateJobs" BackgroundCssClass="modalBackground" CacheDynamicResults="false">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pnlConsolidateJobs" runat="server" CssClass="ModalPopupPanel" Style="border-radius: 5px; padding: 5px">
                    <div class="header">
                        <div class="fleft">
                            Consolidate Movement Job 
                        </div>
                        <div class="fright">
                            <asp:Button ID="btnSaveJob" runat="server" Text="Move Job" ValidationGroup="vgConsolidate" OnClientClick="return confirm('Are you sure to Move the Job To Scrutiny ?');"
                                OnClick="btnSaveJob_Click" Style="background: white; color: black" />
                            <asp:Button ID="imgEmailClose" runat="server" OnClick="btnCancel_Popup" ToolTip="Close" Text="Close" Style="background: white; color: black" />
                        </div>
                    </div>
                    <div id="DivABC" runat="server" style="height: 650px; width: 800px; overflow: auto; padding: 5px">
                        <fieldset>
                            <legend>Consolidate Jobs</legend>
                            <div style="height: 200px; overflow: auto;">
                                <asp:GridView ID="gvConsolidateJobs" runat="server" CssClass="table" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRowNumber" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Job Id" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblJobId" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Job Ref No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblJobRefNo" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomer" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Nominated CFS Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCFSName" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="btnDeleteRow" runat="server" OnClick="btnDeleteRow_Click" Text="Remove" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                        <fieldset>
                            <legend>Movement Detail</legend>
                            <div>
                                <table border="0" cellpadding="0" cellspacing="0" class="table">
                                    <tr>
                                        <td>Job Ref No
                                        </td>
                                        <td>
                                            <asp:TextBox ID="lblMovementJobNo" runat="server" Enabled="false" Width="160px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Branch <span style="color: red">*</span>
                                            <asp:RequiredFieldValidator ID="rfvBranch" runat="server" ControlToValidate="ddBranch" InitialValue="0" SetFocusOnError="true" Display="Dynamic"
                                                ForeColor="Red" Text="Required" ValidationGroup="vgConsolidate"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddBranch" runat="server" AppendDataBoundItems="true" DataSourceID="SqlDataSourceBranch"
                                                DataTextField="BranchName" DataValueField="lid" AutoPostBack="true" OnSelectedIndexChanged="ddBranch_OnSelectedIndexChanged">
                                                <asp:ListItem Value="0" Selected="True" Text="-Select-"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Customer <span style="color: red">*</span>
                                            <asp:RequiredFieldValidator ID="rfvCustomer" runat="server" ControlToValidate="ddlCustomerMS" InitialValue="0" SetFocusOnError="true" Display="Dynamic"
                                                ForeColor="Red" Text="Required" ValidationGroup="vgConsolidate"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCustomerMS" runat="server" AppendDataBoundItems="true" DataSourceID="SqlDataSourceCustomer" AutoPostBack="true"
                                                DataTextField="CustName" DataValueField="lid" OnSelectedIndexChanged="ddlCustomerMS_SelectedIndexChanged" Width="300px">
                                                <asp:ListItem Value="0" Selected="True" Text="-Select-"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Division <span style="color: red">*</span>
                                            <asp:RequiredFieldValidator ID="rfvDivision" runat="server" ControlToValidate="ddDivision" InitialValue="0" SetFocusOnError="true" Display="Dynamic"
                                                ForeColor="Red" Text="Required" ValidationGroup="vgConsolidate"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddDivision" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddDivision_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Plant <span style="color: red">*</span>
                                            <asp:RequiredFieldValidator ID="rfvPlant" runat="server" ControlToValidate="ddPlant" InitialValue="0" SetFocusOnError="true" Display="Dynamic"
                                                ForeColor="Red" Text="Required" ValidationGroup="vgConsolidate"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddPlant" runat="server"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Remark</td>
                                        <td>
                                            <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Rows="3" Width="290px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div>
                                <asp:Repeater ID="rptDocument" runat="server" OnItemDataBound="rptDocument_ItemDataBound">
                                    <HeaderTemplate>
                                        <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr bgcolor="#FF781E">
                                                <th>Sl
                                                </th>
                                                <th>Name
                                                </th>
                                                <th>Browse
                                                </th>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <%#Container.ItemIndex +1%>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkDocType" Text='<%#DataBinder.Eval(Container.DataItem,"sName") %>' runat="server"
                                                    OnCheckedChanged="chkDocType_CheckedChanged" AutoPostBack="true" />&nbsp;
                                                <asp:HiddenField ID="hdnDocId" Value='<%#DataBinder.Eval(Container.DataItem,"lId") %>' runat="server"></asp:HiddenField>
                                            </td>
                                            <td>
                                                <asp:FileUpload ID="fuDocument" runat="server" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </fieldset>
                    </div>
                </asp:Panel>
            </div>
            <%-- Popup for Consolidate Jobs --%>

            <div>
                <asp:SqlDataSource ID="DataSourceContReceived" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CM_GetContReceivedJobs" SelectCommandType="StoredProcedure" UpdateCommand="CM_insCFSContainerRecdDate"
                    UpdateCommandType="StoredProcedure" OnUpdated="DataSourceContReceived_Updated" OnUpdating="DataSourceContReceived_Updating">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="lid" Type="Int32" />
                        <asp:Parameter Name="ContCFSReceivedDate" DbType="Date" />
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:Parameter Name="OutPut" Type="Int32" Direction="Output" Size="4" />
                    </UpdateParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSourceCustomer" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetCustomerMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSourceBranch" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetBabajiBranch" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

