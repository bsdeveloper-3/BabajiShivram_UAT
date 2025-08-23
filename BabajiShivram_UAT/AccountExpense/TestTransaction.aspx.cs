using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BankAPI.YesBank;
using BankAPI.Open;

public partial class AccountExpense_TestTransaction : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnGetLiveBalance_Click(object sender, EventArgs e)
    {
        lblToken.Text = "";

           BankReqBalance getUATBalance = new BankReqBalance();

        BankReqBalance.Root objRoot = new BankReqBalance.Root();
        BankReqBalance.Data objData = new BankReqBalance.Data();
        BankReqBalance.DebtorAccount objDebAccount = new BankReqBalance.DebtorAccount();

        objRoot.Data = objData;
        objData.DebtorAccount = objDebAccount;

        objDebAccount.ConsentId = "418585";
        objDebAccount.Identification = "007881300000296";
        objDebAccount.SecondaryIdentification = "418585";

        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        var jsonPayLoad = serializer.Serialize(objRoot);

        BankRespBalance.Root result = new BankRespBalance.Root();

        lblToken.Text = BankAPIMethods.BankGetLiveBalance(jsonPayLoad);

        //lblToken.Text =  result.Data.FundsAvailableResult.BalanceAmount;
    }
}