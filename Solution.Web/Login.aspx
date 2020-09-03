<%@ Page Title="SMC-IIMS Login" Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" EnableEventValidation="true" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title> SalesRoll - Login </title>
    <link rel="stylesheet" href="LoginAssets/css/style.css" media="screen" type="text/css" />
</head>
<body style="background-image: url(images/MaintingandProtecting.jpg)">
    <span > 
        <img src="css/Logo/zaslogo.png" style="width: 200px; height: 130px;  display: block; margin: 63px auto 0px;" alt="Alternate Text" /> 
        </span>
    <div id="login" style="margin-top: 6px !important">
        <div id="triangle">
        </div>
        
        
        <h1 style="font-size: 17px; color: white;">
            <span style="/*! font-size: ; */font-weight: bold;font-size: 30px;" > Salesroll </span> 
            <br />
           Log In        
        </h1>
        <form id="Form1" runat="server">
        <asp:TextBox ID="userNameTextBox" runat="server" CssClass="textbox" placeholder="Enter username"></asp:TextBox>
        <asp:TextBox ID="passwordTextBox" runat="server" CssClass="textbox" placeholder="Enter password"
            TextMode="Password"></asp:TextBox>
        <asp:Button ID="loginButton" runat="server" Text="Login" CssClass="login100-form-btn"
            OnClick="loginButton_Click" />
        <asp:Label ID="msgLabel" runat="server" Font-Size="Smaller" ForeColor="Black"></asp:Label>
        <%--<input type="email" placeholder="Email" />
        <input type="password" placeholder="Password" />
        <input type="submit" value="Log in" />--%>
        </form>
    </div>
    <script src='http://codepen.io/assets/libs/fullpage/jquery.js'></script>
    <script src="LoginAssets/js/index.js"></script>
</body>
</html>