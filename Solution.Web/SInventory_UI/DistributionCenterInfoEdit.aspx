<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DistributionCenterInfoEdit.aspx.cs" Inherits="SInventory_UI_DistributionCenterInfoEdit" %>

<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

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
                                    <td colspan="6" class="TableHeading">Distribution Center Edit</td>
                                </tr>
                                <tr>
                                    <td class="TDLeft" width="13%">&nbsp;
                            <asp:HiddenField ID="salesCenterInfoIdHiddenField" runat="server" />
                                        <asp:HiddenField ID="nameHiddenField" runat="server" />
                                        <asp:HiddenField ID="shortNameHiddenField" runat="server" />
                                    </td>
                                    <td class="TDRight" width="20%"></td>
                                    <td class="TDLeft" width="13%"></td>
                                    <td class="TDRight" width="20%">
                                        <asp:Label ID="MessageLabel" runat="server" ForeColor="Black"></asp:Label>
                                    </td>
                                    <td class="TDLeft" width="13%"></td>
                                    <td class="TDRight" width="20%"></td>
                                </tr>

                                <tr>
                                    <td width="13%" class="TDLeft"></td>
                                    <td width="20%" class="TDRight"></td>
                                    <td width="13%" class="TDLeft">Company Name <span style="color: red">* </span></td>
                                    <td width="20%" class="TDRight">
                                        <asp:DropDownList ID="companyNameDropDownList" runat="server"
                                            CssClass="DropDown" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                    <td width="13%" class="TDLeft"></td>
                                    <td width="20%" class="TDRight"></td>
                                </tr>
                                <tr>
                                    <td class="TDLeft" width="13%"></td>
                                    <td class="TDRight" width="20%"></td>
                                    <td class="TDLeft" width="13%">Distribution Type: <span class="star-mark">* </span></td>
                                    <td class="TDRight" width="20%">
                                        <asp:DropDownList ID="ddlDcType" runat="server"
                                            CssClass="DropDown">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="TDLeft" width="13%"></td>
                                    <td class="TDRight" width="20%"></td>
                                </tr>

                                <tr>
                                    <td class="TDLeft" width="13%"></td>
                                    <td class="TDRight" width="20%"></td>
                                    <td class="TDLeft" width="13%">Distribution Center Name <span style="color: red">* </span></td>
                                    <td class="TDRight" width="20%">
                                        <asp:TextBox ID="salesCenternameTextBox" runat="server" CssClass="TextBox" AutoPostBack="true" OnTextChanged="salesCenternameTextBox_OnTextChanged"></asp:TextBox>
                                    </td>
                                    <td class="TDLeft" width="13%"></td>
                                    <td class="TDRight" width="20%"></td>
                                </tr>

                                <tr>
                                    <td width="13%" class="TDLeft">&nbsp;</td>
                                    <td width="20%" class="TDRight">&nbsp;</td>
                                    <td width="13%" class="TDLeft">DC Short Name</td>
                                    <td width="20%" class="TDRight">
                                        <asp:TextBox ID="dcShortNameTextBox" runat="server" CssClass="TextBox" AutoPostBack="True" OnTextChanged="dcShortNameTextBox_OnTextChanged"></asp:TextBox>
                                    </td>
                                    <td width="13%" class="TDLeft">&nbsp;</td>
                                    <td width="20%" class="TDRight">&nbsp;</td>
                                </tr>

                                <tr>
                                    <td class="TDLeft" width="13%"></td>
                                    <td class="TDRight" width="20%">&nbsp;
                                    </td>
                                    <td class="TDLeft" width="13%">Address <span style="color: red">* </span></td>
                                    <td class="TDRight" width="20%">
                                        <asp:TextBox ID="addressTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                                    </td>
                                    <td class="TDLeft" width="13%">&nbsp;
                                    </td>
                                    <td class="TDRight" width="20%"></td>
                                </tr>
                                <tr>
                                    <td class="TDLeft" width="13%">&nbsp;
                                    </td>
                                    <td class="TDRight" width="20%">&nbsp;
                                    </td>
                                    <td class="TDLeft" width="13%">Phone No</td>
                                    <td class="TDRight" width="20%">
                                        <asp:TextBox ID="phoneNoTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                            Enabled="True" TargetControlID="phoneNoTextBox" FilterType="Custom" ValidChars="0123456789">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td class="TDLeft" width="13%">&nbsp;
                                    </td>
                                    <td class="TDRight" width="20%">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td width="13%" class="TDLeft">&nbsp;
                                    </td>
                                    <td width="20%" class="TDRight"></td>
                                    <td width="13%" class="TDLeft">Mobile No</td>
                                    <td width="20%" class="TDRight">
                                        <asp:TextBox ID="mobileNoTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderqty" runat="server"
                                            Enabled="True" TargetControlID="mobileNoTextBox" FilterType="Custom" ValidChars="0123456789+">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td width="13%" class="TDLeft"></td>
                                    <td width="20%" class="TDRight"></td>
                                </tr>
                                <tr>
                                    <td width="13%" class="TDLeft"></td>
                                    <td width="20%" class="TDRight"></td>
                                    <td width="13%" class="TDLeft">Fax No</td>
                                    <td width="20%" class="TDRight">
                                        <asp:TextBox ID="faxNoTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                            Enabled="True" TargetControlID="faxNoTextBox" FilterType="Custom" ValidChars="0123456789+">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td width="13%" class="TDLeft">&nbsp;
                                    </td>
                                    <td width="20%" class="TDRight"></td>
                                </tr>

                                <tr>
                                    <td class="TDLeft" width="13%">&nbsp;</td>
                                    <td class="TDRight" width="20%">&nbsp;</td>
                                    <td class="TDLeft" width="13%" >Cash-GL Account:</td>
                                    <td class="TDRight" width="20%">
                                        <asp:DropDownList ID="ddlCashAccount" runat="server" CssClass="DropDown"></asp:DropDownList>
                                    </td>
                                    <td class="TDLeft" width="13%">&nbsp;</td>
                                    <td class="TDRight" width="20%">&nbsp;</td>
                                </tr>

                                <tr>
                                    <td width="13%" class="TDLeft">&nbsp;
                                    </td>
                                    <td width="20%" class="TDRight">&nbsp;
                                    </td>
                                    <td width="13%" class="TDLeft">&nbsp;
                                    </td>
                                    <td width="20%" class="TDRight">&nbsp;
                                    </td>
                                    <td width="13%" class="TDLeft">&nbsp;
                                    </td>
                                    <td width="20%" class="TDRight">&nbsp;
                                    </td>
                                </tr>

                                <tr>
                                    <td width="13%" class="TDLeft">&nbsp;
                                    </td>
                                    <td width="20%" class="TDRight">&nbsp;
                                    </td>
                                    <td width="13%" class="TDLeft">&nbsp;
                                    </td>
                                    <td width="20%" class="TDRight">
                                        <asp:Button ID="updateButton" runat="server" OnClick="updateButton_Click" CssClass="button-margin-right"
                                            Text="Update" />
                                        <asp:Button ID="closeButton" runat="server" OnClick="closeButton_Click"
                                            Text="Close" />
                                    </td>
                                    <td width="13%" class="TDLeft">&nbsp;
                                    </td>
                                    <td width="20%" class="TDRight">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td width="13%" class="TDLeft">&nbsp;
                                    </td>
                                    <td width="20%" class="TDRight">&nbsp;
                                    </td>
                                    <td width="13%" class="TDLeft">&nbsp;
                                    </td>
                                    <td width="20%" class="TDRight">&nbsp;
                                    </td>
                                    <td width="13%" class="TDLeft">&nbsp;
                                    </td>
                                    <td width="20%" class="TDRight">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TDLeft" width="13%">&nbsp;</td>
                                    <td class="TDRight" width="20%">&nbsp;</td>
                                    <td class="TDLeft" width="13%">&nbsp;</td>
                                    <td class="TDRight" width="20%">&nbsp;</td>
                                    <td class="TDLeft" width="13%">&nbsp;</td>
                                    <td class="TDRight" width="20%">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="TDLeft" width="13%">&nbsp;</td>
                                    <td class="TDRight" width="20%">&nbsp;</td>
                                    <td class="TDLeft" width="13%">&nbsp;</td>
                                    <td class="TDRight" width="20%">&nbsp;</td>
                                    <td class="TDLeft" width="13%">&nbsp;</td>
                                    <td class="TDRight" width="20%">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="TDLeft" width="13%">&nbsp;</td>
                                    <td class="TDRight" width="20%">&nbsp;</td>
                                    <td class="TDLeft" width="13%">&nbsp;</td>
                                    <td class="TDRight" width="20%">&nbsp;</td>
                                    <td class="TDLeft" width="13%">&nbsp;</td>
                                    <td class="TDRight" width="20%">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="TDLeft" width="13%">&nbsp;</td>
                                    <td class="TDRight" width="20%">&nbsp;</td>
                                    <td class="TDLeft" width="13%">&nbsp;</td>
                                    <td class="TDRight" width="20%">&nbsp;</td>
                                    <td class="TDLeft" width="13%">&nbsp;</td>
                                    <td class="TDRight" width="20%">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="TDLeft" width="13%">&nbsp;</td>
                                    <td class="TDRight" width="20%">&nbsp;</td>
                                    <td class="TDLeft" width="13%">&nbsp;</td>
                                    <td class="TDRight" width="20%">&nbsp;</td>
                                    <td class="TDLeft" width="13%">&nbsp;</td>
                                    <td class="TDRight" width="20%">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="TDLeft" width="13%">&nbsp;</td>
                                    <td class="TDRight" width="20%">&nbsp;</td>
                                    <td class="TDLeft" width="13%">&nbsp;</td>
                                    <td class="TDRight" width="20%">&nbsp;</td>
                                    <td class="TDLeft" width="13%">&nbsp;</td>
                                    <td class="TDRight" width="20%">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="TDLeft" width="13%">&nbsp;</td>
                                    <td class="TDRight" width="20%">&nbsp;</td>
                                    <td class="TDLeft" width="13%">&nbsp;</td>
                                    <td class="TDRight" width="20%">&nbsp;</td>
                                    <td class="TDLeft" width="13%">&nbsp;</td>
                                    <td class="TDRight" width="20%">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="TDLeft" width="13%">&nbsp;</td>
                                    <td class="TDRight" width="20%">&nbsp;</td>
                                    <td class="TDLeft" width="13%">&nbsp;</td>
                                    <td class="TDRight" width="20%">&nbsp;</td>
                                    <td class="TDLeft" width="13%">&nbsp;</td>
                                    <td class="TDRight" width="20%">&nbsp;</td>
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
