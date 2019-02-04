using System.Xml.Serialization;

namespace MVP.Source.Models
{
    [XmlRootAttribute(ElementName = "SearchData", Namespace = GlobalVars.MY_NAMESPACE)]
    public class SearchData
    {
        [XmlAttribute(AttributeName = "Text")]
        public string Text { get; set; }
    }
}
