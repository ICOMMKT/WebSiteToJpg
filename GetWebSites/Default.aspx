<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="WebPageToJPG._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-sm-6">
            <h1>Get Jpg from WebPage</h1>
            <p class="lead">ASP.NET is a free web framework for building great Web sites and Web applications using HTML, CSS, and JavaScript.</p>
            <p><a href="http://www.asp.net" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>
            <div>
                <asp:Label Text="Write URL:" runat="server" /><asp:TextBox Id="txtUrl" runat="server"></asp:TextBox>
                <asp:Button Text="Generate Preview" runat="server" class="btn btn-primary" OnClick="Preview_Gen_Click"/>
                <button type="button" id="btnCrop"><i class="fa fa-crop"></i> Crop!</button>
            </div>
            <div class="img-preview preview-lg"></div>
        </div>
        <div class="col-sm-6">
            <div class="cont">
                <img src="" id="imgPreview" runat="server" alt="" />
            </div>
            <div id="iresult" runat="server"></div>
            <!--<canvas id="myCanvas">This text is displayed if your browser does not support HTML5 Canvas.</canvas>-->
        </div>
    </div>
    <!--<button type="button" id="copy" class="btn btn-primary">Copy</button>-->
    <script>
        var $cropperEl = $('.cont > img').cropper({
            aspectRatio: NaN,
            preview: ".img-preview"
        });
        $('#btnCrop').click(function () {
            var data = $cropperEl.cropper('getData');
            console.log(data);
            data = JSON.stringify(data, null, 2);
            console.log(data);
            /*$.post("Default.aspx/CropImage", data)
                .done(function (data) {
                    console.log(data);
                });*/
            $.ajax({
                type: "POST",
                url: "Default.aspx/CropImage",
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
        /*$("iframe").zoomer({
            zoom: 0.5,
            width: 200
        });*/
        /*$("#copy.btn").click(function () {
            var iresult = document.getElementById("iresult");
            html2canvas(iresult, {
                logging: true,
                useCORS: true,
                onrendered: function (canvas) {
                    document.body.appendChild(canvas);
                }
            });
        });*/
        /*function LoadImg(imgUrl)
        {
            var canvas = document.getElementById('myCanvas');
            var context = canvas.getContext('2d');
            var imageObj = new Image();

            imageObj.onload = function() {
                var sourceWidth = this.width;
                var sourceHeight = this.height;
                var destWidth = sourceWidth;
                var destHeight = sourceHeight;
                canvas.width = sourceWidth;
                canvas.height = sourceHeight;
                context.drawImage(imageObj, 10, 10);
            };
            imageObj.src = imgUrl;
            //imageObj.src = 'http://www.html5canvastutorials.com/demos/assets/darth-vader.jpg';
        }
        (function ($) {
            $(function () {
                LoadImg('Content/Images/Screenshots/file1.jpg');
                init2();
            });
        })(jQuery);*/
    </script>
</asp:Content>