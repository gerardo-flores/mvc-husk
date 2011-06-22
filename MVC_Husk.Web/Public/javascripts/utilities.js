jQuery.fn.submitWithAjax = function () {
    this.submit(function () {
        $.post($(this).attr('action'), $(this).serialize(), null, "script");
        return false;
    });
};

(function ($) {
    $.fn.loadChart = function (options, url) {
        if ($(this).length) {
            render_to = $(this).attr('id');
            return $.ajax({
                url: url,
                dataType: 'json',
                success: function (data) {
                    for (i = 0; i < data.length; i++) {
                        options.series[i].data = data[i]
                    }
                    options.chart.renderTo = render_to;
                    new Highcharts.Chart(options);
                },
                error: function (status, statusText, responses, headers) {
                    alert('Oops something went wrong..');
                }
            });
        }
    }
})(jQuery);