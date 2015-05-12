/*
var countries = new Bloodhound({
    datumTokenizer: Bloodhound.tokenizers.whitespace,
    queryTokenizer: Bloodhound.tokenizers.whitespace,
    // url points to a json file that contains an array of country names, see
    // https://github.com/twitter/typeahead.js/blob/gh-pages/data/countries.json
    prefetch: '../data/countries.json'
});

// passing in `null` for the `options` arguments will result in the default
// options being used
$('#prefetch .typeahead').typeahead(null, {
    name: 'countries',
    source: countries
});

*/

$(function () {
    $(document).ready(function () {
        $('#queryString').keypress(function (e) {
            if (e.which == 13 && $('#queryString').val() != '') {
                document.location = '/Home/Search/' + $("#queryString").val();
            }
        });
    });
    $('#headerSearch .form-control').autocomplete({
        source: "/User/UserQuery/",
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

