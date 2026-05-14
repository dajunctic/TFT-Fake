using System;
using System.Collections.Generic;
using UnityEngine;

namespace FMODUnity {
    public class EventReference {}
}

namespace UnityEngine.Localization {
    public class LocalizedString {
        public object TableEntryReference;
        public bool TryGetValue(string k, out object v) { v = null; return false; }
        public object this[string k] { get => null; set {} }
        public List<string> Keys;
        public bool ContainsKey(string k) => false;
    }
}
namespace UnityEditor.Localization {
    public static class LocalizationEditorSettings {
        public static UnityEngine.Localization.Tables.StringTableCollection GetStringTableCollection(string s) => null;
        public static List<UnityEngine.Localization.Tables.Locale> GetLocales() => new List<UnityEngine.Localization.Tables.Locale>();
    }
}
namespace UnityEngine.Localization.Tables {
    public class StringTableCollection {
        public StringTable GetTable(object locale) => null;
    }
    public class StringTableEntry {
        public object Identifier;
        public bool IsSmart;
        public object Key;
        public string Value;
    }
    public class StringTable : ScriptableObject {
        public StringTableEntry GetEntryFromReference(object reference) => null;
        public string TableCollectionName { get; }
    }
    public class Locale {
        public string Identifier;
    }
}
namespace UnityEngine.Localization.SmartFormat {
    public class SmartFormatter {}
}
namespace UnityEngine.Localization.SmartFormat.PersistentVariables {
    public class PersistentVariables {}
    public class StringVariable {
        public string Value;
    }
}
namespace Febucci.UI.Core {
    public class TypewriterCore {}
}
namespace Febucci.UI.Effects {
    public class Effect {}
}
namespace Febucci {
    public class FebucciStub {}
}
