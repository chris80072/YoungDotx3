$.ajaxPrefilter(function (options) {
    if (!options.beforeSend && options.type === 'Post') {
        options.beforeSend = function (xhr) {
            xhr.setRequestHeader("__RequestVerificationToken", $('[name=__RequestVerificationToken]').val());
        }
    }
});

function checkTimeout(obj) {
    return true;
}