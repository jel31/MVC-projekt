document.addEventListener('DOMContentLoaded', function () {
    var toasts = document.querySelectorAll('[data-toast]');
    if (!toasts.length) {
        return;
    }

    var reduceMotion = window.matchMedia && window.matchMedia('(prefers-reduced-motion: reduce)').matches;

    toasts.forEach(function (toast) {
        var closeBtn = toast.querySelector('[data-toast-close]');
        var hideTimer = null;

        function dismissToast() {
            toast.classList.add('is-exiting');

            var removeDelay = reduceMotion ? 0 : 220;
            window.setTimeout(function () {
                if (toast.parentNode) {
                    toast.parentNode.removeChild(toast);
                }
            }, removeDelay);
        }

        function scheduleDismiss() {
            hideTimer = window.setTimeout(dismissToast, 4200);
        }

        function cancelDismiss() {
            if (hideTimer) {
                window.clearTimeout(hideTimer);
                hideTimer = null;
            }
        }

        if (closeBtn) {
            closeBtn.addEventListener('click', function () {
                cancelDismiss();
                dismissToast();
            });
        }

        toast.addEventListener('mouseenter', cancelDismiss);
        toast.addEventListener('mouseleave', scheduleDismiss);
        toast.addEventListener('focusin', cancelDismiss);
        toast.addEventListener('focusout', scheduleDismiss);

        scheduleDismiss();
    });
});
