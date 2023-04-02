namespace Cyberia.Api.Datacenter
{
    [Table("ScriptDialogs")]
    public sealed class ScriptDialog : ITable
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
