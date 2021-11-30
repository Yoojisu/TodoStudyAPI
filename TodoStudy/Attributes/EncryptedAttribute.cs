using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoStudy.Attributes
{
    public class EncryptedAttribute : Attribute
    {
        public string FieldName { get; set; }

        public EncryptedAttribute(string fieldName)
        {
            FieldName = fieldName;
        }
    }
}
