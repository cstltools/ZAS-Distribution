<%@ Page Title="Product Price Report" Language="C#" MasterPageFile="~/MasterPages/MainMasterPage.master"
    AutoEventWireup="true" CodeFile="WeaightedAVG_Report.aspx.cs" Inherits="SInventory_UI_WeaightedAVG_Report" %>
<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .grid-container {
            height: 400px;
            overflow: auto;
            margin: 0 auto;
        }
        .gridview th, .gridview td {
            text-align: center !important;
        }
        .btnExcel {
            background-color: #1A7343;
            color: #fff !important;
            padding: 4px 10px;
            border-radius: 4px;
            border: none;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" class="TableWorkArea" style="padding-top: 10px !important">
                <tr>
                    <td colspan="6" class="TableHeading" style="margin-bottom:20px;">
                        Product Price Report
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

                <!-- Business Unit -->
                <tr>
                    <td width="13%" class="TDLeft">&nbsp;</td>
                    <td width="20%" class="TDRight">&nbsp;</td>
                    <td width="13%" class="TDLeft" style="text-align: right; padding-right:10px; padding-top:20px;">
                        Business Unit: <span style="color:orangered">*</span>
                    </td>
                    <td width="20%" class="TDRight" style="padding-top:20px;">
                        <asp:DropDownList ID="unitViewDropDownList" runat="server" CssClass="DropDown"
                            AutoPostBack="true" OnSelectedIndexChanged="unitViewDropDownList_OnSelectedIndexChanged"></asp:DropDownList>
                    </td>
                    <td width="13%" class="TDLeft">&nbsp;</td>
                    <td width="20%" class="TDRight">&nbsp;</td>
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

                <!-- Location -->
                <tr style="display:none;">
                    <td class="TDLeft">&nbsp;</td>
                    <td class="TDRight">&nbsp;</td>
                    <td class="TDLeft" style="text-align: right; padding-right:10px;">
                        Location:
                    </td>
                    <td class="TDRight">
                        <asp:DropDownList ID="locationDropDownList" runat="server" CssClass="DropDown"></asp:DropDownList>
                    </td>
                    <td class="TDLeft">&nbsp;</td>
                    <td class="TDRight">&nbsp;</td>
                </tr>

                <!-- Item Group -->
                <tr style="display:none;">
                    <td class="TDLeft">&nbsp;</td>
                    <td class="TDRight">&nbsp;</td>
                    <td class="TDLeft" style="text-align: right; padding-right:10px;">
                        Item:
                    </td>
                    <td class="TDRight">
                        <asp:DropDownList ID="itemGroupDropDownList" runat="server" CssClass="DropDown"></asp:DropDownList>
                    </td>
                    <td class="TDLeft">&nbsp;</td>
                    <td class="TDRight">&nbsp;</td>
                </tr>

                <!-- From Date -->
        
                <tr style="display:none;">
                    <td class="TDLeft">&nbsp;</td>
                    <td class="TDRight">&nbsp;</td>
                    <td class="TDLeft" style="text-align: right; padding-right:10px;">
                        From Date: <span style="color:orangered">*</span>
                    </td>
                    <td class="TDRight">
                        <asp:TextBox ID="tbxFromDate" runat="server" CssClass="TextBoxCalander"></asp:TextBox>
                        <asp:ImageButton ID="imgFromDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                            AlternateText="Click to show calendar" />
                        <ajaxToolkit:CalendarExtender ID="fromDateCalendarExtender" runat="server"
                            Format="dd-MMM-yyyy" TargetControlID="tbxFromDate" PopupButtonID="imgFromDate" />
                    </td>
                    <td class="TDLeft">&nbsp;</td>
                    <td class="TDRight">&nbsp;</td>
                </tr>

                <!-- To Date -->
          
                <tr style="display:none;">
                    <td class="TDLeft">&nbsp;</td>
                    <td class="TDRight">&nbsp;</td>
                    <td class="TDLeft" style="text-align: right; padding-right:10px;">
                        To Date: <span style="color:orangered">*</span>
                    </td>
                    <td class="TDRight">
                        <asp:TextBox ID="tbxToDate" runat="server" CssClass="TextBoxCalander"></asp:TextBox>
                        <asp:ImageButton ID="imgToDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                            AlternateText="Click to show calendar" />
                        <ajaxToolkit:CalendarExtender ID="toDateCalendarExtender" runat="server"
                            Format="dd-MMM-yyyy" TargetControlID="tbxToDate" PopupButtonID="imgToDate" />
                    </td>
                    <td class="TDLeft">&nbsp;</td>
                    <td class="TDRight">&nbsp;</td>
                </tr>

                <!-- Search Button -->
                <tr>
                    <td class="TDLeft">&nbsp;</td>
                    <td class="TDRight">&nbsp;</td>
                    <td class="TDLeft">&nbsp;</td>
                    <td class="TDRight">
                        <asp:Button ID="searchButton" runat="server" Text="Search" CssClass="Button" OnClick="searchButton_Click" />
                    </td>
                    <td class="TDLeft">&nbsp;</td>
                    <td class="TDRight">&nbsp;</td>
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

                <!-- Grid Section -->
                <tr>
                    <td colspan="6" class="TDRight">
                        <div class="grid-container" style="width: 70% !important; margin: 0 auto !important">
                            <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="False"
                                CssClass="gridview" OnPreRender="gv_DocumentUpload_PreRender"
                                OnPageIndexChanging="loadGridView_PageIndexChanging">
                                <Columns>
                                    <asp:BoundField DataField="ProductCode" HeaderText="Product Code" />
                                    <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                    <asp:BoundField DataField="Avg_Rate" HeaderText="Landing Price" />
                                    <asp:BoundField DataField="UnitPrice" HeaderText="TP" />
                                    <asp:BoundField DataField="MRPPrice" HeaderText="MRP" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>

                <!-- Export -->
                <tr>
                    <td class="TDLeft">&nbsp;</td>
                    <td class="TDRight">&nbsp;</td>
                    <td class="TDLeft" style="text-align: right; padding-right:10px;">Export:</td>
                    <td class="TDRight">
                        <asp:LinkButton ID="btnExport" runat="server" CssClass="btnExcel"
                            OnClick="btnExport_Click">
                            <i class="fa fa-file-excel-o"></i>&nbsp;Export to Excel
                        </asp:LinkButton>
                    </td>
                    <td class="TDLeft">&nbsp;</td>
                    <td class="TDRight">&nbsp;</td>
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
        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>

    <!-- Loader -->
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0" DynamicLayout="true">
        <ProgressTemplate>
            <div class="divWaiting">
                <asp:Image ID="imgWait" runat="server" ImageAlign="Middle"
                    ImageUrl="~/images/progress-bar-opt.gif" Height="80" Width="80" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
