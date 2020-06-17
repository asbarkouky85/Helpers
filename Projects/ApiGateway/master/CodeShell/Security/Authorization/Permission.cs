using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CodeShell.Security.Authorization
{
    public class Permission
    {

        public string SysResourceAction { get; set; }

        public bool? PrivilegeBool { get; set; }

        public string SysRescource { get; set; }

        protected bool Privilege = false;
        protected List<string> Actions;

        public Permission()
        {

        }

        public Permission(bool perm, IEnumerable<string> actions = null)
        {
            Privilege = perm;
            if (actions != null)
            {
                Actions = new List<string>();
                Actions.AddRange(actions.Distinct());
            }

        }

        public void Append(bool priv)
        {
            Privilege = Privilege | priv;
            Console.WriteLine(this);
        }

        public void Set(IEnumerable<string> lst)
        {
            if (lst == null)
            {
                Actions = null;

            }
            else
            {
                Actions = new List<string>();
                Append(lst);
            }
        }

        public void Append(string action)
        {
            if (Actions == null)
                Actions = new List<string>();

            Actions.Add(action);
            Actions = Actions.Distinct().ToList();
        }

        public void Append(IEnumerable<string> actions)
        {
            if (Actions == null)
                Actions = new List<string>();
            Actions.AddRange(actions);
            Actions = Actions.Distinct().ToList();
        }

        public bool Can(string action)
        {
            if (Actions == null)
                return false;

            return Actions.Contains(action);
        }


        //public bool FromBit(int bitOrder)
        //{
        //    int data = (int)Math.Pow(2D, bitOrder);
        //    bool val = (((Privilege & data) >> bitOrder) == 1);
        //    Console.WriteLine(this.ToString() + " - " + bitOrder + " -> " + val);
        //    return val;
        //}

        //public void SetBit(int bitOrder, bool value)
        //{
        //    int data = (int)Math.Pow(2D, bitOrder);

        //    if (value == true)
        //        Privilege = Privilege | data;
        //    else if (Privilege >= data)
        //        Privilege = Privilege - data;

        //    Console.WriteLine(this);
        //}

        //public override string ToString()
        //{
        //    return Convert.ToString(Privilege, 2);
        //}
    }
}
