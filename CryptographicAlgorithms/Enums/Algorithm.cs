using System.ComponentModel;
using CryptographicAlgorithms.Extensions;

namespace CryptographicAlgorithms.Enums {
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum Algorithm {
        [Description("Система Вижинера")]
        Vigenere,
        [Description("Шифр Цезаря")]
        Caesar,
        [Description("Шифр Гронсфельда")]
        Gronsfeld,
        [Description("Полибианский квадрат")]
        Polybius,
        [Description("Скитала")]
        Scytale
    }
}