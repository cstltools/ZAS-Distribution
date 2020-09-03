<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MainMasterPage.master"
    AutoEventWireup="true" CodeFile="WarehouseStockIn.aspx.cs" Inherits="SInventory_UI_WarehouseStockIn" %>

<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
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
            width: 25% !important;
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
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            Warehouse Stock In
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
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:LinkButton ID="viewLinkButton" CssClass="btn buttons smallBtn" ForeColor="green"
                                OnClick="viewLinkButton_OnClick" runat="server">View Details</asp:LinkButton>
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
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
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Company Name: <span class="star-mark">* </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="companyNameDropDownList" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="companyNameDropDownList_OnSelectedIndexChanged" CssClass="DropDown">
                            </asp:DropDownList>
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Warehouse: <span style="color: red">*</span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="warehouseDropDownList" Width="175px" Height="23px" runat="server"
                                CssClass="DropDown">
                            </asp:DropDownList>
                            <asp:HiddenField ID="stockInIdHiddenField" runat="server" />
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Stock In Date: <span style="color: red">*</span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="stockInDateTextBox" Width="150px" runat="server" CssClass="TextBoxCalander"
                                AutoPostBack="True" OnTextChanged="stockInDateTextBox_OnTextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" AlternateText="Click to show calendar" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                TabIndex="4" ID="imgDate"></asp:ImageButton>
                            <cc1:CalendarExtender ID="manufacturerDate1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="stockInDateTextBox" PopupButtonID="imgDate">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Challan No: <span style="color: red">*</span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="challanNoTextBox" Height="23px" runat="server" CssClass="TextBox"></asp:TextBox>
                        </td>
                        <td width="13%" style="text-align: right; padding-right: 10px;" class="TDLeft">
                            Challan Date: <span style="color: red">*</span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="challanDateTextBox" Width="150px" runat="server" CssClass="TextBoxCalander"
                                AutoPostBack="True" OnTextChanged="challanDateTextBox_OnTextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" AlternateText="Click to show calendar" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                TabIndex="4" ID="ImageButton4"></asp:ImageButton>
                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="challanDateTextBox" PopupButtonID="ImageButton4">
                            </cc1:CalendarExtender>
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;" class="TDLeft">
                            Reference No:&nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="referenceNoTextBox" Height="23px" CssClass="TextBox" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Reference Date:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="referenceDateTextBox" Width="150px" Height="23px" runat="server"
                                CssClass="TextBoxCalander" AutoPostBack="True" OnTextChanged="referenceDateTextBox_OnTextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" AlternateText="Click to show calendar" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                TabIndex="4" ID="ImageButton5"></asp:ImageButton>
                            <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="referenceDateTextBox" PopupButtonID="ImageButton5">
                            </cc1:CalendarExtender>
                        </td>
                        <td width="13%" style="text-align: right; padding-right: 10px;" class="TDLeft">
                            Remarks:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="remarksTextBox" Height="20px" TextMode="MultiLine" Rows="2" Width="170px"
                                runat="server"></asp:TextBox>
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Supplier: <span style="color: red">*</span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="ddlSupplier" Width="175px" Height="23px" runat="server"
                                CssClass="DropDown">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                            &nbsp;
                        </td>
                        <td width="13%" style="text-align: right; padding-right: 10px;" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
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
                    <tr runat="server" id="grid" visible="false">
                        <td class="TDRight" width="20%" colspan="6">
                            <div id="gridContainer1" style="height: auto; overflow: auto; width: 97%; margin: 0 auto;">
                                <asp:GridView ID="productGridView" runat="server" AutoGenerateColumns="False" CssClass="gridview">
                                    <Columns>
                                        <asp:TemplateField HeaderText="SL">
                                            <ItemTemplate>
                                                <asp:Label ID="LabelSL" Text='<%# Container.DataItemIndex + 1 %>' runat="server"
                                                    Width="20px"></asp:Label>
                                                <asp:HiddenField ID="productidHiddenField" Value='<%# Eval("ProductId")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Product Code">
                                            <ItemTemplate>
                                                <asp:TextBox ID="productCodeTextBox" runat="server" CssClass="TextBoxCalander" Width="100px"
                                                    AutoPostBack="True" ToolTip="true" OnTextChanged="productCodeTextBox_TextChanged"
                                                    Text='<%# Eval("ProductCode")%>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Product Name">
                                            <ItemTemplate>
                                                <asp:TextBox ID="productNameTextBox" runat="server" CssClass="TextBoxCalander" Text='<%# Eval("ProductName")%>'
                                                    AutoPostBack="True" ToolTip="true" OnTextChanged="productNameTextBox_TextChanged"></asp:TextBox>
                                                <cc1:AutoCompleteExtender ID="productNameTextBox_AutoCompleteExtender" runat="server"
                                                    DelimiterCharacters="" EnableCaching="true" Enabled="True" MinimumPrefixLength="1"
                                                    CompletionSetCount="10" ServiceMethod="GetProduct" ServicePath="SInventoryWebService.asmx"
                                                    TargetControlID="productNameTextBox" UseContextKey="True" CompletionListCssClass="autocomplete_completionListElement"
                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    ShowOnlyCurrentWordInCompletionListItem="true">
                                                </cc1:AutoCompleteExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PackSize">
                                            <ItemTemplate>
                                                <asp:TextBox ID="packSizeTextBox" CssClass="TextBoxCalander" runat="server" Width="60px"
                                                    ReadOnly="True" Text='<%# Eval("PackSize")%>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Batch">
                                            <ItemTemplate>
                                                <asp:TextBox ID="batchTextBox" CssClass="TextBoxCalander" runat="server" Width="50px"
                                                    Text='<%# Eval("Batch")%>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Mfg.Date">
                                            <ItemTemplate>
                                                <asp:TextBox ID="mfgDateTextBox" runat="server" Text='<%# Eval("MfgDate")%>' CssClass="TextBoxCalander"
                                                    AutoPostBack="True" OnTextChanged="mfgDateTextBox_OnTextChanged" DataFormatString="{0:dd-MMM-yyyy}"></asp:TextBox>
                                                <asp:ImageButton runat="server" AlternateText="Click to show calendar" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                    TabIndex="4" ID="ImageButton15"></asp:ImageButton>
                                                <cc1:CalendarExtender ID="manufacturerDate" runat="server" Format="dd-MMM-yyyy" TargetControlID="mfgDateTextBox"
                                                    PopupButtonID="ImageButton15">
                                                </cc1:CalendarExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Exp.Date">
                                            <ItemTemplate>
                                                <asp:TextBox ID="expDateDateTextBox" runat="server" Text='<%# Eval("ExpDate")%>'
                                                    CssClass="TextBoxCalander" AutoPostBack="True" OnTextChanged="expDateDateTextBox_OnTextChanged"
                                                    DataFormatString="{0:dd-MMM-yyyy}"></asp:TextBox>
                                                <asp:ImageButton runat="server" AlternateText="Click to show calendar" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                    TabIndex="4" ID="ImageButton25"></asp:ImageButton>
                                                <cc1:CalendarExtender ID="ExpDate" runat="server" PopupButtonID="ImageButton25" Format="dd-MMM-yyyy"
                                                    TargetControlID="expDateDateTextBox">
                                                </cc1:CalendarExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty">
                                            <ItemTemplate>
                                                <asp:TextBox ID="reqQtyTextBox" runat="server" CssClass="TextBoxCalander" Text='<%# Eval("Quantity")%>'
                                                    AutoPostBack="True" OnTextChanged="reqQtyTextBox_OnTextChanged"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderQuantity" runat="server"
                                                    Enabled="True" TargetControlID="reqQtyTextBox" FilterType="Custom" ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Price">
                                            <ItemTemplate>
                                                <asp:TextBox ID="costPriceTextBox" runat="server" Text='<%# Eval("Price")%>' CssClass="TextBoxCalander"
                                                    AutoPostBack="True" OnTextChanged="costPriceTextBox_OnTextChanged"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderPrice" runat="server" Enabled="True"
                                                    TargetControlID="costPriceTextBox" FilterType="Custom" ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Vat (Per Unit)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="vatTextBox" runat="server" CssClass="TextBoxCalander" Text='<%# Eval("Vat")%>'
                                                    AutoPostBack="True" OnTextChanged="vatTextBox_OnTextChanged"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderVat" runat="server" Enabled="True"
                                                    TargetControlID="vatTextBox" FilterType="Custom" ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Amt">
                                            <ItemTemplate>
                                                <asp:TextBox ID="totalValueTextBox" runat="server" CssClass="TextBoxCalander" Text='<%# Eval("TotalAmount")%>'
                                                    ReadOnly="True" AutoPostBack="True" OnTextChanged="totalValueTextBox_OnTextChanged"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/lineAdd.png"
                                                    OnClick="ImageButton1_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/images/lineDelete.png"
                                                    OnClick="ImageButton2_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
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
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" style="text-align: right; padding-right: 10px;" class="TDLeft">
                            Total Qty :
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="totalQtyTextBox" Height="23px" runat="server" class="TextBox" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;
                        </td>
                        <td class="TDRight" style="text-align: right; padding-right: 10px;" width="20%">
                            &nbsp;
                        </td>
                        <td class="TDLeft" style="text-align: right; padding-right: 10px;" width="13%">
                            Total Vat:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="totalVatTextBox" Height="23px" ReadOnly="True" class="TextBox" runat="server"></asp:TextBox>
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
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" style="text-align: right; padding-right: 10px;" class="TDLeft">
                            Grand Total:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="grandTotalTextBox" Height="23px" runat="server" class="TextBox"
                                ReadOnly="True"></asp:TextBox>
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
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
                            <asp:Button ID="submitButton" CssClass="button-padding-right" runat="server" OnClick="submitButton_Click"
                                OnClientClick="return confirm('Are you sure you want to Save ?');" Text="Submit" />
                            <asp:Button ID="cancelButton" BackColor="#cc6600" runat="server" OnClick="cancelButton_Click"
                                Text="Cancel" />
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
