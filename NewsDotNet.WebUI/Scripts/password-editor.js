$('#passwordModal').on('hidden.bs.modal', function (e) {
    $("#passwordForm")[0].reset();
    $('#passwordForm .input-validation-error').removeClass('input-validation-error');
    $('#passwordForm .text-danger').empty();
    $('#passwordModal .alert').remove();
})

$('#passwordModal').on('shown.bs.modal', function (e) {
    $('#passwordForm input#oldPassword').focus();
});

$('#passwordForm').submit(function (event) {
    var oldPassword = $("#passwordForm input#oldPassword")[0].value;
    var newPassword = $("#passwordForm input#newPassword")[0].value;
    var confirmNewPassword = $("#passwordForm input#confirmNewPassword")[0].value;
    var $idField = $('#passwordForm input[type=hidden]');
    var isEditedPasswordOwn = $idField.length == 0;
    var requestData;
    var id;
    if (isEditedPasswordOwn)
        requestData = 'oldPassword=' + oldPassword + '&newPassword=' + newPassword + '&confirmNewPassword=' + confirmNewPassword;
    else {
        id = $idField[0].value;
        requestData = 'userId=' + id + '&oldPassword=' + oldPassword + '&newPassword=' + newPassword + '&confirmNewPassword=' + confirmNewPassword;
    }
        
    setTimeout(function () {
        if ($('#passwordForm .input-validation-error').length == 0) {
            $.ajax({
                type: "POST",
                url: event.target.action,
                data: requestData,
                success: function (data) {
                    //reset all existing error messages
                    $('#passwordModal .alert').remove();

                    if (data['result'] == 'success') {
                        $('#passwordModal').modal('hide');
                        if (isEditedPasswordOwn) {
                            $('#redirectModal').modal('show');
                            setTimeout("location.href='/Admin/Users/Login'", 5000);
                        } else {
                            $editedUserRow = $('tr:has(td:contains(' + id + '))');
                            $editedUserRow.popover({
                                content: "Пароль пользователя успешно изменен",
                                placement: 'auto bottom'
                            });
                            $editedUserRow.popover('show');
                            setTimeout(function () { $editedUserRow.popover('destroy') }, 3000);
                        }
                    } else {
                        var $alert = $('<div class="alert alert-danger" role="alert"></div>');
                        for (var key in data['errors']) {
                            var $errors = $('<span></span>');
                            $errors.append(data['errors'][key].join());
                        }
                        $alert.append($errors);
                        $('#passwordModal .modal-body').prepend($alert);
                    }
                }
            });
        }
    }, 0);
    return false;
});