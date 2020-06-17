using CodeShell.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Services
{
    public abstract class ServiceBase
    {
        IUnitOfWork _unit;

        protected virtual IUnitOfWork UnitOfWork
        {
            get
            {
                if (_unit == null)
                    _unit = Shell.Unit;
                return _unit;

            }
        }
    }
}
