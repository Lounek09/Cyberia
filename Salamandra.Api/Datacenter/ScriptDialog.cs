namespace Salamandra.Api.Datacenter
{
    [Table("ScriptDialogs")]
    public sealed class ScriptDialog
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Message { get; set; }

        public ScriptDialog()
        {
            Message = string.Empty;
        }
    }
}
