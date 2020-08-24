<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MainMasterPage.master"
    AutoEventWireup="true" CodeFile="CreatePickingDetailForWareHouse.aspx.cs" Inherits="SInventory_UI_CreatePickingDetailForWareHouse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
                                Company Name: 
                            </td>
                            <td class="TDRight" width="20%">
                                 <asp:Label ID="companyLabel" runat="server"  ReadOnly="True"></asp:Label> &nbsp;
                                 <asp:HiddenField ID="companyIdHiddenField" runat="server" />
                            </td>
                            <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px">
                               Warehouse Name: 
                            </td>
                            <td class="TDRight" width="20%">
                                 <asp:Label ID="warehouseLabel" runat="server"  ReadOnly="True"></asp:Label> &nbsp;
                                 <asp:HiddenField ID="warehouseIdHiddenField" runat="server" />
                            </td>
                            <td class="TDLeft" width="13%">
                                
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
                                <asp:Label ID="reqNoTextBox" runat="server"  ReadOnly="True"></asp:Label>
                            </td>
                            <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px">
                                Req Date:
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:Label ID="reqDateTextBox" runat="server"  ReadOnly="True"></asp:Label>
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
                                Picking No:
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:Label ID="clnNoTextBox" runat="server" ReadOnly="True"></asp:Label>
                            </td>
                            <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px">
                                Picking Date:
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:Label ID="clnDateTextBox" runat="server" ReadOnly="True"></asp:Label>
                            </td>
                            <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px">
                                Driver No
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:TextBox ID="driverNameTextBox" runat="server"></asp:TextBox>
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
                                
                            </td>
                            <td class="TDRight" width="20%">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="TDLeft" width="13%" colspan="6">
                                <div id="gridContainer1" style="height: auto; overflow: auto; width: 97%; margin: 0 auto;">
                                    <asp:GridView ID="issueGridView" runat="server" CssClass="gridview" AutoGenerateColumns="False"
                                        DataKeyNames="ReqChildId">
                                        <Columns>
                                            <asp:BoundField DataField="ProductCode" HeaderText="Code" />
                                            <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                            <asp:BoundField DataField="PackSize" HeaderText="Pack Size" />
                                            <asp:BoundField DataField="CurrentStockQty" HeaderText="Curr. Stock" />
                                            <asp:BoundField DataField="ProductUnitPrice" HeaderText="Unit Price" />
                                            <asp:BoundField DataField="ReqQty" HeaderText="Req Qty" />
                                            <asp:TemplateField HeaderText="Pick Qty">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="issueQtyTextBox" runat="server" ReadOnly="True" AutoPostBack="True"
                                                        CssClass="TextBoxCalander" Text='<%# Eval("ReqQty")%>' OnTextChanged="issueQtyTextBox_TextChanged"></asp:TextBox>
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
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                                </HeaderTemplate>
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
                            <td class="TDLeft" width="13%"style="text-align: right; padding-right: 10px">
                               Total Amount :&nbsp;
                            </td>
                            <td class="TDRight" width="20%" >
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
                                Total VAT :&nbsp;
                            </td>
                            <td class="TDRight" width="20%" >
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
                               Grand Total Amount :&nbsp;
                            </td>
                            <td class="TDRight" width="20%" >
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
                                <asp:Button ID="bmitButton" runat="server" OnClick="bmitButton_Click" Text="Submit" CssClass="button-padding-right"
                                    OnClientClick="return confirm('Are you sure you want Save ?');" />
                                    
                                    <%--<asp:Button ID="cancelButton" BackColor="#cc6600" runat="server" OnClick="cancelButton_Click"
                                Text="Cancel" />--%>
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
