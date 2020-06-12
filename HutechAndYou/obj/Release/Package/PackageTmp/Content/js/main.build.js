$(document).ready(function () {
    var winWidth = $(window).width(); 
    $('#slide-sidebar').on("click", function () {
        if ($(this).prop('checked')) {
            $('#sidebar').css('width', $(window).width() >= 425 ? 335 : '100%' );
            $('#sidebar ul').css('display', 'block');
            $('.custom-name').css('display', 'block');
        }
        else {
            $('#sidebar').css('width', 0);
            $('#sidebar ul').css('display', 'none');
            $('.custom-name').css('display', 'none');
        }
    });
});

function ErrorNotify(str) {
    $("#notification").data("kendoNotification").show({
        message: str
    }, "error");
}
function SuccessNotify(str) {
    $("#notification").data("kendoNotification").getNotifications().parent().remove();
    $("#notification").data("kendoNotification").show({
        message: str
    }, "success");
}
function CloseNotify() {
    $("#notification").data("kendoNotification").hide();
}


function sideScroll(element, direction, speed, distance, step) {
    scrollAmount = 0;
    var slideTimer = setInterval(function () {
        if (direction == 'left') {
            element.scrollLeft -= step;
        } else {
            element.scrollLeft += step;
        }
        scrollAmount += step;
        if (scrollAmount >= distance) {
            window.clearInterval(slideTimer);
        }
    }, speed);
}

function showLoadingOverlay(selector) {
    if (selector) {
        $(selector).LoadingOverlay("show");
    } else {
        $.LoadingOverlay("show");
    }
}

function hideLoadingOverlay(selector) {
    if (selector) {
        $(selector).LoadingOverlay("hide");
    } else {
        $.LoadingOverlay("hide");
    }
}
$.notifyDefaults({
    offset: {
        y: 25
    },
    placement: {
        from: "top",
        align: "center"
    },
    animate: {
        enter: 'animated fadeInDown',
        exit: 'animated fadeOutUp'
    },
    delay: 3000
});
//$.LoadingOverlaySetup({
//    image: "/Content/Images/loading.gif",
//    maxSize: "20px",
//    minSize: "20px"
//});