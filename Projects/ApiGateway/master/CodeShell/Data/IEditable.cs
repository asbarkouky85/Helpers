using System.ComponentModel.DataAnnotations.Schema;

namespace CodeShell.Data
{
    public interface IEditable
    {
        [NotMapped]
        string State { get; set; }
    }
}
