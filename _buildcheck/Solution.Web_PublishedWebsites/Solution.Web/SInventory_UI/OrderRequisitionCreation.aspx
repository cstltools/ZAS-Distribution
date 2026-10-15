<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_OrderRequisitionCreation, App_Web_3bs1ezh0" %>

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
            width: 23% !important;
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
                            Stock Transfer Order
                        </td>
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
                                <asp:Label ID="msgLabel" runat="server"></asp:Label>
                            </td>
                            <td class="TDLeft" width="13%">
                                &nbsp;
                            </td>
                            <td class="TDRight" width="20%">
                                &nbsp;
                            </td>
                        </tr>
                        <%--<tr>
                        <td width="13%" class="TDLeft">
                            &nbsp; </td>
                        <td width="20%" class="TDRight">
                         
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="MessageLabel" runat="server" ForeColor="#009900"></asp:Label>
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                         <asp:HyperLink ID="HyperLink1" runat="server" ForeColor="green"
                                NavigateUrl="~/SInventory_UI/OrderRequisitionView.aspx">View List</asp:HyperLink>
                    </tr>--%>
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
                                <asp:Label ID="Label2" runat="server"></asp:Label>
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
                            <td class="TDRight" width="20%" style="text-align: right; padding-right: 10px;">
                                Company Name: <span class="star-mark">* </span>
                            </td>
                            <td class="TDLeft" width="13%">
                                <asp:DropDownList ID="companyNameDropDownList" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="companyNameDropDownList_OnSelectedIndexChanged" CssClass="DropDown">
                                </asp:DropDownList>
                            </td>
                            <td class="TDRight" width="20%" style="text-align: right; padding-right: 10px;">
                                Requsition No : <span class="star-mark">* </span>
                            </td>
                            <td class="TDLeft" width="13%">
                                <asp:TextBox ID="reqNoTextBox" runat="server" ReadOnly="True" CssClass="TextBox"></asp:TextBox>
                            </td>
                            <td class="TDRight" width="20%">
                            </td>
                        </tr>
                        <tr>
                            <td class="TDLeft" width="13%">
                                &nbsp;
                            </td>
                            <td class="TDRight" width="20%" style="text-align: right; padding-right: 10px;">
                                Warehouse : <span class="star-mark">* </span>
                            </td>
                            <td class="TDLeft" width="13%">
                                <asp:DropDownList ID="warehouseDropDownList" runat="server" CssClass="DropDown">
                                </asp:DropDownList>
                            </td>
                            <td class="TDRight" width="20%" style="text-align: right; padding-right: 10px;">
                                Requsition Date : <span class="star-mark">* </span>
                            </td>
                            <td class="TDLeft" width="13%">
                                <asp:TextBox ID="reqDateTextBox" runat="server" CssClass="TextBoxCalander" ReadOnly="True"
                                    Width="139px"></asp:TextBox>
                                <asp:ImageButton runat="server" AlternateText="Click to show calendar" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                    TabIndex="4" ID="imgDate"></asp:ImageButton>
                                <ajaxToolkit:CalendarExtender ID="Date" runat="server" Format="dd-MMM-yyyy" TargetControlID="reqDateTextBox"
                                    PopupButtonID="imgDate">
                                </ajaxToolkit:CalendarExtender>
                            </td>
                            <td class="TDRight" width="20%">
                                &nbsp;
                            </td>
                        </tr>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;
                        </td>
                        <td class="TDRight" width="20%" style="text-align: right; padding-right: 10px;">
                            Distribution Center : <span class="star-mark">* </span>
                        </td>
                        <td class="TDLeft" width="13%">
                            <asp:DropDownList ID="dcDropDownList" runat="server" CssClass="DropDown">
                            </asp:DropDownList>
                        </td>
                        <td class="TDRight" width="20%" style="text-align: right; padding-right: 10px;">
                            <span runat="server" visible="False">Upload File: </span>
                        </td>
                        <td class="TDLeft" width="13%" runat="server">
                            <asp:FileUpload ID="id_fu" runat="server" ToolTip="Select File To Upload." Visible="False"
                                class="btn" />
                            <asp:Button ID="btnUpload" runat="server" class="btn btn-primary" Text="Upload" OnClick="btnUpload_Click"
                                Visible="False" />
                            <asp:Label ID="lbl_up_status" runat="server" Visible="False"></asp:Label>
                            <asp:HiddenField ID="IsFileUploaded" runat="server" Visible="False" />
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
                    <tr runat="server" visible="false" id="productGrid">
                        <td class="TDRight" width="20%" colspan="6">
                            <div id="gridContainer1" style="height: auto; overflow: auto; width: 97%; margin: 0 auto;">
                                <asp:GridView ID="productGridView" runat="server" AutoGenerateColumns="False" CssClass="gridview">
                                    <Columns>
                                        <asp:TemplateField HeaderText="#SL">
                                            <ItemTemplate>
                                                <asp:Label ID="LabelSL" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Product Code">
                                            <ItemTemplate>
                                                <asp:TextBox ID="productCodeTextBox" runat="server" CssClass="TextBoxCalander" AutoPostBack="True"
                                                    ToolTip="true" OnTextChanged="productCodeTextBox_TextChanged" Text='<%# Eval("ProductCode")%>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Product Name">
                                            <ItemTemplate>
                                                <asp:TextBox ID="productNameTextBox" runat="server" CssClass="TextBox" Text='<%# Eval("ProductName")%>'
                                                    AutoPostBack="True" ToolTip="true" OnTextChanged="productNameTextBox_TextChanged"></asp:TextBox>
                                                <ajaxToolkit:AutoCompleteExtender ID="productNameTextBox_AutoCompleteExtender" runat="server"
                                                    DelimiterCharacters="" EnableCaching="true" Enabled="True" MinimumPrefixLength="1"
                                                    CompletionSetCount="10" ServiceMethod="GetApprovedProduct" ServicePath="SInventoryWebService.asmx"
                                                    TargetControlID="productNameTextBox" UseContextKey="True" CompletionListCssClass="autocomplete_completionListElement"
                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    ShowOnlyCurrentWordInCompletionListItem="true">
                                                </ajaxToolkit:AutoCompleteExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PackSize">
                                            <ItemTemplate>
                                                <asp:TextBox ID="packSizeTextBox" runat="server" CssClass="TextBoxCalander" ReadOnly="True"
                                                    Text='<%# Eval("PackSize")%>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty">
                                            <ItemTemplate>
                                                <asp:TextBox ID="reqQtyTextBox" runat="server" CssClass="TextBoxCalander" Text='<%# Eval("Quantity")%>'></asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="fcurrentStockTextBox" runat="server" TargetControlID="reqQtyTextBox"
                                                    FilterType="Custom, Numbers" ValidChars="." />
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
                            <asp:Button ID="submitButton" runat="server" CssClass="button-padding-right" OnClick="submitButton_Click"
                                OnClientClick="return confirm('Are you sure you want to Create Stock Transfer Order ?');"
                                Text="Submit" />
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
