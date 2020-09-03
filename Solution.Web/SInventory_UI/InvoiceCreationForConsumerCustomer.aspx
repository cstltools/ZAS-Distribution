<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MainMasterPage.master"
    AutoEventWireup="true" CodeFile="InvoiceCreationForConsumerCustomer.aspx.cs"
    Inherits="SInventory_UI_InvoiceCreationForCustomer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            Direct Sales Invoice
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
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Company: <span class="star-mark"> * </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="companyDropDownList" runat="server" AutoPostBack="True" CssClass="DropDown"
                                OnSelectedIndexChanged="companyDropDownList_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Distribution Center: <span class="star-mark"> * </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="dcDropDownList" runat="server" CssClass="DropDown" AutoPostBack="True"
                                OnSelectedIndexChanged="dcDropDownList_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Invoice Date: <span class="star-mark"> * </span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="invDateTextBox" runat="server" CssClass="TextBoxCalander" ReadOnly="True"></asp:TextBox>
                        </td>
                        
                    </tr>
                    <tr>
                        
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Customer Name: <span class="star-mark"> * </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="custNameTextBox" runat="server" AutoPostBack="True" CssClass="TextBox"
                                OnTextChanged="custNameTextBox_TextChanged"></asp:TextBox>
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Customer Phone No: <span class="star-mark"> * </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="custPhoneNoTextBox" runat="server" CssClass="TextBox" AutoPostBack="True"
                                OnTextChanged="custPhoneNoTextBox_OnTextChanged" ReadOnly="False"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="custNameTextBox_AutoCompleteExtender" runat="server"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="10" DelimiterCharacters=""
                                EnableCaching="true" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetConsumerCustomer"
                                ServicePath="SInventoryWebService.asmx" ShowOnlyCurrentWordInCompletionListItem="true"
                                TargetControlID="custPhoneNoTextBox" UseContextKey="True">
                            </asp:AutoCompleteExtender>
                        </td>
                        
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Customer Address:<span class="star-mark"> * </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="custAddressTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Payment Type: <span class="star-mark"> * </span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="payTypeDDL" runat="server" CssClass="DropDown">
                            </asp:DropDownList>
                        </td>
                        <td class="TDLeft" width="13%">
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
                        <td class="TDLeft" width="13%" colspan="6">
                            <div id="gridContainer1" style="height: auto; overflow: auto; width: 97%; margin: 0 auto;">
                                <asp:GridView ID="gridLineItemGridView" runat="server" AutoGenerateColumns="False"
                                    CssClass="gridview">
                                    <Columns>
                                        <asp:TemplateField HeaderText="SL">
                                            <ItemTemplate>
                                                <asp:Label ID="slLabel" runat="server" Text='<%# Eval("SL")%>'></asp:Label> &nbsp; &nbsp;
                                                <asp:ImageButton ID="addImageButton" runat="server" ImageUrl="~/images/lineAdd.png"
                                                    OnClick="addImageButton_Click" /> &nbsp; &nbsp;
                                                <asp:ImageButton ID="removeImageButton" runat="server" ImageUrl="~/images/lineDelete.png"
                                                    OnClick="removeImageButton_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Product Code">
                                            <ItemTemplate>
                                                <asp:TextBox ID="codeTextBox" runat="server" Text='<%# Eval("ProductCode")%>' CssClass="TextBoxMicroMini"
                                                    AutoPostBack="True" OnTextChanged="codeTextBox_TextChanged"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="fcodeTextBox" runat="server" TargetControlID="codeTextBox"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Product Name">
                                            <ItemTemplate>
                                                <asp:TextBox ID="nameTextBox" runat="server" CssClass="TextBox" Text='<%# Eval("ProductName")%>'
                                                    AutoPostBack="True" OnTextChanged="nameTextBox_TextChanged"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="nameTextBox_AutoCompleteExtender" runat="server" DelimiterCharacters=""
                                                    EnableCaching="true" Enabled="True" MinimumPrefixLength="1" CompletionSetCount="10"
                                                    ServiceMethod="GetProductByMenufracturer" ServicePath="SInventoryWebService.asmx" TargetControlID="nameTextBox"
                                                    UseContextKey="True" CompletionListCssClass="autocomplete_completionListElement"
                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    ShowOnlyCurrentWordInCompletionListItem="true">
                                                </asp:AutoCompleteExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CStock">
                                            <ItemTemplate>
                                                <asp:TextBox ID="currentStockTextBox" runat="server" CssClass="TextBoxMicroMini"
                                                    Text='<%# Eval("StockQty")%>' ReadOnly="True"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="fcurrentStockTextBox" runat="server" TargetControlID="currentStockTextBox"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="UP">
                                            <ItemTemplate>
                                                <asp:TextBox ID="unitPriceTextBox" runat="server" CssClass="TextBoxMicroMini" Text='<%# Eval("UnitPrice")%>'
                                                    ReadOnly="True"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="funitPriceTextBox" runat="server" TargetControlID="unitPriceTextBox"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="UVAT">
                                            <ItemTemplate>
                                                <asp:TextBox ID="upVatTextBox" runat="server" CssClass="TextBoxMicroMini" Text='<%# Eval("UnitVAT")%>'
                                                    ReadOnly="True"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="fupVatTextBox" runat="server" TargetControlID="upVatTextBox"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty">
                                            <ItemTemplate>
                                                <asp:TextBox ID="qtyTextBox" runat="server" CssClass="TextBoxMicroMini" Text='<%# Eval("Quantity")%>'
                                                    AutoPostBack="True" OnTextChanged="qtyTextBox_TextChanged"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="fqtyTextBox" runat="server" TargetControlID="qtyTextBox"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TP">
                                            <ItemTemplate>
                                                <asp:TextBox ID="tpTextBox" runat="server" CssClass="TextBoxMicroMini" Text='<%# Eval("TotalPrice")%>'
                                                    ReadOnly="True"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftpTextBox" runat="server" TargetControlID="tpTextBox"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TPVAT">
                                            <ItemTemplate>
                                                <asp:TextBox ID="tpVatTextBox" runat="server" CssClass="TextBoxMicroMini" Text='<%# Eval("VAT")%>'
                                                    ReadOnly="True"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftpVatTextBox" runat="server" TargetControlID="tpVatTextBox"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DP" Visible="False">
                                            <ItemTemplate>
                                                <asp:TextBox ID="dpTextBox" runat="server" CssClass="TextBoxMicroMini" Text='<%# Eval("DiscountPercentage")%>'
                                                    ReadOnly="True"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="fdpTextBox" runat="server" TargetControlID="dpTextBox"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DAmt">
                                            <ItemTemplate>
                                                <asp:TextBox ID="dpAmtTextBox" runat="server" CssClass="TextBoxMicroMini" OnTextChanged="dpAmtTextBox_OnTextChanged"
                                                    AutoPostBack="True" Text='<%# Eval("DiscountAmount")%>'></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="fdpAmtTextBox" runat="server" TargetControlID="dpAmtTextBox"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="NP">
                                            <ItemTemplate>
                                                <asp:TextBox ID="npTextBox" runat="server" CssClass="TextBoxMicroMini" Text='<%# Eval("NetPrice")%>'
                                                    ReadOnly="True"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="fnpTextBox" runat="server" TargetControlID="npTextBox"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BQty">
                                            <ItemTemplate>
                                                <asp:TextBox ID="bQtyTextBox" runat="server" CssClass="TextBoxMicroMini" Text='<%# Eval("BonusQty")%>'
                                                    AutoPostBack="True" OnTextChanged="bQtyTextBox_TextChanged" ReadOnly="True"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="fbQtyTextBox" runat="server" TargetControlID="bQtyTextBox"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TQty">
                                            <ItemTemplate>
                                                <asp:TextBox ID="tQtyTextBox" runat="server" CssClass="TextBoxMicroMini" Text='<%# Eval("TotalQty")%>'
                                                    ReadOnly="True"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftQtyTextBox" runat="server" TargetControlID="tQtyTextBox"
                                                    FilterType="Custom, Numbers" ValidChars="." />
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
                        <td class="TDLeft" width="13%">
                            &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            TP Total:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="tpTptalTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:TextBox>
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
                            <asp:HiddenField ID="orderIdHiddenField" runat="server" />
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            VAT Total:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="vatTotalTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:TextBox>
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
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Discount Total:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="disTotalTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:TextBox>
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
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Grand Total:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="grandTotalTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:TextBox>
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
                            <asp:Button ID="saveButton" runat="server" BackColor="#339933" Text="Invoice Generate" OnClick="saveButton_Click" />
                        </td>
                        <td class="TDLeft" width="13%">
                            
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
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Print InvNo:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="invTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                        </td>
                        <td class="TDLeft" width="13%">
                            <asp:Button ID="printButton" runat="server" OnClick="printButton_Click" Text="Print" />
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            <asp:HiddenField ID="hdCustomerMasterId" runat="server" />
                            <asp:HiddenField ID="hdComUnitId" runat="server" />
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:HiddenField ID="hdMiaId" runat="server" />
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
