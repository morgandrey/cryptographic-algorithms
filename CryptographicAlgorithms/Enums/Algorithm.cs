using System.ComponentModel;
using CryptographicAlgorithms.Extensions;

namespace CryptographicAlgorithms.Enums {
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum Algorithm {
        [Description("Система Виженера")]
        Vigenere,
        [Description("Шифр Цезаря")]
        Caesar,
        [Description("Шифр Гронсфельда")]
        Gronsfeld,
        [Description("Полибианский квадрат")]
        Polybius,
        [Description("Скитала")]
        Scytale,
        [Description("Шифрующие таблицы")]
        TableTransposition,
        [Description("Двойная перестановка")]
        DoubleTransposition,
        [Description("Магический квадрат")]
        MagicSquare,
        [Description("Шифр Уитстона")]
        Wheatstone
    }
}