<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_NewStockTransferDcToDc, App_Web_3bs1ezh0" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <style type="text/css">
        /*AutoComplete flyout */
        .autocomplete_completionListElement
        {
            margin: 0px !important;
            background-color: White;
            color: windowtext;
            border: buttonshadow;
            border-width: 1px;
            border-style: solid;
            cursor: default;
            overflow: auto;
            font-family: Calibri;
            font-size: 12px;
            text-align: left;
            list-style-type: none;
            margin-left: 0px;
            padding-left: 0px;
            max-height: 350px;
            width: 40% !important;
        }
        
        /* AutoComplete highlighted item */
        
        .autocomplete_highlightedListItem
        {
            background-color: yellow;
            color: black;
            padding: 1px;
        }
        
        /* AutoComplete item */
        
        .autocomplete_listItem
        {
            background-color: white;
            color: blue;
            padding: 0px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            Stock Transfer DC to DC
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
                        <td width="20%" class="TDRight">
                            <span style="font-size: 16px; font-weight: bold;">From</span>
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                            <span style="font-size: 16px; font-weight: bold;">To</span>
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
                        </td>
                        <td class="TDRight" width="20%">
                            Chalan Date
                        </td>
                        <td class="TDLeft" width="13%">
                            <asp:TextBox ID="chalanDateTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td class="TDRight" width="20%">
                            ChalanNo
                        </td>
                        <td class="TDLeft" width="13%">
                            <asp:TextBox ID="chalanNoTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:TextBox>
                            &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                            Company:
                        </td>
                        <td width="13%" class="TDLeft">
                            <asp:DropDownList ID="ddlFromCmpany" runat="server" AutoPostBack="True" CssClass="DropDown"
                                OnSelectedIndexChanged="ddlFromCmpany_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td width="20%" class="TDRight">
                            Company:
                        </td>
                        <td width="13%" class="TDLeft">
                            <asp:DropDownList ID="ddlToCompany" runat="server" AutoPostBack="True" CssClass="DropDown"
                                OnSelectedIndexChanged="ddlToCompany_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                            DC (From)
                        </td>
                        <td width="13%" class="TDLeft">
                            <asp:DropDownList ID="salescenterDropDownList1" runat="server" AutoPostBack="True"
                                CssClass="DropDown" OnSelectedIndexChanged="salescenterDropDownList1_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td width="20%" class="TDRight">
                            DC (To)
                        </td>
                        <td width="13%" class="TDLeft">
                            <asp:DropDownList ID="salescenterDropDownList2" runat="server" AutoPostBack="True"
                                CssClass="DropDown" OnSelectedIndexChanged="salescenterDropDownList2_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                            From Sales Center Code
                        </td>
                        <td width="13%" class="TDLeft">
                            <asp:TextBox ID="fromComUnitCodeTextBox" runat="server" AutoPostBack="True" CssClass="TextBox"
                                OnTextChanged="fromComUnitCodeTextBox_TextChanged"></asp:TextBox>
                        </td>
                        <td width="20%" class="TDRight">
                            To Sales Center Code
                        </td>
                        <td width="13%" class="TDLeft">
                            <asp:TextBox ID="toComUnitCodeTextBox" runat="server" CssClass="TextBox" OnTextChanged="toComUnitCodeTextBox_TextChanged"
                                AutoPostBack="True"></asp:TextBox>
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                            Name
                        </td>
                        <td width="13%" class="TDLeft">
                            <asp:TextBox ID="fromComUnitNameTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td width="20%" class="TDRight">
                            Name
                        </td>
                        <td width="13%" class="TDLeft">
                            <asp:TextBox ID="toComUnitNameTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                            Address
                        </td>
                        <td class="TDLeft" width="13%">
                            <asp:TextBox ID="fromComUnitAddressTextBox" runat="server" CssClass="TextBox" ReadOnly="True"
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                        <td class="TDRight" width="20%">
                            Address
                        </td>
                        <td class="TDLeft" width="13%">
                            <asp:TextBox ID="toComUnitAddressTextBox" runat="server" CssClass="TextBox" TextMode="MultiLine"></asp:TextBox>
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
                            Truck Number
                        </td>
                        <td class="TDLeft" width="13%">
                            <asp:TextBox ID="truckNoTextBox" runat="server" CssClass="TextBox" Height="21px"></asp:TextBox>
                        </td>
                        <td class="TDRight" width="20%">
                            Driver Name
                        </td>
                        <td class="TDLeft" width="13%">
                            <asp:TextBox ID="driverNameTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
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
                            Manufacturer
                        </td>
                        <td class="TDLeft" width="13%">
                            <asp:DropDownList ID="manufacDropDownList" runat="server" CssClass="DropDown">
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
                            Product Code
                        </td>
                        <td class="TDLeft" width="13%">
                            <asp:TextBox ID="prodctCodeTextBox" runat="server" CssClass="TextBox" AutoPostBack="True" Height="21px" OnTextChanged="productNameTextBox_TextChanged"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="productNameTextBox_AutoCompleteExtender" runat="server"
                                DelimiterCharacters="" EnableCaching="true" Enabled="True" MinimumPrefixLength="1"
                                CompletionSetCount="10" ServiceMethod="GetProductByMenufracturer" ServicePath="SInventoryWebService.asmx"
                                TargetControlID="prodctCodeTextBox" CompletionListCssClass="autocomplete_completionListElement"
                                CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                ShowOnlyCurrentWordInCompletionListItem="true">
                            </asp:AutoCompleteExtender>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:Button ID="Button1" runat="server" Text="Search" OnClick="Button1_Click" />
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
                        <td class="TDLeft" width="13%" colspan="6">
                            <asp:GridView ID="productGridView" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                                DataKeyNames="DCStoreId,VATAmountPerUnit,UnitPrice">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="PCode" HeaderText="Product Code" />
                                    <asp:BoundField DataField="PName" HeaderText="Product Name" />
                                    <asp:BoundField DataField="StockQty" HeaderText="Stock Qty" />
                                    <asp:BoundField DataField="BatchNo" HeaderText="Batch No" />
                                    <asp:BoundField DataField="ExpDate" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="ExpDate" />
                                    <asp:BoundField DataField="ReceiveDate" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="ReceiveDate" />
                                    <asp:TemplateField HeaderText="Transfer Qty">
                                        <ItemTemplate>
                                            <asp:TextBox ID="transferQtyTextBox" runat="server" CssClass="TextBox" Height="21px"
                                                OnTextChanged="dQtyTextBox_TextChanged" AutoPostBack="True"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderconvRate" runat="server"
                                                Enabled="True" TargetControlID="transferQtyTextBox" FilterType="Custom" ValidChars="0123456789">
                                            </asp:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
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
                            <asp:Button ID="addButton" runat="server" Text="Add" OnClick="addButton_Click" />
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
                        <td class="TDLeft" colspan="6">
                            <asp:GridView ID="chalanGridView" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                                DataKeyNames="DCStoreId,VATAmountPerUnit,UnitPrice">
                                <Columns>
                                    <asp:BoundField DataField="ProductCode" HeaderText="Product Code" />
                                    <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                    <asp:BoundField DataField="TransferQty" HeaderText="Transfer Qty" />
                                    <asp:BoundField DataField="BatchNo" HeaderText="Batch No" />
                                    <asp:BoundField DataField="ExpDate" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="ExpDate" />
                                    <asp:TemplateField HeaderText="Remove Item">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="DeleteImageButton" runat="server" ImageUrl="~/images/lineDelete.png"
                                                OnClick="DeleteImageButton_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
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
                        <td class="TDRight" width="20%" colspan="2">
                            Taka :&nbsp;&nbsp;
                            <asp:Label ID="grandTotalWordLabel" runat="server"></asp:Label>
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
                            <asp:Button ID="submitButton" runat="server" OnClick="submitButton_Click1" Text="Submit" />
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
                            Chalan No
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="printChalanNoTextBox" runat="server" AutoPostBack="True" CssClass="TextBox"
                                OnTextChanged="fromComUnitCodeTextBox_TextChanged"></asp:TextBox>
                        </td>
                        <td class="TDLeft" width="13%">
                            <asp:Button ID="Button2" runat="server" BackColor="#660033" OnClick="Button2_Click"
                                Text="Print" />
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
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" ClientIDMode="Static" DisplayAfter="0"
        DynamicLayout="true">
        <ProgressTemplate>
            <div class="divWaiting">
                <asp:Image ID="imgWait" CssClass="position-set" runat="server" ImageAlign="Middle"
                    ImageUrl="~/images/progress-bar-opt.gif" Height="80" Width="80" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
