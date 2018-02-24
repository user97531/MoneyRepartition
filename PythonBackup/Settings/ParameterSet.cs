using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace PythonBackup.Settings
{
    public class ParameterSet
    {
        private const string RootName = "Settings";
        private const string ParameterTag = "Parameter";
        private const string NameAttribute = "name";
        private const string ValueAttribute = "value";

        private const string MissingAttributeExceptionMessage = "Incorrect format of settings file ({0}). Missing '{1}' attribute in a parameter.";
        private const string UnhandledDatatypeExceptionMessage = "Unhandled data type '{0}'.";

        // =============================================================================
        // MONEYWISE
        // =============================================================================
        public int StartCurrency { get; set; }

        /* =============================================================================
		MEMBER NUMBERS
		=============================================================================*/
        public int NStart { get; set; }
        public int SteadyMembers { get; set; }
        public int MaxMembers { get; set; }
        public int MinMembers { get; set; }
        public float NewProp { get; set; } //dictates the growth
        public float LeavingProp { get; set; } //dictates the decline

        /*=============================================================================
		TIMEWISE
		=============================================================================*/
        //As 1 year has not a fixed length we use days. The Timespan class allow to store durations not specific to any unit. You can manipulate them more easily than values stored in different units.
        //Like adding/substracting (negative values allowed) several of them, multiplying them by a number or adding them to a date to get another date.
        public int LifeExpectancy { get; set; }//average life expectancy in year
        public float MaxLongevity { get; set; } //max longevity of a human
        public float Sigma { get; set; } //deviation around the average life expectancy in years
        public int ExpLength { get; set; } //experiment length in month

        /// <summary>
        /// Dictionary of additional parameters
        /// </summary>
        public Dictionary<string, string> AdditionalParameters { get; set; }

        /// <summary>
        /// Default constructor, sets the default values
        /// </summary>
        internal ParameterSet()
        {
            NStart = 20;
            SteadyMembers = 500;
            MaxMembers = 550;
            MinMembers = 450;
            NewProp = 0.005f;
            LeavingProp = 0.004f;
            LifeExpectancy = 80;
            MaxLongevity = 1.5f * LifeExpectancy;
            Sigma = 5;
            ExpLength = 3 * LifeExpectancy * 12;
            AdditionalParameters = new Dictionary<string, string>();
        }

        /// <summary>
        /// Serializes current instance
        /// </summary>
        /// <param name="filepath">File to serialize to</param>
        internal void Serialize(string filepath)
        {
            XDocument xmlDocument = new XDocument(new XElement(RootName));
            //We use System.Reflection namespace. Reflection is a generic word that contains everything that concerns accessing that would not be accesible normally in this scope at runtime: any declaration in loaded projects, their values,...
            //Reflection is how allow you to see and even set private fields declared in any class (even in project you don't own like Microsoft's ones)
            //Here we retrieve properties of this class, we could have done it directly (not by reflection) in this case, but this way we don't need to update this method when we declare new properties (or delete or rename...)
            PropertyInfo[] properties = GetType().GetProperties();
            foreach (PropertyInfo pi in properties)
            {
                //If the property is a collection, we don't save it this, one could do it but not needed at the moment
                //AdditionalParameters property will be save differently
                if (typeof(IEnumerable).IsAssignableFrom(pi.PropertyType))
                {
                    continue;
                }
                XElement parameter = CreateParameter(pi.Name, pi.GetValue(this));//pi.GetValue(this) retrieves the value of the property on this instance of ParameterSet
                xmlDocument.Root.Add(parameter);
            }
            IEnumerable<string> propertyNames = properties.Select(p => p.Name);
            foreach (KeyValuePair<string, string> kvp in AdditionalParameters)
            {
                if (propertyNames.Contains(kvp.Key))
                {
                    continue;
                }
                XElement parameter = CreateParameter(kvp.Key, kvp.Value);
                xmlDocument.Root.Add(parameter);
            }
            xmlDocument.Save(filepath);
        }

        /// <summary>
        /// Deserializes a parameter set
        /// </summary>
        /// <param name="filepath">File to deserialize from</param>
        /// <returns>Expected parameter set</returns>
        internal static ParameterSet Deserialize(string filepath)
        {
            ParameterSet result = new ParameterSet();
            XDocument xmlDocument = XDocument.Load(filepath);
            PropertyInfo[] properties = typeof(ParameterSet).GetProperties();
            IEnumerable<XElement> parameters = xmlDocument.Root.Elements(ParameterTag);
            foreach (XElement parameter in parameters)
            {
                Tuple<string, string> readValue = ReadParameter(parameter, filepath);
                PropertyInfo relatedProperty = properties.FirstOrDefault(pi => pi.Name == readValue.Item1);
                if (relatedProperty != null)
                {
                    relatedProperty.SetValue(result, ReadValue(relatedProperty.PropertyType, readValue.Item2));//relatedProperty.SetValue sets the value of the property of the 'result' instance of ParameterSet we're creating
                }
                else
                {
                    result.AdditionalParameters[readValue.Item1] = readValue.Item2;
                }
            }
            return result;
        }

        /// <summary>
        /// Creates a parameter
        /// </summary>
        /// <param name="name">Name of the parameter</param>
        /// <param name="value">Value of the parameter</param>
        /// <returns>Expected parameter</returns>
        private static XElement CreateParameter(string name, object value)
        {
            XElement parameter = new XElement(ParameterTag);
            XAttribute nameAttribute = new XAttribute(NameAttribute, name);
            XAttribute valueAttribute = new XAttribute(ValueAttribute, value);
            parameter.Add(nameAttribute, valueAttribute);
            return parameter;
        }

        /// <summary>
        /// Reads the attributes of a parameter
        /// </summary>
        /// <param name="parameter">Parameter to read from</param>
        /// <param name="filepath">Path of file the parameter is written into</param>
        /// <returns>Name and value of the parameter</returns>
        private static Tuple<string, string> ReadParameter(XElement parameter, string filepath)
        {
            string parameterName = parameter.Attribute(NameAttribute)?.Value;
            if (parameterName == null)
            {
                throw new Exception(string.Format(MissingAttributeExceptionMessage, filepath, NameAttribute));
            }
            string parameterValue = parameter.Attribute(ValueAttribute)?.Value;
            if (parameterName == null)
            {
                throw new Exception(string.Format(MissingAttributeExceptionMessage, filepath, ValueAttribute));
            }
            return new Tuple<string, string>(parameterName, parameterValue);
        }

        /// <summary>
        /// Transforms a string representation to a C# object
        /// </summary>
        /// <param name="expectedType">Expected type of the read object</param>
        /// <param name="stringRepresentation">String representation of te object</param>
        /// <returns>Read object</returns>
        private static object ReadValue(Type expectedType, string stringRepresentation)
        {
            switch (expectedType.Name)
            {
                case "string":
                    return stringRepresentation;
                case "Boolean":
                    return bool.Parse(stringRepresentation);
                case "Byte":
                    return byte.Parse(stringRepresentation);
                case "Int32":
                    return int.Parse(stringRepresentation);
                case "UInt32":
                    return uint.Parse(stringRepresentation);
                case "Int64":
                    return long.Parse(stringRepresentation);
                case "UInt64":
                    return ulong.Parse(stringRepresentation);
                case "Single":
                    return float.Parse(stringRepresentation, CultureInfo.InvariantCulture);
                case "Double":
                    return double.Parse(stringRepresentation, CultureInfo.InvariantCulture);
            }
            throw new NotImplementedException(string.Format(UnhandledDatatypeExceptionMessage, expectedType.Name));
        }
    }
}