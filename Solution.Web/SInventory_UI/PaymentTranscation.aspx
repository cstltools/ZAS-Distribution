<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MainMasterPage.master" AutoEventWireup="true" CodeFile="PaymentTranscation.aspx.cs" Inherits="SInventory_UI_PaymentTranscation" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
        <style type="text/css">
        
        .balance {
            color: #008080;
            font-size: 20px;
        }
        
    </style>
    
    
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
            width: 60% !important;
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            Customer Payment
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
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                    </tr>
                    <tr runat="server" Visible="false">
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            <span id="Span1">Company Name:</span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="manufacDropDownList" runat="server" AutoPostBack="True" CssClass="DropDown"
                                OnSelectedIndexChanged="ddlCompany_OnSelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                         <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Distribution Center: <span class="star-mark"> * </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="dcDropDownList" runat="server" CssClass="DropDown" OnSelectedIndexChanged="dcDropDownList_SelectedIndexChanged"
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                    </tr>
                    
                    <tr>
                        
                        <td width="13%"   style="text-align: right; padding-right: 10px; " class="TDLeft">                           
                            <asp:CheckBox ID="customerCheckBox" AutoPostBack="True" OnCheckedChanged="customerCheckBox_OnCheckedChanged" CssClass="pading" runat="server" />
                        </td>

                        <td width="20%" class="TDRight">
                            Is Single Customer
                        </td>
                        
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Reference No:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="refNameTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Total Balance:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="totalBalance" Text="" CssClass="balance" runat="server" />
                        </td>
                    </tr>

                    <tr id="Tr1" > 
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Delivery Man:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="delDropDownList" runat="server" CssClass="DropDown" AutoPostBack="True"
                                OnSelectedIndexChanged="delDropDownList_OnTextChanged">
                            </asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Batch Creation Date:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="dateTextBox" runat="server" CssClass="TextBoxCalander" AutoPostBack="True"
                                OnTextChanged="dateTextBox_TextChanged"></asp:TextBox>
                            <asp:ImageButton ID="imgDate" runat="server" AlternateText="Click to show calendar"
                                ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="4" />
                            <asp:CalendarExtender ID="Date" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgDate"
                                TargetControlID="dateTextBox">
                            </asp:CalendarExtender>
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Batch No:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="batchDropDownList" runat="server" CssClass="DropDown" AutoPostBack="True"
                                OnSelectedIndexChanged="batchDropDownList_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        
                        <td id="Td1" class="TDLeft" runat="server" visible="False" width="13%" style="text-align: right;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            padding-right: 10px;">
                            Customer Name:
                        </td>
                        <td id="Td2" class="TDRight" runat="server" visible="False" width="20%">
                            <asp:DropDownList ID="customerDropDownList" runat="server" AutoPostBack="True" CssClass="DropDown"
                                OnSelectedIndexChanged="customerDropDownList_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td width="13%" style="text-align: right; padding-right: 10px; " class="TDLeft">
                            Customer Name:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="singleCustomerTextBox" Enabled="False" CssClass="TextBox" AutoPostBack="True" OnTextChanged="singleCustomerTextBox_OnTextChanged" runat="server"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="customerTextBox_AutoCompleteExtender" runat="server"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="10" DelimiterCharacters=""
                                EnableCaching="true" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCustomernew"
                                ServicePath="SInventoryWebService.asmx" ShowOnlyCurrentWordInCompletionListItem="true"
                                TargetControlID="singleCustomerTextBox" >
                            </asp:AutoCompleteExtender>
                            <asp:HiddenField ID="hdCustomerId" runat="server" />
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Payment Date:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="paymentDtTextBox" runat="server" AutoPostBack="True" ReadOnly="True"
                                CssClass="TextBoxCalander"></asp:Label>
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Payment Amount:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="paymentAmountTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                            <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderunitValue" runat="server"
                                Enabled="True" TargetControlID="paymentAmountTextBox" FilterType="Custom" ValidChars="0123456789.">
                            </asp:FilteredTextBoxExtender>--%>
                        </td>
                    </tr>
                    
                    
                    <tr runat="server" Visible="False">
                        
                         <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Payment Type: <span style="color: red">*</span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="payTypeDDL" AutoPostBack="True" OnSelectedIndexChanged="payTypeDDL_OnSelectedIndexChanged" runat="server" CssClass="DropDown">
                            </asp:DropDownList>
                        </td>
                        
                        <td width="13%" style="text-align: right; padding-right: 10px; " class="TDLeft">
                            Bank Account:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="ddlBank" AutoPostBack="True" runat="server" CssClass="DropDown">
                            </asp:DropDownList>
                        </td>
                        <td width="13%" style="text-align: right; padding-right: 10px; " class="TDLeft">
                            Cash Account:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="ddlCash" AutoPostBack="True" runat="server" CssClass="DropDown">
                            </asp:DropDownList>
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
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                    </tr>
                    
                    <tr id="DivCust" runat="server" visible="False">
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                            &nbsp;
                        </td>
                        <td width="13%" class="TDLeft">
                            Customer
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="customerTextBox" runat="server" CssClass="TextBox" AutoPostBack="True"
                                OnTextChanged="customerTextBox_TextChanged"></asp:TextBox>
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;
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
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;
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
                            <asp:Button ID="submitButton" runat="server" Text="Search" CssClass="button-margin-right"
                                OnClick="Button1_Click2" BackColor="#16A085" />
                            <asp:Button ID="saveButton" runat="server" Enabled="False" OnClick="saveButton_Click"
                                CssClass="button-margin-right" OnClientClick="return confirm('Are you sure you want to Save ?');"
                                Text="Submit" />
                            <asp:Button ID="clearButton" runat="server" OnClick="clearButton_OnClick" Text="Clear"
                                BackColor="#E67E22" />
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
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%" colspan="6">
                            <div id="gridContainer1" style="height: auto; overflow: auto; width: 97%; margin: 0 auto;">
                                <asp:GridView ID="orderGridView" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                                    DataKeyNames="InvoiceId,CustomerMasterId,CustomerCode,CompanyId">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" AutoPostBack="True" runat="server" OnCheckedChanged="chkSelect_CheckedChanged" />
                                            </ItemTemplate>
                                        </asp:TemplateField>    
                                        <asp:BoundField DataField="ComUnitName" HeaderText="Unit Name" />
                                        <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" />
                                        <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                        <asp:BoundField DataField="DelivaryInvoiceNo" HeaderText="Del. Invoice" />
                                        <asp:BoundField DataField="UpdateDate" HeaderText="Del. Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                        <asp:BoundField DataField="DeliveryTpGrandTotal" HeaderText="Receiveable" HtmlEncodeFormatString="False" />
                                        <asp:BoundField DataField="PaymentAmount" HeaderText="Previous Pay" HtmlEncodeFormatString="False" />
                                        <asp:BoundField DataField="Due" HeaderText="Due Amount" />
                                        <asp:TemplateField HeaderText="Pay Amount">
                                            <ItemTemplate>
                                                <asp:TextBox ID="payAmountTextBox" runat="server" Width="80px" AutoPostBack="True" OnTextChanged="payAmountTextBox_TextChanged"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderunitValue" runat="server"
                                                    Enabled="True" TargetControlID="payAmountTextBox" FilterType="Custom" ValidChars="0123456789.">
                                                </asp:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Is Bank">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="isBank" AutoPostBack="True" runat="server" OnCheckedChanged="isBank_CheckedChanged" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Bank Account">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlBank" Width="80px" runat="server" AutoPostBack="True"></asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        
                                        <asp:TemplateField HeaderText="Is Cash">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="isCash" AutoPostBack="True" runat="server" OnCheckedChanged="isCash_CheckedChanged" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Cash Account">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlCash" Width="80px" runat="server" AutoPostBack="True"></asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="AIT">
                                            <ItemTemplate>
                                                <asp:TextBox ID="aitTextBox"  Width="80px" runat="server" AutoPostBack="True"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderunitValue1" runat="server"
                                                    Enabled="True" TargetControlID="aitTextBox" FilterType="Custom" ValidChars="0123456789.">
                                                </asp:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Discount">
                                            <ItemTemplate>
                                                <asp:TextBox ID="discountTextBox" Width="80px" runat="server" AutoPostBack="True"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderunitValue12" runat="server"
                                                    Enabled="True" TargetControlID="discountTextBox" FilterType="Custom" ValidChars="0123456789.">
                                                </asp:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:TextBox ID="remarksTextBox" runat="server" AutoPostBack="True"></asp:TextBox>                                               
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
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;
                        </td>
                    </tr>
                    <tr id="Tr2" runat="server" visible="False">
                        <td class="TDLeft" width="13%">
                            &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;
                        </td>
                        <td class="TDLeft" width="13%">
                            Ref Date
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="refDtTextBox" runat="server" AutoPostBack="True" CssClass="TextBoxCalander"></asp:TextBox>
                            <asp:CalendarExtender ID="paymentDtTextBox_CalendarExtender0" runat="server" Format="dd-MMM-yyyy"
                                PopupButtonID="ImageButton0" TargetControlID="refDtTextBox">
                            </asp:CalendarExtender>
                            <asp:ImageButton ID="ImageButton0" runat="server" AlternateText="Click to show calendar"
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
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;
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
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;
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
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;
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
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;
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
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

