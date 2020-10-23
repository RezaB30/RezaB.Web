using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RezaB.Web.CustomAttributes
{
    public class EnumTypeAttribute : Attribute, IMetadataAware
    {
        private readonly Type _enumType;
        private readonly Type _resourceType;

        public EnumTypeAttribute(Type enumType, Type resourceType)
        {
            _enumType = enumType;
            _resourceType = resourceType;
        }
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues["EnumType"] = _enumType;
            metadata.AdditionalValues["EnumResourceType"] = _resourceType;
        }

        public Type EnumType
        {
            get
            {
                return _enumType;
            }
        }

        public Type ResourceType
        {
            get
            {
                return _resourceType;
            }
        }
    }
}