<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MainMasterPage.master" AutoEventWireup="true" CodeFile="PrettyCashEntry.aspx.cs" Inherits="SInventory_UI_PrettyCashEntry" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <style type="text/css">
        /*AutoComplete flyout */
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
            margin-left: 0px;
            padding-left: 0px;
            max-height: 350px;
            width: 40% !important;
        }

        /* AutoComplete highlighted item */

        .autocomplete_highlightedListItem {
            background-color: yellow;
            color: black;
            padding: 1px;
        }

        /* AutoComplete item */

        .autocomplete_listItem {
            background-color: white;
            color: blue;
            padding: 0px;
        }


        #ContentPlaceHolder1_itemGridView tr > td > .textbox {
            margin-top: 4px !important;
            padding: 3px !important;
        }
    </style>




</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link type="text/css" href="../css/Date/jquery.datepick.css" rel="stylesheet">
    <script type="text/javascript" src="../css/Date/jquery.datepick.js"></script>
    <script type="text/javascript">
        $(function () {
            $('.datepick').datepick();
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {

                    $('.datepick').datepick();
                }
            });
        };
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            Pretty Cash Entry
                        </td>
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight">&nbsp;
                         
                        </td>
                        <td width="13%" class="TDLeft">
                            <asp:HiddenField ID="custIdHiddenField" runat="server" />
                            <asp:HiddenField ID="MasterIdHiddenField" runat="server" />
                        </td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft">&nbsp;
                        </td>
                        <td width="20%" class="TDRight"></td>
                    </tr>


                    <tr>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            View List :
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:ImageButton ID="ImageButton3" runat="server"
                                ImageUrl="~/images/viewList.png" OnClick="ListImageButton_Click" />
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>

                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                    </tr>


                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Company Name: <span class="star-mark">* </span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="CompanyDropDownList" runat="server" CssClass="DropDown" AutoPostBack="True" OnSelectedIndexChanged="companyDropDownList_OnSelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>

                   


                    <tr>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Cash Account: <span class="star-mark">* </span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="CashAccDropDownlist" runat="server" CssClass="DropDown">
                            </asp:DropDownList>
                        </td>


                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                    </tr>
                    
                     <tr>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                        <td class="TDLeft" width="13%" style="text-align: right; padding-right: 10px;">
                             Posting Date : <span class="star-mark">* </span>
                        </td>

                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="txtDate" runat="server" CssClass="datepick" AutoPostBack="true" Width="137px" OnTextChanged="dateOfBirthTextBox_OnTextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" AlternateText="Click to show calendar" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                TabIndex="4" ID="imgDatse"></asp:ImageButton>
                            <asp:CalendarExtender ID="Date" runat="server" Format="dd-MMM-yyyy" TargetControlID="txtDate"
                                PopupButtonID="imgDatse">
                            </asp:CalendarExtender>
                        </td>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>

                    </tr>


                    <tr runat="server" visible="False">

                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>

                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            <span id="Span7">Balance</span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:TextBox ID="TextBalance" runat="server" CssClass="textbox" autocomplete="off"></asp:TextBox>
                        </td>


                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>


                    </tr>


                    <tr>
                        <td width="13%" class="TDLeft"></td>
                        <td width="20%" class="TDRight"></td>
                        <td width="13%" class="TDLeft" style="text-align: right; padding-right: 10px;">
                            Remark:
                        </td>
                        <td width="15%" class="TDRight">
                            <asp:TextBox ID="TextRemark" CssClass="TextBox" TextMode="MultiLine" Rows="2" runat="server" />
                        </td>



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

                        <td class="TDRight" width="13%" colspan="6">

                            <div id="divMain" style="height: auto; overflow: auto; width: 97%; margin: 0 auto;">

                                <asp:GridView ID="itemGridView" runat="server" AutoGenerateColumns="False" Height="60px"
                                    CssClass="gridview" DataKeyNames="PerticulerId">
                                    <Columns>
                                        <asp:TemplateField HeaderText="SL">
                                            <ItemTemplate>
                                                <asp:Label ID="LabelSL" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Perticular (COA of expense)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPerticular" autocomplete="off" Width="350px" runat="server" class="textbox" Text='<%# Eval("ParticularName")%>' AutoPostBack="True" ToolTip="true" OnTextChanged="PerticularTextBox_TextChanged"></asp:TextBox>

                                                <asp:AutoCompleteExtender ID="productNameTextBox_AutoCompleteExtender" runat="server"
                                                    DelimiterCharacters="" EnableCaching="true" Enabled="True" MinimumPrefixLength="1"
                                                    CompletionSetCount="10" ServiceMethod="GetPerticularaction" ServicePath="SInventoryWebService.asmx"
                                                    TargetControlID="txtPerticular" CompletionListCssClass="autocomplete_completionListElement"
                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    ShowOnlyCurrentWordInCompletionListItem="true">
                                                </asp:AutoCompleteExtender>

                                                <asp:HiddenField ID="perticularhiddenId" Value='<%# Eval("PerticulerId")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Cost Reason">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtCostReason" Width="450px"  autocomplete="off" runat="server" class="textbox" Text='<%# Eval("CashReason")%>'></asp:TextBox>


                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Amount">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAmount" autocomplete="off" runat="server" class="textbox" Text='<%# Eval("Amount")%>'></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="freqQtyTextBoxs" runat="server"
                                                                                     TargetControlID="txtAmount"         
                                                                                     FilterType="Custom, Numbers"
                                                                                     ValidChars="." />
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Add">
                                            <ItemTemplate>

                                                <asp:ImageButton ID="ImageButton1" runat="server" CommandName="AddData" ImageUrl="~/images/lineAdd.png"
                                                    OnClick="itemaddImageButton_Click" />


                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Remove">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton2" runat="server" CommandName="DeleteData" ImageUrl="~/images/lineDelete.png"
                                                    OnClick="itemdeleteImageButton_Click" />
                                                <asp:LinkButton ID="itemdeleteImageButton" runat="server" class="btn btn-danger btn-sm  btnTextShadow" CommandName="DeleteData" OnClick="itemdeleteImageButton_Click"><i class="fa fa-trash" aria-hidden="true"></i>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>


                            </div>
                        </td>
                    </tr>



                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%"></td>
                        <td class="TDRight" width="20%"></td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                    </tr>

                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
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
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">
                            <asp:Button ID="btnSave" runat="server" OnClick="submitButton_Click1"
                                Text="Save" />
                            <asp:Button ID="Button2" CssClass="clearButton" BackColor="#E1B12C" runat="server" OnClick="clearButton_OnClick"
                                Text="Clear" />
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;</td>
                        <td class="TDRight" width="20%">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
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
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%"></td>
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
                        </td>
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
                        <td class="TDLeft" width="13%">&nbsp;
                        </td>
                        <td class="TDRight" width="20%">&nbsp;
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
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

