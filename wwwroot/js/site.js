window.productAddedAlert = function () {
    alert('Product added to the table!');
};

window.scrollTableToBottom = function () {
    var container = document.getElementById('checkoutTable');
    if (container) {
        container.scrollTop = container.scrollHeight;
    }
};