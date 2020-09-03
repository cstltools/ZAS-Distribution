<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MainMasterPage.master"
    AutoEventWireup="true" CodeFile="RSMSetup.aspx.cs" Inherits="SInventory_UI_RSMSetup" %>
<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        form
        {
            /* set width in form, not fieldset (still takes up more room w/ fieldset width */
            margin: 0 auto;
            padding: 0;
        }
        
        .fieldsetArea
        {
            width: 30%;
            margin: 0 auto;
            text-align: center;
            border: 1px solid #fff;
            padding: 10px;
        }
        
        form fieldset legend
        {
            padding: 5px;
            font-size: 1.6em;
        }
        
        
        tr.spacer {
            border-spacing: 5em;
        }
        
        .left-align {
            text-align: left;
        }
        
        #ContentPlaceHolder1_UpdatePanel1 {
            background-color: #fff;
        }
        
        

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
                            RSM Setup
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
                            <span class="modetitle">Operational Mode: </span>
                            <asp:Label ID="modeLabel" CssClass="modelabel" runat="server" Text=""></asp:Label>
                            &nbsp;
                        </td>
                    </tr>
					
                    <tr>
                        <td class="TDLeft" width="13%">
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                        <td class="TDLeft" width="13%">
                            Company Name:<span class="star-mark"> * </span></td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="companyNameDropDownList" runat="server" 
                                CssClass="DropDown" AutoPostBack="True" 
                                onselectedindexchanged="companyNameDropDownList_SelectedIndexChanged">                               
                            </asp:DropDownList>
                            <asp:HiddenField ID="rsmIdHiddenField" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="updateStatusHiddenField" runat="server" />
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
                        <td class="TDLeft" width="13%">
                            Region Name:<span class="star-mark"> * </span></td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="regionDropDownList" runat="server" 
                                CssClass="DropDown" AutoPostBack="True">
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
                        <td class="TDLeft" width="13%">
                           Employee Name:<span class="star-mark"> * </span></td>
                        <td class="TDRight" width="20%">
                            <asp:DropDownList ID="employeeDropDownList" runat="server" 
                                CssClass="DropDown" AutoPostBack="True" OnSelectedIndexChanged="employeeDropDownList_OnSelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="TDLeft" width="13%">
                        </td>
                        <td class="TDRight" width="20%">
                        </td>
                    </tr>
                    
                    <tr>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            Is Vacant 
                        </td>
                        <td class="TDRight" width="20%">
                            <asp:CheckBox ID="vacantCheckBox" OnCheckedChanged="vacantCheckBox_OnCheckedChanged" AutoPostBack="True" runat="server" />
                            <span style="color: #9400d3; display: block; font-size: 10px;"> <strong>N.B.</strong> Check Is Vacent checkbox if RSM position is vacant  </span>
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
                            Is Active <span class="star-mark"> * </span></td>
                        <td class="TDRight" width="20%">
                            <asp:CheckBox ID="isActiveCheckBox" OnCheckedChanged="isActiveCheckBox_OnCheckedChanged" AutoPostBack="True" runat="server" />
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>

                    <tr ID="activeDiv" runat="server" Visible="True">
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            Active Date:<span class="star-mark"> * </span> </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="activeDateTextBox" runat="server" CssClass="TextBoxCalander" ></asp:TextBox>
                             <asp:ImageButton runat="server" AlternateText="Click to show calendar" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                TabIndex="4" ID="imgDate"></asp:ImageButton>
                             <ajaxToolkit:CalendarExtender ID="Date" runat="server" Format="dd-MMM-yyyy" TargetControlID="activeDateTextBox"
                                PopupButtonID="imgDate">
                            </ajaxToolkit:CalendarExtender>
                        </td>
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                    </tr>

                     <tr id="inActive" runat="server" Visible="false">
                        <td class="TDLeft" width="13%">
                            &nbsp;</td>
                        <td class="TDRight" width="20%">
                            &nbsp;</td>
                        <td class="TDLeft" width="13%">
                            In Active Date </td>
                        <td class="TDRight" width="20%">
                            <asp:TextBox ID="inActiveDateTextBox" runat="server" CssClass="TextBoxCalander" ></asp:TextBox>
                             <asp:ImageButton runat="server" AlternateText="Click to show calendar" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                TabIndex="4" ID="ImageButton1"></asp:ImageButton>
                             <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy" TargetControlID="inActiveDateTextBox"
                                PopupButtonID="ImageButton1">
                            </ajaxToolkit:CalendarExtender>
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
                                    <asp:Button ID="clearButton" CssClass="clearButton" BackColor="#E8BC31"  runat="server" OnClick="clearButton_OnClick" Text="Clear" />
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
                    
                    <tr>

                        <td class="TDRight" colspan="6">
                            <div id ="gridContainer1" style ="height:300px;overflow:auto;width:97%; margin: 0 auto; ">
                            <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False" 
                                CssClass="gridview" DataKeyNames="RSMId" 
                                onrowcommand="loadGridView_RowCommand" OnRowCreated="loadGridView_OnRowCreated">
                                <Columns>
                                    <asp:BoundField DataField="EmpMasterCode" HeaderText="Employee Code" />
                                    <asp:BoundField DataField="EmpName" HeaderText="RSM Name" />
                                    <asp:BoundField DataField="CompanyName" HeaderText="Company Name" />
                                    <asp:BoundField DataField="RegionName" HeaderText="Region Name" />
                                    <asp:BoundField DataField="IsActive" HeaderText="Active Status" />
                                    <asp:BoundField DataField="ActiveDate" HeaderText="Active Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                    <asp:BoundField DataField="InActiveDate" HeaderText="In Active Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                    
                                    <asp:BoundField DataField="EntryBy" HeaderText="Create By"/>
                                    <asp:BoundField DataField="EntryDate" HeaderText="Create Date" DataFormatString="{0:dd-MMM-yyyy}" />                                        
                                    <asp:BoundField DataField="UpdateBy" HeaderText="UpdateBy"/>
                                    <asp:BoundField DataField="UpdateDate" HeaderText="UpdateDate" DataFormatString="{0:dd-MMM-yyyy}" />
                                  
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="editImageButton" runat="server" 
                                                CommandArgument="<%# Container.DataItemIndex %>" CommandName="EditData" 
                                                ImageUrl="~/images/edit.png" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                           </div>
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
