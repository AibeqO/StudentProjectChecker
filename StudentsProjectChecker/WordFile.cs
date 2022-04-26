using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;
using OfficeApplication = Microsoft.Office.Interop.Word.Application;
using Window = System.Windows.Window;


namespace StudentsProjectChecker
{
    public class WordFile
    {
        OfficeApplication app;
        Document doc;
        
        public OfficeApplication getAppplication { get { return app; } }
        public Document getDocument { get { return doc; } }
        private bool open;
        public bool isOpen()
        {
            return open;
        }
        public WordFile()
        {
         //   app = new Microsoft.Office.Interop.Word.Application();
        }
        ~WordFile()
        {
            closeFile();
            closeApp();
        }

        public void openFile(string filePath)
        {
            app = new Microsoft.Office.Interop.Word.Application();
            doc = app.Documents.Open(filePath, Visible: true);
            open = true;
        }

        public void closeFile()
        {
            doc.Close(false);
            doc = null;
            open = false;
        }

        public void closeApp()
        {
            app.Quit(false);
            app = null;
        }
    }
}
