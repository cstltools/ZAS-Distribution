<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_WarehouseStockIn, App_Web_pcm1iuwz" %>

<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .button-padding-right
        {
            margin-right: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            Warehouse Stock In
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
                        </td>
                        <td width="13%" class="TDLeft">
                            <asp:LinkButton ID="viewLinkButton" CssClass="btn buttons smallBtn" ForeColor="green"
                                OnClick="viewLinkButton_OnClick" runat="server">View Details</asp:LinkButton>
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
                        <td width="13%" style="text-align: right; padding-right: 10px;" class="TDLeft">
                            Manufacturer: <span style="color: red">*</span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="manufacturerDropDownList" Width="175px" Height="23px" runat="server"
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
                            &nbsp;
                        </td>
                        <td width="13%" style="text-align: right; padding-right: 10px;" class="TDLeft">
                            Stock In Date: <span style="color: red">*</span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="stockInDateTextBox" Width="150px" Height="23px" runat="server" CssClass="TextBoxCalander"
                                AutoPostBack="True" OnTextChanged="stockInDateTextBox_OnTextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" AlternateText="Click to show calendar" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                TabIndex="4" ID="imgDate"></asp:ImageButton>
                            <cc1:CalendarExtender ID="manufacturerDate1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="stockInDateTextBox" PopupButtonID="imgDate">
                            </cc1:CalendarExtender>
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
                            &nbsp;
                        </td>
                        <td width="13%" style="text-align: right; padding-right: 10px;" class="TDLeft">
                            Challan No: <span style="color: red">*</span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="challanNoTextBox" Height="23px" runat="server" CssClass="TextBox"></asp:TextBox>
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
                        <td class="TDLeft" style="text-align: right; padding-right: 10px;" width="13%">
                            Challan Date: <span style="color: red">*</span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="challanDateTextBox" Width="150px" Height="23px" runat="server" CssClass="TextBoxCalander"
                                AutoPostBack="True" OnTextChanged="challanDateTextBox_OnTextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" AlternateText="Click to show calendar" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                TabIndex="4" ID="ImageButton4"></asp:ImageButton>
                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="challanDateTextBox" PopupButtonID="ImageButton4">
                            </cc1:CalendarExtender>
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
                        <td class="TDLeft" style="text-align: right; padding-right: 10px;" width="13%">
                            Reference No:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="referenceNoTextBox" Height="23px" runat="server"></asp:TextBox>
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
                        <td class="TDLeft" style="text-align: right; padding-right: 10px;" width="13%">
                            Remarks:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="remarksTextBox" Height="20px" Width="170px" runat="server" ></asp:TextBox>
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
                        <td class="TDRight" width="20%" colspan="4">
                            <asp:GridView ID="productGridView" runat="server" AutoGenerateColumns="False" CssClass="gridview">
                                <Columns>
                                    <asp:TemplateField HeaderText="SL">
                                        <ItemTemplate>
                                            <asp:Label ID="LabelSL" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Label>
                                            <asp:HiddenField ID="productidHiddenField" Value='<%# Eval("ProductId")%>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="P.Code">
                                        <ItemTemplate>
                                            <asp:TextBox ID="productCodeTextBox" runat="server"  CssClass="TextBoxCalander" Width="60px" 
                                                AutoPostBack="True" ToolTip="true" OnTextChanged="productCodeTextBox_TextChanged"
                                                Text='<%# Eval("ProductCode")%>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="P.Name">
                                        <ItemTemplate>
                                            <asp:TextBox ID="productNameTextBox" runat="server"  CssClass="TextBoxCalander" 
                                                Text='<%# Eval("ProductName")%>' AutoPostBack="True" ReadOnly="True" ToolTip="true"
                                                OnTextChanged="productNameTextBox_TextChanged"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PackSize">
                                        <ItemTemplate>
                                            <asp:TextBox ID="packSizeTextBox"  CssClass="TextBoxCalander" runat="server" Width="60px" 
                                                ReadOnly="True" Text='<%# Eval("PackSize")%>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Batch">
                                        <ItemTemplate>
                                            <asp:TextBox ID="batchTextBox"  CssClass="TextBoxCalander" runat="server" Width="50px" 
                                                Text='<%# Eval("Batch")%>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mfg.Date">
                                        <ItemTemplate>
                                            <asp:TextBox ID="mfgDateTextBox" runat="server"  Text='<%# Eval("MfgDate")%>' 
                                                CssClass="TextBoxCalander" AutoPostBack="True" OnTextChanged="mfgDateTextBox_OnTextChanged"></asp:TextBox>
                                               <%--   <asp:ImageButton runat="server" AlternateText="Click to show calendar" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                TabIndex="4" ID="ImageButton15"></asp:ImageButton>--%>
                                            <cc1:CalendarExtender ID="manufacturerDate1" runat="server" Format="dd-MMM-yyyy"
                                                TargetControlID="mfgDateTextBox" PopupButtonID="ImageButton151">
                                            </cc1:CalendarExtender>
                                            
                                         
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Exp.Date">
                                        <ItemTemplate>
                                            <asp:TextBox ID="expDateDateTextBox" runat="server"  Text='<%# Eval("ExpDate")%>'
                                                CssClass="TextBoxCalander" AutoPostBack="True" OnTextChanged="expDateDateTextBox_OnTextChanged"></asp:TextBox>
                                            <%--<asp:ImageButton runat="server" AlternateText="Click to show calendar" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                TabIndex="4" ID="ImageButton25"></asp:ImageButton>--%>
                                            <cc1:CalendarExtender ID="manufacturerDate" runat="server" PopupButtonID="ImageButton25" Format="dd-MMM-yyyy" TargetControlID="expDateDateTextBox">
                                            </cc1:CalendarExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty">
                                        <ItemTemplate>
                                            <asp:TextBox ID="reqQtyTextBox" runat="server"  CssClass="TextBoxCalander"
                                                Text='<%# Eval("Quantity")%>' AutoPostBack="True" OnTextChanged="reqQtyTextBox_OnTextChanged"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderQuantity" runat="server"
                                                Enabled="True" TargetControlID="reqQtyTextBox" FilterType="Custom" ValidChars="0123456789.">
                                            </cc1:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Price">
                                        <ItemTemplate>
                                            <asp:TextBox ID="costPriceTextBox" runat="server"  ReadOnly="True" 
                                                Text='<%# Eval("Price")%>' CssClass="TextBoxCalander" AutoPostBack="True"  OnTextChanged="costPriceTextBox_OnTextChanged"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderPrice" runat="server" Enabled="True"
                                                TargetControlID="costPriceTextBox" FilterType="Custom" ValidChars="0123456789.">
                                            </cc1:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Vat">
                                        <ItemTemplate>
                                            <asp:TextBox ID="vatTextBox" runat="server"  CssClass="TextBoxCalander" ReadOnly="True" 
                                                Text='<%# Eval("Vat")%>' AutoPostBack="True" OnTextChanged="vatTextBox_OnTextChanged"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderVat" runat="server" Enabled="True"
                                                TargetControlID="vatTextBox" FilterType="Custom" ValidChars="0123456789.">
                                            </cc1:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Amt">
                                        <ItemTemplate>
                                            <asp:TextBox ID="totalValueTextBox" runat="server"  CssClass="TextBoxCalander"
                                                Text='<%# Eval("TotalAmount")%>' ReadOnly="True" AutoPostBack="True" OnTextChanged="totalValueTextBox_OnTextChanged"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField >
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/lineAdd.png"
                                                OnClick="ImageButton1_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField >
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/images/lineDelete.png"
                                                OnClick="ImageButton2_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
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
                            &nbsp;
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
                            <asp:Button ID="submitButton" CssClass="button-padding-right" runat="server" OnClick="submitButton_Click"
                             OnClientClick="return confirm('Are you sure you want to Save ?');"   Text="Submit" />
                            <asp:Button ID="cancelButton" BackColor="#cc6600" runat="server" OnClick="cancelButton_Click"
                                Text="Cancel" />
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
</asp:Content>
