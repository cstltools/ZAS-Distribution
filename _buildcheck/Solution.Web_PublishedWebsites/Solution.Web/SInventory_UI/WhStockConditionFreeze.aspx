<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_WhStockConditionFreeze, App_Web_dfmjdr5z" %>

<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .margin-right
        {
            margin-right: 7px;
        }
    </style>
    <style type="text/css">
        .divWaiting
        {
            position: absolute;
            z-index: 2147483647 !important;
            opacity: 0.5;
            overflow: hidden;
            text-align: center;
            top: 0;
            left: 0;
            height: 100%;
            width: 100%;
            padding-top: 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            WH Stock Condition Freeze
                        </td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            <asp:UpdateProgress ID="progress" runat="server" ClientIDMode="Static" DisplayAfter="0"
                                DynamicLayout="true">
                                <ProgressTemplate>
                                    <div class="divWaiting">
                                        <asp:Image ID="imgWait" runat="server" ImageAlign="Middle" ImageUrl="~/Images/loading-icon-big.gif"
                                            Height="100%" Width="100%" />
                                    </div>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
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
                            Company Name: <span class="star-mark">* </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="companyNameDropDownList" runat="server" Width="175px" AutoPostBack="True"
                                OnSelectedIndexChanged="companyNameDropDownList_OnSelectedIndexChanged" CssClass="DropDown">
                            </asp:DropDownList>
                        </td>
                        <td class="TDLeft" width="13%">
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                            &nbsp;
                        </td>
                        <td width="13%" style="text-align: right; padding-right: 10px;" class="TDLeft">
                            Warehouse: <span style="color: red">*</span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="warehouseDropDownList" Width="175px" Height="23px" runat="server"
                                CssClass="DropDown">
                            </asp:DropDownList>
                            <asp:HiddenField ID="stockInIdHiddenField" runat="server" />
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                        <td class="TDLeft" width="13%">
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
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
                            <asp:Button ID="submitButton" runat="server" Text="Search" CssClass="margin-right"
                                OnClick="searchButton_Click" BackColor="#16A085" />
                            <asp:Button ID="submitButton0" runat="server" OnClick="submitButton0_OnClick" Text="Submit" />
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
                                <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                                    DataKeyNames="ReceiveId">
                                    <Columns>
                                        <asp:TemplateField HeaderText="#SL">
                                            <ItemTemplate>
                                                <asp:Label ID="LabelSL" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ProductCode" HeaderText="ProductCode" />
                                        <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                        <asp:BoundField DataField="BatchNo" HeaderText="Batch" />
                                        <asp:BoundField DataField="ExpDate" HeaderText="ExpDate" DataFormatString="{0:dd-MMM-yyyy}" />
                                        <asp:BoundField DataField="ReceiveDate" HeaderText="Receive Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                        <asp:BoundField DataField="TotalQuantity" HeaderText="StockInQty" />
                                        <asp:BoundField DataField="StockQty" HeaderText="C.StockQty" />
                                        <%--<asp:BoundField DataField="Amount" HeaderText="Amount" />--%>
                                        <asp:BoundField DataField="StockCondition" HeaderText="Stock" />
                                        <asp:TemplateField HeaderText="Stock Condition">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="StockConditionDropDownList" runat="server" class="form-control"
                                                    Width="90px">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty">
                                            <ItemTemplate>
                                                <asp:TextBox ID="returnQtyTextBox" runat="server" Width="60px" AutoPostBack="True"
                                                    OnTextChanged="returnQtyTextBox_OnTextChanged"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderconvRate" runat="server"
                                                    Enabled="True" TargetControlID="returnQtyTextBox" FilterType="Custom" ValidChars="0123456789">
                                                </cc1:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:TextBox ID="remarksTextBox" runat="server"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" AutoPostBack="True" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
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
