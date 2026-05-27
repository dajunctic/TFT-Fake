using System;
using UnityEngine;

public class GuidReferenceableAttribute : Attribute {
    public GuidReferenceableAttribute(Type t) {}
}

public class NamedIdAttribute : Attribute {}
public class ReadOnlyAttribute : PropertyAttribute {}
public enum DamageAttribute { Basic, Skill }
