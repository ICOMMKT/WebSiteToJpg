<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IcommktAuthButton.ascx.cs" Inherits="GetWebSitesToJPG.Account.IcommktAuthButton" %>
<fieldset class="open-auth-providers">
    <legend>Log in using another service</legend>
    <asp:ListView runat="server" ID="providerDetails" ItemType="System.String"
        SelectMethod="GetProviderNames" ViewStateMode="Disabled">
        <ItemTemplate>  
            <button type="submit" class="btn btn-default" name="provider" value="<%#: Item %>"
                title="Log in using your <%#: Item %> account.">
                <%#: Item %>
            </button>   
        </ItemTemplate>
        <EmptyDataTemplate>
            <div>
                <p>There are no external authentication services configured. See <a href="http://go.microsoft.com/fwlink/?LinkId=252803">this article</a> for details on setting up this ASP.NET application to support logging in via external services.</p>
            </div>
        </EmptyDataTemplate>
    </asp:ListView>
</fieldset>