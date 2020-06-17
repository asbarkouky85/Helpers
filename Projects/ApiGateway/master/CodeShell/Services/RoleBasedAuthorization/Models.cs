using CodeShell.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Business.RoleBasedAuthorization
{
    public class RoleEditDTO : IEditable
    {
        public int Id { get; set; }
        public string State { get; set; }
        public List<PagePermissionDTO> Pages { get; set; }
    }
    public class PagePermissionDTO : IModel, IEditable
    {
        public int Id { get; set; }
        public IProtectedPage Page { get; set; }
        public string State { get; set; }
    }

    public class PagePermissionIntDTO : PagePermissionDTO
    {
        public int Privilege { get; set; }
    }

    public class PagePermissionStringDTO : PagePermissionDTO
    {
        public List<PageActionDTO> Privilege { get; set; }
    }

    public class PageActionDTO : IModel
    {
        public int Id { get; set; }
        public string ActionName { get; set; }
    }
}
