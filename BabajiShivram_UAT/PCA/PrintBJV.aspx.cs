using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class PCA_PrintBJV : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["BJVJobList"] != null)
            {
                rptBJVDetails.DataSource = DBOperations.GetJobDetailByList(Session["BJVJobList"].ToString());
                rptBJVDetails.DataBind();
            }
        }
    }

    protected void rptBJVDetails_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            string strJobID = Convert.ToString(dr["JobID"]);

            GridView gvjobexpenseDetail = (GridView)e.Item.FindControl("gvjobexpenseDetail");
            
            DataSourceExpenseDetail.SelectParameters[0].DefaultValue = strJobID;

            gvjobexpenseDetail.DataSource = DataSourceExpenseDetail;
            gvjobexpenseDetail.DataBind();

            //Calculate Debit Sum and display in Footer Row
            DataSourceSelectArguments args = new DataSourceSelectArguments();
            
            DataView view = (DataView)DataSourceExpenseDetail.Select(args);

            DataTable dt = view.ToTable();

            int d = 0;
            decimal total = dt.AsEnumerable()
                .Where(row => !row.IsNull("DEBITAMT") && Int32.TryParse(row["DEBITAMT"].ToString(), out d))
                .Sum(row => d);

            //.Sum(row => row.Field<Int32>("DEBITAMT"));

            if (gvjobexpenseDetail.Rows.Count > 0)
            {
                gvjobexpenseDetail.FooterRow.Cells[1].Text = "Total";
                gvjobexpenseDetail.FooterRow.Cells[5].Text = total.ToString();
            }
                      
        }
       
    }
}