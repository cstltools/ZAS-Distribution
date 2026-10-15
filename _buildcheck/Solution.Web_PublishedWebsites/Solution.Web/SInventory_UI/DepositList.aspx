<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_DepositList, App_Web_bhssqfjy" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
    <style>
        .margin-right {
            margin-right: 4px !important;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            Deposit List
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
                     <tr runat="server" ID="entry" Visible="True">
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">&nbsp; Add New </td>
                        <td width="20%" class="TDRight">
                            <asp:ImageButton ID="productListImageButton" runat="server"
                                ImageUrl="~/images/lineAdd.png" OnClick="ListImageButton_Click" />
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="MessageLabel" runat="server" ForeColor="#009900"></asp:Label>
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
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
                                OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged" AutoPostBack="True" >
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
                            Distribution Center:
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
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            (Deposit) From Date: <span class="star-mark"> * </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="fromDateTextBox" runat="server" CssClass="TextBoxCalander"></asp:TextBox>
                            <asp:ImageButton runat="server" AlternateText="Click to show calendar" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                TabIndex="4" ID="imgDate"></asp:ImageButton>
                            <asp:CalendarExtender ID="Date" runat="server" Format="dd-MMM-yyyy" TargetControlID="fromDateTextBox"
                                PopupButtonID="imgDate">
                            </asp:CalendarExtender>
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
                            (Deposit) To Date: <span class="star-mark"> * </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="toDateTextBox" runat="server" CssClass="TextBoxCalander"></asp:TextBox>
                            <asp:ImageButton runat="server" AlternateText="Click to show calendar" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                TabIndex="4" ID="ImageButton1"></asp:ImageButton>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="toDateTextBox" PopupButtonID="ImageButton1">
                            </asp:CalendarExtender>
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
                            <asp:Button ID="submitButton" runat="server" OnClick="Button_Click" CssClass="margin-right" Text="Search" />
                            <asp:Button ID="ReportButton" runat="server" BackColor="Green" OnClick="reportButton_Click" CssClass="margin-right" Text="Preview" />
                            <asp:Button ID="clearButton" BackColor="#E8BC31" runat="server" OnClick="clearButton_OnClick" Text="Clear" />
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
                        <td class="TDLeft" colspan="6">
                            
                            <div id="gridContainer" style=" text-align: center; height: auto; overflow: auto; width: 97%; margin: 0 auto;">
                                
                                <asp:GridView ID="invoiceGridView" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                                DataKeyNames="DepositId"  onrowcommand="loadGridView_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="CompanyName" HeaderText="Company Name" />
                                    <asp:BoundField DataField="ComUnitName" HeaderText="Unit Name" />
                                    <asp:BoundField DataField="DepositDate" HeaderText="Deposit Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                    <%--<asp:BoundField DataField="Collection" HeaderText="Collection" />--%>
                                    <asp:BoundField DataField="Amount" HeaderText="Deposit Amount" />
                                    <asp:BoundField DataField="DepositType" HeaderText="Deposit Type" />
                                    <%--<asp:BoundField DataField="DueAmount" HeaderText="Due" />--%>
                                    <asp:BoundField DataField="BankName" HeaderText="Bank Name" />
                                    <asp:BoundField DataField="BranchName" HeaderText="Branch Name" />                                  
                                    <asp:BoundField DataField="AccountName" HeaderText="Account No" />                                  
                                    <asp:BoundField DataField="CheckNumber" HeaderText="Check Number" />                                  
                                    <asp:BoundField DataField="CheckDate" HeaderText="Check Date" DataFormatString="{0:dd-MMM-yyyy}" />    
                                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" />    
                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="editImageButton" runat="server" 
                                                CommandArgument="<%# Container.DataItemIndex %>" CommandName="EditData" 
                                                ImageUrl="~/images/lineDelete.png" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                                 
                            </div>
                            
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
                     <tr>
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
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
                     <tr>
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
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
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

