using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Security.Permissions;


namespace OS.Serialization
{
    [Serializable]
    [XmlRoot("dictionary")]
    public class SerializableDictionary<TKey, TValue>
        : Dictionary<TKey, TValue>, IXmlSerializable
    {
        protected string _keyName = "key";
        protected string _valueName = "value";
        protected string _itemName = "item";

        public SerializableDictionary() : base()
        {
        }

        protected SerializableDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");
        }


        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");
            base.GetObjectData(info, context);
        }

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));
            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();
            if (wasEmpty)
                return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement(this._itemName);
                reader.ReadStartElement(this._keyName);
                TKey key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();
                reader.ReadStartElement(this._valueName);
                TValue value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();
                this.Add(key, value);
                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));
            foreach (TKey key in this.Keys)
            {
                writer.WriteStartElement(this._itemName);
                writer.WriteStartElement(this._keyName);
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();
                writer.WriteStartElement(this._valueName);
                TValue value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
        }

        #endregion
    }
}
