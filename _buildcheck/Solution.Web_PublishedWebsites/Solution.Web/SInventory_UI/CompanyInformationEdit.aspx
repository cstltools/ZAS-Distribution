<%@ page language="C#" autoeventwireup="true" inherits="SInventory_UI_CompanyInformationEdit, App_Web_ufowa0j4" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Edit</title>
    <link href="../css/custom.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../css/style.css" type="text/css">
    <link rel="stylesheet" href="../css/colors/blue.css" id="colors" type="text/css">
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div>
                        <table width="100%" class="TableWorkArea">
                            <tr>
                                <td colspan="6" class="TableHeading">
                                    Company Information Edit
                                </td>
                            </tr>
                            <tr>
                                <td class="TDLeft" width="13%">
                                    &nbsp;
                                    <asp:HiddenField ID="companyInfoIdHiddenField" runat="server" />
                                </td>
                                <td class="TDRight" width="20%">
                                </td>
                                <td class="TDLeft" width="13%">
                                </td>
                                <td class="TDRight" width="20%">
                                    <asp:Label ID="MessageLabel" runat="server" ForeColor="Black"></asp:Label>
                                </td>
                                <td class="TDLeft" width="13%">
                                </td>
                                <td class="TDRight" width="20%">
                                </td>
                            </tr>
                            <tr>
                                <td class="TDLeft" width="13%">
                                </td>
                                <td class="TDRight" width="20%">
                                </td>
                                <td class="TDLeft" width="13%">
                                </td>
                                <td class="TDRight" width="20%">
                                    &nbsp;
                                </td>
                                <td class="TDLeft" width="13%">
                                    &nbsp;
                                </td>
                                <td class="TDRight" width="20%">
                                </td>
                            </tr>
                            <tr>
                                <td class="TDLeft" width="13%">
                                </td>
                                <td class="TDRight" width="20%">
                                </td>
                                <td class="TDLeft" width="13%">
                                    Group Name  <span style="color:red"> * </span>
                                </td>
                                <td class="TDRight" width="20%">
                                    <asp:DropDownList ID="groupDropDownList" CssClass="DropDown" runat="server" Enabled="false">
                                    </asp:DropDownList>
                                </td>
                                <td class="TDLeft" width="13%">
                                    &nbsp;
                                </td>
                                <td class="TDRight" width="20%">
                                </td>
                            </tr>
                            <tr>
                                <td class="TDLeft" width="13%">
                                </td>
                                <td class="TDRight" width="20%">
                                </td>
                                <td class="TDLeft" width="13%">
                                    Company Name <span style="color:red"> * </span>
                                </td>
                                <td class="TDRight" width="20%">
                                    <asp:TextBox ID="companyNameTextBox" runat="server" CssClass="TextBox" AutoPostBack="True" OnTextChanged="companyNameTextBox_OnTextChanged"></asp:TextBox>
                                    <asp:HiddenField ID="cmpNameHiddenField" runat="server" />
                                    <asp:HiddenField ID="cmpShortHiddenField" runat="server" />
                                </td>
                                <td class="TDLeft" width="13%">
                                </td>
                                <td class="TDRight" width="20%">
                                </td>
                            </tr>
                            <tr>
                                <td class="TDLeft" width="13%">
                                </td>
                                <td class="TDRight" width="20%">
                                </td>
                                <td class="TDLeft" width="13%">
                                    Company Short Name 
                                </td>
                                <td class="TDRight" width="20%">
                                    <asp:TextBox ID="cmpShortNameTextBox" runat="server" CssClass="TextBox" AutoPostBack="true" OnTextChanged="cmpShortNameTextBox_OnTextChanged"></asp:TextBox>
                                </td>
                                <td class="TDLeft" width="13%">
                                    &nbsp;
                                </td>
                                <td class="TDRight" width="20%">
                                </td>
                            </tr>
                            <tr>
                                <td class="TDLeft" width="13%">
                                </td>
                                <td class="TDRight" width="20%">
                                    &nbsp;
                                </td>
                                <td class="TDLeft" width="13%">
                                    Address <span style="color:red"> * </span>
                                </td>
                                <td class="TDRight" width="20%">
                                    <asp:TextBox ID="addressNameTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                                </td>
                                <td class="TDLeft" width="13%">
                                    &nbsp;
                                </td>
                                <td class="TDRight" width="20%">
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
                                    Contact No <span style="color:red"> * </span>
                                </td>
                                <td class="TDRight" width="20%">
                                    <asp:TextBox ID="contactNoTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                                </td>
                                <td class="TDLeft" width="13%">
                                    &nbsp;
                                </td>
                                <td class="TDRight" width="20%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td width="13%" class="TDLeft">
                                    &nbsp;
                                </td>
                                <td width="20%" class="TDRight">
                                </td>
                                <td width="13%" class="TDLeft">
                                    Fax No
                                </td>
                                <td width="20%" class="TDRight">
                                    <asp:TextBox ID="faxNoTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                                </td>
                                <td width="13%" class="TDLeft">
                                </td>
                                <td width="20%" class="TDRight">
                                </td>
                            </tr>
                            <tr>
                                <td width="13%" class="TDLeft">
                                </td>
                                <td width="20%" class="TDRight">
                                </td>
                                <td width="13%" class="TDLeft">
                                    Remarks
                                </td>
                                <td width="20%" class="TDRight">
                                    <asp:TextBox ID="remarksTextBox" runat="server" CssClass="TextBox" TextMode="MultiLine"></asp:TextBox>
                                </td>
                                <td width="13%" class="TDLeft">
                                    &nbsp;
                                </td>
                                <td width="20%" class="TDRight">
                                </td>
                            </tr>
                            <tr>
                                <td width="13%" class="TDLeft">
                                </td>
                                <td width="20%" class="TDRight">
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
                                </td>
                                <td width="20%" class="TDRight">
                                    &nbsp;
                                </td>
                                <td width="13%" class="TDLeft">
                                </td>
                                <td width="20%" class="TDRight">
                                </td>
                                <td width="13%" class="TDLeft">
                                    &nbsp;
                                </td>
                                <td width="20%" class="TDRight">
                                </td>
                            </tr>
                            <tr>
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
                                    <asp:Button ID="updateButton" runat="server" OnClick="updateButton_Click" Text="Update" />
                                    <asp:Button ID="closeButton" runat="server" OnClick="closeButton_Click" Text="Close" />
                                </td>
                                <td width="13%" class="TDLeft">
                                    &nbsp;
                                </td>
                                <td width="20%" class="TDRight">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
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
                                <td width="13%" class="TDLeft">
                                    &nbsp;
                                </td>
                                <td width="20%" class="TDRight">
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
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    </form>
</body>
</html>
