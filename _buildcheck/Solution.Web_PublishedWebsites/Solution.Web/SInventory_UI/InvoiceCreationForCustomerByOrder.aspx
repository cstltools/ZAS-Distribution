<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_InvoiceCreationForCustomer, App_Web_dfmjdr5z" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <link href="../PopCss/default.css" rel="stylesheet" type="text/css" />
    <link href="../PopCss/Styles.css" rel="stylesheet" type="text/css" />
        <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/3.5.4/select2.css" rel="stylesheet" />
    
    
    <style type="text/css">
        
        .deliveryMan {

            width:250px !important;
            display: block;
    width: 100%;
    padding: .375rem 2.25rem .375rem .75rem;
    -moz-padding-start: calc(0.75rem - 3px);
    font-size: 1rem;
    font-weight: 400;
    line-height: 1.5;
    color: #212529;
    background-color: #fff;
    background-image: url(data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 16 16'%3e%3cpath fill='none' stroke='%23343a40' stroke-linecap='round' stroke-linejoin='round' stroke-width='2' d='M2 5l6 6 6-6'/%3e%3c/svg%3e);
    background-repeat: no-repeat;
    background-position: right .75rem center;
    background-size: 16px 12px;
    border: 1px solid #ced4da;
    border-radius: .25rem;
    transition: border-color .15s ease-in-out, box-shadow .15s ease-in-out;
    -webkit-appearance: none;
    -moz-appearance: none;
    appearance: none;
            margin-bottom: 1px !important;
        }

    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">Proforma Invoice Creation
                        </td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%"></td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;<asp:HiddenField ID="hdfCustomer" runat="server" />
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Customer Code:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="custCodeTextBox" runat="server" AutoPostBack="True" ReadOnly="True"
                                CssClass="TextBox" OnTextChanged="custCodeTextBox_TextChanged"></asp:Label>
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Customer Name:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="custNameTextBox" runat="server" CssClass="TextBox" ReadOnly="True"
                                AutoPostBack="True" OnTextChanged="custNameTextBox_TextChanged"></asp:Label>
                            <%--<asp:AutoCompleteExtender ID="custNameTextBox_AutoCompleteExtender" runat="server"
                                DelimiterCharacters="" EnableCaching="true" Enabled="True" MinimumPrefixLength="1"
                                CompletionSetCount="10" ServiceMethod="GetCustomer" ServicePath="SInventoryWebService.asmx"
                                TargetControlID="custNameTextBox" UseContextKey="True" CompletionListCssClass="autocomplete_completionListElement"
                                CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                ShowOnlyCurrentWordInCompletionListItem="true">
                            </asp:AutoCompleteExtender>--%>
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Customer Address:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="custAddressTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Salse Center:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="comUnitNameTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">MIO Code:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="miaCodeTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">MIO Name:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="miaNameTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>

                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Territory Name:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="areaNameTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                        </td>

                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Merket:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="marketNameTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                        </td>


                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Invoice Date:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="invDateTextBox" runat="server" CssClass="TextBoxCalander" ReadOnly="True"></asp:Label>
                        </td>




                    </tr>
                    <tr>

                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Order No:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="orderNoTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Order Date:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="orderDateTextBox" runat="server" CssClass="TextBoxCalander" ReadOnly="True"></asp:Label>
                            <%-- <asp:ImageButton runat="server" AlternateText="Click to show calendar" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                TabIndex="4" ID="imgorderDate"></asp:ImageButton>
                            <asp:CalendarExtender ID="orderDate" runat="server" Format="dd-MMM-yyyy" TargetControlID="orderDateTextBox"
                                PopupButtonID="imgorderDate">
                            </asp:CalendarExtender>--%>
                        </td>

                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Customer Category:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="custCategoryTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>

                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Payment Type:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="payTypeDDL" runat="server" CssClass="DropDown">
                            </asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Remarks:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="remarksTextBox" runat="server" TextMode="MultiLine" Rows="3" CssClass="TextBox"></asp:TextBox>
                        </td>

                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Delivery Person Name:&nbsp;<span class="star-mark"> * </span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="deliverypersonNameTextBox" runat="server" CssClass="DropDown deliveryMan"
                                AutoPostBack="True" OnSelectedIndexChanged="deliverypersonNameTextBox_SelectedIndexChanged">
                            </asp:DropDownList>
                            

                        </td>
                    </tr>
                    <tr>

                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Delivery Person Mob.No:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="deliverypersonMobileTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;"></td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="districtNameTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                        </td>

                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">PO Number:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="txtPONumber" runat="server" CssClass="TextBox"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            <asp:HiddenField ID="orderIdHiddenField" runat="server" />
                            <asp:HiddenField ID="isVatApplicable" runat="server" />
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%" colspan="6">
                            <div id="gridContainer1" style="height: auto; overflow: auto; width: 97%; margin: 0 auto;">
                                <asp:GridView ID="gridLineItemGridView" runat="server" AutoGenerateColumns="False"
                                    CssClass="gridview">
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
                                                <asp:TextBox ID="qtyTextBox" runat="server" CssClass="TextBoxMicroMini" ReadOnly="True"
                                                    Text='<%# Eval("Quantity")%>' AutoPostBack="True" OnTextChanged="qtyTextBox_TextChanged"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="fqtyTextBox" runat="server" TargetControlID="qtyTextBox"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                                
                                                <asp:HiddenField ID="hdfActualOrderQuantity" runat="server" />
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
                                                <asp:TextBox ID="dpTextBox" runat="server" CssClass="TextBoxMicroMini" Text='<%# Eval("DiscountPercentage")%>' ReadOnly="True"></asp:TextBox>
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
                                                <asp:TextBox ID="sdTextBox" ReadOnly="True" runat="server" CssClass="TextBoxMicroMini" Text='<%# Eval("SpecialAmount")%>' AutoPostBack="True" OnTextChanged="sdTextBox_OnTextChanged"></asp:TextBox>
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
                                                <%--   <asp:FilteredTextBoxExtender ID="ftQtyTextBox" runat="server"
                                                        TargetControlID="tQtyTextBox"         
                                                        FilterType="Custom, Numbers"  ReadOnly="True"
                                                        ValidChars="." />--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="add" runat="server" Visible="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="addImageButton2" runat="server" ImageUrl="~/images/lineAdd.png"
                                                    OnClick="addImageButton_Click" />
                                                <%-- <asp:ImageButton ID="removeImageButton" runat="server" 
                                                ImageUrl="~/images/lineDelete.png" onclick="removeImageButton_Click" />--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <br />
                        </td>
                    </tr>

                    <%--<tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;
                        </td>
                        <td class="TDLeft" width="13%">
                            
                            <asp:UpdateProgress ID="progress" runat="server" ClientIDMode="Static" DisplayAfter="0"
                                DynamicLayout="true">
                                <ProgressTemplate>
                                    <div class="divWaiting">
                                        <asp:Image ID="imgWait" runat="server" ImageAlign="Middle" ImageUrl="~/Images/loading-icon-big.gif"
                                            Height="100%" Width="100%" />
                                    </div>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
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
                    </tr>--%>
                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">TP Total:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="tpTptalTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Discount Total:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="disTotalTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="InvoiceCreationByOrder.aspx"> &lt; &lt; &lt; Back to List </asp:HyperLink>
                            &nbsp;
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Special Discount:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="pdTextBox" runat="server" CssClass="TextBox" ReadOnly="True" AutoPostBack="True" OnTextChanged="pdTextBox_OnTextChanged"></asp:TextBox>
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">VAT Total:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="vatTotalTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Grand Total:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="grandTotalTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>

                    <tr>
                        <td class="TDLeft" width="13%"></td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>

                    <tr>
                        <td class="TDLeft" width="13%"></td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%"></td>
                        <td class="TDRight" width="20%">
                            <asp:Button ID="saveButton" runat="server" Text="Invoice Generate"
                                OnClick="saveButton_Click1" UseSubmitBehavior="False"
                                OnClientClick="if (this.disabled) return false; this.disabled = true; this.value = 'Processing...';" />
                            <asp:ModalPopupExtender ID="ModalPopupExtender10" runat="server" TargetControlID="HiddenField10"
                                PopupControlID="pnlModal" BackgroundCssClass="modalBackground">
                            </asp:ModalPopupExtender>


                            <asp:HiddenField ID="HiddenField10" runat="server" />
                            <asp:Panel ID="pnlModal" runat="server" CssClass="modalPopup" Style="display: none;">
                                <div class="popup_Titlebar" id="PopupHeader">
                                    <div class="TitlebarLeft">Confirm Message</div>
                                    <div class="TitlebarRight">
                                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../PopCss/Images/close.jpg" />
                                    </div>
                                </div>
                                <div class="popup_Body" align="center">
                                    <div class="mainLeft ">
                                        <asp:Image ID="Image2" runat="server" ImageUrl="~/PopCss/Images/question.png" Width="30px" />
                                    </div>
                                    <div class="mainRight">
                                        <p align="center">Are you want to save the invoice ? </p>
                                        <asp:TextBox ID="invoiceNoTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="popup_Buttons" align="center">
                                    <div class="right_button">
                                        <asp:Button ID="yesButton" runat="server" Text="Yes" BackColor="#1E90FF" Width="60px" Height="30px" OnClick="yesButton_Click" />
                                    </div>

                                    <div class="left_button">
                                        <asp:Button ID="noButton" runat="server" Text="No" BackColor="red" Width="60px" Height="30px" OnClick="noButton_Click" />
                                    </div>
                                </div>
                            </asp:Panel>
                        </td>
                        <td class="TDLeft" width="13%">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel3"
                                DisplayAfter="0" DynamicLayout="true">
                                <ProgressTemplate>
                                    <center>
                                        <asp:Image ID="Img2" runat="server" ImageUrl="~/Images/ajax-loader.gif" />
                                    </center>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>

                    <tr>
                        <td class="TDLeft" width="13%"></td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>

                    <tr>
                        <td class="TDLeft" width="13%"></td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Print InvNo
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:Label ID="invTextBox" runat="server" CssClass="TextBox"></asp:Label>
                            <asp:Label ID="rptInvoiceId" runat="server" Visible="False"  CssClass="TextBox"></asp:Label>
                        </td>
                        <td class="TDLeft" width="13%">
                            <asp:Button ID="printButton" BackColor="#cc6600" runat="server" OnClick="printButton_Click" Text="Print" />
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            <asp:HiddenField ID="hdCustomerMasterId" runat="server" />
                            <asp:HiddenField ID="hdComUnitId" runat="server" />
                            <asp:HiddenField ID="hdgrossValue" runat="server" />
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:HiddenField ID="hdMiaId" runat="server" />
                            <asp:HiddenField ID="orderHiddenField" runat="server" />
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
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

    <!-- Select2 Initialization Script -->
        <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/3.5.4/select2.min.js"></script>

    <script type="text/javascript">
        // The save button disables itself on click (double-submit guard). If an async postback
        // dies on the server the UpdatePanel never re-renders, so unlock the button here instead
        // of leaving it stuck on "Processing...". Registered first so a later handler that throws
        // (select2 needs jQuery, which is not always loaded) cannot stop it running.
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function (sender, args) {
            if (!args.get_error()) return;
            var b = document.getElementById('<%= saveButton.ClientID %>');
            if (b) { b.disabled = false; b.value = 'Invoice Generate'; }
            var p = document.getElementById('UpdateProgress1');
            if (p) { p.style.display = 'none'; }
        });

        $(document).ready(function () {
            $(".deliveryMan").select2({
                placeholder: "Select an option",
                allowClear: true
            });
        });

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(".deliveryMan").select2({
                placeholder: "Select an option",
                allowClear: true
            });
        });
    </script>
</asp:Content>
