using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.PettyCash_BLL;
using Library.DAL.PettyCash_DaL;
using Library.DAO.PettyCashDao;

public partial class SInventory_UI_PrettyCashEntry : System.Web.UI.Page
{

    PettyCashBLL aCashBll = new PettyCashBLL();
    PettyCashDaL aCashDaL = new PettyCashDaL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoaddropdownList();
            LoadInitialGrid();


            if (!string.IsNullOrEmpty(Request.QueryString["MasID"]))
            {
                GetOneRecord(Convert.ToInt32(Request.QueryString["MasID"]));

            }
        }

    }

    protected void ListImageButton_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(),
            "alert",
            "window.location ='PrettyCashView.aspx';",
            true);


    }

    private void Clear()
    {

        LoadInitialGrid();
        TextRemark.Text = String.Empty;
        MasterIdHiddenField.Value = String.Empty;
        txtDate.Text = String.Empty;
        custIdHiddenField.Value = string.Empty;
        CompanyDropDownList.Items.Clear();
        CashAccDropDownlist.Items.Clear();


    }


    private void GetOneRecord(Int32 id)
    {

        using (DataTable dt = aCashBll.GetDataMasterByIdBll(id))
        {
            btnSave.Text = "Update";
            btnSave.BackColor = Color.DodgerBlue;

            Int32 rowIndex = 0;
            MasterIdHiddenField.Value = dt.Rows[rowIndex].Field<Int32>("PettyCashmasterId").ToString();



            CompanyDropDownList.SelectedValue = dt.Rows[0]["ConpanyId"].ToString();



            try
            {
                txtDate.Text = Convert.ToDateTime(dt.Rows[0]["Postingdate"].ToString()).ToString("dd-MMM-yyyy");
            }
            catch (Exception)
            {

            }




            aCashBll.LoadCashAcc(CashAccDropDownlist, CompanyDropDownList.SelectedValue);
            CashAccDropDownlist.SelectedValue = dt.Rows[0]["CashAccId"].ToString();


            TextRemark.Text = dt.Rows[0]["Remark"].ToString();
        }







        using (DataTable dtDetail = aCashBll.GetDetailsInfoByIdBll(id))
        {
            itemGridView.DataSource = dtDetail;
            itemGridView.DataBind();

            for (int j = 0; j < itemGridView.Rows.Count; j++)
            {
                Int32 rowIndex = 0;
                TextBox txtPerticular = ((TextBox)itemGridView.Rows[j].Cells[1].FindControl("txtPerticular"));
                HiddenField perticularhiddenId = (HiddenField)itemGridView.Rows[j].Cells[0].FindControl("perticularhiddenId");
                TextBox txtCostReason = ((TextBox)itemGridView.Rows[j].Cells[2].FindControl("txtCostReason"));
                TextBox txtAmount = ((TextBox)itemGridView.Rows[j].Cells[3].FindControl("txtAmount"));

            }


        }





    }







    protected void dateOfBirthTextBox_OnTextChanged(object sender, EventArgs e)
    {
        if (txtDate.Text != "")
        {
            string dateString = txtDate.Text.Trim();

            DateTime dateValue;

            if (!DateTime.TryParse(dateString, out dateValue))
            {
                txtDate.Text = "";
                showMessageBox("Please insert a valid date!!!");
            }
        }
    }
    private void LoaddropdownList()
    {

        aCashBll.LoadCompany(CompanyDropDownList);

    }


    private void LoadInitialGrid()
    {
        DataTable aTable = new DataTable();

        aTable.Columns.Add("ParticularName");
        aTable.Columns.Add("PerticulerId");
        aTable.Columns.Add("CashReason");
        aTable.Columns.Add("Amount");






        DataRow dr;

        dr = aTable.NewRow();
        dr["ParticularName"] = "";
        dr["PerticulerId"] = "";
        dr["CashReason"] = "";
        dr["Amount"] = "";




        aTable.Rows.Add(dr);

        itemGridView.DataSource = null;
        itemGridView.DataBind();
        itemGridView.DataSource = aTable;
        itemGridView.DataBind();



    }





    protected void companyDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (CompanyDropDownList.SelectedValue != "")
        {
            aCashBll.LoadCashAcc(CashAccDropDownlist, CompanyDropDownList.SelectedValue);
        }

    }

    protected void ddlCashACC_OnSelectedIndexChanged(object sender, EventArgs e)
    {

    }


    protected void itemaddImageButton_Click(object sender, EventArgs e)
    {

        int rowIndex = ((GridViewRow)(((ImageButton)sender).Parent.Parent)).RowIndex;
        DataTable aTable = new DataTable();

        aTable.Columns.Add("ParticularName");
        aTable.Columns.Add("PerticulerId");
        aTable.Columns.Add("CashReason");
        aTable.Columns.Add("Amount");


        DataRow dr;



        for (int i = 0; i < itemGridView.Rows.Count; i++)
        {
            dr = aTable.NewRow();
            dr["ParticularName"] = ((TextBox)itemGridView.Rows[i].Cells[1].FindControl("txtPerticular")).Text.Trim();
            dr["CashReason"] = ((TextBox)itemGridView.Rows[i].Cells[2].FindControl("txtCostReason")).Text.Trim();
            dr["Amount"] = ((TextBox)itemGridView.Rows[i].Cells[3].FindControl("txtAmount")).Text.Trim();
            
            HiddenField perticularhiddenId = (HiddenField)itemGridView.Rows[i].Cells[0].FindControl("perticularhiddenId");
            dr["PerticulerId"] = perticularhiddenId.Value.Trim();

            aTable.Rows.Add(dr);



        }
        dr = aTable.NewRow();

        dr["ParticularName"] = "";
        dr["PerticulerId"] = "";
        dr["CashReason"] = "";
        dr["Amount"] = "";





        aTable.Rows.Add(dr);

        itemGridView.DataSource = null;
        itemGridView.DataBind();
        itemGridView.DataSource = aTable;
        itemGridView.DataBind();


    }



    protected void itemdeleteImageButton_Click(object sender, EventArgs e)
    {
        int rowIndex = ((GridViewRow)(((ImageButton)sender).Parent.Parent)).RowIndex;

        DataTable aTable = new DataTable();
        aTable.Columns.Add("PerticulerId");
        aTable.Columns.Add("CashReason");
        aTable.Columns.Add("Amount");


        DataRow dr;


        for (int i = 0; i < itemGridView.Rows.Count; i++)
        {



            if (i != rowIndex)
            {
                dr = aTable.NewRow();

                dr["PerticulerId"] = ((TextBox)itemGridView.Rows[i].Cells[1].FindControl("txtPerticular")).Text.Trim();
                dr["CashReason"] = ((TextBox)itemGridView.Rows[i].Cells[2].FindControl("txtCostReason")).Text.Trim();
                dr["Amount"] = ((TextBox)itemGridView.Rows[i].Cells[3].FindControl("txtAmount")).Text.Trim();

                aTable.Rows.Add(dr);
            }
        }

        itemGridView.DataSource = aTable;
        itemGridView.DataBind();
        AllGridCalc();



    }


    public void AllGridCalc()
    {

        for (int i = 0; i < itemGridView.Rows.Count; i++)
        {
            TlayerCalculation_BytxtLayer(i);
        }
    }
    private void TlayerCalculation_BytxtLayer(int rowIndex)
    {
        decimal layer = 0;



        if (rowIndex == 0)
        {
            if (IsInteger(((TextBox)itemGridView.Rows[rowIndex].Cells[1].FindControl("txtPerticular")).Text))
            {
                ((TextBox)itemGridView.Rows[rowIndex].Cells[2].FindControl("txtCostReason")).Text =
                    ((TextBox)itemGridView.Rows[rowIndex].Cells[3].FindControl("txtAmount")).Text;
            }
        }
        else
        {
            try
            {
                ((TextBox)itemGridView.Rows[rowIndex].Cells[3].FindControl("txtPerticular")).Text = (Convert.ToDecimal(((TextBox)itemGridView.Rows[rowIndex - 1].Cells[1].FindControl("txtCostReason")).Text.Trim()) + Convert.ToDecimal(((TextBox)itemGridView.Rows[rowIndex].Cells[1].FindControl("txtAmount")).Text.Trim())).ToString();
            }
            catch (Exception)
            {


            }
            //for (int i = 1; i < itemGridView.Rows.Count; i++)
            //{
            //    Decimal MultLayer = 0;
            //    if (((TextBox)itemGridView.Rows[i].Cells[1].FindControl("txtLayer")).Text.Trim() != "")
            //    {


            //        MultLayer =
            //            Convert.ToDecimal(((TextBox)itemGridView.Rows[i].Cells[1].FindControl("txtLayer")).Text.Trim());
            //    }
            //    layer += MultLayer;
            //}

            //try
            //{
            //    ((TextBox)itemGridView.Rows[rowIndex].Cells[3].FindControl("txtTLayer")).Text = layer.ToString();
            //}
            //catch (Exception)
            //{

            // //   ((TextBox) itemGridView.Rows[rowIndex].Cells[3].FindControl("txtTLayer")).Text = "0";

            //    //throw;
            //}
        }



    }

    Regex regex = new Regex(@"[1-9]");

    private bool IsInteger(string str)
    {
        try
        {
            if (String.IsNullOrWhiteSpace(str))
            {
                return false;
            }
            if (!regex.IsMatch(str))
            {
                return false;
            }

            return true;

        }
        catch (Exception ex)
        {
            showMessageBox(ex.Message);
        }

        return false;

    }



    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }


    private PettyCashMasterDao PrepareDataForSaveMaster()
    {


        PettyCashMasterDao aCashMasterDao = new PettyCashMasterDao();
        {
            aCashMasterDao.ConpanyId = Convert.ToInt32(CompanyDropDownList.SelectedValue);
            aCashMasterDao.PostingDate = Convert.ToDateTime(txtDate.Text);
            aCashMasterDao.CashAccId = Convert.ToInt32(CashAccDropDownlist.SelectedValue);
            aCashMasterDao.Remark = Convert.ToString(TextRemark.Text);
            aCashMasterDao.EntryBy = Session["LoginName"].ToString();
            aCashMasterDao.EntryDate = DateTime.Now;
            aCashMasterDao.ActiveStatus = "Posted";

        }
        ;



        return aCashMasterDao;

    }

    private PettyCashMasterDao PrepareMasterForUpdate()
    {


        PettyCashMasterDao aCashMasterDao = new PettyCashMasterDao();
        {
            aCashMasterDao.PettyCashmasterId = Convert.ToInt32(MasterIdHiddenField.Value);
            aCashMasterDao.ConpanyId = Convert.ToInt32(CompanyDropDownList.SelectedValue);
            aCashMasterDao.PostingDate = Convert.ToDateTime(txtDate.Text);
            aCashMasterDao.CashAccId = Convert.ToInt32(CashAccDropDownlist.SelectedValue);
            aCashMasterDao.Remark = Convert.ToString(TextRemark.Text);
            aCashMasterDao.UpdateBy = Session["LoginName"].ToString();
            aCashMasterDao.UpdateDate = DateTime.Now;

        }
        ;



        return aCashMasterDao;

    }



    private List<PettyCashDetailsDao> PrepareDataForSaveDetails(Int32 Val)
    {
        PettyCashDetailsDao apetPettyCashDetailsDao;
        List<PettyCashDetailsDao> aPettyCashDetailsDaolist = new List<PettyCashDetailsDao>();



        for (int i = 0; i < itemGridView.Rows.Count; i++)
        {

            HiddenField perticularhiddenId = (HiddenField)itemGridView.Rows[i].Cells[0].FindControl("perticularhiddenId");
            TextBox txtCostReason = ((TextBox)itemGridView.Rows[i].Cells[2].FindControl("txtCostReason"));
            TextBox txtAmount = ((TextBox)itemGridView.Rows[i].Cells[3].FindControl("txtAmount"));


            apetPettyCashDetailsDao = new PettyCashDetailsDao();

            apetPettyCashDetailsDao.PettyCashmasterId = Val;

            apetPettyCashDetailsDao.PerticulerId = Convert.ToInt32(perticularhiddenId.Value);

            apetPettyCashDetailsDao.CashReason = txtCostReason.Text.Trim();
            apetPettyCashDetailsDao.Amount = Convert.ToDecimal(txtAmount.Text.Trim());

            aPettyCashDetailsDaolist.Add(apetPettyCashDetailsDao);

        }
        return aPettyCashDetailsDaolist;

    }



    private List<PettyCashDetailsDao> PrepareDataUpdateDetails()
    {
        PettyCashDetailsDao apetPettyCashDetailsDao;
        List<PettyCashDetailsDao> aPettyCashDetailsDaolist = new List<PettyCashDetailsDao>();



        for (int i = 0; i < itemGridView.Rows.Count; i++)
        {
            HiddenField perticularhiddenId = (HiddenField)itemGridView.Rows[i].Cells[0].FindControl("perticularhiddenId");

            // TextBox txtPerticular = ((TextBox)itemGridView.Rows[i].Cells[2].FindControl("txtPerticular"));
            TextBox txtCostReason = ((TextBox)itemGridView.Rows[i].Cells[2].FindControl("txtCostReason"));
            TextBox txtAmount = ((TextBox)itemGridView.Rows[i].Cells[3].FindControl("txtAmount"));

            apetPettyCashDetailsDao = new PettyCashDetailsDao();
            apetPettyCashDetailsDao.PettyCashmasterId = Convert.ToInt32(MasterIdHiddenField.Value);
            apetPettyCashDetailsDao.PerticulerId = Convert.ToInt32(perticularhiddenId.Value);

            apetPettyCashDetailsDao.CashReason = Convert.ToString(txtCostReason.Text.Trim());
            apetPettyCashDetailsDao.Amount = Convert.ToDecimal(txtAmount.Text.Trim());

            aPettyCashDetailsDaolist.Add(apetPettyCashDetailsDao);

        }
        return aPettyCashDetailsDaolist;

    }






    private Int32 SaveChangesforPettycashDetails(Int32 Val)
    {
        Int32 retVal;
        try
        {
            retVal = aCashBll.SaveDataForDetails(PrepareDataForSaveDetails(Val));
        }
        catch (Exception ex)
        {
            retVal = 0;
            throw ex;
        }

        return retVal;
    }




    private Int32 SaveChangesMaster()
    {
        Int32 retVal;
        try
        {
            retVal = aCashBll.SaveDataBll(PrepareDataForSaveMaster());

        }
        catch (Exception ex)
        {
            retVal = 0;
            throw ex;
        }

        return retVal;
    }





    private bool UpdateChangesMaster()
    {
        bool retVal;
        try
        {
            retVal = aCashBll.UpdateDataBll(PrepareMasterForUpdate());

        }
        catch (Exception ex)
        {
            retVal = false;
            throw ex;
        }

        return retVal;
    }





    private bool UpdateChangesforPettycashDetails()
    {
        bool retVal;
        try
        {
            retVal = aCashBll.UpdateDetailsDataBll(PrepareDataUpdateDetails());
        }
        catch (Exception ex)
        {
            retVal = false;
            throw ex;
        }

        return retVal;
    }



    protected void submitButton_Click1(object sender, EventArgs e)
    {

        if (MasterIdHiddenField.Value == "")
        {
            if (Validation())
            {
                int MasID = SaveChangesMaster();
                if (MasID > 0)
                {

                    int Valdetail = SaveChangesforPettycashDetails(MasID);

                    ScriptManager.RegisterStartupScript(this, this.GetType(),
                        "alert",
                        "alert('Opearation Successfully Done... ');window.location ='PrettyCashView.aspx';",
                        true);

                    Clear();
                }
            }
            else
            {
                showMessageBox("Input Validation Failed...!! Please check again");
            }

        }



        if (MasterIdHiddenField.Value != "")
        {
            if (Validation())
            {
                if (UpdateChangesMaster())
                {

                    bool Valdetail = UpdateChangesforPettycashDetails();


                    ScriptManager.RegisterStartupScript(this, this.GetType(),
                        "alert",
                        "alert('Opearation Successfully Done...');window.location ='PrettyCashView.aspx';",
                        true);
                }
            }
            else
            {
                showMessageBox("Input Validation Failed...!! Please check again");
            }

        }



    }





    protected void PerticularTextBox_TextChanged(object sender, EventArgs e)
    {
        TextBox TextBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)TextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        TextBox txtPerticular = (TextBox)itemGridView.Rows[rowindex].Cells[2].FindControl("txtPerticular");


        string Perticular = txtPerticular.Text.Trim();


        if (Perticular.Contains('~'))
        {
            string[] emp = Perticular.Split('~');

            txtPerticular.Text = emp[0];
            var perticularhiddenId = (HiddenField)itemGridView.Rows[rowindex].Cells[0].FindControl("perticularhiddenId");

            perticularhiddenId.Value = emp[1];

        }
        else
        {

            txtPerticular.Text = "";
            custIdHiddenField.Value = "";
            showMessageBox("Input Correct Data !");
        }


    }




    private bool Validation()
    {

        if (CompanyDropDownList.SelectedValue == "")
        {
            showMessageBox("Please Select Company !!!");
            CompanyDropDownList.Focus();

            return false;
        }

        if (CashAccDropDownlist.SelectedValue == "")
        {
            showMessageBox("Please Select Cash Account !!!");
            CashAccDropDownlist.Focus();

            return false;
        }

        if (txtDate.Text == "")
        {
            showMessageBox("Please Select Date !!!");
            txtDate.Focus();

            return false;
        }



       

        if (itemGridView.Rows.Count > 0)
        {
            for (int i = 0; i < itemGridView.Rows.Count; i++)
            {
                if (((TextBox)itemGridView.Rows[i].FindControl("txtPerticular")).Text == "")
                {
                    ((TextBox)itemGridView.Rows[i].FindControl("txtPerticular")).Focus();
                    showMessageBox("Please fill out PerticularId!!");
                    return false;
                }
            }
        }

        if (itemGridView.Rows.Count > 0)
        {
            for (int i = 0; i < itemGridView.Rows.Count; i++)
            {
                if (((TextBox)itemGridView.Rows[i].FindControl("txtAmount")).Text == "")
                {
                    ((TextBox)itemGridView.Rows[i].FindControl("txtAmount")).Focus();
                    showMessageBox("Please fill out Amount!!");
                    return false;
                }
            }
        }






        return true;
    }


    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(),
            "alert",
            "alert('Opearation Successfully Done...');window.location ='PrettyCashEntry.aspx';",
            true);
    }
}