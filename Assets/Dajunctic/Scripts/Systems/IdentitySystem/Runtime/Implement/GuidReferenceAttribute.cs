using System;
using UnityEngine;

namespace Dajunctic
{
    public class GuidReferenceAttribute: PropertyAttribute
    {
        public Type[] Types {get; }
        public string Prefix {get; }

        public GuidReferenceAttribute(params Type[] types) 
        {
            Types = types;
            Prefix = "";
        }
        public GuidReferenceAttribute(string prefix, params Type[] types)
        {
            Types = types;
            Prefix = prefix;
        }

    }
}