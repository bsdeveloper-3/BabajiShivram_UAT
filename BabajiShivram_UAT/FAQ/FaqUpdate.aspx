            <%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
            EnableViewState="true" ValidateRequest="false" CodeFile="FaqUpdate.aspx.cs" Inherits="FAQ_FaqUpdate" %>

            <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
            <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
            <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" EnablePartialRendering="true" ScriptMode="Release" />
            <script type="text/javascript" src='../JS/tinymce/tinymce.min.js'></script>
            <script type="text/javascript">
            tinymce.init({
            selector: 'textarea',
            height: 400,
            browser_spellcheck: true,
            contextmenu: false,
            theme: 'modern',
            plugins: [
            'advlist autolink lists charmap preview hr  pagebreak',
            'searchreplace wordcount visualblocks visualchars code fullscreen ',
            'insertdatetime  nonbreaking save table contextmenu directionality',
            'template paste textcolor colorpicker textpattern codesample toc'
            ],
            toolbar1: 'undo redo | insert | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent ',
            toolbar2: 'forecolor backcolor | codesample ',
            image_advtab: true,
            templates: [
            { title: 'template 1', content: 'Test 1' },
            { title: 'template 2', content: 'Test 2' }
            ]
            });

            </script>
            <br />
   
            <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
            <asp:Button ID="btnUpdate" OnClick="btnUpdate_Click" runat="server" OnClientClick="return confirm('Have You Save Discription Befor Update?')" Text="Update" ValidationGroup="rec" />
            <center>
            <asp:Label ID="Lblerror" runat="server" ForeColor="Green" Font-Size="Small"></asp:Label></center>
            <br />
           
            <fieldset>

            <table border="50px" cellpadding="25px" cellspacing="50px" width="1000px" bgcolor="white" style="border-style: solid">
            <tr>
            <td>Servicess
            </td>
            <td>
            <asp:HiddenField ID="hdnfaqID" runat="server" />
            <asp:Label ID="lblServicess" runat="server" Width="130%"></asp:Label>
            </td>
            <td>Title
            </td>
            <td style="width: 100%">
            <asp:TextBox ID="txtTital" runat="server" Width="95%"> </asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ErrorMessage="*" ForeColor="Red"
                SetFocusOnError="true" ControlToValidate="txtTital" ValidationGroup="recd">
            </asp:RequiredFieldValidator>
            </td>
            </tr>
            </table>
            </fieldset>
            <%-- <br />
            <h3 style="font-size: large">Discription :  --%>
            <fieldset>
            <legend>Discription</legend>
            <asp:RequiredFieldValidator ID="rfvdiscription" runat="server" ErrorMessage="*" ForeColor="Red"
            SetFocusOnError="true" ControlToValidate="mytextarea" ValidationGroup="recd">
            </asp:RequiredFieldValidator></h3>
            <div class="myeditablediv">
            <asp:TextBox ID="mytextarea" runat="server" TextMode="MultiLine" Width="50%" Height="50%"></asp:TextBox>
            </div>
            </fieldset>
            <fieldset>
            <legend>Contact Person </legend>
            <asp:UpdatePanel ID="updgrd" runat="server">
            <ContentTemplate>
            <%-- <h3 style="font-size: large">Contact Person :</h3>--%>
            <div>
                <asp:GridView ID="GrvContactPerson" runat="server" ShowFooter="true"
                    OnRowEditing="GrvContactPerson_RowEditing"
                    OnRowCancelingEdit="GrvContactPerson_RowCancelingEdit" OnRowUpdating="GrvContactPerson_RowUpdating"
                    AutoGenerateColumns="false"
                    DataKeyNames="lid" Width="100%" PagerStyle-CssClass="pgr" CssClass="table"
                    DataSourceID="DataSorceContactPerson"
                    PagerSettings-Position="TopAndBottom"
                    CellPadding="4" AllowPaging="True" PageSize="20" AlternatingRowStyle-CssClass="alt"
                    Style="white-space: normal;">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name:">
                            <EditItemTemplate>
                                <asp:TextBox ID="TxtName" runat="server" Text='<%#Eval("ContactPerName") %>' class="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvName" runat="server" ErrorMessage="*" ForeColor="Red"
                                    SetFocusOnError="true" ControlToValidate="TxtName" ValidationGroup="recd">
                                </asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblname" Text='<%#Eval("ContactPerName") %>'></asp:Label>

                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Branch:">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblbranch" Text='<%#Eval("BranchName") %>'></asp:Label>

                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddBranch" runat="server" DataSourceID="SqlDataSourceBranchMS"
                                    DataTextField="BranchName" DataValueField="lid" AppendDataBoundItems="true">
                                    <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvbranch" runat="server" ErrorMessage="*" ForeColor="Red"
                                    SetFocusOnError="true" ControlToValidate="ddBranch" ValidationGroup="recd" InitialValue="0"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Phone No:">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtPhoneNo" runat="server" Text='<%#Eval("ContactPerPhoneNo") %>' class="form-control" MaxLength="12"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RFVPhonNO" ErrorMessage="*" ForeColor="Red" runat="server" ControlToValidate="txtPhoneNo" SetFocusOnError="true" ValidationGroup="recd">
                                </asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblphonNo" Text='<%#Eval("ContactPerPhoneNo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="EmailID:">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEmailID" runat="server" class="form-control" Text='<%#Eval("ContactPerEmailid") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RFVEmailid" ErrorMessage="*" ForeColor="Red" runat="server" ControlToValidate="txtEmailID" SetFocusOnError="true" ValidationGroup="recd">
                                </asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RFVpersonEmailID1" runat="server" ControlToValidate="txtEmailID"
                                    ErrorMessage="Invalid Email" ValidationGroup="recd" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                </asp:RegularExpressionValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblemailid" Text='<%#Eval("ContactPerEmailid") %>'></asp:Label>

                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:TextBox ID="txtContacttype" runat="server" Visible="false" Text='<%#Eval("ContactType") %>' class="form-control" MaxLength="12"></asp:TextBox>

                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit/Remove Contact Detail">
                            <ItemTemplate>

                                      
                    <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit" Width="22" runat="server"
                        Text="Edit" Font-Underline="true" CausesValidation="false" ValidationGroup="recd"></asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="lnkDelete" CommandName="Delete" ToolTip="Delete" Width="39" CausesValidation="false"
                                    runat="server" Text="Remove" Font-Underline="true" OnClientClick="return confirm('Are you sure you want to remove FAQ Report Field?')">
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update" Width="45" runat="server"
                                    Text="Update" Font-Underline="true" ValidationGroup="recd"></asp:LinkButton>
                                <asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" Width="39" CausesValidation="false"
                                    runat="server" Text="Cancel" Font-Underline="true"></asp:LinkButton>
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            </ContentTemplate>
            </asp:UpdatePanel>
            </fieldset>

            <fieldset>
            <legend>Upload Document / Form</legend>
            <table width="100%" style="height:100px">
            <tr>
            <td>
            <b>Document</b> :  &nbsp;&nbsp;&nbsp;  
                <asp:FileUpload ID="fuDoc1" runat="server" />
                            <asp:HiddenField ID="HFfuDoc1" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvdoc1" runat="server" SetFocusOnError="true"
                                ErrorMessage="Reqvird" ForeColor="Red" ControlToValidate="fuDoc1" ValidationGroup="rec" Enabled="false"></asp:RequiredFieldValidator>
            </td>
            <td>
            <%--<strong>Note:</strong><strong style="color: red"> More Than One Document  Please Upload Into The Zip File</strong>--%>
            <b>Form </b>:  &nbsp;&nbsp;&nbsp;  
                <asp:FileUpload ID="fuDocForm1" runat="server" />
                            <asp:HiddenField ID="HFfuDocForm1" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvdoc4" runat="server" SetFocusOnError="true"
                                ErrorMessage="Reqvird" ForeColor="Red" ControlToValidate="fuDocForm1" ValidationGroup="rec" Enabled="false"></asp:RequiredFieldValidator>
            </td>
            </tr>
            <tr>
            <td colspan="2">
            <strong style="color:red">Note* : &nbsp;</strong><strong > More Than One Documents/Forms Upload Into The Zip File.</strong>
            </td>
            </tr>
            </table>
            </fieldset>

            <asp:SqlDataSource ID="DataSorceContactPerson" runat="server" SelectCommand="FAQ_GetFAQContactPersonDetail"
            SelectCommandType="StoredProcedure" DeleteCommand="delFAQContactPerson" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            DeleteCommandType="StoredProcedure" OnDeleted="DataSourceContactPerson_Deleted">
            <SelectParameters>
            <asp:SessionParameter Name="serviceid" SessionField="FaqId" />
            </SelectParameters>
            <DeleteParameters>
            <asp:Parameter Name="lid" />
            <asp:SessionParameter Name="lUser" SessionField="UserId" />
            <asp:Parameter Name="outPut" Direction="outPut" Size="4" />
            </DeleteParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourceBranchMS" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="GetBabajiBranch" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </asp:Content>

