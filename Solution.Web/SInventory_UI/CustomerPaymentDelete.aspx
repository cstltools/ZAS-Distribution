<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MainMasterPage.master" AutoEventWireup="true" CodeFile="CustomerPaymentDelete.aspx.cs" Inherits="SInventory_UI_CustomerPaymentDelete" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            Customer Payment Delete</td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">
                            &nbsp;</td>
                        <td width="20%" class="TDRight">
                            &nbsp;</td>
                        <td width="13%" class="TDLeft">
                            Depo Name: <span class="star-mark"> * </span></td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="depoDropDownList" runat="server" AutoPostBack="True"
                                CssClass="DropDown" onselectedindexchanged="depoDropDownList_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;</td>
                        <td width="20%" class="TDRight">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">
                            &nbsp;</td>
                        <td width="20%" class="TDRight">
                            &nbsp;</td>
                        <td width="13%" class="TDLeft">
                            Route: <span class="star-mark"> * </span></td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="routeDropDownList" runat="server" AutoPostBack="True"
                                CssClass="DropDown" onselectedindexchanged="routeDropDownList_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;</td>
                        <td width="20%" class="TDRight">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">
                            &nbsp;</td>
                        <td width="20%" class="TDRight">
                            &nbsp;</td>
                        <td width="13%" class="TDLeft">
                            Invoice No: <span class="star-mark"> * </span></td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="invoiceDropDownList" runat="server" AutoPostBack="True"
                                CssClass="DropDown" onselectedindexchanged="invoiceDropDownList_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;</td>
                        <td width="20%" class="TDRight">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%" colspan="6">
                            <b>Payment List</b>
                            <div id="gridContainer1" style="height: auto; overflow: auto; width: 97%; margin: 0 auto;">
                                <asp:GridView ID="paymentGridView" runat="server" AutoGenerateColumns="False"
                                    CssClass="gridview" DataKeyNames="CustPayDetailId,CustPayId" OnRowCommand="paymentGridView_RowCommand"
                                    EmptyDataText="No Payment Found!">
                                    <Columns>
                                        <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" />
                                        <asp:BoundField DataField="PaymentAmount" HeaderText="Payment Amount" DataFormatString="{0:N2}" />
                                        <asp:BoundField DataField="PaymentDate" HeaderText="Payment Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:TextBox ID="remarksTextBox" runat="server" CssClass="TextBox" ReadOnly="True"
                                                    Text='<%# Eval("Remarks") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delete">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="deleteImageButton" runat="server" CommandArgument="<%# Container.DataItemIndex %>"
                                                    CommandName="DeleteData" ImageUrl="~/images/DeleteIcon.jpg"
                                                    OnClientClick="return confirm('Are you sure you want to Delete this Payment ?');" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
