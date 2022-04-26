using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;
using Application = Microsoft.Office.Interop.Word.Application;
using Window = System.Windows.Window;
using Range = Microsoft.Office.Interop.Word.Range;
using Word = Microsoft.Office.Interop.Word;

namespace StudentsProjectChecker
{
    public class collector //Коллекционер (собиратель) элементов документа
    {
        public WordFile fileToCollectDataFrom { set; get; }

        public struct collectedData
        {
            public List<Paragraph> cParagraphs;
            public List<Table> cTalbes;
            public List<InlineShape> cShapes;
        }

        private List<Word.List> getListsInRange(int start, int end)
        {
            return (start < end) ? fileToCollectDataFrom.getDocument.Range(start, end).Tables.Cast<Word.List>().ToList()
                : fileToCollectDataFrom.getDocument.Range(start).Tables.Cast<Word.List>().ToList();
        }

        private List<Table> getTablesInRange(int start, int end)
        {
            return (start < end) ? fileToCollectDataFrom.getDocument.Range(start, end).Tables.Cast<Table>().ToList()
                : fileToCollectDataFrom.getDocument.Range(start).Tables.Cast<Table>().ToList();
        }

        private List<InlineShape> GetShapesInRange(int start, int end)
        {
            return (start < end) ? fileToCollectDataFrom.getDocument.Range(start, end).InlineShapes.Cast<InlineShape>().ToList()
                : fileToCollectDataFrom.getDocument.Range(start).InlineShapes.Cast<InlineShape>().ToList();
        }

        private List<Paragraph> getParagraphInRange(int start, int end)
        {
            return (start < end) ? fileToCollectDataFrom.getDocument.Range(start, end).Paragraphs.Cast<Paragraph>().ToList()
                                                   : fileToCollectDataFrom.getDocument.Range(start).Paragraphs.Cast<Paragraph>().ToList();
        }

        int getStartPosition(string number)
        {
            return fileToCollectDataFrom.getAppplication.Selection.GoTo(What: WdGoToItem.wdGoToHeading,
                            Which: WdGoToDirection.wdGoToAbsolute,
                            Count: number).Start; //set position to the first heading
        }

        int getEndPosition(string number)
        {
            return fileToCollectDataFrom.getAppplication.Selection.GoTo(What: WdGoToItem.wdGoToHeading,
                            Which: WdGoToDirection.wdGoToAbsolute,
                            Count: number).End;  //set position to the next heading
        }

        protected Array headings()
        {
            return (Array)fileToCollectDataFrom.getDocument.GetCrossReferenceItems(WdReferenceType.wdRefTypeHeading);
        }

        protected List<collectedData> collectData()
        {
            List<collectedData> collectObject = new List<collectedData>();

            var listOfEachHeading = (Array)fileToCollectDataFrom.getDocument.GetCrossReferenceItems(WdReferenceType.wdRefTypeHeading);

            var headingsCount = listOfEachHeading.Length;

            for (int i = 1; i <= headingsCount; i++)
            {
                var start = getStartPosition(i.ToString());

                var end = getEndPosition((i + 1).ToString());

                var p = getParagraphInRange(start, end);

                if (GetShapesInRange(start, end).Count == 0)
                {
                    p.RemoveAll(p => p.Range.Text.Contains('/'));
                }

                collectObject.Add(new collectedData
                {
                    cParagraphs = p,  //Абзацы
                    cTalbes = getTablesInRange(start, end),         //Таблицы
                    cShapes = GetShapesInRange(start, end)          //Рисунки
                });
            }
            return collectObject;
        }

    }
}
