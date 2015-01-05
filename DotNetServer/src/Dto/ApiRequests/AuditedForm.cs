namespace Dto.ApiRequests
{
    public abstract class AuditedForm : FormBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageData { get; set; }

        public override string GetCommandValue()
        {
            return Name;
        }
    }
}