using System.ComponentModel;
using CryptographicAlgorithms.Extensions;

namespace CryptographicAlgorithms.Enums {
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum Alphabet {
        [Description("Русский")]
        Russian,
        [Description("Латинский")]
        Latin
    }
}