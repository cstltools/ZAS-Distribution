<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_CustMasterEntry, App_Web_tmcwonuw" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
       
        .style1
        {
            VERTICAL-ALIGN: TOP;
            TEXT-ALIGN: LEFT;
            FONT-WEIGHT: BOLD;
            FONT-SIZE: 7pt;
            COLOR: #2E2E2E;
            FONT-FAMILY: Verdana,Arial,Times New Roman;
            TEXT-DECORATION: NONE;
            BACKGROUND: #F2F2F2;
            height: 21px;
        }
        .style2
        {
            VERTICAL-ALIGN: TOP;
            TEXT-ALIGN: LEFT;
            FONT-WEIGHT: NONE;
            FONT-SIZE: 9pt;
            COLOR: #000000;
            FONT-FAMILY: Estrangelo Edessa,Arial,Times New Roman;
            TEXT-DECORATION: NONE;
            BACKGROUND: #F2F2F2;
            height: 21px;
        }
       
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            Customer Master Entry
                        </td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft" style="text-align: center"> View List</td>
                        <td width="20%" class="TDRight">
                            <asp:ImageButton ID="custMastListImageButton" runat="server" 
                                ImageUrl="~/images/viewList11.png" 
                                onclick="custMasterListImageButton_Click" Height="35px" Width="35px" /></td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">
                            &nbsp;</td>
                        <td width="13%" class="TDLeft">&nbsp;</td>
                        <td width="20%" class="TDRight">
                            &nbsp;</td>
                    </tr>
                    
                    <tr>
                        <td class="TDLeft" width="13%">
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:CheckBox ID="isVatApplicable" runat="server" /> &nbsp; Is Vat Applicable
                        </td>
                        <td class="TDLeft" width="13%">
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                    </tr>

                    <tr>
                        <td class="TDLeft" width="13%"></td>
                        <td class="TDRight" width="20%"></td>
                        <td class="TDLeft" width="13%">
                            Customer Name <span class="star-mark"> * </span></td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="customernameTextBox" runat="server" CssClass="TextBox"></asp:TextBox></td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%"></td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft">Address <span class="star-mark"> * </span></td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="addressTextBox" runat="server" CssClass="TextBox"></asp:TextBox></td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            Address Second</td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="addressTextBox2" runat="server" CssClass="TextBox"></asp:TextBox>
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;</td>
                        <td width="13%" class="TDLeft">
                            Contact No</td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="contactTextBox" runat="server" CssClass="TextBox"></asp:TextBox></td>
                        <td width="13%" class="TDLeft">&nbsp;</td>
                        <td width="20%" class="TDRight"></td>
                    </tr>
                    
                    <tr>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;</td>
                        <td width="13%" class="TDLeft">
                            Drug License No </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="tbxTradeLicenseNo" runat="server" CssClass="TextBox"></asp:TextBox></td>
                        <td width="13%" class="TDLeft">&nbsp;</td>
                        <td width="20%" class="TDRight"></td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;</td>
                        <td width="13%" class="TDLeft">
                            Drug License Expiry Date</td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="tbxTradeLicenseExpiryDate" runat="server" Width="145px" CssClass="TextBoxCalander"></asp:TextBox>
                            <asp:ImageButton ID="imgTradeLicenseExpiryDate" runat="server" AlternateText="Click to show calendar"
                                ImageUrl="~/Images/Calendar_scheduleHS.png" />
                            <asp:CalendarExtender ID="tradeLicenseExpiryDateCalendar" runat="server" Format="dd-MMM-yyyy"
                                PopupButtonID="imgTradeLicenseExpiryDate" TargetControlID="tbxTradeLicenseExpiryDate">
                            </asp:CalendarExtender>
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;</td>
                        <td width="20%" class="TDRight">
                            &nbsp;</td>
                    </tr>

                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            City</td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="cityTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            Contact Person</td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="contactPersonTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                    <tr runat="server" Visible="false">
                        <td class="style1" width="13%">
                            </td>
                        <td class="style2" width="20%">
                            </td>
                        <td class="style1" width="13%">
                            Shipping Condition</td>
                        <td class="style2" width="20%">
                            <asp:TextBox ID="shippingConditionTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                        </td>
                        <td class="style1" width="13%">
                            </td>
                        <td class="style2" width="20%">
                            </td>
                    </tr>
                    

                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                        <td class="TDLeft" width="13%">
                            Region <span class="star-mark"> * </span></td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="regionDropDownList" runat="server" AutoPostBack="True" 
                                CssClass="DropDown" 
                                onselectedindexchanged="regionDropDownList_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            Area <span class="star-mark"> * </span></td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="areaDropDownList" runat="server" AutoPostBack="True" 
                                CssClass="DropDown" 
                                onselectedindexchanged="areaDropDownList_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            Territory <span class="star-mark"> * </span></td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="territoryDropDownList" runat="server" AutoPostBack="True" 
                                CssClass="DropDown" 
                                onselectedindexchanged="territoryDropDownList_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            Market<span class="star-mark"> * </span></td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="marketDropDownList" runat="server" AutoPostBack="True" 
                                CssClass="DropDown">
                            </asp:DropDownList>
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                    
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            GL Name (AIT) </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="ddlAIT" runat="server" CssClass="DropDown">
                            </asp:DropDownList>
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            Category Name <span class="star-mark"> * </span></td>
                        <td class="TDRight" width="20%">
                            <asp:RadioButtonList ID="categoryNameDropDownList" runat="server" 
                                AutoPostBack="True" CssClass="inline-rb">
                            </asp:RadioButtonList>
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                        <td class="TDLeft" width="13%">
                            Payment Type <span class="star-mark"> * </span></td>
                        <td class="TDRight" width="20%">
                            <asp:RadioButtonList ID="paymentTypeDropDownList" runat="server" 
                                AutoPostBack="True" CssClass="inline-rb">
                                <asp:ListItem>Cash</asp:ListItem>
                                <asp:ListItem>Credit</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                    </tr>
                    
                    
                     <tr id="fix" runat="server" Visible="False">
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                        <td class="TDLeft" width="13%">
                            Fixed Customer <span class="star-mark"> * </span></td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="fbDropDownList" runat="server" 
                                AutoPostBack="True" CssClass="DropDown">
                                <asp:ListItem Value="0">False</asp:ListItem>
                                <asp:ListItem Value="1">True</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                    </tr>
                        <tr runat="server" Visible="False">
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                            &nbsp;
                        </td>
                        <td width="13%" class="TDLeft" >
                            Ledger Head </td>
                            <td width="20%" class="TDRight">
                            <asp:DropDownList ID="ddlLedgerHead" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLedgerHead_OnSelectedIndexChanged" CssClass="DropDown"></asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                            &nbsp;
                        </td>
                    </tr>
                      <tr runat="server" Visible="False">
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                            &nbsp;
                        </td>
                        <td width="13%" class="TDLeft" >
                            Control Ledger </td>
                            <td width="20%" class="TDRight">
                            <asp:DropDownList ID="ddlControlLedger" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlControlLedger_OnSelectedIndexChanged"  CssClass="DropDown"></asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                            &nbsp;
                        </td>
                    </tr>
                    
                      <tr runat="server" Visible="False">
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                            &nbsp;
                        </td>
                        <td width="13%" class="TDLeft" >
                            Subsidiary Ledger </td>
                            <td width="20%" class="TDRight">
                            <asp:DropDownList ID="ddlSubsidiaryLedger" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSubsidiaryLedger_OnSelectedIndexChanged"  CssClass="DropDown"></asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                            &nbsp;
                        </td>
                    </tr>
                    
                      <tr runat="server" Visible="False">
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                            &nbsp;
                        </td>
                        <td width="13%" class="TDLeft" >
                           Sub-Subsidiary Ledger </td>
                            <td width="20%" class="TDRight">
                            <asp:DropDownList ID="ddlSubSubsidiaryLedger" AutoPostBack="True" OnSelectedIndexChanged="ddlSubSubsidiaryLedger_OnSelectedIndexChanged" runat="server"  CssClass="DropDown"></asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                            &nbsp;
                        </td>
                    </tr>
                    
                      <tr runat="server" Visible="False">
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                            &nbsp;
                        </td>
                        <td width="13%" class="TDLeft" >
                            Account Name </td>
                            <td width="20%" class="TDRight">
                            <asp:DropDownList ID="ddlAccountName" runat="server"  CssClass="DropDown"></asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                            &nbsp;
                        </td>
                    </tr>
                    

                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            <asp:Button ID="submitButton" runat="server" onclick="submitButton_Click1" 
                                Text="Submit" />

                                <asp:Button ID="clearButton" CssClass="clearButton" BackColor="#E1B12C" runat="server" OnClick="clearButton_OnClick"
                                Text="Clear" /> 
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    
     <asp:UpdateProgress ID="UpdateProgress1" runat="server" ClientIDMode="Static" DisplayAfter="0" DynamicLayout="true">
        <ProgressTemplate>
            <div class="divWaiting">
                <asp:Image ID="imgWait" CssClass="position-set" runat="server" ImageAlign="Middle" ImageUrl="~/images/progress-bar-opt.gif"
                    Height="80" Width="80" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


</asp:Content>

