using Microsoft.Office.Interop.Word;
using Microsoft.Office.Core;

using MVP.Source.Models;

using System;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace MVP.Source.Repositories
{
    class SearchDataXmlRepository : ISearchDataRepository
    {
        private static readonly Lazy<ISearchDataRepository> instance =
            new Lazy<ISearchDataRepository>(() => new SearchDataXmlRepository());

        public static ISearchDataRepository Instance
        {
            get
            {
                return instance.Value;
            }
        }

        private SearchDataXmlRepository()
        {

        }

        private Document GetActiveDocument()
        {
            if (Globals.ThisAddIn.Application.Documents.Count > 0)
            {
                return Globals.ThisAddIn.Application?.ActiveDocument;
            }

            return null;
        }

        public SearchData GetSearchData()
        {
            Document doc = GetActiveDocument();
            if (doc == null)
            {
                return new SearchData();
            }

            CustomXMLParts parts = doc.CustomXMLParts.SelectByNamespace(GlobalVars.MY_NAMESPACE);
            if (parts.Count > 0)
            {
                foreach (CustomXMLPart p in parts)
                {
                    var stringReader = new System.IO.StringReader(p.XML);
                    var serializer = new XmlSerializer(typeof(SearchData));
                    return serializer.Deserialize(stringReader) as SearchData;
                }
            }
            return new SearchData();
        }

        public void SetSearchData(SearchData searchData)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SearchData));
                var stringwriter = new System.IO.StringWriter();
                serializer.Serialize(stringwriter, searchData);
                string xml = stringwriter.ToString();

                Document activeDoc = GetActiveDocument();
                CustomXMLParts parts = activeDoc.CustomXMLParts.SelectByNamespace(GlobalVars.MY_NAMESPACE);
                bool anyDeleted = false;
                string noSpaceXml = Regex.Replace(xml, @"\s+", "");
                if (parts.Count > 0)
                {
                    foreach (CustomXMLPart p in parts)
                    {
                        string pXml = Regex.Replace(p.XML, @"\s+", "");
                        if (!pXml.Equals(noSpaceXml))
                        {
                            p.Delete();
                            anyDeleted = true;
                        }
                    }
                    if (!anyDeleted) return;
                }

                try
                {
                    CustomXMLPart cp = activeDoc.CustomXMLParts.Add(xml);
                }
                catch (System.Runtime.InteropServices.COMException ex)
                {
                    System.Console.Out.Write(ex.Message);
                }
            }
            catch (COMException ex)
            {
                System.Console.Out.Write(ex.Message);
            }
        }

    }
}
