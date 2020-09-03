<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_CustMasterEntry, App_Web_uja4wjh3" %>

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
                        <td width="13%" class="TDLeft"> View List</td>
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
                        <td width="13%" class="TDLeft">
                            &nbsp;</td>
                        <td width="20%" class="TDRight">
                            &nbsp;</td>
                        <td width="13%" class="TDLeft">
                            Customer Code</td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="customerCodeTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;</td>
                        <td width="20%" class="TDRight">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%"></td>
                        <td class="TDRight" width="20%"></td>
                        <td class="TDLeft" width="13%">
                            Customer Name</td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="customernameTextBox" runat="server" CssClass="TextBox"></asp:TextBox></td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%"></td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft">Address</td>
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
                    <tr>
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
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            FE Person Name</td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="feNameTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
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
                            DZSM Person Name</td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="dzsmNameTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                        <td class="TDLeft" width="13%">DZSM&nbsp; Name</td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="regionNameDropDownList" runat="server" 
                                AutoPostBack="True" CssClass="DropDown" 
                                onselectedindexchanged="regionNameDropDownList_SelectedIndexChanged"></asp:DropDownList></td>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                       <td class="TDLeft" width="13%">
                            Distribution Center</td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="comUnitNameDropDownList" runat="server" 
                                CssClass="DropDown" AutoPostBack="True" 
                                onselectedindexchanged="comUnitNameDropDownList_SelectedIndexChanged">
                            </asp:DropDownList></td>
                       <td class="TDLeft" width="13%">&nbsp;</td>
                       <td class="TDRight" width="20%">&nbsp;</td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">&nbsp;</td>
                        <td width="20%" class="TDRight">&nbsp;</td>
                        <td width="13%" class="TDLeft" >
                            FE Name</td>
                            <td width="20%" class="TDRight">
                                <asp:DropDownList ID="districtNameDropDownList" runat="server" 
                                    CssClass="DropDown" AutoPostBack="True" 
                                    onselectedindexchanged="districtNameDropDownList_SelectedIndexChanged">
                                </asp:DropDownList></td>
                        <td width="13%" class="TDLeft">&nbsp;</td>
                        <td width="20%" class="TDRight">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                        <td class="TDLeft" width="13%">
                            Territory Name</td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="areaNameDropDownList" runat="server" AutoPostBack="True" 
                                CssClass="DropDown" 
                                onselectedindexchanged="areaNameDropDownList_SelectedIndexChanged">
                            </asp:DropDownList></td>                            
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                        <td class="TDLeft" width="13%">MIO Name</td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="miaNameDropDownList" runat="server" AutoPostBack="True" 
                                CssClass="DropDown" 
                                onselectedindexchanged="miaNameDropDownList_SelectedIndexChanged">
                            </asp:DropDownList></td>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                   </tr>
                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                        <td class="TDLeft" width="13%">
                            Market Name</td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="marketNameDropDownList" runat="server" CssClass="DropDown" 
                                AutoPostBack="True">
                            </asp:DropDownList></td>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                        <td class="TDLeft" width="13%">
                            Category Name</td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="categoryNameDropDownList" runat="server" 
                                CssClass="DropDown" AutoPostBack="True">
                            </asp:DropDownList></td>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                        <td class="TDLeft" width="13%">
                            Payment Type</td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="paymentTypeDropDownList" runat="server" 
                                AutoPostBack="True" CssClass="DropDown">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem>Cash</asp:ListItem>
                                <asp:ListItem>Credit</asp:ListItem>
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

</asp:Content>

