using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CodeShell.Tracer
{
    public class LocationCollection
    {
        [XmlElement(ElementName = "Root")]
        public FileLocation[] Locations { get; set; }
    }

    public class FileLocation
    {
        [XmlAttribute(AttributeName = "AppName")]
        public string AppName { get; set; }
        [XmlAttribute(AttributeName = "FolderPath")]
        public string FolderPath { get; set; }
        [XmlAttribute(AttributeName = "FilePath")]
        public string FilePath { get; set; }

        public override string ToString()
        {
            return AppName;
        }
    }
}
