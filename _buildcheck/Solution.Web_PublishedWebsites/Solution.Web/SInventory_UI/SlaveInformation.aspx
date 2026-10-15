<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_SlaveInformation, App_Web_pkilq3oa" %>

<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
     <style type="text/css">
       .modetitle
        {
            font-size: 1.5em;
            color: #25394A;
        }
        
        .modelabel
        {
            background-color: #EAEAEA;
            color: #2B78D6;
            padding: 10px;
            border-radius: 5px;
            letter-spacing: 0.1em;
            text-transform: uppercase;
            font-weight: 700;
        }
        
        
        .clearButton {
            background: #F7E543;
            color: #fff;
            padding: 10px 15px;
            border-radius: 5px;
            letter-spacing: 0.1em;
            font-weight: 700;
            margin-left: 10px;
        }
        .clearButton:hover {
            background: #F7A704;
        }
        
        
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            Slave Information
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
                        <td class="TDLeft" width="13%" style="text-align: center">
                            View List &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:ImageButton ID="ListImageButton" runat="server" 
                                ImageUrl="~/images/viewList.png" onclick="ListImageButton_Click" />
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
                            <span class="modetitle"> Operational Mode: </span>
                            <asp:Label ID="modeLabel" CssClass="modelabel" runat="server" Text=""></asp:Label>
                            &nbsp;
                        </td>
                    </tr>
                    

                    <tr>
                        <td width="13%" class="TDLeft">
                            <asp:HiddenField ID="slaveIdHiddenField" runat="server" />
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
                        <td class="TDLeft" width="13%">
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                        <td class="TDLeft" width="13%">
                            Company Name <span class="star-mark">* </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="companyNameDropDownList" runat="server" CssClass="DropDown" AutoPostBack="True" OnSelectedIndexChanged="companyNameDropDownList_OnSelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:TextBox ID="hiddenTextBox" runat="server" Visible="false"></asp:TextBox>
                        </td>
                        <td class="TDLeft" width="13%">
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft">
                        </td>
                        <td width="20%" class="TDRight">
                            &nbsp;
                        </td>
                        <td width="13%" class="TDLeft">
                            Maximum Amount <span class="star-mark">* </span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="maxAmountTextBox" runat="server" CssClass="TextBox" AutoPostBack="true" OnTextChanged="maxAmountTextBox_OnTextChanged">  </asp:TextBox>
                            <ajaxToolkit:FilteredTextBoxExtender ID="fcostPriceTextBox" runat="server" TargetControlID="maxAmountTextBox"
                                FilterType="Custom, Numbers" ValidChars=".0123456789" />
                                <span style="color: #9400d3; display: block; font-size: 10px;"> <strong>N.B.</strong> Select company before amount entry! </span>
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: center">
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
                            Minimum Amount <span class="star-mark">* </span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="minAmmountTextBox" runat="server" CssClass="TextBox" AutoPostBack="true" OnTextChanged="minAmmountTextBox_OnTextChanged">  </asp:TextBox>
                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server"
                                TargetControlID="minAmmountTextBox" FilterType="Custom, Numbers" ValidChars=".0123456789" />
                                <span style="color: #9400d3; display: block; font-size: 10px;"> <strong>N.B.</strong> Select company before amount entry! </span>
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
                            Discount Percent % <span class="star-mark">* </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="disPerTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                            <ajaxToolkit:FilteredTextBoxExtender ID="vatperTextBox_FilteredTextBoxExtender" runat="server"
                                FilterType="Custom, Numbers" TargetControlID="disPerTextBox" ValidChars="0123456789" />
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
                            Is Active <span class="star-mark">* </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:CheckBox ID="isActiveCheckBox" runat="server" AutoPostBack="true" OnCheckedChanged="isActiveCheckBox_OnCheckedChanged" />
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
                            Active Date <span class="star-mark">* </span>
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="activeDtTextBox" runat="server" CssClass="TextBoxCalander" AutoPostBack="True"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="activeDtTextBox_CalendarExtender" runat="server"
                                Format="dd-MMM-yyyy" PopupButtonID="ImageButton" TargetControlID="activeDtTextBox">
                            </ajaxToolkit:CalendarExtender>
                            <asp:ImageButton ID="ImageButton" runat="server" AlternateText="Click to show calendar"
                                ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="4" />
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;
                        </td>
                        <td class="TDRight" width="20%">
                            &nbsp;
                        </td>
                    </tr>
                    
                    <div runat="server" id="newSlave" visible="false">
                        
                        <tr>
                            <td class="TDLeft" width="13%">
                                &nbsp;
                            </td>
                            <td class="TDRight" width="20%">
                                &nbsp;
                            </td>
                            <td class="TDLeft" width="13%">
                                InActive Date  <span class="star-mark">* </span>
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:TextBox ID="inActiveDateTextBox" runat="server" CssClass="TextBoxCalander" AutoPostBack="True"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy"
                                    PopupButtonID="ImageButton3" TargetControlID="inActiveDateTextBox">
                                </ajaxToolkit:CalendarExtender>
                                <asp:ImageButton ID="ImageButton3" runat="server" AlternateText="Click to show calendar"
                                    ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="4" />
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
                                &nbsp;
                            </td>
                            <td width="13%" class="TDLeft">
                                Maximum Amount(New) <span class="star-mark">* </span>
                            </td>
                            <td width="20%" class="TDRight">
                                <asp:TextBox ID="newMaxAmountTextBox" runat="server" CssClass="TextBox" AutoPostBack="True" OnTextChanged="newMaxAmountTextBox_OnTextChanged">  </asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                    TargetControlID="newMaxAmountTextBox" FilterType="Custom, Numbers" ValidChars=".0123456789" />
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
                            <td width="13%" class="TDLeft">
                                Minimum Amount(New) <span class="star-mark">* </span>
                            </td>
                            <td width="20%" class="TDRight">
                                <asp:TextBox ID="newMinAmountTextBox" runat="server" CssClass="TextBox" AutoPostBack="True" OnTextChanged="newMinAmountTextBox_OnTextChanged">  </asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                    TargetControlID="newMinAmountTextBox" FilterType="Custom, Numbers" ValidChars=".0123456789" />
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
                                Discount Percent % (New) <span class="star-mark">* </span>
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:TextBox ID="newDisPerTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server"
                                    FilterType="Custom, Numbers" TargetControlID="newDisPerTextBox" ValidChars="0123456789" />
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
                                Active Date (New) <span class="star-mark">* </span>
                            </td>
                            <td class="TDRight" width="20%">
                                <asp:TextBox ID="newActiveDateTextBox" runat="server" CssClass="TextBoxCalander" AutoPostBack="True"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                    PopupButtonID="ImageButton1" TargetControlID="newActiveDateTextBox">
                                </ajaxToolkit:CalendarExtender>
                                <asp:ImageButton ID="ImageButton1" runat="server" AlternateText="Click to show calendar"
                                    ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="4" />
                            </td>
                            <td class="TDLeft" width="13%">
                                &nbsp;
                            </td>
                            <td class="TDRight" width="20%">
                                &nbsp;
                            </td>
                        </tr>
                        
                    </div>
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
                            <asp:Button ID="submitButton" runat="server" OnClick="submitButton_Click" Text="Submit" />
                            
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
