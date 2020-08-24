<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MainMasterPage.master"
    AutoEventWireup="true" CodeFile="DelivaryInvoiceCreationForCustomerByOrderAuto.aspx.cs"
    Inherits="SInventory_UI_DelivaryInvoiceCreationForCustomerByOrderAuto" %>

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
                            Delivery Invoice Confirmation
                        </td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            
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
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Customer Code:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="custCodeTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Customer Name:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="custNameTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                            <%--<asp:AutoCompleteExtender ID="custNameTextBox_AutoCompleteExtender" runat="server"
                                DelimiterCharacters="" EnableCaching="true" Enabled="True" MinimumPrefixLength="1"
                                CompletionSetCount="10" ServiceMethod="GetCustomer" ServicePath="SInventoryWebService.asmx"
                                TargetControlID="custNameTextBox" UseContextKey="True" CompletionListCssClass="autocomplete_completionListElement"
                                CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                ShowOnlyCurrentWordInCompletionListItem="true">
                            </asp:AutoCompleteExtender>--%>
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Customer Address:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="custAddressTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Salse Center:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="comUnitNameTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            MIO Code:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="miaCodeTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            MIO Name:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="miaNameTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Territory Name:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="areaNameTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Merket Name:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="marketNameTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                        </td>
                        
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Customer Category:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="custCategoryTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Order No:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="orderNoTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Order Date:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="orderDateTextBox" runat="server" CssClass="TextBoxCalander" ReadOnly="True"></asp:Label>
                           <%-- <asp:ImageButton ID="imgorderDate" runat="server" AlternateText="Click to show calendar"
                                ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="4" />--%>
                        </td>
                        
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Invoice Date:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="invDateTextBox" runat="server" CssClass="TextBoxCalander" ReadOnly="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Delivery Person Mob.No:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="deliverypersonMobileTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Delivery Person Name:
                        </td>
                        <td width="20%" class="TDRight">
                            <%--<asp:CalendarExtender ID="orderDate" runat="server" Format="dd-MMM-yyyy" TargetControlID="orderDateTextBox"
                                PopupButtonID="imgorderDate">
                            </asp:CalendarExtender>--%>
                            <asp:Label ID="deliverypersonNameTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                        </td>
                        
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Remarks:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="remarksTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                        </td>
                    </tr>
                    <tr runat="server" id="asdfs">
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
                        <td id="Td1" width="13%" class="TDLeft">
                        </td>
                        <td id="Td2" width="20%" class="TDRight">
                        </td>
                        <td width="13%" class="TDLeft" runat="server" visible="False">
                            Payment Type
                        </td>
                        <td width="20%" class="TDRight" runat="server" visible="False">
                            <asp:DropDownList ID="payTypeDDL" runat="server" CssClass="DropDown">
                            </asp:DropDownList>
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
                            <asp:HiddenField ID="invoiceNoHiddenField" runat="server" />
                            <asp:HiddenField ID="invoiceHiddenField" runat="server" />
                            <asp:HiddenField ID="orderIdHiddenField" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%" colspan="6">
                            <div id="gridContainer1" style="height: auto; overflow: auto; width: 97%; margin: 0 auto;">
                                <asp:GridView ID="gridLineItemGridView" runat="server" AutoGenerateColumns="False"
                                    CssClass="gridview" DataKeyNames="InvoiceDetailId,DCStoreId">
                                    <Columns>
                                        <asp:TemplateField HeaderText="SL" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="slLabel" runat="server" Text='<%# Eval("SL")%>'></asp:Label>
                                                <asp:ImageButton ID="addImageButton" runat="server" ImageUrl="~/images/lineAdd.png"
                                                    OnClick="addImageButton_Click" />
                                                <asp:ImageButton ID="removeImageButton" runat="server" ImageUrl="~/images/lineDelete.png"
                                                    OnClick="removeImageButton_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Code">
                                            <ItemTemplate>
                                                <asp:TextBox ID="codeTextBox" runat="server" Text='<%# Eval("ProductCode")%>' CssClass="TextBoxMicroMini"
                                                    AutoPostBack="True" OnTextChanged="codeTextBox_TextChanged"></asp:TextBox>
                                                <asp:HiddenField ID="orderdetailIdHiddenField" runat="server" Value='<%# Eval("OrderDetailsId")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name">
                                            <ItemTemplate>
                                                <asp:TextBox ID="nameTextBox" runat="server" CssClass="TextBox" Text='<%# Eval("ProductName")%>'
                                                    AutoPostBack="True" OnTextChanged="nameTextBox_TextChanged"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="nameTextBox_AutoCompleteExtender" runat="server" DelimiterCharacters=""
                                                    EnableCaching="true" Enabled="True" MinimumPrefixLength="1" CompletionSetCount="10"
                                                    ServiceMethod="GetProduct2" ServicePath="SInventoryWebService.asmx" TargetControlID="nameTextBox"
                                                    UseContextKey="True" CompletionListCssClass="autocomplete_completionListElement"
                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    ShowOnlyCurrentWordInCompletionListItem="true">
                                                </asp:AutoCompleteExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CStock" Visible="False">
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
                                                <asp:TextBox ID="qtyTextBox" runat="server" CssClass="TextBoxMicroMini" ReadOnly="True"
                                                    Text='<%# Eval("Quantity")%>' AutoPostBack="True" OnTextChanged="qtyTextBox_TextChanged"></asp:TextBox>
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
                                        <asp:TemplateField HeaderText="DP">
                                            <ItemTemplate>
                                                <asp:TextBox ID="dpTextBox" runat="server" CssClass="TextBoxMicroMini" Text='<%# Eval("DiscountPercentage")%>'
                                                    ReadOnly="True"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="fdpTextBox" runat="server" TargetControlID="dpTextBox"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DAmt">
                                            <ItemTemplate>
                                                <asp:TextBox ID="dpAmtTextBox" runat="server" CssClass="TextBoxMicroMini" Text='<%# Eval("DiscountAmount")%>'
                                                    ReadOnly="True"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="fdpAmtTextBox" runat="server" TargetControlID="dpAmtTextBox"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SD">
                                            <ItemTemplate>
                                                <asp:TextBox ID="sdTextBox" runat="server" CssClass="TextBoxMicroMini" AutoPostBack="True"
                                                    Text='<%# Eval("SpecialAmount")%>' ReadOnly="True"  OnTextChanged="sdTextBox_OnTextChanged"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="fsdTextBox" runat="server" TargetControlID="sdTextBox"
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
                                        <asp:TemplateField HeaderText="DQty">
                                            <ItemTemplate>
                                                <asp:TextBox ID="dQtyTextBox" runat="server" CssClass="TextBoxMicroMini" AutoPostBack="True"
                                                    OnTextChanged="dQtyTextBox_TextChanged"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="fdQtyTextBox" runat="server" TargetControlID="dQtyTextBox"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="statusLabel" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reason">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="reasonReturnDropDownList" runat="server" CssClass="TextBoxMini"
                                                    Enabled="False">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DOD">
                                            <ItemTemplate>
                                                <asp:TextBox ID="dodTextBox" runat="server" CssClass="TextBoxMicroMini"
                                                    AutoPostBack="True"
                                                    OnTextChanged="dodTextBox_TextChanged"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="dodTextBox11" runat="server" TargetControlID="dodTextBox"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <br>
                            <br>
                        </td>
                    </tr>
                    
                     <tr>
                        <td class="TDLeft" width="13%">
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
                            
                        </td>
                        <td class="TDRight" width="20%">
                           <asp:HyperLink ID="HyperLink1" ForeColor="#339933" runat="server" NavigateUrl="DelivaryInvoiceCreationAuto.aspx"> &lt; &lt; &lt; Back to List</asp:HyperLink>
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Special Discount:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="pdTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:TextBox>
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
                            <asp:Button ID="Button1" runat="server" BackColor="#339933" Text="Invoice Generate"
                                OnClick="saveButton_Click" OnClientClick="return confirm('Are you sure you want to Save Delivery Invoice ?');" />
                        </td>
                         <td class="TDLeft" width="13%">
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;
                        </td>
                    </tr>
                    
                     <tr>
                        <td class="TDLeft" width="13%">
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
                            <asp:Label ID="invTextBox" runat="server" CssClass="TextBox"></asp:Label>
                        </td>
                        <td class="TDLeft" width="13%">
                            <asp:Button ID="printButton" runat="server" OnClick="printButton_Click" BackColor="#cc6600"
                                Text="Print" />
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
