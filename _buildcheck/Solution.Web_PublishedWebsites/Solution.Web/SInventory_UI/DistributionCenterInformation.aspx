<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_DistributionCenterInformation, App_Web_clqxceh0" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            Distribution Center Information
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
                            &nbsp;</td>
                        <td width="20%" class="TDRight">
                            &nbsp;</td>
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                            &nbsp;
                        </td>
                    </tr>

                    <tr>
                        <td width="13%" class="TDLeft">
                            &nbsp; View List</td>
                        <td width="20%" class="TDRight">
                            <asp:ImageButton ID="unitViewImageButton" runat="server" 
                                ImageUrl="~/images/viewList11.png" onclick="unitViewImageButton_Click" />
                        </td>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                            &nbsp;</td>
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
                            &nbsp;
                        </td>
                    </tr>
                    
                    <tr>
                        <td class="TDLeft" width="13%">
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Company Name: <span class="star-mark"> * </span> </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="companyNameDropDownList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="companyNameDropDownList_OnSelectedIndexChanged"
                                CssClass="DropDown" >
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
                        <td class="TDLeft" width="13%"  style="text-align: right; padding-right: 10px;">
                            Distribution Type: <span class="star-mark"> * </span> </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="ddlDcType" runat="server" 
                                CssClass="DropDown" >
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
                           Distribution Center Name: <span class="star-mark"> * </span> </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="salesCenternameTextBox" runat="server" CssClass="TextBox" AutoPostBack="true" OnTextChanged="salesCenternameTextBox_OnTextChanged" ></asp:TextBox>
                            <span style="color: #9400d3; display: block; font-size: 10px;"> <strong>N.B.</strong> Follow this format: <strong style="color: #696969"> Location (Company Short Name)</strong>  </span>
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                        </td>
                    </tr>

                    <tr>
                        <td width="13%" class="TDLeft">
                            &nbsp;</td>
                        <td width="20%" class="TDRight">
                            &nbsp;</td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            DC Short Name: </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="dcShortNameTextBox" runat="server" CssClass="TextBox" AutoPostBack="true" OnTextChanged="dcShortNameTextBox_OnTextChanged"></asp:TextBox>
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;</td>
                        <td width="20%" class="TDRight">
                            &nbsp;</td>
                    </tr>
                    
                    <tr>
                        <td width="13%" class="TDLeft">
                            &nbsp;</td>
                        <td width="20%" class="TDRight">
                            &nbsp;</td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Address: <span class="star-mark"> * </span> </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="addressTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;</td>
                        <td width="20%" class="TDRight">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Phone No: </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="phoneNoTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                            Enabled="True" TargetControlID="phoneNoTextBox" FilterType="Custom" ValidChars="0123456789"></cc1:FilteredTextBoxExtender>

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
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Mobile No: </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="mobileNoTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                             <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderqty" runat="server"
                            Enabled="True" TargetControlID="mobileNoTextBox" FilterType="Custom" ValidChars="0123456789+"></cc1:FilteredTextBoxExtender>
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
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Fax-No:</td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="faxNoTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                            Enabled="True" TargetControlID="faxNoTextBox" FilterType="Custom" ValidChars="0123456789+"></cc1:FilteredTextBoxExtender>
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
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                            Cash-GL Account:</td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="ddlCashAccount" runat="server" CssClass="DropDown" ></asp:DropDownList>
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                    
                    <tr>
                        <td width="13%" class="TDLeft">
                            &nbsp;
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
                            &nbsp;</td>
                            <td width="20%" class="TDRight">
                                <asp:Button ID="submitButton" runat="server" onclick="submitButton_Click1" 
                                    Text="Submit" />
                                    
                                <asp:Button ID="clearButton" CssClass="clearButton" BackColor="#E1B12C" runat="server" OnClick="clearButton_OnClick"
                                Text="Clear" /> 
                        </td>
                        <td width="13%" class="TDLeft">
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                            &nbsp;
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

