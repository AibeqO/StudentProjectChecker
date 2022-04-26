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
    public class checker : collector
    {
        public struct Log
        {
            public string errorType;                //Тип ошибки
            public Dictionary<UInt16, string> pageNumber_errorMsg;

            public List<string> errorMessages;      //Сообщения об ошибках.
                                                    //[0] - Ошибка со шрифтами
                                                    //[1] - Ошибка в обычном тексте
                                                    //[2] - Ошибка в расположении рисунка
                                                    //[3] - Ошибка в заголовках
            public Log(string eType)                
            {
                errorMessages = new List<string>();
                errorType = eType;
                pageNumber_errorMsg = new Dictionary<ushort, string>();
            }
        }
        public List<Log> rLog;
        Dictionary <ErrorCode, dynamic> errorCodeDictionary = new Dictionary<ErrorCode, dynamic>(50);
        public void Init(WordFile file)
        {
            rLog = new List<Log>() {
                new Log("Параметры заголовокa первого уровня"),
                new Log("Параметры заголовокa второго уровня"),
                new Log("Параметры заголовокa третьего уровня"),
                new Log("Параметры заголовокa четвертого уровня"),
                new Log("Цвет шрифта"),
                new Log("Название шрифта"),
                new Log("Стиль шрифта"),
                new Log("Размер шрифта"),
                new Log("Выравнивание абзаца"),
                new Log("Отступ первой строки абзаца"),
                new Log("Отступ до и после изображения"),
                new Log("Проверка текста над таблицей"),
                new Log("Отступ до и после абзаца"),
                new Log("Проверка свойств перечисления")
            };

            errorCodeDictionary.Add(ErrorCode.eFirstLvlHeading, new FirstLvlHeading(rLog));
            errorCodeDictionary.Add(ErrorCode.eSecondLvlHeading, new SecondLvlHeading(rLog));
            errorCodeDictionary.Add(ErrorCode.eThirdLvlHeading, new ThirdLvlHeading(rLog));
            errorCodeDictionary.Add(ErrorCode.eFourthLvlHeading, new FourthLvlHeading(rLog));

            errorCodeDictionary.Add(ErrorCode.eColor, new EColor(rLog));
            errorCodeDictionary.Add(ErrorCode.eFontName, new EFontName(rLog));
            errorCodeDictionary.Add(ErrorCode.eFontStyle, new EFontStyle(rLog));
            errorCodeDictionary.Add(ErrorCode.eFontSize, new EFontSize(rLog));

            errorCodeDictionary.Add(ErrorCode.eParagraphAlignment, new paragraphAlignment(rLog));
            errorCodeDictionary.Add(ErrorCode.eParagraphIndent, new paragraphIndent(rLog));

            errorCodeDictionary.Add(ErrorCode.eImageSpacing, new imageInterval(rLog));
            errorCodeDictionary.Add(ErrorCode.eTableDescription, new TableDescription(rLog));

            errorCodeDictionary.Add(ErrorCode.eSpacingBeforeAndAfter, new ESpacingBeforeAndAfter(rLog));
            errorCodeDictionary.Add(ErrorCode.eListParagraphFormat, new EListParagraphFormat(rLog));
        }

        public void Run(List<bool> PropertiesToCheck)
        {
            var data4check = collectData();
            var errorCount = Enum.GetNames(typeof(ErrorCode)).Length;

            foreach (var data in data4check)
            {
                for (int i = 0; i < errorCount; i++)
                {
                    if (PropertiesToCheck[i])
                    {
                        errorCodeDictionary[(ErrorCode)i].check(data);
                    }
                }
            }
        }
    }
}
