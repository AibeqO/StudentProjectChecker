using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;
using System.Text.RegularExpressions;


namespace StudentsProjectChecker
{

    enum ErrorCode
    {
        eFirstLvlHeading = 0,
        eSecondLvlHeading = 1,
        eThirdLvlHeading = 2,
        eFourthLvlHeading = 3,
        eColor = 4,
        eFontName = 5,
        eFontStyle = 6,
        eFontSize = 7,

        eParagraphAlignment = 8,
        eParagraphIndent = 9,

        eImageSpacing = 10,
        eTableDescription = 11,

        eSpacingBeforeAndAfter = 12,
        eListParagraphFormat = 13,
    }

    //=============================================
    public abstract class FontError
    {
        protected Regex emptyStr = new Regex(@"^\s*$");
        public virtual void check(collector.collectedData Data) { }
        protected static int GetPageNumberOfRange(Microsoft.Office.Interop.Word.Range range)
        {
            return (int)range.get_Information(WdInformation.wdActiveEndPageNumber);
        }
        protected static bool isFirstLvlHeading(string heading)
        {
            return new Regex(@" ^[А-Я-A-Z\s\d]+$").IsMatch(heading);
        }
        protected static bool isSimpleText(Microsoft.Office.Interop.Word.Range range)
        {
            return (range.Italic == 0 && range.Bold == 0 && range.Underline == 0);
        }
    }

    class EColor : FontError
    {
        List<checker.Log> rLog;
        public EColor(List<checker.Log> rLog)
        {
            this.rLog = rLog;
        }
        public override void check(collector.collectedData Data)
        {
            bool hasError = false;
            var paragraphsUnder = Data.cParagraphs;
            string errorMsg = "Проверка цвета абзацев под заголовком " + paragraphsUnder[0].Range.Text.ToUpper() + '\n';
            errorMsg += "Цвет абзаца не является черным:\n";
            foreach (Paragraph paragraph in paragraphsUnder)
            {
                if (!emptyStr.IsMatch(paragraph.Range.Text))
                {
                    var RGB = paragraph.Range.Font.TextColor.RGB;
                    var Color = paragraph.Range.Font.Color;

                    if (paragraph.Range.Font.Color != WdColor.wdColorBlack && paragraph.Range.Font.Color != WdColor.wdColorAutomatic)
                    {
                        var singleString = string.Join(" ", paragraph.Range.Text.Split(" ").Take(10).ToArray());
                        errorMsg += "Абзац: " + singleString + '\n';
                        hasError = true;
                    }
                }
            }
            if (!hasError)
            {
                errorMsg = errorMsg.Replace("Цвет абзаца не является черным:\n",
                    "Ошибок не обнаружено.\n");
            }
            rLog[(int)ErrorCode.eColor].errorMessages.Add(errorMsg);
        }
    }

    class EFontName : FontError
    {
        List<checker.Log> rLog;
        public EFontName(List<checker.Log> rLog)
        {
            this.rLog = rLog;
        }
        public override void check(collector.collectedData Data)
        {
            bool hasError = false;
            var paragraphsUnder = Data.cParagraphs;
            string errorMsg = "Проверка названии шрифта абзацев под заголовком " + paragraphsUnder[0].Range.Text.ToUpper() + '\n';
            errorMsg += "Шрифт абзаца должен быть Times New Roman:\n";
            foreach (Paragraph paragraph in paragraphsUnder)
            {
                if (!emptyStr.IsMatch(paragraph.Range.Text))
                {
                    if (paragraph.Range.Font.Name != "Times New Roman")
                    {
                        var singleString = string.Join(" ", paragraph.Range.Text.Split(" ").Take(10).ToArray());
                        errorMsg += "Абзац: " + singleString + '\n';
                        hasError = true;
                    }
                }
            }
            if (!hasError)
            {
                errorMsg = errorMsg.Replace("Шрифт абзаца должен быть Times New Roman:\n",
                    "Ошибок не обнаружено.\n");
            }
            rLog[(int)ErrorCode.eFontName].errorMessages.Add(errorMsg);
        }
    }

