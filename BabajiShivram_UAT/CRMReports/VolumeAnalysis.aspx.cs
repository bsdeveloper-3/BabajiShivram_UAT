using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using ClosedXML.Excel;

public partial class CRMReports_VolumeAnalysis : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkExport);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Volume Analysis";
        }
    }

    protected void ddlMode_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void gvImport_PreRender(object sender, EventArgs e)
    {
        if (gvImport.Rows.Count > 1)
        {
            gvImport.Rows[gvImport.Rows.Count - 1].BackColor = System.Drawing.Color.FromName("#CBCBDC");
            gvImport.Rows[gvImport.Rows.Count - 1].Cells[0].Text = "";
        }
    }

    protected void gvExport_PreRender(object sender, EventArgs e)
    {
        if (gvExport.Rows.Count > 1)
        {
            gvExport.Rows[gvExport.Rows.Count - 1].BackColor = System.Drawing.Color.FromName("#CBCBDC");
            gvExport.Rows[gvExport.Rows.Count - 1].Cells[0].Text = "";
        }
    }

    protected void gvFreight_PreRender(object sender, EventArgs e)
    {
        if (gvFreight.Rows.Count > 1)
        {
            gvFreight.Rows[gvFreight.Rows.Count - 1].BackColor = System.Drawing.Color.FromName("#CBCBDC");
            gvFreight.Rows[gvFreight.Rows.Count - 1].Cells[0].Text = "";
        }
    }

    protected void gvTransportation_PreRender(object sender, EventArgs e)
    {
        if (gvTransportation.Rows.Count > 1)
        {
            gvTransportation.Rows[gvTransportation.Rows.Count - 1].BackColor = System.Drawing.Color.FromName("#CBCBDC");
            gvTransportation.Rows[gvTransportation.Rows.Count - 1].Cells[0].Text = "";
        }
    }

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        using (XLWorkbook wb = new XLWorkbook())
        {
            gvImport.DataBind();
            gvExport.DataBind();
            gvFreight.DataBind();
            gvTransportation.DataBind();

            #region IMPORT_CHA
            if (gvImport.Rows.Count > 1)
            {
                DataTable dtImport = new DataTable("Import CHA");
                foreach (TableCell cell in gvImport.HeaderRow.Cells)
                {
                    dtImport.Columns.Add(cell.Text);
                }

                //Loop and add rows from GridView to DataTable.
                foreach (GridViewRow row in gvImport.Rows)
                {
                    dtImport.Rows.Add();
                    for (int j = 0; j < row.Cells.Count; j++)
                    {
                        dtImport.Rows[dtImport.Rows.Count - 1][j] = row.Cells[j].Text;
                    }
                }
                dtImport.Columns.RemoveAt(0);
                wb.Worksheets.Add(dtImport);
            }
            #endregion

            #region EXPORT_CHA
            if (gvExport.Rows.Count > 1)
            {
                DataTable dtExport = new DataTable("Export CHA");
                foreach (TableCell cell in gvExport.HeaderRow.Cells)
                {
                    dtExport.Columns.Add(cell.Text);
                }
                //Loop and add rows from GridView to DataTable.
                foreach (GridViewRow row in gvExport.Rows)
                {
                    dtExport.Rows.Add();
                    for (int j = 0; j < row.Cells.Count; j++)
                    {
                        dtExport.Rows[dtExport.Rows.Count - 1][j] = row.Cells[j].Text;
                    }
                }
                dtExport.Columns.RemoveAt(0);
                wb.Worksheets.Add(dtExport);
            }
            #endregion

            #region FREIGHT FORWARDING
            if (gvFreight.Rows.Count > 1)
            {
                DataTable dtFreight = new DataTable("Freight Forwarding");
                foreach (TableCell cell in gvFreight.HeaderRow.Cells)
                {
                    dtFreight.Columns.Add(cell.Text);
                }

                //Loop and add rows from GridView to DataTable.
                foreach (GridViewRow row in gvFreight.Rows)
                {
                    dtFreight.Rows.Add();
                    for (int j = 0; j < row.Cells.Count; j++)
                    {
                        dtFreight.Rows[dtFreight.Rows.Count - 1][j] = row.Cells[j].Text;
                    }
                }
                dtFreight.Columns.RemoveAt(0);
                wb.Worksheets.Add(dtFreight);
            }
            #endregion

            #region TRANSPORTATION
            if (gvTransportation.Rows.Count > 1)
            {
                DataTable dtTransportation = new DataTable("Transportation");
                foreach (TableCell cell in gvTransportation.HeaderRow.Cells)
                {
                    dtTransportation.Columns.Add(cell.Text);
                }

                //Loop and add rows from GridView to DataTable.
                foreach (GridViewRow row in gvTransportation.Rows)
                {
                    dtTransportation.Rows.Add();
                    for (int j = 0; j < row.Cells.Count; j++)
                    {
                        dtTransportation.Rows[dtTransportation.Rows.Count - 1][j] = row.Cells[j].Text;
                    }
                }
                dtTransportation.Columns.RemoveAt(0);
                wb.Worksheets.Add(dtTransportation);
            }
            #endregion

            //Export the Excel file.
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=GridView.xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvImport.DataBind();
        gvExport.DataBind();
        gvFreight.DataBind();
        gvTransportation.DataBind();
    }
}