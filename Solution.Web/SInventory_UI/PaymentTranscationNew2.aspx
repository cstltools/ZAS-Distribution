<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MainMasterPage.master" AutoEventWireup="true" CodeFile="PaymentTranscationNew2.aspx.cs" Inherits="SInventory_UI_PaymentTranscation" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
     <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/3.5.4/select2.css" rel="stylesheet" />

    <style type="text/css">
        .balance {
            color: #008080;
            font-size: 20px;
        }

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
 

        .gridview {
            width: 100%;
            border-collapse: collapse;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            font-size: 14px;
            background-color: #fff;
            border: 1px solid #ccc;
        }

        .custom-btn {
    color: white;
    font-weight: bold;
    border: none;
    border-radius: 4px;
    padding: 6px 18px;
    font-size: 14px;
    height: 34px;
    margin-right: 8px;
    cursor: pointer;
    transition: background-color 0.2s ease;
}

.custom-btn:hover {
    opacity: 0.9;
}


        .btnAdd {
    background-color: #007bff;   /* Bootstrap Primary Blue */
    color: white;
    padding: 6px 16px;
    border: none;
    border-radius: 4px;
    font-weight: bold;
    font-size: 14px;
    height: 32px;
    cursor: pointer;
    transition: background-color 0.2s ease;
}

