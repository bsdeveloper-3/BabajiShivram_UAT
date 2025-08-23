using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.ConstrainedExecution;

public partial class AccountExpense_CheckClientCertificate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckClientCertificate();
        //checkCrtificateInstall();
    }

    private void CheckClientCertificate()
    {
        var context = HttpContext.Current;
        var request = context.Request;
        var certificate = request.ClientCertificate;

        HttpClientCertificate cert = Request.ClientCertificate;

        if (cert.Count > 0 )
        {
            if (cert.IsPresent)
            {
                string para = "<div style='margin: 10px 0 0 0; font-weight: bold'>{0}</div>";
                string subpara = "<div style='margin-left: 15px; font-size: 90%'>{0}</div>";

                lblMessage.Text = cert.Count.ToString();// "Certificate Found";

                X509Certificate2 x509Cert2 = new X509Certificate2(Page.Request.ClientCertificate.Certificate);

                Response.Write(string.Format(para, "Issued To:"));
                Response.Write(string.Format(subpara, x509Cert2.Subject));

                Response.Write(string.Format(para, "Issued By:"));
                Response.Write(string.Format(subpara, x509Cert2.Issuer));

                Response.Write(string.Format(para, "Friendly Name:"));
                Response.Write(string.Format(subpara, string.IsNullOrEmpty(x509Cert2.FriendlyName) ? "(None Specified)" : x509Cert2.FriendlyName));

                Response.Write(string.Format(para, "Valid Dates:"));
                Response.Write(string.Format(subpara, "From: " + x509Cert2.GetEffectiveDateString()));
                Response.Write(string.Format(subpara, "To: " + x509Cert2.GetExpirationDateString()));

                Response.Write(string.Format(para, "Thumbprint:"));
                Response.Write(string.Format(subpara, x509Cert2.Thumbprint));
            }

            foreach(string strKey in Request.ClientCertificate)
            {

            }
            //HttpClientCertificate cert1 = cert.Certificate;
        }
        else
        {
            lblMessage.Text = "Certificate Not Found";

                var response = context.Response;
                response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                response.StatusDescription = "No valid certificate";
                response.Flush();
                response.Close();
        }
    }

    private void checkCrtificateInstall()
    {
        string friendlyName = "Live.BabajiShivram.Com0720";
        var store = GetLocalMachineCertificates();
        X509Certificate2 certificate = null;
        foreach (var cert in store.Cast<X509Certificate2>().Where(cert => cert.FriendlyName.Equals(friendlyName)))
        {
            certificate = cert;
        }

        if(certificate != null)
        {
            lblMessage.Text = certificate.FriendlyName;
            lblMessage.Text = certificate.SerialNumber.ToString();
        }
        else
        {
            lblMessage.Text = "certificate not found";
        }

        //return certificate != null ? certificate.Export(X509ContentType.Pkcs12) : null;
    }

    private static X509Certificate2Collection GetLocalMachineCertificates()
    {
        var localMachineStore = new X509Store(StoreLocation.LocalMachine);
        localMachineStore.Open(OpenFlags.ReadOnly);
        var certificates = localMachineStore.Certificates;
        localMachineStore.Close();
        return certificates;
    }
}