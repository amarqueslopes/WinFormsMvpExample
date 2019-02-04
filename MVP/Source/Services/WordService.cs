using Word = Microsoft.Office.Interop.Word;
using System;

namespace MVP.Source.Services
{
    class WordService : IWordService
    {

        private static readonly Lazy<IWordService> instance =
            new Lazy<IWordService>(() => new WordService());

        public static IWordService Instance
        {
            get
            {
                return instance.Value;
            }
        }

        private WordService()
        {
        }
        public bool FindNext(string text)
        {
            Word.Selection selecao = Globals.ThisAddIn.Application.Selection;
            Word.Document doc = Globals.ThisAddIn.Application.ActiveDocument;
            Word.Selection selection = Globals.ThisAddIn.Application.ActiveDocument.Application.Selection;
            Word.Find findObject = Globals.ThisAddIn.Application.Selection.Find;
            object findText = text;
            selection.Find.ClearFormatting();
            selection.Find.Forward = true;
            selection.Find.MatchCase = false;
            selection.Find.Execute(ref findText);
            if (!selection.Find.Found)
            {
                doc.Range(0, 0).Select();
                selection.Find.Execute(ref findText);
                if (!selection.Find.Found)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
