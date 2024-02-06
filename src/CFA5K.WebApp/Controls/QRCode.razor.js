
var _target;
var _qrOpts;
var _qrCode;

/*
 * Creates a new instance of QRCode and returns a refernce ID
 * that can be used to access and update the QRCode.
 */
export function createQRCode(elm, opts) {
    _target = elm;
    _qrOpts = opts;
    _qrCode = new QRCode(elm, opts);

    // DBG:
    console.log(`QR Code created: ${_qrCode}`);

    return true;
}

/*
 * Updates the content of an existing QR Code.
 */
export function updateQRCode(text) {
    if (_qrCode) {
        // DBG:
        console.log(`Got existing QR Code: ${_qrCode}`);
        _qrCode.makeCode(text);
    }
    else {
        console.error("QR Code is missing");
    }
}

/*
 * Clears and deletes an existing QR Code and associated elements.
 */
export function deleteQRCode() {
    if (_qrCode && _target) {
        // DBG:
        console.log(`Got existing QR Code: ${_qrCode}`);

        // The native implementation is broken, so we re-implement using the same logic:
        //  https://github.com/davidshimjs/qrcodejs/blob/04f46c6a0708418cb7b96fc563eacae0fbf77674/qrcode.js#L215
        // BROKEN:
        //qrCode.qrCode.clear();

        while (_target.hasChildNodes()) {
            _target.removeChild(_target.lastChild);
        }
    }
    else {
        console.error("QR Code is missing");
    }
}
