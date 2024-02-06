// Chapman Farm Almost 5K.
// Copyright (C) Eugene Bekker.

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace CFA5K.WebApp.Controls;

/// <summary>
/// QR Code component based on
/// <see href="https://github.com/davidshimjs/qrcodejs">qrcode.js</see>.
/// </summary>
public partial class QRCode : IAsyncDisposable
{
    [Parameter] public string Text { get; set; } = "(n/a)";
    [Parameter] public int? Width { get; set; } // QRCode.js default = 256
    [Parameter] public int? Height { get; set; } // QRCode.js default = 256
    [Parameter] public string? DarkColor { get; set; } // QRCode.js default = #000000
    [Parameter] public string? LightColor { get; set; } // QRCode.js default = #ffffff
    [Parameter] public int? TypeNumber{ get; set; } // QRCode.js default = 4
    [Parameter] public QRErrorCorrectLevel? CorrectLevel { get; set; } // QRCode.js default = QRErrorCorrectLevel.H

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Inject] private IJSRuntime JS { get; set; } = default!;

    private ElementReference _div = default!;
    private IJSObjectReference? _module;
    private bool _qrCodeCreated;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _module = await JS.InvokeAsync<IJSObjectReference>("import",
                "./Controls/QRCode.razor.js");
            await InvokeCreate();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_module != null)
        {
            await _module.DisposeAsync();
            _module = null;
        }
    }

    public async Task InvokeCreate()
    {
        // Based on the sample:
        /*
            <div id="qrcode"></div>
            <script type="text/javascript">
                // simple form
                new QRCode(document.getElementById("qrcode"), "http://jindo.dev.naver.com/collie");
                // -- OR -- with options
                var qrcode = new QRCode("test", {
                    text: "http://jindo.dev.naver.com/collie",
                    width: 128,
                    height: 128,
                    colorDark : "#000000",
                    colorLight : "#ffffff",
                    correctLevel : QRCode.CorrectLevel.H
                });
                // updates
                qrcode.clear(); // clear the code.
                qrcode.makeCode("http://naver.com"); // make another code.
            </script>
         */

        var qrOpts = new QRCodeOptions { ["text"] = Text }
            .AddIf("width", Width)
            .AddIf("height", Height)
            .AddIf("colorDark", DarkColor)
            .AddIf("colorLight", LightColor)
            .AddIf("typeNumber", TypeNumber)
            .AddIf("correctLevel", CorrectLevel)
            ;
        _qrCodeCreated = await _module!.InvokeAsync<bool>("createQRCode", _div, qrOpts);
    }

    public async Task InvokeUpdate(string text)
    {
        if (!_qrCodeCreated)
        {
            return;
        }
        await _module!.InvokeVoidAsync("updateQRCode", text);
    }

    public async Task InvokeDelete()
    {
        if (_qrCodeCreated)
        {
            return;
        }
        await _module!.InvokeVoidAsync("deleteQRCode");
        _qrCodeCreated = false;
    }

    // QRCode.js => QRErrorCorrectLevel = {L:1,M:0,Q:3,H:2}
    public enum QRErrorCorrectLevel
    {
        H = 2, // Listed first to make the default
        Q = 3,
        M = 0,
        L = 1,
    }
}

file class QRCodeOptions : Dictionary<string, object>
{
    public QRCodeOptions AddIf(string key, object? value)
    {
        if (value != null)
        {
            this.Add(key, value);
        }
        return this;
    }

    public QRCodeOptions SetIf(string key, object? value)
    {
        if (value != null)
        {
            this[key] = value;
        }
        return this;
    }
}
