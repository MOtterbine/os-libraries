using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.OleDb;
using OS.Interfaces;



namespace OS.MSOffice
{
    /// <summary>
    /// Basic class to open and read an excel file using an oledb connection
    /// </summary>
    public class ExcelReader : IReportInfo
    {
        private OleDbConnection _oledbConnection = null;
        private OleDbCommand _oledbCommand = null;
        private OleDbDataAdapter _oledbDataAdapter = null;
        private DataTable _oledbRecordSet = null;
        private String _targetFile = "";
        private string _FileName = "";

        #region IReportInfo

        public string InfoText { get; private set; }

        #endregion IReportInfo

        /// <summary>
        /// List of all sheets within the specified Excel file
        /// </summary>
        public List<String> Sheets { get; private set; }

        /// <summary>
        /// The connection string is being used to access the excel file.
        /// </summary>
        public String ConnectionString { get; private set; }

        /// <summary>
        /// ExcelReader constructor - attempts to open the specified file before constructor is finished executing
        /// </summary>
        /// <param name="filePath">path to an excel file</param>
        public ExcelReader(string filePath)
        {

            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException("MSAccessReader requires a valid file path. 'filePath' value is empty or null");
            if (!File.Exists(filePath)) throw new ArgumentNullException(string.Format("MSAccessReader requires a valid file path. {0} doesn't exist.", filePath));

            try
            {

                string fileExtension = Path.GetExtension(filePath);
                _FileName = Path.GetFileName(filePath);

                string updateFileName = Path.GetFileName(filePath);
                switch (fileExtension.ToLower())
                {
                    case ".xls":
                        ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\";";
                        break;
                    case ".xlsx":
                        //ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;\"";
                        ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0 Macro;HDR=Yes;IMEX=1\";";
                        break;
                    default:
                        throw new InvalidOperationException(string.Format("Import file is an invalid format. File: [{0}]", filePath));
                }

                this._targetFile = filePath;
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(string.Format("ExcelReader.Ctor() - error while attempting to open excel file. - {0}", ex.Message));
            }
            try
            {
                _oledbConnection = new OleDbConnection(ConnectionString);
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(string.Format("ExcelReader.Ctor() - unable to establish OleDb connection to:{1}. Conn Str:{2}. - {0}", ex.Message, filePath, ConnectionString));
            }
            try
            {
                _oledbCommand = new OleDbCommand();
                _oledbCommand.CommandType = System.Data.CommandType.Text;
                _oledbCommand.Connection = _oledbConnection;
                _oledbDataAdapter = new OleDbDataAdapter(_oledbCommand);
                _oledbRecordSet = new DataTable();
                _oledbConnection.Open();
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(string.Format("ExcelReader.Ctor() - unable to create OleDb objects. - {0}", ex.Message));
            }
            try
            {
                this.InitializeSheetData();
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(string.Format("ExcelReader.Ctor() - unable to get a list of excel sheets in the file. - {0}", ex.Message));
            }
        }

        /// <summary>
        /// Loads Excel sheet rows into a <see cref="DataTable"/>
        /// </summary>
        private void InitializeSheetData()
        {
            StringBuilder sb = new StringBuilder();
            this.Sheets = new List<string>();
            DataTable excelSheetDataTable = _oledbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            foreach (DataRow row in excelSheetDataTable.Rows)
            {
                int lastCharIndex = row["Table_Name"].ToString().Length - 1;

                // Is the sheet name an-excel-inspired quoted style (like a number for a sheet name)
                if (row["Table_Name"].ToString()[lastCharIndex] == '\'')
                {
                    if (row["Table_Name"].ToString()[lastCharIndex-1] == '$')
                    {
                        // Save the file/sheet name and get rid of the dollar sign, '$'
                        this.Sheets.Add(row["Table_Name"].ToString().Remove(lastCharIndex-1, 1));
                        // forget the rest and loop again - we're done with this iteration
                        continue;
                    }
                }


                // Legitimate sheets contain a dollar sigh '$' at the end of the table name 
                if (row["Table_Name"].ToString()[lastCharIndex] == '$')
                {
                    // Save the file/sheet name and get rid of the dollar sign, '$'
                    this.Sheets.Add(row["Table_Name"].ToString().Remove(lastCharIndex, 1));
                }

                //int indexOf = row["Table_Name"].ToString().IndexOf("$");
                //// if there exists a dollar sigh '$' at the end of the table name - then it's a legit sheet...
                //if (indexOf == row["Table_Name"].ToString().Length - 1)
                //{
                //    // Save the file/sheet name and get rid of the dollar sign, '$'
                //    this.Sheets.Add(row["Table_Name"].ToString().Remove(indexOf,1));
                //}
            }
        }

        /// <summary>
        /// Returns a <see cref="DataRowCollection"/> representing all the rows of the specified sheet
        /// </summary>
        /// <param name="sheetName">The name of the sheet to get data from</param>
        /// <returns><see cref="DataRowCollection"/></returns>
        public DataTable GetSheetData(string sheetName)
        {
            try
            {
                this._oledbRecordSet.Clear();
                this._oledbRecordSet.Columns.Clear();
                

                // if the user creates a sheet that is a number, for example, excel will apply single quotes to outside - but won't for regular text
                if(sheetName[sheetName.Length-1] == '\'')
                {
                    _oledbCommand.CommandText = "SELECT * FROM [" + sheetName.Insert(sheetName.Length-1, "$") +  "]";

                }
                else // just normal text...
                {
                    // CREATE A COMMAND TO EXTRACT THE IMPORT DATA FROM THE SPECIFIC SHEET USING THE SAME CONNECTION WE HAD TO OPEN THE FILE - INTO AN IN-MEMORY DATATABLE
                  //  _oledbCommand.CommandText = "SELECT * FROM [" + sheetName.Replace("'", "''") + "$]";
                    _oledbCommand.CommandText = "SELECT * FROM [" + sheetName + "$]";

                }


                _oledbDataAdapter.SelectCommand = _oledbCommand;
                
                _oledbDataAdapter.Fill(_oledbRecordSet);
            }
            catch (Exception ex)
            {
                this.InfoText = string.Format("GetSheetData() - unable to extract rows from sheet: \"{0}\" in file: [{1}]", ex.Message, this._FileName);
                return null;
            }
            finally
            {
                _oledbConnection.Close();
            }
            return this._oledbRecordSet;
            
        }


        public void Close()
        {
            this._oledbConnection.Close();
        }


    }
}
