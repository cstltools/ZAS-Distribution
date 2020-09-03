<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MainMasterPage.master" AutoEventWireup="true" CodeFile="StockReceiveReportByDc.aspx.cs" Inherits="SInventory_UI_StockReceiveReportByDc" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            DC Stock Receive Report
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
                                Distribution Center(DC): <span class="star-mark">* </span>
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
                             <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                                From Date: <span class="star-mark">* </span>
                            </td>
                            <td width="20%" class="TDRight">
                                <asp:TextBox ID="fromDateTextBox" runat="server" Width="145px" CssClass="TextBoxCalander"></asp:TextBox>
                                <asp:ImageButton ID="imgDate" runat="server" AlternateText="Click to show calendar"
                                    ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="4" />
                                <asp:CalendarExtender ID="Date" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgDate"
                                    TargetControlID="fromDateTextBox">
                                </asp:CalendarExtender>
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
                            <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                                To Date: <span class="star-mark">* </span>
                            </td>
                            <td width="20%" class="TDRight">
                                <asp:TextBox ID="toDateTextBox" runat="server" Width="145px" CssClass="TextBoxCalander"></asp:TextBox>
                                <asp:ImageButton ID="ImageButton1" runat="server" AlternateText="Click to show calendar"
                                    ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="5" />
                                <asp:CalendarExtender ID="CalendarExtender12" runat="server" Format="dd-MMM-yyyy"
                                    PopupButtonID="ImageButton1" TargetControlID="toDateTextBox">
                                </asp:CalendarExtender>
                            </td>
                            <td width="13%" class="TDLeft">
                            </td>
                            <td width="20%" class="TDRight">
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
                                        CssClass="gridview" DataKeyNames="ReqId">
                                        <Columns>
                                            <asp:BoundField DataField="ReqNo" HeaderText="Req. No" />
                                            <asp:BoundField DataField="ReqDate" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Req. Date" />
                                            <asp:BoundField DataField="ComUnitName" HeaderText="Unit" />
                                            <asp:BoundField DataField="IssueChalanNo" HeaderText="ChalanNo" />
                                            <asp:BoundField DataField="IssuChalanDate" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Chalan Date" />
                                            <asp:BoundField DataField="PickingNo" HeaderText="PickingNo" />
                                            <asp:BoundField DataField="PickingDate" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Picking Date" />
                                            <asp:BoundField DataField="TruckNo" HeaderText="Truck No" />
                                            <asp:BoundField DataField="DriverName" HeaderText="DriverName" />
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="invoiceButton" runat="server" Text="Preview Report &gt; &gt; &gt;"
                                                        ForeColor="#DE751F" OnClick="invoiceButton_Click" />
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

