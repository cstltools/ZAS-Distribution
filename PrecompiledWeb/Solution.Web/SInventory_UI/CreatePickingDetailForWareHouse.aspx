<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_CreatePickingDetailForWareHouse, App_Web_uja4wjh3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
             <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            Picking Information
                        </td>
                          <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
            <tr>
                        <td class="TDLeft" width="13%">
                            Req No </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="reqNoTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td class="TDLeft" width="13%">
                            Req Date</td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="reqDateTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td class="TDLeft" width="13%">
                            Truck No</td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="truckNoTextBox" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            Picking No </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="clnNoTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td class="TDLeft" width="13%">
                            Picking Date</td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="clnDateTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td class="TDLeft" width="13%">
                            Driver No</td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="driverNameTextBox" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                     <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%" colspan="6">
                           
                            <asp:GridView ID="issueGridView" runat="server" CssClass="gridview" 
                                AutoGenerateColumns="False" DataKeyNames="ReqChildId">
                                <Columns>
                                    <asp:BoundField DataField="ProductCode" HeaderText="Pro Code" />
                                    <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                    <asp:BoundField DataField="PackSize" HeaderText="Pack Size" />
                                    <asp:BoundField DataField="CurrentStockQty" HeaderText="Curr. Stock" />
                                    <asp:BoundField DataField="ProductUnitPrice" HeaderText="Unit Price" />
                                    <asp:BoundField DataField="ReqQty" HeaderText="Req Qty" />
                                    <asp:TemplateField HeaderText="Pick Qty">
                                        <ItemTemplate>
                                            <asp:TextBox ID="issueQtyTextBox" runat="server" ReadOnly="True"
                                                AutoPostBack="True" CssClass="TextBoxCalander" Text= <%# Eval("ReqQty")%> 
                                                ontextchanged="issueQtyTextBox_TextChanged"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Price">
                                        <ItemTemplate>
                                            <asp:TextBox ID="priceTextBox" runat="server" CssClass="TextBoxCalander" ReadOnly="True"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="VAT">
                                        <ItemTemplate>
                                            <asp:TextBox ID="vatTextBox" runat="server" CssClass="TextBoxCalander" ReadOnly="True"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Value">
                                        <ItemTemplate>
                                            <asp:TextBox ID="totalPriceTextBox" runat="server" CssClass="TextBoxCalander" ReadOnly="True"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Case">
                                        <ItemTemplate>
                                            <asp:TextBox ID="caseTextBox" runat="server" CssClass="TextBoxMini" ReadOnly="True"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Pick">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="issueCheckBox" runat="server" AutoPostBack="True" 
                                                oncheckedchanged="issueCheckBox_CheckedChanged" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                       
                        
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            <asp:LinkButton ID="LinkButton1" runat="server" Font-Bold="True" 
                                onclick="LinkButton1_Click">&lt;&lt;&lt;&lt;&lt;Back To List </asp:LinkButton>
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            Total Amount :</td>
                        <td class="TDLeft" width="13%">
                            <asp:TextBox ID="totalAllPriceTextBox" runat="server" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            Total VAT :</td>
                        <td class="TDLeft" width="13%">
                            <asp:TextBox ID="vatAllPriceTextBox" runat="server" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            Grand Total Amount :</td>
                        <td class="TDLeft" width="13%">
                            <asp:TextBox ID="grandTotalTextBox" runat="server" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            <asp:HiddenField ID="hdReqId" runat="server" />
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            <asp:Button ID="bmitButton" runat="server" onclick="bmitButton_Click" 
                                Text="Submit"  OnClientClick="return confirm('Are you sure you want Save ?');" />
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                     <tr>
                            <td class="TDLeft" width="13%">
                                &nbsp;</td>
                            <td class="TDRight" width="20%">
                                &nbsp;</td>
                            <td class="TDLeft" width="13%">
                                &nbsp;</td>
                            <td class="TDRight" width="20%">
                                &nbsp;</td>
                            <td class="TDLeft" width="13%">
                                &nbsp;</td>
                            <td class="TDRight" width="20%">
                                &nbsp;</td>
                        </tr>
             </table>
                 </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

