<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AgentPreAlert.aspx.cs" 
    Inherits="FreightOperation_AgentPreAlert" Title="Agent PreAlert Details" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPanelDetail" runat="server">
            <ProgressTemplate>
                <div style="position:absolute;visibility:visible;border:none;z-index:100;width:90%;height:90%;background:#FAFAFA; filter: alpha(opacity=80);-moz-opacity:.8; opacity:.8;">
                    <img alt="progress" src="../images/progress-indicator.gif" style="position:relative; top:40%;left:40%; "/>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPanelDetail" runat="server">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="ValSummaryFreightDetail" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
                <asp:HiddenField ID="hdnUploadPath" runat="server" />
            </div>
            <div class="clear"></div>
            <fieldset><legend>PreAlert Detail</legend>
                <div class="m clear">
                    <asp:Button ID="btnUpdate" runat="server" Text="Save" OnClick="btnSubmit_Click" ValidationGroup="Required" />
                    <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CausesValidation="False" Text="Cancel"/>
                    <asp:HiddenField ID="hdnModeId" runat="server" />
                </div>
                <table border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
                    <tr>
                        <td>
                            Job No
                        </td>
                        <td>
                            <asp:Label ID="lblJobNo" runat="server"></asp:Label>
                        </td>
                        <td>
                            Booking Month
                        </td>
                        <td>
                            <asp:Label ID="lblBookingMonth" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            MBL No
                            <%--<asp:RequiredFieldValidator ID="RFVMBLNo" runat="server" ControlToValidate="txtMBLNo" Display="Dynamic" 
                                SetFocusOnError="true" Text="*" ErrorMessage="Please Enter MBL Number." ValidationGroup="Required"></asp:RequiredFieldValidator>--%>
                            <asp:regularexpressionvalidator id="RFVMBLNo" runat="server" controltovalidate="txtMBLNo" Display="Dynamic"
                               SetFocusOnError="true" ErrorMessage="Please enter proper MBL No" ValidationExpression="[a-zA-Z0-9]+$" ValidationGroup="Required"/>
                        </td>
                        <td>
                           <asp:TextBox ID="txtMBLNo" runat="server" MaxLength="20" OnTextChanged="txtMBLNo_TextChanged" AutoPostBack="true"></asp:TextBox>
                        </td>
                        <td>
                            MBL Date
                            <AjaxToolkit:CalendarExtender ID="calMBLDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgMBLDate" PopupPosition="BottomRight"
                                TargetControlID="txtMBLDate">
                            </AjaxToolkit:CalendarExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMBLDate" runat="server"  Width="100px" OnTextChanged="txtMBLDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <asp:Image ID="imgMBLDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server"/>
                            <AjaxToolkit:MaskedEditExtender ID="MskExtMBLDate" TargetControlID="txtMBLDate" Mask="99/99/9999" MessageValidatorTip="true" 
                                MaskType="Date" AutoComplete="false" runat="server"></AjaxToolkit:MaskedEditExtender>
                            <AjaxToolkit:MaskedEditValidator ID="MskValMBLDate" ControlExtender="MskExtMBLDate" ControlToValidate="txtMBLDate" IsValidEmpty="true" 
                                InvalidValueMessage="MBL Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                EmptyValueMessage="Please Enter MBL Date" EmptyValueBlurredText="Required" MinimumValueMessage="Invalid Date" 
                                MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" 
                                Runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            HBL No
                            <asp:RequiredFieldValidator ID="RFVHBLNo" runat="server" ControlToValidate="txtHBLNo" Display="Dynamic" 
                            SetFocusOnError="true" Text="*" ErrorMessage="Please Enter HBL Number." ValidationGroup="Required"></asp:RequiredFieldValidator>
                            <asp:regularexpressionvalidator id="RFVHBLNo1" runat="server" controltovalidate="txtHBLNo" SetFocusOnError="true"
                                ErrorMessage="HBL No Space not allowed" ValidationExpression="[^\s]+" ValidationGroup="Required"/>
                        </td>
                        <td>
                           <asp:TextBox ID="txtHBLNo" runat="server" MaxLength="20" OnTextChanged="txtHBLNo_TextChanged" AutoPostBack="true"></asp:TextBox>
                        </td>
                        <td>
                            HBL Date
                            <AjaxToolkit:CalendarExtender ID="calHBLDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgHBLDate" PopupPosition="BottomRight"
                                TargetControlID="txtHBLDate">
                            </AjaxToolkit:CalendarExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtHBLDate" runat="server" Width="100px" OnTextChanged="txtHBLDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <asp:Image ID="imgHBLDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server"/>
                            <AjaxToolkit:MaskedEditExtender ID="MskExtHBLDate" TargetControlID="txtHBLDate" Mask="99/99/9999" MessageValidatorTip="true" 
                                MaskType="Date" AutoComplete="false" runat="server"></AjaxToolkit:MaskedEditExtender>
                            <AjaxToolkit:MaskedEditValidator ID="MskValHBLDate" ControlExtender="MskExtHBLDate" ControlToValidate="txtHBLDate" IsValidEmpty="false" 
                                InvalidValueMessage="HBL Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                EmptyValueMessage="Please Enter HBL Date" EmptyValueBlurredText="Required" MinimumValueMessage="Invalid Date" 
                                MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" 
                                Runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           Vessel / Airline
                        </td>
                        <td>
                            <asp:TextBox ID="txtVesselName" runat="server" MaxLength="100"></asp:TextBox>
                        </td>
                        <td>
                           Flight / Voyage
                        </td>
                        <td>
                            <asp:TextBox ID="txtVesselNumber" runat="server" MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            ETD
                            <AjaxToolkit:CalendarExtender ID="calETDDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgETDDate" PopupPosition="BottomRight"
                                TargetControlID="txtETDDate">
                            </AjaxToolkit:CalendarExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtETDDate" runat="server" Width="100px"></asp:TextBox>
                            <asp:Image ID="imgETDDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server"/>
                            <AjaxToolkit:MaskedEditExtender ID="MskExtETDDate" TargetControlID="txtETDDate" Mask="99/99/9999" MessageValidatorTip="true" 
                                MaskType="Date" AutoComplete="false" runat="server"></AjaxToolkit:MaskedEditExtender>
                            <AjaxToolkit:MaskedEditValidator ID="MskValETDDate" ControlExtender="MskExtETDDate" ControlToValidate="txtETDDate" IsValidEmpty="true" 
                                InvalidValueMessage="ETD Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                EmptyValueMessage="Please Enter ETD Date" EmptyValueBlurredText="Required" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" 
                                MinimumValue="01/01/2015" MaximumValue="01/01/2026" Runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                        </td>
                        <td>
                            ETA
                            <AjaxToolkit:CalendarExtender ID="calETADate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgETADate" PopupPosition="BottomRight"
                                TargetControlID="txtETADate">
                            </AjaxToolkit:CalendarExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtETADate" runat="server" Width="100px"></asp:TextBox>
                            <asp:Image ID="imgETADate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server"/>
                            <AjaxToolkit:MaskedEditExtender ID="MskExtETADate" TargetControlID="txtETADate" Mask="99/99/9999" MessageValidatorTip="true" 
                                MaskType="Date" AutoComplete="false" runat="server"></AjaxToolkit:MaskedEditExtender>
                            <AjaxToolkit:MaskedEditValidator ID="MskValETADate" ControlExtender="MskExtETADate" ControlToValidate="txtETADate" IsValidEmpty="false" 
                                InvalidValueMessage="ETA Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                EmptyValueMessage="Please Enter ETA Date" EmptyValueBlurredText="Required" MinimumValueMessage="Invalid Date" 
                                MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2026" 
                                Runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           Final Agent
                           <asp:RequiredFieldValidator ID="RFVFinalAgent" runat="server" ControlToValidate="ddFinalAgent" Display="Dynamic" 
                            InitialValue="0" SetFocusOnError="true" Text="*" ErrorMessage="Please Select Final Agent" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <%--<asp:TextBox ID="txtFinalAgent" runat="server" MaxLength="100"></asp:TextBox>--%>
                            <asp:DropDownList ID="ddFinalAgent" runat="server" DataSourceID="DatasourceFreightAgent" 
                                DataTextField="CustName" DataValueField="lid" AppendDataBoundItems="true">
                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>LCL/FCL</td>
                        <td>
                            <asp:Label ID="lblLCLFCL" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Container20"
                        </td>
                        <td>
                            <asp:Label ID="lblContainer20" runat="server"></asp:Label>
                        </td>
                        <td>Container40"
                        </td>
                        <td>
                            <asp:Label ID="lblContainer40" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <AjaxToolkit:Accordion ID="Accordion1" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" 
                    ContentCssClass="accordionContent" runat="server" SelectedIndex="0" FadeTransitions="true" SuppressHeaderPostbacks="true" 
                    TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false" AutoSize="None">
                    <Panes>
                    <AjaxToolkit:AccordionPane ID="accDocument" runat="server">
                    <Header>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Document</Header>
                    <Content>
                    <fieldset><legend>Upload</legend>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td width="110px" align="center">
                                Document Name
                                <asp:RequiredFieldValidator ID="RFVDocName" runat="server" ControlToValidate="ddl_DocumentType" Display="Dynamic" ValidationGroup="validateDocument"
                                    SetFocusOnError="true" Text="*" ErrorMessage="Enter Document Name."></asp:RequiredFieldValidator>
                            </td>
                            <td width="50" align="center">
                                <%--<asp:TextBox ID="txtDocName" runat="server"></asp:TextBox>--%>
                                <asp:DropDownList ID="ddl_DocumentType" runat="server" DataSourceID="FrDocTypeSqlDataSource"
                                    DataTextField="Sname" DataValueField="lid">
                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                </asp:DropDownList>

                            </td>
                            <td align="center">
                                Attachment
                                <asp:RequiredFieldValidator ID="RFVAttach" runat="server" ControlToValidate="fuDocument" Display="Dynamic" ValidationGroup="validateDocument"
                                    SetFocusOnError="true" Text="*" ErrorMessage="Attach File For Upload."></asp:RequiredFieldValidator>
                            </td>    
                            <td>
                                <asp:FileUpload ID="fuDocument" runat="server" /><asp:Button ID="btnUpload" runat="server"
                                    Text="Upload" OnClick="btnUpload_Click" ValidationGroup="validateDocument"  />
                            </td>
                        </tr>
                    </table>
                        <div>
                        <asp:SqlDataSource ID="FrDocTypeSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="Get_FRDocumentType" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                                <asp:QueryStringParameter Name="JobType" DbType="String" DefaultValue='Import' />
                                <asp:SessionParameter Name="JobMode" SessionField="JobMode" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                    </fieldset>
                    <fieldset>
                    <legend>Download</legend>
                        <asp:GridView ID="gvFreightDocument" runat="server" AutoGenerateColumns="False" Width="100%"  
                        DataKeyNames="DocId" DataSourceID="FreightDocumentSqlDataSource" CssClass="table"
                        OnRowCommand="gvFreightDocument_RowCommand" CellPadding="4" PagerStyle-CssClass="pgr"
                        AllowPaging="true" PageSize="20" AllowSorting="True" PagerSettings-Position="TopAndBottom">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="DocName" HeaderText="Document Name" SortExpression="DocName" />
                            <asp:BoundField DataField="UserName" HeaderText="Uploaded By" />
                            <asp:BoundField DataField="UploadedDate" HeaderText="Uploaded Date" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:TemplateField HeaderText="Download">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                                        CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Remove" Visible="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnlRemoveDocument" runat="server" Text="Remove" CommandName="RemoveDocument"
                                        CommandArgument='<%#Eval("DocId") %>' OnClientClick="return confirm('Are you sure to remove document?');" ></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </fieldset>
                    </Content>
                    </AjaxToolkit:AccordionPane>
                    <AjaxToolkit:AccordionPane ID="accContainer" runat="server">
                    <Header>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Container</Header>
                    <Content>
                    <fieldset><legend>Add Container</legend>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>Container 20</td>
                                <td>
                                    <asp:label ID="lblCont20" runat="server"></asp:label>
                                </td>
                                <td>Container 40</td>
                                <td>
                                    <asp:label ID="lblCont40" runat="server"></asp:label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Container No
                                    <asp:RequiredFieldValidator ID="RFVContainer" runat="server" ControlToValidate="txtContainerNo"
                                        ValidationGroup="valContainer" SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Container No"
                                        Display="Dynamic">
                                    </asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtContainerNo" runat="server" MaxLength="11"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="REVContainer" runat="server" ControlToValidate="txtContainerNo"
                                        ValidationGroup="valContainer" SetFocusOnError="true" ErrorMessage="Enter 11 Digit Container No."
                                        Display="Dynamic" ValidationExpression="^[a-zA-Z0-9]{11}$"></asp:RegularExpressionValidator>
                                </td>
                                <td>
                                    Container Type
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddContainerType" runat="server">
                                        <asp:ListItem Text="FCL" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="LCL" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Container Size
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddContainerSize" runat="server">
                                        <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="20" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="40" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                <asp:Button ID="btnAddContainer" Text="Add Container" OnClick="btnAddContainer_Click"
                                        ValidationGroup="valContainer" runat="server"/>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <br />
                    <div>
                        <fieldset><legend>Container Detail</legend>
                            <asp:GridView ID="gvContainer" runat="server" AllowPaging="true" CssClass="table"
                                PagerStyle-CssClass="pgr" AutoGenerateColumns="false" DataKeyNames="lid" Width="100%"
                                PageSize="20" DataSourceID="DataSourceContainer">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ContainerNo" HeaderText="Container No" />
                                    <asp:BoundField DataField="ContainerTypeName" HeaderText="Container Type" />
                                    <asp:BoundField DataField="ContainerSizeName" HeaderText="Container Size" />
                                </Columns>
                            </asp:GridView>
                        </fieldset>
                    </div>
                    <div>
                        <asp:SqlDataSource ID="DataSourceContainer" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="FOP_GetContainerMS" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                    </Content>
                    </AjaxToolkit:AccordionPane>
                    </Panes>
                </AjaxToolkit:Accordion>
                <div>
                    <asp:SqlDataSource ID="FreightDocumentSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="FR_GetUploadedDocument" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="DatasourceFreightAgent" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="GetCompanyByCategoryID" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:Parameter Name="CategoryID" DefaultValue="5" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>  
</asp:Content>