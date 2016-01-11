﻿<%@ Page Title="Login" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Login.aspx.cs" Inherits="GetWebSitesToJPG.Login" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
     <asp:PlaceHolder runat="server" ID="LoginStatus" Visible="false">
            <p>
               <asp:Literal runat="server" ID="StatusText" />
            </p>
         </asp:PlaceHolder>
    <asp:PlaceHolder runat="server" ID="logForm">
        <div style="margin-bottom: 10px">
            <asp:Label runat="server" AssociatedControlID="UserName">User name</asp:Label>
            <div>
                <asp:TextBox runat="server" ID="UserName" />
            </div>
        </div>
        <div style="margin-bottom: 10px">
            <asp:Label runat="server" AssociatedControlID="Password">Password</asp:Label>
            <div>
                <asp:TextBox runat="server" ID="Password" TextMode="Password" />
            </div>
        </div>
        <div style="margin-bottom: 10px">
            <div>
                <asp:Button runat="server" OnClick="SignIn" Text="Log in" CausesValidation="true" OnClientClick="return validate()"/>
            </div>
        </div>
    </asp:PlaceHolder>
    <asp:HyperLink runat="server" ID="RegisterHyperLink" ViewStateMode="Disabled">Register as a new user</asp:HyperLink>
    <script>
        $(document).ready(function () {
            $('#MainContent_UserName').focus();
        });
        function validate() {
            if ($('#MainContent_UserName').val() == "") {
                alert('You must enter a Username');
                return false;
            }
            if ($('#MainContent_Password').val() == "") {
                alert('You must enter a password');
                return false;
            }
        }
    </script>
</asp:Content>