    class EFontStyle : FontError
    {
        List<checker.Log> rLog;
        public EFontStyle(List<checker.Log> rLog)
        {
            this.rLog = rLog;
        }
        public override void check(collector.collectedData Data)
        {
            bool hasError = false;
            var paragraphsUnder = Data.cParagraphs;
            string errorMsg = "Проверка стиля шрифта абзацев под заголовком " + paragraphsUnder[0].Range.Text.ToUpper() + '\n';
            errorMsg += "В абзацах присутствует полужирный или наклонный или подчеркнутый текст:\n";
            foreach (Paragraph paragraph in paragraphsUnder.Skip(1))
            {
                if (!isSimpleText(paragraph.Range) && !emptyStr.IsMatch(paragraph.Range.Text))
                {
                    hasError = true;
                    var singleString = string.Join(" ", paragraph.Range.Text.Split(" ").Take(10).ToArray());
                    errorMsg += "Абзац: " + singleString + '\n';
                }
            }
            if (!hasError)
            {
                errorMsg = errorMsg.Replace("В абзацах присутствует полужирный или наклонный или подчеркнутый текст:\n",
                    "Ошибок не обнаружено.\n");
            }
            rLog[(int)ErrorCode.eFontStyle].errorMessages.Add(errorMsg);
        }
    }

    class EFontSize : FontError
    {
        List<checker.Log> rLog;
        public EFontSize(List<checker.Log> rLog)
        {
            this.rLog = rLog;
        }
        public override void check(collector.collectedData Data)
        {
            bool hasError = false;
            var paragraphsUnder = Data.cParagraphs;
            string errorMsg = "Проверка размера шрифта абзацев под заголовком " + paragraphsUnder[0].Range.Text.ToUpper() + '\n';
            errorMsg += "Размер шрифта должен быть 12пт:\n";
            foreach (Paragraph paragraph in paragraphsUnder)
            {
                if (!emptyStr.IsMatch(paragraph.Range.Text) && (paragraph.Range.Font.Size != 12.0f))
                {
                    hasError = true;
                    var singleString = string.Join(" ", paragraph.Range.Text.Split(" ").Take(10).ToArray());
                    errorMsg += "Абзац: " + singleString + '\n';
                }
            }
            if (!hasError)
            {
                errorMsg = errorMsg.Replace("Размер шрифта должен быть 12пт:\n",
                    "Ошибок не обнаружено.\n");
            }
            rLog[(int)ErrorCode.eFontSize].errorMessages.Add(errorMsg);
        }
    }
    //============================================

    public abstract class paragraphPropertiesChecker
    {
        protected Regex emptyStr = new Regex(@"^\s*$");
        public virtual void check(collector.collectedData Data) { }
        protected static int GetPageNumberOfRange(Microsoft.Office.Interop.Word.Range range)
        {
            return (int)range.get_Information(WdInformation.wdActiveEndPageNumber);
        }
    }

    class ESpacingBeforeAndAfter : paragraphPropertiesChecker
    {
        List<checker.Log> rLog;
        public ESpacingBeforeAndAfter(List<checker.Log> rLog)
        {
            this.rLog = rLog;
        }

        public override void check(collector.collectedData Data)
        {
            bool hasError = false;
            var paragraphsUnder = Data.cParagraphs;
            string errorMsg = "Проверка отступов перед и после абзацев под заголовком " + paragraphsUnder[0].Range.Text.ToUpper() + '\n';
            errorMsg += "Список абзацев в которых отступы перед и после не равны нулю:\n";
            foreach (var paragraph in paragraphsUnder)
            {
                if (!emptyStr.IsMatch(paragraph.Range.Text) && paragraph.Range.ListParagraphs.Count == 0
                    && (paragraph.SpaceAfter != 0.0f || paragraph.SpaceBefore != 0.0f))
                {
                    hasError = true;
                    var singleString = string.Join(" ", paragraph.Range.Text.Split(" ").Take(10).ToArray());
                    errorMsg += "Абзац:" + singleString + '\n';
                }
            }
            if (!hasError)
            {
                errorMsg = errorMsg.Replace("Список абзацев в которых отступы перед и после не равны нулю:\n",
                    "Ошибок не обнаружено.\n");
            }
            rLog[(int)ErrorCode.eSpacingBeforeAndAfter].errorMessages.Add(errorMsg);
        }
    }


