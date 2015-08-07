$(function () {
    $.ajaxSetup({ cache: false });
    $("a[data-modal].openModal_TipoPieza").on("click", function (e) {

        $('#miModalContenido').load(this.href, function () {
            $('#miModal').modal({
                backdrop: 'static',
                keyboard: true
            }, 'show');
            bindForm_TipoPieza(this);
        });
        return false;
    });
});

function bindForm_TipoPieza(dialog) {
    $('form', dialog).submit(function () {

        if ($(this).validate().valid()) {
            $.ajax({
                url: this.action,
                type: this.method,
                data: $(this).serialize(),
                success: function (result) {
                    if (result.success) {
                        $('#miModal').modal('hide');

                        if ($('#renderListaTipoPiezas').length)
                            $('#renderListaTipoPiezas').load(result.url); //  Campo que actualizara
                        else
                            window.location.reload();

                        $('#alertasDiv').load('@Url.Action("_Alertas","Base")');

                    } else {
                        $('#miModalContenido').html(result);
                        $('#alertasDiv').load('@Url.Action("_Alertas","Base")');
                        bindForm_TipoPieza(dialog);
                    }
                }
            });
        }
        return false;
    });
}