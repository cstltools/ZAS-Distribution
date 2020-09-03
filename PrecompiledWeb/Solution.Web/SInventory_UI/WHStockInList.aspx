<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_WHStockInList, App_Web_uja4wjh3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            WH StockIn List</td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight" colspan="4">
                            <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False" 
                                CssClass="gridview" DataKeyNames="WHStockInMasterID" 
                                onrowcommand="loadGridView_RowCommand" >
                                <Columns>
                                    <asp:BoundField DataField="WHStockInCode" HeaderText="Code" />
                                    <asp:BoundField DataField="ApproveDate" HeaderText="StockIn Date" DataFormatString="{0:dd-MMM-yyyy}"/>
                                    <asp:BoundField DataField="ChallanNo" HeaderText="Challan No" />
                                    <asp:BoundField DataField="ChallanDate" HeaderText="Challan Date" DataFormatString="{0:dd-MMM-yyyy}"/>
                                    <asp:BoundField DataField="TotalQuantity" HeaderText="TotalQty" />
                                    <asp:BoundField DataField="TotalVat" HeaderText="TotalVat" />
                                    <asp:BoundField DataField="TotalValue" HeaderText="TotalAmount" />
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="HyperLink1" runat="server"  OnClick="HyperLink1_OnClick" >Go&gt;&gt;&gt;&gt;&gt;</asp:LinkButton>
                                            <%--<asp:LinkButton ID="LinkButton1" runat="server">LinkButton</asp:LinkButton>--%>
                                            <%--<asp:ImageButton ID="editImageButton" runat="server" 
                                                CommandArgument="<%# Container.DataItemIndex %>" 
                                                ImageUrl="~/images/edit.png" />--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" class="TDLeft">
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
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                            &nbsp;
                        </td>
                        <td width="13%" class="TDLeft" >
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
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

