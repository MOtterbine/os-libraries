using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.IO;
using OS.Interfaces;

namespace OS.MSOffice
{

    /****** Requires: Microsoft Visual Studio Tools for Office ******/

    public class ExcelInterop : IReportInfo, IDisposable
    {


        #region IReportInfo

        public string InfoText { get; private set; }

        #endregion IReportInfo


        public System.Data.DataSet ExcelDataSet { get; private set; }

        private string _filePath = "";

        public ExcelInterop(string filePath)
        {
            if(string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("filePath");
            }
            if(!File.Exists(filePath))
            {
                throw new FileNotFoundException(string.Format("Could not find the file [{0}]", filePath));
            }
            this.ExcelDataSet = new System.Data.DataSet();
            this.Sheets = new List<string>();
            this._filePath = filePath;
            this.ReadFile();
        }

        public List<String> Sheets { get; private set; }

        /// <summary>
        /// Opens the specified file and reads the data into memory which is accessible through properties
        /// of this class. The file and the excel application objects are closed before returning.
        /// </summary>
        private void ReadFile()
        {

            Excel.Application excelApp = new Excel.Application();
            excelApp.DisplayAlerts = false;
            //Excel.Workbook book = excelApp.Workbooks.Open(this._filePath, Missing.Value, Missing.Value, Missing.Value
            //                                  , Missing.Value, Missing.Value, Missing.Value, Missing.Value
            //                                 , Missing.Value, Missing.Value, Missing.Value, Missing.Value
            //                                , Missing.Value, Missing.Value, Missing.Value);
            Excel.Workbook book = excelApp.Workbooks.Open(this._filePath);

            // Remember all of the worksheets (and their data) in this document
            foreach (Excel.Worksheet wksht in book.Worksheets)
            {
                this.SaveSheetData(wksht);
            }
            book.Close();
            excelApp.Quit();

        }


        public System.Data.DataTable SaveSheetData(Excel.Worksheet workSheet)
        {
            this.Sheets.Add(workSheet.Name);

            // Create a table for the sheet data - using the name of the sheet for the table
            this.ExcelDataSet.Tables.Add(new System.Data.DataTable(workSheet.Name));

            // Get a reference to the newly-created table
            System.Data.DataTable sheetTable = this.ExcelDataSet.Tables[workSheet.Name];

            StringBuilder sb = new StringBuilder();
            try
            {

                //foreach (System.Data.DataTable tbl in this._ExcelDataSet.Tables)
                //{
                //  Range rng = currentWorksheet.get_Range("A1", rangeEnd);
                    Excel.Range rng = workSheet.UsedRange;
                    object[,] values = (object[,])rng.Value2;

                    // Get the column names
                    for (int j = 1; j <= values.GetLength(1); j++)
                    {
                        sheetTable.Columns.Add(new System.Data.DataColumn(values[1, j] == null ? "" : values[1, j].ToString()));
                    }

                    object[] newRowArray = new object[values.GetLength(1)];

                    // Now, get the data
                    for (int i = 2; i <= values.GetLength(0); i++)
                    {
                        for (int j = 1; j <= values.GetLength(1); j++)
                        {
                            newRowArray[j - 1] = values[i, j] == null ? "" : values[i, j];
                        }
                        sheetTable.Rows.Add(newRowArray);
                    }
                //}
                    return sheetTable;
            }
            catch (Exception ex)
            {
                this.InfoText = string.Format("GetSheetData() - unable to extract rows from sheet: \"{0}\" - {1}", workSheet.Name, ex.Message);
            }
            return null;

        }






















        /// <summary>
        /// Returns a <see cref="DataRowCollection"/> representing all the rows of the specified sheet
        /// </summary>
        /// <param name="sheetName">The name of the sheet to get data from</param>
        /// <returns><see cref="DataRowCollection"/></returns>
        //public System.Data.DataSet GetSheetData(string rangeEnd)
        //{
        //    Worksheet currentWorksheet = null;


        //    StringBuilder sb = new StringBuilder();
        //    try
        //    {

        //        foreach (System.Data.DataTable tbl in this.ExcelDataSet.Tables)
        //        {
        //            // Get the worksheet by name
        //            foreach(Worksheet wksht in this.book.Worksheets)
        //            {
        //                if(wksht.Name == tbl.TableName)
        //                {
        //                    currentWorksheet = wksht;
        //                    break;
        //                }
        //            }
        //          //  Range rng = currentWorksheet.get_Range("A1", rangeEnd);
        //            Range rng = currentWorksheet.UsedRange;
        //            object[,] values = (object[,])rng.Value2;

        //            // Get the column names
        //            for (int j = 1; j <= values.GetLength(1); j++)
        //            {
        //                tbl.Columns.Add(new System.Data.DataColumn(values[1, j] == null ? "" : values[1, j].ToString()));
        //            }

        //            object[] newRowArray = new object[values.GetLength(1)];

        //            // Now, get the data
        //            for (int i = 2; i <= values.GetLength(0); i++)
        //            {
        //                for (int j = 1; j <= values.GetLength(1); j++)
        //                {
        //                    newRowArray[j - 1] = values[i, j] == null ? "" : values[i, j];
        //                }
        //                tbl.Rows.Add(newRowArray);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        this.InfoText = string.Format("GetSheetData() - unable to extract rows from sheet: \"{0}\" in file: [{1}]", ex.Message);
        //        return null;
        //    }
        //    return this.ExcelDataSet;

        //}

        #region IDisposable Members

        private bool disposed = false;
		public void Dispose()
		{
			Dispose(true);
			// This object will be cleaned up by the Dispose method.
			// Therefore, you should call GC.SupressFinalize to
			// take this object off the finalization queue
			// and prevent finalization code for this object
			// from executing a second time.
			GC.SuppressFinalize(this);
		}
		// Dispose(bool disposing) executes in two distinct scenarios.
		// If disposing equals true, the method has been called directly
		// or indirectly by a user's code. Managed and unmanaged resources
		// can be disposed.
		// If disposing equals false, the method has been called by the
		// runtime from inside the finalizer and you should not reference
		// other objects. Only unmanaged resources can be disposed.
		private void Dispose(bool disposing)
		{
			// Check to see if Dispose has already been called.
			if (!this.disposed)
			{
				// If disposing equals true, dispose all managed
				// and unmanaged resources.
				if (disposing)
				{
                    if (this.ExcelDataSet != null) this.ExcelDataSet.Dispose();
                }
				// Note disposing has been done.
				disposed = true;
			}
		}
        ~ExcelInterop()
		{
			// Do not re-create Dispose clean-up code here.
			// Calling Dispose(false) is optimal in terms of
			// readability and maintainability.
			Dispose(false);
        }

        #endregion IDisposable Members


    }
}
