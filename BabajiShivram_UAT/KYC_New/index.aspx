<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="Vendor1_22" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<script type="text/javascript">


    //Created / Generates the captcha function    
    function DrawCaptcha() {
        var a = Math.ceil(Math.random() * 10) + '';
        var b = Math.ceil(Math.random() * 10) + '';
        var c = Math.ceil(Math.random() * 10) + '';
        var d = Math.ceil(Math.random() * 10) + '';
        var e = Math.ceil(Math.random() * 10) + '';
        var f = Math.ceil(Math.random() * 10) + '';
        var g = Math.ceil(Math.random() * 10) + '';
        var code = a + ' ' + b + ' ' + ' ' + c + ' ' + d + ' ' + e + ' ' + f + ' ' + g;
        document.getElementById("txtCaptcha").value = code

        
    }

    // Validate the Entered input aganist the generated security code function   
    function ValidCaptcha() {
        var str1 = removeSpaces(document.getElementById('txtCaptcha').value);
        var str2 = removeSpaces(document.getElementById('txtInput').value);


        if (str1 == str2) 
        {

           
            return true;
            
        }
        else {

            alert("Invalid Captcha");
            return false;
        }
        
    }

    // Remove the spaces from the entered and generated code
    function removeSpaces(string) {
        return string.split(' ').join('');
    }
    
 
</script>
<html>

<head>
</head>


<body onload="DrawCaptcha();" style="background-color: lightblue; background-image: url(a.gif)">

    <form runat="server">
        <center>
            <br />
            <br />
            <br />
            <br />
            <br />
            <table border="3px" style="border-collapse: separate; background-image: url(e.jpg); height: 210px" cellpadding="2px" cellspacing="10px" width="500px" bgcolor="lightblue" class="table-responsive">
                <tr>
                    <td>Please Enter Text To Proceed For KYC
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>


                        <input type="text" id="txtCaptcha" style="background-image: url(1.png); text-align: center; border: none; font-weight: bold; font-size: large; font-family: Modern"
                            class="form-control" />

                        <input type="button" id="btnrefresh" st onclick="DrawCaptcha();" value="Refresh"
                            style="background-color: #337ab7; outline-color: Aqua; font-size: 15px; height: 45px; width: 65px; font-family: @Meiryo; background-image: url(c.png);" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <input type="text" id="txtInput" class="form-control" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <%--<asp:LinkButton ID="Button1"  onclick="alert(ValidCaptcha());" PostBackUrl="KYC.aspx" >LinkButton</asp:LinkButton>--%>
                        <asp:Button ID="Button1" runat="server" Text="Submit"
                            OnClientClick="return ValidCaptcha();" OnClick="Button1_Click"
                            Style="background-color: #337ab7; filter: alpha(opacity=50); outline-color: Aqua; background-image: url(d.png)"
                            Font-Size="15px" Height="37px" Width="75px" />
                        <%--<input id="Button1" type="button" value="Check"   onclick="alert(ValidCaptcha());"  />--%>
                    </td>
                </tr>
            </table>
        </center>
    </form>
</body>

</html>
