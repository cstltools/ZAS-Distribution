<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_ProUnitPriceEntry, App_Web_dfmjdr5z" %>

<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            Unit Price Entry
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">
                            &nbsp; View List
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:ImageButton ID="productListImageButton" runat="server" ImageUrl="~/images/viewList.png"
                                OnClick="productImageButton_Click" />
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="MessageLabel" runat="server" ForeColor="#009900"></asp:Label>
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">
                            <asp:HiddenField ID="unitPriceIdHiddenField" runat="server" />
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
                            Product Code <span class="star-mark">* </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="productCodeTextBox" runat="server" AutoPostBack="True" CssClass="TextBox"
                                OnTextChanged="productCodeTextBox_TextChanged"></asp:TextBox>
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
                        </td>
                        <td class="TDLeft" width="13%">
                            Product Name<span class="star-mark"> * </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="productNameTextBox" runat="server" CssClass="TextBox" AutoPostBack="True"
                                OnTextChanged="productNameTextBox_TextChanged" ReadOnly="True"></asp:TextBox>
                            <ajaxToolkit:AutoCompleteExtender ID="productNameTextBox_AutoCompleteExtender" runat="server"
                                DelimiterCharacters="" EnableCaching="true" Enabled="True" MinimumPrefixLength="1"
                                CompletionSetCount="10" ServiceMethod="GetProduct" ServicePath="SInventoryWebService.asmx"
                                TargetControlID="productNameTextBox" UseContextKey="True" CompletionListCssClass="autocomplete_completionListElement"
                                CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                ShowOnlyCurrentWordInCompletionListItem="true">
                            </ajaxToolkit:AutoCompleteExtender>
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" class="TDLeft">
                            Pack Size <span class="star-mark">* </span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="packSizeTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:TextBox>
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
                            MRP Price <span class="star-mark">* </span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="mrpPriceTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server"
                                TargetControlID="mrpPriceTextBox" FilterType="Custom, Numbers" ValidChars=".0123456789" />
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                    </tr>
                    <tr runat="server" visible="False">
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                            &nbsp;
                        </td>
                        <td width="13%" class="TDLeft">
                            Cost Price <span class="star-mark">* </span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="costPriceTextBox" runat="server" CssClass="TextBox">0</asp:TextBox>
                            <ajaxToolkit:FilteredTextBoxExtender ID="fcostPriceTextBox" runat="server" TargetControlID="costPriceTextBox"
                                FilterType="Custom, Numbers" ValidChars=".0123456789" />
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
                            Unit Price <span class="star-mark">* </span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="unitPriceTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                            <ajaxToolkit:FilteredTextBoxExtender ID="FunitPriceTextBox" runat="server" TargetControlID="unitPriceTextBox"
                                FilterType="Custom, Numbers" ValidChars=".0123456789" />
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
                            Vat Percent % <span class="star-mark">* </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="vatperTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                            <ajaxToolkit:FilteredTextBoxExtender ID="vatperTextBox_FilteredTextBoxExtender" runat="server"
                                FilterType="Custom, Numbers" TargetControlID="vatperTextBox" ValidChars=".0123456789" />
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
                            Vat Amount <span class="star-mark">* </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="vatAmountTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                            <ajaxToolkit:FilteredTextBoxExtender ID="vatAmountTextBox_FilteredTextBoxExtender"
                                runat="server" FilterType="Custom, Numbers" TargetControlID="vatAmountTextBox"
                                ValidChars=".0123456789" />
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
                            Active Date <span class="star-mark">* </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="activeDtTextBox" runat="server" CssClass="TextBoxCalander" AutoPostBack="True"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="activeDtTextBox_CalendarExtender" runat="server"
                                Format="dd-MMM-yyyy" PopupButtonID="ImageButton" TargetControlID="activeDtTextBox">
                            </ajaxToolkit:CalendarExtender>
                            <asp:ImageButton ID="ImageButton" runat="server" AlternateText="Click to show calendar"
                                ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="4" />
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
                            Status <span class="star-mark">* </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:RadioButtonList ID="RadioButtonList1" runat="server" CssClass="inline-rb" AutoPostBack="True"
                                OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged">
                                <asp:ListItem>IsActive</asp:ListItem>
                                <asp:ListItem>Inactive</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;
                        </td>
                    </tr>
                    <div runat="server" id="new" visible="False">
                        <tr>
                            <td class="TDLeft" width="13%">
                                &nbsp;
                            </td>
                            <td class="TDRight" width="20%">
                                &nbsp;
                            </td>
                            <td class="TDLeft" width="13%">
                                InActive Date <span class="star-mark">* </span>
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:TextBox ID="inactiveTextBox" runat="server" AutoPostBack="True" CssClass="TextBoxCalander"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                    PopupButtonID="ImageButton1" TargetControlID="inactiveTextBox">
                                </ajaxToolkit:CalendarExtender>
                                <asp:ImageButton ID="ImageButton1" runat="server" AlternateText="Click to show calendar"
                                    ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="4" />
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
                                &nbsp;
                            </td>
                            <td width="13%" class="TDLeft">
                                New MRP Price <span class="star-mark">* </span>
                            </td>
                            <td width="20%" class="TDRight">
                                <asp:TextBox ID="newMRPTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server"
                                    TargetControlID="mrpPriceTextBox" FilterType="Custom, Numbers" ValidChars=".0123456789" />
                            </td>
                            <td width="13%" class="TDLeft">
                                &nbsp;
                            </td>
                            <td width="20%" class="TDRight">
                            </td>
                        </tr>
                        <tr runat="server" visible="False">
                            <td width="13%" class="TDLeft">
                            </td>
                            <td width="20%" class="TDRight">
                                &nbsp;
                            </td>
                            <td width="13%" class="TDLeft">
                                New Cost Price <span class="star-mark">* </span>
                            </td>
                            <td width="20%" class="TDRight">
                                <asp:TextBox ID="newCostPriceTextBox" runat="server" CssClass="TextBox">0</asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                    TargetControlID="newCostPriceTextBox" FilterType="Custom, Numbers" ValidChars=".0123456789" />
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
                                New Unit Price <span class="star-mark">* </span>
                            </td>
                            <td width="20%" class="TDRight">
                                <asp:TextBox ID="newUnitPriceTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                    TargetControlID="newUnitPriceTextBox" FilterType="Custom, Numbers" ValidChars=".0123456789" />
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
                                New Vat Percent % <span class="star-mark">* </span>
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:TextBox ID="newvatPerceTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server"
                                    FilterType="Custom, Numbers" TargetControlID="newvatPerceTextBox" ValidChars=".0123456789" />
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
                                New Vat Amount <span class="star-mark">* </span>
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:TextBox ID="newvatAmountTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server"
                                    FilterType="Custom, Numbers" TargetControlID="newvatAmountTextBox" ValidChars=".0123456789" />
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
                                Active Date <span class="star-mark">* </span>
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:TextBox ID="newactiveDtTextBox" runat="server" CssClass="TextBoxCalander" AutoPostBack="True"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                    PopupButtonID="ImageButton2" TargetControlID="newactiveDtTextBox">
                                </ajaxToolkit:CalendarExtender>
                                <asp:ImageButton ID="ImageButton2" runat="server" AlternateText="Click to show calendar"
                                    ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="4" />
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
                    </div>
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
                            &nbsp;
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Button ID="submitButton" runat="server" OnClick="submitButton_Click1" Text="Submit" />
                            <asp:Button ID="clearButton" runat="server" OnClick="clearButton_Click" Text="Clear" />
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
                            <asp:HiddenField ID="productIdHiddenField" runat="server" />
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
</asp:Content>
