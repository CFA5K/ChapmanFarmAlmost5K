
var _qrCodeDefs = [];
var _qrCodeIndex = 0;

/*
 * Creates a new instance of QRCode and returns a refernce ID
 * that can be used to access and update the QRCode.
 */
export function createQRCode(elm, opts) {
    var qrCode = new QRCode(elm, opts);
    _qrCodeDefs[_qrCodeIndex] = {
        target: elm,
        qrOpts: opts,
        qrCode: qrCode
    };

    // DBG:
    console.log(`QR Code created (${_qrCodeIndex}): ${_qrCodeDefs[_qrCodeIndex]}`);
    return _qrCodeIndex++;
}

/*
 * Updates the content of an existing QR Code.
 */
export function updateQRCode(refId, text) {
    var qrCode = _qrCodeDefs[refId];
    if (qrCode && qrCode.qrCode) {
        // DBG:
        console.log(`Got existing QR Code (${refId}): ${qrCode}`);
        qrCode.qrCode.makeCode(text);
    }
    else {
        console.error(`Invalid QR Code Reference ID (${refId})`);
    }
}

/*
 * Clears and deletes an existing QR Code and associated elements.
 */
export function deleteQRCode(refId) {
    var qrCode = _qrCodeDefs[refId];
    if (qrCode && qrCode.qrCode && qrCode.target) {
        // DBG:
        console.log(`Got existing QR Code (${refId}): ${qrCode}`);

        // The native implementation is broken, so we re-implement using the same logic:
        //  https://github.com/davidshimjs/qrcodejs/blob/04f46c6a0708418cb7b96fc563eacae0fbf77674/qrcode.js#L215
        // BROKEN:
        //qrCode.qrCode.clear();

        var t = qrCode.target;
        while (t.hasChildNodes()) {
            t.removeChild(t.lastChild);
        }
    }
    else {
        console.error(`Invalid QR Code Reference ID (${refId})`);
    }
}
