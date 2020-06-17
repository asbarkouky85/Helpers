using CodeShell.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Globalization
{
    public class Strings
    {
        static ResourceManager wordRes;
        static ResourceManager colRes;
        static ResourceManager messRes;
        static ResourceManager pageRes;

        protected static Dictionary<string, Dictionary<string, string>> WordsDictionary= new Dictionary<string, Dictionary<string, string>>();
        protected static Dictionary<string, Dictionary<string, string>> ColsDictionary= new Dictionary<string, Dictionary<string, string>>();
        protected static Dictionary<string, Dictionary<string, string>> MessDictionary = new Dictionary<string, Dictionary<string, string>>();
        protected static Dictionary<string, Dictionary<string, string>> PageDictionary = new Dictionary<string, Dictionary<string, string>>();

        static void InitializeCurrentCulture()
        {
            string assembly = Shell.LocalizationAssembly;

            string wordsType = assembly + ".Localization.Words";
            string colsType = assembly + ".Localization.Columns";
            string messType = assembly + ".Localization.Messages";
            string pageType = assembly + ".Localization.Pages";

            Assembly ass = Assembly.Load(assembly);
            
            wordRes = new ResourceManager(wordsType, ass);
            colRes = new ResourceManager(colsType, ass);
            messRes = new ResourceManager(messType,ass);
            pageRes = new ResourceManager(pageType, ass);

            WordsDictionary[Language.Culture.TwoLetterISOLanguageName] = ResourceToDictionary(wordRes, Language.Culture);
            ColsDictionary[Language.Culture.TwoLetterISOLanguageName] = ResourceToDictionary(colRes, Language.Culture);
            MessDictionary[Language.Culture.TwoLetterISOLanguageName] = ResourceToDictionary(messRes, Language.Culture);
            PageDictionary[Language.Culture.TwoLetterISOLanguageName] = ResourceToDictionary(pageRes, Language.Culture);
        }

        public static Dictionary<string, string> ResourceToDictionary(ResourceManager man, CultureInfo info)
        {
            var dic = new Dictionary<string, string>();
            try
            {
                ResourceSet st = man.GetResourceSet(info, true, true);
                
                foreach (DictionaryEntry v in st)
                {
                    dic[v.Key.ToString()] = v.Value.ToString();
                }
                    
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return dic;
        }

        public static string Word(string index)
        {
            string cult = Language.Culture.TwoLetterISOLanguageName;

            if (!WordsDictionary.ContainsKey(cult))
                InitializeCurrentCulture();

            if (!WordsDictionary[cult].ContainsKey(index))
                return index;

            return WordsDictionary[cult][index];
        }

        public static string Column(string index)
        {
            string cult = Language.Culture.TwoLetterISOLanguageName;

            if (!ColsDictionary.ContainsKey(cult))
                InitializeCurrentCulture();

            if (!ColsDictionary[cult].ContainsKey(index))
                return Word(index.GetAfter("_"));

            return ColsDictionary[cult][index];
        }

        public static string Page(string index)
        {
            string cult = Language.Culture.TwoLetterISOLanguageName;

            if (!PageDictionary.ContainsKey(cult))
                InitializeCurrentCulture();

            if (!PageDictionary[cult].ContainsKey(index))
                return index;

            return PageDictionary[cult][index];
        }

        public static string Message(string index, params string[] formatElements)
        {
            string cult = Language.Culture.TwoLetterISOLanguageName;

            if (!MessDictionary.ContainsKey(cult))
                InitializeCurrentCulture();

            if (!MessDictionary[cult].ContainsKey(index))
                return index;

            return string.Format(MessDictionary[cult][index], formatElements);
        }

    }
}
