<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Test.aspx.cs" Inherits="Test" %>
<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
            <asp:Button ID="btncheck" runat="server" Text="check" OnClick="btncheck_Click" /><br />
            <asp:TextBox ID="txtUserName" runat="server"></asp:TextBox>
        </div>
<%--        <rsweb:ReportViewer ID="ReportViewer1" runat="server" BackColor="" ClientIDMode="AutoID" HighlightBackgroundColor="" InternalBorderColor="204, 204, 204" InternalBorderStyle="Solid" InternalBorderWidth="1px" LinkActiveColor="" LinkActiveHoverColor="" LinkDisabledColor="" PrimaryButtonBackgroundColor="" PrimaryButtonForegroundColor="" PrimaryButtonHoverBackgroundColor="" PrimaryButtonHoverForegroundColor="" SecondaryButtonBackgroundColor="" SecondaryButtonForegroundColor="" SecondaryButtonHoverBackgroundColor="" SecondaryButtonHoverForegroundColor="" SplitterBackColor="" ToolbarDividerColor="" ToolbarForegroundColor="" ToolbarForegroundDisabledColor="" ToolbarHoverBackgroundColor="" ToolbarHoverForegroundColor="" ToolBarItemBorderColor="" ToolBarItemBorderStyle="Solid" ToolBarItemBorderWidth="1px" ToolBarItemHoverBackColor="" ToolBarItemPressedBorderColor="51, 102, 153" ToolBarItemPressedBorderStyle="Solid" ToolBarItemPressedBorderWidth="1px" ToolBarItemPressedHoverBackColor="153, 187, 226">
            <LocalReport ReportPath="SSRS\BillClosingReport.rdl">
            </LocalReport>
        </rsweb:ReportViewer>--%>

        <div _ngcontent-jiu-c166="" class="option-container ng-tns-c102-6">
            <div _ngcontent-jiu-c166="" class="select-all ng-star-inserted">
                <mat-checkbox _ngcontent-jiu-c166="" class="mat-checkbox mat-accent ng-untouched ng-pristine ng-valid" 
                    id="mat-checkbox-1"><label class="mat-checkbox-layout" for="mat-checkbox-1-input">
                        <span class="mat-checkbox-inner-container">
                            <input type="checkbox" class=" cdk-visually-hidden" id="mat-checkbox-1-input" 
                                tabindex="0" aria-checked="false">
                            <span matripple="" class="mat-ripple mat-checkbox-ripple ">
                                    <span class="mat-ripple-element mat-checkbox-persistent-ripple"><span class="mat-checkbox-frame">
                            <span class="mat-checkbox-background"><svg version="1.1" focusable="false" viewBox="0 0 24 24" xml:space="preserve" aria-hidden="true" class="mat-checkbox-checkmark"><path fill="none" stroke="white" d="M4.1,12.7 9,17.6 20.3,6.3" class="mat-checkbox-checkmark-path"></path></svg><span class="mat-checkbox-mixedmark"><span class="mat-checkbox-label"><span style="display: none;">&nbsp;Select All</label></mat-checkbox></div>
                        <mat-option _ngcontent-jiu-c166="" role="option" class="mat-option  mat-option-multiple ng-star-inserted" id="mat-option-0" tabindex="0" aria-selected="false" aria-disabled="false"><mat-pseudo-checkbox class="mat-pseudo-checkbox mat-option-pseudo-checkbox ng-star-inserted"></mat-pseudo-checkbox><!---->
                        1 VAHAN 
                        2 SARATHI 
                        3. FASTag 
                        4. FOIS 
                        5. ACCS 
                        6. DGFT 
                        7. COPT 
                        8. ICEGATE 
                        9. AAICLAS 
                        10. ACMES 
                        11. IPORTMAN 
                        12. JNPT 
                        13. LDB 
                        14. MBPT 
                        15. MPT 
                        16. NMPT 
                        17. PCS 
                        18. VOCPT 
                        19. IWAI 
                        20. GST 
                        21. ADANI 
                        22. MCA 
                        23. CFSICD 
                        24. DIGILOCKER 
    </form>
</body>
</html>
