<%@ Page Language="C#" Title="Register" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Register.aspx.cs" Inherits="GetWebSitesToJPG.Register" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div>
    <h4 style="font-size: medium">Register a new user</h4>
        <hr />
        <p>
            <asp:Literal runat="server" ID="StatusMessage" />
        </p>                
        <div style="margin-bottom:10px">
            <asp:Label runat="server" AssociatedControlID="UserName">User name</asp:Label>
            <div>
                <asp:TextBox runat="server" ID="UserName" />                
            </div>
        </div>
        <div style="margin-bottom:10px">
            <asp:Label runat="server" AssociatedControlID="Password">Password</asp:Label>
            <div>
                <asp:TextBox runat="server" ID="Password" TextMode="Password" />                
            </div>
        </div>
        <div style="margin-bottom:10px">
            <asp:Label runat="server" AssociatedControlID="ConfirmPassword">Confirm password</asp:Label>
            <div>
                <asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" />                
            </div>
        </div>
        <div>
            <div>
                <asp:Button runat="server" OnClick="CreateUser_Click" Text="Register" CausesValidation="true" OnClientClick="return validate()" />
            </div>
        </div>
    </div>
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
            else {
                if ($('#MainContent_ConfirmPassword').val() == "") {
                    alert('You must confirm your password');
                    return false;
                }
            }

        }
    </script>
</asp:Content>
