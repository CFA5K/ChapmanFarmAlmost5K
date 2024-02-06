// Chapman Farm Almost 5K.
// Copyright (C) Eugene Bekker.

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace CFA5K.WebApp.Controls;

/// <summary>
/// QR Code component based on
/// <see href="https://github.com/davidshimjs/qrcodejs">qrcode.js</see>.
/// </summary>
public partial class QRCodes : IAsyncDisposable
{
    [Parameter] public IEnumerable<string> Texts { get; set; } = Enumerable.Empty<string>();
    [Parameter] public RenderFragment<QRCodeRef>? ChildContent { get; set; }
    [Parameter] public int? Width { get; set; } // QRCode.js default = 256
    [Parameter] public int? Height { get; set; } // QRCode.js default = 256
    [Parameter] public string? DarkColor { get; set; } // QRCode.js default = #000000
    [Parameter] public string? LightColor { get; set; } // QRCode.js default = #ffffff
    [Parameter] public int? TypeNumber{ get; set; } // QRCode.js default = 4
    [Parameter] public QRErrorCorrectLevel? CorrectLevel { get; set; } // QRCode.js default = QRErrorCorrectLevel.H

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Inject] private IJSRuntime JS { get; set; } = default!;

    private IJSObjectReference? _module;

    private string _idPrefix = Guid.NewGuid().ToString();
    private List<int>? _refIds;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _module = await JS.InvokeAsync<IJSObjectReference>("import",
                "./Controls/QRCodes.razor.js");
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
        if (_refIds != null)
        {
            return;
        }

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

        _refIds = new();

        var qrOpts = new QRCodeOptions()
            .AddIf("width", Width)
            .AddIf("height", Height)
            .AddIf("colorDark", DarkColor)
            .AddIf("colorLight", LightColor)
            .AddIf("typeNumber", TypeNumber)
            .AddIf("correctLevel", CorrectLevel)
            ;

        var ndx = 0;
        foreach (var t in Texts)
        {
            qrOpts["text"] = t;
            var id = $"{_idPrefix}_{ndx++}";
            _refIds.Add(await _module!.InvokeAsync<int>("createQRCode", id, qrOpts));
        }
    }

    public async Task InvokeUpdate(int index, string text)
    {
        if (_refIds == null)
        {
            return;
        }

        await _module!.InvokeVoidAsync("updateQRCode", _refIds[index], text);
    }

    public async Task InvokeDelete(int? index = null)
    {
        if (_refIds == null)
        {
            return;
        }

        foreach (var ndx in (index == null ? _refIds : [_refIds[index.Value]]))
        {
            await _module!.InvokeVoidAsync("deleteQRCode", _refIds[ndx]);
        }

        _refIds.Clear();
        _refIds = null;
    }

    public record QRCodeRef(
        string ElementId,
        IReadOnlyDictionary<string, object>? AdditionalAttributes);

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
