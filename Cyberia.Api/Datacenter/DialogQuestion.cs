namespace Cyberia.Api.Datacenter
{
    [Table("DialogQuestions")]
    public sealed class DialogQuestion : ITable
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Message { get; set; }

        public DialogQuestion()
        {
            Message = string.Empty;
        }
    }
}
