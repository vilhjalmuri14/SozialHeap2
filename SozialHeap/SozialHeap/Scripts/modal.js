/* Focus correctly when modal dialog are shown */
$(function () {

    $('#loginmodal').on('shown.bs.modal', function () {
        $('#UserName').val('');
        $('#Password').val('');
        $('#UserName').focus();
    });

    $('#createPost').on('shown.bs.modal', function () {

        $('#PostName').val('');
        $('#PostBody').val('');
        $('#PostName').focus();
    });

    $('#createAnswer').on('shown.bs.modal', function () {

        $('#AnswerTitle').val('');
        $('#AnswerBody').val('');
        $('#AnswerTitle').focus();

    });

    $('#registermodal').on('shown.bs.modal', function () {
        $('#RegisterUserName').val('');
        $('#RegisterConfirmPassword').val('');
        $('#RegisterPassword').val('');
        $('#RegisterUserName').focus();
    });
});

/* Intercept enter in forms */
function stopRKey(evt) {
    var evt = (evt) ? evt : ((event) ? event : null);
    var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
    if ((evt.keyCode == 13) && (node.type == "text") && (node.id == "UserName")) {
        $('#Password').focus();
        return false;
    }
    else if ((evt.keyCode == 13) && (node.type == "text") && (node.id == "RegisterUserName")) {
        $('#RegisterPassword').focus();
        return false;
    }
    else if ((evt.keyCode == 13) && (node.type == "password") && (node.id == "RegisterPassword")) {
        $('#RegisterConfirmPassword').focus();
        return false;
    }
    else if ((evt.keyCode == 13) && (node.type == "text") && (node.id == "PostName")) {
        $('#PostBody').focus();
        return false;
    }
    else if ((evt.keyCode == 13) && (node.type == "text") && (node.id == "AnswerTitle")) {
        $('#AnswerBody').focus();
        return false;
    }
}

document.onkeypress = stopRKey;


$(function() {
    $(".followed").mouseenter(
          function ()
          { 
              $(this).addClass('remove');
              $(this).html('<span class="glyphicon glyphicon-remove followed"> </span> UnFollow');
          });
    $(".followed").mouseleave(
          function () {
              $(this).removeClass("remove");
           
              $(this).html('<span class="glyphicon glyphicon-ok"> </span> Following')
          });
    $(".notfollowing").mouseenter(
      function () {
          $(this).addClass('add');
          $(this).html('<span class="glyphicon glyphicon-ok"> Follow');
      });
    $(".notfollowing").mouseleave(
        function () {
            $(this).removeClass('add');
            $(this).html('<span class="glyphicon glyphicon-plus"></span> Follow');

        });

});

$(function () {
    /* Ugly hack to fix a cheap font */
    $(".panel-title").each(function (index) {
        var text = $(this).text().replace(/([^a-zA-Z\d\s])/g, '<span style="font-family: \'Helvetica Neue\', Helvetica, Arial, sans-serif;">$1</span>');
        $(this).html(text);
    });
});