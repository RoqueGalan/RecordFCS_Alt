﻿$(function () {
    $.ajaxSetup({ cache: false });
    $("a[data-modal].openModal_TipoAtributo").on("click", function (e) {

        $('#miModalContenido').load(this.href, function () {
            $('#miModal').modal({
                backdrop: 'static',
                keyboard: true
            }, 'show');
            bindForm_TipoAtributo(this);
        });
        return false;
    });
});

function bindForm_TipoAtributo(dialog) {


    $('form', dialog).submit(function (e) {
        e.preventDefault();

        if ($(this).validate().valid()) {
            $.ajax({
                url: this.action,
                type: this.method,
                data: $(this).serialize(),
                success: function (result) {
                    if (result.success) {
                        $('#miModal').modal('hide');

                        $('#alertasDiv').load('/Base/_Alertas');


                        if ($('#renderListaTipoAtributo').length)
                            $('#renderListaTipoAtributo').load(result.url); //  Campo que actualizara
                        else
                            window.location.reload();


                    } else {
                        $('#miModalContenido').html(result);
                        bindForm_TipoAtributo(dialog);
                    }
                }
            });
        }
        return false;
    });
}