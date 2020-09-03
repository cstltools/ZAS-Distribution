<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MainMasterPage.master"
    AutoEventWireup="true" CodeFile="WhFreezeStockRelease.aspx.cs" Inherits="SInventory_UI_WhFreezeStockRelease" %>

<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <style type="text/css">
        .margin-right
        {
            margin-right: 7px;
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
                            Wh Freeze Stock Release
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
                        <tr>
                        <td class="TDLeft" width="13%">
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Company Name: <span class="star-mark"> * </span> </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="companyNameDropDownList" runat="server" Width="175px" AutoPostBack="True" OnSelectedIndexChanged="companyNameDropDownList_OnSelectedIndexChanged"
                                CssClass="DropDown" >
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
                                
                                <asp:Button ID="submitButton" runat="server" Text="Search" CssClass="margin-right" OnClick="searchButton_Click" BackColor="#16A085" />

                                 <asp:Button ID="submitButton0" runat="server" OnClick="submitButton_OnClick"  Text="Submit" />                                
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
                                    DataKeyNames="WhStoreFreezeId,ReceiveId">
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
                                        <asp:BoundField DataField="StockQty" HeaderText="StockQty" />
                                        <%--<asp:BoundField DataField="Amount" HeaderText="Amount" />--%>
                                        <asp:BoundField DataField="StockCondition" HeaderText="StockCondition" />
                                        <asp:TemplateField HeaderText="Return Qty">
                                            <ItemTemplate>
                                                <asp:TextBox ID="returnQtyTextBox" runat="server" Width="80px" AutoPostBack="True" OnTextChanged="returnQtyTextBox_OnTextChanged"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderconvRate" runat="server"
                                                    Enabled="True" TargetControlID="returnQtyTextBox" FilterType="Custom" ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
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
    
     <asp:UpdateProgress ID="UpdateProgress1" runat="server" ClientIDMode="Static" DisplayAfter="0" DynamicLayout="true">
        <ProgressTemplate>
            <div class="divWaiting">
                <asp:Image ID="imgWait" CssClass="position-set" runat="server" ImageAlign="Middle" ImageUrl="~/images/progress-bar-opt.gif"
                    Height="80" Width="80" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


</asp:Content>
