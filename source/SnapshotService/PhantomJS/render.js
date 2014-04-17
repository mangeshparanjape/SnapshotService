var page = require('webpage').create();
var system = require('system');
var fs = require('fs');
var address = system.args[1];
var output = system.args[2];
var viewportSizeWidth = system.args[3];
var viewportSizeHeight = system.args[4];
var clipRectangleWidth = system.args[5];
var clipRectangleHeight = system.args[6];
var imageType = system.args[7];
var resourceTimeout = system.args[8];
var pageRenderDelay = system.args[9];
var logFile = 'Log/log.txt';

page.settings.userAgent = 'Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/33.0.1750.117 Safari/537.36';
page.settings.resourceTimeout = resourceTimeout;
page.clipRect = { top: 0, left: 0, width: clipRectangleWidth, height: clipRectangleHeight };
page.viewportSize = { width: viewportSizeWidth, height: viewportSizeHeight };

phantom.onError = function (msg, trace) {
    //var msgStack = ['PHANTOM ERROR: ' + msg];
    //if (trace && trace.length) {
    //  msgStack.push('TRACE:');
    //trace.forEach(function(t) {
    //  msgStack.push(' -> ' + (t.file || t.sourceURL) + ': ' + t.line + (t.function ? ' (in function ' + t.function + ')' : ''));
    //});
    //}

    //writeLog(msgStack);
    phantom.exit(1);
};

page.onError = function (msg, trace) {
    //va msgStack = ['= onError()'];
    //msgStack.push('  ERROR: ' + msg);
    //if (trace) {
    //msgStack.push('  TRACE:');
    //trace.forEach(function (t) {
    //msgStack.push('    -> ' + t.file + ': ' + t.line + (t.function ? ' (in function "' + t.function + '")' : ''));
    //});
    //}

    //writeLog(msgStack);

    //phantom.exit(1);
};

console.log("Opening " + address + " ...");

//page.onResourceRequested = function (request) {
//    var msgStack =['= onResourceRequested()'];
//    msgStack.push('  request: ' + JSON.stringify(request, undefined, 4));
//	writeLog(msgStack);
//};

//page.onResourceReceived = function(response) {
//    var msgStack =['= onResourceReceived()'];
//    msgStack.push('  id: ' + response.id + ', stage: "' + response.stage + '", response: ' + JSON.stringify(response));
//	writeLog(msgStack);
//};

//page.onLoadStarted = function() {
//    var msgStack =['= onLoadStarted()'];
//    var currentUrl = page.evaluate(function() {
//        return window.location.href;
//    });
//    msgStack.push('  leaving url: ' + currentUrl);
//	writeLog(msgStack);
//};

//page.onLoadFinished = function(status) {
//    var msgStack =['= onLoadFinished()'];
//    msgStack.push('  status: ' + status);
//	writeLog(msgStack);
//};

//page.onNavigationRequested = function(url, type, willNavigate, main) {
//    var msgStack =['= onNavigationRequested'];
//    msgStack.push('  destination_url: ' + url);
//    msgStack.push('  type (cause): ' + type);
//    msgStack.push('  will navigate: ' + willNavigate);
//    msgStack.push('  from page\'s main frame: ' + main);
//	writeLog(msgStack);
//};

//page.onResourceError = function(resourceError) {
//    var msgStack =['= onResourceError()'];
//    msgStack.push('  - unable to load url: "' + resourceError.url + '"');
//    msgStack.push('  - error code: ' + resourceError.errorCode + ', description: ' + resourceError.errorString );
//	writeLog(msgStack);
//};



getSNapshot();

function writeLog(msgStack) {
    try {
        fs.write(logFile, "\n" + msgStack.join('\n') + "\n", 'a');
    }
    catch (err) {
    }
}

function getSNapshot() {
    try {
        page.open(address, function (status) {

            if (status !== 'success') {
                phantom.exit();
            } else {

                window.setTimeout(function () {
                    console.log("Rendering screenshot " + output + " for " + address);
                    page.render("Images/" + output, { format: imageType, quality: 100 });
                    phantom.exit();
                }, pageRenderDelay);
            }

        });
    }
    catch (err) {
        txt = "Error description: " + err.message + "\n\n";
        writeLog(txt);
        phantom.exit();
    }
}