<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_CustomerPaymentProcess, App_Web_htam3fti" %>

<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <style type="text/css">
        .modalBackground {
            background-color: Black;
            filter: alpha(opacity=40);
            opacity: 0.4;
        }

        .modalPopup {
            background-color: #FFFFFF;
            width: 300px;
            border: 3px solid;
        }
    </style>
    <style scoped>
        .example_a {
            color: #fff !important;
            text-transform: uppercase;
            text-decoration: none;
            background: #ed3330;
            padding: 20px;
            border-radius: 5px;
            display: inline-block;
            border: none;
            transition: all 0.4s ease 0s;
        }

            .example_a:hover {
                background: #434343;
                letter-spacing: 1px;
                -webkit-box-shadow: 0px 5px 40px -10px rgba(0,0,0,0.57);
                -moz-box-shadow: 0px 5px 40px -10px rgba(0,0,0,0.57);
                box-shadow: 5px 40px -10px rgba(0,0,0,0.57);
                transition: all 0.4s ease 0s;
            }

        .box {
            margin: auto;
            width: 50%;
            border: 3px solid black;
            padding: 10px;
            text-align: center;
        }

        .currency
        {
            font-size: 18px;
            color: #800000;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading"> Customer Payment process
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                    </tr>

                    <tr>
                        <td class="TDLeft" width="13%">
                           <%-- View --%>
                        </td>
                        <td class="TDRight" width="20%">
                         <%--   <asp:ImageButton ID="cusMasterNewImageButton" runat="server" ImageUrl="~/images/viewList.png"
                                             OnClick="CustMasterNewImageButton_Click" />--%>
                            <asp:HiddenField ID="paymentBatchId" runat="server" />
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>
                    
                    <tr runat="server" Visible="false">
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            <span id="Span1">Company Name:</span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="manufacDropDownList" runat="server" AutoPostBack="True" CssClass="DropDown"
                                OnSelectedIndexChanged="ddlCompany_OnSelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                         <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Distribution Center: <span class="star-mark"> * </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="dcDropDownList" runat="server" CssClass="DropDown" OnSelectedIndexChanged="dcDropDownList_SelectedIndexChanged"
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                    </tr>
                    
                    <tr>
                        
                        <td width="13%"   style="text-align: right; padding-right: 10px; " class="TDLeft">                           
                            <asp:CheckBox ID="customerCheckBox" AutoPostBack="True" OnCheckedChanged="customerCheckBox_OnCheckedChanged" CssClass="pading" runat="server" />
                        </td>

                        <td width="20%" class="TDRight">
                            Is Single Customer
                        </td>
                        

                        
                        <td width="13%" class="TDLeft" runat="server" visible="False" style="text-align: right; padding-right: 10px;">
                            Reference No:
                        </td>
                        <td width="20%" runat="server" visible="False" class="TDRight">
                            <asp:TextBox ID="refNameTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Customer Name:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="totalBalance" Visible="False" Text="" CssClass="balance" runat="server" />
                            <asp:TextBox ID="singleCustomerTextBox" Enabled="False" CssClass="TextBox" AutoPostBack="True" OnTextChanged="singleCustomerTextBox_OnTextChanged" runat="server"></asp:TextBox>
                            <ajaxToolkit:AutoCompleteExtender ID="customerTextBox_AutoCompleteExtender" runat="server"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="10" DelimiterCharacters=""
                                EnableCaching="true" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCustomernew"
                                ServicePath="SInventoryWebService.asmx" ShowOnlyCurrentWordInCompletionListItem="true"
                                TargetControlID="singleCustomerTextBox" >
                            </ajaxToolkit:AutoCompleteExtender>
                            <asp:HiddenField ID="hdCustomerId" runat="server" />
                        </td>
                        
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                    </tr>

                    <tr id="Tr1" > 
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Delivery Man:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="delDropDownList" runat="server" CssClass="DropDown" AutoPostBack="True"
                                OnSelectedIndexChanged="delDropDownList_OnTextChanged">
                            </asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Batch Creation Date:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="dateTextBox" runat="server" CssClass="TextBoxCalander" AutoPostBack="True"
                                OnTextChanged="dateTextBox_TextChanged"></asp:TextBox>
                            <asp:ImageButton ID="imgDate" runat="server" AlternateText="Click to show calendar"
                                ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="4" />
                            <ajaxToolkit:CalendarExtender ID="Date" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgDate"
                                TargetControlID="dateTextBox">
                            </ajaxToolkit:CalendarExtender>
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Batch No:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="batchDropDownList" runat="server" CssClass="DropDown" AutoPostBack="True"
                                OnSelectedIndexChanged="batchDropDownList_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        
                        <td id="Td1" class="TDLeft" runat="server" visible="False" width="13%" style="text-align: right;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            padding-right: 10px;">
                            Customer Name:
                        </td>
                        <td id="Td2" class="TDRight" runat="server" visible="False" width="20%">
                            <asp:DropDownList ID="customerDropDownList" runat="server" AutoPostBack="True" CssClass="DropDown"
                                OnSelectedIndexChanged="customerDropDownList_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td width="13%" style="text-align: right; padding-right: 10px; " class="TDLeft">
                            
                        </td>
                        <td width="20%" class="TDRight">
                            
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" runat="server" visible="False" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Payment Date:
                        </td>
                        <td width="20%" runat="server" visible="False" class="TDRight">
                            <asp:Label ID="paymentDtTextBox" runat="server" AutoPostBack="True" ReadOnly="True"
                                CssClass="TextBoxCalander"></asp:Label>
                        </td>
                        <td width="13%" runat="server" visible="False" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Payment Amount:
                        </td>
                        <td width="20%" runat="server" visible="False" class="TDRight">
                            <asp:Label ID="paymentAmountTextBox" runat="server" CssClass="TextBox" ReadOnly="True"></asp:Label>
                            <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderunitValue" runat="server"
                                Enabled="True" TargetControlID="paymentAmountTextBox" FilterType="Custom" ValidChars="0123456789.">
                            </asp:FilteredTextBoxExtender>--%>
                        </td>
                    </tr>
                    
                   <tr runat="server" Visible="False">
                        
                         <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Payment Type: <span style="color: red">*</span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="payTypeDDL" AutoPostBack="True" OnSelectedIndexChanged="payTypeDDL_OnSelectedIndexChanged" runat="server" CssClass="DropDown">
                            </asp:DropDownList>
                        </td>
                        
                        <td width="13%" style="text-align: right; padding-right: 10px; " class="TDLeft">
                            Bank Account:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="ddlBank" AutoPostBack="True" runat="server" CssClass="DropDown">
                            </asp:DropDownList>
                        </td>
                        <td width="13%" style="text-align: right; padding-right: 10px; " class="TDLeft">
                            Cash Account:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="ddlCash" AutoPostBack="True" runat="server" CssClass="DropDown">
                            </asp:DropDownList>
                        </td>
                    </tr>

                    <tr id="DivCust" runat="server" visible="False">
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                            &nbsp;
                        </td>
                        <td width="13%" class="TDLeft">
                            Customer
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="customerTextBox" runat="server" CssClass="TextBox" AutoPostBack="True"
                                OnTextChanged="customerTextBox_TextChanged"></asp:TextBox>
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;
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
                            <asp:Button ID="Button1" runat="server" Text="Search" CssClass="button-margin-right"
                                OnClick="Button1_Click2" BackColor="#16A085" />
                            <asp:Button ID="saveBatchButton" runat="server" OnClick="saveBatchButton_Click"
                                CssClass="button-margin-right" OnClientClick="return confirm('Are you sure you want to Save ?');"
                                Text="Generate Batch" />
                            <asp:Button ID="Button2" runat="server" OnClick="clearButton_OnClick" Text="Clear"
                                BackColor="#E67E22" />
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
                           <%-- View --%>
                        </td>
                        <td class="TDRight" width="20%">
                         <%--   <asp:ImageButton ID="cusMasterNewImageButton" runat="server" ImageUrl="~/images/viewList.png"
                                             OnClick="CustMasterNewImageButton_Click" />--%>
                            <asp:HiddenField ID="HiddenField1" runat="server" />
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft" colspan="6">
                            <div id="gridContainer1" style="height: auto; overflow: auto; width: 85%; margin: 0 auto;">
                                <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                                    DataKeyNames="CustPayDetailId,CustomerMasterId,CustPayId,IsBatchCreated">
                                    <Columns>

                                        <asp:BoundField DataField="ComUnitName" HeaderText="Unit Name" />
                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                                        <asp:BoundField DataField="InvoiceNo" HeaderText="InvoiceNo" />
                                        <asp:BoundField DataField="DeliveryMan" HeaderText="Delivery Man" />
                                        <asp:BoundField DataField="InvoiceBatch" HeaderText="Invoice Batch" />
                                        <asp:BoundField DataField="PaymentDate" HeaderText="Payment Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                        <asp:BoundField DataField="PaymentAmount" HeaderText="Payment Amount" />
                                        <asp:BoundField DataField="AIT" HeaderText="AIT" />
                                        <asp:BoundField DataField="Discount" HeaderText="Discount" />

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelect_CheckedChanged" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <%--<asp:TemplateField HeaderText="Reject"> 
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkReject" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>

                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="invoiceButton" OnClientClick="return confirm('Are you sure you want to Reject ?')" runat="server" Text="Reject &gt;"
                                                    ForeColor="#EC3D18" OnClick="invoiceButton_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            <span style="font-size: 16px; font-weight: bold;">Total Amount: </span>
                        </td>
                        <td class="TDRight" width="20%">                                                       
                            <asp:Label ID="totalReceiveableLabel" style="font-size: 16px; font-weight: bold; color: #008080; border: 1px solid #008080; padding: 3px;" runat="server"></asp:Label>                       
                        </td>
                        <td class="TDLeft" width="13%"></td>
                        <td class="TDRight" width="20%"></td>

                    </tr>


                   
                    <tr>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:Button ID="submitButton" runat="server" OnClientClick="return confirm('Are you sure you want to Save ?')" CssClass="button-padding-right" OnClick="saveButton_Click" Text="Confirm" />
                            <%--<asp:Button ID="saveButton" runat="server" Enabled="False" OnClick="saveButton_Click"
                                CssClass="button-margin-right" OnClientClick="return confirm('Are you sure you want to Save ?');"
                                Text="Submit" />--%>
                            <asp:Button ID="clearButton" runat="server" OnClick="clearButton_OnClick" Text="Reload"
                                BackColor="#E67E22" />
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight"></td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight">&nbsp;
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

