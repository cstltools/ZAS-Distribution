<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MainMasterPage.master"
    AutoEventWireup="true" CodeFile="BonusCampaignSetup.aspx.cs" Inherits="SInventory_UI_CampaignSetup" %>

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
                        <td colspan="6" class="TableHeading">Bonus Campaign Setup
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft" style="text-align: center">View List</td>
                        <td width="20%" class="TDRight">
                            <asp:ImageButton ID="areaInfoNewImageButton" runat="server"
                                ImageUrl="~/images/viewList.png" OnClick="areaInfoNewImageButton_Click" />
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft" style="text-align: center"></td>
                        <td width="20%" class="TDRight"></td>
                    </tr>

                    <tr>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                    </tr>

                    <tr>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Wings Name: <span class="star-mark">* </span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="ddlWingsName" runat="server" AutoPostBack="True" CssClass="DropDown"
                                OnSelectedIndexChanged="ddlCompany_OnSelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:HiddenField ID="masterHiddenField" runat="server" />
                        </td>

                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">From Date: <span class="star-mark">* </span>
                        </td>
                        <td width="20%" class="TDRight">
                            
                             <asp:TextBox ID="fromDateTextBox" runat="server" Width="145px" CssClass="TextBoxCalander"></asp:TextBox>
                                <asp:ImageButton ID="imgDate" runat="server" AlternateText="Click to show calendar"
                                    ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="4" />
                                <ajaxToolkit:CalendarExtender ID="Date" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgDate"
                                    TargetControlID="fromDateTextBox">
                                </ajaxToolkit:CalendarExtender>


                            <%--<asp:TextBox
                                ID="fromDateTextBox"
                                runat="server"
                                CssClass="form-control form-control-sm"
                                TextMode="DateTimeLocal">
                            </asp:TextBox>--%>


                        </td>

                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Campaign Name: <span class="star-mark">* </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="tbxCampaignName" runat="server" CssClass="TextBox"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>

                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Bonus Product Name: <span class="star-mark">* </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="ddlBonusProductName" runat="server" CssClass="DropDown"></asp:DropDownList>
                        </td>


                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">To Date: <span class="star-mark">* </span>
                        </td>
                        <td width="20%" class="TDRight">
                            
                             
                             <asp:TextBox ID="toDateTextBox" runat="server" Width="145px" CssClass="TextBoxCalander"></asp:TextBox>
                                <asp:ImageButton ID="ImageButton3" runat="server" AlternateText="Click to show calendar"
                                    ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="4" />
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgDate"
                                    TargetControlID="toDateTextBox">
                                </ajaxToolkit:CalendarExtender>
                           
                           <%-- <asp:TextBox
                                ID="toDateTextBox"
                                runat="server"
                                CssClass="form-control form-control-sm"
                                TextMode="DateTimeLocal">
                            </asp:TextBox>--%>

                        </td>

                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;"> Campaign Description:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="tbxCampaignDescription" runat="server" CssClass="TextBox"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>

                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Bonus Quantity: <span class="star-mark">* </span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="tbxBonusQuantity" runat="server" CssClass="TextBox"></asp:TextBox>
                            <ajaxToolkit:FilteredTextBoxExtender ID="fbonusQtyTextBox" runat="server" TargetControlID="tbxBonusQuantity"
                                FilterType="Custom, Numbers" ValidChars="." />
                        </td>

                        <td width="13%" style="text-align: right; padding-right: 10px;" class="TDLeft"></td>

                        <td width="20%" class="TDRight">
                            <asp:CheckBox ID="cbxIsActive" Checked="True" CssClass="pading" runat="server" />
                            Is Active
                        </td>

                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;"></td>
                        <td width="20%" class="TDRight"></td>
                    </tr>


                    <tr>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
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
                                                <asp:TextBox ID="productCodeTextBox" runat="server" Width="100px" CssClass="TextBoxCalander" AutoPostBack="True"
                                                    ToolTip="true" OnTextChanged="productCodeTextBox_TextChanged" Text='<%# Eval("ProductCode")%>'></asp:TextBox>
                                                <asp:HiddenField ID="productidHiddenField" Value='<%# Eval("ProductId")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Point Product">
                                            <ItemTemplate>
                                                <asp:TextBox ID="productNameTextBox" Width="500px" runat="server" CssClass="TextBox" Text='<%# Eval("ProductName")%>'
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
                                                <asp:TextBox ID="qtyTextBox" AutoPostBack="True" Width="100px" runat="server" CssClass="TextBoxCalander"
                                                    Text='<%# Eval("Quantity")%>'></asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="freqQtyTextBox" runat="server" TargetControlID="qtyTextBox"
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
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight"></td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight"></td>
                    </tr>



                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">

                            <asp:Button ID="saveButton" runat="server" OnClick="Button1_Click"
                                CssClass="button-margin-right" OnClientClick="return confirm('Are you sure you want to Save ?');"
                                Text="Submit" />
                            <asp:Button ID="clearButton" runat="server" OnClick="clearButton_OnClick" Text="Clear"
                                BackColor="#E67E22" />
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>


                    <tr>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight"></td>
                    </tr>

                    <tr>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight"></td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight"></td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight"></td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight"></td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
