using Microsoft.AspNetCore.Mvc;

namespace Vjezba.Model.Controllers;

internal static class ControllerToastExtensions
{
    public static void SetSuccessToast(this Controller controller, string message)
    {
        controller.TempData["ToastMessage"] = message;
        controller.TempData["ToastType"] = "success";
    }

    public static void SetErrorToast(this Controller controller, string message)
    {
        controller.TempData["ToastMessage"] = message;
        controller.TempData["ToastType"] = "error";
    }
}