    //Абзац – логически выделенная часть текста, не имеющая номера.
    //Абзац начинается с красной строки – 1,25 см,
    //выравнивание строки производится по ширине листа.
    //Интервалы перед и после абзаца равны нулю, междустрочный интервал равен 1,5.
    class paragraphAlignment : paragraphPropertiesChecker
    {
        List<checker.Log> rLog;
        public paragraphAlignment(List<checker.Log> rLog)
        {
            this.rLog = rLog;
        }

        public override void check(collector.collectedData Data)
        {
            bool hasError = false;
            var paragraphsUnder = Data.cParagraphs;
            string errorMsg = "Проверка выравнивания абзаца под заголовком " + paragraphsUnder[0].Range.Text.ToUpper() + '\n';
            errorMsg += "Список абзацев не выровненных по ширине листа:\n";
            foreach (var paragraph in paragraphsUnder)
            {
                if (!emptyStr.IsMatch(paragraph.Range.Text) &&
                    paragraph.Alignment != WdParagraphAlignment.wdAlignParagraphJustify)
                {
                    hasError = true;
                    var singleString = string.Join(" ", paragraph.Range.Text.Split(" ").Take(10).ToArray());
                    errorMsg += "Абзац:" + singleString + '\n';
                }
            }
            if (!hasError)
            {
                errorMsg = errorMsg.Replace("Список абзацев не выровненных по ширине листа:\n",
                    "Ошибок не обнаружено.\n");
            }
            rLog[(int)ErrorCode.eParagraphAlignment].errorMessages.Add(errorMsg);
        }

    }

    class paragraphIndent : paragraphPropertiesChecker
    {
        List<checker.Log> rLog;
        public paragraphIndent(List<checker.Log> rLog)
        {
            this.rLog = rLog;
        }

        public override void check(collector.collectedData Data)
        {
            bool hasError = false;
            var paragraphsUnder = Data.cParagraphs;
            string errorMsg = "Проверка абзацев без отступа первой строки " + paragraphsUnder[0].Range.Text.ToUpper() + '\n';
            errorMsg += "Список абзацев в которых отступ первой строки не равно 1.25см:\n";
            foreach (var paragraph in paragraphsUnder)
            {
                if (!emptyStr.IsMatch(paragraph.Range.Text) &&
                    paragraph.FirstLineIndent != 35.45f)
                {
                    hasError = true;
                    var singleString = string.Join(" ", paragraph.Range.Text.Split(" ").Take(10).ToArray());
                    errorMsg += "Абзац:" + singleString + '\n';
                }
            }
            if (!hasError)
            {
                errorMsg = errorMsg.Replace("Список абзацев в которых отступ первой строки не равно 1.25см:\n",
                    "Ошибок не обнаружено.\n");
            }
            rLog[(int)ErrorCode.eParagraphIndent].errorMessages.Add(errorMsg);
        }

    }

    class lineSpacing : paragraphPropertiesChecker
    {
        List<checker.Log> rLog;
        public lineSpacing(List<checker.Log> rLog)
        {
            this.rLog = rLog;
        }

