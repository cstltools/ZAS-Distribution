<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_MIOTargetEntryByProduct, App_Web_htam3fti" %>
<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<style type="text/css">
    /* Outer wrapper like screenshot */
    #gridContainer1 {
        width: 92%;
        margin: 0 auto;
        border: 2px solid #a9a9c9;
        padding: 10px 10px 6px 10px;
        background: #efefef;
    }

    .gridview {
        width: 100%;
        border-collapse: collapse;
        font-family: Arial;
        font-size: 13px;
        background: #fff;
    }

    /* Header */
    .gridview th {
        background: #8b0f3a;
        color: #fff;
        font-weight: 700;
        padding: 7px 10px;
        text-align: center;
        border: 1px solid #6f0c2e;
        white-space: nowrap;
    }

    /* Body */
    .gridview td {
        border: 1px solid #c9c9c9;
        padding: 6px 8px;
        background: #f7f7f7;
        text-align: center;
        vertical-align: middle;
    }

    .gridview tr:nth-child(even) td {
        background: #f2f2f2;
    }

    /* Column widths */
    .w-sl { width: 55px; }
    .w-code { width: 120px; }
    .w-name { width: 360px; }
    .w-pack { width: 110px; }
    .w-qty { width: 140px; }
    .w-amt { width: 170px; }
    .w-check { width: 60px; }

    /* Product Name left align */
    .txt-left { text-align: left !important; padding-left: 10px !important; }

    /* Input boxes */
    .qty-box {
        width: 92%;
        height: 24px;
        border: 2px solid #000;
        padding: 2px 6px;
        background: #fff;
        box-sizing: border-box;
        text-align: right;
        font-weight: 600;
    }

    .amt-box {
        width: 92%;
        height: 24px;
        border: 1px solid #8f8f8f;
        padding: 2px 6px;
        background: #d9d9d9;
        box-sizing: border-box;
        text-align: right;
        font-weight: 700;
    }

    /* Footer TOTAL row like 2nd image */
    .gridview tfoot td {
        background: #e6e6e6 !important;
        font-weight: 800;
        text-transform: uppercase;
        border-top: 2px solid #a9a9c9;
        padding: 8px;
        text-align: center;
    }

    /* footer boxes */
    .footer-box {
        display: block;
        width: 92%;
        margin: 0 auto;
        border: 1px solid #8f8f8f;
        background: #d9d9d9;
        padding: 4px 6px;
        text-align: right;
        font-weight: 800;
        height: 24px;
        line-height: 16px;
        box-sizing: border-box;
    }

    .footer-box-white {
        display: block;
        width: 92%;
        margin: 0 auto;
        border: 2px solid #000;
        background: #fff;
        padding: 4px 6px;
        text-align: right;
        font-weight: 800;
        height: 24px;
        line-height: 16px;
        box-sizing: border-box;
    }

    /* Checkbox align */
    .check-cell { text-align: center; }
