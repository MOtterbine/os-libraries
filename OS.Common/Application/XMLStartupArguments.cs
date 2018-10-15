using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace OS.Application
{
    /// <summary>
    /// Used to parse values and validate minimum requirements from command-line parameter xml string
    /// </summary>
    public class XMLStartupArguments : IRuntimeArguments
    {

        #region Properties and Fields

        /// <summary>
        /// All found data
        /// </summary>
        public Dictionary<String, String> ArgumentsFound { get; private set; }
        /// <summary>
        /// Expected parameter data that was found
        /// </summary>
        private Dictionary<string, string> _KVParameters = null;
        // first string (key) is parameter name, second is its value
        public string MessageString { get; private set; }

        #endregion Properties and Fields

        #region Methods

        public XMLStartupArguments(string xmlString, IEnumerable<string> requiredArguments)
        {
            // Create a searchable dictionary for the parameters, along with a boolean to indicate whether we go the value
            Dictionary<string, bool> reqParamsCheckList = new Dictionary<string, bool>(); 
            foreach(string str in requiredArguments)
            {
                reqParamsCheckList.Add(str, false);
            }

            // Initialize - ALL incoming parameter value pairs dictionary
            this.ArgumentsFound = new Dictionary<string, string>();
            // Initialize - ALL found/expected parameter value pairs dictionary
            this._KVParameters = new Dictionary<string, string>();

            // Read the xml string and track what we found in reqParamsCheckList
            if (!this.ReadXML(xmlString, reqParamsCheckList))
            {
                throw new Exception(this.MessageString);
            }
        }

        /// <summary>
        /// Returns the value corresponding to the specified key (paramName), or null if not in dictionary
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        private string WriteXML(string paramSet, Dictionary<string, string> keyValuePairs)
        {
            XmlWriter xmlWriter = null;
            StringWriter strWriter = new StringWriter();
            xmlWriter = new XmlTextWriter(strWriter);
            xmlWriter.WriteStartElement(paramSet);
            foreach (KeyValuePair<string, string> kvp in this._KVParameters)
            {
                xmlWriter.WriteElementString(kvp.Key, kvp.Value);
            }
            xmlWriter.Close();
            return strWriter.ToString();
        }
        private bool ReadXML(string xmlString, IDictionary<string, bool> paramList)
        {
            // optimistic...until proven false
            bool allParamsWereFound = true;

            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(xmlString);

                foreach (XmlNode nd in xDoc.ChildNodes[0].ChildNodes)
                {
                    this.ArgumentsFound.Add(nd.LocalName, nd.InnerText);
                    // note the parameter was found
                    if (paramList.ContainsKey(nd.LocalName))
                    {
                        paramList[nd.LocalName] = true;
                        this._KVParameters.Add(nd.LocalName, nd.InnerText);
                        continue;
                    }

                    // Flag that we had could not find an expected parameter, but continue - so we can record ALL that are missing
                  //  allParamsWereFound = false;
                }

                // Get a list of what parmeters are missing
                StringBuilder sb = new StringBuilder("One, or more, required parameters are missing:");
                foreach (KeyValuePair<string, bool> kvp in paramList)
                {
                    // search for expected parameters that were not found
                    if (kvp.Value == false)
                    {
                        sb.AppendFormat(" {0}", kvp.Key);
                        // flag a failure
                        allParamsWereFound = false;
                    }
                }
                if (!allParamsWereFound) throw new System.ArgumentException(this.MessageString = sb.ToString());
            }
            catch (System.Xml.XmlException ex_xml)
            {
                this.MessageString = ex_xml.Message;
                return false;
            }
            catch (ArgumentException ex)
            {
                this.MessageString = ex.Message;
                return false;
            }
            catch (Exception ex)
            {
                this.MessageString = ex.Message;
                return false;
            }

            return allParamsWereFound;
        }


        #endregion Methods

        #region IRuntimeArguments Members

        /// <summary>
        /// Returns a value, or null
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public string GetValue(string paramName)
        {
            try
            {
                if (this.ArgumentsFound.ContainsKey(paramName))
                {
                    return this.ArgumentsFound[paramName];   // We gotta 'hit'!
                }
                //if (this._KVParameters.ContainsKey(paramName))
                //{
                //    return this._KVParameters[paramName];   // We gotta 'hit'!
                //}
            }
            // Assumption: _KVParameters is instantiated inline so an unknown exception would happen here, but isn't
            // this property's concern - So, return null
            catch { }
            return null;
        }
        /// <summary>
        /// Returns an xml-formed string of the parameters
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public string CreateArgs(string paramSet, Dictionary<string, string> kvVals)
        {
            return WriteXML(paramSet, kvVals);

        }

        #endregion IRuntimeArguments

    }

}