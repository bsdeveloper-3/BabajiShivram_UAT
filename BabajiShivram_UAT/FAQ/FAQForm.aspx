<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="FAQForm.aspx.cs"
    Inherits="FAQ_FAQForm" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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

    <asp:Button ID="btnSave" runat="server" Text="Save FAQ" OnClick="btnSave_Click" ValidationGroup="rec" />
    <br />

    <center>
        <asp:Label ID="Lblerror" runat="server" Style="font-size: large; color: forestgreen"></asp:Label></center>
    <br />
    <fieldset>
        
        <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="font-size: small">Services
                <asp:RequiredFieldValidator ID="RFVServicess" runat="server" ControlToValidate="ddServicess" InitialValue="0"
                    SetFocusOnError="true" ValidationGroup="rec" ErrorMessage="*"> </asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:DropDownList ID="ddServicess" runat="server"></asp:DropDownList>
                </td>
                <td style="font-size: small">Title (Subject)
                <asp:RequiredFieldValidator ID="RFVTitle" runat="server" ValidationGroup="rec" ControlToValidate="TxtTitle"
                    SetFocusOnError="true" ErrorMessage="*"></asp:RequiredFieldValidator>
                </td>
                <td style="width: 74%; height: 40%">
                    <asp:TextBox ID="TxtTitle" runat="server" class="form-control" Rows="5" Width="99%" Height="50%"></asp:TextBox>
                </td>
            </tr>
        </table>
    </fieldset>
    
    <fieldset>
        <legend>Contact Person</legend>
        <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td>
                    <asp:GridView ID="GrvContactPerson" runat="server" ShowFooter="true" AutoGenerateColumns="false"
                        OnRowCreated="GrvContactPerson_RowCreated" class="table table-bordered">
                        <Columns>
                            <asp:BoundField DataField="RowNumber" HeaderText="Sr" />
                            <asp:TemplateField HeaderText="Name">
                                <ItemTemplate>
                                    <asp:TextBox ID="TxtName" runat="server" class="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RfvName" runat="server" ErrorMessage="*"
                                        SetFocusOnError="true" ForeColor="red" ControlToValidate="TxtName"
                                        ValidationGroup="rec"></asp:RequiredFieldValidator>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Left" />
                                <FooterTemplate>
                                    <asp:Button ID="btnContactAdd" runat="server" Text="Add New Row" class="btn btn-primary"
                                        OnClick="btnContactAdd_Click" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Branch">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddBranch" runat="server" DataSourceID="SqlDataSourceBranchMS"
                                        DataTextField="BranchName" DataValueField="lid" AppendDataBoundItems="true">
                                        <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvbranch" runat="server" ControlToValidate="ddBranch"
                                        SetFocusOnError="true" ValidationGroup="rec" InitialValue="0" ErrorMessage="*" ForeColor="Red">
                                    </asp:RequiredFieldValidator>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Phone No">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPhoneNo" runat="server" class="form-control" MaxLength="12"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RFVPhonNO" ErrorMessage="*" runat="server" ForeColor="Red"
                                        ControlToValidate="txtPhoneNo" SetFocusOnError="true" ValidationGroup="rec"></asp:RequiredFieldValidator>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="EmailID">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtEmailID" runat="server" class="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RfvHSNCode" runat="server" ErrorMessage="*"
                                        SetFocusOnError="true" ForeColor="Red" ControlToValidate="txtEmailID" ValidationGroup="rec"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RFVpersonEmailID1" runat="server" ControlToValidate="txtEmailID"
                                        ErrorMessage="Invalid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                    </asp:RegularExpressionValidator>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <a href="#">
                                        <asp:LinkButton ID="LinkbtnFaqRemove" runat="server" BorderColor="Black" OnClick="LinkbtnFaqRemove_Click"
                                            Text="Remove" ForeColor="Red" class="btn btn-danger btn-xs">Remove</asp:LinkButton>
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle BackColor="#E6E9ED" ForeColor="#73879C" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSourceBranchMS" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="GetBabajiBranch" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                </td>
            </tr>
        </table>
    </fieldset>

    <fieldset>
        <legend>Discription</legend>
        <div class="myeditablediv">
            <asp:TextBox ID="mytextarea" runat="server" TextMode="MultiLine" Width="50%" Height="50%" ></asp:TextBox>
        </div>
    </fieldset>

    <fieldset>
        <legend>Upload Document / Form</legend>
        <table width="100%" style="height:100px">
            <tr>
                <td>
                    <b>Document</b> :  &nbsp;&nbsp;&nbsp;  
                    <asp:FileUpload ID="fuDoc1" runat="server" />
                </td>
                <td>
                    <%--<strong>Note:</strong><strong style="color: red"> More Than One Document  Please Upload Into The Zip File</strong>--%>
                    <b>Form </b>:  &nbsp;&nbsp;&nbsp;  
                    <asp:FileUpload ID="fuDocForm1" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <strong style="color:red">Note* : &nbsp;</strong><strong > More Than One Documents/Forms Upload Into The Zip File.</strong>
                </td>
            </tr>
        </table>
    </fieldset>

</asp:Content>

