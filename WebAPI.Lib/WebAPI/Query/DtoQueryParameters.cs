namespace WebAPI.Lib.WebAPI.Query;

public class DtoQueryParameters : QueryParameters
{
    public string? Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;

}

public class DtoWithActiveQueryParameters : DtoQueryParameters
{
    public bool? IsActive { get; set; }

}
