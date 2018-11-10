﻿namespace FolderSyns.Core
{
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    using FolderSyns.Interfaces;

    public class HistoryManager : IHistoryManager
    {
        #region Constants

        private const string FILE_NAME = "History.xml";

        #endregion Constants

        #region Fields

        private readonly ISettingsManager _settingsManager;
        private readonly ILogManager _logManager;
        private readonly string _filePath;

        #endregion Fields

        #region Constuctors

        public HistoryManager(ISettingsManager settingsManager, ILogManager logManager)
        {
            _settingsManager = settingsManager;
            _logManager = logManager;
            _filePath = Path.Combine(_settingsManager.FolderForHistory, FILE_NAME);
        }

        #endregion Constuctors

        #region Methods
        public void SerializeObject<T>(T serializableObject)
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
                    xmlDocument.Save(_filePath);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                _logManager.SaveError(ex);
            }
        }

        public T DeSerializeObject<T>()
        {
            T objectOut = default(T);
            try
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(_filePath);
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
                _logManager.SaveError(ex);
            }

            return objectOut;
        }
        #endregion Methods
    }
}