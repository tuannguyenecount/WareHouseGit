
function ShowPopUpSuccess(message) {
    $("#modal-success .box-body  p").html(message);
    $("button[data-target='#modal-success']").click();
}
function ShowPopUpError(message) {
    $("#modal-error .box-body  p").html(message);
    $("button[data-target='#modal-error']").click();
}
function CloseModal(modal)
{
    modal.modal('hide');
    $('body').removeClass('modal-open');
    $(".modal-backdrop").remove();
}
function RemoveModal(modal)
{
    modal.remove();
    $('body').removeClass('modal-open');
    $(".modal-backdrop").remove();
}
function ReplareUrl(url)
{
    document.location.replace(url);
}
function OnSuccessAjax1(result) {
    if (result.status == 1) {
        ReplareUrl(result.urlReferer);
    }
    else {
        ShowPopUpError(result.message);
    }
}
function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}