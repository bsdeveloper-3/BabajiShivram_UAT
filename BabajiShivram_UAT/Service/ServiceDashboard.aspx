<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ServiceDashboard.aspx.cs" Inherits="Service_ServiceDashboard" 
    MasterPageFile="~/MasterPage.master" Title="Service Dashboard" EnableEventValidation="false" Culture="en-GB" %>
 <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
 <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    
    <asp:UpdatePanel ID="upPanel" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div style="float:left; width:70%;">
            <!--Notification -->
            <fieldset><legend>Notification</legend>
                <div>
                
                </div>
            </fieldset>
            <!--Notification -->
            <fieldset><legend>Upcomming Event</legend>
                <div>
                
                </div>
            </fieldset>
            
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
 </asp:Content>