<%@ Page Title="E-WayBill Entry Form" Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master"
    CodeFile="EwayTest.aspx.cs" Inherits="Transport_EwayTest" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>
    <script src="../JS/jquery-1.9.1.min.js"></script>
    <script src="../JS/jquery-ui.js"></script>
    <script src="../KYC/vendors/bootstrap/dist/js/bootstrap.min.js"></script>
    
    
    <script type="text/javascript">
        function CheckSubType() {
            var vType = document.getElementById("<%=rdlTransType.ClientID%>");
            var vItem = vType.getElementsByTagName("Input");

            var vInward = document.getElementById("divInSubType");
            var vOutward = document.getElementById('divOutSubType');

            // Inward Check
            if (vItem[0].checked) {
                vInward.style = "display:block";
                vOutward.style = "display:none";
            }
            else {
                vInward.style = "display:none";
                vOutward.style = "display:block";
            }
        }

    </script>
    
    <script type="text/javascript">
        var rowCount = 1;
        $('#add_product').click(function () {
            alert('test');
            try {
                var div = $('#tblProducts tr:last').clone().insertAfter('#tblProducts tbody>tr:last');
                rowCount++;
                $(div).find("td").each(function () {

                    $(this).find("input:text").each(function () {
                        var name = $(this).attr("id");
                        if (name.lastIndexOf("_") > 0) {
                            name = name.substr(0, name.indexOf("_"));
                        }
                        name += "_" + rowCount;
                        $(this).attr("id", name);
                        $(this).val("");
                    });

                    $(this).find("select").each(function () {
                        var name = $(this).attr("id");
                        if (name.lastIndexOf("_") > 0) {
                            name = name.substr(0, name.indexOf("_"));
                        }
                        name += "_" + rowCount;
                        $(this).attr("id", name);
                    });
                });

            }
            catch (err) {
                $.alert(err.message)
            }
        });
    </script>
    <div class="container1">
        <div class="ewbLayout">
            <table class="table-condensed table-responsive table_css" id="HeadTable">
                <tbody>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <fieldset>
                                <legend>Transaction Detail</legend>
                                <table class="table" style="margin-bottom: 0px; margin-top: 0px">
                                    <tbody>
                                        <tr>
                                            <td class="lbl_css" style="background-color: #dadee0; border: 1px  solid #a2aaae !important; border-radius: 10px 0px 0px 10px; border-right: none !important; width: 133px">Transaction Type<span class="m-cir redcss"></span>
                                            </td>
                                            <td style="background-color: transparent; border: 1px  solid #a2aaae !important; border-radius: 0px 10px 10px 0px; border-left: none !important; width: 100px">
                                                <asp:RadioButtonList ID="rdlTransType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" onclick="Javascript:CheckSubType();">
                                                    <asp:ListItem Text="Inward" Value="I" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Outward" Value="O"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td class="lbl_css" style="background-color: #dadee0; border: 1px  solid #a2aaae !important; border-radius: 10px 0px 0px 10px; border-right: none !important;">Sub Type<span class="m-cir redcss"></span> </td>
                                            <td class="form-inline " style="background-color: transparent; border: 1px  solid #a2aaae !important; border-radius: 0px 10px 10px 0px; border-left: none !important;">
                                                <div id="divInSubType">
                                                    <asp:RadioButtonList ID="rdlInSubType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table">
                                                        <asp:ListItem Text="Import" Value="2" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="Supply" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Job Work" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="SKD/CKD" Value="9"></asp:ListItem>
                                                        <asp:ListItem Text="For Own Use" Value="5"></asp:ListItem>
                                                        <asp:ListItem Text="Exhibition or Fairs" Value="12"></asp:ListItem>
                                                        <asp:ListItem Text="Line Sales" Value="10"></asp:ListItem>
                                                        <asp:ListItem Text="Others" Value="8"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                                <div id="divOutSubType" style="display: none;">
                                                    <asp:RadioButtonList ID="rdlOutSubType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table">
                                                        <asp:ListItem Text="Export" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="Supply" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Job Work" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="SKD/CKD" Value="9"></asp:ListItem>
                                                        <asp:ListItem Text="Recipient Not Known" Value="11"></asp:ListItem>
                                                        <asp:ListItem Text="For Own Use" Value="5"></asp:ListItem>
                                                        <asp:ListItem Text="Exhibition or Fairs" Value="12"></asp:ListItem>
                                                        <asp:ListItem Text="Line Sales" Value="10"></asp:ListItem>
                                                        <asp:ListItem Text="Others" Value="8"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="margin-bottom: 0">
                            <fieldset>
                                <legend>Document Detail</legend>
                                <table class="table" style="margin-bottom: 0">
                                    <tbody>
                                        <tr>
                                            <td>Document Type 
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddDocumentType" runat="server">
                                                    <asp:ListItem Value="" Text="--Select--"></asp:ListItem>
                                                    <asp:ListItem Value="INV" Text="Tax Invoice"></asp:ListItem>
                                                    <asp:ListItem Value="BIL" Text="Bill of Supply"></asp:ListItem>
                                                    <asp:ListItem Value="BOE" Text="Bill of Entry"></asp:ListItem>
                                                    <asp:ListItem Value="CHL" Text="Delivery Challan"></asp:ListItem>
                                                    <asp:ListItem Value="CNT" Text="Credit Note"></asp:ListItem>
                                                    <asp:ListItem Value="OTH" Text="Others"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>Document No
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDocNo" MaxLength="16" runat="server"></asp:TextBox>
                                            </td>
                                            <td>Document Date
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDocDate" Style="width: 100px;" runat="server"></asp:TextBox>
                                                <cc1:CalendarExtender ID="calExtDocDate" runat="server" TargetControlID="txtDocDate" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <fieldset>
                                <legend>From</legend>
                                <table class="table" style="margin-bottom: 0">
                                    <tbody>
                                        <tr>
                                            <td>Name
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFromTrdName" runat="server"></asp:TextBox>
                                            </td>
                                            <td>Address
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFromAddr1" MaxLength="100" placeholder="Address 1" Style="width: 235px;" runat="server"></asp:TextBox>
                                                <asp:TextBox ID="txtFromAddr2" MaxLength="100" placeholder="Address 2" Style="width: 235px;" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Place
                                            </td>
                                            <td>
                                                <asp:TextBox MaxLength="50" ID="txtFromPlace" runat="server"></asp:TextBox>
                                            </td>
                                            <td>Pincode
                                            </td>
                                            <td>
                                                <asp:TextBox MaxLength="6" ID="txtFromPincode" Style="width: 80px;" runat="server"></asp:TextBox>
                                                <asp:DropDownList ID="ddlFromState" Style="width: 150px; font-size: 12px" runat="server">
                                                    <asp:ListItem Value="0">-State-</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>GSTIN
                                            </td>
                                            <td>
                                                <asp:TextBox MaxLength="15" ID="txtFromGSTIN" Style="text-transform: uppercase" runat="server"></asp:TextBox>
                                            </td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <fieldset>
                                <legend>To</legend>
                                <table class="table" style="margin-bottom: 0">
                                    <tbody>
                                        <tr>
                                            <td>Name
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtToTrdName" runat="server"></asp:TextBox>
                                            </td>
                                            <td>Address
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtToAddr1" MaxLength="100" placeholder="Address 1" Style="width: 235px;" runat="server"></asp:TextBox>
                                                <asp:TextBox ID="txtToAddr2" MaxLength="100" placeholder="Address 2" Style="width: 235px;" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Place
                                            </td>
                                            <td>
                                                <asp:TextBox MaxLength="50" ID="txtToPlace" runat="server"></asp:TextBox>
                                            </td>
                                            <td>Pincode
                                            </td>
                                            <td>
                                                <asp:TextBox MaxLength="6" ID="txtToPincode" Style="width: 80px;" runat="server"></asp:TextBox>
                                                <asp:DropDownList ID="ddlToState" Style="width: 150px; font-size: 12px" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>GSTIN
                                            </td>
                                            <td>
                                                <asp:TextBox MaxLength="15" ID="txtToGSTIN" Style="text-transform: uppercase" runat="server"></asp:TextBox>
                                            </td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <fieldset>
                                <legend>Item Details</legend>
                                <table class="Table table-responsive text-center bg-info " id="Tbl_products" style="margin-bottom: 0; width: 100%">
                                    <tr id="trClone">
                                        <td>
                                            <table id="tblProducts" class="table-condensed  table-responsive text-center bg-info" style="margin-bottom: 0">
                                                <thead>
                                                    <th>Product Name</th>
                                                    <th class="lbl_css">Description</th>
                                                    <th class="lbl_css">HSN</th>
                                                    <th class="lbl_css">Quantity </th>
                                                    <th class="lbl_css">Unit </th>
                                                    <th class="lbl_css">Value/Taxable Value (Rs.)</th>
                                                    <th colspan="2">Tax Rate(C+S+I+Cess)</th>
                                                </thead>
                                                <tbody>
                                                    <tr id="first_tr">
                                                        <td>
                                                            <asp:TextBox ID="txtProductName_1" placeholder="Name" MaxLength="100" Style="width: 180px" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_Description_1" placeholder="Description" MaxLength="100" Style="width: 180px" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_HSN_1" placeholder="HSN" MaxLength="10" Style="width: 80px" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_Quanity_1" Style="width: 100px" placeholder="Quantity" MaxLength="8" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_Unit_1" placeholder="Unit" MaxLength="3" Style="width: 60px" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_TRC_1" MaxLength="16" Style="width: 120px" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:TextBox ID="TextBox1" PlaceHolder="C" MaxLength="16" Style="width: 40px" runat="server"></asp:TextBox>
                                                            <asp:TextBox ID="TextBox2" PlaceHolder="S" MaxLength="16" Style="width: 40px" runat="server"></asp:TextBox>
                                                            <asp:TextBox ID="TextBox3" PlaceHolder="I" MaxLength="16" Style="width: 40px" runat="server"></asp:TextBox>
                                                            <asp:TextBox ID="TextBox4" PlaceHolder="CESS" MaxLength="16" Style="width: 40px" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class=" text-center pull-left">
                                            <div style="margin-left: 20px;">
                                                <button type="button" title="Add" id="add_product">
                                                  <img src="../images/addimg.jpg" alt="Add" style="width=22px; height:22px;" />
                                                </button>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <br /><br />
                                <table class="Table table-responsive text-center bg-info " id="Tbl_products1" style="margin-bottom: 0; width: 100%">
                                    <thead>
                                        <th>Total Amount/Tax'ble Amount</th>
                                        <th>CGST Amount</th>
                                        <th>SGST Amount</th>
                                        <th>IGST Amount</th>
                                        <th>CESS Amount</th>
                                        <th>Total Inv. Value</th>
                                    </thead>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtTotbasicval" MaxLength="18" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox type="text" ID="txtCGST" MaxLength="18" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSGST" MaxLength="18" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtIGST" MaxLength="18" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCess" MaxLength="18" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTotInvVal" MaxLength="18" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <fieldset>
                                <legend>Transporter Details</legend>
                                <table style="border-spacing: 10px;">
                                    <tbody>
                                        <tr>
                                            <td style="width: 100px; background-color: #dadee0; border: 1px  solid #a2aaae !important; border-radius: 10px 0px 0px 10px; border-right: none !important; padding-left: 10px" class="lbl_css">Mode
                                            </td>
                                            <td colspan="2" style="background-color: transparent; border: 1px  solid #a2aaae !important; border-radius: 10px 10px 10px 10px; border-left: none !important;">
                                                <asp:RadioButtonList ID="rdlTransMode" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table">
                                                    <asp:ListItem Text="Road" Value="1" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Rail" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Air" Value="3"></asp:ListItem>
                                                    <asp:ListItem Text="Ship" Value=""></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td style="text-align: right; padding-left: 20px;" class="lbl_css">Approximate Distance (in KM)</td>
                                            <td class=" form-inline ">&nbsp;<asp:TextBox ID="txtDistance" Style="width: 100px" MaxLength="9" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="font-size: 18px; font-weight: bold; color: maroon">Part - A
                                            </td>
                                            <td>Transporter Name
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransporter" placeholder="Name" MaxLength="100" runat="server"></asp:TextBox>

                                            </td>
                                            <td>Transporter ID
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransid" Style="text-transform: uppercase" MaxLength="15" Text="27AAACN1163G1ZR" runat="server"></asp:TextBox>
                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px; font-size: 18px; font-weight: bold; color: maroon">OR
                                            </td>
                                            <td>Transporter Doc. No.
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransDocNo" MaxLength="15" runat="server"></asp:TextBox>
                                            </td>
                                            <td>Transporter Doc. Date
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransDocDt" runat="server"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CalExtTransDocDate" runat="server" TargetControlID="txtTransDocDt" />
                                            </td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px; font-size: 18px; font-weight: bold; color: maroon">Part - B
                                            </td>
                                            <td>Vehicle No.
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtVehicleNo" MaxLength="20" Style="text-transform: uppercase" runat="server"></asp:TextBox>
                                            </td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btnEwayJson" runat="server" Text=" Generate Eway Bill" OnClick="btnEwayJson_Click" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</asp:Content>