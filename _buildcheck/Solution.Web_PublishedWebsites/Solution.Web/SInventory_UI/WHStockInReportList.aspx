<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_WHStockInReportList, App_Web_clqxceh0" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .mg-lft
        {
            margin-left: 20px;
            color: #006666;
        }
        
        .button-padding-right {
            margin-right: 5px;
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
                            Warehouse Stock Report
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                            <%--<asp:ImageButton ID="AddImageButton" runat="server" ImageUrl="~/images/Add.png" OnClick="AddImageButton_Click" />--%>
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                            <%-- <asp:ImageButton ID="ReloadImageButton" runat="server" ImageUrl="~/images/refresh.png"
                                OnClick="ReloadImageButton_Click" />--%>
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
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" style="text-align: right; padding-right: 10px;" class="TDLeft">
                            Report Type:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="reportDropDownList" AutoPostBack="True" OnTextChanged="reportDropDownList_OnTextChanged" CssClass="DropDown" Height="23px" runat="server">
                                <asp:ListItem> --- Select --- </asp:ListItem>
                                <asp:ListItem Value="STD"> StockIn Detail Report </asp:ListItem>
                                <asp:ListItem Value="CSR"> Current Stock Report </asp:ListItem>
                                <asp:ListItem Value="STI"> Stock In Report </asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                    </tr>
                    <tr id="fromDate" runat="server" visible="False">
                        <td class="TDLeft" width="13%">
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                         <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            (Stock In) From Date: <span class="star-mark"> * </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="fromDateTextBox" runat="server" CssClass="TextBoxCalander"></asp:TextBox>
                            <asp:ImageButton runat="server" AlternateText="Click to show calendar" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                TabIndex="4" ID="ImageButton1"></asp:ImageButton>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy" TargetControlID="fromDateTextBox"
                                PopupButtonID="ImageButton1">
                            </asp:CalendarExtender>
                        </td>
                        <td class="TDLeft" width="13%">
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                    </tr>
                    <tr id="toDate" runat="server" visible="False">
                        <td class="TDLeft" width="13%">
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            (Stock In) To Date: <span class="star-mark"> * </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="toDateTextBox" runat="server" CssClass="TextBoxCalander"></asp:TextBox>
                            <asp:ImageButton runat="server" AlternateText="Click to show calendar" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                TabIndex="4" ID="ImageButton2"></asp:ImageButton>
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="toDateTextBox" PopupButtonID="ImageButton2">
                            </asp:CalendarExtender>
                        </td>
                        <td class="TDLeft" width="13%">
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                    </tr>
                    <tr id="stockInDate" runat="server" visible="False">
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" style="text-align: right; padding-right: 10px;" class="TDLeft">
                            Stock In Date:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="stockInDateTextBox" Height="23px" Width="140px" runat="server" CssClass="TextBoxCalander"></asp:TextBox>
                            <asp:ImageButton runat="server" AlternateText="Click to show calendar" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                TabIndex="4" ID="imgDate"></asp:ImageButton>
                            <asp:CalendarExtender ID="Date" runat="server" Format="dd-MMM-yyyy" TargetControlID="stockInDateTextBox"
                                PopupButtonID="imgDate">
                            </asp:CalendarExtender>
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                    </tr>
                    <tr id="productName" runat="server" visible="False">
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" style="text-align: right; padding-right: 10px;" class="TDLeft">
                            Product:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="productDropDownList" Height="23px" CssClass="DropDown" runat="server">
                            </asp:DropDownList>
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
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                            <br />
                            <asp:Button ID="searchButton"  runat="server" CssClass="button-padding-right" Visible="False" Text="Search" OnClick="searchButton_Click" />
                            <asp:Button ID="reportButton" runat="server" CssClass="button-padding-right" BackColor="#cc6600" Text="View Report" OnClick="reportButton_OnClick" />
                            <asp:Button ID="excelButton" runat="server" BackColor="#006666" Text="Excel" OnClick="excelButton_OnClick" />
                        </td>
                        <td width="13%" class="TDLeft">
                            
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td class="TDRight" colspan="4">
                            <div id="gridContainer1" style="height: auto; overflow: auto; width: 97%; margin: 0 auto;">
                                &nbsp;<asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False"
                                    CssClass="gridview" DataKeyNames="WHStockInMasterID">
                                    <Columns>
                                        <asp:BoundField DataField="WHStockInCode" HeaderText="Code" />
                                        <asp:BoundField DataField="ChallanNo" HeaderText="Challan No" />
                                        <asp:BoundField DataField="ChallanDate" HeaderText="Challan Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                        <asp:BoundField DataField="TotalQuantity" HeaderText="Total Qty" />
                                        <asp:BoundField DataField="TotalValue" HeaderText="Total Amt" />
                                        <asp:TemplateField HeaderText="Report">
                                            <ItemTemplate>
                                               <%-- <asp:LinkButton ID="printButton" OnClick="printButton_Click" CssClass="mg-lft" runat="server"> View </asp:LinkButton>--%>
                                                  <asp:ImageButton ID="printButton" runat="server" 
                                                  OnClick="printButton_Click"  ImageUrl="../images/image/if_paste-clipboard-copy_2931174.png"/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
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
</asp:Content>
