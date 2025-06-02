window.productAddedAlert = function () {
    alert('Product added to the table!');
};

window.scrollTableToBottom = function () {
    var container = document.getElementById('checkoutTable');
    if (container) {
        container.scrollTop = container.scrollHeight;
    }
};

window.focusElement = function (element) {
    element.focus();
};


// QR Code generation function using QRCode.js library
window.generateQRCode = (elementId, text) => {
    const element = document.getElementById(elementId);
    if (element && window.QRCode) {
        // Clear any existing content
        element.innerHTML = '';
        
        // Generate actual QR code that will work for navigation
        QRCode.toCanvas(element, text, {
            width: 120,
            height: 120,
            margin: 2,
            color: {
                dark: '#000000',
                light: '#ffffff'
            },
            errorCorrectionLevel: 'M'
        }, function (error) {
            if (error) {
                console.error('QR Code generation failed:', error);
                // Fallback: show text link
                element.innerHTML = `<a href="${text}" target="_blank" style="font-size: 12px; color: #666;">${text}</a>`;
            }
        });
    } else {
        // Fallback if QRCode library is not available
        console.warn('QRCode library not available');
        element.innerHTML = `<a href="${text}" target="_blank" style="font-size: 12px; color: #666;">${text}</a>`;
    }
};