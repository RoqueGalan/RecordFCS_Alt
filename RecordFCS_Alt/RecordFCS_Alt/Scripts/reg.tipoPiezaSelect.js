//logica para renderizar los campos requeridos

$(function () {
    $.ajaxSetup({ cache: false });
    $('#TipoPiezaID').change(function () {
        var strSelecto = "";

        $('#TipoPiezaID option:selected').each(function () {
            strSelecto += $(this)[0].value;
        });

        if (strSelecto != "" || strSelecto != 0) {
            var myUrl = '@Url.Action("RegistroFormulario", "Atributo")';

            //$('#renderIcono').html('' +
            //            '<div class="text-center">' +
            //                '<span class="fa fa-8x fa-spinner fa-pulse text-muted"></span>' +
            //                '<p>' +
            //                    '<i class="text-muted">' +
            //                        'Por favor espera, se estan extrayendo los valores de la base de datos...' +
            //                    '</i>' +
            //                '</p>' +
            //            '</div>');

            $.ajax({
                url: myUrl,
                type: "POST",
                data: { id: strSelecto },
                success: function (result) {

                    //$('#renderIcono').html('');

                    $('#renderAtributosRequeridos').html(result);
                }
            });

        } else {

            $('#renderAtributosRequeridos').html('');

            //$('#renderAtributosRequeridos').html('' +
            //    '<div class="text-center">' +
            //        '<span class="fa fa-10x fa-list-alt text-muted"></span>' +
            //    '</div>');

            //$('#renderIcono').html('' +
            //            '<div class="text-center">' +
            //                '<span class="fa fa-4x fa-arrow-up text-primary"></span>' +
            //                '<p>' +
            //                    '<i class="text-muted">' +
            //                        'Selecciona el TIPO DE PIEZA.' +
            //                    '</i>' +
            //                '</p>' +
            //            '</div>');


        }
    });
    return false;
});