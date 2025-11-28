using WebAPI.Data.Models;
using WebAPI.Lib.Data.Services;

namespace WebApp.Lib.Data.ViewModels.Base;

public abstract class DtoVM<T> : VMBase
    where T : Dto, new()
{
    public T Entry { get; set; } = new();

    public DtoVM() : base(CRUD.LIST) { }
    public DtoVM(CRUD crud, IDtoInfo dtoInfo, bool isModal, string path,
        T entry)
        : base(crud, dtoInfo, isModal, path)
    {
        Entry = entry;
        ResolveEntryId = () => Entry?.Id ?? 0;

        Title = Title.Replace("<>", entry.AsString());
    }

    public new void UpdateBase(CRUD crud, IDtoInfo? dtoInfo = null, bool isModal = false, string? path = "")
    {
        base.UpdateBase(crud, dtoInfo, isModal, path);

        ResolveEntryId = () => Entry?.Id ?? 0;

        Title = Title.Replace("<>", Entry.AsString());
    }

    //public static VMBase New()
    //{

    //    return new DtoVM<T>();
    //}

}

public abstract class DtosVM<T> : VMBase
    where T : Dto
{
    public IEnumerable<T> Entries { get; set; } = [];

    public DtosVM() : base(CRUD.LIST) { }

    public DtosVM(CRUD crud, IDtoInfo dtoInfo, bool isModal, string path,
        IEnumerable<T> entries)
        : base(crud, dtoInfo, isModal, path)
    {
        Entries = entries;
    }
}
