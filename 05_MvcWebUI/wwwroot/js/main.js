
$(document).ready(function () {
    var sideBar = sessionStorage.getItem("sidebar");
    if (sideBar == null) {
        sideBar = "1";
        sessionStorage.setItem("sidebar", "1");
    }
    if (sideBar === "1") {
        $('#i-collapse').removeClass('fa-chevron-right');
        $('#i-collapse').addClass('fa-chevron-left');
        $('#sidebarCollapse').attr('title', 'Soldaki Menüyü Kapat');
        $('#sidebar').removeAttr("style");
        $('#sidebar').removeClass("active");
    } else {
        $('#i-collapse').removeClass('fa-chevron-left');
        $('#i-collapse').addClass('fa-chevron-right');
        $('#sidebarCollapse').attr('title', 'Soldaki Menüyü Aç');
        $('#sidebar').css("display", "none");
        $('#sidebar').addClass("active");
    }
    $('#sidebarCollapse').on('click', function (event) {
        event.preventDefault();
        sideBar = sessionStorage.getItem("sidebar");
        if (sideBar === "1") {
            sideBar = "0";
        } else {
            sideBar = "1";
        }
        sessionStorage.setItem("sidebar", sideBar);
        window.location.reload();
    });
});