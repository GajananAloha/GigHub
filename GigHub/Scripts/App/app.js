function initGigs() {
    $(".js-toggle-attendance").click(function (e) {
        var btn = $(e.target);
        if (btn.hasClass("btn-default")) {
            $.post("/gighub/api/attendances", { "GigId": btn.attr("data-gig-id") })
        .done(function () {
            btn.removeClass("btn-default").addClass("btn-info").text("Going");
        })
        .fail(function () {
            alert("Something Went Wrong");
        })
        }
        else {
            $.ajax({
                url: "/gighub/api/attendances/" + btn.attr("data-gig-id"),
                method: "DELETE"
            })
        .done(function () {
            btn.removeClass("btn-info").addClass("btn-default").text("Going?");
        })
        .fail(function () {
            alert("Something Went Wrong");
        })
        }
    });
}