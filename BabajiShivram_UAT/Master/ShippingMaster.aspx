<%@ Page Title="Shipping Master" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ShippingMaster.aspx.cs"
    Inherits="Master_ShippingMaster" Culture="en-GB" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <style type="text/css">
        .modalBackground {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.5;
        }

        .modalPopup {
            background-color: #000032;
            border-width: 1px;
            border-style: solid;
            border-color: black;
            padding-top: 1px;
            padding-left: 0px;
            width: 650px;
            height: 200px;
        }
    </style>

    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingchecklist" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:ValidationSummary ID="ValSummaryJobDetail" runat="server" ShowMessageBox="True"
        ShowSummary="False" ValidationGroup="JobRequired" CssClass="errorMsg" EnableViewState="false" />

    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <fieldset>
                <legend>Shipping Line Detail</legend>
                <!-- Filter Content Start-->
                <div class="fleft">
                    <uc1:DataFilter ID="DataFilter1" runat="server" />
                </div>
                <!-- Filter Content END-->
                <div class="fleft">
                   <asp:Button ID="btnNew" runat="server" Text="NEW" OnClick="btnNew_Click"/>
                </div>

                <div class="fright">
                    <asp:LinkButton ID="lnkexport" runat="server"  OnClick="lnkexport_Click" > 
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Excel.jpg" ToolTip="Export To Excel" />
                    </asp:LinkButton>
                </div>

                <div class="clear"></div>
                <div>
                    <asp:GridView ID="GridView1" runat="server" DataSourceID="GridViewSqlDataSource"
                        DataKeyNames="lid" Width="100%" AllowPaging="True" PageSize="20" AllowSorting="true"
                        CssClass="table" OnRowCommand="GridView1_RowCommand" AutoGenerateColumns="False"
                        PagerStyle-CssClass="pgr" PagerSettings-Position="TopAndBottom" OnPreRender="GridView1_PreRender">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Shipping Line Name" SortExpression="CompName">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkShippingName" runat="Server" Text='<%#Eval("CompName") %>' CommandName="navigate"
                                        CommandArgument='<%#Eval("SCode") +","+ Eval("lid") %>' CausesValidation="false"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ShippingLIneCode" HeaderText="Shipping Code" SortExpression="ShippingLIneCode"/>
                            <asp:BoundField DataField="CompName" HeaderText="Company Name" Visible="false" />
                            <asp:BoundField DataField="SCode" HeaderText="SCode" SortExpression="SCode" />                          

                            <%--<asp:TemplateField HeaderText="Add Branch">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbranchdetail" runat="server" CommandArgument='<%#Eval("lid") %>'
                                    CommandName="Navigate" CausesValidation="False">Add New Branch</asp:LinkButton>
                            </ItemTemplate>
                            </asp:TemplateField>--%>
                        </Columns>
                        <PagerTemplate>
                            <asp:GridViewPager runat="server" />
                        </PagerTemplate>
                    </asp:GridView>
                    <asp:SqlDataSource ID="GridViewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="GetShippingMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                </div>
            </fieldset>

            <div id="divHold">
                <cc1:ModalPopupExtender ID="ModalPopupStatus" runat="server" PopupControlID="PanelSEZStatus" TargetControlID="btnNew" CacheDynamicResults="false"
                    CancelControlID="imgStatusClose" BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="PanelSEZStatus" runat="server" CssClass="modalPopup" align="center" Style="display: none" BackColor="#EBF4FA" >
                        <%-- <center>  <asp:Label ID="lblPopMessageQuery" runat="server" EnableViewState="false"></asp:Label></center>                                                --%>
                        <table width="100%" style="background-color: #c7a879">
                        <tr>
                            <td style="width: 33%"></td>
                            <td style="width: 34%">
                                <div>
                                    <center>
                                        <b>Add New Shipping Company </b>
                                    </center>
                                </div>

                            </td>
                            <td style="width: 33%">
                                <div class="fright">
                                    <asp:ImageButton ID="imgStatusClose" ImageUrl="~/Images/delete.gif" runat="server" ToolTip="Close" />
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div class="m"></div>
                    <div>
                        <asp:Label ID="lblPopMessage" runat="server" EnableViewState="false"></asp:Label>
                        <asp:HiddenField ID="hdnJobId" runat="server" />   
                    </div>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td><b>SCode</b></td>
                            <td>
                                <asp:Label ID="lblSCode" runat="server" ></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td><b> Shipping Line Name </b></td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtShipCompName"
                            Text="Required" ErrorMessage="Please Enter The Shipping Line Name" ValidationGroup="JobRequired" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            <td>
                                <asp:TextBox ID="txtShipCompName" runat="server" Width="500px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td><b> Shipping Line Code </b></td>
                            <%--<asp:RequiredFieldValidator ID="RFVShippingCode" runat="server" ControlToValidate="txtShipCompCode"
                            Text="Required" ErrorMessage="Please Enter The Code" ValidationGroup="JobRequired" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                            <td>
                                <asp:TextBox ID="txtShipCompCode" runat="server" Width="100px" MaxLength="4"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:40%"><b> Address </b>
                            <asp:RequiredFieldValidator ID="RFVReason1" runat="server" ControlToValidate="txtAddress"
                            Text="Required" ErrorMessage="Please Enter The Address" ValidationGroup="JobRequired" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                            <td >
                                <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" MaxLength="200" Rows="5"></asp:TextBox>
                            </td>
                        </tr>  
                            <tr>
                            <td>
                                <asp:Button ID="BtnSaveStatus" Text="ADD" runat="server" ValidationGroup="JobRequired" OnClick="BtnSaveStatus_Click"/> <%--  OnClick="BtnSaveStatus_Click"  OnClick="BtnSaveQuery_Click"--%>
                            </td>
                            <td>
                            </td>                                    
                            </tr>                            
                        </table>                           
                    </asp:Panel>

                </div>
                <div>
                    <asp:LinkButton ID="lnkDummy" runat="server" Text=""></asp:LinkButton>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

