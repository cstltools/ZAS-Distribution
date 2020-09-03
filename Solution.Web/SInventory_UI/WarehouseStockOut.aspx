<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MainMasterPage.master"
    AutoEventWireup="true" CodeFile="WarehouseStockOut.aspx.cs" Inherits="SInventory_UI_WarehouseStockIn" %>

<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            Warehouse Stock Out
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" style="text-align: right; padding-right: 10px;" class="TDRight">
                            <%--<asp:LinkButton ID="viewLinkButton" CssClass="btn buttons smallBtn" ForeColor="green"
                                OnClick="viewLinkButton_OnClick" runat="server">View Details</asp:LinkButton>--%>
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                    </tr>
                    <tr>
                        
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Company Name: <span class="star-mark"> * </span> </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="companyNameDropDownList" Enabled="False" runat="server" AutoPostBack="True" OnSelectedIndexChanged="companyNameDropDownList_OnSelectedIndexChanged"
                                CssClass="DropDown" >
                            </asp:DropDownList>
                        </td>

                        <td width="13%" style="text-align: right; padding-right: 10px;" class="TDLeft">
                            Warehouse: <span style="color: red">*</span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="warehouseDropDownList" Width="175px" Height="23px" runat="server" Enabled="False"
                                CssClass="DropDown">
                            </asp:DropDownList>
                            <asp:HiddenField ID="masterIdHiddenField" runat="server" />
                        </td>
                        
                        <td width="13%" style="text-align: right; padding-right: 10px;" class="TDLeft">
                            Stock In Date: 
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="stockInDateTextBox" Width="150px" Height="23px" ReadOnly="True" runat="server" CssClass="TextBoxCalander" AutoPostBack="True" OnTextChanged="stockInDateTextBox_OnTextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" AlternateText="Click to show calendar" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                TabIndex="4" ID="imgDate"></asp:ImageButton>
                            <cc1:CalendarExtender ID="manufacturerDate1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="stockInDateTextBox" PopupButtonID="imgDate">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>
                    
                    <tr>
                        
                        <td width="13%" style="text-align: right; padding-right: 10px;" class="TDLeft">
                            Challan No: 
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="challanNoTextBox" Height="23px" runat="server"></asp:TextBox>
                        </td>
                        <td class="TDLeft" style="text-align: right; padding-right: 10px;" width="13%">
                            Challan Date: 
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="challanDateTextBox" Width="150px" Height="23px" runat="server" CssClass="TextBoxCalander" AutoPostBack="True" OnTextChanged="challanDateTextBox_OnTextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" AlternateText="Click to show calendar" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                TabIndex="4" ID="ImageButton4"></asp:ImageButton>
                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="challanDateTextBox" PopupButtonID="ImageButton4">
                            </cc1:CalendarExtender>
                        </td>

                        <td class="TDLeft" style="text-align: right; padding-right: 10px;" width="13%">
                            Reference No:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="referenceNoTextBox" Height="23px" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    
                    <tr>
                    
                     <td class="TDLeft" style="text-align: right; padding-right: 10px;" width="13%">
                            Reference Date:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="referenceDateTextBox" Width="150px" Height="23px" runat="server"
                                CssClass="TextBoxCalander" AutoPostBack="True" OnTextChanged="referenceDateTextBox_OnTextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" AlternateText="Click to show calendar" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                TabIndex="4" ID="ImageButton5"></asp:ImageButton>
                            <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="referenceDateTextBox" PopupButtonID="ImageButton5">
                            </cc1:CalendarExtender>
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;" width="13%">
                            Reason<span style="color: red">*</span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="reasonTextBox" TextMode="MultiLine" Rows="2" Columns="15" runat="server"></asp:TextBox>
                        </td>
                       
                        <td class="TDLeft" width="13%">
                            &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;
                        </td>
                    </tr>
                    <tr runat="server" Visible="False">
                        <td class="TDLeft" width="13%">
                            &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;
                        </td>
                        <td class="TDLeft" style="text-align: right; padding-right: 10px;" width="13%">
                            Remarks:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="remarksTextBox" Height="20px" runat="server"></asp:TextBox>
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

                        <td class="TDRight" width="20%" colspan="6">
                             <div id="gridContainer1" style="height: auto; overflow: auto; width: 97%; margin: 0 auto;">
                                 <asp:GridView ID="productGridView" runat="server" AutoGenerateColumns="False" 
                                CssClass="gridview" DataKeyNames="WHStockInDetailID,ReceiveId">
                                <Columns>
                                    <asp:BoundField DataField="SL" HeaderText="SL" />
                                    <asp:TemplateField HeaderText="Code">
                                        <ItemTemplate>
                                            <asp:TextBox ID="productCodeTextBox" runat="server" Height="23px" Width="50px" CssClass="TextBoxCalander"
                                                AutoPostBack="True" ToolTip="true" OnTextChanged="productCodeTextBox_TextChanged"
                                                Text='<%# Eval("ProductCode")%>'></asp:TextBox>
                                            <asp:HiddenField ID="productidHiddenField" Value='<%# Eval("ProductId")%>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Product Name">
                                        <ItemTemplate>
                                            <asp:TextBox ID="productNameTextBox" runat="server" Height="23px" Width="130px" CssClass="TextBox"
                                                Text='<%# Eval("ProductName")%>' AutoPostBack="True" ReadOnly="True" ToolTip="true"
                                                OnTextChanged="productNameTextBox_TextChanged"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PackSize">
                                        <ItemTemplate>
                                            <asp:TextBox ID="packSizeTextBox" Height="23px" Width="30px" runat="server" CssClass="TextBoxCalander"
                                                ReadOnly="True" Text='<%# Eval("PackSize")%>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Batch">
                                        <ItemTemplate>
                                            <asp:TextBox ID="batchTextBox" Width="40px" Height="23px" runat="server" CssClass="TextBoxCalander" ReadOnly="True"
                                                Text='<%# Eval("Batch")%>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mfg. Date">
                                        <ItemTemplate>
                                            <asp:TextBox ID="mfgDateTextBox" runat="server" Height="23px" Width="80px" Text='<%# Eval("MfgDate")%>'
                                                CssClass="TextBox" AutoPostBack="True" OnTextChanged="mfgDateTextBox_OnTextChanged"></asp:TextBox>
                                            <cc1:CalendarExtender ID="manufacturerDate1" runat="server" Format="dd-MMM-yyyy"
                                                TargetControlID="mfgDateTextBox">
                                            </cc1:CalendarExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Exp. Date">
                                        <ItemTemplate>
                                            <asp:TextBox ID="expDateDateTextBox" runat="server" Height="23px" Width="80px" Text='<%# Eval("ExpDate")%>'
                                                CssClass="TextBox" AutoPostBack="True" OnTextChanged="expDateDateTextBox_OnTextChanged"></asp:TextBox>
                                            <cc1:CalendarExtender ID="manufacturerDate" runat="server" Format="dd-MMM-yyyy" TargetControlID="expDateDateTextBox">
                                            </cc1:CalendarExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty">
                                        <ItemTemplate>
                                            <asp:TextBox ID="reqQtyTextBox" runat="server" Height="23px" Width="60px" CssClass="TextBoxCalander"
                                                Text='<%# Eval("Quantity")%>' AutoPostBack="True" OnTextChanged="reqQtyTextBox_OnTextChanged"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderQuantity" runat="server"
                                                Enabled="True" TargetControlID="reqQtyTextBox" FilterType="Custom" ValidChars="0123456789.">
                                            </cc1:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Price">
                                        <ItemTemplate>
                                            <asp:TextBox ID="costPriceTextBox" runat="server" Height="23px" Width="60px" Text='<%# Eval("Price")%>'
                                                CssClass="TextBoxCalander" AutoPostBack="True" OnTextChanged="costPriceTextBox_OnTextChanged"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderPrice" runat="server" Enabled="True"
                                                TargetControlID="costPriceTextBox" FilterType="Custom" ValidChars="0123456789.">
                                            </cc1:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Vat">
                                        <ItemTemplate>
                                            <asp:TextBox ID="vatTextBox" runat="server" Height="23px" Width="60px" CssClass="TextBoxCalander"
                                                Text='<%# Eval("Vat")%>' AutoPostBack="True" OnTextChanged="vatTextBox_OnTextChanged"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderVat" runat="server" Enabled="True"
                                                TargetControlID="vatTextBox" FilterType="Custom" ValidChars="0123456789.">
                                            </cc1:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Amount">
                                        <ItemTemplate>
                                            <asp:TextBox ID="totalValueTextBox" runat="server" Height="23px" Width="60px" CssClass="TextBoxCalander"
                                                Text='<%# Eval("TotalAmount")%>' ReadOnly="True" AutoPostBack="True" OnTextChanged="totalValueTextBox_OnTextChanged"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="C.Stock">
                                        <ItemTemplate>
                                            <asp:TextBox ID="cStockTextBox" runat="server" Height="23px" Width="60px" CssClass="TextBoxCalander"
                                                Text='<%# Eval("CStock")%>' ReadOnly="True" AutoPostBack="True" ></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Stock Out Qty.">
                                        <ItemTemplate>
                                            <asp:TextBox ID="qtyTextBox" runat="server" Height="23px" Width="60px" CssClass="TextBoxCalander"
                                                 AutoPostBack="True" OnTextChanged="qtyTextBox_OnTextChanged"></asp:TextBox>
                                                 <cc1:FilteredTextBoxExtender ID="fqtyTextBox" runat="server"
                                                        TargetControlID="qtyTextBox"         
                                                        FilterType="Custom, Numbers"
                                                        ValidChars="." />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" 
                                                    oncheckedchanged="chkSelectAll_CheckedChanged" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" AutoPostBack="True" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Add">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/lineAdd.png"
                                                OnClick="ImageButton1_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remove">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/images/lineDelete.png"
                                                OnClick="ImageButton2_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
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
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" style="text-align: right; padding-right: 10px;" class="TDLeft">
                            Total Qty :
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="totalQtyTextBox" Height="23px" runat="server" class="TextBox" ReadOnly="True"></asp:TextBox>
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
                        <td class="TDRight" style="text-align: right; padding-right: 10px;" width="20%">
                            &nbsp;<asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click">&lt;&lt;&lt;&lt;-Back To List</asp:LinkButton>
                        </td>
                        <td class="TDLeft" style="text-align: right; padding-right: 10px;" width="13%">
                            Total Vat:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="totalVatTextBox" Height="23px" ReadOnly="True" class="TextBox" runat="server"></asp:TextBox>
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
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" style="text-align: right; padding-right: 10px;" class="TDLeft">
                            Grand Total:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="grandTotalTextBox" Height="23px" runat="server" class="TextBox"
                                ReadOnly="True"></asp:TextBox>
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
                            <asp:Button ID="submitButton" runat="server" OnClick="submitButton_Click" Text="Submit"
                            OnClientClick="return confirm('Are you sure you want to Save ?');" />
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
    
     <asp:UpdateProgress ID="UpdateProgress1" runat="server" ClientIDMode="Static" DisplayAfter="0" DynamicLayout="true">
        <ProgressTemplate>
            <div class="divWaiting">
                <asp:Image ID="imgWait" CssClass="position-set" runat="server" ImageAlign="Middle" ImageUrl="~/images/progress-bar-opt.gif"
                    Height="80" Width="80" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


</asp:Content>
