<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MainMasterPage.master"
    AutoEventWireup="true" CodeFile="SalesHubReport2.aspx.cs" Inherits="SInventory_UI_SalesHubReport2" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

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
                        
                        <td width="13%" class="TDLeft"  style="text-align: right; padding-right: 10px;">
                            Report Name: <span class="star-mark"> * </span>
                        </td>
                        <td width="20%" class="TDRight">
                             <asp:DropDownList ID="reportNameDropDownList" runat="server" Width="175px" AutoPostBack="True"
                                 CssClass="DropDown">
                               <asp:ListItem Text="---- Select any one ----" ></asp:ListItem>
                                <%--<asp:ListItem Text="Region Report" Value="REGION"></asp:ListItem>--%>
                                <asp:ListItem Text="Area Report" Value="AREA"></asp:ListItem>
                                <asp:ListItem Text="Territory Report" Value="TERRITORY"></asp:ListItem>
                                <asp:ListItem Text="Market Report" Value="MARKET"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        

                        <td width="13%" class="TDLeft"  style="text-align: right; padding-right: 10px;">
                            Company: <span class="star-mark"> * </span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="manufacDropDownList" runat="server" AutoPostBack="True" CssClass="DropDown"
                                OnSelectedIndexChanged="manufacDropDownList_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft"  style="text-align: right; padding-right: 10px;">
                            Sales Center: <span class="star-mark"> * </span>
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="salesCenterDropDownList" runat="server" AutoPostBack="True"
                                CssClass="DropDown" OnSelectedIndexChanged="salesCenterDropDownList_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>

                        
                    </tr>
                    <tr>
                        <td width="13%" class="TDLeft"  style="text-align: right; padding-right: 10px;">
                            Area:
                        </td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="areaDropDownList" runat="server" AutoPostBack="True" CssClass="DropDown"
                                OnSelectedIndexChanged="areaDropDownList_OnSelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
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
                                >
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
