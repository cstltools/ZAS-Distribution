<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_TotalSummary, App_Web_clqxceh0" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            Business Summary
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
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" class="TDLeft">
                            From Date
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="fromDateTextBox" runat="server" CssClass="TextBoxCalander"></asp:TextBox>
                            <asp:CalendarExtender ID="fromDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgDateFrom"
                                TargetControlID="fromDateTextBox">
                            </asp:CalendarExtender>
                            <asp:ImageButton ID="imgDateFrom" runat="server" AlternateText="Click to show calendar"
                                ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="4" />
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
                            To Date
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="toDateTextBox" runat="server" CssClass="TextBoxCalander"></asp:TextBox>
                            <asp:CalendarExtender ID="toDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgDateTo"
                                TargetControlID="toDateTextBox">
                            </asp:CalendarExtender>
                            <asp:ImageButton ID="imgDateTo" runat="server" AlternateText="Click to show calendar"
                                ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="4" />
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
                            <asp:Button ID="viewRptButton" runat="server" OnClick="viewRptButton_Click" Text="Search" />
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                            &nbsp;
                            <asp:Button ID="excelButton1" runat="server" Text="Export to Excel" OnClick="excelButton1_Click" />
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
                    <tr>
                        <td width="13%" class="TDLeft" colspan="6">
                            <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False" OnPageIndexChanging="OnPageIndexChanging"
                                CssClass="gridview" ShowFooter="True">
                                <Columns>
                                    <%--<asp:TemplateField HeaderText="#SL">
                                        <ItemTemplate>
                                            <asp:Label ID="LabelSL" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:BoundField DataField="ComUnitCode" HeaderText="Sales Center" />
                                    <asp:BoundField DataField="ShortName" HeaderText="Sales Center Name "  />

                                    <%-- <asp:BoundField DataField="NumberofProformaInvoice" HeaderText="Number of Proforma Invoice" 
                                ItemStyle-Width="60" DataFormatString="{0:D}"
                                  ItemStyle-HorizontalAlign="Right" >
                                    <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:BoundField>--%>
                                    

                                <asp:BoundField DataField="NumberofProformaInvoice" HeaderText="Number of Proforma Invoice" 
                                ItemStyle-Width="60" DataFormatString="{0:D}"
                                  ItemStyle-HorizontalAlign="Right" >
                                    <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:BoundField>
                                  <%--  <asp:BoundField DataField="NumberofProformaInvoice" HeaderText="Number of Proforma Invoice" />--%>
                                 
                                    <asp:BoundField DataField="SumofNetProformaAmount" 
                                        HeaderText="Sum of Net Proforma Amount(TP)"  ItemStyle-Width="60" DataFormatString="{0:N2}"
                                  ItemStyle-HorizontalAlign="Right" >
                                    <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:BoundField>
                                      <asp:BoundField DataField="ProTpVat" HeaderText="Proforma Total Vat" />
                                    <asp:BoundField DataField="NumberofInvoiceSold" HeaderText="Number of Invoices Sold" DataFormatString="{0:D}"/>
                                    <asp:BoundField DataField="SumofNetSalesAmount" HeaderText="Sum of Net Sales Amount(TP)" />
                                     <asp:BoundField DataField="DelTpVat" HeaderText="Sales Total Vat" />
                                    <asp:BoundField DataField="NumberofReturnInvoice" HeaderText="Number of Returned Invoices" DataFormatString="{0:D}"/>
                                    <asp:BoundField DataField="SumofNetReturnAmount" HeaderText="Sum of Net Return Amount(TP)" />
                                     <asp:BoundField DataField="DelReTpVat" HeaderText="Return Total Vat" />
                                   
                                </Columns>
                            </asp:GridView>
                            <br/>  <br/>  <br/>  <br/>  
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
