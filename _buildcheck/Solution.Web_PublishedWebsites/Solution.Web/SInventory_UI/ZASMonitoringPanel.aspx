<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_ZASMonitoringPanel, App_Web_3bs1ezh0" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">Reconciliation
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
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
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
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">&nbsp;
                            
                            Reconciliation Type:

                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                            
                            <%--AutoPostBack="True" OnSelectedIndexChanged="Selection_Change"--%>
                            <asp:DropDownList ID="ddlType" runat="server">


                                <asp:ListItem Value="Silver"> Select from list </asp:ListItem>
                                <asp:ListItem Value="Stock"> Stock </asp:ListItem>
                                <asp:ListItem Value="Accounts"> Accounts Integration </asp:ListItem>

                            </asp:DropDownList>


                        </td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
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
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                            <%-- <asp:Button ID="SearchButton" runat="server" CssClass="button-padding-right" Text="Search" onclick="SearchButton_Click"  />--%>

                            <asp:Button ID="reportButton" runat="server" BackColor="#339933" CssClass="button-padding-right" OnClick="viewRptButton_Click"
                                Text="View" />

                            <asp:Button ID="clearButton" runat="server" BackColor="#cc6600" OnClick="clearButton_OnClick" CssClass="button-padding-right"
                                Text="Clear" />



                            <%--  <asp:Button ID="Button1" runat="server" BackColor="#054D51"  Text="Excel" onclick="opsheetButton_Click" />--%>
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
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
                        <td class="TDRight" width="20%" colspan="6">
                            <div id="gridContainer1" style="overflow: auto; width: 97%; width: auto">
                                <asp:GridView ID="loadStockGridView" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                                    DataKeyNames="DCStoreId">

                                    <Columns>

                                        <asp:TemplateField HeaderText="#SL">
                                            <ItemTemplate>
                                                <asp:Label ID="LabelSL" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="ProductCode" HeaderText="Product Code" />
                                        <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                        <asp:BoundField DataField="TotalQuantity" HeaderText="Receive Quantity" />

                                        <asp:BoundField DataField="B2bIssue" HeaderText="B2B Transfer" />
                                        <asp:BoundField DataField="SoldQuantity" HeaderText="Sales" />
                                        <asp:BoundField DataField="BookForDelivery" HeaderText="Book for Delivery" />
                                        <asp:BoundField DataField="SampleQty" HeaderText="Sample Qty" />
                                        <asp:BoundField DataField="DirectStockOutQty" HeaderText="Direct Stock Out" />
                                        <asp:BoundField DataField="Destroy" HeaderText="Destroy" />

                                        <asp:BoundField DataField="TotalIssue" HeaderText="Total Issue" />
                                        <asp:BoundField DataField="Closing" HeaderText="Closing Qty" />

                                        <asp:BoundField DataField="AvilableStock" HeaderText="Avilable Stock" />
                                        <asp:BoundField DataField="ReturnStock" HeaderText="Return Stock" />
                                        <asp:BoundField DataField="Restricted" HeaderText="Restricted" />
                                        <asp:BoundField DataField="Blocked" HeaderText="Blocked" />

                                        <asp:BoundField DataField="Error" HeaderText="Error Quantity" />

                                    </Columns>

                                </asp:GridView>
                            </div>
                        </td>


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
                        <td class="TDRight" width="20%" colspan="6">
                            <div id="gridContainer2" style="overflow: auto; width: 60%; width: auto">
                                <asp:GridView ID="loadAccountsIntegration" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                                    DataKeyNames="InvoiceId,VType">

                                    <Columns>

                                        <asp:TemplateField HeaderText="#SL">
                                            <ItemTemplate>
                                                <asp:Label ID="LabelSL" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="InvoiceNo" HeaderText="Proforma / Invoice No" />
                                        <asp:BoundField DataField="ProformaValue" HeaderText="Proforma / Sales Value" />
                                        <asp:BoundField DataField="VoucherCode" HeaderText="Voucher Code" />
                                        <asp:BoundField DataField="PostingValue" HeaderText="Posting Value" />
                                        <asp:BoundField DataField="VType" HeaderText="Type" />

                                    </Columns>

                                </asp:GridView>
                            </div>
                        </td>


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


    <asp:UpdateProgress ID="UpdateProgress1" runat="server" ClientIDMode="Static" DisplayAfter="0" DynamicLayout="true">
        <ProgressTemplate>
            <div class="divWaiting">
                <asp:Image ID="imgWait" CssClass="position-set" runat="server" ImageAlign="Middle" ImageUrl="~/images/progress-bar-opt.gif"
                    Height="80" Width="80" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


</asp:Content>

