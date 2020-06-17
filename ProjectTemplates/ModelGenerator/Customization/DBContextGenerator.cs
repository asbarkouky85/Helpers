using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Generator.Customization
{
    public class DBContextGenerator : CSharpDbContextGenerator, ICSharpDbContextGenerator
    {
        public string BaseContext { get; set; }
        public IEnumerable<FunctionRemover> Removers
        {
            get
            {
                return new List<FunctionRemover>
                {
                    new FunctionRemover { FunctionName="void OnConfiguring", LineCount=11 }
                };
            }
        }
        public DBContextGenerator(
#pragma warning disable CS0618 // Type or member is obsolete
            IEnumerable<IScaffoldingProviderCodeGenerator> legacyProviderCodeGenerators,
#pragma warning restore CS0618 // Type or member is obsolete
            IEnumerable<IProviderConfigurationCodeGenerator> providerCodeGenerators,
            IAnnotationCodeGenerator annotationCodeGenerator,
            ICSharpHelper cSharpHelper) : base(legacyProviderCodeGenerators, providerCodeGenerators, annotationCodeGenerator, cSharpHelper)
        {

        }



        public override string WriteCode(IModel model, string @namespace, string contextName, string connectionString, bool useDataAnnotations, bool suppressConnectionStringWarning)
        {
            string st = base.WriteCode(model, @namespace, contextName, connectionString, useDataAnnotations, suppressConnectionStringWarning);
            string ass = GetType().Assembly.GetName().Name;
            st = st.Replace($"namespace {ass}.Entities", $"namespace {ass}");
            if (BaseContext != null)
            {
                st = st.Replace(": DbContext", ": " + BaseContext);
            }


            if (Removers != null)
            {
                string[] lines = st.Split("\n");

                List<string> applied = new List<string>();
                int lineCount = 0;
                FunctionRemover rem = null;
                foreach (string line in lines)
                {
                    foreach (var r in Removers)
                    {
                        if (line.Contains(r.FunctionName))
                        {
                            rem = r;
                            lineCount++;
                        }
                    }


                    if (rem != null)
                    {
                        lineCount++;
                        if (lineCount == rem.LineCount)
                            rem = null;
                    }

                    if (rem == null)
                        applied.Add(line);

                }
                return string.Join("", applied);
            }

            return st;



        }
    }
}