        public override void check(collector.collectedData Data)
        {
            bool hasError = false;
            var paragraphsUnder = Data.cParagraphs;
            string errorMsg = "Проверка абзацев на ошибку в межстрочном интервале " + paragraphsUnder[0].Range.Text.ToUpper() + '\n';
            errorMsg += "Список абзацев в которых межстрочный интервал не равен 1.5 строк:\n";
            foreach (var paragraph in paragraphsUnder)
            {
                if (!emptyStr.IsMatch(paragraph.Range.Text) &&
                    paragraph.LineSpacingRule != WdLineSpacing.wdLineSpace1pt5)
                {
                    hasError = true;
                    var singleString = string.Join(" ", paragraph.Range.Text.Split(" ").Take(10).ToArray());
                    errorMsg += "Абзац:" + singleString + '\n';
                }
            }
            if (!hasError)
            {
                errorMsg = errorMsg.Replace("Список абзацев в которых межстрочный интервал не равен 1.5 строк:\n",
                    "Ошибок не обнаружено.\n");
            }
            rLog[(int)ErrorCode.eParagraphIndent].errorMessages.Add(errorMsg);
        }
    }

    class EListParagraphFormat : paragraphPropertiesChecker
    {
        List<checker.Log> rLog;
        public EListParagraphFormat(List<checker.Log> rLog)
        {
            this.rLog = rLog;
        }

        public override void check(collector.collectedData Data)
        {
            bool hasError = false;
            var paragraphsUnder = Data.cParagraphs;
            string errorMsg = "Проверка перечислении под заголовком " + paragraphsUnder[0].Range.Text + '\n';
            errorMsg += "Отступ текста перечисления не равно 1.9см:\n";
            foreach (var paragraph in paragraphsUnder)
            {
                if (paragraph.Range.ListParagraphs.Count != 0)
                {
                    if (!emptyStr.IsMatch(paragraph.Range.Text) &&
                        paragraph.Range.ListParagraphs[1].FirstLineIndent != -14.2f &&
                         paragraph.Range.ListParagraphs[1].LeftIndent != 49.65f)
                    {
                        hasError = true;
                        var singleString = string.Join(" ", paragraph.Range.Text.Split(" ").Take(10).ToArray());
                        errorMsg += "Текст:" + singleString + '\n';
                    }
                }
            }
            if (!hasError)
            {
                errorMsg = errorMsg.Replace("Отступ текста перечисления не равно 1.9см:\n",
                    "Ошибок не обнаружено.\n");
            }
            rLog[(int)ErrorCode.eListParagraphFormat].errorMessages.Add(errorMsg);
        }
    }

    
    //===========================================

    public abstract class imageProperties
    {
        public virtual void check(collector.collectedData Data) { }
        protected static int GetPageNumberOfRange(Microsoft.Office.Interop.Word.Range range)
        {
            return (int)range.get_Information(WdInformation.wdActiveEndPageNumber);
        }
    }

    class imageInterval : imageProperties
    {
        List<checker.Log> rLog;
        public imageInterval(List<checker.Log> rLog)
        {
            this.rLog = rLog;
        }
        public override void check(collector.collectedData Data)
        {
            bool hasError = false;
            var paragraphsUnder = Data.cParagraphs;
            string errorMsg = "";

            foreach (var paragraph in paragraphsUnder)
            {
                if (paragraph.Range.Text == "/\r")
                {
                    errorMsg = "Проверка отступов до и после иллюстрации " + paragraphsUnder[0].Range.Text + '\n';
                    errorMsg += "Отсутпы до и после иллюстрации не равны 12.0 и 6.0:\n";
                    if (paragraph.SpaceAfter != 12.0f && paragraph.SpaceBefore != 6.0f)
                    {
                        hasError = true;
                        errorMsg += "Страница на котором расположена иллюстрация:" +
                            GetPageNumberOfRange(paragraph.Range).ToString() + '\n';
                    }
                }
            }
            if (!hasError)
            {
                errorMsg = errorMsg.Replace("Отсутпы до и после иллюстрации не равны 12.0 и 6.0:\n",
                    "Ошибок не обнаружено.\n");
            }
            rLog[(int)ErrorCode.eImageSpacing].errorMessages.Add(errorMsg);
        }
    }

    class TableDescription : paragraphPropertiesChecker
    {
        List<checker.Log> rLog;
        public TableDescription(List<checker.Log> rLog)
        {
            this.rLog = rLog;
        }

