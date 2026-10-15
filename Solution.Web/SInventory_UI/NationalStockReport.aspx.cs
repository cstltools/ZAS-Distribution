using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;

public partial class SInventory_UI_NationalStockReport : System.Web.UI.Page
{
    readonly WhStockConditionFreezeBll _aConditionFreezeBll = new WhStockConditionFreezeBll();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdownList();
        }
    }

    private void LoadDropdownList()
    {
        //_aConditionFreezeBll.LoadWhInfoOnDropDownList(whDropDownList);
        _aConditionFreezeBll.LoadCompanyDropdownList(companyNameDropDownList);
    }


    protected void reportButton_OnClick(object sender, EventArgs e)
    {
        Session["PRAM"] = "";
        Session["PRAM"] = GstParameter();

        string url = "../SInventory_RPTVIEW/NationalStockReportViewer.aspx";
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    protected void htmlReportButton_OnClick(object sender, EventArgs e)
    {
        Session["PRAM"] = "";
        Session["PRAM"] = GstParameter();
        Session["CompanyName"] = companyNameDropDownList.SelectedItem.Text;

        string url = "../HTMLReport/NationalStockReportHTMLReport.aspx";
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW_HTML", fullURL, true);
    }

    private string GstParameter()
    {
        string pram = "WHERE vTblTotal.ProductCode IS NOT NULL";

        if (companyNameDropDownList.SelectedValue != "")
        {
            pram = pram + " AND CASE WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '371' THEN 'ZAS Dermal' " +
                                " WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '372' THEN 'ZAS Critical Care' " +
                                " WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '373' THEN 'ZAS Ophthalmic'" +
                                " WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '379' THEN 'ZAS Active'" +
                                " WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '375' THEN 'ZAS Dental ( Clinic )'" +
                                " WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '381' THEN 'ZAS Dental ( Lab  )'" +
                                " WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '377' THEN 'ZAS Gloves & Instruments'" +
                                " WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '376' THEN 'ZAS Gyno'" +
                                " WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '378' THEN 'ZAS Nutrition'" +
                                " WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '374' THEN 'ZAS Onco' " +
                                " WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '382' THEN 'ZAS OTC'" +
                                " WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '383' THEN 'ZAS NOC'" +
                                " WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '384' THEN 'ZAS Baby Care'" +
                                " WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '385' THEN 'ZAS Neurology'" +
                                " WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '386' THEN 'ZAS Hematology'" +
                                " WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '387' THEN 'Aqvida Bangladesh'" +
                                " WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '391' THEN 'Z A S Nutrition'" +
                                " WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '392' THEN 'ZAS I V F'" +
                                " WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '393' THEN 'ZAS Bioplasma'" +
                                " WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '397' THEN 'ZAS Distribution Sales & Marketing'" +
                                " WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '394' THEN 'Esha Enterprise'" +
                                " WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '389' THEN 'Z A S Industries'" +
                                " WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '396' THEN 'Dermal-Devision-1'" +
                                " WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '398' THEN 'ZAS Rare Deasese'" +
                                " WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '399' THEN 'ZAS Nutrition-IV'" +
                                " WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '411' THEN 'ZAS Named Patient Access'" +
                                " WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '412' THEN 'ZAS Biotech'" +
                                " WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '413' THEN 'ZAS Anesthesia'" +
                                " WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '414' THEN 'Pharma Parts  International'" +
                                " WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '415' THEN 'ZAS Dermatology'" +
                                " WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '416' THEN 'ZAS Pharma LTD ( New )'" +
                                " WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '388' THEN 'ZAS Pharma Ltd.'" +
                                " WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '417' THEN 'ZAS Regenerative'" +
                                " WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '418' THEN 'ZAS Cardiology'" +
                                " ELSE ''" +
                                " END = '" + companyNameDropDownList.SelectedItem.Text + "'";
        }

        return pram;
    }
}