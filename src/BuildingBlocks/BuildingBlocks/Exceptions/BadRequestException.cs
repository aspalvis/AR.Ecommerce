namespace BuildingBlocks.Exceptions
{
    public class BadRequestException(string message) : Exception(message)
    {
        public BadRequestException(string name, object key) : this($"Entity \"{name}\" ({key}) was not found.") { }
    }
}
