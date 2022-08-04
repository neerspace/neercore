namespace NeerCore.Data.EntityFramework.Design;

public class SqlDbMap
{
    public string NewGuidFunc { get; set; } = default!;
    public string NewSeqGuidFunc { get; set; } = default!;


    public static readonly SqlDbMap SqlServer = new()
    {
        NewGuidFunc = "NEWID()",
        NewSeqGuidFunc = "NEWSEQUENTIALID()"
    };
}