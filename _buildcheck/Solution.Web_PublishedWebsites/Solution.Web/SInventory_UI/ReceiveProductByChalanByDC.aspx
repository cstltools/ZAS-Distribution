<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_ReceiveProductByChalanByDC, App_Web_clqxceh0" %>

<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            DC Stock Receive
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
                            <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                                Chalan No:
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:Label ID="clnNoTextBox" runat="server" ReadOnly="True"></asp:Label>
                            </td>
                            <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                                Chalan Date :
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:Label ID="clnDateTextBox" runat="server" ReadOnly="True"></asp:Label>
                            </td>
                            <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                                Receive Date :
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:Label ID="rcvDateTextBox" runat="server" ReadOnly="True"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                                Truck No:
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:Label ID="truckTextBox" runat="server" ReadOnly="True"></asp:Label>
                            </td>
                            <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                                Driver Name :
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:Label ID="driverNameTextBox" runat="server" ReadOnly="True"></asp:Label>
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
                                    
                                    <asp:GridView ID="rcvGridView" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                                    DataKeyNames="ReqId,ReqChildId,StockInTransfarId">
                                    <Columns>
                                        <asp:BoundField DataField="ProductCode" HeaderText="Product Code" />
                                        <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                        <asp:BoundField DataField="PackSize" HeaderText="Pack Size" />
                                        <asp:BoundField DataField="BatchNo" HeaderText="Batch No" />
                                        <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                                        <asp:BoundField DataField="ExpDate" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Exp. Date" />
                                        <asp:BoundField DataField="ReceiveDate" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Receive Date" />
                                        <asp:TemplateField HeaderText="Rcv. Qty">
                                            <ItemTemplate>
                                                <asp:TextBox ID="rcvQtyTextBox" runat="server" Text='<%# Eval("Quantity")%>' ReadOnly="True"
                                                    AutoPostBack="True" OnTextChanged="rcvQtyTextBox_TextChanged"></asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="currentStockTextBox" runat="server" TargetControlID="rcvQtyTextBox"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="UnRcvQty">
                                            <ItemTemplate>
                                                <asp:TextBox ID="damageTextBox" runat="server" AutoPostBack="True" ReadOnly="True" OnTextChanged="damageTextBox_TextChanged">0</asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="fcurrentStockTextBox" runat="server" TargetControlID="damageTextBox"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="MfgDate" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Mfg. date" />
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
                                <asp:LinkButton ID="backLinkButton" runat="server" Font-Bold="True" OnClick="backLinkButton_Click">&lt;&lt;&lt;&lt;&lt;Back To List</asp:LinkButton>
                            </td>
                            <td class="TDLeft" width="13%">
                                &nbsp;
                            </td>
                            <td class="TDRight" width="20%">
                                <%-- <asp:Button ID="submitButton" runat="server" onclick="submitButton_Click" 
                                Text="Submit" />--%>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="saveButton" runat="server" Text="Submit" OnClientClick="return confirm('Are you sure you want to Save ?');"
                                            OnClick="submitButton_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel3"
                                    DisplayAfter="0" DynamicLayout="true">
                                    <ProgressTemplate>
                                        <center>
                                            <asp:Image ID="Img2" runat="server" ImageUrl="~/Images/ajax-loader.gif" />
                                        </center>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
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
                                <asp:HiddenField ID="hdComUnitId" runat="server" />
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
