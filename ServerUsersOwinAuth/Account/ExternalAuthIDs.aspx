<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ExternalAuthIDs.aspx.cs" Inherits="ServerUsersOwinAuth.Account.ExternalAuthIDs" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1>List of URLs authorized</h1>
    <div class="modal fade" tabindex="-1" role="dialog" aria-labelledby="ModalCreateAuthURLs">
        <div class="modal-dialog ">
            <div class="modal-content">
                <h3 style="padding-left: 12px;">Enter the new Url:</h3>
                <div style="padding: 20px;">
                    <asp:TextBox ID="txtUrl" runat="server"></asp:TextBox>
                    <label>App Name</label>
                    <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                    <asp:Button ID="btnCreate" runat="server" Text="Create" OnClick="btnCreate_Click" class="btn btn-primary" />
                </div>
            </div>
        </div>
    </div>
    <asp:PlaceHolder runat="server" ID="StatusPane" Visible="false">
        <p>
            <asp:Literal runat="server" ID="StatusText" />
        </p>
    </asp:PlaceHolder>
    <div style="margin-bottom: 15px">
        <button type="button" id="btnCreateClient" data-toggle="modal" data-target=".modal" class="btn">Create New</button>
    </div>
    <div style="width: 60%;">
        <asp:GridView ID="grdVAuthLogins" runat="server" CssClass="table" border="0"></asp:GridView>
    </div>
    <script type="text/javascript">
        $('.modal').on('shown.bs.modal', function () {
            $('#MainContent_txtUrl').focus()
        })
    </script>
</asp:Content>
