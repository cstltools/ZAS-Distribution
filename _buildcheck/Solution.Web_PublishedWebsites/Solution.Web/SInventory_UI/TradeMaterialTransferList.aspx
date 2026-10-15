<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_TradeMaterialTransferList, App_Web_5xlyooqp" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/3.5.4/select2.css" rel="stylesheet" />
    <style type="text/css">
        /* Modern Filter Styles */
        .filter-container {
            display: flex;
            flex-wrap: wrap;
            gap: 16px;
            padding: 20px;
            background-color: #f8fafc;
            border-bottom: 1px solid #e2e8f0;
        }

        .filter-group {
            flex: 1 1 200px;
            display: flex;
            flex-direction: column;
        }

        .filter-group label {
            font-family: Calibri, sans-serif;
            font-size: 13px;
            font-weight: 600;
            color: #475569;
            margin-bottom: 5px;
        }

        .filter-control {
            padding: 6px 12px;
            font-size: 13px;
            font-family: Calibri, sans-serif;
            border: 1px solid #cbd5e1;
            border-radius: 4px;
            background-color: #fff;
            height: 32px;
            box-sizing: border-box;
            color: #1e293b;
        }

        .filter-actions {
            flex: 1 1 100%;
            display: flex;
            justify-content: flex-end;
            gap: 12px;
            margin-top: 10px;
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

        .tmt-btn-search {
            background-color: #0284c7;
            color: #ffffff;
        }

        .tmt-btn-search:hover {
            background-color: #0369a1;
        }

        .tmt-btn-clear {
            background-color: #64748b;
            color: #ffffff;
        }

        .tmt-btn-clear:hover {
            background-color: #475569;
        }

        /* Modern Gridview Styles */
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

        /* Badge Styles */
        .badge {
            display: inline-block;
            padding: 3px 8px;
            font-size: 11px;
            font-weight: bold;
            border-radius: 12px;
            text-transform: uppercase;
        }

        .badge-pending {
            background-color: #fef3c7;
            color: #d97706;
        }

        .badge-approved {
            background-color: #dcfce7;
            color: #16a34a;
        }

        /* Modal Styles */
        .modal-background {
            background-color: Black;
            filter: alpha(opacity=50);
            opacity: 0.5;
            z-index: 10001;
        }

        .modal-panel {
            background-color: #ffffff;
            border: 1px solid #cbd5e1;
            border-radius: 8px;
            width: 90%;
            max-width: 650px;
            padding: 0;
            box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1);
            font-family: Calibri, sans-serif;
            z-index: 10002;
        }

        .modal-header {
            background-color: #8c1b3f;
            color: #ffffff;
            padding: 14px 20px;
            font-size: 15px;
            font-weight: bold;
            border-top-left-radius: 7px;
            border-top-right-radius: 7px;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .modal-body {
            padding: 20px;
            max-height: 450px;
            overflow-y: auto;
        }

        .modal-footer {
            padding: 15px 20px;
            background-color: #f8fafc;
            border-top: 1px solid #e2e8f0;
            display: flex;
            justify-content: flex-end;
            gap: 12px;
            border-bottom-left-radius: 7px;
            border-bottom-right-radius: 7px;
        }

        .details-table {
            width: 100%;
            border-collapse: collapse;
        }

        .details-table td {
            padding: 8px 12px;
            border-bottom: 1px solid #f1f5f9;
            font-size: 13px;
        }

        .details-table td.label-cell {
            font-weight: bold;
            color: #475569;
            width: 35%;
            background-color: #f8fafc;
        }

        .details-table td.value-cell {
            color: #1e293b;
        }

        .header-action-row {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 12px 24px;
            background: #ffffff;
            border-bottom: 1px solid #e2e8f0;
        }

        /* Custom styles to adjust Select2 to match filter styling */
        .select2-container .select2-choice {
            height: 32px !important;
            line-height: 32px !important;
            background-image: none !important;
            background-color: #fff !important;
            border: 1px solid #cbd5e1 !important;
            border-radius: 4px !important;
            color: #1e293b !important;
        }
        .select2-container {
            width: 100% !important;
        }

        /* Fix overlapping and double-borders on Select2 */
        select.tmt-select2 {
            display: none !important;
        }
        .select2-container.filter-control {
            padding: 0 !important;
            height: 32px !important;
            border: none !important;
            background-color: transparent !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea" cellpadding="0" cellspacing="0" style="border-collapse: collapse;">
                    <tr>
                        <td class="TableHeading">
                            Trade Material Transfer List
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 0;">
                            <!-- Header Action Bar -->
                            <div class="header-action-row">
                                <span style="font-size: 14px; font-weight: bold; color: #475569; font-family: Calibri;">Existing Transfers</span>
                                <asp:LinkButton ID="newLinkButton" runat="server" CssClass="tmt-btn-list" style="background-color: #0284c7; color:#fff; text-decoration: none; padding: 6px 16px; border-radius: 4px; font-size:12px; font-weight:bold; font-family:Calibri;" OnClick="newLinkButton_Click">
                                    + Add New Transfer
                                </asp:LinkButton>
                            </div>

                            <!-- Search Filters -->
                            <div class="filter-container">
                                <div class="filter-group">
                                    <label>From Date</label>
                                    <asp:TextBox ID="fromDateTextBox" runat="server" CssClass="filter-control" placeholder="dd-MMM-yyyy"></asp:TextBox>
                                    <cc1:CalendarExtender ID="CalendarExtenderFrom" runat="server" Format="dd-MMM-yyyy" TargetControlID="fromDateTextBox"></cc1:CalendarExtender>
                                </div>
                                <div class="filter-group">
                                    <label>To Date</label>
                                    <asp:TextBox ID="toDateTextBox" runat="server" CssClass="filter-control" placeholder="dd-MMM-yyyy"></asp:TextBox>
                                    <cc1:CalendarExtender ID="CalendarExtenderTo" runat="server" Format="dd-MMM-yyyy" TargetControlID="toDateTextBox"></cc1:CalendarExtender>
                                </div>
                                <div class="filter-group">
                                    <label>From Business Unit</label>
                                    <asp:DropDownList ID="fromBusinessUnitDropDownList" runat="server" CssClass="filter-control tmt-select2"></asp:DropDownList>
                                </div>
                                <div class="filter-group">
                                    <label>To Business Unit</label>
                                    <asp:DropDownList ID="toBusinessUnitDropDownList" runat="server" CssClass="filter-control tmt-select2"></asp:DropDownList>
                                </div>
                                <div class="filter-group">
                                    <label>RM Name</label>
                                    <asp:TextBox ID="rmNameTextBox" runat="server" CssClass="filter-control" placeholder="Enter RM Name"></asp:TextBox>
                                </div>
                                <div class="filter-actions">
                                    <asp:Button ID="searchButton" runat="server" Text="Search" CssClass="tmt-btn tmt-btn-search" OnClick="searchButton_Click" />
                                    <asp:Button ID="clearButton" runat="server" Text="Clear" CssClass="tmt-btn tmt-btn-clear" OnClick="clearButton_Click" />
                                </div>
                            </div>

                            <!-- Grid Container -->
                            <div class="grid-container">
                                <asp:GridView ID="tradeMaterialTransferGridView" runat="server" AutoGenerateColumns="False"
                                    CssClass="modern-grid" DataKeyNames="TradeMaterialTransferId" OnRowCommand="tradeMaterialTransferGridView_RowCommand">
                                    <Columns>
                                        <asp:BoundField DataField="TransferCode" HeaderText="Transfer Code" />
                                        <asp:BoundField DataField="FromCompanyName" HeaderText="From Business Unit" />
                                        <asp:BoundField DataField="FromLocationName" HeaderText="From Location" />
                                        <asp:BoundField DataField="FromProductName" HeaderText="FG Name" />
                                        <asp:BoundField DataField="ToCompanyName" HeaderText="To Business Unit" />
                                        <asp:BoundField DataField="ToLocationName" HeaderText="To Location" />
                                        <asp:BoundField DataField="ToItemDescription" HeaderText="RM Name" />
                                        <asp:BoundField DataField="TransferQty" HeaderText="Qty" ItemStyle-Font-Bold="true" />
                                         <asp:BoundField DataField="EntryBy" HeaderText="Entry By" />
                                         <asp:BoundField DataField="EntryDate" HeaderText="Entry Date" DataFormatString="{0:dd-MMM-yyyy hh:mm tt}" />
                                         <asp:BoundField DataField="UpdateBy" HeaderText="Update By" NullDisplayText="-" />
                                         <asp:BoundField DataField="UpdateDate" HeaderText="Update Date" DataFormatString="{0:dd-MMM-yyyy hh:mm tt}" NullDisplayText="-"/>
                                        
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <span class='<%# "badge " + (Eval("ApprovalStatus").ToString() == "Approved" ? "badge-approved" : "badge-pending") %>'>
                                                    <%# string.IsNullOrEmpty(Eval("ApprovalStatus").ToString()) ? "Pending" : Eval("ApprovalStatus").ToString() %>
                                                </span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Actions" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                                            <ItemTemplate>
                                                <!-- View Details Command -->
                                                <asp:ImageButton ID="detailsImageButton" runat="server" CommandName="ViewDetails"
                                                    CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/images/viewList.png" ToolTip="View Details" style="margin-right: 8px; vertical-align: middle;" />
                                                
                                                <!-- Edit Command (Only allowed if pending) -->
                                                <asp:ImageButton ID="editImageButton" runat="server" CommandName="EditData"
                                                    CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/images/edit.png" ToolTip="Edit"
                                                    Visible='<%# Eval("ApprovalStatus").ToString() != "Approved" %>' style="margin-right: 8px; vertical-align: middle;" />
                                                
                                                <!-- Delete Command (Only allowed if pending) -->
                                                <asp:ImageButton ID="deleteImageButton" runat="server" CommandName="DeleteData"
                                                    CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/images/lineDelete.png" ToolTip="Delete"
                                                    OnClientClick="return confirm('Are you sure you want to delete this transfer?');"
                                                    Visible='<%# Eval("ApprovalStatus").ToString() != "Approved" %>' style="vertical-align: middle;" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div style="padding: 20px; text-align: center; font-style: italic; color: #64748b;">
                                            No transfers found matching search criteria.
                                        </div>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>

            <!-- Details Popup Panel -->
            <asp:Button ID="dummyButton" runat="server" style="display:none;" />
            <asp:Panel ID="detailsPanel" runat="server" CssClass="modal-panel" style="display:none;">
                <div class="modal-header">
                    <span>Trade Material Transfer Details</span>
                    <asp:LinkButton ID="closeLinkButton" runat="server" style="color: white; text-decoration: none; font-weight: bold; font-size: 16px;">&times;</asp:LinkButton>
                </div>
                <div class="modal-body">
                    <asp:HiddenField ID="selectedTmtIdHiddenField" runat="server" />
                    <table class="details-table">
                        <tr>
                            <td class="label-cell">Transfer Code:</td>
                            <td class="value-cell"><asp:Label ID="lblDetailId" runat="server" Font-Bold="true"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="label-cell">From Business Unit:</td>
                            <td class="value-cell"><asp:Label ID="lblDetailFromBU" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="label-cell">From Location:</td>
                            <td class="value-cell"><asp:Label ID="lblDetailFromLoc" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="label-cell">Finished Goods (FG):</td>
                            <td class="value-cell"><asp:Label ID="lblDetailFGName" runat="server" Font-Bold="true" ForeColor="#8c1b3f"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="label-cell">To Business Unit:</td>
                            <td class="value-cell"><asp:Label ID="lblDetailToBU" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="label-cell">To Location:</td>
                            <td class="value-cell"><asp:Label ID="lblDetailToLoc" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="label-cell">Raw Material (RM):</td>
                            <td class="value-cell"><asp:Label ID="lblDetailRMName" runat="server" Font-Bold="true" ForeColor="#0284c7"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="label-cell">Transfer Quantity:</td>
                            <td class="value-cell"><asp:Label ID="lblDetailQty" runat="server" Font-Bold="true" Font-Size="14px"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="label-cell">Remarks:</td>
                            <td class="value-cell"><asp:Label ID="lblDetailRemarks" runat="server" Font-Italic="true"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="label-cell">Entry User & Date:</td>
                            <td class="value-cell">
                                <asp:Label ID="lblDetailEntryBy" runat="server"></asp:Label> on 
                                <asp:Label ID="lblDetailEntryDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="label-cell">Approval Status:</td>
                            <td class="value-cell">
                                <asp:Label ID="lblDetailStatus" runat="server" CssClass="badge"></asp:Label>
                            </td>
                        </tr>
                        <tr id="approvedByRow" runat="server">
                            <td class="label-cell">Approved By & Date:</td>
                            <td class="value-cell">
                                <asp:Label ID="lblDetailApprovedBy" runat="server"></asp:Label> on 
                                <asp:Label ID="lblDetailApprovedDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="closeButton" runat="server" Text="Close" CssClass="tmt-btn tmt-btn-clear" />
                </div>
            </asp:Panel>

            <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server"
                TargetControlID="dummyButton"
                PopupControlID="detailsPanel"
                CancelControlID="closeButton"
                OkControlID="closeLinkButton"
                BackgroundCssClass="modal-background">
            </cc1:ModalPopupExtender>

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

    <!-- Select2 Script Initialization -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/3.5.4/select2.min.js"></script>
    <script type="text/javascript">
        function initListSelect2() {
            $(".tmt-select2").select2({
                placeholder: "Select from list",
                allowClear: true
            });
        }

        $(document).ready(function () {
            initListSelect2();
        });

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            initListSelect2();
        });
    </script>
</asp:Content>
