namespace Cyberia.Api.Datacenter
{
    [Table("DialogAnswers")]
    public sealed class DialogAnswer : ITable
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Message { get; set; }

        public DialogAnswer()
        {
            Message = string.Empty;
        }
    }
}
