<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MainMasterPage.master" AutoEventWireup="true" CodeFile="SampleProductrIssue.aspx.cs" Inherits="SInventory_UI_ManualOrderrCreation" %>

<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <style type="text/css">
        /*AutoComplete flyout */
        .autocomplete_completionListElement {
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

        .autocomplete_highlightedListItem {
            background-color: yellow;
            color: black;
            padding: 1px;
        }

        /* AutoComplete item */

        .autocomplete_listItem {
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
                        <td colspan="6" class="TableHeading">Sample product Issue
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:HiddenField ID="orderIdHiddenField" runat="server" />
                            <asp:HiddenField ID="custIdHiddenField" runat="server" />
                            <asp:HiddenField ID="paymentHiddenField" runat="server" />
                            <asp:HiddenField ID="dueHiddenField" runat="server" />
                        </td>
                        <td width="13%" class="TDLeft">
                            <asp:TextBox ID="orderNoTextBox" runat="server" ReadOnly="True" Visible="False"></asp:TextBox>
                        </td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">
                            <asp:HiddenField ID="mioIdHiddenField" runat="server" />
                            <asp:HiddenField ID="delIdHiddenField" runat="server" />
                        </td>
                    </tr>


                    <tr>
                        <td width="13%" class="TDLeft" style="text-align: center;">&nbsp; View List </td>
                        <td width="20%" class="TDRight">
                            <asp:ImageButton ID="productListImageButton" runat="server"
                                ImageUrl="~/images/viewList.png" OnClick="productImageButton_Click" />
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="MessageLabel" runat="server" ForeColor="#009900"></asp:Label>
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                    </tr>

                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;
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
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Customer Code: &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="custCodeTextBox" runat="server" CssClass="textbox" AutoPostBack="True"
                                OnTextChanged="custCodeTextBox_TextChanged"></asp:TextBox>
                            <ajaxToolkit:AutoCompleteExtender ID="customerTextBox_AutoCompleteExtender" runat="server"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="10" DelimiterCharacters=""
                                EnableCaching="true" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCustomernew"
                                ServicePath="SInventoryWebService.asmx" ShowOnlyCurrentWordInCompletionListItem="true"
                                TargetControlID="custCodeTextBox" UseContextKey="True">
                            </ajaxToolkit:AutoCompleteExtender>
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
                        <td class="TDRight" width="20%">&nbsp;</td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;</td>

                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>
                    <tr>

                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Distribution Center: <span class="star-mark">* </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="dcDropDownList" runat="server" CssClass="DropDown" OnSelectedIndexChanged="dcDropDownList_SelectedIndexChanged"
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </td>

                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Region: </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="regionDropDownList" runat="server" AutoPostBack="True"
                                CssClass="DropDown" Enabled="False">
                            </asp:DropDownList>
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Area:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="areaDropDownList" runat="server" AutoPostBack="True" CssClass="DropDown"
                                Enabled="False">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Company: &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="companyDropDownList" runat="server" AutoPostBack="True"
                                CssClass="DropDown"
                                Enabled="False">
                            </asp:DropDownList>
                        </td>

                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Territory:&nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="territoryDropDownList" runat="server" AutoPostBack="True" CssClass="DropDown"
                                Enabled="False">
                            </asp:DropDownList>
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Market:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="marketDropDownList" runat="server" AutoPostBack="True" CssClass="DropDown"
                                Enabled="False">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            <asp:HiddenField ID="discountIdHiddenField" runat="server" />
                            MIO:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="mioDropDownList" runat="server" AutoPostBack="True" CssClass="DropDown"
                                Enabled="False">
                            </asp:DropDownList>
                        </td>

                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">MIO Code:&nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:Label ID="mioCodeLabel" runat="server"></asp:Label>
                        </td>

                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">MIO Name:&nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:Label ID="mioNameLabel" runat="server"></asp:Label>
                        </td>

                    </tr>


                    <tr>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Issue Date: <span class="star-mark">* </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:Label ID="orderDateTextBox" runat="server" Width="140px"
                                CssClass="TextBoxCalander" AutoPostBack="True"></asp:Label>
                            <%--<asp:ImageButton ID="imgDate" runat="server" AlternateText="Click to show calendar"
                                ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="4" />
                            <ajaxToolkit:CalendarExtender ID="Date" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgDate"
                                TargetControlID="orderDateTextBox">
                            </ajaxToolkit:CalendarExtender>--%>
                        </td>

                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Customer Name:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:Label ID="custNameLabel" runat="server"></asp:Label>
                        </td>

                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Market Name:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:Label ID="marketNameLabel" runat="server"></asp:Label>
                        </td>
                    </tr>

                    <tr>

                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Delivery Person Name:&nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="deliverypersonNameTextBox" runat="server" CssClass="DropDown"
                                AutoPostBack="True" OnSelectedIndexChanged="deliverypersonNameTextBox_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Delivery Person Mob.No:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="deliverypersonMobileTextBox" ReadOnly="True" runat="server" CssClass="TextBox"></asp:TextBox>
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;"></td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="districtNameTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;</td>

                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>


                    <tr runat="server" visible="False">

                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Previous Due:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:Label ID="previousDueLabel" runat="server" Text=""></asp:Label>
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Credit Limit:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:Label ID="creditLimitNameLabel" runat="server" ForeColor="#339966"></asp:Label>
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Credit Taken:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:Label ID="crtakenNameLabel" runat="server" ForeColor="#0066FF"></asp:Label>
                        </td>
                    </tr>

                    <tr runat="server" visible="False">

                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Total Gross: 
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:Label ID="grossLabel" ForeColor="#d2691e" runat="server"></asp:Label>
                        </td>

                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Remaining: 
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:Label ID="remainingcrLabel" runat="server" ForeColor="#9900CC"></asp:Label>
                        </td>

                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">% of Used Credit:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:Label ID="percentOfUsedCrLabel" runat="server" ForeColor="#9900CC"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%"></td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>

                    <tr runat="server" visible="False">
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>

                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            <span style="color: #d2691e" runat="server" id="spord" visible="False">Is Special Order: </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:CheckBox ID="isSpecialAppCheckBox" runat="server" CssClass="inline-rb" Visible="False" />
                        </td>

                        <td class="TDLeft" width="13%"></td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%"></td>
                        <td class="TDLeft" width="13%"></td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>


                    <tr>


                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%"></td>
                        <td class="TDLeft" width="13%"></td>
                        <td class="TDRight" width="20%">&nbsp;
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
                                        <asp:TemplateField HeaderText="Product Code">
                                            <ItemTemplate>
                                                <asp:TextBox ID="productCodeTextBox" runat="server" CssClass="TextBoxCalander" AutoPostBack="True"
                                                    ToolTip="true" OnTextChanged="productCodeTextBox_TextChanged" Text='<%# Eval("ProductCode")%>'></asp:TextBox>
                                                <asp:HiddenField ID="unitpriceHiddenField" Value='<%# Eval("UnitPrice")%>' runat="server" />
                                                <asp:HiddenField ID="productidHiddenField" Value='<%# Eval("ProductId")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Product Name">
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
                                        <asp:TemplateField HeaderText="PackSize">
                                            <ItemTemplate>
                                                <asp:TextBox ID="packSizeTextBox" runat="server" CssClass="TextBoxCalander" ReadOnly="True"
                                                    Text='<%# Eval("PackSize")%>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Req. Qty">
                                            <ItemTemplate>
                                                <asp:TextBox ID="reqQtyTextBox" AutoPostBack="True" OnTextChanged="reqQtyTextBox_OnTextChanged"
                                                    runat="server" CssClass="TextBoxCalander" Text='<%# Eval("Quantity")%>'></asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="freqQtyTextBox" runat="server"
                                                    TargetControlID="reqQtyTextBox"
                                                    FilterType="Custom, Numbers"
                                                    ValidChars="." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="VAT">
                                            <ItemTemplate>
                                                <asp:TextBox ID="vatTextBox" runat="server" Enabled="False" CssClass="TextBoxCalander" Text='<%# Eval("VAT")%>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TP">
                                            <ItemTemplate>
                                                <asp:TextBox ID="tpTextBox" Enabled="False" runat="server" CssClass="TextBoxCalander" Text='<%# Eval("UnitPrice")%>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TPGross">
                                            <ItemTemplate>
                                                <asp:TextBox ID="grossTextBox" Enabled="False" runat="server" CssClass="TextBoxCalander" Text='<%# Eval("GrossValue")%>'></asp:TextBox>
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
                        <td class="TDLeft" width="13%">&nbsp;
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
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:Button ID="submitButton" runat="server" CssClass="button-padding-right" OnClick="submitButton_Click" OnClientClick="return confirm('Are you sure you want to Create Order ?');"
                                Text="Submit" />

                            <asp:Button ID="cancelButton" BackColor="#cc6600" runat="server" OnClick="cancelButton_Click"
                                Text="Cancel" />
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
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%"></td>
                        <td class="TDLeft" width="13%"></td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>

                    <%-- <tr>
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
                    </tr>
                    --%>
                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%"></td>
                        <td class="TDLeft" width="13%"></td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>

                    <tr runat="server" visible="False">

                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%"></td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Issue Code:
                            
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="code" runat="server" CssClass="textbox" AutoPostBack="True"></asp:TextBox>
                        </td>
                        <td class="TDLeft" width="13%"></td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>

                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;
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

                    <tr runat="server" visible="False">
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:Button ID="viewButton" runat="server" CssClass="button-padding-right" OnClick="view_Click"
                                Text="View Report" />
                        </td>
                        <td class="TDLeft" width="13%"></td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>

                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%"></td>
                        <td class="TDLeft" width="13%"></td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>

                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%"></td>
                        <td class="TDLeft" width="13%"></td>
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

</asp:Content>

