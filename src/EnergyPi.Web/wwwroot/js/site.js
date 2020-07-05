// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function executeApiCall(url, method, responseDataType, successCallback, errorCallback) {
    var options = {};
    options.url = url;
    options.type = method;
    options.dataType = responseDataType;
    options.success = successCallback;
    options.error = errorCallback;
    $.ajax(options);
}

function executeHttpGet(url, successCallback, errorCallback) {
    executeApiCall(url, "GET", "json", successCallback, errorCallback);
}
