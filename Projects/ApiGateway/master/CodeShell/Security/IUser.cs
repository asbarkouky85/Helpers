﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Security
{
    public interface IUser
    {
        object UserId { get; }
        string Name { get; }
        string LogonName { get; }
       
    }
}
