using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAL.SInventory_DAL;

public partial class Dashboard_Dashboard : System.Web.UI.Page
{
    DashboardInfoDal aInfoDal =  new DashboardInfoDal();
    RequisitionBLL aRequisitionBll = new RequisitionBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           
            LoadDropDownList();

            //ShowData();
        }
    }

    private void LoadDropDownList()
    {
        aRequisitionBll.LoadCompanyDropdownList(companyNameDropDownList);
    }

    private void ShowData()
    {

        if (companyNameDropDownList.SelectedValue != "")
        {
            DataTable tb = aInfoDal.LoadUnitWiseMonthlySales(companyNameDropDownList.SelectedValue);

            if (tb.Rows.Count > 0)
            {
                String chart = "";
                // You can change your chart height by modify height value
                chart = "<canvas id=\"line-chart\" width=\"100%\" height=\"40\"></canvas>";
                chart += "<script>";
                chart += "new Chart(document.getElementById(\"line-chart\"), { type: 'bar', data: {labels: [";

                // more details in x-axis
                for (int i = 0; i < tb.Rows.Count; i++)
                    chart += tb.Rows[i]["MonthName"].ToString() + ",";
                chart = chart.Substring(0, chart.Length - 1);

                chart += "],datasets: [{ data: [";

                // put data from database to chart
                String value = "";
                for (int i = 0; i < tb.Rows.Count; i++)
                    value += tb.Rows[i]["NhietDo"].ToString() + ",";
                value = value.Substring(0, value.Length - 1);

                chart += value;

                chart += "],label: \"Sales (Monthly)\",borderColor: \"#196F3D\",fill: true}"; // Chart color
                chart += "]},options: { title: { display: true,text: 'Wings Sales (TK)'} }"; // Chart title
                chart += "});";
                chart += "</script>";

                ltChart.Text = chart;
            }
        }

                 
     }

    protected void companyNameDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ShowData();
    }
}