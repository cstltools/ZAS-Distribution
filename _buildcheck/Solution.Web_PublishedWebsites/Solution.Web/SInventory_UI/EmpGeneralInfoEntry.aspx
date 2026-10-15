<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="HRM_UI_EmpGeneralInfo, App_Web_3bs1ezh0" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            vertical-align: TOP;
            text-align: right;
            font-weight: bold;
            font-size: 9pt;
            color: #000000;
            font-family: Estrangelo Edessa,Arial,Times New Roman;
            text-decoration: NONE;
            background: #F2F2F2; /*C0C0C0*/
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="../scripts/jquery.min.js" type="text/javascript"></script>
    <script src="../scripts/jquery-ui.js" type="text/javascript"></script>
    <%--<script src="../scripts/jquery.js" type="text/javascript"></script>--%>
    <link href="../css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link type="text/css" href="../css/Date/jquery.datepick.css" rel="stylesheet">
    <script type="text/javascript" src="../css/Date/jquery.datepick.js"></script>
    <script type="text/javascript">
        $(function () {
            $('.datepick').datepick();
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {

                    $('.datepick').datepick();
                }
            });
        }; 
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#accordion').accordion({
                collapsible: true,
                active: 0,
                autoHeight: false
            });
        });
        ////On UpdatePanel Refresh
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {

                    $("#accordion").accordion({
                        collapsible: true,
                        active: parseInt(document.getElementById('<%= this.hidAccordionIndex.ClientID %>').value),
                        autoHeight: false
                    });
                }
            });
        };         
    </script>
    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
    <div>
        <table width="100%" class="TableWorkArea">
            <tr>
                <td colspan="6" class="TableHeading">
                    Employee General Information
                </td>
            </tr>
            <tr>
                <td class="TDLeft" width="13%">
                    &nbsp;
                </td>
                <td class="TDRight" width="20%">
                    &nbsp;
                </td>
                <td class="TDLeft" width="13%">
                    <asp:HiddenField ID="empIdHiddenField" runat="server" />
                    &nbsp;
                </td>
                <td class="TDRight" width="20%">
                    &nbsp;
                </td>
                <td class="TDLeft" width="13%">
                    &nbsp;
                </td>
                <td class="TDRight" width="20%">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td width="13%" class="TDLeft" style="text-align: center">
                    View List
                </td>
                <td width="20%" class="TDRight">
                    <asp:ImageButton ID="empViewImageButton" runat="server" ImageUrl="~/images/viewList.png"
                        OnClick="empViewImageButton_Click" />
                </td>
                <td width="13%" class="TDLeft">
                </td>
                <td width="20%" class="TDRight">
                    &nbsp;
                </td>
                <td width="13%" class="TDLeft">
                </td>
                <td width="20%" class="TDRight">
                </td>
            </tr>
            <tr>
                <td width="13%" class="TDLeft">
                    &nbsp;
                </td>
                <td class="TDRight" width="20%">
                    &nbsp;
                </td>
                <td width="13%" class="TDLeft">
                    &nbsp;
                </td>
                <td width="20%" class="TDRight">
                    &nbsp;
                </td>
                <td width="13%" class="TDLeft">
                    &nbsp;
                </td>
                <td width="20%" class="TDRight">
                    &nbsp;
                </td>
            </tr>
            <tr runat="server">
                <td class="TDLeft" width="13%">
                    &nbsp;
                </td>
                <td class="style1" width="20%">
                    Company Name: <span class="star-mark">* </span>
                </td>
                <td class="TDLeft" width="13%">
                    <asp:DropDownList ID="companyNameDropDownList" runat="server" CssClass="DropDown">
                    </asp:DropDownList>
                    &nbsp;
                </td>
                
                <td width="13%" class="TDLeft">
                    &nbsp;
                </td>
                <td width="20%" class="TDRight">
                    &nbsp;
                </td>
                <td class="TDRight" width="20%">
                    &nbsp;
                </td>
            </tr>
            <div>
                <tr>
                    <td width="13%" class="TDLeft">
                        &nbsp;
                    </td>
                    <td class="style1" width="20%">
                        Employee Name: <span class="star-mark">* </span>
                    </td>
                    <td width="13%" class="TDLeft">
                        <asp:TextBox ID="empNameTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                    </td>
                    <td class="style1" width="20%">
                        Joining Date: <span class="star-mark">* </span>
                    </td>
                    <td width="20%" class="TDRight">
                        <asp:TextBox ID="joiningDateTextBox" runat="server" CssClass="datepick" Width="157px" AutoPostBack="True" OnTextChanged="joiningDateTextBox_OnTextChanged"></asp:TextBox>
                        &nbsp;
                    </td>
                    <td class="TDRight" width="20%">
                        &nbsp;
                    </td>
                </tr>
            </div>
            <tr>
                <td class="TDLeft" width="13%">
                    &nbsp;
                </td>
                <td class="style1" width="20%">
                    Short Name: <span class="star-mark">* </span>
                </td>
                <td class="TDLeft" width="13%">
                    <asp:TextBox ID="shortNameTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                </td>
                <td class="style1" width="20%">
                    Designation:<span class="star-mark"> * </span>
                </td>
                <td width="20%" class="TDRight">
                    <asp:DropDownList ID="designationDropDownList" runat="server" CssClass="DropDown">
                    </asp:DropDownList>
                </td>
                <td class="TDRight" width="20%">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="TDLeft" width="13%">
                    &nbsp;
                </td>
                <td class="style1" width="20%">
                    Father Name: <span class="star-mark">* </span>
                </td>
                <td class="TDLeft" width="13%">
                    <asp:TextBox ID="fatherNameTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                </td>
                <td class="style1" width="20%">
                    Department: <span class="star-mark">* </span>
                </td>
                <td width="20%" class="TDRight">
                    <asp:DropDownList ID="departmentDropDownList" runat="server" CssClass="DropDown">
                    </asp:DropDownList>
                </td>
                <td class="TDRight" width="20%">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="TDLeft" width="13%">
                    &nbsp;
                </td>
                <td class="style1" width="20%">
                    Mother Name: <span class="star-mark">* </span>
                </td>
                <td class="TDLeft" width="13%">
                    <asp:TextBox ID="motherNameTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                </td>
                <td class="style1" width="20%">
                    Gender:<span class="star-mark"> * </span>
                </td>
                <td class="TDLeft" width="13%">
                    <asp:DropDownList ID="genderDropDown" runat="server" CssClass="DropDown">
                        <asp:ListItem>--------Select--------</asp:ListItem>
                        <asp:ListItem>Male</asp:ListItem>
                        <asp:ListItem>Female</asp:ListItem>
                        <asp:ListItem>Other</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="TDRight" width="20%">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="TDLeft" width="13%">
                    &nbsp;
                </td>
                <td class="style1" width="20%">
                    Religion: <span class="star-mark">* </span>
                </td>
                <td class="TDLeft" width="13%">
                    <asp:DropDownList ID="religionDropDown" runat="server" CssClass="DropDown">
                        <asp:ListItem>---------Select---------</asp:ListItem>
                        <asp:ListItem>Muslim</asp:ListItem>
                        <asp:ListItem>Hindu</asp:ListItem>
                        <asp:ListItem>Christian</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="style1" width="20%">
                    Blood Group&nbsp;:
                </td>
                <td class="TDLeft" width="13%">
                    <asp:DropDownList ID="bloodGroupDropDown" runat="server" CssClass="DropDown">
                        <asp:ListItem>---------Select---------</asp:ListItem>
                        <asp:ListItem>A+</asp:ListItem>
                        <asp:ListItem>A-</asp:ListItem>
                        <asp:ListItem>B+</asp:ListItem>
                        <asp:ListItem>B-</asp:ListItem>
                        <asp:ListItem>O+</asp:ListItem>
                        <asp:ListItem>O-</asp:ListItem>
                        <asp:ListItem>AB+</asp:ListItem>
                        <asp:ListItem>AB-</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="TDRight" width="20%">
                    &nbsp;
                </td>
                <tr>
                    <td class="TDLeft" width="13%">
                        &nbsp;
                    </td>
                    <td class="style1" width="20%">
                        Nationality: <span class="star-mark">* </span>
                    </td>
                    <td class="TDLeft" width="13%">
                        <asp:DropDownList ID="nationalityStatusDropDown" runat="server" CssClass="DropDown">
                            <asp:ListItem>---------Select---------</asp:ListItem>
                            <asp:ListItem>Bangladeshi</asp:ListItem>
                            <asp:ListItem>Indian</asp:ListItem>
                            <asp:ListItem>Others</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="style1" width="20%">
                        Marital Status :
                    </td>
                    <td class="TDRight" width="20%">
                        <asp:DropDownList ID="maritalStatusDropDown" runat="server" CssClass="DropDown">
                            <asp:ListItem>---------Select---------</asp:ListItem>
                            <asp:ListItem>Married</asp:ListItem>
                            <asp:ListItem>UnMarried</asp:ListItem>
                            <asp:ListItem>Other</asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;
                    </td>
                    <td class="TDRight" width="20%">
                        &nbsp;
                    </td>
                </tr>
            </tr>
            <tr>
                <td class="TDLeft" width="13%">
                    &nbsp;
                </td>
                <td class="style1" width="20%">
                    Date Of Birth: <span class="star-mark">* </span>
                </td>
                <td class="TDLeft" width="13%">
                    <asp:TextBox ID="dateOfBirthTextBox" runat="server" Width="157px" CssClass="datepick" AutoPostBack="true" OnTextChanged="dateOfBirthTextBox_OnTextChanged"></asp:TextBox>
                </td>
                <td class="style1" width="20%">
                    National Id No:<span class="star-mark"> * </span>
                </td>
                <td class="TDLeft">
                    <asp:TextBox ID="nationalIdNoTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                        TargetControlID="nationalIdNoTextBox" FilterType="Custom" ValidChars="0123456789">
                    </asp:FilteredTextBoxExtender>
                </td>
                <td class="TDRight" width="20%">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="TDLeft" width="13%">
                    &nbsp;
                </td>
                <td class="style1" width="20%">
                    Place Of Birth&nbsp; :
                </td>
                <td class="TDLeft" width="13%">
                    <asp:TextBox ID="placeOfBirthTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                </td>
                <td class="style1" width="20%">
                    Mobile No: <span class="star-mark">* </span>
                </td>
                <td class="TDLeft" width="13%">
                    <asp:TextBox ID="cellNumberTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                        TargetControlID="cellNumberTextBox" FilterType="Custom" ValidChars="0123456789">
                    </asp:FilteredTextBoxExtender>
                </td>
                <td class="TDRight" width="20%">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="TDLeft" width="13%">
                    &nbsp;
                </td>
                <td class="style1" width="20%">
                    Present Address: <span class="star-mark">* </span>
                </td>
                <td class="TDLeft" width="13%">
                    <asp:TextBox ID="prtAddressTextBox" runat="server" CssClass="TextBox" TextMode="MultiLine"></asp:TextBox>
                </td>
                <td class="style1" width="20%">
                    Permanent Address: <span class="star-mark">* </span>
                </td>
                <td class="TDLeft" width="13%">
                    <asp:TextBox ID="permAddressTextBox" runat="server" CssClass="TextBox" TextMode="MultiLine"></asp:TextBox>
                </td>
                <td class="TDRight" width="20%">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="TDLeft" width="13%">
                    &nbsp;
                </td>
                <td class="style1" width="20%">
                    Present City :&nbsp; <span class="star-mark">&nbsp;* </span>
                </td>
                <td class="TDLeft" width="13%">
                    <asp:TextBox ID="precityTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                </td>
                <td class="style1" width="20%">
                   Permanent City: <span class="star-mark">* </span>
                </td>
                <td class="TDLeft" width="13%">
                    <asp:TextBox ID="parcityTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                </td>
                <td class="TDRight" width="20%">
                    &nbsp;
                </td>
            </tr>
            
            <tr>
                <td class="TDLeft" width="13%">
                    &nbsp;
                </td>
                <td class="style1" width="20%">
                    Reference Name:
                </td>
                <td class="TDLeft" width="13%">
                    <asp:TextBox ID="referenceNameTextBox" runat="server" CssClass="TextBox" ></asp:TextBox>
                </td>
                <td class="style1" width="20%">
                    Reference Contact No:
                </td>
                <td class="TDLeft" width="13%">
                    <asp:TextBox ID="referenceContactNoTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                        TargetControlID="referenceContactNoTextBox" FilterType="Custom" ValidChars="0123456789">
                    </asp:FilteredTextBoxExtender>
                </td>
                <td class="TDRight" width="20%">
                    &nbsp;
                </td>
            </tr>

            <tr>
                <td class="TDLeft" width="13%">
                    &nbsp;
                </td>
                <td class="style1" width="20%">
                   Emergency Contact No:
                </td>
                <td class="TDLeft" width="13%">
                   <asp:TextBox ID="emergencyNoTextBox" runat="server" CssClass="TextBox"></asp:TextBox> &nbsp;
                   <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                        TargetControlID="emergencyNoTextBox" FilterType="Custom" ValidChars="0123456789">
                    </asp:FilteredTextBoxExtender>
                </td>
                <td class="style1" width="20%">
                    Email:<span class="star-mark">* </span>
                </td>
                <td class="TDRight" width="20%">
                    <asp:TextBox ID="emailTextBox" runat="server" CssClass="TextBox" AutoPostBack="True" OnTextChanged="emailTextBox_OnTextChanged"></asp:TextBox>&nbsp;
                </td>
                <td class="TDRight" width="20%">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="TDLeft" width="13%">
                    &nbsp;
                </td>
                <td class="TDRight" width="20%">
                    &nbsp;
                </td>
                <td class="TDLeft" width="13%">
                    &nbsp;
                </td>
                <td class="TDRight" width="20%">
                    &nbsp;
                </td>
                <td class="TDLeft" width="13%">
                    &nbsp;
                </td>
                <td class="TDRight" width="20%">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="TDLeft" width="13%">
                    &nbsp;
                </td>
                <td class="TDRight" width="20%">
                    &nbsp;
                </td>
                <td class="TDLeft" width="13%">
                    &nbsp;
                </td>
                <td class="TDRight" width="20%">
                    <asp:Button ID="submitButton" runat="server" OnClick="submitButton_Click" Text="Submit" />
                    <asp:Button ID="clearButton" CssClass="clearButton" BackColor="#E1B12C" runat="server"
                        OnClick="clearButton_OnClick" Text="Clear" />
                </td>
                <td class="TDLeft" width="13%">
                    &nbsp;
                </td>
                <td class="TDRight" width="20%">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="TDLeft" width="13%">
                    &nbsp;
                </td>
                <td class="TDRight" width="20%">
                    &nbsp;
                </td>
                <td class="TDLeft" width="13%">
                    &nbsp;
                </td>
                <td class="TDRight" width="20%">
                    &nbsp;
                </td>
                <td class="TDLeft" width="13%">
                    &nbsp;
                </td>
                <td class="TDRight" width="20%">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="TDLeft" width="13%">
                    &nbsp;
                </td>
                <td class="TDRight" width="20%">
                    &nbsp;
                </td>
                <td class="TDLeft" width="13%">
                    &nbsp;
                </td>
                <td class="TDRight" width="20%">
                    &nbsp;
                </td>
                <td class="TDLeft" width="13%">
                    &nbsp;
                </td>
                <td class="TDRight" width="20%">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="TDLeft" width="13%">
                    &nbsp;
                </td>
                <td class="TDRight" width="20%">
                    &nbsp;
                </td>
                <td class="TDLeft" width="13%">
                    &nbsp;
                </td>
                <td class="TDRight" width="20%">
                    &nbsp;
                </td>
                <td class="TDLeft" width="13%">
                    &nbsp;
                </td>
                <td class="TDRight" width="20%">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="TDLeft" width="13%">
                    &nbsp;
                </td>
                <td class="TDRight" width="20%">
                    &nbsp;
                </td>
                <td class="TDLeft" width="13%">
                    &nbsp;
                </td>
                <td class="TDRight" width="20%">
                    &nbsp;
                </td>
                <td class="TDLeft" width="13%">
                    &nbsp;
                </td>
                <td class="TDRight" width="20%">
                    &nbsp;
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hidAccordionIndex" runat="server" Value="1" />
        <div id="accordion" width="100%">
        </div>
    </div>
    
     <asp:UpdateProgress ID="UpdateProgress1" runat="server" ClientIDMode="Static" DisplayAfter="0" DynamicLayout="true">
        <ProgressTemplate>
            <div class="divWaiting">
                <asp:Image ID="imgWait" CssClass="position-set" runat="server" ImageAlign="Middle" ImageUrl="~/images/progress-bar-opt.gif"
                    Height="80" Width="80" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
