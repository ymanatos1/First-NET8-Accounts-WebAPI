using Microsoft.AspNetCore.Mvc;
using WebAPI.Data.Models;
using WebAPI.Lib.Data.Services;

namespace WebApp.Lib.Data.ViewModels.Base;

public abstract class VMBaseUIVisuals
{
    public enum ToastType { NONE = 0, SUCCESS, INFO, DANGER, WARNING }
    public enum ToastIcon { NONE = 0, CHECK_CIRCLE, PENCIL, TRASH, EXCLAMATION_TRIANGLE, INFO_CIRCLE }

    public string Theme { get; set; } = "popup-success";  // default
    public ToastType Type { get; set; }
    public ToastIcon Icon { get; set; }
}
public abstract class VMBaseUI : VMBaseUIVisuals
{
    public string Title { get; set; } = string.Empty;
    public string ButtonUIStyle { get; protected set; } = string.Empty;
    public string ButtonUICaption { get; protected set; } = string.Empty;
}

public abstract class VMBasePage : VMBaseUI
{
    public string PAGE_NAME { get; protected set; } = string.Empty;
    public string PARTIAL_NAME { get; protected set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
}
public abstract class VMBaseDtoPage : VMBasePage
{
    public string DtoName { get; protected set; } = string.Empty;
    public string DtoNamePlural { get; protected set; } = string.Empty;

    // Delegate that derived classes can wire
    public Func<int>? ResolveEntryId { get; protected set; }
    // Safe unified access
    public int EntryId => ResolveEntryId?.Invoke() ?? 0;
}

public abstract class VMBase : VMBaseDtoPage
{
    public enum CRUD { LIST = 0, CREATE, READ, UPDATE, DELETE }

    public CRUD CRUD_TYPE { get; private set; } = CRUD.LIST;
    //public string PAGE_NAME { get; private set; } = string.Empty;
    //public string PARTIAL_NAME { get; private set; } = string.Empty;

    public bool IsModal { get; set; } = false;

    //public string? Message { get; private set; } = null;
    public string? Error { get; set; } = null;
    //public string Theme { get; set; } = "popup-success";  // default
    //public ToastTypes Type { get; set; }
    //public ToastIcons Icon { get; set; }
    //public string Title { get; set; } = string.Empty;

    //public string Controller { get; set; } = string.Empty;
    //public string Path { get; set; } = string.Empty;
    //public string DtoName { get; private set; } = string.Empty;
    //public string DtoNamePlural { get; private set; } = string.Empty;

    //// Delegate that derived classes can wire
    //public Func<int>? ResolveEntryId { get; protected set; }
    //// Safe unified access
    //public int EntryId => ResolveEntryId?.Invoke() ?? 0;

    public VMBase(CRUD crud, IDtoInfo? dtoInfo = null, bool isModal = false, string? path = "")
    {
        UpdateBase(crud, dtoInfo, isModal, path);
    }

