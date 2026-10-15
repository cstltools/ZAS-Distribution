<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_TradeMaterialTransferApproval, App_Web_clqxceh0" %>

<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .inline-rb td {
            padding-right: 20px;
        }
        
        .tmt-btn {
            padding: 6px 20px;
            font-size: 13px;
            font-weight: bold;
            font-family: Calibri, sans-serif;
            border-radius: 4px;
            cursor: pointer;
            border: none;
            transition: background-color 0.2s ease;
        }

        .tmt-btn-submit {
            background-color: #0284c7;
            color: #ffffff;
        }

        .tmt-btn-submit:hover {
            background-color: #0369a1;
        }
        
        .grid-container {
            padding: 20px;
            background: #ffffff;
        }

        .modern-grid {
            width: 100%;
            border-collapse: collapse;
            font-family: Calibri, sans-serif;
            font-size: 13px;
            color: #334155;
        }

        .modern-grid th {
            background-color: #8c1b3f;
            color: #ffffff;
            font-weight: bold;
            padding: 10px 12px;
            text-align: left;
            border-bottom: 2px solid #e2e8f0;
        }

        .modern-grid td {
            padding: 10px 12px;
            border-bottom: 1px solid #e2e8f0;
        }

        .modern-grid tr:hover {
            background-color: #f8fafc;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea" cellpadding="0" cellspacing="0" style="border-collapse: collapse;">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            Trade Material Transfer Approval
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
                        </td>
                        <td width="20%" class="TDRight">
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
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" class="TDLeft" style="font-family: Calibri; font-weight: bold; font-size: 14px; text-align: right; padding-right: 15px;">
                            Take Action :
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:RadioButtonList ID="actionRadioButtonList" runat="server" CssClass="inline-rb" RepeatDirection="Horizontal" Font-Names="Calibri" Font-Size="13px">
                                <asp:ListItem Selected="True" Value="Approved">Approve</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit Action" CssClass="tmt-btn tmt-btn-submit" OnClientClick="return confirm('Are you sure you want to approve the selected transfers?');" />
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
                        <td width="13%" class="TDLeft" colspan="6">
                            <div class="grid-container">
                                <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False" CssClass="modern-grid"
                                    DataKeyNames="TradeMaterialTransferId">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TransferCode" HeaderText="Transfer Code" />
                                        <asp:BoundField DataField="FromCompanyName" HeaderText="From BU" />
                                        <asp:BoundField DataField="FromLocationName" HeaderText="From Location" />
                                        <asp:BoundField DataField="FromProductName" HeaderText="FG Name" />
                                        <asp:BoundField DataField="ToCompanyName" HeaderText="To BU" />
                                        <asp:BoundField DataField="ToLocationName" HeaderText="To Location" />
                                        <asp:BoundField DataField="ToItemDescription" HeaderText="RM Name" />
                                        <asp:BoundField DataField="TransferQty" HeaderText="Qty" ItemStyle-Font-Bold="true" />
                                        <asp:BoundField DataField="EntryBy" HeaderText="Entry By" />
                                        <asp:BoundField DataField="EntryDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                        <asp:BoundField DataField="ApprovalStatus" HeaderText="Status" NullDisplayText="Pending" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div style="padding: 20px; text-align: center; font-style: italic; color: #64748b; font-family: Calibri; font-size: 14px;">
                                            No pending trade material transfers for approval.
                                        </div>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
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
