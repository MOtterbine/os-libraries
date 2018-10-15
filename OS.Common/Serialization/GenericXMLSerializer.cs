using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OS.Serialization
{
    public class GenericXMLSerializer
    {
        private static object syncLoadObject = new object();
        private static object syncSaveObject = new object();
        public static void LoadObjectFromLocalXML<T>(string fileName, out T obj)
        {
            lock (syncLoadObject)
            {
                string thisExePath = new Uri(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath;
                string DataFilePath = (System.IO.Path.GetDirectoryName(thisExePath) + "\\" + fileName);

                // Load file
                System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));
                System.IO.TextReader reader = new System.IO.StreamReader(DataFilePath);
                obj = (T)ser.Deserialize(reader);
                reader.Close();
            }
        }

        public static void SaveObjectToLocalXML<T>(string fileName, T obj)
        {
            lock (syncSaveObject)
            {
                string thisExePath = new Uri(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath;
                string DataFilePath = (System.IO.Path.GetDirectoryName(thisExePath) + "\\" + fileName);

                // Create file*`
                System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));
                System.IO.TextWriter writer = new System.IO.StreamWriter(DataFilePath);
                ser.Serialize(writer, obj);
                writer.Close();
            }
        }
    }



}
