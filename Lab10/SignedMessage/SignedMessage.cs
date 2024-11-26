namespace Lab10.SignedMessage
{
    public class SignedMessage
    {
        public string? Message { get; set; }
        public required dynamic Hash { get; set; }
    }
}