<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_ManufacturerEntry, App_Web_k52p5woi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            Manufacturer Entry
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">
                            &nbsp; View List</td>
                        <td width="20%" class="TDRight">
                            <asp:ImageButton ID="ListImageButton" runat="server" 
                                ImageUrl="~/images/viewList.png" onclick="ListImageButton_Click" />
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:Label ID="MessageLabel" runat="server" ForeColor="#009900"></asp:Label>
                        </td>
                        <td width="13%" class="TDLeft">
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
                        </td>
                        <td width="20%" class="TDRight">
                        </td>
                        <td width="13%" class="TDLeft">
                            Manufacturer Name <span class="star-mark"> * </span></td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="manufacturerNameTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                        </td>
                        <td width="13%" class="TDLeft">
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
                            Manufacturer Address <span class="star-mark"> * </span></td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="manufacturerAddressTextBox" runat="server" CssClass="TextBox" 
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
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
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            <asp:Button ID="submitButton" runat="server" onclick="submitButton_Click" 
                                Text="Submit" />
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
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



