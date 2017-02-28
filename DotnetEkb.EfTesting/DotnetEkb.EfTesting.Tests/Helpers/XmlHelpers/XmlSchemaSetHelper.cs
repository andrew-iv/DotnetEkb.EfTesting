using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;

namespace DotnetEkb.EfTesting.Tests.Helpers.XmlHelpers
{
    public static class XmlSchemaSetHelper
    {
        public static XmlSchemaValidationResult Validate(this XmlSchemaSet @this, XmlDocument xmlDocument)
        {
            var memoryStream = new MemoryStream();
            xmlDocument.Save(memoryStream);
            return @this.Validate(memoryStream);
        }

        public static XmlSchemaValidationResult Validate(this XmlSchemaSet @this, byte[] xml)
        {
            var memoryStream = new MemoryStream(xml);
            return @this.Validate(memoryStream);
        }

        private static XmlSchemaValidationResult Validate(this XmlSchemaSet @this, Stream xmlDocumentStream)
        {
            var validationResult = new XmlSchemaValidationResult { IsValid = true, ValidationErrors = new List<string>() };

            var settings = new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema,
                ValidationFlags = XmlSchemaValidationFlags.ProcessInlineSchema | XmlSchemaValidationFlags.ReportValidationWarnings
            };

            settings.ValidationEventHandler += (o, e) =>
            {
                validationResult.IsValid = false;
                validationResult.ValidationErrors.Add(e.Message);
            };

            try
            {
                settings.Schemas.Add(@this);
            }
            catch (XmlException ex)
            {
                return new XmlSchemaValidationResult
                {
                    IsValid = false,
                    ValidationErrors = new List<string> { ex.Message }
                };
            }

            try
            {
                xmlDocumentStream.Seek(0, SeekOrigin.Begin);
                var reader = XmlReader.Create(xmlDocumentStream, settings);
                while (reader.Read()) { }
            }
            catch (Exception ex) when (ex is XmlException || ex is XmlSchemaValidationException)
            {
                return new XmlSchemaValidationResult
                {
                    IsValid = false,
                    ValidationErrors = new List<string> { ex.Message }
                };
            }

            return validationResult;
        }

    }

    public class XmlSchemaValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> ValidationErrors { get; set; }
        public string ValidationError => ValidationErrors.FirstOrDefault();
    }
}
