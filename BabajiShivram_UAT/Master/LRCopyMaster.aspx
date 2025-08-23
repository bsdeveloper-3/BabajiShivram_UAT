<%@ Page Title="LR Copy" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="LRCopyMaster.aspx.cs"
    Inherits="Master_LRCopyMaster" Culture="en-GB" EnableEventValidation="false" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <style type="text/css">
         .grid th {
            background-color: darkslateblue;
            color: #ffffff;
            height: 5px;
        }

        .grid tr {
            height: 20px;
            border: 2px solid #ccc;
        }

        .grid td {
            padding-left: 10px;
            border: 2px solid #ccc;
        }
    </style>

    <script type="text/javascript">
        function OnJobSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnJobId').value = results.JobId;
        }
    </script>

    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
    <%--<asp:ScriptManager runat="server" ID="ScriptManager1" />--%>
        <cc1:toolkitscriptmanager id="ScriptManager1" runat="server" scriptmode="Release">
    </cc1:toolkitscriptmanager>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingLRCopy" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:ValidationSummary ID="ValSummaryJobDetail" runat="server" ShowMessageBox="True"
        ShowSummary="False" ValidationGroup="JobRequired" CssClass="errorMsg" EnableViewState="false" />

    <asp:UpdatePanel ID="upPendingLRCopy" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>

            <%--<asp:GridView ID="grdTest" runat="server" AutoGenerateColumns="false" Width="90%">
                  <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Packages">
                            <ItemTemplate>
                                <asp:TextBox ID="txtPackages" runat="server" Width="90%"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Description (said to contain)">
                            <ItemTemplate>
                                <asp:TextBox ID="txtDescription" runat="server" Width="90%"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Actual Wt.">
                            <ItemTemplate>
                                <asp:TextBox ID="txtActualWt" runat="server" Width="90%"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Charged">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCharged" runat="server" Width="90%"></asp:TextBox>
                            </ItemTemplate>                            
                        </asp:TemplateField>

                    </Columns>
            </asp:GridView>--%>
           
            <asp:Button  ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" ValidationGroup="JobRequired"/>
            <asp:HiddenField ID="hdnJobId" runat="server" Value="0" />
         <div align="center">
            <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
         </div>

            <fieldset>
                <legend>Delivery From</legend>
                <table width="70%" class="table">
                    <tr>
                        <td>Company</td>
                        <td align="left">
                            <asp:DropDownList ID="ddlCompany" runat="server" Width="200px">
                                <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                <asp:ListItem Text="NAV BHARAT CLEARING AGENTS PRIVATE LIMITED" Value="1"></asp:ListItem>
                                <asp:ListItem Text="NAVRAJ FINANCE PRIVATE LIMITED" Value="2"></asp:ListItem>                               
                            </asp:DropDownList>
                        </td>
                        <td>GSTIN/Unique Reg. No. of Person liable to pay
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlPersonLiable" runat="server" Width="200px">
                                <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Consignor" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Consignee" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Transporter" Value="3"></asp:ListItem>
                            </asp:DropDownList>
                        </td>                      
                    </tr>
                    <tr>
                        <td>C.N. No.</td>
                        <td>
                            <%--<asp:TextBox ID="txtCNNO" runat="server" Enabled="false"></asp:TextBox>--%>
                            <asp:Label ID="lblCNNO" runat="server" Enabled="false"></asp:Label>
                        </td>
                        <td>C.N.Date</td>
                        <td>
                            <asp:TextBox ID="txtCNDate" runat="server" Enabled="false" ForeColor="Black"></asp:TextBox>
                            <%--<cc1:calendarextender id="CalendarExtender1" runat="server" enabled="True" enableviewstate="False"
                                firstdayofweek="Sunday" format="dd/MM/yyyy" popupbuttonid="imgCNDate" popupposition="BottomRight"
                                targetcontrolid="txtCNDate">
                            </cc1:calendarextender>
                            <asp:Image ID="imgCNDate" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif" runat="server" />--%>

                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="JobRequired"
                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCNDate"
                                Text="*" ErrorMessage="Please Enter C. N. Date"></asp:RequiredFieldValidator>

                        </td>

                    </tr>
                    <tr>
                        <td>Invoice No.</td>
                        <td>
                            <asp:TextBox ID="txtInvoiceNo" runat="server"></asp:TextBox>
                        </td>
                        <td>Invoice Date</td>
                        <td>
                            <asp:TextBox ID="txtInvoiceDate" runat="server"></asp:TextBox>
                            <cc1:calendarextender id="calDOBUpd" runat="server" enabled="True" enableviewstate="False"
                                firstdayofweek="Sunday" format="dd/MM/yyyy" popupbuttonid="imgInvoiceDate" popupposition="BottomRight"
                                targetcontrolid="txtInvoiceDate">
                            </cc1:calendarextender>
                            <asp:Image ID="imgInvoiceDate" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>From</td>
                        <td>
                            <asp:TextBox ID="txtFrom" runat="server"></asp:TextBox>
                        </td>
                        <td>To</td>
                        <td>
                            <asp:TextBox ID="txtTo" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Consignor's Name & Address</td>
                        <td colspan="3">
                            <asp:TextBox ID="txtConsignorNmAddr" runat="server" TextMode="MultiLine" Width="400px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </fieldset>

            <fieldset>
                <legend>Delivery To</legend>

                <table width="70%" class="table">
                      <tr>
                        <td>Vehicle No.</td>
                        <td>
                            <asp:TextBox ID="txtVehicleNo" runat="server"></asp:TextBox>
                        </td>
                        <td>Our Job No.</td>
                        <td>
                            <asp:TextBox ID="txtOurJobNo" runat="server" ToolTip="Enter Job Number."  CssClass="SearchTextbox"
                                 placeholder="Search" TabIndex="1" AutoPostBack="true" OnTextChanged="txtOurJobNo_TextChanged"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="JobRequired"
                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtOurJobNo"
                                Text="*" ErrorMessage="Please Search Job No"></asp:RequiredFieldValidator>


                            <div id="divwidthCust_Loc" runat="server">
                            </div>
                            <cc1:autocompleteextender id="AutoCompleteJobNo" runat="server" targetcontrolid="txtOurJobNo"
                                completionlistelementid="divwidthCust_Loc" servicepath="~/WebService/LRJobDetailsAutoComplete.asmx"
                                servicemethod="GetJobRefNoList" minimumprefixlength="2" behaviorid="divwidthCust_Loc"
                                contextkey="4567" usecontextkey="True" onclientitemselected="OnJobSelected"
                                completionlistcssclass="AutoExtender" completionlistitemcssclass="AutoExtenderList"
                                completionlisthighlighteditemcssclass="AutoExtenderHighlight">
                            </cc1:autocompleteextender>

                        </td>
                    </tr>
                     <tr>
                        <td>Way Bill No.</td>
                        <td>
                            <asp:TextBox ID="txtWayBillNo" runat="server"></asp:TextBox>
                        </td>
                        <td>Vehicle Type</td>
                        <td>
                            <asp:TextBox ID="txtVehicleType" runat="server"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <td>Address of Delivery Office
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtDeliveryAddr" runat="server" TextMode="MultiLine" Width="400px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>State</td>
                        <td>
                            <asp:TextBox ID="txtState" runat="server"></asp:TextBox>
                        </td>
                        <td>Tel. No.</td>
                        <td>
                            <asp:TextBox ID="txtTelNo" runat="server"></asp:TextBox>
                        </td>

                    </tr>                  
                   
                    <tr>
                        <td>B/E No.</td>
                        <td>
                            <asp:TextBox ID="txtBENo" runat="server"></asp:TextBox>
                        </td>
                        <td>B/L No.</td>
                        <td>
                            <asp:TextBox ID="txtBLNo" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Consignee Name & Address</td>
                        <td colspan="3">
                            <asp:TextBox ID="txtConsigneeNmAddr" runat="server" TextMode="MultiLine" Width="400px"></asp:TextBox>

                              <asp:RequiredFieldValidator ID="rfvJobNo" runat="server" ValidationGroup="JobRequired"
                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtConsigneeNmAddr"
                                Text="*" ErrorMessage="Please Enter The Consignee Name & Address."></asp:RequiredFieldValidator>


                        </td>
                    </tr>
                </table>
            </fieldset>            

            <fieldset>
                <legend>Other Info</legend>              
                    <div align="center">
                        <asp:Label ID="lblOtherError" runat="server" EnableViewState="false"></asp:Label>
                    </div>

                    <asp:GridView ID="GrdOtherInfo" runat="server" ShowFooter="true" AutoGenerateColumns="false" Width="90%"
                    CssClass="grid" OnRowDataBound="GrdOtherInfo_OnRowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Packages">
                            <ItemTemplate>
                                <asp:TextBox ID="txtPackages" runat="server" Width="90%"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Description (said to contain)">
                            <ItemTemplate>
                                <asp:TextBox ID="txtDescription" runat="server" Width="90%"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Actual Wt.">
                            <ItemTemplate>
                                <asp:TextBox ID="txtActualWt" runat="server" Width="90%"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Charged">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCharged" runat="server" Width="90%"></asp:TextBox>
                            </ItemTemplate>  
                            
                            <FooterStyle HorizontalAlign="Right" />
                            <FooterTemplate>
                                <asp:Button ID="btnAdd" runat="server" Text="Add New Row" OnClick="btnAdd_Click"/>
                            </FooterTemplate>
                                                      
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>


            </fieldset>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

