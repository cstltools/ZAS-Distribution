<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MainMasterPage.master" AutoEventWireup="true" CodeFile="ProformaorInvoiceReturn.aspx.cs" Inherits="SInventory_UI_ProformaorInvoiceReturn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            Proforma Invoice Return
                        </td>
                    </tr>
                    <tr>
                <td class="TDLeft" width="13%">
                    &nbsp;</td>
                <td class="TDRight" width="20%">
                    &nbsp;</td>
                <td class="TDLeft" width="13%">
                    &nbsp;</td>
                <td class="TDRight" width="20%">
                    &nbsp;</td>
                <td class="TDLeft" width="13%">
                    &nbsp;</td>
                <td class="TDRight" width="20%">
                    &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                           
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:CheckBox ID="ProformaCheckBox" runat="server" AutoPostBack="True" 
                                oncheckedchanged="ProformaCheckBox_CheckedChanged"  />
                            Is Proforma Invoice
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                    </tr>
                     <tr runat="server">
                        <td class="TDLeft" width="13%">
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">  
                        </td>
                        <td class="TDRight" width="20%" >
                            <asp:CheckBox ID="InvoiceCheckBox" runat="server" AutoPostBack="True" 
                                oncheckedchanged="InvoiceCheckBox_CheckedChanged"   /> 
                            Is Delivery Invoice
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                    </tr>
                     <tr id="Tr1" runat="server">
                        <td class="TDLeft" width="13%">
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">  
                        </td>
                        <td class="TDRight" width="20%" >
                            <asp:CheckBox ID="chkSampleInvoice" runat="server" AutoPostBack="True" 
                                oncheckedchanged="InvoiceCheckBox_CheckedChanged"   /> 
                            Is Sample Invoice
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                    </tr>
                    <tr runat="server"  id="DIVProforma">
                        <td class="TDLeft" width="13%">
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                           Invoice No:
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="proformaInvoiceTextBox" runat="server" 
                                ontextchanged="proformaInvoiceTextBox_TextChanged" AutoPostBack="True"></asp:TextBox>
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                    </tr>
                    <tr runat="server" visible="True" id="DIVINV">
                        <td class="TDLeft" width="13%">
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                        <td class="TDLeft" width="13%">
                            
                        </td>
                        <td class="TDRight" width="20%">
                          
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
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
                             <asp:Button ID="saveButton" runat="server" Text="Delete" 
                                onclick="saveButton_Click" OnClientClick="return confirm('Are you sure you want to Delete ?');"/></td>
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
                        <td width="13%" class="TDLeft">
                            &nbsp;</td>
                        <td width="20%" class="TDRight">
                            &nbsp;</td>
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
                        <td width="13%" class="TDLeft" >
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

