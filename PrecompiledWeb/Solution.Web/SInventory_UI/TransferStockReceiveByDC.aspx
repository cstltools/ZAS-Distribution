<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_TransferStockReceiveByDC, App_Web_t3jyjdi4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
             <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            Stock Received By DC
                        </td>
            <tr>
                <td class="TDLeft" width="13%">
                    DC Name :</td>
                <td class="TDRight" width="20%">
                    <asp:DropDownList ID="dcDropDownList" runat="server"  
                        CssClass="DropDown" 
                        >
                    </asp:DropDownList>
                </td>
                <td class="TDLeft" width="13%">
                    <asp:Button ID="searchButton" runat="server" onclick="searchButton_Click" 
                        Text="Search" />
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
                        <tr>
                            <td class="TDLeft" width="13%">
                                &nbsp;</td>
                            <td class="TDRight" width="20%" colspan="4">
                                <asp:GridView ID="stockInTraGridView" runat="server" 
                                    AutoGenerateColumns="False" CssClass="gridview" DataKeyNames="ChalanId">
                                    <Columns>
                                        <asp:BoundField DataField="ChalanNo" HeaderText="ChalanNo" />
                                        <asp:BoundField DataField="ChalanDate" DataFormatString="{0:dd-MMM-yyyy}" 
                                            HeaderText="ChalanDate" />
                                        <asp:BoundField DataField="TrackNo" HeaderText="TruckNo" />
                                        <asp:BoundField DataField="DriverName" HeaderText="DriverName" />
                                        <asp:HyperLinkField HeaderText="Receive Product" DataNavigateUrlFields="ChalanId" 
                                        DataNavigateUrlFormatString="TransferReceiveProductByChalanByDC.aspx?ChalanId={0}"
                                            Text="Receive Product&gt;&gt;" />
                                    </Columns>
                                </asp:GridView>
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

