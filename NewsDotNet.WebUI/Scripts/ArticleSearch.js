$(document).ready(function () {
    $('#search-results-block').hide();

    var options = {
        target: '#search-results',
        beforeSubmit: function (arr, $form, options) {
            if (arr[0]['value'].length < 1) {
                return false;
            }
            var $resultsBlock = $('#search-results-block');
            $resultsBlock.children('#search-results').html('<div class="loader">Загрузка...</div>')
            var $body = $('body');
            if (!$body.hasClass('search-active')) {
                $body.addClass('search-active');
                $resultsBlock.slideDown();
            }
        }
    }

    $('#searchForm').ajaxForm(options);

    $('#searchCloseBtn').click(function () {
        $('#search-results-block').slideUp();
        $('body').removeClass('search-active');
        $('#searchForm').resetForm();
    })
})