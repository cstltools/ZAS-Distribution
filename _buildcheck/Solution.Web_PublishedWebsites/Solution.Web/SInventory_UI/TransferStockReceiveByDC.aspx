<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_TransferStockReceiveByDC, App_Web_dfmjdr5z" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
                            </td>
                            <td class="TDRight" width="20%">
                            </td>
                            <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                                Group Name:
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:DropDownList ID="groupDropDownList" Width="180px" runat="server" CssClass="DropDown"
                                    Enabled="False" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td class="TDLeft" width="13%">
                            </td>
                            <td class="TDRight" width="20%">
                            </td>
                        </tr>
                        <tr>
                            <td class="TDLeft" width="13%">
                            </td>
                            <td class="TDRight" width="20%">
                            </td>
                            <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                                Company Name:
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:DropDownList ID="companyNameDropDownList" Width="180px" runat="server" CssClass="DropDown"
                                    OnSelectedIndexChanged="companyNameDropDownList_SelectedIndexChanged" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td class="TDLeft" width="13%">
                            </td>
                            <td class="TDRight" width="20%">
                            </td>
                        </tr>
                        <tr>
                            <td class="TDLeft" width="13%">
                            </td>
                            <td class="TDRight" width="20%">
                            </td>
                            <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                                Distribution Center(DC):  <span class="star-mark"> * </span>
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:DropDownList ID="dcDropDownList" Width="180px" runat="server" CssClass="DropDown">
                                </asp:DropDownList>
                            </td>
                            <td class="TDLeft" width="13%">
                            </td>
                            <td class="TDRight" width="20%">
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
                                &nbsp;
                            </td>
                            <td width="20%" class="TDRight">
                                &nbsp;
                            </td>
                            <td width="13%" class="TDLeft">
                                &nbsp;
                            </td>
                            <td width="20%" class="TDRight">
                                <asp:Button ID="submitButton" runat="server" OnClick="searchButton_Click" Text="Search" />
                                <asp:Button ID="clearButton" CssClass="clearButton" BackColor="#E8BC31" runat="server"
                                    OnClick="clearButton_OnClick" Text="Clear" />
                            </td>
                            <td width="13%" class="TDLeft">
                                &nbsp;
                            </td>
                            <td width="20%" class="TDRight">
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
                            <td width="20%" class="TDRight" colspan="6">
                                <div id="gridContainer1" style="height: auto; overflow: auto; width: 97%; margin: 0 auto;">
                                    <asp:GridView ID="stockInTraGridView" runat="server" AutoGenerateColumns="False"
                                        CssClass="gridview" DataKeyNames="ChalanId">
                                        <Columns>
                                            <asp:BoundField DataField="ChalanNo" HeaderText="ChalanNo" />
                                            <asp:BoundField DataField="ChalanDate" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="ChalanDate" />
                                            <asp:BoundField DataField="TrackNo" HeaderText="TruckNo" />
                                            <asp:BoundField DataField="DriverName" HeaderText="DriverName" />
                                            <asp:HyperLinkField HeaderText="Receive Product" DataNavigateUrlFields="ChalanId"
                                                DataNavigateUrlFormatString="TransferReceiveProductByChalanByDC.aspx?ChalanId={0}"
                                                Text="Receive Product&gt;&gt;" />
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
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" ClientIDMode="Static" DisplayAfter="0"
        DynamicLayout="true">
        <ProgressTemplate>
            <div class="divWaiting">
                <asp:Image ID="imgWait" CssClass="position-set" runat="server" ImageAlign="Middle"
                    ImageUrl="~/images/progress-bar-opt.gif" Height="80" Width="80" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
