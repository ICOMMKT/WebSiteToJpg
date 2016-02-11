<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Site.Master" CodeBehind="authenticate.aspx.cs" Inherits="PruebaOwinIconmkt.oauth.authenticate" Async="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:PlaceHolder runat="server" ID="ErrorMessage" >
        <asp:Literal Id="FailureText" Text="" runat="server" />
    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server" ID="LogForm" >
        <h1>Log to your account</h1>    
        <div>
            <label>Username:</label>
            <asp:TextBox runat="server" Id="txt_Username"/>
        </div>
        <div>
            <label>Password:</label>
            <asp:TextBox runat="server" Id="txt_Password" TextMode="Password"/>

        </div>
        <div>
            <asp:Button Text="Log In" runat="server" Id="btnLog" CssClass="btn" OnClick="LogIn"/>
        </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server" Id="AuthPrompt" Visible="false">
        <h3>An application would like to connect to your account</h3>
        <p>The app <%= AppName %> would like the ability to access your basic information.</p>
        <div style="text-align:center">
            <p>Allow <%= AppName %> access?</p>
            <asp:Button Text="Deny" CssClass="btn" OnClick="DenyAccess" ID="btnDeny" runat="server"/><button type="button" style="margin-left:20px;" class="btn btnAllow"> Allow</button>
        </div>
    </asp:PlaceHolder>
    <asp:HiddenField ID="hdn_Id" runat="server" />
    <script>
        $(document).ready(function () {
            $('.btnAllow').click(function () {
                var id = $('#MainContent_hdn_Id').val();
                $.ajax({
                    type: "POST",
                    url: "authenticate.aspx/AllowAccess",
                    data: "{id:'" + id + "'}",
                    contentType: "application/json",
                    dataType: "json",
                    success: function (msg) {
                        console.log(msg.d);
                        var response = JSON.parse(msg.d);
                        var token_uri = response.RedirectUri + "?token=" + response.Token + "&state="+ response.State;
                        window.location.href = token_uri;
                    }
                });
            });
        });
    </script>
</asp:Content>