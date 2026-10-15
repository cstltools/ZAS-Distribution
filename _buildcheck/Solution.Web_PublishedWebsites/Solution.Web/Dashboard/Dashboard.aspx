<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="Dashboard_Dashboard, App_Web_ca5zefwb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
    <script src="../LoginAssets/js/Chart.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    
   <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
            <div>
                <table width="100%" class="TableWorkArea">
                    <tr>
                        <td colspan="6" class="TableHeading">
                            Pack Size Entry
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
                            Unit Name <span class="star-mark"> * </span></td>
                        <td width="20%" class="TDRight">
                            <asp:DropDownList ID="companyNameDropDownList" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="companyNameDropDownList_OnSelectedIndexChanged" CssClass="DropDown">
                                </asp:DropDownList>
                        </td>
                        <td width="13%" class="TDLeft">
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
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>
                </table>
            </div>
        <%--</ContentTemplate>
    </asp:UpdatePanel>
    --%>
    <div>
        <asp:Literal ID="ltChart" runat="server"></asp:Literal>
    </div>
   

</asp:Content>

