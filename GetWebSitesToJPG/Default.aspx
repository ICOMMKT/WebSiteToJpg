<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GetWebSitesToJPG._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-sm-12">
            <h1>Get Jpg from WebPage</h1>
            <p class="lead">ASP.NET is a free web framework for building great Web sites and Web applications using HTML, CSS, and JavaScript.</p>
            <p><a href="http://www.asp.net" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>
            <div>
                <asp:Label Text="Write URL:" runat="server" /><asp:TextBox Id="txtUrl" runat="server"></asp:TextBox>
                <asp:Button Text="Generate Preview" runat="server" class="btn btn-primary" OnClick="Preview_Gen_Click"/>
                <button type="button" id="btnCrop" class="btn btn-default"><i class="fa fa-crop"></i> Crop!</button>
            </div>
            <div class="img-preview preview-lg"></div>
        </div>
        <div class="col-sm-12">
            <asp:Label id="lblMsg" Text="" runat="server" />
            <div class="cont">
                <img src="" id="imgPreview" runat="server" alt="" />
            </div>
            <div id="iresult" runat="server"></div>
            <iframe runat="server" id="iframeLoader" src="Content/Images/Screenshots/result.html" visible="false" style="width:100%;height:600px"></iframe>
            <!--<canvas id="myCanvas">This text is displayed if your browser does not support HTML5 Canvas.</canvas>-->
        </div>
    </div>
    <!--<button type="button" id="copy" class="btn btn-primary">Copy</button>-->
    <script>
        var $cropperEl = $('.cont > img').cropper({
            movable: false,
            zoomable: true,
            rotatable: false,
            scalable: true           
        });
        $('#btnCrop').click(function () {
            var data = $cropperEl.cropper('getData');
            console.log(data);
            var imgUrl = $('#MainContent_imgPreview').attr('src');
            var fileNameIndex = imgUrl.lastIndexOf("/") + 1;
            var filename = imgUrl.substr(fileNameIndex);
            data.filename = filename;
            data = JSON.stringify(data, null, 2);
            console.log(data);
            /*$.post("Default.aspx/CropImage", data)
                .done(function (data) {
                    console.log(data);
                });*/
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("Default.aspx/CropImage") %>',
                data: data,
                contentType: "application/json;",
                dataType: "json",
                successs: OnSuccess,
                error: onError
            });
            function OnSuccess(response) {
                console.log (response.d);
            }
            function onError(response) {
                console.log(response.d);
            }
        });
    </script>

</asp:Content>
