using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Transactions;
using Library.BLL.SInventory_BLL;
using Library.CrystalReports.SInventory_DS;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_InvoiceCreationForCustomer : System.Web.UI.Page
{
    RequisitionBLL aRequisitionBll = new RequisitionBLL();
    InvoiceBLL aInvoiceBll = new InvoiceBLL();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Todate();
            DeliveryMan();
            aInvoiceBll.PaymentTypeLoadBLL(payTypeDDL);

            if (Session["OrderId"] !=null)
            {
                orderHiddenField.Value = Session["OrderId"].ToString();
                LoadAllDataByOrder(Session["OrderId"].ToString());
                Session["OrderId"] = null;
            }



            if (hdCustomerMasterId.Value != "")
            {
                DataTable aTable = aInvoiceBll.LoadCustomerPayType(hdCustomerMasterId.Value);

                if (aTable.Rows.Count > 0)
                {

                    if (aTable.Rows[0].Field<String>("TermOfPayment") == "Credit")
                    {
                        payTypeDDL.SelectedIndex = 2;
                    }
                    else
                    {
                        payTypeDDL.SelectedIndex = 1;
                    }
                    
                }
                payTypeDDL.Enabled = false;
            }

            
            Todate();
        }
    }


    public void DeliveryMan()
    {
        aInvoiceBll.LoadDeliveryMan(deliverypersonNameTextBox);
        
    }
    public void Todate()
    {
        invDateTextBox.Text = Convert.ToDateTime(DateTime.Today.ToShortDateString()).ToString("dd-MMM-yyyy");
    }

    private bool Validation()
    {

        if (invDateTextBox.Text == "")
        {
            showMessageBox(" You should select invoice Date !!");
            return false;
        }

         //DataTable aTable = aInvoiceBll.GetFinancialYearInfo(invDateTextBox.Text, hdComUnitId.Value);

         //if (aTable.Rows.Count == 0)
         // {
         //       showMessageBox("You should declare financial year !!");
         //       return false;
         // }
            

        if (Convert.ToDecimal(tpTptalTextBox.Text.Trim()) == 0)
        {
            showMessageBox("Product not avilable for invoice generate !!");
            return false;
        }
        if (orderNoTextBox.Text == "")
        {
            showMessageBox("Please Input Order Number!!");
            return false;
        }
        if (orderDateTextBox.Text == "")
        {
            showMessageBox("Please Input Order Date!!");
            return false;
        }

        if (payTypeDDL.Text == "")
        {
            showMessageBox("Please Input Payment Type!!");
            return false;
        }

        if (deliverypersonNameTextBox.SelectedIndex == 0)
        {
            showMessageBox("Please Input Delivery Person!!");
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

            hdfCustomer.Value = "";
            custCodeTextBox.Text = custCode;
            DataTable aDataTable = new DataTable();
            aDataTable = aInvoiceBll.CustomerMaster(custCode);
            if (aDataTable.Rows.Count > 0)
            {
                hdComUnitId.Value = aDataTable.Rows[0]["ComUnitId"].ToString();
                hdCustomerMasterId.Value = aDataTable.Rows[0]["CustomerMasterId"].ToString();
                custNameTextBox.Text = aDataTable.Rows[0]["CustomerName"].ToString();
                custAddressTextBox.Text = aDataTable.Rows[0]["Address"].ToString();
                //districtNameTextBox.Text = aDataTable.Rows[0]["DistrictName"].ToString();
                areaNameTextBox.Text = aDataTable.Rows[0]["AreaName"].ToString();
                comUnitNameTextBox.Text = aDataTable.Rows[0]["ComUnitCode"].ToString() + ":" + aDataTable.Rows[0]["ComUnitName"].ToString();
                //miaCodeTextBox.Text = aDataTable.Rows[0]["EmpMasterCode"].ToString();
                //hdMiaId.Value = aDataTable.Rows[0]["MIOId"].ToString();
                marketNameTextBox.Text = aDataTable.Rows[0]["MarketName"].ToString();
                //miaNameTextBox.Text = aDataTable.Rows[0]["EmpName"].ToString();
                custCategoryTextBox.Text = aDataTable.Rows[0]["CategoryName"].ToString();
                //hdMiaId.Value = aDataTable.Rows[0]["MIOId"].ToString();

                hdfCustomer.Value = custCodeTextBox.Text + " - " + custNameTextBox.Text;
            }
            else
            {

            }
        }
    }

    public bool OrderExists(string orderId)
    {
        OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();
        DataTable aTable = new DataTable();
        aTable = aOrderInfoBll.LoadOrderExistsBll(orderId);
        if (aTable.Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    //private bool ChkProductOfferFlexidolParagesic()
    //{
    //    for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
    //    {
    //        string ProductCode = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text;
    //        int Qty = Convert.ToInt32(((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("qtyTextBox")).Text);
    //        if (ProductCode == "AID01" && Qty >= 5)
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}
    //private bool ChkProductOffer2FlexidolParagesic()
    //{
    //    for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
    //    {
    //        string ProductCode = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text;
    //        int Qty = Convert.ToInt32(((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("qtyTextBox")).Text);
    //        if (ProductCode == "ANA02" && Qty >= 5)
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    private bool ChkProductOffer()
    {
        for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
        {
            string ProductCode = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text;
            int Qty = !string.IsNullOrEmpty(((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("qtyTextBox")).Text)
     ? Convert.ToInt32(((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("qtyTextBox")).Text.Trim())
     : 0;

            if (ProductCode == "AID01" && Qty >= 2)
            {
                return true;
            }
        }
        return false;
    }


    //private bool ChkProductOfferEsomium20()
    //{
    //    //  int TotalRecord = gridLineItemGridView.Rows.Count;&& TotalRecord == 1
    //    for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
    //    {
    //        string ProductCode = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text;
    //        int Qty = Convert.ToInt32(((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("qtyTextBox")).Text);
    //        if (ProductCode == "ARD02" && Qty >= 8 )
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    public void LoadAllDataByOrder(string orderId)
    {
        if (OrderExists(orderHiddenField.Value) == false)
        {
            OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();
            DataTable aTable = new DataTable();

            aTable = aOrderInfoBll.LoadOrderWithDetail(orderId);

            orderNoTextBox.Text = aTable.Rows[0]["OrderCode"].ToString();
            orderIdHiddenField.Value = aTable.Rows[0]["OrderId"].ToString();
            orderDateTextBox.Text = Convert.ToDateTime(aTable.Rows[0]["SubmissionDate"].ToString()).ToString("dd-MMM-yyyy");
            GetCustInfo(aTable.Rows[0]["CustomerCode"].ToString());
            comUnitNameTextBox.Text = aTable.Rows[0]["ComUnitName"].ToString();
            hdComUnitId.Value = aTable.Rows[0]["ComUnitId"].ToString();
            isVatApplicable.Value = aTable.Rows[0].Field<bool>("IsVatApplicable").ToString(CultureInfo.InvariantCulture);
            hdgrossValue.Value = aTable.Rows[0].Field<Decimal>("GrossValue").ToString(CultureInfo.InvariantCulture);

            miaCodeTextBox.Text = aTable.Rows[0]["MIOCode"].ToString();
            miaNameTextBox.Text = aTable.Rows[0]["MIOName"].ToString();
            hdMiaId.Value = aTable.Rows[0]["MIOId"].ToString();

            decimal discountPerRow = 0;
            decimal percentagePerRow = 0;

            if (aTable.Rows.Count > 0)
            {
                DataTable fixedDiscount = aInvoiceBll.GetFixedDiscount(hdCustomerMasterId.Value, hdComUnitId.Value);
                if (fixedDiscount.Rows.Count > 0)
                {
                    percentagePerRow = fixedDiscount.Rows[0].Field<decimal>("DiscountParcentage") / aTable.Rows.Count;
                    discountPerRow = (Convert.ToDecimal(hdgrossValue.Value) * fixedDiscount.Rows[0].Field<decimal>("DiscountParcentage") / 100) / aTable.Rows.Count;
                }
            }

            // Add Product

            for (int i = 0; i < aTable.Rows.Count; i++)
            {
                AddFunc();
                ((HiddenField)gridLineItemGridView.Rows[i].Cells[1].FindControl("orderdetailIdHiddenField")).Value =
                    aTable.Rows[i]["OrderDetailId"].ToString();

                GetProduct(i, aTable.Rows[i]["ProductCode"].ToString());

                decimal cstock = 0;
                if (((TextBox)gridLineItemGridView.Rows[i].Cells[3].FindControl("currentStockTextBox")).Text != "")
                {
                    cstock = Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[3].FindControl("currentStockTextBox")).Text);
                }

                if (cstock < Convert.ToDecimal(aTable.Rows[i]["Quantity"].ToString()))
                {
                    gridLineItemGridView.Rows[i].BackColor = Color.Red;
                    GetQty(cstock, i, discountPerRow, percentagePerRow);
                }
                else
                {
                    GetQty(Convert.ToDecimal(aTable.Rows[i]["Quantity"].ToString()), i, discountPerRow, percentagePerRow);
                }
            }

            AddCampaignProduct();

            decimal totalprice = Convert.ToDecimal(tpTptalTextBox.Text);
            decimal percentage = 0;

            {
                DataTable dttradepolicy = aOrderInfoBll.GetTradeTerm(totalprice.ToString());
                if (dttradepolicy.Rows.Count > 0)
                {
                    percentage = Convert.ToDecimal(dttradepolicy.Rows[0]["DiscountPerc"].ToString());
                }
            }

            decimal totaldiscount = 0;
            totaldiscount = (percentage * Convert.ToDecimal(tpTptalTextBox.Text)) / 100;

            disTotalTextBox.Text = (string.IsNullOrEmpty(disTotalTextBox.Text) ? 0 : Convert.ToDecimal(disTotalTextBox.Text) + totaldiscount).ToString();

            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {
                ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("sdTextBox")).Text = "0.00";

                decimal tqty = !string.IsNullOrEmpty(((TextBox)gridLineItemGridView.Rows[i].Cells[14].FindControl("tQtyTextBox")).Text)
                    ? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[14].FindControl("tQtyTextBox")).Text.Trim())
                    : 0m;

                decimal cstock = !string.IsNullOrEmpty(((TextBox)gridLineItemGridView.Rows[i].Cells[3].FindControl("currentStockTextBox")).Text)
                    ? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[3].FindControl("currentStockTextBox")).Text.Trim())
                    : 0m;

                if (cstock == 0 || cstock < tqty)
                {
                    gridLineItemGridView.Rows[i].BackColor = Color.Red;
                    GetQty(cstock, i, 0, 0);
                }
                else
                {
                    DataTable dtproductvat = aOrderInfoBll.ProductVat(
                        ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text);

                    DataTable dtdiscount = aOrderInfoBll.ProductDiscount(
                        ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text,
                        hdCustomerMasterId.Value, invDateTextBox.Text);

                    decimal percamount = 0;
                    if (dtdiscount.Rows.Count > 0)
                    {
                        percamount = Convert.ToDecimal(dtdiscount.Rows[0]["DiscountPercentage"].ToString());
                    }

                    decimal totalamount = Convert.ToDecimal(tpTptalTextBox.Text);

                    decimal productamount = !string.IsNullOrEmpty(((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text)
                        ? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim())
                        : 0m;

                    decimal productperc = 0;
                    decimal mainper = (percentage * productperc) / 100;

                    if (((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text == "AID01" && ChkProductOffer() == true)
                    {
                        ((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text = 0.ToString();
                        ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text = 0.ToString("F");
                    }

                    decimal withdiscount = (
                        !string.IsNullOrEmpty(((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text)
                            ? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim())
                            : 0m
                    )
                    -
                    (
                        !string.IsNullOrEmpty(((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text)
                            ? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text.Trim())
                            : 0m
                    )
                    -
                    (
                        !string.IsNullOrEmpty(((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("sdTextBox")).Text)
                            ? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("sdTextBox")).Text.Trim())
                            : 0m
                    );

                    TextBox tpVatTextBox = (TextBox)gridLineItemGridView.Rows[i].Cells[8].FindControl("tpVatTextBox");
                    TextBox npTextBox = (TextBox)gridLineItemGridView.Rows[i].Cells[11].FindControl("npTextBox");

                    decimal tp = !string.IsNullOrEmpty(((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text)
                        ? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim())
                        : 0m;

                    decimal dpAmt = !string.IsNullOrEmpty(((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text)
                        ? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text.Trim())
                        : 0m;

                    decimal sd = !string.IsNullOrEmpty(((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("sdTextBox")).Text)
                        ? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("sdTextBox")).Text.Trim())
                        : 0m;

                    decimal tpVat = !string.IsNullOrEmpty(tpVatTextBox.Text)
                        ? Convert.ToDecimal(tpVatTextBox.Text.Trim())
                        : 0m;

                    npTextBox.Text = (tp - dpAmt - sd + tpVat).ToString();

                    TotalValueCalculation();
                }
            }

            if (B() == true)
            {
                addImageButton_Click(this, null);
            }

            GetDiscounttotalValue();

            if (gridLineItemGridView.Rows.Count > 0)
            {
                if (Session["LoginName"].ToString() == "rezaul")
                {
                    for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
                    {
                        ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("sdTextBox")).ReadOnly = false;
                    }
                }
            }
        }
        else
        {
            showMessageBox("Order Information already Exists !!");
        }
    }


    private void GetOrderDetailValue(string orderId)
    {
        OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();
        DataTable aTable = new DataTable();
        aTable = aOrderInfoBll.LoadOrderWithDetail(orderId);

        for (int i = 0; i < aTable.Rows.Count; i++)
        {
            ((HiddenField) gridLineItemGridView.Rows[i].Cells[1].FindControl("orderdetailIdHiddenField")).Value =
                aTable.Rows[i]["OrderDetailId"].ToString();
        }
    }

    private void GetDiscounttotalValue()
    {
        decimal disTotal = 0;
        decimal tqty = 0;

        for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
        {
             tqty = !string.IsNullOrEmpty(((TextBox)gridLineItemGridView.Rows[i].Cells[14].FindControl("tQtyTextBox")).Text)
    ? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[14].FindControl("tQtyTextBox")).Text.Trim())
    : 0m;


            //if (((TextBox) gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text == "FGD02" && tqty >= 3)
            //{
            //    disTotal = 0;
            //    //(((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text != "") ? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text) : 0;
            //}
            //else
            {
                disTotal += (((TextBox) gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text != "")
                    ? Convert.ToDecimal(((TextBox) gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text)
                    : 0;
            }
        }


        disTotalTextBox.Text = disTotal.ToString();
    }

    //FOC Bonus Qty Start//
    private bool B()
    {
        for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
        {
            string ProductCode = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text;
            
            int Qty = !string.IsNullOrEmpty(((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("qtyTextBox")).Text)
            ? Convert.ToInt32(((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("qtyTextBox")).Text.Trim())
            : 0;

            if (ProductCode == "AID01" && Qty >= 2)
            {

                return true;
            }
        }
        return false;
    }
    //FOC Bonus Qty End//

    protected void custCodeTextBox_TextChanged(object sender, EventArgs e)
    {
        string custCode = custCodeTextBox.Text.Trim();
        GetCustInfo(custCode);
    }

    private void InitialGrid()
    {
        DataTable aDataTable = new DataTable();
        aDataTable.Columns.Add("SL");
        aDataTable.Columns.Add("ProductCode");
        aDataTable.Columns.Add("ProductName");
        aDataTable.Columns.Add("OrderDetailsId");
        aDataTable.Columns.Add("StockQty");
        aDataTable.Columns.Add("SpecialAmount");
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
        dataRow["SpecialAmount"] = "";
        dataRow["OrderDetailsId"] = "";
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

    protected void sdTextBox_OnTextChanged(object sender, EventArgs e)
    {
        TotalValueCalculation();

        CalculateGrandTotal();
    }

    private void CalculateGrandTotal()
    {
        if (pdTextBox.Text != "" && grandTotalTextBox.Text != "")
        {
            Decimal ttldiscount = Convert.ToDecimal(pdTextBox.Text.Trim());
            Decimal grandTtl = Convert.ToDecimal(grandTotalTextBox.Text.Trim());
            Decimal total = 0;

            if (grandTtl > ttldiscount)
            {
                total = grandTtl - ttldiscount;
            }
            else
            {
                total = ttldiscount - grandTtl;
            }

            grandTotalTextBox.Text = total.ToString(CultureInfo.InvariantCulture);

        }
    }

    private void TotalProformaValueCalculation()
    {
        decimal tpTotal = 0;
        decimal vatTotal = 0;
        decimal disTotal = 0;
        decimal gTotal = 0;
        decimal sptotatl = 0;
        if (gridLineItemGridView.Rows.Count > 0)
        {
            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {
                decimal delqty = 0;
                delqty = string.IsNullOrEmpty(((TextBox)gridLineItemGridView.Rows[i].Cells[14].FindControl("dQtyTextBox")).Text) ? 0 :
                    Convert.ToDecimal(
                        ((TextBox)gridLineItemGridView.Rows[i].Cells[14].FindControl("dQtyTextBox")).Text);
                TextBox unitPriceTextBox =
                    (TextBox)gridLineItemGridView.Rows[i].Cells[4].FindControl("unitPriceTextBox");

                ((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text =
                    (Convert.ToDecimal(unitPriceTextBox.Text) * delqty).ToString();

                tpTotal += (((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text != "")
                    ? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text)
                    : 0;
                vatTotal += (((TextBox)gridLineItemGridView.Rows[i].Cells[8].FindControl("tpVatTextBox")).Text != "")
                    ? Convert.ToDecimal(
                        ((TextBox)gridLineItemGridView.Rows[i].Cells[8].FindControl("tpVatTextBox")).Text)
                    : 0;
                disTotal += (((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text != "")
                    ? Convert.ToDecimal(
                        ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text)
                    : 0;
                gTotal += (((TextBox)gridLineItemGridView.Rows[i].Cells[11].FindControl("npTextBox")).Text != "")
                    ? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[11].FindControl("npTextBox")).Text)
                    : 0;
                sptotatl += (((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("sdTextBox")).Text != "")
                    ? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("sdTextBox")).Text)
                    : 0;
            }
        }
        tpTptalTextBox.Text = tpTotal.ToString();
        vatTotalTextBox.Text = vatTotal.ToString();
        disTotalTextBox.Text = disTotal.ToString();
        grandTotalTextBox.Text = gTotal.ToString();
        pdTextBox.Text = sptotatl.ToString();
    }

    public void AddFunc()
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
        aDataTable.Columns.Add("SpecialAmount");

        aDataTable.Columns.Add("BonusQty");
        aDataTable.Columns.Add("TotalQty");
        aDataTable.Columns.Add("OrderDetailsId");
        DataRow dataRow;

        if (gridLineItemGridView.Rows.Count > 0)
        {
            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {
                dataRow = aDataTable.NewRow();

                dataRow["SL"] = Convert.ToString(i + 1);
                dataRow["ProductCode"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text.Trim();
                dataRow["OrderDetailsId"] = ((HiddenField)gridLineItemGridView.Rows[i].Cells[1].FindControl("orderdetailIdHiddenField")).Value.Trim();
                dataRow["SpecialAmount"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("sdTextBox")).Text.Trim();
                dataRow["ProductName"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[2].FindControl("nameTextBox")).Text.Trim();
                dataRow["StockQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[3].FindControl("currentStockTextBox")).Text.Trim();
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
        dataRow["OrderDetailsId"] = "";
        dataRow["ProductName"] = "";
        dataRow["StockQty"] = "";
        dataRow["UnitPrice"] = "";
        dataRow["UnitVAT"] = "";
        dataRow["Quantity"] = "";
        dataRow["TotalPrice"] = "";
        dataRow["VAT"] = "";
        dataRow["DiscountPercentage"] = "";
        dataRow["DiscountAmount"] = "";
        dataRow["SpecialAmount"] = "";
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
        aDataTable.Columns.Add("OrderDetailsId");
        aDataTable.Columns.Add("SpecialAmount");

        DataRow dataRow;

        if (gridLineItemGridView.Rows.Count > 0)
        {
            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {
                dataRow = aDataTable.NewRow();

                dataRow["SL"] = Convert.ToString(i + 1);
                dataRow["ProductCode"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text.Trim();
                dataRow["ProductName"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[2].FindControl("nameTextBox")).Text.Trim();
                dataRow["StockQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[3].FindControl("currentStockTextBox")).Text.Trim();
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
                dataRow["OrderDetailsId"] = ((HiddenField)gridLineItemGridView.Rows[i].Cells[1].FindControl("orderdetailIdHiddenField")).Value;
                aDataTable.Rows.Add(dataRow);
            }
        }

        decimal cs = 0;
        DataTable aDataTable2 = new DataTable();
        aDataTable2 = aInvoiceBll.ProductInfo(hdComUnitId.Value, "OAD01");

        if (aDataTable2.Rows.Count > 0)
        {
            cs = Convert.ToDecimal(aDataTable2.Rows[0]["StockQty"].ToString());
        }


        int sl = aDataTable.Rows.Count;
        dataRow = aDataTable.NewRow();

        dataRow["SL"] = Convert.ToString(sl + 1);
        dataRow["ProductCode"] = "AID01";
        dataRow["ProductName"] = "Flexidol Tablet~100mg:10 X 10s ";
        dataRow["StockQty"] = cs;
        dataRow["UnitPrice"] = "0";
        dataRow["UnitVAT"] = "0";
        dataRow["Quantity"] = Qty();
        dataRow["TotalPrice"] = "0";
        dataRow["VAT"] = "0";
        dataRow["DiscountPercentage"] = "0";
        dataRow["DiscountAmount"] = "0";
        dataRow["NetPrice"] = "0";
        dataRow["BonusQty"] = "0";
        dataRow["TotalQty"] = Qty();
        dataRow["OrderDetailsId"] = "0";
        dataRow["SpecialAmount"] = "0";

        aDataTable.Rows.Add(dataRow);

        gridLineItemGridView.DataSource = null;
        gridLineItemGridView.DataBind();
        gridLineItemGridView.DataSource = aDataTable;
        gridLineItemGridView.DataBind();
        gridLineItemGridView.Columns[4].Visible = false;
        gridLineItemGridView.Columns[5].Visible = false;
    }
    private int Qty()
    {
        int Bqty = 0;

        for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
        {
            string ProductCode = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text;
            int Qty = Convert.ToInt32(((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("qtyTextBox")).Text);

            DataTable aDataTableBQty = new DataTable();
            if (ProductCode == "AID01")
            {
                aDataTableBQty = aInvoiceBll.ProductFocBonusQtyBLL(Convert.ToDateTime(DateTime.Today.ToShortDateString()).ToString("dd-MMM-yyyy"), ProductCode, Qty);
                if (aDataTableBQty.Rows.Count > 0)
                {
                    return Bqty = Convert.ToInt16(aDataTableBQty.Rows[0]["BonusQty"]);
                }
            }
        }
        return 0;
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
                aDataTable = aInvoiceBll.ProductInfo(hdComUnitId.Value, productCode,custCodeTextBox.Text);
                ((TextBox) gridLineItemGridView.Rows[rowindex].Cells[1].FindControl("codeTextBox")).Text = productCode;
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
                    unitPriceTextBox.Text = aDataTable.Rows[0]["UnitPrice"].ToString();
                    TextBox upVatTextBox =
                        (TextBox) gridLineItemGridView.Rows[rowindex].Cells[5].FindControl("upVatTextBox");
                    //upVatTextBox.Text = aDataTable.Rows[0]["VAT"].ToString();

                    upVatTextBox.Text = isVatApplicable.Value == "False" ? 0.ToString(CultureInfo.InvariantCulture) : aDataTable.Rows[0]["VAT"].ToString();
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
       
            //for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            //{
            //    if (rowindex!=i)
            //    {
            //        if (((TextBox) gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text.Trim() ==
            //            productCode.Trim())
            //        {
            //            return false;
            //        }
            //    }
            //}

        return true;
    }
    protected void showMessageBox(string message)
    {
        string sScript;
        sScript = String.Format("alert('{0}');", HttpUtility.JavaScriptStringEncode(message));
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    public void GetQty(decimal qty, int rowindex, decimal discountPerRow, decimal percentagePerRow)
    {
        
        TextBox qtyTextBox1 = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[6].FindControl("qtyTextBox");
        qtyTextBox1.Text = qty.ToString();

        TextBox unitPriceTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[4].FindControl("unitPriceTextBox");
        TextBox upVatTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[5].FindControl("upVatTextBox");

        TextBox tpTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[7].FindControl("tpTextBox");
        tpTextBox.Text = Convert.ToString(Convert.ToDecimal(unitPriceTextBox.Text.Trim()) * qty);

        TextBox tpVatTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[8].FindControl("tpVatTextBox");
        tpVatTextBox.Text = Convert.ToString(Convert.ToDecimal(upVatTextBox.Text.Trim()) * qty);

        TextBox codeTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[1].FindControl("codeTextBox");

        decimal discountPer = 0;
        discountPer = aInvoiceBll.ProductDiscount(codeTextBox.Text.Trim(), hdCustomerMasterId.Value,invDateTextBox.Text);
        TextBox dpTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[9].FindControl("dpTextBox");
        TextBox dpAmtTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[10].FindControl("dpAmtTextBox");
        DataTable fixedDiscount = aInvoiceBll.GetFixedDiscount(hdCustomerMasterId.Value, hdComUnitId.Value);


        if (fixedDiscount.Rows.Count > 0)
        {
            dpAmtTextBox.Text = discountPerRow.ToString();
            dpTextBox.Text = 0.ToString();
        }
        else
        {
            dpTextBox.Text = Convert.ToString(discountPer);
            dpAmtTextBox.Text = ((Convert.ToDecimal(unitPriceTextBox.Text.Trim()) / 100) * discountPer * qty).ToString("F");
        }

       

        TextBox npTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[11].FindControl("npTextBox");
        npTextBox.Text = Convert.ToString((Convert.ToDecimal(tpTextBox.Text.Trim()) - Convert.ToDecimal(dpAmtTextBox.Text.Trim())) + Convert.ToDecimal(tpVatTextBox.Text.Trim()));

        ((TextBox)gridLineItemGridView.Rows[rowindex].Cells[13].FindControl("tQtyTextBox")).Text = qtyTextBox1.Text.Trim();

        


        //if (aTable.Rows.Count > 0)
        //{
        //    if (Convert.ToBoolean(aTable.Rows[0]["IsSame"]))
        //    {
        //        ((TextBox) gridLineItemGridView.Rows[rowindex].Cells[12].FindControl("bQtyTextBox")).Text = aTable.Rows[0]["BonusQty"].ToString();
        //        TotalQtyCalulation(rowindex);
        //    }
        //    else
        //    {
        //        DataTable aDataTable = new DataTable();
        //        aDataTable.Columns.Add("SL");
        //        aDataTable.Columns.Add("ProductCode");
        //        aDataTable.Columns.Add("ProductName");

        //        aDataTable.Columns.Add("StockQty");
        //        aDataTable.Columns.Add("UnitPrice");
        //        aDataTable.Columns.Add("UnitVAT");

        //        aDataTable.Columns.Add("Quantity");

        //        aDataTable.Columns.Add("TotalPrice");
        //        aDataTable.Columns.Add("VAT");

        //        aDataTable.Columns.Add("DiscountPercentage");
        //        aDataTable.Columns.Add("DiscountAmount");

        //        aDataTable.Columns.Add("NetPrice");
        //        aDataTable.Columns.Add("SpecialAmount");

        //        aDataTable.Columns.Add("BonusQty");
        //        aDataTable.Columns.Add("TotalQty");
        //        aDataTable.Columns.Add("OrderDetailsId");
        //        DataRow dataRow;

        //        if (gridLineItemGridView.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
        //            {
        //                dataRow = aDataTable.NewRow();

        //                dataRow["SL"] = Convert.ToString(i + 1);
        //                dataRow["ProductCode"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text.Trim();
        //                dataRow["OrderDetailsId"] = ((HiddenField)gridLineItemGridView.Rows[i].Cells[1].FindControl("orderdetailIdHiddenField")).Value.Trim();
        //                dataRow["SpecialAmount"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("sdTextBox")).Text.Trim();
        //                dataRow["ProductName"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[2].FindControl("nameTextBox")).Text.Trim();
        //                dataRow["StockQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[3].FindControl("currentStockTextBox")).Text.Trim();
        //                dataRow["UnitPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[4].FindControl("unitPriceTextBox")).Text.Trim();
        //                dataRow["UnitVAT"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[5].FindControl("upVatTextBox")).Text.Trim(); ;
        //                dataRow["Quantity"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[6].FindControl("qtyTextBox")).Text.Trim();
        //                dataRow["TotalPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text.Trim();
        //                dataRow["VAT"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[8].FindControl("tpVatTextBox")).Text.Trim();
        //                dataRow["DiscountPercentage"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text.Trim();
        //                dataRow["DiscountAmount"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text.Trim();
        //                dataRow["NetPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[11].FindControl("npTextBox")).Text.Trim();
        //                dataRow["BonusQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[12].FindControl("bQtyTextBox")).Text.Trim();
        //                dataRow["TotalQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[13].FindControl("tQtyTextBox")).Text.Trim();
        //                aDataTable.Rows.Add(dataRow);
        //            }
        //        }

        //        int sl = aDataTable.Rows.Count;
        //        dataRow = aDataTable.NewRow();


        //        DataTable csTable = aInvoiceBll.ProductInfo(hdComUnitId.Value, aTable.Rows[0]["BonusProductCode"].ToString());

        //        dataRow["SL"] = Convert.ToString(sl + 1);
        //        dataRow["ProductCode"] = aTable.Rows[0]["BonusProductCode"];
        //        dataRow["OrderDetailsId"] = 0;
        //        dataRow["ProductName"] = aTable.Rows[0]["ProductName"];
        //        dataRow["StockQty"] = csTable.Rows[0]["StockQty"];
        //        dataRow["UnitPrice"] = 0;
        //        dataRow["UnitVAT"] = 0;
        //        dataRow["Quantity"] = 0;
        //        dataRow["TotalPrice"] = 0;
        //        dataRow["VAT"] = 0;
        //        dataRow["DiscountPercentage"] = 0;
        //        dataRow["DiscountAmount"] = 0;
        //        dataRow["SpecialAmount"] = 0;
        //        dataRow["NetPrice"] = 0;
        //        dataRow["BonusQty"] = aTable.Rows[0]["BonusQty"].ToString();
        //        dataRow["TotalQty"] = aTable.Rows[0]["BonusQty"].ToString();
        //        aDataTable.Rows.Add(dataRow);

        //        gridLineItemGridView.DataSource = null;
        //        gridLineItemGridView.DataBind();
        //        gridLineItemGridView.DataSource = aDataTable;
        //        gridLineItemGridView.DataBind();

        //        ((TextBox)gridLineItemGridView.Rows[rowindex].Cells[12].FindControl("bQtyTextBox")).Text = "0";
        //    }
            
        //}
        //else
        //{
        //    ((TextBox)gridLineItemGridView.Rows[rowindex].Cells[12].FindControl("bQtyTextBox")).Text = "0";
        //}


        
        //((TextBox)gridLineItemGridView.Rows[rowindex].Cells[12].FindControl("bQtyTextBox")).Text = "0";
        TotalValueCalculation();
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
            dpAmtTextBox.Text = Convert.ToString((Convert.ToDecimal(unitPriceTextBox.Text.Trim()) / 100) * discountPer);
        }

        TextBox npTextBox = (TextBox)gridLineItemGridView.Rows[rowindex].Cells[11].FindControl("npTextBox");
        npTextBox.Text = Convert.ToString((Convert.ToDecimal(tpTextBox.Text.Trim()) - Convert.ToDecimal(dpAmtTextBox.Text.Trim())) + Convert.ToDecimal(tpVatTextBox.Text.Trim()));

        ((TextBox)gridLineItemGridView.Rows[rowindex].Cells[13].FindControl("tQtyTextBox")).Text = qtyTextBox1.Text.Trim();

        //Campaign Code

        string ProductCode = ((TextBox)gridLineItemGridView.Rows[rowindex].Cells[1].FindControl("codeTextBox")).Text;
        int Qty = Convert.ToInt32(((TextBox)gridLineItemGridView.Rows[rowindex].Cells[1].FindControl("qtyTextBox")).Text);

        DataTable aTable = aInvoiceBll.GetBonusCampaignInfo(ProductCode, Convert.ToDateTime(invDateTextBox.Text), DateTime.Now.TimeOfDay, Qty);

        if (aTable.Rows.Count > 0)
        {
            ((TextBox)gridLineItemGridView.Rows[rowindex].Cells[12].FindControl("bQtyTextBox")).Text = aTable.Rows[0]["BonusQty"].ToString();
        }
        else
        {
            ((TextBox)gridLineItemGridView.Rows[rowindex].Cells[12].FindControl("bQtyTextBox")).Text = "0";
        }


        TotalQtyCalulation(rowindex);
        TotalValueCalculation();
    }



    private void AddCampaignProduct()
    {
        for (int rowindex = 0; rowindex < gridLineItemGridView.Rows.Count; rowindex++)
        {
            string productCode = ((TextBox)gridLineItemGridView.Rows[rowindex].Cells[1].FindControl("codeTextBox")).Text;
            int qty;

            try
            {
                qty = Convert.ToInt32(((TextBox)gridLineItemGridView.Rows[rowindex].Cells[1].FindControl("qtyTextBox")).Text);
            }
            catch
            {
                continue; // Skip this row if quantity is invalid
            }

            DataTable aTable = aInvoiceBll.GetBonusCampaignInfoByCompany(productCode, Convert.ToDateTime(invDateTextBox.Text), orderIdHiddenField.Value);

            if (aTable.Rows.Count > 0)
            {
                // 🟡 Instead of DB, read orderDetails from grid (no `out`)
                DataTable orderDetails = new DataTable();
                orderDetails.Columns.Add("ProductId", typeof(int));
                orderDetails.Columns.Add("Quantity", typeof(decimal));

                for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
                {
                    TextBox codeTextBox = (TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox");
                    TextBox qtyTextBox = (TextBox)gridLineItemGridView.Rows[i].Cells[6].FindControl("qtyTextBox");

                    if (codeTextBox != null && qtyTextBox != null)
                    {
                        string code = codeTextBox.Text.Trim();
                        string quantity = qtyTextBox.Text.Trim();

                        try
                        {
                            int pid = Convert.ToInt32(code);
                            decimal q = Convert.ToDecimal(quantity);

                            DataRow dr = orderDetails.NewRow();
                            dr["ProductId"] = pid;
                            dr["Quantity"] = q;
                            orderDetails.Rows.Add(dr);
                        }
                        catch
                        {
                            // Ignore invalid rows
                        }
                    }
                }

                int campaignId = Convert.ToInt32(aTable.Rows[0]["CampgainMasterId"]);
                DataTable campaignDetails = aInvoiceBll.GetCampaignDetailsByCampaignId(campaignId);

                bool allProductsPresent = true;
                List<decimal> setCounts = new List<decimal>();

                foreach (DataRow campaignRow in campaignDetails.Rows)
                {
                    int pointProductId = Convert.ToInt32(campaignRow["ProductCode"]);
                    decimal requiredQty = Convert.ToDecimal(campaignRow["Quantity"]);

                    var matchedRows = orderDetails.AsEnumerable()
                        .Where(r => Convert.ToInt32(r["ProductId"]) == pointProductId)
                        .ToList();

                    if (!matchedRows.Any())
                    {
                        allProductsPresent = false;
                        break;
                    }

                    decimal totalOrderedQty = matchedRows.Sum(r => Convert.ToDecimal(r["Quantity"]));
                    decimal setCount = Math.Floor(totalOrderedQty / requiredQty);

                    if (setCount <= 0)
                    {
                        allProductsPresent = false;
                        break;
                    }

                    setCounts.Add(setCount);
                }

                if (allProductsPresent)
                {
                    decimal bonusPerSet = Convert.ToDecimal(aTable.Rows[0]["BonusProductQuantity"]);
                    decimal totalBonusQty = setCounts.Min() * bonusPerSet;

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
                    aDataTable.Columns.Add("SpecialAmount");
                    aDataTable.Columns.Add("BonusQty");
                    aDataTable.Columns.Add("TotalQty");
                    aDataTable.Columns.Add("OrderDetailsId");

                    for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
                    {
                        DataRow dataRow = aDataTable.NewRow();
                        dataRow["SL"] = (i + 1).ToString();
                        dataRow["ProductCode"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text.Trim();
                        dataRow["OrderDetailsId"] = ((HiddenField)gridLineItemGridView.Rows[i].Cells[1].FindControl("orderdetailIdHiddenField")).Value.Trim();
                        dataRow["SpecialAmount"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("sdTextBox")).Text.Trim();
                        dataRow["ProductName"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[2].FindControl("nameTextBox")).Text.Trim();
                        dataRow["StockQty"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[3].FindControl("currentStockTextBox")).Text.Trim();
                        dataRow["UnitPrice"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[4].FindControl("unitPriceTextBox")).Text.Trim();
                        dataRow["UnitVAT"] = ((TextBox)gridLineItemGridView.Rows[i].Cells[5].FindControl("upVatTextBox")).Text.Trim();
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

                    string bonusProductCode = aTable.Rows[0]["BonusProductCode"].ToString();

                    bool bonusExists = aDataTable.AsEnumerable()
                        .Any(r => r["ProductCode"].ToString() == bonusProductCode && Convert.ToDecimal(r["BonusQty"]) > 0);

                    if (!bonusExists)
                    {
                        DataTable csTable = aInvoiceBll.ProductInfo(hdComUnitId.Value, bonusProductCode);

                        if (csTable != null && csTable.Rows.Count > 0)
                        {
                            DataRow bonusRow = aDataTable.NewRow();
                            bonusRow["SL"] = (aDataTable.Rows.Count + 1).ToString();
                            bonusRow["ProductCode"] = bonusProductCode;
                            bonusRow["OrderDetailsId"] = 0;
                            bonusRow["ProductName"] = aTable.Rows[0]["ProductName"];
                            bonusRow["StockQty"] = csTable.Rows[0]["StockQty"];
                            bonusRow["UnitPrice"] = 0;
                            bonusRow["UnitVAT"] = 0;
                            bonusRow["Quantity"] = 0;
                            bonusRow["TotalPrice"] = 0;
                            bonusRow["VAT"] = 0;
                            bonusRow["DiscountPercentage"] = 0;
                            bonusRow["DiscountAmount"] = 0;
                            bonusRow["SpecialAmount"] = 0;
                            bonusRow["NetPrice"] = 0;
                            bonusRow["BonusQty"] = totalBonusQty.ToString();
                            bonusRow["TotalQty"] = totalBonusQty.ToString();

                            aDataTable.Rows.Add(bonusRow);
                        }
                    }

                    gridLineItemGridView.DataSource = null;
                    gridLineItemGridView.DataBind();
                    gridLineItemGridView.DataSource = aDataTable;
                    gridLineItemGridView.DataBind();

                    ((TextBox)gridLineItemGridView.Rows[rowindex].Cells[12].FindControl("bQtyTextBox")).Text = "0";
                }
            }
        }
    }


    private void TotalQtyCalulation(int rowindex)
    {
        ((TextBox)gridLineItemGridView.Rows[rowindex].Cells[13].FindControl("tQtyTextBox")).Text =
            Convert.ToString(
                Convert.ToDecimal(
                    ((TextBox)gridLineItemGridView.Rows[rowindex].Cells[12].FindControl("bQtyTextBox")).Text.Trim()) +
                Convert.ToDecimal(
                    ((TextBox)gridLineItemGridView.Rows[rowindex].Cells[13].FindControl("tQtyTextBox")).Text.Trim()));
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



    protected void saveButton_Click1(object sender, EventArgs e)
    {
        if (!Validation())
        {
            return;
        }

        if (OrderExists(orderIdHiddenField.Value))
        {
            showMessageBox("Invoice already Generated !!");
            return;
        }

        int invId = 0;

        try
        {
            // Master + details + stock + order status must commit together or not at all.
            // ReadCommitted (not the Serializable default) to avoid range locks on tblDCStore.
            TransactionOptions options = new TransactionOptions
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                Timeout = TimeSpan.FromMinutes(5)
            };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                invId = aInvoiceBll.SaveInvoiceMater(PrepareMasterData());

                if (invId <= 0)
                {
                    // sp_I_InvoiceMaster returns 0 when the same OrderNo was invoiced in the last
                    // 60 seconds, when the generated InvoiceNo already exists, or when SaveAction
                    // swallowed a SQL error. Leaving the scope without Complete() rolls everything back.
                    showMessageBox("Invoice could not be created. This order may have just been invoiced - please wait a minute, reload the order and try again. If it keeps failing, contact IT.");
                    return;
                }

                if (!SaveInvoiceDetail(invId))
                {
                    showMessageBox("Invoice items could not be saved. Nothing was saved, please generate the invoice again.");
                    return;
                }

                OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();
                aOrderInfoBll.UpdateInvoiceStatus(orderIdHiddenField.Value);

                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            // Nothing to undo - the TransactionScope rolled back on the way out.
            showMessageBox("Invoice generate failed: " + ex.Message);
            return;
        }

        // Committed. Everything below is display only.
        Clear();

        DataTable aTable = aInvoiceBll.GetInvoiceNoById(invId);

        if (aTable.Rows.Count > 0)
        {
            invTextBox.Text = aTable.Rows[0].Field<string>("InvoiceNo");
            rptInvoiceId.Text = invId.ToString();
        }

        showMessageBox("Proforma Invoice Created Successsfully!!");
    }

    

    private Invoice PrepareMasterData()
    {
        string[] forComUCode = comUnitNameTextBox.Text.Split(':');
        string comUnitCode = forComUCode[0];

        Invoice aInvoice = new Invoice();

        aInvoice.InvoiceDate = Convert.ToDateTime(invDateTextBox.Text.Trim());
        aInvoice.OrderNo = orderNoTextBox.Text.Trim();
        aInvoice.OrderDate = Convert.ToDateTime(orderDateTextBox.Text.Trim());
        aInvoice.CustomerMasterId = Convert.ToInt32(hdCustomerMasterId.Value);
        aInvoice.ComUnitId = Convert.ToInt32(hdComUnitId.Value);
        aInvoice.MiaId = Convert.ToInt32(hdMiaId.Value);
        aInvoice.PaymentTypeId = Convert.ToInt32(payTypeDDL.SelectedValue);
        aInvoice.TpTotal = Convert.ToDecimal(tpTptalTextBox.Text.Trim());
        aInvoice.TpDiscount = Convert.ToDecimal(disTotalTextBox.Text.Trim());
        aInvoice.TpVat = Convert.ToDecimal(vatTotalTextBox.Text.Trim());
        aInvoice.TpGrandTotal = Convert.ToDecimal(grandTotalTextBox.Text.Trim());
        aInvoice.UserId = Convert.ToInt32(Session["UserId"].ToString());
        aInvoice.ComUnitCode = comUnitCode;
        aInvoice.OrderId = Convert.ToInt32(orderIdHiddenField.Value);
        aInvoice.TotalSpecialAmount = Convert.ToDecimal(pdTextBox.Text);
        aInvoice.OldTradePolicy = false;
        aInvoice.Remarks = remarksTextBox.Text;
        aInvoice.FixedCustomer = false;
        aInvoice.DeliveryManId = Convert.ToInt32(deliverypersonNameTextBox.SelectedValue);
        aInvoice.DpNAme = deliverypersonNameTextBox.Text.Trim();
        aInvoice.DpMob = deliverypersonMobileTextBox.Text.Trim();
        aInvoice.ProductOffer = "False";
        aInvoice.PONumber = txtPONumber.Text;

        return aInvoice;
    }

    protected void saveButton_Click(object sender, EventArgs e)
    {
        if (Validation())
        {
            if (OrderExists(orderIdHiddenField.Value) == false)
            {
                {
                    try
                    {
                        int invId = SaveInvoice();
                        if (invId > 0)
                        {
                            bool dtl = SaveInvoiceDetail(invId);
                            if (dtl)
                            {

                                // Journal Posting

                                if (aInvoiceBll.PostJournal(invId))
                                {
                                    OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();
                                    aOrderInfoBll.UpdateInvoiceStatus(orderIdHiddenField.Value);
                                    Clear();

                                    invoiceNoTextBox.Text = invTextBox.Text;
                                    ModalPopupExtender10.Show();
                                    
                                    
                                }
                                
                            }
                            else
                            {
                               // aInvoiceBll.DeleteInvoice(orderIdHiddenField.Value);
                                Clear();
                                showMessageBox("Internet  error code 1231!! Please generate Invoice again!");
                               
                            }
                        }
                    }
                    catch (Exception)
                    {
                      //  aInvoiceBll.DeleteInvoice(orderIdHiddenField.Value);
                        Clear();
                        showMessageBox("Internet error code 1231 !! Please generate Invoice again! ");
                    }
                }
            }
            else
            {
                showMessageBox("Invoice already Generated !!");
            }
        }
    }

    private bool SaveInvoiceDetail(int invoiceId)
    {
        
        List<InvoiceDetail> aInvoiceDetailsList = new List<InvoiceDetail>();

        if (gridLineItemGridView.Rows.Count>0)
        {
            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {
                InvoiceDetail aInvoiceDetail = new InvoiceDetail();
                aInvoiceDetail.ProductCode =
                    ((TextBox) gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text;
                string product = ((TextBox)gridLineItemGridView.Rows[i].Cells[2].FindControl("nameTextBox")).Text;
                string[] proNameAndPackSize = product.Split(':');
                aInvoiceDetail.ProductName = proNameAndPackSize[0];
                aInvoiceDetail.PackSize = proNameAndPackSize[1];
                aInvoiceDetail.UnitPrice =Convert.ToDecimal(((TextBox) gridLineItemGridView.Rows[i].Cells[4].FindControl("unitPriceTextBox")).Text);

                aInvoiceDetail.UnitVatAmount = isVatApplicable.Value == "False" ? Convert.ToDecimal(0) : Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[5].FindControl("upVatTextBox")).Text);

                //aInvoiceDetail.UnitVatAmount =Convert.ToDecimal(((TextBox) gridLineItemGridView.Rows[i].Cells[5].FindControl("upVatTextBox")).Text);
                aInvoiceDetail.Quantity = Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[6].FindControl("qtyTextBox")).Text);
                aInvoiceDetail.DiscountPercentage = Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[9].FindControl("dpTextBox")).Text);
                aInvoiceDetail.DiscountAmount = Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text);
                aInvoiceDetail.BonusQuantity = string.IsNullOrEmpty(((TextBox)gridLineItemGridView.Rows[i].Cells[12].FindControl("bQtyTextBox")).Text.Trim())
    ? 0
    : Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[12].FindControl("bQtyTextBox")).Text.Trim());

                aInvoiceDetail.SpecialAmount = Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("sdTextBox")).Text.Trim());
                string id = ((HiddenField) gridLineItemGridView.Rows[i].Cells[1].FindControl("orderdetailIdHiddenField")).Value;
                aInvoiceDetail.OrderDetailsId = Convert.ToInt32(id);
                aInvoiceDetail.InvoiceId = invoiceId;

                aInvoiceDetail.TotalPriceVatAmount = isVatApplicable.Value == "False" ? Convert.ToDecimal(0) : Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[8].FindControl("tpVatTextBox")).Text);

                //aInvoiceDetail.TotalPriceVatAmount = Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[8].FindControl("tpVatTextBox")).Text);
                TextBox npTextBox = (TextBox)gridLineItemGridView.Rows[i].Cells[11].FindControl("npTextBox");
                aInvoiceDetail.NetAmount = Convert.ToDecimal(npTextBox.Text);
                //if (dtdiscount.Rows.Count>0)
                //{
                //    aInvoiceDetail.SpecialAmountPer = Convert.ToDecimal(dtdiscount.Rows[0]["DiscountPercentage"].ToString());    
                //}
                //else
                //{
                //    aInvoiceDetail.SpecialAmountPer = 0;
                //}
                //if (aInvoiceDetail.SpecialAmountPer==0)
                //{
                //    aInvoiceDetail.SpecialAmountPer = (aInvoiceDetail.SpecialAmount/aInvoiceDetail.NetAmount);
                //}
                
                decimal cstock = 0;
                decimal tqty = 0;
                tqty = Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[14].FindControl("tQtyTextBox")).Text);
                cstock = Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[3].FindControl("currentStockTextBox")).Text);
                /////SMC Low Order Method /////
                //if (tqty>cstock)
                //{
                //    aInvoiceBll.UpdateOrder("Undelivered", aInvoiceDetail.OrderDetailsId.ToString());
                //}
                //else
                //{
                //    aInvoiceDetailsList.Add(aInvoiceDetail);    
                //}
                ///////////////////
                if (cstock==0)
                {
                    aInvoiceBll.UpdateOrder("Undelivered", aInvoiceDetail.OrderDetailsId.ToString());
                }
                else
                {
                    aInvoiceDetailsList.Add(aInvoiceDetail);
                }
            }

            // Every row can end up skipped as zero-stock/Undelivered above - an empty list must not
            // be reported as a successful save (that's a master with no details).
            if (aInvoiceDetailsList.Count == 0)
            {
                return false;
            }

            return aInvoiceBll.SaveInvoiceDetails(aInvoiceDetailsList, hdComUnitId.Value);
        }

        return false;
    }

    private int SaveInvoice()
    {

        int invoiceId=0;
        DataTable aDataTable = new DataTable();
        aDataTable = aInvoiceBll.CustomerMaster(custCodeTextBox.Text);
        string invoiceNo = string.Empty;
        string[] forComUCode = comUnitNameTextBox.Text.Split(':');
        string ComUnitCode = forComUCode[0];
        Invoice aInvoice = new Invoice();
                              // {
                                   aInvoice.InvoiceDate = Convert.ToDateTime(invDateTextBox.Text.Trim());
                                   aInvoice.OrderNo = orderNoTextBox.Text.Trim();
                                   aInvoice.OrderDate = Convert.ToDateTime(orderDateTextBox.Text.Trim());
                                   aInvoice.CustomerMasterId = Convert.ToInt32(hdCustomerMasterId.Value);
                                   aInvoice.ComUnitId=Convert.ToInt32(hdComUnitId.Value);
                                   aInvoice.MiaId = Convert.ToInt32(hdMiaId.Value);
                                   aInvoice.PaymentTypeId=Convert.ToInt32(payTypeDDL.SelectedValue);
                                   aInvoice.TpTotal = Convert.ToDecimal(tpTptalTextBox.Text.Trim());
                                   aInvoice.TpDiscount = Convert.ToDecimal(disTotalTextBox.Text.Trim());
                                   aInvoice.TpVat=Convert.ToDecimal(vatTotalTextBox.Text.Trim());
                                   aInvoice.TpGrandTotal=Convert.ToDecimal(grandTotalTextBox.Text.Trim());
                                   aInvoice.UserId = Convert.ToInt32(Session["UserId"].ToString());
                                   aInvoice.ComUnitCode = ComUnitCode;
                                   aInvoice.OrderId = Convert.ToInt32(orderIdHiddenField.Value);
                                   aInvoice.TotalSpecialAmount = Convert.ToDecimal(pdTextBox.Text);
                                   aInvoice.OldTradePolicy = false;
                                    ////// SMC Low Stock Method////////
                                   aInvoice.Remarks = remarksTextBox.Text;
                                   //aInvoice.MIACode = aDataTable.Rows[0]["MIACode"].ToString();
                                   //aInvoice.MIAName = aDataTable.Rows[0]["MIAName"].ToString();
                                   //aInvoice.MarketCode = aDataTable.Rows[0]["MarketCode"].ToString();
                                   //aInvoice.MarketName = aDataTable.Rows[0]["MarketName"].ToString();
                                   //aInvoice.AreaCode = aDataTable.Rows[0]["AreaCode"].ToString();
                                   //aInvoice.DisCode = aDataTable.Rows[0]["DistrictCode"].ToString();
                                   //aInvoice.FEName = aDataTable.Rows[0]["FEName"].ToString();
                                   //aInvoice.RegionCode = aDataTable.Rows[0]["RegionCode"].ToString();
                                   //aInvoice.DZSMName = aDataTable.Rows[0]["DZSMName"].ToString();
                                   aInvoice.FixedCustomer = false;

                                   aInvoice.DeliveryManId = Convert.ToInt32(deliverypersonNameTextBox.SelectedValue);
                                   aInvoice.DpNAme = deliverypersonNameTextBox.Text.Trim();
                                   aInvoice.DpMob = deliverypersonMobileTextBox.Text.Trim();
                                   ////////////////Product Multiple Offer Start ///////////
                                   //if (ChkProductOffer() == true && ChkProductOfferEsomium20() == true)
                                   //{
                                   //    aInvoice.ProductOffer = "FOCEsomium20";
                                   //}
                                  // else
                                   //if (ChkProductOffer() == true && ChkProductOfferEsomium20() == false)
                                   //{
                                   //    aInvoice.ProductOffer = "MFOC";
                                   //}
                                   //if (ChkProductOffer2FlexidolParagesic() == true && ChkProductOfferFlexidolParagesic() == true)
                                   //{
                                   //    aInvoice.ProductOffer = "FlexidolParagesic";
                                   //}
                                   //if (ChkProductOffer() == true)
                                   //{
                                   //    aInvoice.ProductOffer = "FlexidolFOC";
                                   //}
                                    //else if (ChkProductOffer2FlexidolParagesic() == true && ChkProductOfferFlexidolParagesic() == true && ChkProductOffer() == true)
                                    //{
                                    //    aInvoice.ProductOffer = "EzeventFlexidolParagesicFoc";
                                    //}
                                   //else
                                 //  {
                                       aInvoice.ProductOffer = "False";
                                 //  }
                                   ////////////////Product Multiple Offer End ///////////

       aInvoiceBll.SaveInvoice(aInvoice,out invoiceId,out invoiceNo);
      
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
        districtNameTextBox.Text = "";
        areaNameTextBox.Text = "";
        comUnitNameTextBox.Text = "";
        miaCodeTextBox.Text = "";
        marketNameTextBox.Text = "";
        miaNameTextBox.Text = "";
        custCategoryTextBox.Text = "";
        hdMiaId.Value = "";
        custCodeTextBox.Text = "";
        orderNoTextBox.Text = "";
        pdTextBox.Text = "";
        orderHiddenField.Value = "";

        remarksTextBox.Text = "";
        payTypeDDL.SelectedValue = "";
        deliverypersonNameTextBox.SelectedValue = "";
        deliverypersonMobileTextBox.Text = "";

        InitialGrid();
    }
    
    private void TotalValueCalculation()
    {
        decimal tpTotal = 0;
        decimal vatTotal = 0;
        decimal disTotal = 0;
        decimal gTotal = 0;
        decimal sptotatl = 0;
        if (gridLineItemGridView.Rows.Count > 0)
        {
           
            for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
            {
                decimal tqty = !string.IsNullOrEmpty(((TextBox)gridLineItemGridView.Rows[i].Cells[14].FindControl("tQtyTextBox")).Text)
     ? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[14].FindControl("tQtyTextBox")).Text.Trim())
     : 0m;


                decimal cstock = !string.IsNullOrEmpty(((TextBox)gridLineItemGridView.Rows[i].Cells[3].FindControl("currentStockTextBox")).Text)
    ? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[3].FindControl("currentStockTextBox")).Text.Trim())
    : 0m;

                DataTable dtdata = aInvoiceBll.LoadProductQty(orderIdHiddenField.Value,
                ((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text);
                /////SMC Low Stock Method/////
                //tqty = Convert.ToDecimal(dtdata.Rows[0][0].ToString());
                /////////////
                if (cstock >= tqty )
                {
                    tpTotal += (((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text != "") ? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[7].FindControl("tpTextBox")).Text) : 0;
                    vatTotal += (((TextBox)gridLineItemGridView.Rows[i].Cells[8].FindControl("tpVatTextBox")).Text != "") ? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[8].FindControl("tpVatTextBox")).Text) : 0;
                   //Offer Applied
                    if (((TextBox)gridLineItemGridView.Rows[i].Cells[1].FindControl("codeTextBox")).Text == "FGD02" && tqty >= 3)
                    {
                        disTotal = 0;
                        //(((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text != "") ? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text) : 0;

                    }
                    else
                    {
                        disTotal += (((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text != "") ? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("dpAmtTextBox")).Text) : 0;
                        
                    }
                    
                    gTotal += (((TextBox)gridLineItemGridView.Rows[i].Cells[11].FindControl("npTextBox")).Text != "") ? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[11].FindControl("npTextBox")).Text) : 0;
                    sptotatl += (((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("sdTextBox")).Text != "") ? Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i].Cells[10].FindControl("sdTextBox")).Text) : 0;
                }
            }
        }
       
        tpTptalTextBox.Text = tpTotal.ToString();
        vatTotalTextBox.Text = vatTotal.ToString();
        disTotalTextBox.Text = disTotal.ToString();
        grandTotalTextBox.Text = gTotal.ToString();
        pdTextBox.Text = sptotatl.ToString();


        
    }

    protected void printButton_Click(object sender, EventArgs e)
    {

        Session["customer"] = "";
        Session["customer"] = hdfCustomer.Value;

        //string url = "../SInventory_RPTVIEW/InvoiceReportViewer.aspx?InvNo=" + Server.UrlEncode(invTextBox.Text.Trim());

        // Create the URL for the report viewer page with encoded parameters
        string url = "../SInventory_RPTVIEW/InvoiceReportViewer.aspx?InvNo=" + Server.UrlEncode(invTextBox.Text.Trim()) + "&invoiceId=" + Server.UrlEncode(rptInvoiceId.Text.Trim());

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
                custCodeTextBox.Text = custInfo[0];
                string custCode = custCodeTextBox.Text.Trim();
                GetCustInfo(custCode);
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
    protected void deliverypersonNameTextBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        OrderInfoBLL aOrderInfoBll=new OrderInfoBLL();
        DataTable dt = aOrderInfoBll.LoadDeliveryMan(deliverypersonNameTextBox.SelectedValue);
        if (dt.Rows.Count>0)
        {
            deliverypersonMobileTextBox.Text = dt.Rows[0]["CellNumber"].ToString();
        }
    }

    protected void pdTextBox_OnTextChanged(object sender, EventArgs e)
    {
        CalculateGrandTotal();
    }

    protected void yesButton_Click(object sender, EventArgs e)
    {
        ModalPopupExtender10.Hide();
        showMessageBox("Proforma Invoice Created Successsfully!!");
    }

    protected void noButton_Click(object sender, EventArgs e)
    {
        ProformaOrInvoiceReturnBLL aInvoiceReturnBll = new ProformaOrInvoiceReturnBLL();
        // Journal Posting

        if (aInvoiceReturnBll.ProformaReturnPosting(invTextBox.Text.Trim()))
        {
            if (aInvoiceReturnBll.DeleteProforma(invTextBox.Text.Trim(), Session["LoginName"].ToString()) > 0)
            {
                ModalPopupExtender10.Hide();
                Clear();
                invTextBox.Text = "";
                showMessageBox("Invoice Canceled Successfully !!");
               
            }
        }
    }
}