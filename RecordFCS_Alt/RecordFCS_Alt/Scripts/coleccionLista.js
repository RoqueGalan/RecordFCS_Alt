﻿



$(function () {
    $.ajaxSetup({ cache: false });
    $("a[data-modal].openModal_Coleccion").on("click", function (e) {

        $('#miModalContenido').load(this.href, function () {
            $('#miModal').modal({
                backdrop: 'static',
                keyboard: true
            }, 'show');
            bindForm_Coleccion(this);
        });
        return false;
    });
});

function bindForm_Coleccion(dialog) {
    $('form', dialog).submit(function (e) {
        e.preventDefault();

        var validarOK = false;


        validarOK = $(this).validate().valid();

        if (validarOK) {
            $.ajax({
                url: this.action,
                type: this.method,
                data: $(this).serialize(),
                success: function (result) {
                    if (result.success) {
                        $('#miModal').modal('hide');

                        if ($('#renderListaColeccion').length)
                            $('#renderListaColeccion').load(result.url); //  Campo que actualizara
                        else
                            window.location.reload();

                        $('#alertasDiv').load('/Base/_Alertas');

                    } else {
                        $('#miModalContenido').html(result);
                        bindForm_Coleccion(dialog);
                    }
                }
            });
        }
        return false;
    });
}