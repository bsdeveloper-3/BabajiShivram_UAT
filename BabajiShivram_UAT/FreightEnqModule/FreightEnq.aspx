<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FreightEnq.aspx.cs" Inherits="FreightEnqModule_FreightEnq"
    Culture="en-GB" MasterPageFile="~/FreightEnqModule/FreightEnqMaster.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="scrptMngr" />
    <style type="text/css">
        .SearchTextbox
        {
            background: #FFF url(../Images/input-text-search.png) no-repeat right;
        }
        
        .AutoExtender
        {
            font-family: "Helvetica Neue" , Helvetica, Arial, sans-serif;
            font-size: 14px;
            font-weight: normal;
            border: 1px solid #ddd;
            border-radius: 4px;
            box-sizing: border-box;
            display: block;
            position: absolute;
            line-height: 20px;
            padding: 3px;
            background-color: white;
            margin: 0px;
            overflow-y: auto;
            text-align: left;
            list-style: none;
            height: 200px;
            width: 391px;
        }
        .AutoExtenderList
        {
            padding: 6px;
            cursor: pointer;
            font-size: 14px;
        }
        .AutoExtenderHighlight
        {
            padding: 6px;
            background-color: #ddd;
            cursor: pointer;
            font-size: 14px;
        }
       
        .ddlColor
        {
            color: Grey;
        }
        .fancy-form > textarea, .fancy-form > input
        {
            padding-left: 12px;
        }
        div#ctl00_ContentPlaceHolder1_ModalPopupExtender1_backgroundElement
        {
            position: absolute;
            left: 0px;
            top: 0px;
            display: none;
        }
        .min
        {
            top: 94% !important;
        }
    </style>
    <script type="text/javascript">
        function MinimizeMsg() {
            var button = document.getElementById('ctl00_ContentPlaceHolder1_btnMinMaxPopup');
            if (button.value.toString() == 'Minimize') {
                var popup = document.getElementById('ctl00_ContentPlaceHolder1_pnlpopup');
                popup.classList.add('min');
                button.value = 'Maximize';
                return false;
            }
            else if (button.value.toString() == 'Maximize') {
                var popup = document.getElementById('ctl00_ContentPlaceHolder1_pnlpopup');
                popup.classList.remove('min');
                button.value = 'Minimize';
                return false;
            }
        }

        function OnPortOfLoadingSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=hdnLoadingPortId.ClientID%>').value = results.PortOfLoadingId;
            $get('<%=txtPortLoading.ClientID%>').focus();
        }
        $addHandler
        (
            $get('<%=txtPortLoading.ClientID%>'), 'keyup',

            function () {
                $get('<%=hdnLoadingPortId.ClientID %>').value = '0';
            }
        );

        function OnPortOfDischargedSelected(source, eventArgs) {
            var resDischarged = eval('(' + eventArgs.get_value() + ')');
            $get('<%=hdnPortOfDischargedId.ClientID%>').value = resDischarged.PortOfLoadingId;
            $get('<%=txtPortOfDischarged.ClientID%>').focus();
        }
        $addHandler
        (
            $get('<%=txtPortOfDischarged.ClientID%>'), 'keyup',

            function () {
                $get('<%=hdnPortOfDischargedId.ClientID %>').value = '0';
            }
        );
    </script>
    <!-- WRAPPER -->
    <div id="wrapper" class="clearfix">
        <aside id="aside">
	        <nav id="sideNav"><!-- MAIN MENU -->
		        <ul class="nav nav-list">
			        <li><!-- dashboard -->
				        <a class="dashboard" href="Dashboard.aspx"><!-- warning - url used by default by ajax (if eneabled) -->
					        <i class="main-icon fa fa-dashboard"></i> <span>Dashboard</span>
				        </a>
			        </li>	
                    <li class="active"><!-- Enquiry -->
                        <a href="FreightEnq.aspx"><!-- warning - url used by default by ajax (if eneabled) -->
	                        <i class="main-icon fa fa-pencil-square-o"></i><span>Enquiry</span>
                        </a>
                    </li>	
                    <li><!-- Enquiry Tracking-->
                        <a href="EnquiryTracking.aspx"><!-- warning - url used by default by ajax (if eneabled) -->
	                        <i class="main-icon fa fa-bar-chart-o"></i><span>Enquiry Tracking</span>
                        </a>
                    </li>																	
		        </ul>		
	        </nav>
	        <span id="asidebg"><!-- aside fixed background --></span>
        </aside>
        <!-- /ASIDE -->
        <!-- HEADER -->
        <header id="header">
	        <!-- Mobile Button -->
	        <button id="mobileMenuBtn"></button>

	        <!-- Logo -->
	        <span class="logo pull-left" style="margin-left:4px;">
		        <img src="FR_assets/images/Babaji_logo3.jpg" alt="admin panel" height="50px" width="225px" style="padding:3px" />
                &nbsp;&nbsp;
            </span>
            <span class="logo pull-left" style="margin-left: 60px;">
               <img src="https://img.alicdn.com/tps/TB1wy3QNXXXXXXMaXXXXXXXXXXX-347-38.png" alt="" height="50px" width="300px" style="padding:3px;">
	        </span>
            <!-- Logo -->		

	        <nav>
		        <!-- OPTIONS LIST -->
		        <ul class="nav pull-right">
			        <!-- USER OPTIONS -->
			        <li class="dropdown pull-left">
				        <a href="#" class="dropdown-toggle" data-toggle="dropdown" data-hover="dropdown" data-close-others="true">
					        <img class="user-avatar" alt="" src="FR_assets/images/noavatar.jpg" height="34" /> 
					        <span class="user-name">
						        <span class="hidden-xs">
							       <asp:Label ID="lblUserName" runat="server"></asp:Label> <i class="fa fa-angle-down"></i>
						        </span>
					        </span>
				        </a>
				        <ul class="dropdown-menu hold-on-click">					       
					        <li id="li_Inbox" runat="server"></li>					       
					        <li class="divider"></li>
					        <li><a href="FRlogin.aspx"><i class="fa fa-power-off"></i> Log Out</a></li>
				        </ul>
			        </li>
			        <!-- /USER OPTIONS -->

		        </ul>
		        <!-- /OPTIONS LIST -->
	        </nav>
        </header>
        <!-- /HEADER -->
        <section id="middle">
            <!-- page title -->
            <header id="page-header">
	            <h1><strong>Freight Enquiry</strong></h1>	          
            </header>
            <!-- /page title -->
	        <div id="content" class="padding-20">
                <div class="row">
                    <div class="col-md-12">					
	                    <div class="panel panel-primary">
		                    <div class="panel-heading panel-heading-transparent">
                                <span class="pull-left" style="padding-right: 15px;">
                                    <a href="javascript:window.history.back();" style="color: white;">
                                        <i class="fa fa-backward" style="font-size: 17px;"></i>
                                    </a>
                                </span>
                                <strong>Add New Enquiry</strong>
		                    </div>
		                    <div class="panel-body">
                                <div id="dvError" runat="server"><!-- DANGER -->
                                </div>
                                <asp:Label ID="lblEnqRefNo" runat="server" Visible="false"></asp:Label>
                              
				                <fieldset>

                                    
                                    <div class="toggle toggle-transparent-body">
                                        
                                        <div class="toggle active">
                                         <label>Mandatory</label>
                                         <div class="toggle-content">
						                 <div class="row">
							            <div class="form-group">
								            <div class="col-md-4 col-sm-4">
                                                <div class="fancy-form">
                                                    <asp:DropDownList ID="ddlMode" runat="server" CssClass="form-control ddlColor" ToolTip="Mode" TabIndex="1">
                                                        <asp:ListItem Value="0" Selected="True">Mode</asp:ListItem>
                                                        <asp:ListItem Value="1" Selected="False">Air</asp:ListItem>
                                                        <asp:ListItem Value="2" Selected="False">Sea</asp:ListItem>
                                                    </asp:DropDownList>
                                                   <span class="fancy-tooltip top-right"> <!-- positions: .top-left | .top-right -->
		                                                <em>Select Mode </em>
	                                               </span>
                                                </div>
                                                <asp:RequiredFieldValidator ID="rfvMode" ControlToValidate="ddlMode" SetFocusOnError="true"
                                                InitialValue="0" Display="Dynamic" runat="server" ValidationGroup="FormReq"></asp:RequiredFieldValidator>
								            </div>
								            <div class="col-md-4 col-sm-4">
                                               <div class="fancy-form">
                                                <asp:DropDownList ID="ddlTerms" runat="server" CssClass="form-control ddlColor" DataSourceID="DataSourceTerms" TabIndex="2"
                                                 DataValueField="lid" DataTextField="sName" AppendDataBoundItems="true">
                                                    <asp:ListItem Value="0" Selected="True">Terms</asp:ListItem>      
                                                </asp:DropDownList>
                                                <span class="fancy-tooltip top-right"> <!-- positions: .top-left | .top-right -->
		                                              <em>Select Terms </em>
	                                            </span>
                                               </div>
                                                <asp:RequiredFieldValidator ID="rfvTerms" ControlToValidate="ddlTerms" SetFocusOnError="true"
                                                InitialValue="0" Display="Dynamic" runat="server" ValidationGroup="FormReq"></asp:RequiredFieldValidator>
								            </div>
                                            <div class="col-md-4 col-sm-4">
                                           
                                                 <asp:TextBox ID="txtCustomer" runat="server" TabIndex="3" placeholder="Customer" CssClass="form-control SearchTextbox"></asp:TextBox>                           
                                            </div>
							            </div>
						            </div>

                                         <div class="row">
                                       <div class="form-group">
	                                        <div class="col-md-4 col-sm-4">
                                            
                                                   <asp:TextBox ID="txtConsignee" runat="server" TabIndex="4" CssClass="form-control SearchTextbox" placeholder="Consignee"></asp:TextBox>
                                             </div>                                            
	                                        <div class="col-md-4 col-sm-4">
                                              
                                                 <asp:TextBox ID="txtPortLoading" runat="server" CssClass=" form-control SearchTextbox" placeholder="Port Of Loading" TabIndex="5" ToolTip="Port Of Loading"></asp:TextBox>
                                                 <asp:HiddenField ID="hdnLoadingPortId" runat="server" Value="0" />
                                                 <div id="divwidthLoadingPort"></div>
                                                 <cc1:AutoCompleteExtender ID="AutoCompletePortLoading" runat="server" TargetControlID="txtPortLoading"
                                                    CompletionListElementID="divwidthLoadingPort" ServicePath="../WebService/PortOfLoadingAutoComplete.asmx"
                                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthLoadingPort"
                                                    ContextKey="1267" UseContextKey="True" OnClientItemSelected="OnPortOfLoadingSelected"
                                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                                 </cc1:AutoCompleteExtender>         
	                                       </div>                                  
	                                        <div class="col-md-4 col-sm-4">
                                            
                                                <asp:TextBox ID="txtPortOfDischarged" runat="server" CssClass="form-control SearchTextbox" placeholder="Port Of Discharged" TabIndex="6" ToolTip="Port Of Loading"></asp:TextBox>
                                                <asp:HiddenField ID="hdnPortOfDischargedId" runat="server" Value="0" />
                                                <div id="divwidthDischargPort"></div>
                                                <cc1:AutoCompleteExtender ID="AutoCompletePortOfDischarged" runat="server" TargetControlID="txtPortOfDischarged"
                                                    CompletionListElementID="divwidthDischargPort" ServicePath="../WebService/PortOfLoadingAutoComplete.asmx"
                                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthDischargPort"
                                                    ContextKey="7268" UseContextKey="True" OnClientItemSelected="OnPortOfDischargedSelected"
                                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight" DelimiterCharacters="" Enabled="True">
                                                </cc1:AutoCompleteExtender>
	                                        </div>                                              
                                       </div>
                                    </div>                                    

                                         <div class="row">
                                        <div class="form-group">
	                                        <div class="col-md-4 col-sm-4">
                                               <div class="fancy-form">
                                                <asp:TextBox ID="txtCommodity" runat="server" CssClass="form-control" placeholder="Commodity" TabIndex="7"></asp:TextBox>
                                                <span class="fancy-tooltip top-right"> <!-- positions: .top-left | .top-right -->
		                                             <em>Enter Commodity</em>
	                                            </span>
                                               </div>
                                                <asp:RequiredFieldValidator ID="rfvcommodity" ControlToValidate="txtCommodity" SetFocusOnError="true"
                                                Display="Dynamic" runat="server" ValidationGroup="FormReq"></asp:RequiredFieldValidator>
	                                        </div>	
                                            <div class="col-md-2 col-sm-2" style="padding-right:2px">
                                              <div class="fancy-form">
                                                <asp:TextBox ID="txtWeight" runat="server" CssClass="form-control" placeholder="Weight" TabIndex="8"
                                                    type="number"></asp:TextBox>
                                                <span class="fancy-tooltip top-right"> <!-- positions: .top-left | .top-right -->
		                                             <em>Enter Weight (in Kgs)</em>
	                                            </span>
                                              </div>
                                                <asp:RequiredFieldValidator ID="rfvweight" ControlToValidate="txtWeight" SetFocusOnError="true"
                                                    Display="Dynamic" runat="server" ValidationGroup="FormReq"></asp:RequiredFieldValidator>
	                                        </div>
                                            <div class="col-md-2 col-sm-2" style="padding-left:2px">
                                              <div class="fancy-form">
                                                <asp:TextBox ID="txtNoofPkg" runat="server" CssClass="form-control" placeholder="No Of Pkg" TabIndex="9"
                                                    type="number"></asp:TextBox>
                                                <span class="fancy-tooltip top-right"> <!-- positions: .top-left | .top-right -->
		                                             <em>Enter No Of Packages</em>
	                                            </span>
                                              </div>
                                                <asp:RequiredFieldValidator ID="rfvpkg" ControlToValidate="txtNoofPkg" SetFocusOnError="true"
                                                    Display="Dynamic" runat="server" ValidationGroup="FormReq"></asp:RequiredFieldValidator>      
	                                        </div>                                       
                                            <div class="col-md-2 col-sm-2" style="padding-right:2px; width:12%">
                                              <div class="fancy-form">
                                                 <asp:TextBox ID="txtLength" runat="server" CssClass="form-control" placeholder="Length" TabIndex="10"></asp:TextBox>
                                                 <span class="fancy-tooltip top-right"> <!-- positions: .top-left | .top-right -->
		                                             <em>Enter Dimension Length (in cms)</em>
	                                            </span>
                                              </div>
                                                 <asp:RequiredFieldValidator ID="rfvlength" ControlToValidate="txtLength"
                                                 SetFocusOnError="true" Display="Dynamic" runat="server" ValidationGroup="FormReq"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-1 col-sm-1" style="padding-right:1px; padding-left:0px; width:10%">
                                              <div class="fancy-form">
                                                <asp:TextBox ID="txtWidth" runat="server" CssClass="form-control" placeholder="Width" TabIndex="11"></asp:TextBox>
                                                <span class="fancy-tooltip top-right"> <!-- positions: .top-left | .top-right -->
		                                             <em>Enter Dimension Width (in cms)</em>
	                                            </span>
                                              </div>
                                                <asp:RequiredFieldValidator ID="rfvwidth" ControlToValidate="txtWidth"
                                                SetFocusOnError="true" Display="Dynamic" runat="server" ValidationGroup="FormReq"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-1 col-sm-1" style="padding-left:0px; width:10%; padding-right:0px">
                                              <div class="fancy-form">
                                                <asp:TextBox ID="txtHeight" runat="server" CssClass="form-control" placeholder="Height" TabIndex="12"></asp:TextBox>
                                                <span class="fancy-tooltip top-right"> <!-- positions: .top-left | .top-right -->
		                                             <em>Enter Dimension Height (in cms)</em>
	                                            </span>
                                              </div>
                                                <asp:RequiredFieldValidator ID="rfvheight" ControlToValidate="txtHeight"
                                                SetFocusOnError="true" Display="Dynamic" runat="server" ValidationGroup="FormReq"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>

                                         <div class="row">
                                        <div class="form-group">
                                            <div class="col-md-4 col-sm-4">
                                               <div class="fancy-form">
                                                <asp:DropDownList ID="ddlDgHaz" runat="server" CssClass="form-control ddlColor" TabIndex="13">
                                                    <asp:ListItem Value="0" Selected="True">Is DG Goods?</asp:ListItem>
                                                    <asp:ListItem Value="1" Selected="False">Yes</asp:ListItem>
                                                    <asp:ListItem Value="2" Selected="False">No</asp:ListItem>
                                                </asp:DropDownList>
                                                <span class="fancy-tooltip top-right"> <!-- positions: .top-left | .top-right -->
		                                             <em>Is Dangerous Goods?</em>
	                                            </span>
                                               </div>
                                                <asp:RequiredFieldValidator ID="rfvDG" ControlToValidate="ddlDgHaz" InitialValue="0"
                                                SetFocusOnError="true" Display="Dynamic" runat="server" ValidationGroup="FormReq"></asp:RequiredFieldValidator>
                                            </div>
	                                        
	                                        <div class="col-md-2 col-sm-2" style="padding-right:2px">
                                              <div class="fancy-form">
                                                <asp:TextBox ID="txtHsCode" runat="server" CssClass="form-control" placeholder="HS Code"
                                                    type="number" TabIndex="14"></asp:TextBox>
                                                <span class="fancy-tooltip top-right"> <!-- positions: .top-left | .top-right -->
		                                             <em>Enter HS Code</em>
	                                            </span>
                                              </div>
                                                <asp:RequiredFieldValidator ID="rfvhscode" ControlToValidate="txtHsCode" SetFocusOnError="true"
                                                    Display="Dynamic" runat="server" ValidationGroup="FormReq"></asp:RequiredFieldValidator>
	                                        </div>
                                            <div class="col-md-2 col-sm-2" style="padding-left:2px">
                                              <div class="fancy-form">
                                                <asp:TextBox ID="txtInvcValue" runat="server" CssClass="form-control" TabIndex="15" placeholder="Invoice Value"
                                                    type="number"></asp:TextBox>
                                                <span class="fancy-tooltip top-right"> <!-- positions: .top-left | .top-right -->
		                                             <em>Enter Invoice Value</em>
	                                            </span>
                                              </div>
                                                <asp:RequiredFieldValidator ID="rfvinvcvalue" ControlToValidate="txtInvcValue" SetFocusOnError="true"
                                                    Display="Dynamic" runat="server" ValidationGroup="FormReq"></asp:RequiredFieldValidator>
                                            </div>
                                           <div class="col-md-4 col-sm-4">
                                              <div class="fancy-form">
                                                <asp:TextBox ID="txtShipper" runat="server" CssClass="form-control" placeholder="Shipper" TabIndex="16"></asp:TextBox>
                                                <span class="fancy-tooltip top-right"> <!-- positions: .top-left | .top-right -->
		                                             <em>Enter Shipper</em>
	                                            </span> 
                                              </div>
                                                <asp:RequiredFieldValidator ID="rfvShipper" ControlToValidate="txtShipper" SetFocusOnError="true"
                                                Display="Dynamic" runat="server" ValidationGroup="FormReq"></asp:RequiredFieldValidator>   
                                           </div>
                                        </div>
                                    </div>

                                         <div class="row">
                                       <div class="form-group">
                                         
                                           <div class="col-md-8 col-sm-8">
                                               <div class="fancy-form">
                                                <asp:TextBox ID="txtShipperAddress" runat="server" CssClass="form-control" placeholder="Shipper Address ..." TabIndex="17"
                                                TextMode="MultiLine" Rows="2"></asp:TextBox>
                                                 <span class="fancy-tooltip top-right"> <!-- positions: .top-left | .top-right -->
		                                             <em>Enter Shipper Address</em>
	                                            </span> 
                                               </div>
                                                <asp:RequiredFieldValidator ID="rfvShipperAdd" ControlToValidate="txtShipperAddress"
                                                SetFocusOnError="true" Display="Dynamic" runat="server" ValidationGroup="FormReq"></asp:RequiredFieldValidator>                                         
	                                        </div>  
                                            <div class="col-md-4 col-sm-4">
                                                <div class="fancy-form">
                                                    <asp:DropDownList ID="ddlIEC" runat="server" CssClass="form-control ddlColor" TabIndex="18">
                                                    <asp:ListItem Value="0" Selected="True">IEC</asp:ListItem>
                                                    <asp:ListItem Value="1" Selected="False">Yes</asp:ListItem>
                                                    <asp:ListItem Value="2" Selected="False">No</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <span class="fancy-tooltip top-right"> <!-- positions: .top-left | .top-right -->
                                                        <em>Select IEC </em>
                                                    </span> 
                                                </div>
                                                  <asp:RequiredFieldValidator ID="rfvIEC" ControlToValidate="ddlIEC" SetFocusOnError="true"
                                                    InitialValue="0" Display="Dynamic" runat="server" ValidationGroup="FormReq"></asp:RequiredFieldValidator>
                                            </div>                                         
                                       </div>
                                    </div>

                                    <div class="row">
                                       <div class="form-group">
                                         <div class="col-md-8 col-sm-8">
                                              <div class="fancy-form">
                                                <asp:TextBox ID="txtDeliveryAdd" runat="server" CssClass="form-control" TabIndex="19" placeholder="Delivery Address ..."
                                                    TextMode="MultiLine" Rows="2"></asp:TextBox>
                                                <span class="fancy-tooltip top-right"> <!-- positions: .top-left | .top-right -->
		                                             <em>Enter Delivery Address</em>
	                                            </span>
                                              </div>
                                                <asp:RequiredFieldValidator ID="rfvDeliveryAdd" ControlToValidate="txtDeliveryAdd"
                                                    SetFocusOnError="true" Display="Dynamic" runat="server" ValidationGroup="FormReq"></asp:RequiredFieldValidator>
	                                     </div>  
                                         <div class="col-md-4 col-sm-4">
                                                 <div class="fancy-form">
                                                    <asp:DropDownList ID="ddlShipmentType" runat="server" CssClass="form-control ddlColor" TabIndex="20">
                                                        <asp:ListItem Value="0" Selected="True">Shipment Type</asp:ListItem>
                                                        <asp:ListItem Value="1" Selected="False">FCL</asp:ListItem>
                                                        <asp:ListItem Value="2" Selected="False">LCL</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <span class="fancy-tooltip top-right"> <!-- positions: .top-left | .top-right -->
		                                                 <em>Select Shipment Type</em>
	                                                </span>
                                                 </div>
                                                <asp:RequiredFieldValidator ID="rfvShipmentType" ControlToValidate="ddlShipmentType"
                                                InitialValue="0" SetFocusOnError="true" Display="Dynamic" runat="server" ValidationGroup="FormReq"></asp:RequiredFieldValidator>
                                            </div>                                                                                    
                                       </div>
                                    </div>

                                         </div>
                                        </div>
                                        <div class="toggle">
                                            <label>Optional</label>
                                            <div class="toggle-content">
                                                <div class="row">
                                                    <div class="form-group">
                                                        <div class="col-md-4 col-sm-4">
                                                            <div class="fancy-form">
                                                                <asp:TextBox ID="txtCont20" runat="server" CssClass="form-control" placeholder="Container 20" TabIndex="21"
                                                                    type="number"></asp:TextBox>   
                                                                <span class="fancy-tooltip top-right"> <!-- positions: .top-left | .top-right -->
                                                                    <em>Enter Container 20</em>
                                                                </span>
                                                            </div>                                           
                                                        </div>
                                                        <div class="col-md-4 col-sm-4">
                                                            <div class="fancy-form">
                                                                <asp:TextBox ID="txtCont40" runat="server" CssClass="form-control" placeholder="Container 40" TabIndex="22"
                                                                    type="number"></asp:TextBox>
                                                                <span class="fancy-tooltip top-right"> <!-- positions: .top-left | .top-right -->
                                                                    <em>Enter Container 40</em>
                                                                </span>
                                                            </div>                                                     
                                                        </div>	
                                                                                                                                        
                                                        <div class="col-md-4 col-sm-4">
                                                            <div class="fancy-form">
                                                                <asp:TextBox ID="txtQty" runat="server" CssClass="form-control" placeholder="Quantity" TabIndex="23"
                                                                    type="number"></asp:TextBox>
                                                                <span class="fancy-tooltip top-right"> <!-- positions: .top-left | .top-right -->
                                                                    <em>Enter Quantity</em>
                                                                </span>
                                                            </div>
                                                        </div>
                                                                                                                                                                        
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="form-group">
	                                                     <div class="col-md-4 col-sm-4">
                                                            <div class="fancy-form">
                                                                <asp:TextBox ID="txtShipperPincode" runat="server" CssClass="form-control" placeholder="Shipper Pin Code" TabIndex="24"
                                                                    type="number"></asp:TextBox>
                                                                <span class="fancy-tooltip top-right"> <!-- positions: .top-left | .top-right -->
                                                                    <em>Enter Shipper Pin Code</em>
                                                                </span>
                                                            </div>
                                                        </div> 
                                                        <div class="col-md-4 col-sm-4">
                                                            <div class="fancy-form">  
                                                                <asp:TextBox ID="txtDeliveryAddPincode" runat="server" TabIndex="25" CssClass="form-control masked" data-format="999999" data-placeholder="X"
                                                                    placeholder="Delivery Address Pin Code"></asp:TextBox>
                                                                <span class="fancy-tooltip top-right"> <!-- positions: .top-left | .top-right -->
                                                                    <em>Enter Delivery Address Pin Code</em>
                                                                </span>
                                                            </div>
                                                        </div> 
                                                        <div class="col-md-4 col-sm-4">
                                                            <div class="fancy-form">
                                                                <asp:TextBox ID="txtProductLink" runat="server" CssClass="form-control" placeholder="Product Link" TabIndex="26"></asp:TextBox>
                                                                <span class="fancy-tooltip top-right"> <!-- positions: .top-left | .top-right -->
                                                                    <em>Enter Product Link</em>
                                                                </span>
                                                            </div>
                                                        </div>                 
                                                    </div>
                                                </div>   
                                            </div>
                                        </div>                                        
                                    </div>                                                                   
                                    
                                    <div class="row" style="height: 35px">
                                    </div>

                                    <div class="row">
                                        <div class="form-group">
	                                        <div class="col-md-4 col-sm-4">
                                             <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save Enquiry" TabIndex="27"
                                               OnClientClick="return ValidatePage();"  ValidationGroup="FormReq" OnClick="btnSave_OnClick" />
	                                        </div>
	                                        <div class="col-md-4 col-sm-4">
                                                
	                                        </div>
                                            <div class="col-md-4 col-sm-4">
                                             
                                            </div>
                                        </div>
                                    </div>

					            </fieldset>	
                                <asp:SqlDataSource ID="DataSourcePOL" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="FR_GetPortOfLoading" SelectCommandType="StoredProcedure"></asp:SqlDataSource>  
                               
                                <asp:SqlDataSource ID="DataSourceTerms" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="FR_GetTermsMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>  	            
		                    </div>
	                    </div>
                    </div>						
                </div>
	        </div>
    </section>
    </div>
    <!-- JAVASCRIPT FILES -->
    <script type="text/javascript">
        var plugin_path = 'FR_assets/plugins/';
    </script>
    <script type="text/javascript" src="FR_assets/plugins/jquery/jquery-2.1.4.min.js"></script>
    <script type="text/javascript" src="FR_assets/js/scripts.js"></script>
    <script src="FR_assets/js/app.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function ValidatePage() {
            ValidateDropdowns();

            var Shipper = document.getElementById("ctl00_ContentPlaceHolder1_rfvShipper");
            var ShipperAddress = document.getElementById("ctl00_ContentPlaceHolder1_rfvShipperAdd");
            var ShipmentType = document.getElementById("ctl00_ContentPlaceHolder1_rfvShipmentType");
            var HsCode = document.getElementById("ctl00_ContentPlaceHolder1_rfvhscode");
            var InvoiceValue = document.getElementById("ctl00_ContentPlaceHolder1_rfvinvcvalue");
            var DeliveryAddress = document.getElementById("ctl00_ContentPlaceHolder1_rfvDeliveryAdd");
            var newLine = "\r\n";
            var Messages = "";

            if (document.getElementById("ctl00_ContentPlaceHolder1_ddlMode").value != "" && document.getElementById("ctl00_ContentPlaceHolder1_ddlMode").value != "0" &&
                document.getElementById("ctl00_ContentPlaceHolder1_ddlTerms") != "" && document.getElementById("ctl00_ContentPlaceHolder1_ddlTerms") != "0") {

                if (document.getElementById("ctl00_ContentPlaceHolder1_ddlMode").value == "1")              // If Mode is Air
                {
                    if (document.getElementById("ctl00_ContentPlaceHolder1_ddlTerms").value == "1")         // If Terms is EXW
                    {
                        //ValidatorEnable(Cont20, false);
                        //ValidatorEnable(Cont40, false);
                        ValidatorEnable(HsCode, false);
                        ValidatorEnable(ShipmentType, false);
                        ValidatorEnable(InvoiceValue, false);
                        ValidatorEnable(DeliveryAddress, false);
                        ValidatorEnable(Shipper, true);
                        ValidatorEnable(ShipperAddress, true);
                        if (typeof (Page_ClientValidate) == "function") {
                            Page_ClientValidate('FormReq');
                        }
                    }
                    if (document.getElementById("ctl00_ContentPlaceHolder1_ddlTerms").value == "5")         // If Terms is FOB
                    {
                        //ValidatorEnable(Cont20, false);
                        //ValidatorEnable(Cont40, false);
                        ValidatorEnable(HsCode, false);
                        ValidatorEnable(ShipmentType, false);
                        ValidatorEnable(InvoiceValue, false);
                        ValidatorEnable(DeliveryAddress, false);
                        ValidatorEnable(Shipper, false);
                        ValidatorEnable(ShipperAddress, false);
                        if (typeof (Page_ClientValidate) == "function") {
                            Page_ClientValidate('FormReq');
                        }
                    }
                    if (document.getElementById("ctl00_ContentPlaceHolder1_ddlTerms").value == "8")         // If Terms is DAP
                    {
                        //ValidatorEnable(Cont20, false);
                        //ValidatorEnable(Cont40, false);
                        ValidatorEnable(ShipmentType, false);
                        ValidatorEnable(HsCode, false);
                        ValidatorEnable(InvoiceValue, false);
                        ValidatorEnable(DeliveryAddress, false);
                        ValidatorEnable(Shipper, true);
                        ValidatorEnable(ShipperAddress, true);
                        if (typeof (Page_ClientValidate) == "function") {
                            Page_ClientValidate('FormReq');
                        }
                    }
                    if (document.getElementById("ctl00_ContentPlaceHolder1_ddlTerms").value == "3")         // If Terms is DDP
                    {
                        //ValidatorEnable(Cont20, false);
                        //ValidatorEnable(Cont40, false);
                        ValidatorEnable(ShipmentType, false);

                        ValidatorEnable(HsCode, true);
                        ValidatorEnable(InvoiceValue, true);
                        ValidatorEnable(DeliveryAddress, true);
                        ValidatorEnable(Shipper, true);
                        ValidatorEnable(ShipperAddress, true);
                        if (typeof (Page_ClientValidate) == "function") {
                            Page_ClientValidate('FormReq');
                        }
                    }
                    if (document.getElementById("ctl00_ContentPlaceHolder1_ddlTerms").value == "2")         // If Terms is DDU
                    {
                        //ValidatorEnable(Cont20, false);
                        //ValidatorEnable(Cont40, false);
                        ValidatorEnable(ShipmentType, false);

                        ValidatorEnable(HsCode, true);
                        ValidatorEnable(InvoiceValue, true);
                        ValidatorEnable(DeliveryAddress, true);
                        ValidatorEnable(Shipper, true);
                        ValidatorEnable(ShipperAddress, true);
                        if (typeof (Page_ClientValidate) == "function") {
                            Page_ClientValidate('FormReq');
                        }
                    }
                }
                if (document.getElementById("ctl00_ContentPlaceHolder1_ddlMode").value == "2")              // If Mode is Air
                {
                    if (document.getElementById("ctl00_ContentPlaceHolder1_ddlTerms").value == "1")         // If Terms is EXW
                    {
                        ValidatorEnable(HsCode, false);
                        ValidatorEnable(InvoiceValue, false);
                        ValidatorEnable(DeliveryAddress, false);
                        ValidatorEnable(ShipmentType, true);
                        ValidatorEnable(Shipper, true);
                        ValidatorEnable(ShipperAddress, true);

                        if (typeof (Page_ClientValidate) == "function") {
                            Page_ClientValidate('FormReq');
                        }
                    }
                    if (document.getElementById("ctl00_ContentPlaceHolder1_ddlTerms").value == "5")         // If Terms is FOB
                    {
                        ValidatorEnable(HsCode, false);
                        ValidatorEnable(InvoiceValue, false);
                        ValidatorEnable(DeliveryAddress, false);

                        //ValidatorEnable(ContType, true);
                        ValidatorEnable(ShipmentType, true);
                        ValidatorEnable(Shipper, false);
                        ValidatorEnable(ShipperAddress, false);
                        if (typeof (Page_ClientValidate) == "function") {
                            Page_ClientValidate('FormReq');
                        }
                    }
                    if (document.getElementById("ctl00_ContentPlaceHolder1_ddlTerms").value == "8")         // If Terms is DAP
                    {
                        //ValidatorEnable(ContType, true);
                        ValidatorEnable(ShipmentType, true);
                        ValidatorEnable(HsCode, true);
                        ValidatorEnable(InvoiceValue, true);
                        ValidatorEnable(DeliveryAddress, true);
                        ValidatorEnable(Shipper, true);
                        ValidatorEnable(ShipperAddress, true);
                        if (typeof (Page_ClientValidate) == "function") {
                            Page_ClientValidate('FormReq');
                        }
                    }
                    if (document.getElementById("ctl00_ContentPlaceHolder1_ddlTerms").value == "3")         // If Terms is DDP
                    {
                        //ValidatorEnable(ContType, true);
                        ValidatorEnable(ShipmentType, true);
                        ValidatorEnable(HsCode, true);
                        ValidatorEnable(InvoiceValue, true);
                        ValidatorEnable(DeliveryAddress, true);
                        ValidatorEnable(Shipper, true);
                        ValidatorEnable(ShipperAddress, true);
                        if (typeof (Page_ClientValidate) == "function") {
                            Page_ClientValidate('FormReq');
                        }
                    }
                    if (document.getElementById("ctl00_ContentPlaceHolder1_ddlTerms").value == "2")         // If Terms is DDU
                    {
                        //ValidatorEnable(ContType, true);
                        ValidatorEnable(ShipmentType, true);
                        ValidatorEnable(HsCode, true);
                        ValidatorEnable(InvoiceValue, true);
                        ValidatorEnable(DeliveryAddress, true);
                        ValidatorEnable(Shipper, true);
                        ValidatorEnable(ShipperAddress, true);
                        if (typeof (Page_ClientValidate) == "function") {
                            Page_ClientValidate('FormReq');
                        }
                    }
                }
            }
            else {
                //ValidatorEnable(ContType, false);
                ValidatorEnable(ShipmentType, false);
                ValidatorEnable(HsCode, false);
                ValidatorEnable(InvoiceValue, false);
                ValidatorEnable(DeliveryAddress, false);
                ValidatorEnable(Shipper, false);
                ValidatorEnable(ShipperAddress, false);
                if (typeof (Page_ClientValidate) == "function") {
                    Page_ClientValidate('FormReq');
                }
            }

            if (checkValidationGroup("FormReq")) {
                return true;
            }
            else {
                return false;
            }

        }

        function checkValidationGroup(valGrp) {
            var rtnVal = true;

            for (i = 0; i < Page_Validators.length; i++) {

                if (Page_Validators[i].validationGroup == valGrp) {

                    ValidatorValidate(Page_Validators[i]);
                    var control = document.getElementById(Page_Validators[i].controltovalidate);
                    if (!Page_Validators[i].isvalid) { //at least one is not valid.
                        control.style.backgroundColor = "#FAEEEF";
                        control.style.borderWidth = "2px";
                        control.style.borderColor = "rgba(255, 0, 0, 0.48)";
                        rtnVal = false;
                    }
                    else {
                        control.removeAttribute("style");

                    }
                }
            }
            return rtnVal;
        }

        function ValidateDropdowns() {
            if (document.getElementById('ctl00_ContentPlaceHolder1_txtCustomer').value == "") {
                var Cust = document.getElementById('ctl00_ContentPlaceHolder1_txtCustomer');
                Cust.style.backgroundColor = "#FAEEEF";
                Cust.style.borderWidth = "2px";
                Cust.style.borderColor = "rgba(255, 0, 0, 0.48)";
            }
            else {
                document.getElementById('ctl00_ContentPlaceHolder1_txtCustomer').removeAttribute("style");
            }

            if (document.getElementById('ctl00_ContentPlaceHolder1_txtConsignee').value == "") {
                var Consg = document.getElementById('ctl00_ContentPlaceHolder1_txtConsignee');
                Consg.style.backgroundColor = "#FAEEEF";
                Consg.style.borderWidth = "2px";
                Consg.style.borderColor = "rgba(255, 0, 0, 0.48)";
            }
            else {
                document.getElementById('ctl00_ContentPlaceHolder1_txtConsignee').removeAttribute("style");
            }

            if (document.getElementById('ctl00_ContentPlaceHolder1_txtPortLoading').value == "") {
                var POL = document.getElementById('ctl00_ContentPlaceHolder1_txtPortLoading');
                POL.style.backgroundColor = "#FAEEEF";
                POL.style.borderWidth = "2px";
                POL.style.borderColor = "rgba(255, 0, 0, 0.48)";
            }
            else {
                document.getElementById('ctl00_ContentPlaceHolder1_txtPortLoading').removeAttribute("style");
            }

            if (document.getElementById('ctl00_ContentPlaceHolder1_txtPortOfDischarged').value == "") {
                var POD = document.getElementById('ctl00_ContentPlaceHolder1_txtPortOfDischarged');
                POD.style.backgroundColor = "#FAEEEF";
                POD.style.borderWidth = "2px";
                POD.style.borderColor = "rgba(255, 0, 0, 0.48)";
            }
            else {
                document.getElementById('ctl00_ContentPlaceHolder1_txtPortOfDischarged').removeAttribute("style");
            }

        }

    </script>
    <!-- MODAL POPUP EVENTS -->
    <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenHL"
        PopupControlID="pnlpopup" CancelControlID="btnCancelPopup" BackgroundCssClass="modalBackground"
        X="700" Y="160">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlpopup" runat="server" Style="display: none;">
        <%-- <div style="padding: 10px">
            <div class="container" style="background-color: rgba(0,0,0,0.80); padding: 17px;
                width: 100%">--%>
        <div class="row">
            <div class="col-md-12 col-sm-12">
                <div class="panel panel-info">
                    <div class="panel-heading">
                        MESSAGING <span class="pull-right">
                            <asp:Button ID="btnCancel" runat="server" OnClick="btnCancelPopup_OnClick" Text="Close"
                                CssClass="btn-xs btn-3d btn-default" CausesValidation="false" />
                            &nbsp;
                            <asp:Button ID="btnMinMaxPopup" runat="server" Text="Minimize" OnClientClick="return MinimizeMsg();"
                                CssClass="btn-xs btn-3d btn-default" CausesValidation="false" />
                        </span>
                    </div>
                    <div id="dvMsgAll" runat="server">
                    </div>
                    <div class="panel-body" style="background-color: rgba(66, 139, 202, 0.16); padding-bottom: 1px">
                        <%--<div class="col-md-8">--%>
                        <div class="col-md-8 col-sm-8" style="padding-right: 2px; padding-left: 2px">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    RECENT CHAT HISTORY
                                </div>
                                <div id="dvRecentChats" runat="server" class="panel-body" style="background-color: White">
                                </div>
                                <div class="panel-footer">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtMessage" runat="server" CssClass="form-control" placeholder="Enter Message"></asp:TextBox>
                                        <span class="input-group-btn">
                                            <asp:Button ID="btnSendMsg" runat="server" Text="SEND" Height="34px" CssClass="btn btn-info"
                                                OnClick="btnSendMsg_OnClick" />
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4" style="padding-right: 2px; padding-left: 2px">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    USERS
                                </div>
                                <div id="dvOnlineUsers" runat="server" class="panel-body" style="background-color: White">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%-- </div>
        </div>--%>
    </asp:Panel>
    <asp:HyperLink ID="HiddenHL" runat="server" />
    <asp:Button ID="btnCancelPopup" runat="server" OnClick="btnCancelPopup_OnClick" CausesValidation="false" />
    <!-- /MODAL POPUP EVENTS -->
</asp:Content>