        public override void check(collector.collectedData Data)
        {
            Regex textAboveTable = new Regex(@"^(Таблица|таблица)(\s\d.\d+)(\s[-|–]\s)([0-9a-zA-Zа-яёА-ЯЁ]+)\r$");
            //Слово «Таблица» выравнивается по левому краюстраницы,
            //интервал 1,5 отступы перед и после 6 пт.
            //После номера таблицы через тире указывается её название таблицы
            //(первая буква прописная, остальные строчные, точка в конце не ставится),
            //без абзацного отступа.
            //Название таблицы должно отражать ее содержание, быть точным, кратким.
            var paragraphs = Data.cParagraphs;
            bool hasError1 = false, hasError2 = false, hasError3 = false;
            string errorMsg = "Проверка текста над таблицей. Страница " + GetPageNumberOfRange(paragraphs[0].Range) + "\n";
            foreach (var paragraph in paragraphs)
            {
                //Паттерн текста над таблицей
                if (textAboveTable.IsMatch(paragraph.Range.Text))
                {
                    //Сам текст
                    var singleString = string.Join(" ", paragraph.Range.Text.Split(" ").Take(10).ToArray());

                    if (paragraph.LineSpacingRule != WdLineSpacing.wdLineSpace1pt5)
                    {
                        hasError1 = true;
                        errorMsg += "Междустрочный интервал не равно 1.5: "
                            + singleString + '\n';
                    }
                    if (paragraph.SpaceBefore != 6.0f)
                    {
                        hasError2 = true;
                        errorMsg += "Отступ до не равно 6пт: "
                            + singleString + '\n';
                    }
                    if (paragraph.SpaceAfter != 6.0f)
                    {
                        hasError3 = true;
                        errorMsg += "Отступ после не равно 6пт: "
                            + singleString + '\n';
                    }
                }
            }
            if (!hasError1 && !hasError2 && !hasError3)
            {
                errorMsg += "Ошибок не обнаружено.\n";
            }
            rLog[(int)ErrorCode.eTableDescription].errorMessages.Add(errorMsg);
        }
    }

    //====================[ЗАГОЛОВКИ]=======================

    public abstract class HeadingFormat
    {
        protected Regex isEmptyStr = new Regex(@"^\s*$");
        public virtual void check(collector.collectedData Data) { }
        protected static int GetPageNumberOfRange(Microsoft.Office.Interop.Word.Range range)
        {
            return (int)range.get_Information(WdInformation.wdActiveEndPageNumber);
        }
    }

    class FirstLvlHeading : HeadingFormat
    {
        static string errorMsg;
        List<checker.Log> rLog;
        public FirstLvlHeading(List<checker.Log> rLog)
        {
            this.rLog = rLog;
        }

        void setErrorMsg(bool predicate, string msg, ref bool hasError)
        {
            if (predicate)
            {
                errorMsg += msg;
                hasError = true;
            }
        }

        void setErrorMsg(Paragraph paragraph)
        {
            errorMsg += "Абзац: ";
            foreach (var word in paragraph.Range.Text.Split(" ").Take(10))
            {
                errorMsg += word;
            }
            errorMsg += "\n\tСтраница: " + GetPageNumberOfRange(paragraph.Range);
            rLog[(int)ErrorCode.eFirstLvlHeading].errorMessages.Add(errorMsg + "...");
            errorMsg = "";
        }



