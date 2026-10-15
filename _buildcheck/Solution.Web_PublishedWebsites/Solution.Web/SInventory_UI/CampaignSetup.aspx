<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_CampaignSetup, App_Web_htam3fti" %>

<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
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
            <div >
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            Campaign Setup
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
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
                        <td width="13%" class="TDLeft" style="text-align: center">
                            View List</td>
                        <td width="20%" class="TDRight">
                            <asp:ImageButton ID="areaInfoNewImageButton" runat="server" 
                                ImageUrl="~/images/viewList.png" onclick="areaInfoNewImageButton_Click" />
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: center">
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
                            Company
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="companyDropDownList" AutoPostBack="True" 
                            OnSelectedIndexChanged="companyDropDownList_OnSelectedIndexChanged" runat="server" CssClass="DropDown">
                            </asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft">
                            <asp:HiddenField ID="masterHiddenField" runat="server" />
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
                        <td class="TDRight" width="20%" colspan="6">
                            <div id="gridContainer1" style="height: auto; overflow: auto; width: 97%; margin: 0 auto;">
                                <asp:GridView ID="productGridView" runat="server" AutoGenerateColumns="False" CssClass="gridview">
                                    <Columns>
                                        <asp:TemplateField HeaderText="SL">
                                            <ItemTemplate>
                                                <span>
                                                    <%#Container.DataItemIndex + 1%>
                                                </span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Code">
                                            <ItemTemplate>
                                                <asp:TextBox ID="productCodeTextBox" runat="server" Width="30px" CssClass="TextBoxCalander" AutoPostBack="True"
                                                    ToolTip="true" OnTextChanged="productCodeTextBox_TextChanged" Text='<%# Eval("ProductCode")%>'></asp:TextBox>
                                                <asp:HiddenField ID="productidHiddenField" Value='<%# Eval("ProductId")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Point Product">
                                            <ItemTemplate>
                                                <asp:TextBox ID="productNameTextBox" runat="server" CssClass="TextBox" Text='<%# Eval("ProductName")%>'
                                                    AutoPostBack="True" ToolTip="true" OnTextChanged="productNameTextBox_TextChanged"></asp:TextBox>
                                                <ajaxToolkit:AutoCompleteExtender ID="productNameTextBox_AutoCompleteExtender" runat="server"
                                                    DelimiterCharacters="" EnableCaching="true" Enabled="True" MinimumPrefixLength="1"
                                                    CompletionSetCount="10" ServiceMethod="GetProductByMenufracturer" ServicePath="SInventoryWebService.asmx"
                                                    TargetControlID="productNameTextBox" CompletionListCssClass="autocomplete_completionListElement"
                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    ShowOnlyCurrentWordInCompletionListItem="true">
                                                </ajaxToolkit:AutoCompleteExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty">
                                            <ItemTemplate>
                                                <asp:TextBox ID="qtyTextBox" AutoPostBack="True" Width="40px" runat="server" CssClass="TextBoxCalander"
                                                    Text='<%# Eval("Quantity")%>'></asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="freqQtyTextBox" runat="server" TargetControlID="qtyTextBox"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Code">
                                            <ItemTemplate>
                                                <asp:TextBox ID="bonusproductCodeTextBox" Width="40px" runat="server" CssClass="TextBoxCalander"
                                                    AutoPostBack="True" ToolTip="true" OnTextChanged="bonusproductCodeTextBox_OnTextChanged"
                                                    Text='<%# Eval("BonusProductCode")%>'></asp:TextBox>
                                                <asp:HiddenField ID="bonusproductidHiddenField" Value='<%# Eval("BonusProductId")%>'
                                                    runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bonus Product Name">
                                            <ItemTemplate>
                                                <asp:TextBox ID="bonusproductNameTextBox" runat="server" CssClass="TextBox" Text='<%# Eval("BonusProductName")%>'
                                                    AutoPostBack="True" ToolTip="true" OnTextChanged="bonusproductNameTextBox_OnTextChanged"></asp:TextBox>
                                                <ajaxToolkit:AutoCompleteExtender ID="bonusproductNameTextBox_AutoCompleteExtendera" runat="server"
                                                    DelimiterCharacters="" EnableCaching="true" Enabled="True" MinimumPrefixLength="1"
                                                    CompletionSetCount="10" ServiceMethod="GetProductByMenufracturer" ServicePath="SInventoryWebService.asmx"
                                                    TargetControlID="bonusproductNameTextBox" CompletionListCssClass="autocomplete_completionListElement"
                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    ShowOnlyCurrentWordInCompletionListItem="true">
                                                </ajaxToolkit:AutoCompleteExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bonus Qty">
                                            <ItemTemplate>
                                                <asp:TextBox ID="bonusQtyTextBox" AutoPostBack="True" Width="30px" runat="server" CssClass="TextBoxCalander"
                                                    Text='<%# Eval("BonusQuantity")%>'></asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="fbQtyTextBox" runat="server" TargetControlID="bonusQtyTextBox"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Is Active">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="isActiveCheckBox" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="From Date">
                                            <ItemTemplate>
                                                <asp:TextBox ID="fromDateTextBox" runat="server" Text='<%# Eval("FromDate")%>' 
                                                    CssClass="TextBoxCalander"></asp:TextBox>
                                              <%--  <asp:ImageButton ID="imgDate" runat="server" AlternateText="Click to show calendar"
                                                    ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="4" />--%>
                                                <ajaxToolkit:CalendarExtender ID="Date" runat="server" Format="dd-MMM-yyyy" PopupButtonID="fromDateTextBox"
                                                    TargetControlID="fromDateTextBox">
                                                </ajaxToolkit:CalendarExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Time">
                                            <ItemTemplate>
                                                <asp:TextBox ID="fromtimeTextBox" Text='<%# Eval("FromTime")%>' runat="server" 
                                                    CssClass="TextBoxCalander"></asp:TextBox>
                                                <ajaxToolkit:MaskedEditExtender ID="from" TargetControlID="fromtimeTextBox" Mask="99:99:99"
                                                    AcceptAMPM="True" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                    OnInvalidCssClass="MaskedEditError" MaskType="Time" InputDirection="RightToLeft"
                                                    AcceptNegative="Left" DisplayMoney="Left" ErrorTooltipEnabled="True" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="To Date">
                                            <ItemTemplate>
                                                <asp:TextBox ID="toDateTextBox" Text='<%# Eval("ToDate")%>' runat="server" 
                                                    CssClass="TextBoxCalander"></asp:TextBox>
                                               <%-- <asp:ImageButton ID="toimgDate" runat="server" AlternateText="Click to show calendar"
                                                    ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="4" />--%>
                                                <ajaxToolkit:CalendarExtender ID="toDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="toDateTextBox"
                                                    TargetControlID="toDateTextBox">
                                                </ajaxToolkit:CalendarExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Time">
                                            <ItemTemplate>
                                                <asp:TextBox ID="totimeTextBox" Text='<%# Eval("ToTime")%>' runat="server" 
                                                    CssClass="TextBoxCalander"></asp:TextBox>
                                                <ajaxToolkit:MaskedEditExtender TargetControlID="totimeTextBox" ID="to" Mask="99:99:99"
                                                    AcceptAMPM="True" MaskType="Time" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                    OnInvalidCssClass="MaskedEditError" InputDirection="RightToLeft" AcceptNegative="Left"
                                                    DisplayMoney="Left" ErrorTooltipEnabled="True" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Add">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/lineAdd.png"
                                                    OnClick="ImageButton1_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remove">
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
                        <td width="13%" class="TDLeft">
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
                            &nbsp;
                            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Save" />
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
</asp:Content>
