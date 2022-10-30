$(function () {
    var $registerForm = $("#register");

    $registerForm.validate({
        rules: {
            name: {
                required: true,
                lattersonly: true
            },
            email: {
                required: true,
                email: true
            },
            password: {
                required: true,
                all: true
            },
            cpassword: {
                required: true,
                equalTo: '#password'
            },
            role: {
                required: true,
                role: true
            },
        },
        messages: {
            name: {
                required: 'name must be required',
                lattersonly: 'invalid name'
            },

            email: {
                required: 'email must be required',
                email: 'email invalid'
            },
            password: {
                required: 'password must be required',
                all: 'space is not allowed'
            },
            cpassword: {
                required: 'confirm password must be required',
                equalTo: 'password mismatch'
            },
            role: {
                required: 'email must be required',
                role: 'role invalid'
            }
        },
    })

    jQuery.validator.addMethod('lattersonly', function (value, element) {
        return /^[^-\s][a-zA-Z_\s-]+$/.test(value);
    });

    jQuery.validator.addMethod('numericonly', function (value, element) {
        return /^[0-9]+$/.test(value);
    });

    jQuery.validator.addMethod('all', function (value, element) {
        return /^[^-\s][a-zA-Z0-9_\s-]+$/.test(value);
    });
})