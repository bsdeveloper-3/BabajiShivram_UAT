using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using AjaxControlToolkit;

public partial class Test_Transport : System.Web.UI.Page
{
    private static Random _random = new Random();
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Transport Request";
            lblRefNo.Text = DBOperations.GetNewTransportNo();

            DBOperations.FillVehicleType(ddVehicleType);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        /****************************************************/
        string strTRRefNo, strCustomer = "", strLocFrom = "", strLocTo = "", strRemarks = "";
        int TransportType, CustomerId;
        int Count20 = 0, Count40 = 0, NoOfpackages = 0;
        string strGrossWeight = "0";

        TransportType = 1; // New Transport Request

        strTRRefNo = lblRefNo.Text.Trim();
        strCustomer = "";// txtCustomer.Text.ToUpper().Trim();
        strLocFrom = txtFrom.Text.Trim();
        strLocTo = txtTo.Text.Trim();
        strRemarks = txtRemark.Text.Trim();

        if (strCustomer == "")
        {
            lblError.Text = "Please Search & Select Customer Name!.";
            lblError.CssClass = "success";
            return;
        }
        else
        {
            CustomerId = 0;// Convert.ToInt32(hdnCustId.Value);
        }

        if (txtNoOfPkgs.Text.Trim() != "")
        {
            NoOfpackages = Convert.ToInt32(txtNoOfPkgs.Text.Trim());
        }

        if (txtCont20.Text.Trim() != "")
        {
            Count20 = Convert.ToInt32(txtCont20.Text.Trim());
        }

        if (txtCont40.Text.Trim() != "")
        {
            Count40 = Convert.ToInt32(txtCont40.Text.Trim());
        }


        if (txtGrossWeight.Text.Trim() != "")
        {
            strGrossWeight = txtGrossWeight.Text.Trim();
        }

        int TransportId = DBOperations.AddNewTransportRequest(strTRRefNo, 0, TransportType, CustomerId, 0, 0, "", strLocFrom, strLocTo, Count20, Count40,
                NoOfpackages, strGrossWeight, strRemarks, 0, LoggedInUser.glUserId);

        if (TransportId > 0)
        {
            lblError.Text = "Transport Details Added Successfully!";
            lblError.CssClass = "success";
            // Redirect To Transport Success
            //Response.Redirect("FreightSuccess.aspx?FEnquiry=343");

        }//END_IF
        else if (TransportId == -2)
        {
            lblError.Text = " Transport Ref No " + lblRefNo.Text + " Already Created.";
            lblError.CssClass = "errorMsg";

        }
        else if (TransportId == -1)
        {
            lblError.Text = "System Error! Please check required fields.";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "System Error! Please check required fields.";
            lblError.CssClass = "errorMsg";
        }
        /***********************/
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        /****************************************************
        Response.Redirect("FreightTracking.aspx");
        ***********************/
    }

    protected void ddFreightMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        /****************************************************
        if (ddFreightMode.SelectedValue == "1") // For AIR
        {
            pnlAir.Visible = true;
            pnlSea.Visible = false;
        }
        else // For Sea and Breakbulk
        {
            pnlAir.Visible = false;
            pnlSea.Visible = true;
        }

        ***********************/
    }

    public string UploadDocument(string FilePath)
    {
        /****************************************************
        if (FilePath == "")
        {
            FilePath = "FreightDoc\\";
        }
        string FileName = fuAttachment.FileName;

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadFiles\\" + FilePath);
        }
        else
        {
            // File Server Path
            ServerFilePath = ServerFilePath + FilePath;
        }

        if (!System.IO.Directory.Exists(ServerFilePath))
        {
            System.IO.Directory.CreateDirectory(ServerFilePath);
        }

        if (fuAttachment.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(fuAttachment.FileName);
                FileName = Path.GetFileNameWithoutExtension(fuAttachment.FileName);
                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            fuAttachment.SaveAs(ServerFilePath + FileName);
        }

        return FilePath + FileName;

        ***********************/
        return "";
    }

    public string RandomString(int size)
    {
        /****************************************************
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < size; i++)
        {
            //26 letters in the alfabet, ascii + 65 for the capital letters
            builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65))));

        }
        return builder.ToString();

        ***********************/
        return "";
    }
}