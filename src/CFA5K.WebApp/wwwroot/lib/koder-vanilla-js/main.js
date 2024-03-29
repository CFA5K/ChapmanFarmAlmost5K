////////////////////////
// Fix iOS AudioContext
////////////////////////
(function () {
    window.AudioContext = window.AudioContext || window.webkitAudioContext;
    if (window.AudioContext) {
        window.audioContext = new window.AudioContext();
    }
    const fixAudioContext = function (e) {
        if (window.audioContext) {
            // Create empty buffer
            const buffer = window.audioContext.createBuffer(1, 1, 22050);
            const source = window.audioContext.createBufferSource();
            source.buffer = buffer;
            // Connect to output (speakers)
            source.connect(window.audioContext.destination);
            // Play sound
            if (source.start) {
                source.start(0);
            } else if (source.play) {
                source.play(0);
            } else if (source.noteOn) {
                source.noteOn(0);
            }
        }
        // Remove events
        document.removeEventListener('touchstart', fixAudioContext);
        document.removeEventListener('touchend', fixAudioContext);
    };
    // iOS 6-8
    document.addEventListener('touchstart', fixAudioContext);
    // iOS 9
    document.addEventListener('touchend', fixAudioContext);
})();

////////////////
// Worker
////////////////
let worker = null;
if (navigator.serviceWorker) {
    navigator.serviceWorker.register('sw.js').then(function (registration) {
        console.log('ServiceWorker registration successful with scope:', registration.scope);
    }).catch(function (error) {
        console.log('ServiceWorker registration failed:', error);
    });
}

function initWorker() {
    worker = new Worker("wasmWorker.js");
    worker.onmessage = (ev) => terminateWorker(ev.data.data, ev.data.ms);
}

const terminateWorker = (data, millis) => {
    worker.terminate();
    beep();
    stopVideo();
    showCode(data, millis);
};

////////////////
// Constants
////////////////
const CANVAS_SIZE = {
    WIDTH: window.innerWidth,
    HEIGHT: window.innerHeight
};
const focusBoxWidth = 200;
const focusBoxHeight = 200;

////////////////
// Elements
////////////////
//const container = document.getElementById("canvas");
//const btnStart = document.getElementById("btnStart");
//const btnStop = document.getElementById("btnStop");
//const barcodeEl = document.getElementById("result");
//const millisEl = document.getElementById("time");
function initElements() {
    window.container = document.getElementById("canvas");
    window.btnStart = document.getElementById("btnStart");
    window.btnStop = document.getElementById("btnStop");
    window.barcodeEl = document.getElementById("result");
    window.millisEl = document.getElementById("time");
}

function init() {
    showElement(btnStart);
    hideElement(btnStop);
    btnStart.onclick = startVideo;
    btnStop.onclick = stopVideo;
    hideCanvas();
}

//init();
//hideElement(barcodeEl);

function initKoderVanilla() {
    alert("I'm starting...");
    initElements();
    init();
    hideElement(barcodeEl);
}

////////////////
// Canvas
////////////////
let ctx = container.getContext('2d');

////////////////
// Video
////////////////
let video = null;
let oldTime = 0;

function tick(time) {
    if (window.video.readyState === window.video.HAVE_ENOUGH_DATA) {
        showElement(btnStop);
        container.width = Math.min(CANVAS_SIZE.WIDTH, window.video.videoWidth);
        container.height = Math.min(CANVAS_SIZE.HEIGHT, window.video.videoHeight);

        const sx = (container.width - focusBoxWidth) / 2;
        const sy = (container.height - focusBoxHeight) / 2;

        ctx.drawImage(window.video, 0, 0);
        ctx.fillStyle = "black";
        ctx.globalAlpha = 0.6;
        ctx.fillRect(0, 0, container.width, container.height);
        ctx.drawImage(window.video, sx, sy, focusBoxWidth, focusBoxHeight, sx, sy, focusBoxWidth, focusBoxHeight);

        if (time - oldTime > 600) {
            oldTime = time;
            let imageData = ctx.getImageData(sx, sy, focusBoxWidth, focusBoxHeight);
            worker.postMessage({ data: imageData.data, width: imageData.width, height: imageData.height });
        }
    }
    requestAnimationFrame(tick);
}

////////////////
// Helpers
////////////////
function startVideo() {
    window.video = document.createElement("video");
    hideElement(btnStart);
    hideElement(barcodeEl);
    showCanvas();

    initWorker();
    navigator.mediaDevices.getUserMedia({ audio: false, window.video: { facingMode: "environment" } }).then(stream => {
        window.video.srcObject = stream;
        window.video.setAttribute("playsinline", "true");
        window.video.play();
        requestAnimationFrame(tick);
    });
}

function stopVideo() {
    hideElement(btnStop);
    window.video.pause();
    window.video.srcObject.getVideoTracks().forEach(track => track.stop());
    window.video.srcObject = null;
    init();
}

const beep = (freq = 750, duration = 150, vol = 5) => {
    try {
        const context = window.audioContext;
        const oscillator = context.createOscillator();
        const gain = context.createGain();
        oscillator.connect(gain);
        oscillator.frequency.value = freq;
        oscillator.type = "square";
        gain.connect(context.destination);
        gain.gain.value = vol * 0.01;
        oscillator.start(context.currentTime);
        oscillator.stop(context.currentTime + duration * 0.001);
    } catch (e) {
        console.warn("Sorry, Web Audio API is not supported by your browser");
        console.warn(e.toString());
    }
};

////////////////
// DOM
////////////////
function hideElement(el) {
    el.style.display = "none";
}

function showElement(el) {
    el.style.display = "initial";
}

function hideCanvas() {
    container.hidden = true;
}

function showCanvas() {
    container.hidden = false;
}

function showCode(qr, ms) {
    barcodeEl.style.display = "flex";
    barcodeEl.innerText = qr;
    millisEl.innerHTML = `<div>Scanned in ${ms} ms</div>`;
}