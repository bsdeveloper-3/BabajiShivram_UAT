using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DynamicData_Content_UCDelivery : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        MEditValExaminDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");
        MEditValOutDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");
    }

    protected void ddTransitType_SelectedIndexChanged(object sender, EventArgs e)
    {
        WarehouseVisibility();
    }

    private void WarehouseVisibility()
    {
        int BranchId = Convert.ToInt32(hdnBranchId.Value);

        if (ddTransitType.SelectedValue == "1") //Move to Customer Place
        {
            ddWarehouse.Visible = false;
            RFVBonded.Enabled = false;
        }
        else if (ddTransitType.SelectedValue == "2") //Move To General Warehouse
        {
            DBOperations.FillWarehouse(ddWarehouse, (Int32)EnumWarehouseType.General, BranchId);
            ddWarehouse.Visible = true;
            RFVBonded.Enabled = true;
        }
        else if (ddTransitType.SelectedValue == "3") //Move To In Bonded Warehouse
        {
            DBOperations.FillWarehouse(ddWarehouse, (Int32)EnumWarehouseType.Bonded, BranchId);
            ddWarehouse.Visible = true;
            RFVBonded.Enabled = true;
        }

    }
}