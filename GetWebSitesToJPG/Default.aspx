<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GetWebSitesToJPG._Default" Async="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Modal -->
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="myModalLabel">Image URL</h4>
                </div>
                <div class="modal-body"></div>
            </div>
        </div>
    </div>
    <div id="mainContent" class="row">
        <div class="col-sm-3">
            <div class="control-panel">
                <h1>Get JPG from WebPage</h1>
                <div>
                    <asp:TextBox ID="txtUrl" runat="server" placeholder="URL" CssClass="watermark-out"></asp:TextBox>
                    <asp:Button Text="Generate Preview" runat="server" class="btn btn-primary genPrev" OnClick="Preview_Gen_Click" CausesValidation="true" OnClientClick="return ShowGif()" />
                    <button type="button" id="btnActCrop" class="btn btn-default">Activate Cropper</button>
                    <div class="HiddenButtons">
                        <button type="button" id="btnHideCrop" class="btn btn-default"><i class="fa fa-eye-slash"></i>Turn off Cropper</button>
                        <button type="button" id="btnCrop" class="btn btn-default"><i class="fa fa-crop"></i>Crop!</button>
                    </div>
                </div>
                <div class="img-preview preview-lg"></div>
            </div>
        </div>
        <div class="col-sm-9">
            <asp:Label ID="lblMsg" Text="" runat="server" />
            <div class="cont">
                <img src="" id="imgPreview" runat="server" alt="" />
            </div>
            <div id="iresult" runat="server"></div>
            <!--<asp:PlaceHolder runat="server" ID="ContentLoaded" Visible="false">
                <p>
                    <asp:Literal runat="server" ID="StatusText" />
                </p>
            </asp:PlaceHolder>-->
            <div class="iframe-holder">
                <!--<div class="shader" style="width: 100%;"></div>-->
                <iframe runat="server" id="iframeLoader" src="Content/Images/Screenshots/result.html" visible="false" style="width: 100%; height: 600px; border: 0; background-color: #fff;"></iframe>
            </div>
            <!-- <canvas id="myCanvas">This text is displayed if your browser does not support HTML5 Canvas.</canvas>-->
        </div>
    </div>
    <!--<button type="button" id="copy" class="btn btn-primary">Copy</button>-->
    <script>
        var isVisible = false;
        var isFullLoaded = false;
        var $cropperEl = null;

        function ShowGif() {
            var textbox = $('#MainContent_txtUrl').val();
            if (textbox == "" || textbox == undefined) {
                alert("The field URL cannot be empty");
                return false;
            }
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
            $('.iframe-holder').height($(this.contentWindow.document).height());
            $('.iframe-holder').width($(this.contentWindow.document).width());
            //this.style.height = $(this.contentWindow.document).height() + 'px';
            //this.style.width = $(this.contentWindow.document).width() + 'px';
            isFullLoaded = true;
        });

        $('#btnActCrop').click(function () {
            if (isVisible) {
                if (isFullLoaded) {
                    $('.HiddenButtons').show();
                    var scroll = $('.iframe-holder > iframe').contents().find("body").scrollTop();

                    $cropperEl = $('.iframe-holder > iframe').cropper({
                        movable: false,
                        zoomable: false,
                        rotatable: false,
                        scalable: false
                    });

                    var canvasData = $cropperEl.cropper('getCropBoxData');
                } else {
                    alert('Wait until the page is full loaded.');
                }
            }
            else {
                alert('you must genereate a preview');
            }
        });

        $('#btnHideCrop').click(function () {
            //$('.shader').width(0);
            if ($cropperEl != null) {
                $cropperEl.cropper('destroy');
            }
            $('.HiddenButtons').hide();
        });

        $('#btnCrop').click(function () {
            var data = $cropperEl.cropper('getData');
            console.log(data);
            var txt_url = $('#MainContent_txtUrl').val();
            data.url = txt_url;
            data = JSON.stringify(data, null, 2);
            console.log(data);
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("Default.aspx/CropImage") %>',
                data: data,
                contentType: "application/json;",
                dataType: "json",
            }).done(function (data) {
                console.log(data.d);
                var $a = $('<a></a>', {
                    href: data.d,
                    text: data.d,
                    target: "_blank"
                });
                $('#myModal .modal-body').empty();
                $('#myModal .modal-body').append($a);
                $('#myModal').modal();
                //alert(data.d);
            });
        });

        $('#MainContent_txtUrl').focusin(function () {
            $(this).removeClass('watermark-out').addClass('watermark-in');
        }).focusout(function () {
            $(this).removeClass('watermark-in').addClass('watermark-out');
        });
    </script>
</asp:Content>
