namespace CodeShell.Data.Helpers
{
    public class EntityDeletable
    {
        public int Id { get; set; }
        public string Identifier { get; set; }
        public bool CanDelete { get; set; }
        public bool CanDeactivate { get; set; }
    }
}
