document.addEventListener('DOMContentLoaded', function () {
    if (!window.jQuery || !window.jQuery.validator) {
        return;
    }

    var $ = window.jQuery;
    var hiddenIgnoreRule = ':hidden:not(.ac-value)';

    $.validator.setDefaults({
        ignore: hiddenIgnoreRule
    });

    function ensureFormValidator(form) {
        var $form = $(form);
        var validator = $form.data('validator');

        if (!validator && $.validator.unobtrusive) {
            $.validator.unobtrusive.parse(form);
            validator = $form.data('validator');
        }

        if (validator) {
            validator.settings.ignore = hiddenIgnoreRule;
        }
    }

    $('form').each(function () {
        ensureFormValidator(this);
    });

    function validateElement(el) {
        if (!el || !el.form) {
            return;
        }

        var $form = $(el.form);
        if (!$form.length) {
            return;
        }

        ensureFormValidator(el.form);

        if (!$form.data('validator')) {
            return;
        }

        $(el).valid();
    }

    document.addEventListener('focusout', function (event) {
        var target = event.target;
        if (!(target instanceof HTMLInputElement || target instanceof HTMLSelectElement || target instanceof HTMLTextAreaElement)) {
            return;
        }

        if (target.type === 'hidden') {
            return;
        }

        validateElement(target);
    }, true);

    document.querySelectorAll('.ac-wrapper .ac-input').forEach(function (input) {
        input.addEventListener('blur', function () {
            var wrapper = input.closest('.ac-wrapper');
            if (!wrapper) {
                return;
            }

            var hidden = wrapper.querySelector('.ac-value');
            if (hidden) {
                validateElement(hidden);
            }
        });
    });

    document.querySelectorAll('.datetime-picker').forEach(function (input) {
        input.addEventListener('change', function () {
            validateElement(input);
        });
    });
});