        //Шрифт: Times New Roman, 12 пт, полужирный, выравнивание по центру,
        //отступ – нет, интервал после 18 пт, Уровень 1, многоуровневый.
        public override void check(collector.collectedData Data)
        {
            bool hasError = false;
            var paragraphs = Data.cParagraphs;
            Regex firstType = new Regex(@"^(\d\s)([ЁёА-я\s]+)$");
            Regex secondType = new Regex(@"^(ПРИЛОЖЕНИЕ\s)(\d)$");
            Regex thirdType = new Regex(@"^([ЁА-Я\s]+)$");
            //Если заголовок первого уровня
            if (firstType.IsMatch(paragraphs[0].Range.Text) ||
                secondType.IsMatch(paragraphs[0].Range.Text) ||
                thirdType.IsMatch(paragraphs[0].Range.Text) &&
                !isEmptyStr.IsMatch(paragraphs[0].Range.Text))
            {
                //Проверяем предикаты, если истина, то копим сообщения об ошибках
                setErrorMsg(paragraphs[0].Range.Font.Name != "Times New Roman",
                    "Название шрифта должно быть Times New Roman.\n", ref hasError);

                setErrorMsg(paragraphs[0].Range.Font.Size != 12.0f,
                    "Размер шрифта должен быть 12пт.\n", ref hasError);

                setErrorMsg(paragraphs[0].Range.Font.Bold != -1,
                    "Стиль раздела должна быть полужирной.\n", ref hasError);

                setErrorMsg(paragraphs[0].Alignment != WdParagraphAlignment.wdAlignParagraphCenter,
                    "Выравнивание текста не по центру\n", ref hasError);

                setErrorMsg(paragraphs[0].FirstLineIndent != 0,
                    "Отступ в первой строке должен отсутствовать\n", ref hasError);

                setErrorMsg(paragraphs[0].SpaceBefore != 18.0f,
                    "Интервал после должно быть 18пт.\n", ref hasError);

                setErrorMsg(paragraphs[0].OutlineLevel != WdOutlineLevel.wdOutlineLevel1,
                    "Уровень текста должен быть равно 12.\n", ref hasError);
            }
            //Если хотя-бы один из предикатов истина, то выводим сообщение
            if (hasError)
            {
                setErrorMsg(paragraphs[0]);
            }
        }
    }

    class SecondLvlHeading : HeadingFormat
    {
        static string errorMsg;
        List<checker.Log> rLog;
        public SecondLvlHeading(List<checker.Log> rLog)
        {
            this.rLog = rLog;
        }

        void setErrorMsg(bool predicate, string msg, ref bool hasError)
        {
            if (predicate)
            {
                errorMsg += msg;
                hasError = true;
            }
        }

        void setErrorMsg(Paragraph paragraph)
        {
            errorMsg += "Абзац: ";
            foreach (var word in paragraph.Range.Text.Split(" ").Take(10))
            {
                errorMsg += word;
            }
            errorMsg += "\n\tСтраница: " + GetPageNumberOfRange(paragraph.Range);
            rLog[(int)ErrorCode.eSecondLvlHeading].errorMessages.Add(errorMsg + "...");
            errorMsg = "";
        }

        //Шрифт: Times New Roman, 12 пт, полужирный, выравнивание по левому краю,
        //отступ первой строки 0,5 см, междустрочный интервал – одинарный,
        //Уровень 2, многоуровневый, отступы перед и после – 12 пт.
        public override void check(collector.collectedData Data)
        {
            bool hasError = false;
            var paragraphs = Data.cParagraphs;
            Regex regularExpr = new Regex(@"^(\d.\d\s)([а-яА-ЯёЁa-zA-Z0-9\s]+)$");

            //Если заголовок второго уровня
            if (regularExpr.IsMatch(paragraphs[0].Range.Text)
                && !isEmptyStr.IsMatch(paragraphs[0].Range.Text))
            {
                //Проверяем предикаты, если истина, то копим сообщения об ошибках

                //Шрифт: Times New Roman
                setErrorMsg(paragraphs[0].Range.Font.Name != "Times New Roman",
                    "Название шрифта должно быть Times New Roman.\n", ref hasError);

                //12 пт
                setErrorMsg(paragraphs[0].Range.Font.Size != 12.0f,
                    "Размер шрифта должен быть 12пт.\n", ref hasError);

                //полужирный
                setErrorMsg(paragraphs[0].Range.Font.Bold != -1,
                    "Стиль раздела должна быть полужирной.\n", ref hasError);

                //выравнивание по левому краю
                setErrorMsg(paragraphs[0].Alignment != WdParagraphAlignment.wdAlignParagraphLeft,
                    "Выравнивание текста не по левому краю\n", ref hasError);

                //отступ первой строки 0,5 см
                setErrorMsg(paragraphs[0].FirstLineIndent != 0.5f,
                    "Отступ в первой строке должно быть 0.5см\n", ref hasError);

                //междустрочный интервал – одинарный
                setErrorMsg(paragraphs[0].LineSpacingRule != WdLineSpacing.wdLineSpaceSingle,
                    "Интервал после должно быть 18пт.\n", ref hasError);

                //Уровень 2
                setErrorMsg(paragraphs[0].OutlineLevel != WdOutlineLevel.wdOutlineLevel2,
                    "Уровень текста должен быть равно 1.\n", ref hasError);

                //отступы перед и после – 12 пт.
                setErrorMsg(paragraphs[0].SpaceBefore != 12.0f || paragraphs[0].SpaceAfter != 12.0f,
                    "Уровень текста должен быть равно 1.\n", ref hasError);

                //Если хотя-бы один из предикатов истина, то выводим сообщение
                if (hasError)
                {
                    setErrorMsg(paragraphs[0]);
                }
            }
        }
    }

