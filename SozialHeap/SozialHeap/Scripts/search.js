
$(function () {

    // tooltips in UserView
    $(function () {
        $('[data-toggle="tooltip"]').tooltip();
    })


    function startsearch()
    {
        document.location = '/Home/Search/' + $("#queryString").val();
    }
    $(document).ready(function () {
        $('#queryString').keypress(function (e) {
            if (e.which == 13 && $('#queryString').val() != '') {
                startsearch();
            }
        });
    });
    $('#headerSearch .form-control').autocomplete({
        source: "/Home/FindTopic/",
        minLength: 1,
        select: function (event, ui) {
            var url = ui;
        },
        html: true,
        open: function (event, ui) {
            // just to make sure it stays on top :)
            $(".ui-autocomplete").css("z-index", 9999990000);
        }
    });
    
});

