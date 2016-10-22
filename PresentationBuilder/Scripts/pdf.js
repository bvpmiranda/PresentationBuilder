var Pdf = {

    splitFile: function (id) {
        if ($('#pdf-form').length < 1) {
            $('<form>').attr({
                method: 'POST',
                id: 'pdf-form',
                action: "api/PdfAPI/Split/" + id
            }).appendTo('body');
        }
        else {
            $('#pdf-form').attr('action', "api/PdfAPI/Split/" + id);
            $('#pdf-form').html('');
        }

        $('#pdf-form').submit();
    }
}