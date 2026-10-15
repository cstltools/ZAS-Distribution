<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_TradeMaterialTransfer, App_Web_pkilq3oa" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/3.5.4/select2.css" rel="stylesheet" />
    <style type="text/css">
        .autocomplete_completionListElement {
            margin: 0px !important;
            background-color: White;
            color: windowtext;
            border: buttonshadow;
            border-width: 1px;
            border-style: solid;
            cursor: default;
            overflow: auto;
            font-family: Calibri;
            font-size: 12px;
            text-align: left;
            list-style-type: none;
            padding-left: 0px;
            max-height: 350px;
            width: 40% !important;
            box-shadow: 0 4px 6px -1px rgba(0,0,0,0.1);
            z-index: 10000;
        }

        .autocomplete_highlightedListItem {
            background-color: #0284c7 !important;
            color: white !important;
            padding: 4px;
        }

        .autocomplete_listItem {
            background-color: white;
            color: #334155;
            padding: 4px;
            border-bottom: 1px solid #f1f5f9;
        }

        /* Modern UI Styles */
        .tmt-container {
            display: flex;
            flex-wrap: wrap;
            gap: 24px;
            padding: 24px;
            background-color: #f8fafc;
            justify-content: center;
        }

        .tmt-card {
            flex: 1 1 450px;
            max-width: 550px;
            background: #ffffff;
            border: 1px solid #e2e8f0;
            border-radius: 8px;
            padding: 24px;
            box-shadow: 0 1px 3px 0 rgba(0, 0, 0, 0.05), 0 1px 2px 0 rgba(0, 0, 0, 0.03);
        }

        .tmt-card-header {
            font-size: 16px;
            font-weight: 600;
            color: #8c1b3f;
            border-bottom: 2px solid #f1f5f9;
            padding-bottom: 12px;
            margin-bottom: 20px;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }

        .tmt-form-group {
            margin-bottom: 18px;
        }

        .tmt-form-group label {
            display: block;
            font-family: Calibri, sans-serif;
            font-size: 13px;
            font-weight: 600;
            color: #475569;
            margin-bottom: 6px;
        }

        .tmt-form-group .required-star {
            color: #ef4444;
            margin-left: 2px;
            font-weight: bold;
        }

        .tmt-control {
            width: 100% !important;
            padding: 6px 12px;
            font-size: 13px;
            font-family: Calibri, sans-serif;
            border: 1px solid #cbd5e1;
            border-radius: 4px;
            background-color: #fff;
            box-sizing: border-box;
            height: 34px;
            color: #1e293b;
            transition: border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
        }

        .tmt-control:focus {
            outline: none;
            border-color: #8c1b3f;
            box-shadow: 0 0 0 2px rgba(140, 27, 63, 0.15);
        }

        textarea.tmt-control {
            height: 75px !important;
            resize: vertical;
        }

        .tmt-actions {
            width: 100%;
            display: flex;
            justify-content: center;
            gap: 16px;
            padding: 20px 0 10px 0;
            border-top: 1px solid #e2e8f0;
            margin-top: 10px;
        }

        .tmt-btn {
            padding: 8px 32px;
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

        .tmt-btn-cancel {
            background-color: #ea580c;
            color: #ffffff;
        }

        .tmt-btn-cancel:hover {
            background-color: #c2410c;
        }

        .tmt-btn-list {
            background-color: #0f172a;
            color: #ffffff;
            padding: 6px 16px;
            font-size: 12px;
            border-radius: 4px;
            text-decoration: none;
            display: inline-block;
        }

        .tmt-btn-list:hover {
            background-color: #334155;
            color: #ffffff;
        }

        .tmt-header-row {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 12px 24px;
            background: #ffffff;
            border-bottom: 1px solid #e2e8f0;
        }

        /* Custom styles to adjust Select2 to match tmt styling */
        .select2-container .select2-choice {
            height: 34px !important;
            line-height: 34px !important;
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
        .select2-container.tmt-control {
            padding: 0 !important;
            height: 34px !important;
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
                            Trade Material Transfer
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 0;">
                            <!-- Header Row with List Button -->
                            <div class="tmt-header-row">
                                <span style="font-size: 14px; font-weight: bold; color: #475569; font-family: Calibri;">Manage Stock Transfers</span>
                                <asp:Button ID="listButton" runat="server" Text="Transfer List" CssClass="tmt-btn-list" style="border: none; cursor: pointer;" OnClick="listButton_Click" />
                            </div>
                            
                            <!-- Main Flex Container for Cards -->
                            <div class="tmt-container">
                                <asp:HiddenField ID="tradeMaterialTransferIdHiddenField" runat="server" />
                                <asp:HiddenField ID="rmIdHiddenField" runat="server" />

                                <!-- From Side Card -->
                                <div class="tmt-card">
                                    <div class="tmt-card-header">From Side (FG Issue)</div>
                                    
                                    <div class="tmt-form-group">
                                        <label>Business Unit <span class="required-star">*</span></label>
                                        <asp:DropDownList ID="fromBusinessUnitDropDownList" runat="server" CssClass="tmt-control tmt-select2"
                                            AutoPostBack="True" OnSelectedIndexChanged="fromBusinessUnitDropDownList_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                    
                                    <div class="tmt-form-group">
                                        <label>Location <span class="required-star">*</span></label>
                                        <asp:DropDownList ID="fromLocationDropDownList" runat="server" CssClass="tmt-control tmt-select2"
                                            AutoPostBack="True" OnSelectedIndexChanged="fromLocationDropDownList_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                    
                                    <div class="tmt-form-group">
                                        <label>Finished Goods Name <span class="required-star">*</span></label>
                                        <asp:DropDownList ID="fgDropDownList" runat="server" CssClass="tmt-control tmt-select2"
                                            AutoPostBack="True" OnSelectedIndexChanged="fgDropDownList_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                    
                                    <div class="tmt-form-group">
                                        <label>Current Stock Qty</label>
                                        <asp:TextBox ID="fromCurrentStockQtyTextBox" runat="server" CssClass="tmt-control" ReadOnly="True" style="background-color: #f1f5f9; color: #475569; font-weight: bold;"></asp:TextBox>
                                    </div>
                                    
                                    <div class="tmt-form-group">
                                        <label>Transfer Qty <span class="required-star">*</span></label>
                                        <asp:TextBox ID="transferQtyTextBox" runat="server" CssClass="tmt-control" placeholder="Enter transfer quantity"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderQty" runat="server"
                                            Enabled="True" TargetControlID="transferQtyTextBox" FilterType="Custom" ValidChars="0123456789.">
                                        </cc1:FilteredTextBoxExtender>
                                    </div>
                                    
                                    <div class="tmt-form-group">
                                        <label>Remarks</label>
                                        <asp:TextBox ID="remarksTextBox" runat="server" CssClass="tmt-control" TextMode="MultiLine" Rows="3" placeholder="Add remarks..."></asp:TextBox>
                                    </div>
                                </div>

                                <!-- To Side Card -->
                                <div class="tmt-card">
                                    <div class="tmt-card-header">To Side (RM Receive)</div>
                                    
                                    <div class="tmt-form-group">
                                        <label>Business Unit <span class="required-star">*</span></label>
                                        <asp:DropDownList ID="toBusinessUnitDropDownList" runat="server" CssClass="tmt-control tmt-select2"
                                            AutoPostBack="True" OnSelectedIndexChanged="toBusinessUnitDropDownList_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                    
                                    <div class="tmt-form-group">
                                        <label>Location <span class="required-star">*</span></label>
                                        <asp:DropDownList ID="toLocationDropDownList" runat="server" CssClass="tmt-control tmt-select2"
                                            AutoPostBack="True" OnSelectedIndexChanged="toLocationDropDownList_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                    
                                    <div class="tmt-form-group">
                                        <label>Raw Material Name <span class="required-star">*</span></label>
                                        <asp:TextBox ID="rmNameTextBox" runat="server" CssClass="tmt-control" AutoPostBack="True" placeholder="Search and select raw material"
                                            OnTextChanged="rmNameTextBox_TextChanged"></asp:TextBox>
                                        <cc1:AutoCompleteExtender ID="rmNameTextBox_AutoCompleteExtender" runat="server"
                                            CompletionListCssClass="autocomplete_completionListElement"
                                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="12"
                                            DelimiterCharacters="" EnableCaching="true" Enabled="True" MinimumPrefixLength="1"
                                            ServiceMethod="GetRawMaterial" ShowOnlyCurrentWordInCompletionListItem="true"
                                            TargetControlID="rmNameTextBox">
                                        </cc1:AutoCompleteExtender>
                                    </div>
                                </div>
                                
                                <!-- Action Buttons -->
                                <div class="tmt-actions">
                                    <asp:Button ID="submitButton" CssClass="tmt-btn tmt-btn-submit" runat="server"
                                        OnClick="submitButton_Click" OnClientClick="return confirm('Are you sure you want to Save ?');"
                                        Text="Submit" />
                                    <asp:Button ID="cancelButton" CssClass="tmt-btn tmt-btn-cancel" runat="server"
                                        OnClick="cancelButton_Click" Text="Cancel" />
                                </div>
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

    <!-- Select2 Script Initialization -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/3.5.4/select2.min.js"></script>
    <script type="text/javascript">
        function initTmtSelect2() {
            $(".tmt-select2").select2({
                placeholder: "Select from list",
                allowClear: true
            });
        }

        $(document).ready(function () {
            initTmtSelect2();
        });

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            initTmtSelect2();
        });
    </script>
</asp:Content>
