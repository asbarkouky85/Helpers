using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Generator.Customization
{
    public class EntityGenerator : CSharpEntityTypeGenerator, ICSharpEntityTypeGenerator
    {
        List<object> obs = new List<object>();
        static string BaseClass;
        static string AssemblyName;
        bool initialized = false;
        public EntityGenerator(ICSharpHelper cSharpHelper) : base(cSharpHelper)
        {
            if (!initialized)
            {
                AssemblyName = GetType().Assembly.GetName().Name;
                BaseClass = GetBaseClassName();
                initialized = true;
            }
        }

        private string GetBaseClassName()
        {
            string assembly = AssemblyName;
            return $"{assembly}ModelBase, I{assembly}Model";
        }

        public override string WriteCode(IEntityType entityType, string @namespace, bool useDataAnnotations)
        {

            string name = entityType.Name;
            string st = base.WriteCode(entityType, @namespace, useDataAnnotations);

            st = st.Replace($"namespace {AssemblyName}.Entities", $"namespace {AssemblyName}");
            if (BaseClass != null)
            {
                st = st.Replace("class " + name, "class " + name + $" : " + BaseClass);
            }

            string[] lines = st.Split("\n");

            List<string> applied = new List<string>();
			applied.Add("using System.Runtime.Serialization;\n");
            Regex reg = new Regex("public (.*?) (.*?) { get; set; }");
            Regex types = new Regex("long|int|decimal|double|float|byte|bool|DateTime|string|ICollection");

            foreach (string line in lines)
            {
                if (reg.IsMatch(line))
                {
                    var m = reg.Match(line);
                    var vals = new List<string>();
                    for (int i = 0; i < m.Groups.Count; i++)
                        vals.Add(m.Groups[i].Value);

                    if (!types.IsMatch(vals[1]))
                        applied.Add("\t\t[IgnoreDataMember]\n");
                }
                applied.Add(line);
            }

            return string.Join("", applied);


        }

        public string WriteObject(object ob)
        {
            PropertyInfo[] infs = ob.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            string ret = "";
            foreach (var s in infs)
            {
                string st = "";
                object i = s.GetValue(ob, null);
                if (!obs.Contains(i))
                {
                    if (i != null)
                        st = "\t" + s.Name + " : " + i.ToString();
                    else
                        st = "\t" + s.Name + " : null";
                    obs.Add(i);
                }
                Console.WriteLine(st);
                ret += st + "\n";
            }

            return ret;
        }
    }
}
