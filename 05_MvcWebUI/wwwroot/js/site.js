$(function () {
    $('[data-toggle="popover"]').popover({
        placement: 'bottom',
        content: function () {
            return $("#notification-content").html();
        },
        html: true
    });

    $('body').append(`<div class="hide" id="notification-content"></div>`)


    function getNotification() {
        var res = "<ul class='list-group notifications'>";
        $.ajax({
            url: "/Notification/getNotification",
            method: "GET",
            success: function (result) {

                if (result.count != 0) {
                    $("#notificationCount").html(result.count);
                    $("#notificationCount").show('slow');
                } else {
                    $("#notificationCount").html();
                    $("#notificationCount").hide('slow');
                    $("#notificationCount").popover();
                }

                var notifications = result.userNotification;
                notifications.forEach(element => {
                    res = res + "<li class='list-group-item notification-text' id='" + element.notification.id + "'>" + element.notification.text + "</li>";
                });

                res = res + "</ul>";

                $("#notification-content").html(res);
            },
            error: function (error) {
                console.log(error);
            }
        });
    }

    $(document).on('click', 'ul.notifications li.notification-text', function (e) {
        var target = e.target;
        var id = $(target).attr('id');

        readNotification(id, target);
    });

    function readNotification(id, target) {
        $.ajax({
            url: "/Notification/ReadNotification",
            method: "GET",
            data: { notificationId: id },
            success: function (result) {
                getNotification();
                $(target).fadeOut('slow');
            },
            error: function (error) {
                console.log(error);
            }
        })
    }

    getNotification();
});