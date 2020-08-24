using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;


public partial class HRM_UI_EmpGeneralInfo : System.Web.UI.Page
{
    EmpGeneralInfoBLL aInfoBll=new EmpGeneralInfoBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            hidAccordionIndex.Value = "3";
            DepartmentNameLoad();
            DesignationNameLoad();
            LoadCompanyInfo();

            if (Session["EmpId"] != null)
            {
                GetOneRecord(Session["EmpId"].ToString());
                Session["EmpId"] = null;
            }

        }      
    }

    private void LoadCompanyInfo()
    {
        aInfoBll.LoadCompanyDropdownList(companyNameDropDownList);
    }

    private void GetOneRecord(string empId)
    {
        var aInfo = new EmpGeneralInfo();

        aInfo = aInfoBll.EmpGeneralInfoEditLoad(empId);

        empIdHiddenField.Value = aInfo.EmpInfoId.ToString(CultureInfo.InvariantCulture);
        designationDropDownList.SelectedValue = Convert.ToString(aInfo.DesignationId);
        departmentDropDownList.SelectedValue = Convert.ToString(aInfo.DepartmentId);

        companyNameDropDownList.SelectedValue = Convert.ToString(aInfo.CompanyId);
        emergencyNoTextBox.Text = aInfo.EmrgContactNo;

        referenceNameTextBox.Text = aInfo.RefName;
        referenceContactNoTextBox.Text = aInfo.RefContactNo;
        emailTextBox.Text = aInfo.Email;


        empNameTextBox.Text = aInfo.EmpName;
        shortNameTextBox.Text = aInfo.ShortName;
        fatherNameTextBox.Text = aInfo.FatherName;
        motherNameTextBox.Text = aInfo.MotherName;
        religionDropDown.Text = aInfo.Religion;
        nationalityStatusDropDown.Text = aInfo.Nationality;
        dateOfBirthTextBox.Text = aInfo.DateOfBirth.ToString("dd-MMM-yyyy"); ;
        placeOfBirthTextBox.Text = aInfo.PlaceOfBirth;
        precityTextBox.Text = aInfo.PreCity;
        parcityTextBox.Text = aInfo.PerCity;
        bloodGroupDropDown.SelectedValue = bloodGroupDropDown.Items.FindByText(aInfo.BloodGroup).Value;
        genderDropDown.SelectedValue = genderDropDown.Items.FindByText(aInfo.Gender).Value;
        prtAddressTextBox.Text = aInfo.AddressPresent;
        permAddressTextBox.Text = aInfo.AddressPermanent;
        //medicalTextBox.Text = aInfo.MedicalInformation;
        //phoneNoTextBox.Text = aInfo.PhoneNo;
        cellNumberTextBox.Text = aInfo.CellNumber;
        //emailTextBox.Text = aInfo.Email;
        maritalStatusDropDown.SelectedValue = maritalStatusDropDown.Items.FindByText(aInfo.MaritalStatus).Value;
        nationalIdNoTextBox.Text = aInfo.NationalIdNo;
        joiningDateTextBox.Text = aInfo.JoiningDate.ToString("dd-MMM-yyyy");
        empNameTextBox.Text = aInfo.EmpName;
        //referanceNameTextBox.Text = aInfo.ReferanceName;
        //refranceCellNoTextBox.Text = aInfo.ReferanceCellNo;
        //refranceAddressTextBox.Text = aInfo.ReferanceAddress;

        submitButton.Text = "Update";
    }

    protected void previewButton_Click(object sender, EventArgs e)
    {
        hidAccordionIndex.Value = "0";
        //Session["ImageBytes"] = pictureFileUpload.FileBytes;
        //pictureImage.ImageUrl = "~/PictureHandler.ashx";
    }

    protected void sigpreviewButton_Click(object sender, EventArgs e)
    {
        hidAccordionIndex.Value = "1";
        //Session["SigImageBytes"] = sigFileUpload.FileBytes;
        //signatureImage.ImageUrl = "~/SignutreHandler.ashx";
    }

    private void Clear()
    {            
            empNameTextBox.Text = string.Empty;
            shortNameTextBox.Text = string.Empty;
            fatherNameTextBox.Text = string.Empty;
            fatherNameTextBox.Text = string.Empty;
            motherNameTextBox.Text = string.Empty;
            religionDropDown.SelectedIndex = 0;
            nationalityStatusDropDown.SelectedIndex = 0;
            dateOfBirthTextBox.Text = string.Empty;
            placeOfBirthTextBox.Text = string.Empty;
            
            permAddressTextBox.Text = string.Empty;
            prtAddressTextBox.Text = string.Empty;
            //emailTextBox.Text = string.Empty;
            //phoneNoTextBox.Text = string.Empty;
            cellNumberTextBox.Text = string.Empty;
            //medicalTextBox.Text = string.Empty;
            //pictureImage.ImageUrl = null;
            //signatureImage.ImageUrl = null;
            nationalIdNoTextBox.Text = string.Empty;
            //referanceNameTextBox.Text = string.Empty;
            //refranceAddressTextBox.Text = string.Empty;
            //refranceCellNoTextBox.Text = string.Empty;
            joiningDateTextBox.Text = string.Empty;
            departmentDropDownList.SelectedValue = null;
            designationDropDownList.SelectedValue = null;

        companyNameDropDownList.SelectedValue = "";
        emailTextBox.Text = "";

        bloodGroupDropDown.SelectedValue = null;
        genderDropDown.SelectedValue = null;
        maritalStatusDropDown.SelectedValue = null;
        precityTextBox.Text = "";
        parcityTextBox.Text = "";
        referenceNameTextBox.Text = "";
        referenceContactNoTextBox.Text = "";
        emergencyNoTextBox.Text = "";

        submitButton.Text = "Submit";



    }

    private void ShowMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    public void DesignationNameLoad()
    {
        aInfoBll.LoadDesignationToDropDownBLL(designationDropDownList);
    }
    public void DepartmentNameLoad()
    {
        aInfoBll.LoadDepartmentToDropDownBLL(departmentDropDownList);
    }
    public void RefDetail()
    {
        //if (referanceNameTextBox.Text=="N/A")
        //{
        //    refranceAddressTextBox.Text = "N/A";
        //    refranceCellNoTextBox.Text = "N/A";
        //    hidAccordionIndex.Value = "1";
        //}
        hidAccordionIndex.Value = "1";
    }

    private bool Validation()
    {
        if (companyNameDropDownList.Text == "")
        {
            ShowMessageBox("Please Select a company!!");
            return false;
        }

        if (empNameTextBox.Text == "")
        {
            ShowMessageBox("Please Input Employee Name!!");
            return false;
        }
        if (shortNameTextBox.Text == "")
        {
            ShowMessageBox("Please Input  Employee Short Name!!");
            return false;
        }

        if (fatherNameTextBox.Text == "")
        {
            ShowMessageBox("Please Input Father Name!!");
            return false;
        }
        if (motherNameTextBox.Text == "")
        {
            ShowMessageBox("Please Input Mother Name!!");
            return false;
        }
        if (religionDropDown.SelectedIndex == 0)
        {
            ShowMessageBox("Please Input Religion!!");
            return false;
        }

        if (nationalityStatusDropDown.SelectedIndex ==0)
        {
            ShowMessageBox("Please Input nationality!!");
            return false;
        }
        if (dateOfBirthTextBox.Text == "")
        {
            ShowMessageBox("Please Input Date Of Birth!!");
            return false;
        }
        if (placeOfBirthTextBox.Text == "")
        {
            ShowMessageBox("Please Input Place Of Birth !!");
            return false;
        }

        if (prtAddressTextBox.Text == "")
        {
            ShowMessageBox("Please Input Present Address!!");
            return false;
        }
        if (permAddressTextBox.Text == "")
        {
            ShowMessageBox("Please Input Permanent Address!!");
            return false;
        }
        if (nationalIdNoTextBox.Text == "")
        {
            ShowMessageBox("Please Input National Id!!");
            return false;
        }

        if (genderDropDown.Text == "")
        {
            ShowMessageBox("Please Input gender!!");
            return false;
        }
        if (bloodGroupDropDown.Text == "")
        {
            ShowMessageBox("Please Input Blood Group!!");
            return false;
        }

        if (maritalStatusDropDown.Text == "")
        {
            ShowMessageBox("Please Input Marital Status!!");
            return false;
        }
        if (designationDropDownList.Text == "")
        {
            ShowMessageBox("Please Input Designation!!");
            return false;
        }

        if (departmentDropDownList.Text == "")
        {
            ShowMessageBox("Please Input Department!!");
            return false;
        }
        if (cellNumberTextBox.Text == "")
        {
            ShowMessageBox("Please Input Mobile No!!");
            return false;
        }

        if (emailTextBox.Text == "")
        {
            ShowMessageBox("Please Input Email!!");
            return false;
        }
        if (joiningDateTextBox.Text == "")
        {
            ShowMessageBox("Please Input joining Date!!");
            return false;
        }
        
        return true;
    }

    protected void submitButton_Click(object sender, EventArgs e)
    {
        EmpGeneralInfo aInformation = new EmpGeneralInfo();
        //FileUpload img = (FileUpload)pictureFileUpload;
        //FileUpload sigimage = (FileUpload)sigFileUpload;

        //if (img.HasFile && img.PostedFile != null && sigimage.HasFile && sigimage.PostedFile != null)
        //{
        //    HttpPostedFile sigFile = sigFileUpload.PostedFile;
        //    HttpPostedFile File = pictureFileUpload.PostedFile;
        //    aInformation.EmpImage = new Byte[File.ContentLength];
        //    aInformation.SignatureImage = new Byte[sigFile.ContentLength];
        //    File.InputStream.Read(aInformation.SignatureImage, 0, File.ContentLength);
        //    File.InputStream.Read(aInformation.EmpImage, 0, sigFile.ContentLength);
        //}

        if (Validation())
        {


            if (empIdHiddenField.Value != "")
            {
                UpdateEmpInfo();
            }
            else
            {
                EmpGeneralInfo employeeInformation = new EmpGeneralInfo()

                {
                    EmpName = empNameTextBox.Text.Trim(),
                    CompanyId = Convert.ToInt32(companyNameDropDownList.SelectedValue),
                    ShortName = shortNameTextBox.Text.Trim(),
                    FatherName = fatherNameTextBox.Text.Trim(),
                    MotherName = motherNameTextBox.Text.Trim(),
                    Religion = religionDropDown.SelectedItem.Text.Trim(),
                    Nationality = nationalityStatusDropDown.SelectedItem.Text.Trim(),
                    DateOfBirth = Convert.ToDateTime(dateOfBirthTextBox.Text.Trim()),
                    PlaceOfBirth = placeOfBirthTextBox.Text.Trim(),
                    BloodGroup = bloodGroupDropDown.Text.Trim(),
                    Gender = genderDropDown.SelectedItem.Text.Trim(),
                    AddressPresent = prtAddressTextBox.Text.Trim(),
                    PerCity = precityTextBox.Text.Trim(),
                    PreCity = parcityTextBox.Text.Trim(),
                    AddressPermanent = permAddressTextBox.Text.Trim(),
                    //MedicalInformation = medicalTextBox.Text,
                    //PhoneNo = phoneNoTextBox.Text,
                    CellNumber = cellNumberTextBox.Text.Trim(),
                    Email = emailTextBox.Text.Trim(),
                    MaritalStatus = maritalStatusDropDown.SelectedItem.Text.Trim(),
                    NationalIdNo = nationalIdNoTextBox.Text.Trim(),
                    RefName = referenceNameTextBox.Text.Trim(),
                    EmrgContactNo = emergencyNoTextBox.Text.Trim(),
                    RefContactNo = referenceContactNoTextBox.Text.Trim(),
                    DepartmentId = Convert.ToInt32(departmentDropDownList.SelectedValue),
                    JoiningDate = Convert.ToDateTime(joiningDateTextBox.Text.Trim()),
                    //Designation = designationDropDownList.SelectedItem.Text.Trim(),
                    DesignationId = Convert.ToInt32(designationDropDownList.SelectedValue),
                    //DeptName = departmentDropDownList.SelectedItem.Text.Trim(),
                    EntryBy = Session["LoginName"].ToString(),
                    EntryDate = DateTime.Now

                };

                var employeeBll = new EmpGeneralInfoBLL();

                ShowMessageBox(employeeBll.SaveDataFoEmployeeInfo(employeeInformation));
                Clear();
            }
        }
        else
        {
           ShowMessageBox("Please Input Data In All TextBox!");
        }
    }

    private void UpdateEmpInfo()
    {
        EmpGeneralInfo employeeInformation = new EmpGeneralInfo()

        {
            EmpInfoId = Convert.ToInt32(empIdHiddenField.Value),
            EmpName = empNameTextBox.Text.Trim(),
            CompanyId = Convert.ToInt32(companyNameDropDownList.SelectedValue),
            ShortName = shortNameTextBox.Text.Trim(),
            FatherName = fatherNameTextBox.Text.Trim(),
            MotherName = motherNameTextBox.Text.Trim(),
            Religion = religionDropDown.SelectedItem.Text.Trim(),
            Nationality = nationalityStatusDropDown.SelectedItem.Text.Trim(),
            DateOfBirth = Convert.ToDateTime(dateOfBirthTextBox.Text.Trim()),
            PlaceOfBirth = placeOfBirthTextBox.Text.Trim(),
            BloodGroup = bloodGroupDropDown.Text.Trim(),
            Gender = genderDropDown.SelectedItem.Text.Trim(),
            AddressPresent = prtAddressTextBox.Text.Trim(),
            PerCity = precityTextBox.Text.Trim(),
            PreCity = parcityTextBox.Text.Trim(),
            AddressPermanent = permAddressTextBox.Text.Trim(),
            //MedicalInformation = medicalTextBox.Text,
            //PhoneNo = phoneNoTextBox.Text,
            CellNumber = cellNumberTextBox.Text.Trim(),
            Email = emailTextBox.Text.Trim(),
            MaritalStatus = maritalStatusDropDown.SelectedItem.Text.Trim(),
            NationalIdNo = nationalIdNoTextBox.Text.Trim(),
            RefName = referenceNameTextBox.Text.Trim(),
            EmrgContactNo = emergencyNoTextBox.Text.Trim(),
            RefContactNo = referenceContactNoTextBox.Text.Trim(),
            DepartmentId = Convert.ToInt32(departmentDropDownList.SelectedValue),
            JoiningDate = Convert.ToDateTime(joiningDateTextBox.Text.Trim()),
            //Designation = designationDropDownList.SelectedItem.Text.Trim(),
            DesignationId = Convert.ToInt32(designationDropDownList.SelectedValue),
            //DeptName = departmentDropDownList.SelectedItem.Text.Trim(),
            UpdateBy = Session["LoginName"].ToString(),
            UpdateDate = DateTime.Now
        };

        if (aInfoBll.UpdateDataForEmpGeneralInfo(employeeInformation))
        {
            ShowMessageBox("Information Updated Successfully !!!");
            Clear();
        }

        else
        {
            ShowMessageBox("Not Updated!!!");
            //Clear();
        }
    }

    protected void empViewImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("EmpGenInfView.aspx");
    }


    protected void referanceNameTextBox_TextChanged(object sender, EventArgs e)
    {
        hidAccordionIndex.Value = "1";
        RefDetail();
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Clear();
    }

    private const string MatchEmailPattern =
            @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
     + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
     + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
     + @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$";

    private static bool IsEmail(string email)
    {
        if (email != null) 
            return Regex.IsMatch(email, MatchEmailPattern);
        else 
            return false;
    }


    protected void emailTextBox_OnTextChanged(object sender, EventArgs e)
    {
        if (emailTextBox.Text != "")
        {
            if (!IsEmail(emailTextBox.Text.Trim()))
            {
                emailTextBox.Text = "";
                ShowMessageBox("Please insert valid email address!!!");
            }
        }
    }


    protected void dateOfBirthTextBox_OnTextChanged(object sender, EventArgs e)
    {
        if (dateOfBirthTextBox.Text != "")
        {
            string dateString = dateOfBirthTextBox.Text.Trim();
            
            DateTime dateValue;

            if (!DateTime.TryParse(dateString, out dateValue))
            {
                dateOfBirthTextBox.Text = "";
                ShowMessageBox("Please insert a valid date!!!");
            }
        }   
   
    }

    protected void joiningDateTextBox_OnTextChanged(object sender, EventArgs e)
    {
        if (joiningDateTextBox.Text != "")
        {
            string dateString = joiningDateTextBox.Text.Trim();

            DateTime dateValue;

            if (!DateTime.TryParse(dateString, out dateValue))
            {
                joiningDateTextBox.Text = "";
                ShowMessageBox("Please insert a valid date!!!");
            }
        }   
    }  
}