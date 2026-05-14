using System;
using UnityEngine;

public class GuidReferenceAttribute : PropertyAttribute {
    public GuidReferenceAttribute() {}
    public GuidReferenceAttribute(Type t) {}
    public GuidReferenceAttribute(string s) {}
    public GuidReferenceAttribute(Type t, string s) {}
    public GuidReferenceAttribute(string s, Type t) {}
}

public class GuidReferenceableAttribute : Attribute {
    public GuidReferenceableAttribute() {}
    public GuidReferenceableAttribute(string s) {}
    public GuidReferenceableAttribute(Type t) {}
}

public class NamedIdAttribute : Attribute {}
public class StatNameAttribute : Attribute {}
public class ReadOnlyAttribute : PropertyAttribute {}
public enum DamageAttribute { Basic, Skill }