    class ThirdLvlHeading : HeadingFormat
    {
        static string errorMsg;
        List<checker.Log> rLog;
        public ThirdLvlHeading(List<checker.Log> rLog)
        {
            this.rLog = rLog;
        }

        void setErrorMsg(bool predicate, string msg, ref bool hasError)
        {
            if (predicate)
            {
                errorMsg += msg;
                hasError = true;
            }
        }

        void setErrorMsg(Paragraph paragraph)
        {
            errorMsg += "Абзац: ";
            foreach (var word in paragraph.Range.Text.Split(" ").Take(10))
            {
                errorMsg += word;
            }
            errorMsg += "\n\tСтраница: " + GetPageNumberOfRange(paragraph.Range);
            rLog[(int)ErrorCode.eThirdLvlHeading].errorMessages.Add(errorMsg + "...");
            errorMsg = "";
        }

        // Шрифт: Times New Roman, 12 пт, полужирный,
        // выравнивание по левому краю, отступ первая строка 0,75 см,
        // междустрочный интервал – одинарный, Уровень 3, многоуровневый,
        // отступы перед и после – 6 пт.
        public override void check(collector.collectedData Data)
        {
            bool hasError = false;
            var paragraphs = Data.cParagraphs;
            Regex regularExpr = new Regex(@"^(\d.\d.\d\s)([а-яА-ЯёЁa-zA-Z0-9\s]+)$");

            //Если заголовок третьего уровня
            if (regularExpr.IsMatch(paragraphs[0].Range.Text)
                && !isEmptyStr.IsMatch(paragraphs[0].Range.Text))
            {
                //Проверяем предикаты, если истина, то копим сообщения об ошибках

                //Шрифт: Times New Roman
                setErrorMsg(paragraphs[0].Range.Font.Name != "Times New Roman",
                    "Название шрифта должно быть Times New Roman.\n", ref hasError);

                //12 пт
                setErrorMsg(paragraphs[0].Range.Font.Size != 12.0f,
                    "Размер шрифта должен быть 12пт.\n", ref hasError);

                //полужирный
                setErrorMsg(paragraphs[0].Range.Font.Bold != -1,
                    "Стиль раздела должна быть полужирной.\n", ref hasError);

                //выравнивание по левому краю
                setErrorMsg(paragraphs[0].Alignment != WdParagraphAlignment.wdAlignParagraphLeft,
                    "Выравнивание текста не по левому краю\n", ref hasError);

                //отступ первой строки 0,75 см
                setErrorMsg(paragraphs[0].FirstLineIndent != 0.75f,
                    "Отступ в первой строке должно быть 0.75см\n", ref hasError);

                //междустрочный интервал – одинарный
                setErrorMsg(paragraphs[0].LineSpacingRule != WdLineSpacing.wdLineSpaceSingle,
                    "Интервал после должно быть 18пт.\n", ref hasError);

                //Уровень 3
                setErrorMsg(paragraphs[0].OutlineLevel != WdOutlineLevel.wdOutlineLevel3,
                    "Уровень текста должен быть равно 1.\n", ref hasError);

                //отступы перед и после – 6 пт.
                setErrorMsg(paragraphs[0].SpaceBefore != 6.0f || paragraphs[0].SpaceAfter != 6.0f,
                    "Уровень текста должен быть равно 6.\n", ref hasError);

                //Если хотя-бы один из предикатов истина, то выводим сообщение
                if (hasError)
                {
                    setErrorMsg(paragraphs[0]);
                }
            }
        }
    }

