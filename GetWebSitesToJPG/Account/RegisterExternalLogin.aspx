<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="RegisterExternalLogin.aspx.cs" Inherits="GetWebSitesToJPG.Account.RegisterExternalLogin" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <asp:PlaceHolder runat="server" ID="LoginStatus" Visible="false">
        <p>
            <asp:Literal runat="server" ID="StatusText" />
        </p>
    </asp:PlaceHolder>
    <hgroup class="title">
        <h1>Register with your <%: ProviderDisplayName %> account</h1>
        <h2><%: ProviderUserName %>.</h2>
    </hgroup>

    
    <asp:ValidationSummary runat="server" />    
    

    <asp:PlaceHolder runat="server" ID="userNameForm">
        <fieldset>
            <legend>Association Form</legend>
            <p>
                You've authenticated with <strong><%: ProviderDisplayName %></strong> as
                <strong><%: ProviderUserName %></strong>. Please enter a user name below for the current site
                and click the Log in button.
            </p>
            <ol>
                <li class="email">
                    <asp:Label ID="Label1" runat="server" AssociatedControlID="Username">User name</asp:Label>
                    <asp:TextBox runat="server" ID="Username" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Username"
                        Display="Dynamic" ErrorMessage="User name is required" ValidationGroup="NewUser" />
                </li>
            </ol>
            <asp:Button ID="Button1" runat="server" Text="Log in" ValidationGroup="NewUser" OnClick="LogIn_Click" />
        </fieldset>
    </asp:PlaceHolder>
</asp:Content>
