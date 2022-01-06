<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MainMasterPage.master" AutoEventWireup="true" CodeFile="CustomerDueReport.aspx.cs" Inherits="SInventory_UI_CustomerDueReport" %>
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
                            Customer Due Report
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
                    
                    <tr runat="server">
                        
                        
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            From Date: <span class="star-mark"> * </span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="fromDateTextBox" runat="server" Width="145px" CssClass="TextBoxCalander"></asp:TextBox>
                            <asp:ImageButton ID="imgDate" runat="server" AlternateText="Click to show calendar"
                                ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="4" />
                            <asp:CalendarExtender ID="Date" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgDate"
                                TargetControlID="fromDateTextBox">
                            </asp:CalendarExtender>
                        </td>
                        
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            To Date: <span class="star-mark"> * </span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="toDateTextBox" runat="server" Width="145px" CssClass="TextBoxCalander"></asp:TextBox>
                            <asp:ImageButton ID="ImageButton1" runat="server" AlternateText="Click to show calendar"
                                ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="5" />
                            <asp:CalendarExtender ID="CalendarExtender12" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ImageButton1"
                                TargetControlID="toDateTextBox">
                            </asp:CalendarExtender>
                        </td>
                        
                        
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">                            Customer Code:                        </td>                        <td width="20%" class="TDRight">                            <asp:TextBox ID="txtCustomerCode" runat="server" Width="145px" CssClass="TextBoxCalander"></asp:TextBox>                        </td>                        <td width="13%" class="TDLeft">                        </td>                        <td width="20%" class="TDRight">                        </td>

                        
                    </tr>
                    

                    <tr>
                        <td width="13%" class="TDLeft"  style="text-align: right; padding-right: 10px;">
                            Company:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="manufacDropDownList" runat="server" AutoPostBack="True" CssClass="DropDown"
                                OnSelectedIndexChanged="manufacDropDownList_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft"  style="text-align: right; padding-right: 10px;">
                            DC Name:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="salesCenterDropDownList" runat="server" AutoPostBack="True"
                                CssClass="DropDown" OnSelectedIndexChanged="salesCenterDropDownList_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft"  style="text-align: right; padding-right: 10px;">
                            Area:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="areaDropDownList" runat="server" AutoPostBack="True" CssClass="DropDown"
                                OnSelectedIndexChanged="areaDropDownList_OnSelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft"  style="text-align: right; padding-right: 10px;">
                            Territory:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="territoryDropDownList" runat="server" AutoPostBack="True" CssClass="DropDown"
                                OnSelectedIndexChanged="territoryDropDownList_OnSelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft"  style="text-align: right; padding-right: 10px;">
                            Market:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="marketDropDownList" runat="server" CssClass="DropDown" AutoPostBack="True"
                                OnSelectedIndexChanged="marketDropDownList_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft"  style="text-align: right; padding-right: 10px;">
                            Customer:
                        </td>
                        <td width="20%" class="TDRight">
                             <asp:DropDownList ID="custDropDownList" runat="server" AutoPostBack="True" CssClass="DropDown">
                            </asp:DropDownList>
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
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                            
                            <asp:Button ID="reportButton1" runat="server" CssClass="button-margin-right" OnClick="reportButton1_Click" Text="View Grid" />
                            <asp:Button ID="reportButton" runat="server" CssClass="button-margin-right" OnClick="reportButton_Click" Text="View Report" />
                            <asp:Button ID="clearButton" runat="server" Text="Clear" CssClass="margin-right"
                                OnClick="clearButton_OnClick" BackColor="#F47322" />
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
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
                                DataKeyNames="InvoiceNo">
                                <Columns>
                                    <asp:BoundField DataField="CompanyName" HeaderText="Company Name" />
                                    <asp:BoundField DataField="ComUnitName" HeaderText="Unit Name" />
                                    <asp:BoundField DataField="CustomerCode" HeaderText="Customer Code" />
                                    <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                                    <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" />
                                    <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                    <asp:BoundField DataField="DeliveryTpGrandTotal" HeaderText="Actual Value" />
                                    <asp:BoundField DataField="DueAmount" HeaderText="Due Amount" />

                                    <asp:TemplateField HeaderText="Action" >
                                        <ItemTemplate>
                                            <asp:LinkButton ID="invoiceButton" runat="server" Text="Preview Invoice &gt; &gt; &gt;" ForeColor="#DE751F"
                                                OnClick="invoiceButton_Click" />
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
    
     <asp:UpdateProgress ID="UpdateProgress1" runat="server" ClientIDMode="Static" DisplayAfter="0" DynamicLayout="true">
        <ProgressTemplate>
            <div class="divWaiting">
                <asp:Image ID="imgWait" CssClass="position-set" runat="server" ImageAlign="Middle" ImageUrl="~/images/progress-bar-opt.gif"
                    Height="80" Width="80" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


</asp:Content>

