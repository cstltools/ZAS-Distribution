<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MainMasterPage.master"
    AutoEventWireup="true" CodeFile="IssueRequisitionProducts.aspx.cs" Inherits="SInventory_UI_IssueRequisitionProducts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            Challan Generation
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
                            <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px">
                                Req No:
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:Label ID="reqNoTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                            </td>
                            <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px">
                                Req Date:
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:Label ID="reqDateTextBox" runat="server" CssClass="TextBoxCalander" ReadOnly="True"></asp:Label>
                            </td>
                            <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px">
                                Truck No:
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:TextBox ID="truckNoTextBox" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px">
                                Issue Challan No:
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:Label ID="clnNoTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                            </td>
                            <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px">
                                Issue Challan Date:
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:Label ID="clnDateTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                            </td>
                            <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px">
                                Driver Name:
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:TextBox ID="driverNameTextBox" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px">
                                Picking No:
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:Label ID="pikNoTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                            </td>
                            <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px">
                                Picking Date:
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:Label ID="pikDateTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
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
                            <td class="TDLeft" width="13%" colspan="6">
                                <div id="gridContainer1" style="height: auto; overflow: auto; width: 97%; margin: 0 auto;">
                                    <asp:GridView ID="issueGridView" runat="server" CssClass="gridview" AutoGenerateColumns="False"
                                        DataKeyNames="ReqChildId,StockInTransfarId">
                                        <Columns>
                                            <asp:BoundField DataField="ProductCode" HeaderText="Pro. Code" />
                                            <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                            <asp:BoundField DataField="PackSize" HeaderText="Pack Size" />
                                            <asp:BoundField DataField="BatchNo" HeaderText="Batch No." />
                                            <asp:BoundField DataField="PickingQty" HeaderText="Pick. Qty." />
                                            <asp:BoundField DataField="UnitPrice" HeaderText="Unit Price" />
                                            <asp:TemplateField HeaderText="Issue Qty.">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="issueQtyTextBox" runat="server" Enabled="False" AutoPostBack="True"
                                                        CssClass="TextBoxCalander" Text='<%# Eval("Quantity")%>' OnTextChanged="issueQtyTextBox_TextChanged"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="T. Price">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="priceTextBox" runat="server" CssClass="TextBoxCalander" Text='<%# Eval("PriceAmount")%>'
                                                        ReadOnly="True"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="VAT">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="vatTextBox" runat="server" CssClass="TextBoxCalander" Text='<%# Eval("VATAmount")%>'
                                                        ReadOnly="True"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tk.Value">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="totalPriceTextBox" runat="server" CssClass="TextBoxCalander" Text='<%# Eval("TotalPriceAmount")%>'
                                                        ReadOnly="True"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Issue">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="issueCheckBox" runat="server" AutoPostBack="True" OnCheckedChanged="issueCheckBox_CheckedChanged" />
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
                                
                            </td>
                            <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px">
                                Total TP Amount:
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:TextBox ID="totalAllPriceTextBox" runat="server" ReadOnly="True"></asp:TextBox>
                            </td>
                            <td class="TDLeft" width="13%">
                                
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
                                <asp:LinkButton ID="LinkButton1" runat="server" Font-Bold="True" OnClick="LinkButton1_Click">&lt;&lt;&lt;&lt;&lt;Back To List </asp:LinkButton>
                            </td>
                            <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px">
                                Total VAT Amount:&nbsp;
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:TextBox ID="vatAllPriceTextBox" runat="server" ReadOnly="True"></asp:TextBox>
                            </td>
                            <td class="TDLeft" width="13%">
                                
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
                            <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px">
                                 Grand Total:&nbsp;
                            </td>
                            <td class="TDRight" width="20%">
                               <asp:TextBox ID="grandTotalTextBox" runat="server" ReadOnly="True"></asp:TextBox>
                            </td>
                            <td class="TDLeft" width="13%">
                                
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
                                <asp:HiddenField ID="hdReqId" runat="server" />
                            </td>
                            <td class="TDRight" width="20%">
                                &nbsp;
                            </td>
                            <td class="TDLeft" width="13%">
                                
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:Button ID="bmitButton" runat="server" OnClick="bmitButton_Click" Text="Submit"
                                    OnClientClick="return confirm('Are you sure you want Save ?');" />
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
