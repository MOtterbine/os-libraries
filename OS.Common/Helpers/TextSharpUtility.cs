using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using System.Text;
using System.Collections;
using System.IO;


namespace OS.Helpers
{
    public class TextSharpUtility
    {
        public string this[string fieldName]
        {
            get
            {
                return this._pdfReader.AcroFields.GetField(fieldName);
            }
            set
            {
                if (this._pdfReader == null) return;
                this._pdfReader.AcroFields.SetField(fieldName, value);

            }
        }
        public string FilePath { get; protected set; }
        private PdfReader _pdfReader = null;
        //public IEnumerable<KeyValuePair<string, AcroFields.Item>> Fields
        //{
        //    get
        //    {
        //        if (this._pdfReader == null)
        //        {
        //            // return an empty list if the pdf origin is bad
        //            return new List<KeyValuePair<string, AcroFields.Item>>();
        //        }
        //        return this._pdfReader.AcroFields.Fields;
        //    }
        //}
        public IEnumerable<KeyValuePair<string, string>> Fields
        {
            get
            {
                if (this._pdfReader == null)
                {
                    // return an empty list if the pdf origin is bad
                    return new List<KeyValuePair<string, string>>();
                }
                return this._pdfReader.AcroFields.Fields.ToList().ConvertAll(l=> new KeyValuePair<string, string>(l.Key, l.Value.ToString()) );
            }
        }



        public TextSharpUtility(string pdfFilePath)
        {
            if (string.IsNullOrEmpty(pdfFilePath))
            {
                throw new ArgumentNullException("pdfFilePath");
            }
            if (!System.IO.File.Exists(pdfFilePath))
            {
                throw new InvalidOperationException("The file " + pdfFilePath + " doesn't exist.");
            }
            this.FilePath = pdfFilePath;
            this._pdfReader = new PdfReader(pdfFilePath);
        }




        public MemoryStream fillPDFForm(IEnumerable<KeyValuePair<string, string>> fieldData)
        {
            //string formFile = Server.MapPath(P_InputStream);
            //string newFile = Server.MapPath(P_OutputStream);
          //  PdfReader reader = new PdfReader(formFile);
            MemoryStream ms = new MemoryStream();
            using (PdfStamper stamper = new PdfStamper(this._pdfReader, ms))
          //  using (PdfStamper stamper = new PdfStamper(reader, new FileStream(newFile, FileMode.Create)))
            {
                AcroFields fields = stamper.AcroFields;
                foreach(KeyValuePair<string, string> kvp in fieldData)
                {
                    fields.SetField(kvp.Key, kvp.Value);
                }

                // flatten form fields and close document
                stamper.FormFlattening = true;
                stamper.Close();
            }
            return ms;
        }











    }
}