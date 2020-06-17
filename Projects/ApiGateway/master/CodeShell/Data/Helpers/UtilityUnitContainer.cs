using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Data.Helpers
{
    public class UtilityUnitContainer
    {
        private Dictionary<string, IUnitOfWork> units;

        public UtilityUnitContainer()
        {
            units = new Dictionary<string, IUnitOfWork>();
        }
        public T GetUnit<T>(string index) where T: IUnitOfWork
        {
            IUnitOfWork u = null;

            if (units.TryGetValue(index, out u))
                return (T)u;

            units[index] = Shell.GetFreshUnit();

            return (T)units[index];
        }
    }
}
