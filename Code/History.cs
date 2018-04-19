namespace FolderSyns.Code
{
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    public static class History
    {
        private static string FILE_NAME = "history.xml";
        static History()
        {
        }

        public static void SerializeObject<T>(T serializableObject)
        {
            if (serializableObject == null)
                return;

            try
            {
                var xmlDocument = new XmlDocument();

                var serializer = new XmlSerializer(serializableObject.GetType());
                using (var stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;

                    xmlDocument.Load(stream);
                    xmlDocument.Save(FILE_NAME);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorSave.SaveError(ex);
            }
        }

        public static T DeSerializeObject<T>()
        {
            if (string.IsNullOrEmpty(FILE_NAME))
                return default(T);

            T objectOut = default(T);
            try
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(FILE_NAME);
                string xmlString = xmlDocument.OuterXml;

                using (var read = new StringReader(xmlString))
                {
                    Type outType = typeof(T);

                    var serializer = new XmlSerializer(outType);
                    using (var reader = new XmlTextReader(read))
                    {
                        objectOut = (T)serializer.Deserialize(reader);
                        reader.Close();
                    }

                    read.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorSave.SaveError(ex);
            }

            return objectOut;
        }
    }
}
