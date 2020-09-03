<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MainMasterPage.master" AutoEventWireup="true" CodeFile="SalesHubReport.aspx.cs" Inherits="SInventory_UI_SalesHubReport" %>

<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            Sales Hub Report
                        </td>
                    </tr>
                    <tr>
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
                        </td>
                        <td class="TDRight" width="20%" style="text-align: right; padding-right: 10px;">
                             Report Name: <span class="star-mark"> </span>
                        </td>
                        <td class="TDLeft" width="13%" >
                           <asp:DropDownList ID="reportNameDropDownList" runat="server" Width="175px" AutoPostBack="True"
                                OnSelectedIndexChanged="reportNameDropDownList_OnSelectedIndexChanged" CssClass="DropDown">
                               <asp:ListItem Text="---- Select any one ----" ></asp:ListItem>
                                <asp:ListItem Text="Region Report" Value="REGION"></asp:ListItem>
                                <asp:ListItem Text="Area Report" Value="AREA"></asp:ListItem>
                                <asp:ListItem Text="Territory Report" Value="TERRITORY"></asp:ListItem>
                                <asp:ListItem Text="Market Report" Value="MARKET"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="TDRight" width="20%">
                            
                        </td>
                        <td class="TDLeft" width="13%">
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                    </tr>
                    
                    <div runat="server" visible="false" id="region">
                        <tr>
                            <td class="TDLeft" width="13%">
                            </td>
                             <td class="TDRight" width="20%" style="text-align: right; padding-right: 10px;">
                             Region: <span class="star-mark"> </span>
                        </td>
                            <td class="TDLeft" width="13%">
                                <asp:DropDownList ID="regionDropDownList" runat="server" AutoPostBack="True"
                                    CssClass="DropDown" 
                                    onselectedindexchanged="regionDropDownList_SelectedIndexChanged" 
                                    Width="175px">
                                </asp:DropDownList>
                            </td>
                            <td class="TDRight" width="20%">
                                &nbsp;</td>
                            <td class="TDLeft" width="13%">
                                &nbsp;
                            </td>
                            <td class="TDRight" width="20%">
                            </td>
                        </tr>
                    </div>

                     <div runat="server" visible="false" id="area">
                        <tr>
                            <td class="TDLeft" width="13%">
                            </td>
                             <td class="TDRight" width="20%" style="text-align: right; padding-right: 10px;">
                             Area: <span class="star-mark"> </span>
                        </td>
                            <td class="TDLeft" width="13%">
                                <asp:DropDownList ID="areaDropDownList" runat="server" AutoPostBack="True" 
                                    CssClass="DropDown" 
                                    onselectedindexchanged="areaDropDownList_SelectedIndexChanged" 
                                    Width="175px">
                                </asp:DropDownList>
                            </td>
                            <td class="TDRight" width="20%">
                                &nbsp;</td>
                            <td class="TDLeft" width="13%">
                                &nbsp;
                            </td>
                            <td class="TDRight" width="20%">
                            </td>
                        </tr>
                    </div>

                      <div runat="server" visible="false" id="territory">
                        <tr>
                            <td class="TDLeft" width="13%">
                            </td>
                             <td class="TDRight" width="20%" style="text-align: right; padding-right: 10px;">
                             Tarritory: <span class="star-mark"> </span>
                        </td>
                            <td class="TDLeft" width="13%">
                                <asp:DropDownList ID="territoryDropDownList" runat="server" AutoPostBack="True" 
                                    CssClass="DropDown" Width="175px" 
>
                                </asp:DropDownList>
                            </td>
                            <td class="TDRight" width="20%">
                                &nbsp;</td>
                            <td class="TDLeft" width="13%">
                                &nbsp;
                            </td>
                            <td class="TDRight" width="20%">
                            </td>
                        </tr>
                    </div>

                    <tr>
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
                            &nbsp;
                        </td>
                        <td width="20%" class="TDRight">
                            &nbsp;
                        </td>
                        <td width="13%" class="TDLeft">
                            <asp:Button ID="submitButton" runat="server" CssClass="button-margin-right" OnClick="submitButton_OnClick" Text="View Report" />
                            <asp:Button ID="clearButton" runat="server" Text="Clear" CssClass="margin-right"
                                OnClick="clearButton_Click" BackColor="#F47322" />
                        </td>
                        <td width="20%" class="TDRight">
                           
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
                        </td>
                        <td width="20%" class="TDRight">
                            &nbsp;
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
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
