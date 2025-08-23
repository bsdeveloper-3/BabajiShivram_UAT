<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CustomerMom.aspx.cs" Inherits="CRM_CustomerMom" MaintainScrollPositionOnPostback="true" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
    </asp:ScriptManager>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upJobDetail" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                    <img alt="progress" src="../images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <style type="text/css">
        .modal-header {
            padding: 5px;
        }

        .modalBackground {
            background-color: black;
            filter: alpha(opacity=90);
            opacity: 0.5;
        }
    </style>
    <asp:UpdatePanel ID="upJobDetail" runat="server">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <div class="clear">
                <asp:Button ID="btnNewMOM" runat="server" Text="New MOM" OnClick="btnNewMOM_Click" TabIndex="1" />
                <asp:Button ID="btnBack" runat="server" Text="Go Back" OnClick="btnBack_Click" TabIndex="2" />
            </div>
            <fieldset>
                <legend>MOM List</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:datafilter id="DataFilter1" runat="server" />
                        </div>
                    </asp:Panel>
                    <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click">
                        <asp:Image ID="imgExcel" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                    </asp:LinkButton>
                </div>
                <div class="clear">
                </div>
                <div>
                    <asp:GridView ID="gvMOMs" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr" DataSourceID="DataSourceMOMs"
                        DataKeyNames="lid" OnRowCommand="gvMOMs_RowCommand" AllowPaging="True" AllowSorting="True" CssClass="table" PageSize="20"
                        PagerSettings-Position="TopAndBottom">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl" ItemStyle-Width="5%">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ItemStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkShowMOM" runat="server" ToolTip="View MOM" Text="View"
                                        CommandName="showmom" CommandArgument='<%#Eval("lid")%>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Customer" HeaderText="Customer" ReadOnly="true" />
                            <asp:BoundField DataField="Title" HeaderText="Meeting Title" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="Date" HeaderText="Meeting Date" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="CreatedBy" HeaderText="Created By" ReadOnly="true" />
                            <asp:BoundField DataField="dtDate" HeaderText="Created Date" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                        </Columns>
                        <PagerTemplate>
                            <asp:GridViewPager runat="server" />
                        </PagerTemplate>
                    </asp:GridView>
                </div>
            </fieldset>
            <%-- POPUP -- > MOM Draft Mail--%>
            <ajaxtoolkit:modalpopupextender id="ModalPopupEmail" runat="server" cachedynamicresults="false"
                dropshadow="False" popupcontrolid="pnlMOM" targetcontrolid="lnkDummy" backgroundcssclass="modalBackground">
            </ajaxtoolkit:modalpopupextender>
            <asp:Panel ID="pnlMOM" runat="server" CssClass="ModalPopupPanel">
                <div style="background-color: #8ab933">
                    <div>
                        <span style="font-weight: 600; font-size: 18px; font-family: Calibri; color: white; padding-left: 10px">MINUTES OF MEETINGS MAIL
                        </span>
                        <asp:ImageButton ID="imgClose" ImageUrl="../Images/Close.gif" runat="server" ImageAlign="Right" OnClick="imgClose_Click"/>
                    </div>
                </div>
                <div class="m"></div>
                <div id="DivABC" runat="server" style="max-height: 600px; max-width: 900px; overflow: auto;">
                    <div style="padding: 10px; font-size: 14px; margin-left: 10px; margin-right: 10px; margin-bottom: 20px;">
                        <div id="divMsg_Popup" runat="server"></div>
                        <asp:HiddenField ID="hdnMomId_Popup" runat="server" Value="0" />
                        <div style="padding-left: 2px; padding-right: 2px">
                            <label>Email To :</label>
                            &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;
                            <asp:TextBox ID="lblCustomerEmail" runat="server" Width="400px"></asp:TextBox>
                        </div>
                        <div style="padding-left: 2px; padding-right: 2px">
                            <label>Email CC :</label>
                            &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtMailCC" runat="server" Width="400px" AutoPostBack="true" OnTextChanged="txtMailCC_TextChanged"></asp:TextBox>
                        </div>
                        <div style="padding-left: 2px; padding-right: 2px">
                            <label>Subject :</label>
                            &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtSubject" runat="server" Width="400px" AutoPostBack="true" OnTextChanged="txtSubject_TextChanged"></asp:TextBox>
                        </div>
                        <div style="padding-left: 2px; padding-right: 2px">
                            <label>Participants :</label>
                            &nbsp;&nbsp;
                            <asp:TextBox ID="txtParticipants" runat="server" Width="400px" AutoPostBack="true" OnTextChanged="txtParticipants_TextChanged"></asp:TextBox>
                        </div>
                    </div>
                    <div id="divPreviewEmail" runat="server" style="margin-left: 10px; margin-right: 10px; margin-bottom: 20px;">
                    </div>
                    <div style="text-align: center">
                        <asp:Button ID="btnCancelPopup" runat="server" TabIndex="3" OnClick="btnCancelPopup_OnClick" Text="CANCEL" CssClass="btn btn-3d btn-default" />
                    </div>
                </div>
            </asp:Panel>
            <asp:HiddenField ID="lnkDummy" runat="server"></asp:HiddenField>
            <!--Customer Email Draft End -->
            <%-- POPUP -- > MOM Draft Mail--%>
            <div>
                <asp:SqlDataSource ID="DataSourceMOMs" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CRM_GetBSCustomerMOM" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYear" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