    public void UpdateBase(CRUD crud, IDtoInfo? dtoInfo = null, bool isModal = false, string? path = "")
    {
        if (dtoInfo != null)
        {
            DtoName = dtoInfo.DtoName;
            DtoNamePlural = dtoInfo.DtoNamePlural;

            //Controller = string.Concat(DtoNamePlural.Where(c => !char.IsWhiteSpace(c)));
        }

        IsModal = isModal;

        CRUD_TYPE = crud;
        switch (crud)
        {
            case CRUD.LIST:
                PAGE_NAME = "Index"; Theme = "popup-info"; Type = ToastType.INFO; Icon = ToastIcon.INFO_CIRCLE;
                Title = $"{PAGE_NAME} of {DtoNamePlural}"; 
                break;
            case CRUD.CREATE: 
                PAGE_NAME = "Create"; Theme = "popup-success"; Type = ToastType.SUCCESS; Icon = ToastIcon.CHECK_CIRCLE;
                Title = $"{PAGE_NAME} new {DtoName}";
                ButtonUIStyle = "btn-success"; ButtonUICaption = "Create";
                break;
            case CRUD.READ: 
                PAGE_NAME = "Details"; Theme = "popup-info"; Type = ToastType.WARNING; Icon = ToastIcon.INFO_CIRCLE;
                Title = $"{PAGE_NAME} of {DtoName} : <>";
                //ButtonUIStyle = "btn-success"; ButtonUICaption = "Create";
                break;
            case CRUD.UPDATE: 
                PAGE_NAME = "Edit"; Theme = "popup-info"; Type = ToastType.INFO; Icon = ToastIcon.PENCIL;
                Title = $"{PAGE_NAME} {DtoName} : <>";
                ButtonUIStyle = "btn-primary"; ButtonUICaption = "Save";
                break;
            case CRUD.DELETE: 
                PAGE_NAME = "Delete"; Theme = "popup-danger"; Type = ToastType.DANGER; Icon = ToastIcon.TRASH;
                Title = $"{PAGE_NAME} {DtoName} : <>";
                ButtonUIStyle = "btn-danger"; ButtonUICaption = "Delete";
                break;
            default: PAGE_NAME = ""; Theme = ""; Type = ToastType.NONE; Icon = ToastIcon.NONE; Title = PAGE_NAME; break;
        }
        //PARTIAL_TYPE = partial_type;
        PARTIAL_NAME = $"_{PAGE_NAME}";
        //Title = PAGE_NAME;

        Path = path ?? string.Empty;
    }


    private string toastTypeJson()
    {
        switch (Type)
        {
            case ToastType.SUCCESS: return "success";
            case ToastType.INFO: return "info";
            case ToastType.DANGER: return "danger";
            case ToastType.WARNING: return "warning";
            default: return "";
        }
    }
    private string toastIconJson()
    {
        switch (Icon)
        {
            case ToastIcon.CHECK_CIRCLE: return "check-circle";
            case ToastIcon.PENCIL: return "pencil";
            case ToastIcon.TRASH: return "trash";
            case ToastIcon.EXCLAMATION_TRIANGLE: return "exclamation-triangle";
            case ToastIcon.INFO_CIRCLE: return "info-circle";
            default: return "";
        }
    }
    private string completionMessage(Dto entry)
    {
        string message = "";

        switch (CRUD_TYPE)
        {
            case CRUD.LIST: message = $"{DtoNamePlural} listed successfully."; break;
            case CRUD.CREATE: message = $"{DtoName} '{entry.AsString()}' created successfully with Id #{entry.Id}!"; break;
            case CRUD.READ: message = $"{DtoName} '{entry?.AsString() ?? "NULL"}' read successfully!"; break;
            case CRUD.UPDATE: message = $"{DtoName} '{entry?.AsString() ?? "NULL"}' updated successfully!"; break;
            case CRUD.DELETE: message = $"{DtoName} '{entry?.AsString() ?? "NULL"}' removed successfully!"; break;
            default: return "...";
        }
        return message;
    }
    public JsonResult SuccessJson(Dto entry)
    {
        //new JsonResult(new
        //{
        //    success = true,
        //    toastType = "info",     // CREATE: success, UPDATE: info, DELETE: danger, WARNING: warning
        //    icon = "pencil",        // CREATE; check-circle, UPDATE: pencil, DELETE: trash, WARNING: exclamation-triangle, INFO: info-circle
        //    message = $"{_service.DtoName} '{Account?.Name ?? "NULL"}' updated successfully!"
        //});

        return new JsonResult(new
        {
            success = true,
            toastType = toastTypeJson(),     // CREATE: success, UPDATE: info, DELETE: danger, WARNING: warning
            icon = toastIconJson(),        // CREATE; check-circle, UPDATE: pencil, DELETE: trash, WARNING: exclamation-triangle, INFO: info-circle
            message = completionMessage(entry)  // = Message ?? completionMessage(entry)
        });
    }
}