.btnAdd:hover {
    background-color: #0056b3;
}

       .InputLikeLabel {
    display: inline-block;
    width: 95%;
    height: 32px;
    padding: 6px 10px;
    border: 1px solid #ccc;
    border-radius: 4px;
    background-color: #f5f5f5;  /* Light grey to indicate non-editable */
    font-family: inherit;
    font-size: 14px;
    color: #666;                /* Slightly dimmed text */
    line-height: 18px;
    box-sizing: border-box;
    cursor: not-allowed;        /* Indicates read-only state */
    user-select: none;          /* Prevents text selection (optional) */
}


            .gridview th {
                background-color: #f2f2f2;
                color: #333;
                font-weight: bold;
                padding: 10px;
                border: 1px solid #ccc;
                text-align: center;
            }

            .gridview td {
                padding: 8px;
                border: 1px solid #ddd;
                text-align: center;
                color: #333;
            }

            .gridview tr:nth-child(even) {
                background-color: #f9f9f9;
            }

            .gridview tr:hover {
                background-color: #eef;
            }

            .gridview input[type="submit"],
            .gridview input[type="button"] {
                padding: 5px 10px;
                background-color: #d9534f;
                color: #fff;
                border: none;
                border-radius: 4px;
                cursor: pointer;
            }

                .gridview input[type="submit"]:hover,
                .gridview input[type="button"]:hover {
                    background-color: #c9302c;
                }


        .gridview {
            width: 100%;
            border-collapse: collapse;
            font-family: 'Segoe UI', sans-serif;
            font-size: 14px;
            border: 1px solid #ccc;
            background-color: #fff;
        }

            .gridview th {
                background-color: #f0f0f0;
                font-weight: bold;
                padding: 10px;
                border: 1px solid #ccc;
                text-align: center;
            }

            .gridview td {
                padding: 8px;
                border: 1px solid #ddd;
                text-align: center;
            }

            .gridview tr:nth-child(even) {
                background-color: #f9f9f9;
            }

            .gridview tr:hover {
                background-color: #e9f3ff;
            }

        .btnAdd {
            padding: 6px 12px;
            background-color: #28a745;
            color: white;
            border: none;
            border-radius: 4px;
            font-weight: bold;
            cursor: pointer;
        }

            .btnAdd:hover {
                background-color: #218838;
            }

        #gridContainer2 {
            border: 1px solid #ccc;
            background-color: #fafafa;
            padding: 5px;
            max-height: 400px;
            overflow-y: auto;
        }


        .divWaiting {
            background-color: #999;
            position: fixed;
            left: 0;
            top: 0;
            z-index: 2147483647;
            width: 100%;
            height: 100%;
            opacity: 0.5;
            overflow: hidden;
            display: flex;
            align-items: center;
            justify-content: center;
        }

            /* Optional: reduce image size */

            .divWaiting img {
                width: 80px; /* adjust size as needed */
                height: auto;
            }
    </style>




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
            width: 60% !important;
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
    
    
    <script type="text/javascript">
        
        function preventDoubleClick(btn) {
            if (confirm('Are you sure you want to Save ?')) {
                // disabling the button synchronously here would cancel the
                // postback itself (a disabled submit control never fires) --
                // defer it so the async postback is already dispatched first.
                setTimeout(function () {
                    btn.disabled = true;
                    btn.value = 'Please wait...';
                }, 0);
                return true;
            } else {
                return false;
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">Customer Payment
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight"></td>
                    </tr>
                    <tr runat="server" visible="false">
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            <span id="Span1">Company Name:</span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="manufacDropDownList" runat="server" AutoPostBack="True" CssClass="DropDown"
                                OnSelectedIndexChanged="ddlCompany_OnSelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Distribution Center: <span class="star-mark">* </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="dcDropDownList" runat="server" CssClass="DropDown" OnSelectedIndexChanged="dcDropDownList_SelectedIndexChanged"
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                    </tr>

                    <tr>

                        <td width="13%" style="text-align: right; padding-right: 10px;" class="TDLeft">
                            <asp:CheckBox ID="customerCheckBox" AutoPostBack="True" OnCheckedChanged="customerCheckBox_OnCheckedChanged" CssClass="pading" runat="server" />
                        </td>

                        <td width="20%" class="TDRight">Is Single Customer
                        </td>

                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Reference No:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="refNameTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Total Balance:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="totalBalance" Text="" CssClass="balance" runat="server" />
                        </td>
                    </tr>

                    <tr id="Tr1">
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Delivery Man:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="delDropDownList" runat="server" CssClass="DropDown deliveryMan" AutoPostBack="True"
                                OnSelectedIndexChanged="delDropDownList_OnTextChanged">
                            </asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Batch Creation Date:
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
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Batch No:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="batchDropDownList" runat="server" CssClass="DropDown" AutoPostBack="True"
                                OnSelectedIndexChanged="batchDropDownList_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>

                        <td id="Td1" class="TDLeft" runat="server" visible="False" width="13%" style="text-align: right; padding-right: 10px;">Customer Name:
                        </td>
                        <td id="Td2" class="TDRight" runat="server" visible="False" width="20%">
                            <asp:DropDownList ID="customerDropDownList" runat="server" AutoPostBack="True" CssClass="DropDown"
                                OnSelectedIndexChanged="customerDropDownList_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td width="13%" style="text-align: right; padding-right: 10px;" class="TDLeft">Customer Name:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="singleCustomerTextBox" Enabled="False" CssClass="TextBox" AutoPostBack="True" OnTextChanged="singleCustomerTextBox_OnTextChanged" runat="server"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="customerTextBox_AutoCompleteExtender" runat="server"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="10" DelimiterCharacters=""
                                EnableCaching="true" Enabled="True" MinimumPrefixLength="3" CompletionInterval="500" ServiceMethod="GetCustomernew"
                                ServicePath="SInventoryWebService.asmx" ShowOnlyCurrentWordInCompletionListItem="true"
                                TargetControlID="singleCustomerTextBox">
                            </asp:AutoCompleteExtender>
                            <asp:HiddenField ID="hdCustomerId" runat="server" />
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Payment Date:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="paymentDtTextBox" runat="server" AutoPostBack="True" ReadOnly="True"
                                CssClass="TextBoxCalander"></asp:Label>
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Payment Amount:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="paymentAmountTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                            <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderunitValue" runat="server"
                                Enabled="True" TargetControlID="paymentAmountTextBox" FilterType="Custom" ValidChars="0123456789.">
                            </asp:FilteredTextBoxExtender>--%>
                        </td>
                    </tr>

                    <tr>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;" width="13%"> Remarks 
                        </td>
                        <td colspan="5" class="TDRight" width="87%">
                            <asp:TextBox ID="tbxRemarks" TextMode="MultiLine" Rows="2" Columns="170" runat="server"></asp:TextBox>
                        </td>
                    </tr>


                    <tr runat="server" visible="False">

                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">Payment Type: <span style="color: red">*</span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="payTypeDDL" AutoPostBack="True" OnSelectedIndexChanged="payTypeDDL_OnSelectedIndexChanged" runat="server" CssClass="DropDown">
                            </asp:DropDownList>
                        </td>

                        <td width="13%" style="text-align: right; padding-right: 10px;" class="TDLeft">Bank Account:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="ddlBank" AutoPostBack="True" runat="server" CssClass="DropDown">
                            </asp:DropDownList>
                        </td>
                        <td width="13%" style="text-align: right; padding-right: 10px;" class="TDLeft">Cash Account:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="ddlCash" AutoPostBack="True" runat="server" CssClass="DropDown">
                            </asp:DropDownList>
                        </td>
                    </tr>

                    <tr>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight"></td>
                    </tr>

                    <tr id="DivCust" runat="server" visible="False">
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft">Customer
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="customerTextBox" runat="server" CssClass="TextBox" AutoPostBack="True"
                                OnTextChanged="customerTextBox_TextChanged"></asp:TextBox>
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
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight"></td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;</td>

                        <td class="TDRight" width="20%">&nbsp;</td>

                        <td class="TDLeft" width="13%">&nbsp;</td>

                        <td class="TDRight" width="20%">
                            <asp:Button
    ID="submitButton"
    runat="server"
    Text="🔍 Search"
    CssClass="custom-btn"
    UseSubmitBehavior="false"
    OnClick="Button1_Click2"
    BackColor="#16A085" />

<asp:Button
    ID="clearButton"
    runat="server"
    Text="🧹 Clear"
    CssClass="custom-btn"
    UseSubmitBehavior="false"
    OnClick="clearButton_OnClick"
    BackColor="#E67E22" />

                        </td>

                        <td class="TDLeft" width="13%">&nbsp;</td>

                        <td class="TDRight" width="20%">&nbsp;</td>
                    </tr>

                    <tr>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight"></td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%" colspan="6">
                            <div id="gridContainer1" style="height: auto; overflow: auto; width: 97%; margin: 0 auto;">
                                <asp:GridView ID="orderGridView" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                                    DataKeyNames="InvoiceId,CustomerMasterId,CustomerCode,CompanyId,ComUnitId,Due,PaymentTypeName">
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
                                        <asp:BoundField DataField="PaymentTypeName" HeaderText="Payment Type" />
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
                                                <asp:DropDownList ID="ddlCash" Width="80px" runat="server"></asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="AIT">
                                            <ItemTemplate>
                                                <asp:TextBox ID="aitTextBox" Width="80px" runat="server" AutoPostBack="True" OnTextChanged="aitTextBox_TextChanged"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderunitValue1" runat="server"
                                                    Enabled="True" TargetControlID="aitTextBox" FilterType="Custom" ValidChars="0123456789.">
                                                </asp:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Discount">
                                            <ItemTemplate>
                                                <asp:TextBox ID="discountTextBox" Width="80px" runat="server" AutoPostBack="True" OnTextChanged="discountTextBox_TextChanged"></asp:TextBox>
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
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%"></td>
                        <td class="TDRight" width="20%"></td>
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

                            <!-- Add to Grid Button -->

                            <div style="text-align: right; margin-bottom: 10px; width: 97%; margin: 0 auto;">
                                <asp:Button ID="btnAddToGrid" 
            runat="server" 
            Text="➕ Add to Grid" 
            CssClass="btnAdd" 
            UseSubmitBehavior="false" 
            OnClick="btnAddToGrid_Click" />

                            </div>

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
                        <td class="TDRight" width="20%"></td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>


                    <tr>
                        <td class="TDLeft" width="13%" colspan="6">


                            <!-- GridView -->

                            <div id="gridContainer2" style="height: auto; overflow: auto; width: 97%; margin: 0 auto;">
                                <asp:GridView ID="selectedOrderGridView" runat="server" AutoGenerateColumns="false" CssClass="gridview"
                                    DataKeyNames="InvoiceId,BankAccId,CashAccId,PayType,PaymentAmount,AIT,Discount,Remarks,BatchId,CustomerMasterId"
                                    OnRowCommand="selectedOrderGridView_RowCommand"
                                    EmptyDataText="No data found.">

                                    <Columns>
                                        <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" />

                                        <asp:BoundField DataField="BankAccountName" HeaderText="Bank Account">
                                            <ItemStyle Width="250px" />
                                            <HeaderStyle Width="250px" />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="CashAccountName" HeaderText="Cash Account">
                                            <ItemStyle Width="250px" />
                                            <HeaderStyle Width="250px" />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="PayType" HeaderText="Payment Type" />
                                        <asp:BoundField DataField="PaymentAmount" HeaderText="Payment Amount" />
                                        <asp:BoundField DataField="AIT" HeaderText="AIT" />
                                        <asp:BoundField DataField="Discount" HeaderText="Discount" />
                                        <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                                        <asp:ButtonField Text="Remove" CommandName="Remove" ButtonType="Button" />
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
                        <td class="TDLeft" width="13%"></td>
                        <td class="TDRight" width="20%"></td>
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

                            <asp:Button
                                ID="saveButton"
                                runat="server"
                                Enabled="False"
                                
                                OnClick="saveButton_ClickNew"
                                OnClientClick="return preventDoubleClick(this);"
                               
                                Text="Submit Payment"
                                CssClass="button-margin-right"
                                BackColor="#2980B9"
                                ForeColor="White"
                                Font-Bold="True"
                                BorderStyle="None"
                                Height="32px"
                                Width="130px"
                                Style="border-radius: 4px;" />

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
                        <td class="TDRight" width="20%"></td>
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
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">Print Money Receipt
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:Label ID="invTextBox" runat="server" CssClass="InputLikeLabel"></asp:Label>
                            <asp:Label ID="rptInvoiceId" runat="server" Visible="False" CssClass="TextBox"></asp:Label>
                            
                            
                        </td>
                        <td class="TDLeft" width="13%">
                            <asp:Button
                                ID="printButton"
                                runat="server"
                                OnClick="printButton_Click"
                                Text="Print"
                                BackColor="#CC6600"
                                ForeColor="White"
                                Font-Bold="True"
                                BorderStyle="None"
                                Height="32px"
                                Width="80px"
                                Style="border-radius: 4px;" />
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>

                    <tr id="Tr2" runat="server" visible="False">
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%">Ref Date
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="refDtTextBox" runat="server" AutoPostBack="True" CssClass="TextBoxCalander"></asp:TextBox>
                            <asp:CalendarExtender ID="paymentDtTextBox_CalendarExtender0" runat="server" Format="dd-MMM-yyyy"
                                PopupButtonID="ImageButton0" TargetControlID="refDtTextBox">
                            </asp:CalendarExtender>
                            <asp:ImageButton ID="ImageButton0" runat="server" AlternateText="Click to show calendar"
                                ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="4" />
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
                        <td class="TDRight" width="20%"></td>
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
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight"></td>
                    </tr>


                    <tr>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight"></td>
                    </tr>


                    <tr>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight"></td>
                    </tr>

                    <tr>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
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


    <asp:UpdateProgress ID="progress" runat="server" ClientIDMode="Static" DisplayAfter="0"
        DynamicLayout="true">
        <ProgressTemplate>
            <div class="divWaiting">
                <asp:Image ID="imgWait" runat="server" ImageAlign="Middle" ImageUrl="~/Images/loading-icon-big.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    
    
    <!-- Select2 Initialization Script -->
        <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/3.5.4/select2.min.js"></script>

    <script type="text/javascript">

        // Submit disables itself on click. If the async postback dies (timeout / server error)
        // the UpdatePanel never re-renders, so unlock it instead of leaving "Please wait..." forever.
        // Registered first so a later handler that throws (select2) cannot stop it running.
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function (sender, args) {
            if (!args.get_error()) return;
            var b = document.getElementById('<%= saveButton.ClientID %>');
            if (b) { b.disabled = false; b.value = 'Submit Payment'; }
            // UpdateProgress hides itself in a later endRequest handler; if select2's handler
            // throws first that never runs and the overlay keeps covering the page.
            var p = document.getElementById('progress');
            if (p) { p.style.display = 'none'; }
            args.set_errorHandled(true);
            alert('The server took too long or failed. Please check Money Receipt / payment list before submitting again to avoid a duplicate.');
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

