using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_WarehouseStockInApproval : System.Web.UI.Page
{
    WarehouseStockInApprovalBll approvalBll = new WarehouseStockInApprovalBll();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            WarehouseStockInInformation();
        }
    }

    private void WarehouseStockInInformation()
    {

        DataTable aDataTable = new DataTable();

        aDataTable = approvalBll.LoadWarehouseStockInInformation();
        loadGridView.DataSource = aDataTable;
        loadGridView.DataBind();
    }


    private void ShowMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    private void PopUp(string Id)
    {
        string url = "AdvanceSalDeductionEdit.aspx?ID=" + Id;
        string fullURL = "var Mleft = (screen.width/2)-(700/2);var Mtop = (screen.height/2)-(600/2);window.open( '" + url + "', null, 'height=600,width=700,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    protected void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditData")
        {
            int rowindex = Convert.ToInt32(e.CommandArgument);
            string AdvanceSalDeductionId = loadGridView.DataKeys[rowindex][0].ToString();
            PopUp(AdvanceSalDeductionId);
        }

    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ChkBoxHeader = (CheckBox)loadGridView.HeaderRow.FindControl("chkSelectAll");

        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            CheckBox ChkBoxRows = (CheckBox)loadGridView.Rows[i].Cells[4].FindControl("chkSelect");
            if (ChkBoxHeader.Checked == true)
            {
                ChkBoxRows.Checked = true;
            }
            else
            {
                ChkBoxRows.Checked = false;
            }
        }
    }

    protected void btnSubmit0_Click(object sender, EventArgs e)
    {
        
        if (Validation())
        {
            SubmitApproval();
            Clear();
        }
        else
        {
            ShowMessageBox("Please select at least one row!!!");
        }
        
        WarehouseStockInInformation();
    }

    private void Clear()
    {

        for (int i = 0; i < statusRadioButtonList.Items.Count; i++)
        {
            statusRadioButtonList.Items[i].Selected = false;
        }
        
    }

    private bool Validation()
    {
        bool status = false;
        int count = 0;

        if (statusRadioButtonList.SelectedValue == "")
        {
            ShowMessageBox("Please select an operation!!");
        }

        else
        {
            for (int i = 0; i < loadGridView.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)loadGridView.Rows[i].Cells[4].FindControl("chkSelect");

                if (cb.Checked)
                {
                    count++;
                }

                if (count > 0)
                {
                    status = true;
                    break;
                }
            }
        }

        return status;
    }

    private void SubmitApproval()
    {

        if (statusRadioButtonList.SelectedItem.Text != "Reject")
        {
            for (int i = 0; i < loadGridView.Rows.Count; i++)
            {

                CheckBox cb = (CheckBox) loadGridView.Rows[i].Cells[4].FindControl("chkSelect");

                if (cb.Checked)
                {
                    WarehouseStockInMasterDao aMasterDao = new WarehouseStockInMasterDao();

                    aMasterDao.WHStockInMasterID = Convert.ToInt32(loadGridView.DataKeys[i][0].ToString());


                    aMasterDao.WHStockInMasterID = Convert.ToInt32(loadGridView.DataKeys[i][0].ToString());

                    
                    aMasterDao.Status = "Approved";
                    aMasterDao.ApproveBy = Session["LoginName"].ToString();
                    aMasterDao.ApproveDate = DateTime.Today;

                    Int32 stockInMasterId = aMasterDao.WHStockInMasterID;

                    var MaterialConversionCount = Convert.ToInt32(loadGridView.DataKeys[i][1].ToString());

                    if (MaterialConversionCount > 0)
                    {
                        approvalBll.ApproveWarehouseStockInMaster(aMasterDao); // Insert from Material Conversion
                    }
                    else
                    {
                        approvalBll.InsertDataIntoCenterlWarehouse(PrepareData(stockInMasterId));
                    }


                   

                   // if (approvalBll.InsertDataIntoCenterlWarehouse(PrepareData(stockInMasterId)))
                   // {
                   //     approvalBll.PostJournal(stockInMasterId);
                   // }

                   approvalBll.ApprovalUpdateBLL(aMasterDao);

                }
            }

            ShowMessageBox("Stock Approved Successfully!!");
        }

        else
        {
            for (int i = 0; i < loadGridView.Rows.Count; i++)
            {

                CheckBox cb = (CheckBox)loadGridView.Rows[i].Cells[4].FindControl("chkSelect");

                if (cb.Checked)
                {
                    WarehouseStockInMasterDao aMasterDao = new WarehouseStockInMasterDao();

                    aMasterDao.WHStockInMasterID = Convert.ToInt32(loadGridView.DataKeys[i][0].ToString());

                    aMasterDao.Status = statusRadioButtonList.SelectedItem.Text;
                    aMasterDao.ApproveBy = Session["LoginName"].ToString();
                    aMasterDao.ApproveDate = DateTime.Today;

                    approvalBll.ApprovalUpdateBLL(aMasterDao);

                }
            }

            ShowMessageBox("Stock Rejected Successfully!!!");
        }

    }

    private List<WHStockInToCenteralStore> PrepareData(Int32 stockInMasterId)
    {
        DataTable aDataTable = new DataTable();

        aDataTable = approvalBll.LoadWarehouseStockInInfo(stockInMasterId);

        WHStockInToCenteralStore aCentralStore;
        List<WHStockInToCenteralStore> aList = new List<WHStockInToCenteralStore>();
      
        for (int i = 0; i < aDataTable.Rows.Count; i++)
        {
            aCentralStore = new WHStockInToCenteralStore();

            aCentralStore.WarehouseId = aDataTable.Rows[i].Field<Int32>("WarehouseId");
            aCentralStore.ProductCode = aDataTable.Rows[i].Field<String>("ProductCode");
            aCentralStore.ProductName = aDataTable.Rows[i].Field<String>("ProductName");
            aCentralStore.PackSize = aDataTable.Rows[i].Field<String>("PackSize");
            aCentralStore.BatchNo = aDataTable.Rows[i].Field<String>("Batch");
            aCentralStore.ExpDate = aDataTable.Rows[i].Field<DateTime>("ExpDate");
            aCentralStore.MfgDate = aDataTable.Rows[i].Field<DateTime>("MfgDate");
            aCentralStore.ReceiveDate = DateTime.Now;
            aCentralStore.ChalanNo = aDataTable.Rows[i].Field<String>("ChallanNo");
            aCentralStore.ChalanDate = aDataTable.Rows[i].Field<DateTime>("ChallanDate");
            aCentralStore.StockInQty = aDataTable.Rows[i].Field<Decimal>("Qty");
            aCentralStore.UnitPrice = aDataTable.Rows[i].Field<Decimal>("Price");
            //aCentralStore.StockInQty = aDataTable.Rows[i].Field<Decimal>("Qty");

            decimal stockQuantity = aDataTable.Rows[i].Field<Decimal>("Qty");
            decimal price = aDataTable.Rows[i].Field<Decimal>("Price");
            decimal totalPrice = price*stockQuantity;
            aCentralStore.TotalPrice = totalPrice;
 
            aCentralStore.VATPerUnit = aDataTable.Rows[i].Field<Decimal>("VAT");

            decimal vatPerUnit = aDataTable.Rows[i].Field<Decimal>("VAT");
            decimal totalVat = vatPerUnit*stockQuantity;
            aCentralStore.TotalVAT = totalVat;

            aCentralStore.TotalAmount = aDataTable.Rows[i].Field<Decimal>("TotalAmount");
            aCentralStore.Quantity = aDataTable.Rows[i].Field<Decimal>("Qty");
            aCentralStore.MigoDetailID = aDataTable.Rows[i].Field<Int32>("WHStockInDetailID");
            aCentralStore.StockCondition = "Available";

            aCentralStore.ProductStockType = "Regular";
            aCentralStore.ProductId = aDataTable.Rows[i].Field<Int32>("ProductId");

            aList.Add(aCentralStore);
        }

        return aList;
    }


    protected void printButton_Click(object sender, EventArgs e)
    {
        int iRowIndex = (((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex;
        string reqId = loadGridView.DataKeys[iRowIndex][0].ToString();

        Session["ReportType"] = "";
        Session["ReportType"] = "STD";

        string url = "../SInventory_RPTVIEW/WHStockInReportViewer.aspx?reqId=" + reqId;
        // string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=900,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    }