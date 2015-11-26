<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GetWebSitesToJPG._Default"  Async="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-sm-12">
            <h1>Get Jpg from WebPage</h1>
            <p class="lead">ASP.NET is a free web framework for building great Web sites and Web applications using HTML, CSS, and JavaScript.</p>
            <p><a href="http://www.asp.net" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>
            <div>
                <asp:Label Text="Write URL:" runat="server" /><asp:TextBox Id="txtUrl" runat="server"></asp:TextBox>
                <asp:Button Text="Generate Preview" runat="server" class="btn btn-primary" OnClick="Preview_Gen_Click" CausesValidation="true" OnClientClick="ShowGif()"/>
                <!--<button type="button" id="btnCrop" class="btn btn-default"><i class="fa fa-crop"></i> Crop!</button>-->
                <button type="button" id="btnActCrop" class="btn btn-default">Activate Cropper</button>
                <div class="HiddenButtons">
                    <button type="button" id="btnHideCrop" class="btn btn-default"><i class="fa fa-eye-slash"></i></button>
                    <button type="button" id="btnCrop" class="btn btn-default"><i class="fa fa-crop"></i> Crop!</button>
                </div>
            </div>
            <div class="img-preview preview-lg"></div>
        </div>
        <div class="col-sm-12">
            <asp:Label id="lblMsg" Text="" runat="server" />
            <div class="cont">
                <img src="" id="imgPreview" runat="server" alt="" />
            </div>
            <div id="iresult" runat="server"></div>
            <div class="iframe-holder">
                <!--<div class="shader" style="width: 100%;"></div>-->
                <iframe runat="server" id="iframeLoader" src="Content/Images/Screenshots/result.html"   visible="false" style="width:100%;height:600px;border:0;background-color:#fff;"></iframe>
            </div>
            <!-- <canvas id="myCanvas">This text is displayed if your browser does not support HTML5 Canvas.</canvas>-->
        </div>
    </div>
    <!--<button type="button" id="copy" class="btn btn-primary">Copy</button>-->
    <script>
        var isVisible = false;
        var $cropperEl = null;
        function ShowGif() {
            $(".iframe-holder").height(600);
            if ($(".iframe-holder iframe").length > 0) {
                $(".iframe-holder iframe").css('display', 'none');
            }
        }

        
        $(document).ready(function () {
            var iframeVisible = '<%= IframeVisible%>'.toLowerCase();
            isVisible = (iframeVisible === 'true');
        });
        $('.iframe-holder iframe').load(function () {
            this.style.height = $(this.contentWindow.document).height() + 'px';
            //if (isVisible) {
                //var $iframeCloned = $('.iframe-holder iframe').clone();
                //var styles = {
               //     width: 0,
               //     height : 0
              //  };
                //styles.width = $(this).parent().width();
                //styles.height = $(this).parent().height();

               // $iframeCloned.css(styles);

                //$('.cropper-view-box').append($iframeCloned);

            //}
        });
        $('#btnActCrop').click(function () {
            //$('.shader').width('100%');
            if (isVisible){
                $('.HiddenButtons').show();
                var scroll = $('.iframe-holder > iframe').contents().find("body").scrollTop();
                //if ($cropperEl == null) {
                $cropperEl = $('.iframe-holder > iframe').cropper({
                        movable: false,
                        zoomable: false,
                        rotatable: false,
                        scalable: false
                });
                var canvasData = $cropperEl.cropper('getCropBoxData');
                //}
                //else {
               //     $cropperEl.cropper('crop');
               // }
            }
            else{
                alert('you must genereate a preview');
            }
        });
        $('#btnHideCrop').click(function () {
            //$('.shader').width(0);
            if ($cropperEl != null)
            {
                $cropperEl.cropper('destroy');
            }
            $('.HiddenButtons').hide();
        });
        
        $('#btnCrop').click(function () {

            var data = $cropperEl.cropper('getData');
            console.log(data);
            var txt_url = $('#MainContent_txtUrl').val();
            data.url = txt_url;
            //var imgUrl = $('#MainContent_imgPreview').attr('src');
            //var fileNameIndex = imgUrl.lastIndexOf("/") + 1;
            //var filename = imgUrl.substr(fileNameIndex);
            //data.filename = filename;
            data = JSON.stringify(data, null, 2);
            console.log(data);
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
                console.log(response.d);
            }
            function onError(response) {
                console.log(response.d);
            }
        });
    </script>

</asp:Content>