    class FourthLvlHeading : HeadingFormat
    {
        static string errorMsg;
        List<checker.Log> rLog;

        public FourthLvlHeading(List<checker.Log> rLog) => this.rLog = rLog;

        void setErrorMsg(bool predicate, string msg, ref bool hasError)
        {
            if (predicate)
            {
                errorMsg += msg;
                hasError = true;
            }
        }

        void setErrorMsg(Paragraph paragraph)
        {
            errorMsg += "Абзац: ";
            foreach (var word in paragraph.Range.Text.Split(" ").Take(10))
            {
                errorMsg += word;
            }
            errorMsg += "\n\tСтраница: " + GetPageNumberOfRange(paragraph.Range);
            rLog[(int)ErrorCode.eFourthLvlHeading].errorMessages.Add(errorMsg + "...");
            errorMsg = "";
        }

        //Шрифт: Times New Roman, 12 пт, полужирный, выравнивание по левому краю,
        //отступ первая строка 1,0 см, междустрочный интервал – одинарный,
        //Уровень 4, многоуровневый, отступы перед и после – 6 пт.
        public override void check(collector.collectedData Data)
        {
            bool hasError = false;
            var paragraphs = Data.cParagraphs;
            Regex regularExpr = new Regex(@"^(\d.\d.\d.\d\s)([а-яА-ЯёЁa-zA-Z0-9\s]+)$");

            //Если заголовок четвертого уровня
            if (regularExpr.IsMatch(paragraphs[0].Range.Text)
                && !isEmptyStr.IsMatch(paragraphs[0].Range.Text))
            {
                //Проверяем предикаты, если истина, то копим сообщения об ошибках

                //Шрифт: Times New Roman
                setErrorMsg(paragraphs[0].Range.Font.Name != "Times New Roman",
                    "Название шрифта должно быть Times New Roman.\n", ref hasError);

                //12 пт
                setErrorMsg(paragraphs[0].Range.Font.Size != 12.0f,
                    "Размер шрифта должен быть 12пт.\n", ref hasError);

                //полужирный
                setErrorMsg(paragraphs[0].Range.Font.Bold != -1,
                    "Стиль раздела должна быть полужирной.\n", ref hasError);

                //выравнивание по левому краю
                setErrorMsg(paragraphs[0].Alignment != WdParagraphAlignment.wdAlignParagraphLeft,
                    "Выравнивание текста не по левому краю\n", ref hasError);

                //отступ первой строки 1 см
                setErrorMsg(paragraphs[0].FirstLineIndent != 1f,
                    "Отступ в первой строке должно быть 1см\n", ref hasError);

                //междустрочный интервал – одинарный
                setErrorMsg(paragraphs[0].LineSpacingRule != WdLineSpacing.wdLineSpaceSingle,
                    "Интервал после должно быть 18пт.\n", ref hasError);

                //Уровень 4
                setErrorMsg(paragraphs[0].OutlineLevel != WdOutlineLevel.wdOutlineLevel4,
                    "Уровень текста должен быть равно 1.\n", ref hasError);

                //отступы перед и после – 6 пт.
                setErrorMsg(paragraphs[0].SpaceBefore != 6.0f || paragraphs[0].SpaceAfter != 6.0f,
                    "Уровень текста должен быть равно 6.\n", ref hasError);

                //Если хотя-бы один из предикатов истина, то выводим сообщение
                if (hasError)
                {
                    setErrorMsg(paragraphs[0]);
                }
            }
        }
    }
}
