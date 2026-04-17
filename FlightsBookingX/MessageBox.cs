using System.Threading.Tasks;
using Avalonia.Controls;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using MsBox;
using MsBox.Avalonia;

namespace FlightsBookingX;

public static class MessageBox
{
    public static async Task<ButtonResult> Show(ContentControl owner, string title, string message, ButtonEnum buttonEnum, Icon icon)
    {
        return await MessageBoxManager.GetMessageBoxStandard(title, message, buttonEnum, icon, windowStartupLocation:WindowStartupLocation.CenterOwner)
            .ShowAsPopupAsync(owner);
    }
    
    public static async Task<ButtonResult> Show(ContentControl owner, string message)
    {
        return await MessageBoxManager.GetMessageBoxStandard("",message, ButtonEnum.Ok, Icon.Info, windowStartupLocation:WindowStartupLocation.CenterOwner)
            .ShowAsPopupAsync(owner);
    }
    
    public static async Task<ButtonResult> ShowInfo(ContentControl owner, string message)
    {
        return await MessageBoxManager.GetMessageBoxStandard("Уведомление",message, ButtonEnum.Ok, Icon.Info, windowStartupLocation:WindowStartupLocation.CenterOwner)
            .ShowAsPopupAsync(owner);
    }
    
    public static async Task<ButtonResult> ShowSuccess(ContentControl owner, string message)
    {
        return await MessageBoxManager.GetMessageBoxStandard("Успех",message, ButtonEnum.Ok, Icon.Success, windowStartupLocation:WindowStartupLocation.CenterOwner)
            .ShowAsPopupAsync(owner);
    }
    
    public static async Task<ButtonResult> ShowWarning(ContentControl owner, string message)
    {
        return await MessageBoxManager.GetMessageBoxStandard("Внимание",message, ButtonEnum.Ok, Icon.Warning, windowStartupLocation:WindowStartupLocation.CenterOwner)
            .ShowAsPopupAsync(owner);
    }

    public static async Task<ButtonResult> ShowError(ContentControl owner, string message)
    {
        return await MessageBoxManager.GetMessageBoxStandard("Ошибка",message, ButtonEnum.Ok, Icon.Error, windowStartupLocation:WindowStartupLocation.CenterOwner)
            .ShowAsPopupAsync(owner);
    }
    
    public static async Task<ButtonResult> ShowQuestion(ContentControl owner, string message)
    {
        return await MessageBoxManager.GetMessageBoxStandard("Требуется подтверждение",message, ButtonEnum.YesNo, Icon.Question, windowStartupLocation:WindowStartupLocation.CenterOwner)
            .ShowAsPopupAsync(owner);
    }
}