</style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div>
            <table width="100%" class="TableWorkArea">
                <tr>
                    <td colspan="6" class="TableHeading">
                        Product Wise Target Entry
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

                <!-- (keep your spacing rows as-is) -->

                <tr>
                    <td class="TDLeft" width="13%"></td>
                    <td class="TDRight" width="20%"></td>
                    <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Group Name:</td>
                    <td class="TDRight" width="20%">
                        <asp:DropDownList ID="groupDropDownList" Width="180px" runat="server" CssClass="DropDown" Enabled="False"></asp:DropDownList>
                    </td>
                    <td class="TDLeft" width="13%"></td>
                    <td class="TDRight" width="20%"></td>
                </tr>

                <tr>
                    <td class="TDLeft" width="13%"></td>
                    <td class="TDRight" width="20%"></td>
                    <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                        Company Name: <span class="star-mark"> * </span>
                    </td>
                    <td class="TDRight" width="20%">
                        <asp:DropDownList ID="companyNameDropDownList" Width="180px" runat="server" CssClass="DropDown"
                            OnSelectedIndexChanged="companyNameDropDownList_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td class="TDLeft" width="13%"></td>
                    <td class="TDRight" width="20%"></td>
                </tr>

                <tr>
                    <td class="TDLeft" width="13%"></td>
                    <td class="TDRight" width="20%"></td>
                    <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                        MIO Name: <span class="star-mark"> * </span>
                    </td>
                    <td class="TDRight" width="20%">
                        <asp:DropDownList ID="mioDropDownList" Width="180px" runat="server" CssClass="DropDown"></asp:DropDownList>
                    </td>
                    <td class="TDLeft" width="13%"></td>
                    <td class="TDRight" width="20%"></td>
                </tr>

                <tr>
                    <td width="13%" class="TDLeft"></td>
                    <td width="20%" class="TDRight">&nbsp;</td>
                    <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                        Period:<span class="star-mark"> * </span>
                    </td>
                    <td width="20%" class="TDRight">
                        <asp:DropDownList ID="periodDropDownList" Width="180px" runat="server" CssClass="DropDown"
                            AutoPostBack="True" OnSelectedIndexChanged="periodDropDownList_OnSelectedIndexChanged">
                            <asp:ListItem>-----Select-----</asp:ListItem>
                            <asp:ListItem>January</asp:ListItem>
                            <asp:ListItem>February</asp:ListItem>
                            <asp:ListItem>March</asp:ListItem>
                            <asp:ListItem>April</asp:ListItem>
                            <asp:ListItem>May</asp:ListItem>
                            <asp:ListItem>June</asp:ListItem>
                            <asp:ListItem>July</asp:ListItem>
                            <asp:ListItem>August</asp:ListItem>
                            <asp:ListItem>September</asp:ListItem>
                            <asp:ListItem>October</asp:ListItem>
                            <asp:ListItem>November</asp:ListItem>
                            <asp:ListItem>December</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td width="13%" class="TDLeft">&nbsp;</td>
                    <td width="20%" class="TDRight"></td>
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
                    <td width="20%" class="TDRight" colspan="6">
                        <div id="gridContainer1" style="height: auto; overflow: auto; width: 85%; margin: 0 auto;">
                            <asp:GridView ID="loadGridView" runat="server"
                                AutoGenerateColumns="False"
                                CssClass="gridview"
                                DataKeyNames="ProductId,AvgUnitPrice"
                                ShowFooter="True"
                                OnRowDataBound="loadGridView_RowDataBound">

                                <Columns>
                                    <asp:TemplateField HeaderText="SL">
                                        <HeaderStyle CssClass="w-sl" />
                                        <ItemStyle CssClass="w-sl" />
                                        <ItemTemplate>
                                            <asp:Label ID="LabelSL" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>TOTAL</FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="ProductCode" HeaderText="Product Code">
                                        <HeaderStyle CssClass="w-code" />
                                        <ItemStyle CssClass="w-code" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="ProductName" HeaderText="Product Name">
                                        <HeaderStyle CssClass="w-name" />
                                        <ItemStyle CssClass="w-name txt-left" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="PackSize" HeaderText="Pack Size">
                                        <HeaderStyle CssClass="w-pack" />
                                        <ItemStyle CssClass="w-pack" />
                                    </asp:BoundField>
                                    
                                    <asp:BoundField DataField="AvgUnitPrice" HeaderText="Unit Price">
                                        <HeaderStyle CssClass="w-pack" />
                                        <ItemStyle CssClass="w-pack" />
                                    </asp:BoundField>

                                    <asp:TemplateField HeaderText="Target Quantity">
                                        <HeaderStyle CssClass="w-qty" />
                                        <ItemStyle CssClass="w-qty" />
                                        <ItemTemplate>
                                            <asp:TextBox ID="returnQtyTextBox"
                                                CssClass="qty-box"
                                                AutoPostBack="True"
                                                OnTextChanged="returnQtyTextBox_TextChanged"
                                                runat="server"></asp:TextBox>

                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtenderconvRate"
                                                runat="server" Enabled="True" TargetControlID="returnQtyTextBox"
                                                FilterType="Custom" ValidChars="0123456789">
                                            </ajaxToolkit:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalQty" CssClass="footer-box-white" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Target Amount">
                                        <HeaderStyle CssClass="w-amt" />
                                        <ItemStyle CssClass="w-amt" />
                                        <ItemTemplate>
                                            <asp:TextBox ID="tbxTargetAmount" ReadOnly="True"
                                                CssClass="amt-box"
                                                runat="server"></asp:TextBox>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalAmount" CssClass="footer-box" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderStyle CssClass="w-check" />
                                        <ItemStyle CssClass="w-check check-cell" />
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkSelectAll" runat="server"
                                                AutoPostBack="True"
                                                OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server"
                                                AutoPostBack="True"
                                                OnCheckedChanged="chkSelect_OnCheckedChanged" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>

                <tr>
                    <td width="13%" class="TDLeft">&nbsp;</td>
                    <td width="20%" class="TDRight">&nbsp;</td>
                    <td width="13%" class="TDLeft">&nbsp;</td>
                    <td width="20%" class="TDRight">&nbsp;</td>
                    <td width="13%" class="TDLeft">&nbsp;</td>
                    <td width="20%" class="TDRight">&nbsp;</td>
                </tr>

                <tr>
                    <td width="13%" class="TDLeft">&nbsp;</td>
                    <td width="20%" class="TDRight">&nbsp;</td>
                    <td width="13%" class="TDLeft">&nbsp;</td>
                    <td width="20%" class="TDRight">
                        <asp:Button ID="submitbutton" runat="server" Text="Submit" onclick="submitButton_Click1" />
                        <asp:Button ID="clearButton" CssClass="clearButton" BackColor="#E8BC31" runat="server"
                            OnClick="clearButton_OnClick" Text="Clear" />
                    </td>
                    <td width="13%" class="TDLeft">&nbsp;</td>
                    <td width="20%" class="TDRight">&nbsp;</td>
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

<asp:UpdateProgress ID="UpdateProgress1" runat="server" ClientIDMode="Static" DisplayAfter="0" DynamicLayout="true">
    <ProgressTemplate>
        <div class="divWaiting">
            <asp:Image ID="imgWait" CssClass="position-set" runat="server" ImageAlign="Middle" ImageUrl="~/images/progress-bar-opt.gif"
                Height="80" Width="80" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>

</asp:Content>
