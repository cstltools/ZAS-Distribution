<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MainMasterPage.master" AutoEventWireup="true" CodeFile="BatchWiseCollection.aspx.cs" Inherits="SInventory_UI_BatchWiseCollection" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            Batch Wise Collection Report
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
                            <asp:DropDownList ID="batchDropDownList" runat="server" CssClass="DropDown">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="Tr1" runat="server" Visible="False">
                        <td width="13%" style="text-align: right; padding-right: 10px;" class="TDLeft">
                            <asp:CheckBox ID="customerCheckBox" AutoPostBack="True" OnCheckedChanged="customerCheckBox_OnCheckedChanged"
                                CssClass="pading" runat="server" />
                        </td>
                        <td width="20%" class="TDRight">
                            Is Single Customer
                        </td>
                        <td width="13%" style="text-align: right; padding-right: 10px;" class="TDLeft">
                            Customer Code:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="singleCustomerTextBox" Enabled="False" CssClass="TextBox" AutoPostBack="True"
                                OnTextChanged="singleCustomerTextBox_OnTextChanged" runat="server"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="customerTextBox_AutoCompleteExtender" runat="server"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="10" DelimiterCharacters=""
                                EnableCaching="true" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCustomernew"
                                ServicePath="SInventoryWebService.asmx" ShowOnlyCurrentWordInCompletionListItem="true"
                                TargetControlID="singleCustomerTextBox">
                            </asp:AutoCompleteExtender>
                            <asp:HiddenField ID="hdCustomerId" runat="server" />
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Customer Name:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="nameTextBox" runat="server" AutoPostBack="True" ReadOnly="True"></asp:Label>
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            <%-- Payment Amount:--%>
                        </td>
                        <td width="20%" class="TDRight">
                            <%-- <asp:Label ID="paymentAmountTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>--%>
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
                           <%-- <asp:Button ID="submitButton" runat="server" Text="View Report" CssClass="button-margin-right"
                                OnClick="submitButton_OnClick" BackColor="#16A085" />--%>
                                <asp:Button ID="Button1" runat="server" Text="View Report" CssClass="button-padding-right"
                                OnClick="submitButton_OnClick" BackColor="#339933" />
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
                    <tr>
                        <td width="20%" class="TDRight" colspan="6">
                            <div id="gridContainer" style="height: 400px; overflow: auto; width: 97%; margin: 0 auto;">
                                <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                                    DataKeyNames="CustPayDetailId">
                                    <Columns>
                                        <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                        <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" />
                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                                        <asp:BoundField DataField="BatchCreationDate" HeaderText="Batch Creation Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                        <asp:BoundField DataField="BatchNo" HeaderText="Batch" />
                                        <asp:BoundField DataField="PaymentDate" HeaderText="Payment Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                        <asp:BoundField DataField="PaidAmount" HeaderText="Paid Amount" />
                                        
                                        <asp:TemplateField HeaderText="Action" >
                                        <ItemTemplate>
                                            <asp:LinkButton ID="invoiceButton" runat="server" Text="Report &gt; &gt; &gt;" ForeColor="#DE751F"
                                                OnClick="invoiceButton_Click" />
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
    
     <asp:UpdateProgress ID="UpdateProgress1" runat="server" ClientIDMode="Static" DisplayAfter="0" DynamicLayout="true">
        <ProgressTemplate>
            <div class="divWaiting">
                <asp:Image ID="imgWait" CssClass="position-set" runat="server" ImageAlign="Middle" ImageUrl="~/images/progress-bar-opt.gif"
                    Height="80" Width="80" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


</asp:Content>

