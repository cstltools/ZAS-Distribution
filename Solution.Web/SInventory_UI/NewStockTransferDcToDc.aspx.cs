using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_NewStockTransferDcToDc : System.Web.UI.Page
{
    ChalanBLL aChalanBll=new ChalanBLL();
    SCtoWHTransferDal aDal = new SCtoWHTransferDal();
    DestroyProductDal aReportDal = new DestroyProductDal();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
            ChalanId();
            Todate();
            aChalanBll.LoadManufac(manufacDropDownList);
            aDal.LoadManufac(ddlFromCmpany);
            aDal.LoadManufac(ddlToCompany);
            //aDal.DCLoad(salescenterDropDownList1);
        }
        
    }

    public void Todate()
    {
        chalanDateTextBox.Text = DateTime.Today.ToShortDateString();
    }
    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ChkBoxHeader = (CheckBox)productGridView.HeaderRow.FindControl("chkSelectAll");

        for (int i = 0; i < productGridView.Rows.Count; i++)
        {
            CheckBox ChkBoxRows = (CheckBox)productGridView.Rows[i].Cells[0].FindControl("chkSelect");
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
    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    protected void salescenterDropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {


        // Reset Grid

        prodctCodeTextBox.Text = "";

        productGridView.DataSource = null;
        productGridView.DataBind();

        chalanGridView.DataSource = null;
        chalanGridView.DataBind();


        if (salescenterDropDownList2.SelectedValue != "")
        {
            if (salescenterDropDownList2.SelectedValue == salescenterDropDownList1.SelectedValue)
            {
                salescenterDropDownList1.SelectedValue = "";
            }
            else
            {
                string unitCode = salescenterDropDownList1.SelectedItem.Text.Split(':')[0];
                fromComUnitCodeTextBox.Text = unitCode;
                DataTable aDataTable = new DataTable();
                if (!string.IsNullOrEmpty(unitCode))
                {
                    aDataTable = aDal.LoadComUnit(unitCode);
                    if (aDataTable.Rows.Count > 0)
                    {
                        fromComUnitNameTextBox.Text = aDataTable.Rows[0]["ComUnitName"].ToString();
                        fromComUnitAddressTextBox.Text = aDataTable.Rows[0]["Address"].ToString();
                    }
                    else
                    {
                        showMessageBox("Company Unit Information Not Found!!");
                        fromComUnitNameTextBox.Text = string.Empty;
                        fromComUnitAddressTextBox.Text = string.Empty;
                        fromComUnitCodeTextBox.Text = string.Empty;
                    }
                }
            }
        }
        else
        {
            string unitCode = salescenterDropDownList1.SelectedItem.Text.Split(':')[0];
            fromComUnitCodeTextBox.Text = unitCode;
            DataTable aDataTable = new DataTable();
            if (!string.IsNullOrEmpty(unitCode))
            {
                aDataTable = aDal.LoadComUnit(unitCode);
                if (aDataTable.Rows.Count > 0)
                {
                    fromComUnitNameTextBox.Text = aDataTable.Rows[0]["ComUnitName"].ToString();
                    fromComUnitAddressTextBox.Text = aDataTable.Rows[0]["Address"].ToString();
                }
                else
                {
                    showMessageBox("Company Unit Information Not Found!!");
                    fromComUnitNameTextBox.Text = string.Empty;
                    fromComUnitAddressTextBox.Text = string.Empty;
                    fromComUnitCodeTextBox.Text = string.Empty;
                }
            } 
        }

       
    }

    private void Clear()
    {
        prodctCodeTextBox.Text = string.Empty;
        chalanDateTextBox.Text = string.Empty;
        chalanNoTextBox.Text = string.Empty;
        fromComUnitAddressTextBox.Text = string.Empty;
        fromComUnitCodeTextBox.Text = string.Empty;
        fromComUnitNameTextBox.Text = string.Empty;
        toComUnitNameTextBox.Text = string.Empty;
        toComUnitCodeTextBox.Text = string.Empty;
        toComUnitAddressTextBox.Text = string.Empty;
        driverNameTextBox.Text = string.Empty;
        truckNoTextBox.Text = string.Empty;
        chalanGridView.DataSource = null;
        chalanGridView.DataBind();
        productGridView.DataSource = null;
        productGridView.DataBind();
    }

    private bool Validation()
    {

        if (ddlFromCmpany.SelectedValue != ddlToCompany.SelectedValue)
        {
            showMessageBox("Company or Wings Should be same !!");
            return false;
        }
        if (chalanNoTextBox.Text == "")
        {
            showMessageBox("Please Insert chalanNo!!");
            return false;
        }
        if (chalanDateTextBox.Text.Trim() == "")
        {
            showMessageBox("Please Insert chalanDate!!");
            return false;
        }
        //if (manufacDropDownList.SelectedValue == "")
        //{
        //    showMessageBox("Please Select Manufacturer!!");
        //    return false;
        //}
        if (fromComUnitCodeTextBox.Text.Trim()  == "")
        {
            showMessageBox("Please Select fromComUnit!!");
            return false;
        }
        if (toComUnitCodeTextBox.Text.Trim()  == "")
        {
            showMessageBox("Please Select toComUnit!!");
            return false;
        }
        //if (chalanGridView.Rows.Count<0)
        //{
        //    showMessageBox("Please Add Product!!");
        //    return false;
        //}
        if (chalanGridView.Rows.Count < 1)
        {
            showMessageBox("Please Add Product!!");
            return false;
        }
        return true;
    }
    protected void submitButton_Click1(object sender, EventArgs e)
    {
        if (Validation())
        {
            var aChalanInfo = new ChalanInfo();

            aChalanInfo.FromComUnitId = Convert.ToInt32(salescenterDropDownList1.SelectedValue);
            aChalanInfo.ChalanDate = Convert.ToDateTime(chalanDateTextBox.Text.Trim());
            aChalanInfo.ChalanNo = chalanNoTextBox.Text.Trim();
            aChalanInfo.TrackNo = truckNoTextBox.Text.Trim();
            aChalanInfo.DriverName = driverNameTextBox.Text.Trim();
            aChalanInfo.FromComUnitCode = fromComUnitCodeTextBox.Text.Trim();
            aChalanInfo.FromComUnitName = fromComUnitNameTextBox.Text.Trim();
            aChalanInfo.FromComUnitAddress = fromComUnitAddressTextBox.Text.Trim();
            aChalanInfo.ToComUnitCode = toComUnitCodeTextBox.Text.Trim();
            aChalanInfo.ToComUnitName = toComUnitNameTextBox.Text.Trim();
            aChalanInfo.ToComUnitAddress = toComUnitAddressTextBox.Text.Trim();

            if (manufacDropDownList.SelectedValue != "")
            {
                aChalanInfo.ManufacId = Convert.ToInt32(manufacDropDownList.SelectedValue);
            }
            
            
            decimal totalvalue = 0;
            decimal totalvat = 0;
            decimal grandtotal = 0;
            for (int i = 0; i < chalanGridView.Rows.Count; i++)
            {
                totalvat +=
                    Convert.ToDecimal(
                        chalanGridView.Rows[i].Cells[2].Text)*
                    Convert.ToDecimal(chalanGridView.DataKeys[i][1].ToString());
                totalvalue += Convert.ToDecimal(chalanGridView.DataKeys[i][2].ToString())*
                              Convert.ToDecimal(
                                  chalanGridView.Rows[i].Cells[2].Text);
                grandtotal += Convert.ToDecimal(chalanGridView.DataKeys[i][2].ToString())*
                              Convert.ToDecimal(
                                  chalanGridView.Rows[i].Cells[2].Text) +
                              Convert.ToDecimal(
                                  chalanGridView.Rows[i].Cells[2].Text)*
                              Convert.ToDecimal(chalanGridView.DataKeys[i][1].ToString());
            }
            aChalanInfo.TotalValue = totalvalue;
            aChalanInfo.TotalVat = totalvat;
            aChalanInfo.GrandTotal = grandtotal;

            aChalanInfo.ChalanBy = Session["LoginName"].ToString();

            int chalanId;
            bool status = aChalanBll.SaveDataForChalanInfo(aChalanInfo, out chalanId);


            if (chalanId > 0)
            {
                // Set Challan No for Report Preview

                DataTable dt = aChalanBll.LoadChalanById(chalanId.ToString());
                if (dt.Rows.Count > 0)
                {
                    printChalanNoTextBox.Text = dt.Rows[0]["ChalanNo"].ToString();
                }

                // Save Details

                List<ChalanDetail> aChalanDetailList = new List<ChalanDetail>();

                for (int i = 0; i < chalanGridView.Rows.Count; i++)
                {
                    ChalanDetail aChalanDetail = new ChalanDetail()
                    {
                        ChalanId = chalanId,
                        DCStoreId = Convert.ToInt32(chalanGridView.DataKeys[i][0].ToString()),
                        ProductCode = chalanGridView.Rows[i].Cells[0].Text,
                        ProductName = chalanGridView.Rows[i].Cells[1].Text,
                        Quantity = Convert.ToDecimal(chalanGridView.Rows[i].Cells[2].Text),
                        BatchNo = chalanGridView.Rows[i].Cells[3].Text,
                        UnitPrice = Convert.ToDecimal(chalanGridView.DataKeys[i][2].ToString()),
                        Value =
                            Convert.ToDecimal(chalanGridView.DataKeys[i][2].ToString())*
                            Convert.ToDecimal(chalanGridView.Rows[i].Cells[2].Text),
                        Vat =
                            Convert.ToDecimal(chalanGridView.Rows[i].Cells[2].Text)*
                            Convert.ToDecimal(chalanGridView.DataKeys[i][1].ToString()),
                        ValueWVat =
                            Convert.ToDecimal(chalanGridView.DataKeys[i][2].ToString())*
                            Convert.ToDecimal(chalanGridView.Rows[i].Cells[2].Text) +
                            Convert.ToDecimal(chalanGridView.Rows[i].Cells[2].Text)*
                            Convert.ToDecimal(chalanGridView.DataKeys[i][1].ToString())
                    };

                    aChalanDetailList.Add(aChalanDetail);
                }


                if (aChalanBll.SaveDataForChalanDetail(aChalanDetailList))
                {
                    showMessageBox("Data save successfully !!!");
                    Clear();
                    Todate();
                    ChalanId();
                }

            }

            else
            {
                showMessageBox("Already Exist !!");
            }

            
        }
    }
    public void ChalanId()
    {
        ChalanBLL aChalanBll = new ChalanBLL();
        chalanNoTextBox.Text = aChalanBll.ChalanNoForShow();
    }
    
    protected void fromComUnitCodeTextBox_TextChanged(object sender, EventArgs e)
    {
        string unitCode = fromComUnitCodeTextBox.Text.Trim();
        DataTable aDataTable = new DataTable();
        if (!string.IsNullOrEmpty(unitCode))
        {
            aDataTable = aChalanBll.ComUnit(unitCode);
            if (aDataTable.Rows.Count > 0)
            {
                fromComUnitNameTextBox.Text = aDataTable.Rows[0]["ComUnitName"].ToString();
                fromComUnitAddressTextBox.Text = aDataTable.Rows[0]["Address"].ToString();
            }
            else
            {
                showMessageBox("Company Unit Information Not Found!!");
                fromComUnitNameTextBox.Text = string.Empty;
                fromComUnitAddressTextBox.Text =  string.Empty;
                fromComUnitCodeTextBox.Text = string.Empty;
            }
        }
    }
    protected void toComUnitCodeTextBox_TextChanged(object sender, EventArgs e)
    {
        string unitCode = toComUnitCodeTextBox.Text.Trim();
        DataTable aDataTable = new DataTable();
        if (!string.IsNullOrEmpty(unitCode))
        {
            aDataTable = aChalanBll.ComUnit(unitCode);
            if (aDataTable.Rows.Count > 0)
            {
                toComUnitNameTextBox.Text = aDataTable.Rows[0]["ComUnitName"].ToString();
                toComUnitAddressTextBox.Text = aDataTable.Rows[0]["Address"].ToString();
            }
            else
            {
                showMessageBox("Company Unit Information Not Found!!");
                toComUnitNameTextBox.Text = string.Empty;
                toComUnitAddressTextBox.Text = string.Empty;
                toComUnitCodeTextBox.Text = string.Empty;
            }
        }
    }
    
    protected void Button1_Click(object sender, EventArgs e)
    {
        DataTable dtdata = aChalanBll.GetProductDcStore(prodctCodeTextBox.Text, fromComUnitCodeTextBox.Text);
        if (dtdata.Rows.Count>0 )
        {
            productGridView.DataSource = dtdata;
            productGridView.DataBind();
        }
        else
        {
            showMessageBox("No Product Found!!");
        }
    }

    public bool HasDCStoreId(int dcstoreId)
    {
        for (int i = 0; i < chalanGridView.Rows.Count; i++)
        {
            if (Convert.ToInt32(chalanGridView.DataKeys[i][0].ToString())==dcstoreId)
            {
                return false;
                break;
                
            }
        }
        return true;
    }
    protected void addButton_Click(object sender, EventArgs e)
    {
        DataTable aDataTable=new DataTable();
        aDataTable.Columns.Add("DCStoreId");
        aDataTable.Columns.Add("ProductCode");
        aDataTable.Columns.Add("ProductName");
        aDataTable.Columns.Add("TransferQty");
        aDataTable.Columns.Add("BatchNo");
        aDataTable.Columns.Add("ExpDate");
        aDataTable.Columns.Add("VATAmountPerUnit");
        aDataTable.Columns.Add("UnitPrice");

        DataRow dataRow = null;
        for (int i = 0; i < productGridView.Rows.Count; i++)
        {
            CheckBox ChkBoxRows = (CheckBox)productGridView.Rows[i].Cells[0].FindControl("chkSelect");
            if (ChkBoxRows.Checked && ((TextBox)productGridView.Rows[i].Cells[6].FindControl("transferQtyTextBox")).Text.Trim() !="")
            {
                if (HasDCStoreId(Convert.ToInt32(productGridView.DataKeys[i][0].ToString())))
                {
                    dataRow = aDataTable.NewRow();
                    dataRow["ProductCode"] = productGridView.Rows[i].Cells[1].Text;
                    dataRow["ProductName"] = productGridView.Rows[i].Cells[2].Text;
                    dataRow["TransferQty"] =
                        ((TextBox)productGridView.Rows[i].Cells[6].FindControl("transferQtyTextBox")).Text.Trim();
                    
                    dataRow["BatchNo"] = productGridView.Rows[i].Cells[4].Text;
                    dataRow["ExpDate"] = productGridView.Rows[i].Cells[5].Text;
                    dataRow["VATAmountPerUnit"] = productGridView.DataKeys[i][1].ToString();
                    dataRow["DCStoreId"] = productGridView.DataKeys[i][0].ToString();
                    dataRow["UnitPrice"] = productGridView.DataKeys[i][2].ToString();
                    
                    aDataTable.Rows.Add(dataRow);
                }
            }
        }
        for (int i = 0; i < chalanGridView.Rows.Count; i++)
        {
            dataRow = aDataTable.NewRow();
            dataRow["ProductCode"] = chalanGridView.Rows[i].Cells[0].Text;
            dataRow["ProductName"] = chalanGridView.Rows[i].Cells[1].Text;
            dataRow["TransferQty"] =
                chalanGridView.Rows[i].Cells[2].Text;
            dataRow["BatchNo"] = chalanGridView.Rows[i].Cells[3].Text;
            dataRow["ExpDate"] = chalanGridView.Rows[i].Cells[4].Text;
            dataRow["VATAmountPerUnit"] = chalanGridView.DataKeys[i][1].ToString();
            dataRow["DCStoreId"] = chalanGridView.DataKeys[i][0].ToString();
            dataRow["UnitPrice"] = chalanGridView.DataKeys[i][2].ToString();

            aDataTable.Rows.Add(dataRow);
        }

        chalanGridView.DataSource = aDataTable;
        chalanGridView.DataBind();
    }
    protected void DeleteImageButton_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton productCodeTextBox = (ImageButton)sender;
        GridViewRow currentRow = (GridViewRow)productCodeTextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;
        DataTable aDataTable = new DataTable();
        aDataTable.Columns.Add("DCStoreId");
        aDataTable.Columns.Add("ProductCode");
        aDataTable.Columns.Add("ProductName");
        aDataTable.Columns.Add("TransferQty");
        aDataTable.Columns.Add("BatchNo");
        aDataTable.Columns.Add("ExpDate");
        aDataTable.Columns.Add("VATAmountPerUnit");
        aDataTable.Columns.Add("UnitPrice");

        DataRow dataRow = null;
        for (int i = 0; i < chalanGridView.Rows.Count; i++)
        {
            if (i != rowindex)
            {


                dataRow = aDataTable.NewRow();
                dataRow["ProductCode"] = chalanGridView.Rows[i].Cells[0].Text;
                dataRow["ProductName"] = chalanGridView.Rows[i].Cells[1].Text;
                dataRow["TransferQty"] =
                    chalanGridView.Rows[i].Cells[2].Text;

                dataRow["BatchNo"] = chalanGridView.Rows[i].Cells[3].Text;
                dataRow["ExpDate"] = chalanGridView.Rows[i].Cells[4].Text;
                dataRow["VATAmountPerUnit"] = chalanGridView.DataKeys[i][1].ToString();
                dataRow["DCStoreId"] = chalanGridView.DataKeys[i][0].ToString();
                dataRow["UnitPrice"] = chalanGridView.DataKeys[i][2].ToString();

                aDataTable.Rows.Add(dataRow);
            }

        }

        chalanGridView.DataSource = aDataTable;
        chalanGridView.DataBind();

    }
    protected void Button2_Click(object sender, EventArgs e)
    {

        string url = "../SInventory_RPTVIEW/DcToDcChalanReportViewer.aspx?chalanno=" + printChalanNoTextBox.Text;
        // string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=900,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    protected void dQtyTextBox_TextChanged(object sender, EventArgs e)
    {
        for (int i = 0; i < productGridView.Rows.Count; i++)
        {
            //CheckBox cbReject = (CheckBox)productGridView.Rows[i].FindControl("chkSelect");
            //if (cbReject.Checked)
            {
                int rowindex = i;
                decimal mainqty = 0;
                decimal delqty = 0;
                delqty =
                    string.IsNullOrEmpty(
                        ((TextBox) productGridView.Rows[rowindex].Cells[3].FindControl("transferQtyTextBox")).Text)
                        ? 0
                        : Convert.ToDecimal(
                            ((TextBox) productGridView.Rows[rowindex].Cells[3].FindControl("transferQtyTextBox")).Text);

                mainqty = string.IsNullOrEmpty(productGridView.Rows[i].Cells[3].Text)
                    ? 0
                    : Convert.ToDecimal(productGridView.Rows[i].Cells[3].Text);
                if (delqty <= mainqty)
                {

                }
                else
                {
                    showMessageBox("Transfer Qty. cantbe more then Stock Quantity");
                    ((TextBox) productGridView.Rows[rowindex].Cells[3].FindControl("transferQtyTextBox")).Text =
                        string.Empty;
                }
            }
        }
    }

    protected void ddlFromCmpany_SelectedIndexChanged(object sender, EventArgs e)
    {
        salescenterDropDownList1.Items.Clear();
        fromComUnitCodeTextBox.Text = "";
        fromComUnitNameTextBox.Text = "";
        fromComUnitAddressTextBox.Text = "";


        if (ddlFromCmpany.SelectedValue != "")
        {
            if (Session["UserType"].ToString() != "")
            {
                //if (Session["UserType"].ToString() != "Admin")
                //{
                //    aReportDal.LoadDcInfoByUser(salescenterDropDownList1, ddlFromCmpany.SelectedValue, Session["UserId"].ToString());
                //}
                //else
                //{
                    aReportDal.LoadDcInfo(salescenterDropDownList1, ddlFromCmpany.SelectedValue);
                //}
            }
            else
            {
                Response.Redirect("Login.aspx");
            }

            AjaxControlToolkit.AutoCompleteExtender modal = (AjaxControlToolkit.AutoCompleteExtender)prodctCodeTextBox.FindControl("productNameTextBox_AutoCompleteExtender");
            modal.ContextKey = ddlFromCmpany.SelectedValue;
        }
        else
        {
            salescenterDropDownList1.Items.Clear();
            fromComUnitCodeTextBox.Text = "";
            fromComUnitNameTextBox.Text = "";
            fromComUnitAddressTextBox.Text = "";
            AjaxControlToolkit.AutoCompleteExtender modal = (AjaxControlToolkit.AutoCompleteExtender)prodctCodeTextBox.FindControl("productNameTextBox_AutoCompleteExtender");
            modal.ContextKey = null;
        }


        // Reset Grid

        prodctCodeTextBox.Text = "";

        productGridView.DataSource = null;
        productGridView.DataBind();

        chalanGridView.DataSource = null;
        chalanGridView.DataBind();

        //if (ddlFromCmpany.SelectedValue != "")
        //{
        //    aDal.DCLoad(salescenterDropDownList1, ddlFromCmpany.SelectedValue);
        //}
        //else
        //{
        //    salescenterDropDownList1.Items.Clear();
        //    fromComUnitCodeTextBox.Text = "";
        //    fromComUnitNameTextBox.Text = "";
        //    fromComUnitAddressTextBox.Text = "";
        //}
    }

    protected void ddlToCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        salescenterDropDownList2.Items.Clear();
        toComUnitCodeTextBox.Text = "";
        toComUnitNameTextBox.Text = "";
        toComUnitAddressTextBox.Text = "";

        if (ddlToCompany.SelectedValue != "")
        {
            if (Session["UserType"].ToString() != "")
            {
                //if (Session["UserType"].ToString() != "Admin")
                //{
                //    aReportDal.LoadDcInfoByUser(salescenterDropDownList2, ddlToCompany.SelectedValue, Session["UserId"].ToString());
                //}
                //else
                //{
                    aReportDal.LoadDcInfo(salescenterDropDownList2, ddlToCompany.SelectedValue);
                //}
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }
        else
        {
            salescenterDropDownList2.Items.Clear();
            toComUnitCodeTextBox.Text = "";
            toComUnitNameTextBox.Text = "";
            toComUnitAddressTextBox.Text = "";
        }


        //if (ddlToCompany.SelectedValue != "")
        //{
        //    aDal.DCLoad(salescenterDropDownList2, ddlToCompany.SelectedValue);
        //}
        //else
        //{
        //    salescenterDropDownList2.Items.Clear();
        //    toComUnitCodeTextBox.Text = "";
        //    toComUnitNameTextBox.Text = "";
        //    fromComUnitAddressTextBox.Text = "";
        //}
    }

    protected void salescenterDropDownList2_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (salescenterDropDownList1.SelectedValue != "")
        {
            if (salescenterDropDownList1.SelectedValue == salescenterDropDownList2.SelectedValue)
            {
                salescenterDropDownList2.SelectedValue = "";
            }
            else
            {
                string unitCode = salescenterDropDownList2.SelectedItem.Text.Split(':')[0];
                toComUnitCodeTextBox.Text = unitCode;
                DataTable aDataTable = new DataTable();
                if (!string.IsNullOrEmpty(unitCode))
                {
                    aDataTable = aDal.LoadComUnit(unitCode);
                    if (aDataTable.Rows.Count > 0)
                    {
                        toComUnitNameTextBox.Text = aDataTable.Rows[0]["ComUnitName"].ToString();
                        toComUnitAddressTextBox.Text = aDataTable.Rows[0]["Address"].ToString();
                    }
                    else
                    {
                        showMessageBox("Company Unit Information Not Found!!");
                        toComUnitCodeTextBox.Text = "";
                        toComUnitNameTextBox.Text = "";
                        toComUnitAddressTextBox.Text = "";
                    }
                }
            }
        }
        else
        {
            string unitCode = salescenterDropDownList2.SelectedItem.Text.Split(':')[0];
            toComUnitCodeTextBox.Text = unitCode;
            DataTable aDataTable = new DataTable();
            if (!string.IsNullOrEmpty(unitCode))
            {
                aDataTable = aDal.LoadComUnit(unitCode);
                if (aDataTable.Rows.Count > 0)
                {
                    toComUnitNameTextBox.Text = aDataTable.Rows[0]["ComUnitName"].ToString();
                    toComUnitAddressTextBox.Text = aDataTable.Rows[0]["Address"].ToString();
                }
                else
                {
                    showMessageBox("Company Unit Information Not Found!!");
                    toComUnitCodeTextBox.Text = "";
                    toComUnitNameTextBox.Text = "";
                    toComUnitAddressTextBox.Text = "";
                }
            }
        }
       
    }

    protected void productNameTextBox_TextChanged(object sender, EventArgs e)
    {

        string productName = prodctCodeTextBox.Text.Trim();

        if (productName.Contains(':'))
        {
            string[] productInfo = productName.Split(':');

            prodctCodeTextBox.Text = productInfo[0];
            //productNameTextBox.Text = productInfo[1];
           
        }
        else
        {
            showMessageBox("Input Correct Data!!");
        }
    }
}