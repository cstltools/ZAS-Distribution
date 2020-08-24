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

public partial class SInventory_UI_InvoiceCreationForCustomer : System.Web.UI.Page
{
    RequisitionBLL aRequisitionBll = new RequisitionBLL();
    InvoiceBLL aInvoiceBll = new InvoiceBLL();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            aInvoiceBll.PaymentTypeLoadBLL(payTypeDDL);
            Todate();
            InitialGrid();
            DropDownList();
        }
    }
    
    public void Todate()
    {
        invDateTextBox.Text = Convert.ToDateTime(DateTime.Today.ToShortDateString()).ToString("dd-MMM-yyyy");
    }

    public void DropDownList()
    {
        aInvoiceBll.LoadCompany(companyDropDownList);
    }

    private bool Validation()
    {

        if (companyDropDownList.SelectedValue == "")
        {
            showMessageBox("Please Select a Company!!!");
            return false;
        }

        if (dcDropDownList.SelectedValue == "")
        {
            showMessageBox("Please Select a DC!!!");
            return false;
        }

        if (custNameTextBox.Text == "")
        {
            showMessageBox("Customer Name is required!!!");
            return false;
        }

        if (custPhoneNoTextBox.Text == "")
        {
            showMessageBox("Customer phone number is required!!!");
            return false;
        }


        if (custAddressTextBox.Text == "")
        {
            showMessageBox("Customer address is required!!!");
            return false;
        }

        if (payTypeDDL.Text == "")
        {
            showMessageBox("Please Input Payment Type!!");
            return false;
        }
        for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
        {
            if (((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text.Trim() == "" || ((TextBox)gridLineItemGridView.Rows[i].Cells[13].FindControl("tQtyTextBox")).Text.Trim() == "")
            {
                showMessageBox("Please Remove Blank Row!!");
                return false;
            }
        }
        return true;
    }

    private void GetCustInfo(string custCode)
    {
        if (!string.IsNullOrEmpty(custCode))
        {
            DataTable aDataTable = new DataTable();
            aDataTable = aInvoiceBll.CustomerMaster(custCode);
            if (aDataTable.Rows.Count > 0)
            {
                hdComUnitId.Value = aDataTable.Rows[0]["ComUnitId"].ToString();
                hdCustomerMasterId.Value = aDataTable.Rows[0]["CustomerMasterId"].ToString();
                custNameTextBox.Text = aDataTable.Rows[0]["CustomerName"].ToString();
                custAddressTextBox.Text = aDataTable.Rows[0]["Address"].ToString();
                //districtNameTextBox.Text = aDataTable.Rows[0]["DistrictName"].ToString();
                //areaNameTextBox.Text = aDataTable.Rows[0]["AreaName"].ToString();
                //comUnitNameTextBox.Text = aDataTable.Rows[0]["ComUnitCode"].ToString() + ":" + aDataTable.Rows[0]["ComUnitName"].ToString();
                //miaCodeTextBox.Text = aDataTable.Rows[0]["MiaCode"].ToString();
                //marketNameTextBox.Text = aDataTable.Rows[0]["MarketName"].ToString();
                //miaNameTextBox.Text = aDataTable.Rows[0]["MiaName"].ToString();
                //custCategoryTextBox.Text = aDataTable.Rows[0]["CategoryName"].ToString();
                hdMiaId.Value = aDataTable.Rows[0]["MiaId"].ToString();
            }
            else
            {

            }
        }
    }

    protected void custCodeTextBox_TextChanged(object sender, EventArgs e)
    {
        //string custCode = custCodeTextBox.Text.Trim();
        //GetCustInfo(custCode);
    }

    private void InitialGrid()
    {
        DataTable aDataTable = new DataTable();
        aDataTable.Columns.Add("SL");
        aDataTable.Columns.Add("ProductCode");
        aDataTable.Columns.Add("ProductName");
        
        aDataTable.Columns.Add("StockQty");
        aDataTable.Columns.Add("UnitPrice");
        aDataTable.Columns.Add("UnitVAT");

        aDataTable.Columns.Add("Quantity");
       
        aDataTable.Columns.Add("TotalPrice");
        aDataTable.Columns.Add("VAT");

        aDataTable.Columns.Add("DiscountPercentage");
        aDataTable.Columns.Add("DiscountAmount");

        aDataTable.Columns.Add("NetPrice");

        aDataTable.Columns.Add("BonusQty");
        aDataTable.Columns.Add("TotalQty");
        DataRow dataRow;

        dataRow = aDataTable.NewRow();

        dataRow["SL"] = "1";
        dataRow["ProductCode"] = "";
        dataRow["ProductName"] = "";
        dataRow["StockQty"] = "";
        dataRow["UnitPrice"] = "";
        dataRow["UnitVAT"] = "";
        dataRow["Quantity"] = "";
        dataRow["TotalPrice"] = "";
        dataRow["VAT"] = "";
        dataRow["DiscountPercentage"] = "";
        dataRow["DiscountAmount"] = "";
        dataRow["NetPrice"] = "";
        dataRow["BonusQty"] = "";
        dataRow["TotalQty"] = "";
        aDataTable.Rows.Add(dataRow);

        gridLineItemGridView.DataSource = null;
        gridLineItemGridView.DataBind();
        gridLineItemGridView.DataSource = aDataTable;
        gridLineItemGridView.DataBind();

        gridLineItemGridView.Columns[4].Visible = false;
        gridLineItemGridView.Columns[5].Visible = false;

    }

    protected void addImageButton_Click(object sender, ImageClickEventArgs e)
    {
        DataTable aDataTable = new DataTable();
        aDataTable.Columns.Add("SL");
        aDataTable.Columns.Add("ProductCode");
        aDataTable.Columns.Add("ProductName");

        aDataTable.Columns.Add("StockQty");
        aDataTable.Columns.Add("UnitPrice");
        aDataTable.Columns.Add("UnitVAT");

        aDataTable.Columns.Add("Quantity");

        aDataTable.Columns.Add("TotalPrice");
        aDataTable.Columns.Add("VAT");

        aDataTable.Columns.Add("DiscountPercentage");
        aDataTable.Columns.Add("DiscountAmount");

        aDataTable.Columns.Add("NetPrice");

        aDataTable.Columns.Add("BonusQty");
        aDataTable.Columns.Add("TotalQty");
        DataRow dataRow;

        if (gridLineItemGridView.Rows.Count>0)
        {
            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {
                dataRow = aDataTable.NewRow();

                dataRow["SL"] = Convert.ToString(i + 1);
                dataRow["ProductCode"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text.Trim();
                dataRow["ProductName"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[2].FindControl("nameTextBox")).Text.Trim();
                dataRow["StockQty"] =((TextBox)gridLineItemGridView.Rows[i].Cells[3].FindControl("currentStockTextBox")).Text.Trim();
                dataRow["UnitPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[4].FindControl("unitPriceTextBox")).Text.Trim();
                dataRow["UnitVAT"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[5].FindControl("upVatTextBox")).Text.Trim(); ;
                dataRow["Quantity"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[6].FindControl("qtyTextBox")).Text.Trim();
                dataRow["TotalPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim();
                dataRow["VAT"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[8].FindControl("tpVatTextBox")).Text.Trim();
                dataRow["DiscountPercentage"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text.Trim();
                dataRow["DiscountAmount"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text.Trim();
                dataRow["NetPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[11].FindControl("npTextBox")).Text.Trim();
                dataRow["BonusQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[12].FindControl("bQtyTextBox")).Text.Trim();
                dataRow["TotalQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[13].FindControl("tQtyTextBox")).Text.Trim();
                aDataTable.Rows.Add(dataRow);
            }
        }

        int sl = aDataTable.Rows.Count;
        dataRow = aDataTable.NewRow();

        dataRow["SL"] = Convert.ToString(sl + 1);
        dataRow["ProductCode"] = "";
        dataRow["ProductName"] = "";
        dataRow["StockQty"] = "";
        dataRow["UnitPrice"] = "";
        dataRow["UnitVAT"] = "";
        dataRow["Quantity"] = "";
        dataRow["TotalPrice"] = "";
        dataRow["VAT"] = "";
        dataRow["DiscountPercentage"] = "";
        dataRow["DiscountAmount"] = "";
        dataRow["NetPrice"] = "";
        dataRow["BonusQty"] = "";
        dataRow["TotalQty"] = "";
        aDataTable.Rows.Add(dataRow);

        gridLineItemGridView.DataSource = null;
        gridLineItemGridView.DataBind();
        gridLineItemGridView.DataSource = aDataTable;
        gridLineItemGridView.DataBind();
        gridLineItemGridView.Columns[4].Visible = false;
        gridLineItemGridView.Columns[5].Visible = false;
    }
    protected void removeImageButton_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton productCodeTextBox = (ImageButton)sender;
        GridViewRow currentRow = (GridViewRow)productCodeTextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        DataTable aDataTable = new DataTable();
        aDataTable.Columns.Add("SL");
        aDataTable.Columns.Add("ProductCode");
        aDataTable.Columns.Add("ProductName");

        aDataTable.Columns.Add("StockQty");
        aDataTable.Columns.Add("UnitPrice");
        aDataTable.Columns.Add("UnitVAT");

        aDataTable.Columns.Add("Quantity");

        aDataTable.Columns.Add("TotalPrice");
        aDataTable.Columns.Add("VAT");

        aDataTable.Columns.Add("DiscountPercentage");
        aDataTable.Columns.Add("DiscountAmount");

        aDataTable.Columns.Add("NetPrice");

        aDataTable.Columns.Add("BonusQty");
        aDataTable.Columns.Add("TotalQty");
        DataRow dataRow;
        
        if (gridLineItemGridView.Rows.Count > 0)
        {
            int sl1 = 1;
            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {
                if (i != rowindex)
                {
                    dataRow = aDataTable.NewRow();

                    dataRow["SL"] = Convert.ToString(sl1);
                    dataRow["ProductCode"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text.Trim();
                    dataRow["ProductName"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[2].FindControl("nameTextBox")).Text.Trim();
                    dataRow["StockQty"] =
                        ((TextBox)gridLineItemGridView.Rows[i].Cells[3].FindControl("currentStockTextBox")).Text.Trim();
                    dataRow["UnitPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[4].FindControl("unitPriceTextBox")).Text.Trim();
                    dataRow["UnitVAT"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[5].FindControl("upVatTextBox")).Text.Trim(); ;
                    dataRow["Quantity"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[6].FindControl("qtyTextBox")).Text.Trim();
                    dataRow["TotalPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim();
                    dataRow["VAT"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[8].FindControl("tpVatTextBox")).Text.Trim();
                    dataRow["DiscountPercentage"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text.Trim();
                    dataRow["DiscountAmount"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text.Trim();
                    dataRow["NetPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[11].FindControl("npTextBox")).Text.Trim();
                    dataRow["BonusQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[12].FindControl("bQtyTextBox")).Text.Trim();
                    dataRow["TotalQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[13].FindControl("tQtyTextBox")).Text.Trim();
                    aDataTable.Rows.Add(dataRow);
                    sl1 += 1;
                }
            }
        }
        
        gridLineItemGridView.DataSource = null;
        gridLineItemGridView.DataBind();
        gridLineItemGridView.DataSource = aDataTable;
        gridLineItemGridView.DataBind();
        gridLineItemGridView.Columns[4].Visible = false;
        gridLineItemGridView.Columns[5].Visible = false;
    }

    protected void codeTextBox_TextChanged(object sender, EventArgs e)
    {
        TextBox TextBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)TextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        TextBox productCodeTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[1].FindControl("codeTextBox");

        string productCode = productCodeTextBox.Text.Trim();
        GetProduct(rowindex, productCode);
    }

    private void GetProduct(int rowindex, string productCode)
    {
        DataTable aDataTable = new DataTable();
        if (!string.IsNullOrEmpty(productCode))
        {
            if (ProductCodeValidation(productCode, rowindex) == true)
            {


                aDataTable = aInvoiceBll.ProductInfo(hdComUnitId.Value, productCode);
                if (aDataTable.Rows.Count > 0)
                {
                    TextBox nameTextBox =
                        (TextBox) gridLineItemGridView.Rows[rowindex].Cells[2].FindControl("nameTextBox");
                    nameTextBox.Text = aDataTable.Rows[0]["ProductName"].ToString();
                    TextBox currentStockTextBox =
                        (TextBox) gridLineItemGridView.Rows[rowindex].Cells[3].FindControl("currentStockTextBox");
                    currentStockTextBox.Text = aDataTable.Rows[0]["StockQty"].ToString();
                    TextBox unitPriceTextBox =
                        (TextBox) gridLineItemGridView.Rows[rowindex].Cells[4].FindControl("unitPriceTextBox");
                    unitPriceTextBox.Text = aDataTable.Rows[0]["MRPPrice"].ToString();
                    TextBox upVatTextBox =
                        (TextBox) gridLineItemGridView.Rows[rowindex].Cells[5].FindControl("upVatTextBox");
                    upVatTextBox.Text = aDataTable.Rows[0]["VAT"].ToString();
                }
                else
                {
                    ((TextBox)gridLineItemGridView.Rows[rowindex].Cells[2].FindControl("nameTextBox")).Text = "";
                    ((TextBox)gridLineItemGridView.Rows[rowindex].Cells[1].FindControl("codeTextBox")).Text = "";
                    showMessageBox("No Any Stock or Product of " + productCode);
                }
            }
            else
            {
                ((TextBox) gridLineItemGridView.Rows[rowindex].Cells[2].FindControl("nameTextBox")).Text = "";
                ((TextBox)gridLineItemGridView.Rows[rowindex].Cells[1].FindControl("codeTextBox")).Text = "";
                showMessageBox(productCode+" No: Product Already Inserted!!!");
            }
        }
    }

    private bool ProductCodeValidation(string productCode, int rowindex)
    {
       
            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {
                if (rowindex!=i)
                {
                    if (((TextBox) gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text.Trim() ==
                        productCode.Trim())
                    {
                        return false;
                    }
                }
            }

        return true;
    }
    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }
    protected void qtyTextBox_TextChanged(object sender, EventArgs e)
    {
        TextBox qtyTextBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)qtyTextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        decimal qty = 0;
        TextBox qtyTextBox1 = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[6].FindControl("qtyTextBox");
        qty = Convert.ToDecimal(qtyTextBox1.Text.Trim());
        
        TextBox unitPriceTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[4].FindControl("unitPriceTextBox");
        TextBox upVatTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[5].FindControl("upVatTextBox");
      
        TextBox tpTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[7].FindControl("tpTextBox");
        tpTextBox.Text = Convert.ToString(Convert.ToDecimal(unitPriceTextBox.Text.Trim())*qty);

        TextBox tpVatTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[8].FindControl("tpVatTextBox");
        tpVatTextBox.Text = Convert.ToString(Convert.ToDecimal(upVatTextBox.Text.Trim()) * qty);
        
         TextBox codeTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[1].FindControl("codeTextBox");

         decimal discountPer = 0;
         discountPer = aInvoiceBll.ProductDiscount(codeTextBox.Text.Trim(), hdCustomerMasterId.Value, invDateTextBox.Text);

         TextBox dpTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[9].FindControl("dpTextBox");
         dpTextBox.Text = Convert.ToString(discountPer);
         TextBox dpAmtTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[10].FindControl("dpAmtTextBox");
         if (discountPer == 0)
         {
             dpAmtTextBox.Text = "0";
         }
         else
         {
             dpAmtTextBox.Text = ((Convert.ToDecimal(unitPriceTextBox.Text.Trim()) / 100) * discountPer * qty).ToString("F");
         }

        TextBox npTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[11].FindControl("npTextBox");
        npTextBox.Text = Convert.ToString((Convert.ToDecimal(tpTextBox.Text.Trim()) - Convert.ToDecimal(dpAmtTextBox.Text.Trim())) + Convert.ToDecimal(tpVatTextBox.Text.Trim()));

        ((TextBox)gridLineItemGridView.Rows[rowindex].Cells[13].FindControl("tQtyTextBox")).Text = qtyTextBox1.Text.Trim();
        ((TextBox)gridLineItemGridView.Rows[rowindex].Cells[12].FindControl("bQtyTextBox")).Text = "0";
        TotalValueCalculation();
    }
    protected void bQtyTextBox_TextChanged(object sender, EventArgs e)
    {
        TextBox bQtyTextBox1 = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)bQtyTextBox1.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        ((TextBox) gridLineItemGridView.Rows[rowindex].Cells[13].FindControl("tQtyTextBox")).Text =
            Convert.ToString(
                Convert.ToDecimal(
                    ((TextBox) gridLineItemGridView.Rows[rowindex].Cells[12].FindControl("bQtyTextBox")).Text.Trim()) +
                Convert.ToDecimal(
                    ((TextBox) gridLineItemGridView.Rows[rowindex].Cells[13].FindControl("tQtyTextBox")).Text.Trim()));

    }
    protected void saveButton_Click(object sender, EventArgs e)
    {
        if (Validation())
        {
            if (hdCustomerMasterId.Value ==string.Empty)
            {
                hdCustomerMasterId.Value = ConsumeCustomer().ToString();
                
                CustomerMasterInfoDal aCustomerMasterDAL = new CustomerMasterInfoDal();

                aCustomerMasterDAL.CreateGlOfCustomer(Convert.ToInt32(hdCustomerMasterId.Value));
            }
            //OrderSave();
            int invId = SaveInvoice();
            SaveInvoiceDetail(invId);
            aInvoiceBll.UpdateInvoiceDetailConsumer(invId.ToString());
            aInvoiceBll.UpdateInvoiceConsumer(invId.ToString());

            // Journal Posting

            if (aInvoiceBll.PostDeliveryConfFullJournal(invId))
            {
                Clear();
                showMessageBox("Create Invoice");
            }
            
        }
    }

    public void UpdateDeliveryInfo()
    {
        
    }
    public int OrderSaveDetail(int idmain,int i)
    {
        int id = 0;
        //if (id > 0)
        {
            //for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {
                TextBox productCodeTextBox = (TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox");
                OrderInfoDetail aOrderInfoDetail = new OrderInfoDetail()
                {
                    OrderId = idmain,
                    ProductCode = productCodeTextBox.Text,
                    ProductName = ((TextBox)gridLineItemGridView.Rows[i].Cells[2].FindControl("nameTextBox")).Text,
                    Quantity = Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[6].FindControl("qtyTextBox")).Text),
                    Status = "Active",
                    TotalTradePrice = Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[8].FindControl("tpVatTextBox")).Text) + Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text),
                    TradePrice = Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text),

                };
                DataTable aDataTable = aInvoiceBll.ProductInfo(hdComUnitId.Value, productCodeTextBox.Text);
                if (aDataTable.Rows.Count > 0)
                {
                    aOrderInfoDetail.ProductId = Convert.ToInt32(aDataTable.Rows[0]["ProductId"].ToString());
                }
                id=aInvoiceBll.SaveOrderDetail(aOrderInfoDetail);

            }
        }



        return id;
    }
    public int OrderSaveMaster(out string ordercode)
    {
        OrderListBLL aOrderInfoBll = new OrderListBLL();
        string[] comunit = dcDropDownList.SelectedItem.Text.Split(':');
        DataTable dtcust = aInvoiceBll.GetConsumerCustomerInfo(hdCustomerMasterId.Value);
        
        OrderInfoMaster aOrderInfoMaster = new OrderInfoMaster()
        {
            ComUnitId = Convert.ToInt32(dcDropDownList.SelectedValue),
            ComUnitCode = comunit[0],
            ComUnitName = comunit[1],
            CustomerMasterId = Convert.ToInt32(hdCustomerMasterId.Value),
            
            IsDirect = true,
            IsSpecialApproval = false,
            IsInvoice = true,
            IsManual = true,
            EntryDate = DateTime.Now,
            EntryBy = Session["LoginName"].ToString(),
            ApprovedBy = Session["LoginName"].ToString(),
            ApprovedDate = DateTime.Now,
            GrossValue = Convert.ToDecimal(tpTptalTextBox.Text)+Convert.ToDecimal(vatTotalTextBox.Text),
            ActionStatus = "Accepted",
            SubmissionDate = Convert.ToDateTime(invDateTextBox.Text),
            OrderCode =aOrderInfoBll.OrdNo(),
            ManufacId = Convert.ToInt32(companyDropDownList.SelectedValue)
        };
        if (dtcust.Rows.Count>0)
        {
            aOrderInfoMaster.CustomerMasterId = Convert.ToInt32(dtcust.Rows[0]["ConsumerCustId"].ToString());
            aOrderInfoMaster.CustomerCode = dtcust.Rows[0]["CustomerCode"].ToString();
            aOrderInfoMaster.CustomerName = dtcust.Rows[0]["CustomerName"].ToString();
        }
        int id = aInvoiceBll.SaveOrderMaster(aOrderInfoMaster);
        ordercode = aOrderInfoMaster.OrderCode;
        return id;

    }

    public int ConsumeCustomer()
    {
        int id = 0;
        ConsumerCustomerDAO aConsumerCustomerDao = new ConsumerCustomerDAO()
        {
            CustomerAddress = custAddressTextBox.Text,
            CustomerName = custNameTextBox.Text,
            CustomerPhoneNo = custPhoneNoTextBox.Text,
            
        };
        bool status= aInvoiceBll.SaveConsumerCustomer(aConsumerCustomerDao,out id);
        return id;
    }

    private bool SaveInvoiceDetail(int invoiceId)
    {
        
        List<InvoiceDetail> aInvoiceDetailsList = new List<InvoiceDetail>();

        if (gridLineItemGridView.Rows.Count>0)
        {
            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {
                int orderdetailId = 0;
                orderdetailId = OrderSaveDetail(Convert.ToInt32(orderIdHiddenField.Value), i);
                //InvoiceDetail aInvoiceDetail = new InvoiceDetail();
                //aInvoiceDetail.ProductCode =
                //    ((TextBox) gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text;
                //string product = ((TextBox)gridLineItemGridView.Rows[i].Cells[2].FindControl("nameTextBox")).Text;
                //string[] proNameAndPackSize = product.Split(':');
                //aInvoiceDetail.ProductName = proNameAndPackSize[0];
                //aInvoiceDetail.PackSize = proNameAndPackSize[1];
                //aInvoiceDetail.UnitPrice =Convert.ToDecimal(((TextBox) gridLineItemGridView.Rows[i].Cells[4].FindControl("unitPriceTextBox")).Text);
                //aInvoiceDetail.UnitVatAmount =Convert.ToDecimal(((TextBox) gridLineItemGridView.Rows[i].Cells[5].FindControl("upVatTextBox")).Text);
                //aInvoiceDetail.Quantity = Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[6].FindControl("qtyTextBox")).Text);
                //aInvoiceDetail.DiscountPercentage = Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text);
                //aInvoiceDetail.BonusQuantity = Convert.ToDecimal(((TextBox) gridLineItemGridView.Rows[i].Cells[12].FindControl("bQtyTextBox")).Text.Trim());
                //aInvoiceDetail.InvoiceId = invoiceId;
                //aInvoiceDetail.OrderDetailsId = orderdetailId;
                InvoiceDetail aInvoiceDetail = new InvoiceDetail();
                aInvoiceDetail.ProductCode =
                    ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text;
                string product = ((TextBox)gridLineItemGridView.Rows[i].Cells[2].FindControl("nameTextBox")).Text;
                string[] proNameAndPackSize = product.Split(':');
                aInvoiceDetail.ProductName = proNameAndPackSize[0];
                aInvoiceDetail.PackSize = proNameAndPackSize[1];
                aInvoiceDetail.UnitPrice = Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[4].FindControl("unitPriceTextBox")).Text);
                aInvoiceDetail.UnitVatAmount = Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[5].FindControl("upVatTextBox")).Text);
                aInvoiceDetail.Quantity = Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[6].FindControl("qtyTextBox")).Text);
                aInvoiceDetail.DiscountPercentage = 0;
                aInvoiceDetail.DiscountAmount = 0;
                aInvoiceDetail.BonusQuantity = 0;
                aInvoiceDetail.ReturnReason = "";
                //aInvoiceDetail.SpecialAmount = 0;
                //Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("sdTextBox")).Text.Trim());
                //string id =
                //    ((HiddenField)gridLineItemGridView.Rows[i].Cells[1].FindControl("orderdetailIdHiddenField")).Value;
                aInvoiceDetail.OrderDetailsId = orderdetailId;
                aInvoiceDetail.InvoiceId = invoiceId;
                aInvoiceDetail.TotalPriceVatAmount =
                    Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[8].FindControl("tpVatTextBox")).Text);
                TextBox npTextBox = (TextBox)gridLineItemGridView.Rows[i].Cells[11].FindControl("npTextBox");
                aInvoiceDetail.NetAmount = Convert.ToDecimal(npTextBox.Text);
                //if (dtdiscount.Rows.Count > 0)
                {
                    aInvoiceDetail.SpecialAmount = Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text);
                    aInvoiceDetail.SpecialAmountPer = 0;

                }
                //else
                //{
                //    aInvoiceDetail.SpecialAmountPer = 0;
                //}
                
                aInvoiceDetailsList.Add(aInvoiceDetail);
            }

            aInvoiceBll.SaveInvoiceDetails(aInvoiceDetailsList, hdComUnitId.Value);
            
        }

        return true;
    }

    private int SaveInvoice()
    {

        int invoiceId=0;
        string orderno = "";
        string invoiceNo = string.Empty;
        string[] forComUCode = dcDropDownList.SelectedItem.Text.Split(':');
        string ComUnitCode = forComUCode[0];
        int orderId = OrderSaveMaster(out orderno);
        orderIdHiddenField.Value = orderId.ToString();
        Invoice aInvoice = new Invoice()
                               {
                                   InvoiceDate = Convert.ToDateTime(invDateTextBox.Text.Trim()),
                                   OrderNo = orderno,
                                   OrderDate = Convert.ToDateTime(invDateTextBox.Text.Trim()),
                                   CustomerMasterId = Convert.ToInt32(hdCustomerMasterId.Value),
                                   ComUnitId=Convert.ToInt32(dcDropDownList.SelectedValue),
                                   //MiaId = Convert.ToInt32(hdMiaId.Value),
                                   PaymentTypeId=Convert.ToInt32(payTypeDDL.SelectedValue),
                                   TpTotal = Convert.ToDecimal(tpTptalTextBox.Text.Trim()),
                                   TpDiscount = Convert.ToDecimal(disTotalTextBox.Text.Trim()),
                                   TpVat=Convert.ToDecimal(vatTotalTextBox.Text.Trim()),
                                   TpGrandTotal=Convert.ToDecimal(grandTotalTextBox.Text.Trim()),
                                   UserId = Convert.ToInt32(Session["UserId"].ToString()),
                                   ComUnitCode = ComUnitCode,
                                   OrderId = orderId,
                                   TotalSpecialAmount = Convert.ToDecimal(disTotalTextBox.Text),
                                   
                               };

       aInvoiceBll.SaveInvoiceConsumer(aInvoice,out invoiceId,out invoiceNo);
        aInvoice.InvoiceId = invoiceId;
        aInvoice.DelivaryInvoiceNo = "DEL-" + invoiceNo;
        aInvoice.UpdateBy = Session["LoginName"].ToString();
        aInvoice.DeliveryInvoiceStatus = "Full";
        aInvoice.UpdateDate = DateTime.Now;
        aInvoiceBll.UpdateInvoice(aInvoice, out invoiceNo);
        invTextBox.Text = invoiceNo;


       return invoiceId;
    }
    private void Clear()
    {
        tpTptalTextBox.Text = "";
        vatTotalTextBox.Text = "";
        disTotalTextBox.Text = "";
        grandTotalTextBox.Text = "";
        hdComUnitId.Value = "";
        hdCustomerMasterId.Value = "";
        custNameTextBox.Text = "";
        custAddressTextBox.Text = "";
        hdMiaId.Value = "";
        

        InitialGrid();
    }
    
    private void TotalValueCalculation()
    {
        decimal tpTotal = 0;
        decimal vatTotal = 0;
        decimal disTotal = 0;
        decimal gTotal = 0;
        if (gridLineItemGridView.Rows.Count > 0)
        {
           
            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {
                tpTotal+= (((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text!="")? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text):0;
                vatTotal += (((TextBox)gridLineItemGridView.Rows[i].Cells[8].FindControl("tpVatTextBox")).Text != "") ? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[8].FindControl("tpVatTextBox")).Text) : 0;
                disTotal += (((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text != "") ? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text) : 0;
                gTotal += (((TextBox)gridLineItemGridView.Rows[i].Cells[11].FindControl("npTextBox")).Text != "") ? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[11].FindControl("npTextBox")).Text) : 0;
            }
        }
        tpTptalTextBox.Text = tpTotal.ToString();
        vatTotalTextBox.Text = vatTotal.ToString();
        disTotalTextBox.Text = disTotal.ToString();
        grandTotalTextBox.Text = gTotal.ToString();
    }
    protected void printButton_Click(object sender, EventArgs e)
    {
        string url = "../SInventory_RPTVIEW/InvoiceReportViewer.aspx?InvNo=" + Server.UrlEncode(invTextBox.Text.Trim());
        // string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=900,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }
    protected void custNameTextBox_TextChanged(object sender, EventArgs e)
    {

        string custName = custNameTextBox.Text.Trim();
        if (!string.IsNullOrEmpty(custName))
        {
            if (custName.Contains(':'))
            {

                string[] custInfo = custName.Split(':');
                //custCodeTextBox.Text = custInfo[0];
                //string custCode = custCodeTextBox.Text.Trim();
                //GetCustInfo(custCode);
            }
        }       
    }
    protected void nameTextBox_TextChanged(object sender, EventArgs e)
    {
        TextBox TextBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)TextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        string product = ((TextBox)gridLineItemGridView.Rows[rowindex].Cells[2].FindControl("nameTextBox")).Text;

        if (!string.IsNullOrEmpty(product))
        {
            if (product.Contains(':'))
            {
                string[] proNameAndPackSize = product.Split(':');


                TextBox productCodeTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[1].FindControl("codeTextBox");
                productCodeTextBox.Text = proNameAndPackSize[1];
                string productCode = productCodeTextBox.Text.Trim();
                GetProduct(rowindex, productCode);
            }
            
        }
    }
    protected void companyDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        aInvoiceBll.LoadDC(dcDropDownList,companyDropDownList.SelectedValue);

        if (companyDropDownList.SelectedValue != "")
        {
            foreach (GridViewRow row in gridLineItemGridView.Rows)
                {
                    TextBox productTextBox = (TextBox)gridLineItemGridView.Rows[row.RowIndex].Cells[3].FindControl("nameTextBox");
                    AjaxControlToolkit.AutoCompleteExtender modal = (AjaxControlToolkit.AutoCompleteExtender)productTextBox.FindControl("nameTextBox_AutoCompleteExtender");
                    modal.ContextKey = companyDropDownList.SelectedValue;
                }
        }
    }

    protected void custPhoneNoTextBox_OnTextChanged(object sender, EventArgs e)
    {
        if (custPhoneNoTextBox.Text.Contains(':'))
        {
            string[] cust = custPhoneNoTextBox.Text.Split(':');
            custPhoneNoTextBox.Text = cust[0];
            hdCustomerMasterId.Value = cust[2];
            ConsumeCustomer(hdCustomerMasterId.Value);
        }
    }

    public void ConsumeCustomer(string custId)
    {
        DataTable dt = aInvoiceBll.GetConsumerCustomerInfo(custId);
        if (dt.Rows.Count>0)
        {
            custNameTextBox.Text = dt.Rows[0]["CustomerName"].ToString();
            custAddressTextBox.Text = dt.Rows[0]["CustomerAddress"].ToString();
        }
    }
    protected void dcDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        hdComUnitId.Value = dcDropDownList.SelectedValue;
    }

    protected void dpAmtTextBox_OnTextChanged(object sender, EventArgs e)
    {
        TextBox qtyTextBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)qtyTextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        decimal qty = 0;
        TextBox qtyTextBox1 = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[6].FindControl("qtyTextBox");
        qty = Convert.ToDecimal(qtyTextBox1.Text.Trim());

        TextBox unitPriceTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[4].FindControl("unitPriceTextBox");
        TextBox upVatTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[5].FindControl("upVatTextBox");

        TextBox tpTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[7].FindControl("tpTextBox");
        tpTextBox.Text = Convert.ToString(Convert.ToDecimal(unitPriceTextBox.Text.Trim()) * qty);

        TextBox tpVatTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[8].FindControl("tpVatTextBox");
        tpVatTextBox.Text = Convert.ToString(Convert.ToDecimal(upVatTextBox.Text.Trim()) * qty);

        TextBox codeTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[1].FindControl("codeTextBox");

        decimal discountPer = 0;
        //discountPer = aInvoiceBll.ProductDiscount(codeTextBox.Text.Trim(), qtyTextBox1.Text.Trim());

        TextBox dpTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[9].FindControl("dpTextBox");
        dpTextBox.Text = Convert.ToString(discountPer);
        TextBox dpAmtTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[10].FindControl("dpAmtTextBox");
        //if (discountPer == 0)
        //{
        //    dpAmtTextBox.Text = "0";
        //}
        //else
        //{
        //    dpAmtTextBox.Text = Convert.ToString((Convert.ToDecimal(tpTextBox.Text.Trim()) / 100) * discountPer);
        //}

        TextBox npTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[11].FindControl("npTextBox");
        npTextBox.Text = Convert.ToString((Convert.ToDecimal(tpTextBox.Text.Trim()) - Convert.ToDecimal(dpAmtTextBox.Text.Trim())) + Convert.ToDecimal(tpVatTextBox.Text.Trim()));

        ((TextBox)gridLineItemGridView.Rows[rowindex].Cells[13].FindControl("tQtyTextBox")).Text = qtyTextBox1.Text.Trim();
        ((TextBox)gridLineItemGridView.Rows[rowindex].Cells[12].FindControl("bQtyTextBox")).Text = "0";
        TotalValueCalculation();
    }
}