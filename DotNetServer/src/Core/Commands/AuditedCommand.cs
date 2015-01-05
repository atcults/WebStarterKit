namespace Core.Commands
{
    public abstract class AuditedCommand : Command
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageData { get; set; }
    }
}