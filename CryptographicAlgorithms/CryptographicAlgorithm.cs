using System;
using CryptographicAlgorithms.Enums;

namespace CryptographicAlgorithms {
    public class CryptographicAlgorithm {
        private const string rusAlphabetLower = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя_";
        private const string engAlphabetLower = "abcdefghijklmnopqrstuvwxyz_";
        private readonly string alphabet;
        private readonly int[,] magicSquare = {
            { 3, 16, 9, 22, 15 },
            { 20, 8, 21, 14, 2 },
            { 7, 25, 13, 1, 19 },
            { 24, 12, 5, 18, 6 },
            { 11, 4, 17, 10, 23 }
        };
        private readonly char[,] wheatStoneFirstMatrix = {
            {'ж', 'щ', 'н', 'ю', 'р'},
            {'и', 'т', 'ь', 'ц', 'б'},
            {'я', 'м', 'е', '.', 'с'},
            {'в', 'ы', 'п', 'ч', ' '},
            {'й', 'д', 'у', 'о', 'к'},
            {'з', 'э', 'ф', 'г', 'ш'},
            {'х', 'а', ',', 'л', 'ъ'}
        };
        private readonly char[,] wheatStoneSecondMatrix = {
            {'и', 'ч', 'г', 'я', 'т'},
            {',', 'ж', 'м', 'ь', 'о'},
            {'з', 'ю', 'р', 'в', 'щ'},
            {'ц', 'й', 'п', 'е', 'л'},
            {'ъ', 'а', 'н', '.', 'х'},
            {'э', 'к', 'с', 'ш', 'д'},
            {'б', 'ф', 'у', 'ы', ' '}
        };
        public CryptographicAlgorithm(Alphabet alphabet) {
            switch (alphabet) {
                case Alphabet.Russian:
                    this.alphabet = rusAlphabetLower;
                    break;
                case Alphabet.Latin:
                    this.alphabet = engAlphabetLower;
                    break;
            }
        }

        #region Caesar
        private string CodeEncodeCaesar(string text, long k) {
            var retVal = "";
            text = text.ToLower();
            var letterQty = alphabet.Length;
            for (int i = 0; i < text.Length; i++) {
                var c = text[i];
                var index = alphabet.IndexOf(c);
                if (index < 0) {
                    retVal += c.ToString();
                } else {
                    var codeIndex = (letterQty + index + k) % letterQty;
                    retVal += alphabet[(int)codeIndex];
                }
            }
            return retVal;
        }
        public string EncryptCaesar(string plainMessage, long key) => CodeEncodeCaesar(plainMessage, key);
        public string DecryptCaesar(string encryptedMessage, long key) => CodeEncodeCaesar(encryptedMessage, -key);
        #endregion

        #region Vigenere
        private string CodeEncodeVigenere(string text, string key, bool encrypt = true) {
            text = text.ToLower();
            key = key.ToLower();
            var keyStr = "";
            var resStr = "";
            for (int i = 0, j = 0; i < text.Length; i++, j++) {
                if (j == key.Length) {
                    j = 0;
                }
                keyStr += key[j];
            }
            var letterQty = alphabet.Length;
            for (int i = 0; i < text.Length; i++) {
                var p = text[i];
                var c = keyStr[i];
                var indexKey = alphabet.IndexOf(c);
                var index = alphabet.IndexOf(p);
                if (index < 0 || indexKey < 0) {
                    resStr += p.ToString();
                } else {
                    var codeIndex = (letterQty + index + (encrypt ? 1 : -1) * indexKey) % letterQty;
                    resStr += alphabet[codeIndex];
                }
            }
            return resStr;
        }
        public string EncryptVigenere(string plainMessage, string key) => CodeEncodeVigenere(plainMessage, key);
        public string DecryptVigenere(string encryptedMessage, string key) => CodeEncodeVigenere(encryptedMessage, key, false);
        #endregion

        #region Gronsfeld
        private string CodeEncodeGronsfeld(string text, long key, bool encrypt = true) {
            text = text.ToLower();
            var keyStr = "";
            var resStr = "";
            for (int i = 0, j = 0; i < text.Length; i++, j++) {
                if (j == key.ToString().Length) {
                    j = 0;
                }
                keyStr += key.ToString()[j];
            }
            var letterQty = alphabet.Length;
            for (int i = 0; i < text.Length; i++) {
                var p = text[i];
                var index = alphabet.IndexOf(p);
                var codeIndex = (letterQty + index + (encrypt ? 1 : -1) * (int)char.GetNumericValue(keyStr[i])) % letterQty;
                resStr += alphabet[codeIndex];
            }
            return resStr;
        }
        public string EncryptGronsfeld(string plainMessage, long key) => CodeEncodeGronsfeld(plainMessage, key);
        public string DecryptGronsfeld(string encryptedMessage, long key) => CodeEncodeGronsfeld(encryptedMessage, key, false);
        #endregion

        #region Polybius
        private char[,] GetPolybiusSquare(string key) {
            var newAlphabet = alphabet;
            for (int i = 0; i < key.Length; i++) {
                newAlphabet = newAlphabet.Replace(key[i].ToString(), "");
            }
            newAlphabet = key + newAlphabet;
            var n = (int)Math.Ceiling(Math.Sqrt(newAlphabet.Length));
            var square = new char[n, n];
            var index = 0;
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    if (index < newAlphabet.Length) {
                        square[i, j] = newAlphabet[index];
                        index++;
                    } else {
                        square[i, j] = ' ';
                    }
                }
            }
            return square;
        }
        public string EncryptPolybius(string text, string key) {
            text = text.ToLower();
            var square = GetPolybiusSquare(key);
            var n = square.GetLength(0);
            var resStr = "";
            for (int i = 0; i < text.Length; i++) {
                for (int j = 0; j < square.GetLength(0); j++) {
                    for (int k = 0; k < square.GetLength(1); k++) {
                        if (text[i] == square[j, k]) {
                            if (j == n - 1) {
                                resStr += square[0, k];
                                continue;
                            }
                            if (square[j + 1, k] == ' ') {
                                resStr += square[0, k];
                                continue;
                            }
                            resStr += square[j + 1, k];
                        }
                    }
                }
            }
            return resStr;
        }
        public string DecryptPolybius(string encryptedMessage, string key) {
            encryptedMessage = encryptedMessage.ToLower();
            var square = GetPolybiusSquare(key);
            var resStr = "";
            var n = square.GetLength(0);
            for (int i = 0; i < encryptedMessage.Length; i++) {
                for (int j = 0; j < square.GetLength(0); j++) {
                    for (int k = 0; k < square.GetLength(1); k++) {
                        if (encryptedMessage[i] == square[j, k]) {
                            if (j == 0 && square[n - 1, k] == ' ') {
                                resStr += square[n - 2, k];
                                continue;
                            }
                            if (j == 0) {
                                resStr += square[n - 1, k];
                                continue;
                            }
                            resStr += square[j - 1, k];
                        }
                    }
                }
            }
            return resStr;
        }
        #endregion

        #region Scytale
        public string EncryptScytale(string plainMessage, long key) {
            plainMessage = plainMessage.ToLower();
            var k = plainMessage.Length % key;
            if (k > 0) {
                plainMessage += new string('_', (int)(key - k));
            }

            var column = plainMessage.Length / key;
            var result = "";

            for (int i = 0; i < column; i++) {
                for (int j = 0; j < key; j++) {
                    result += plainMessage[(int)(i + column * j)].ToString();
                }
            }

            return result;
        }
        public string DecryptScytale(string encryptedMessage, long key) {
            encryptedMessage = encryptedMessage.ToLower();
            var column = encryptedMessage.Length / key;
            var symbols = new char[encryptedMessage.Length];
            var index = 0;
            for (int i = 0; i < column; i++) {
                for (int j = 0; j < key; j++) {
                    symbols[i + column * j] = encryptedMessage[index];
                    index++;
                }
            }
            return string.Join("", symbols);
        }
        #endregion

        #region Tables
        public string EncryptTables(string text, long key) {
            text = text.ToLower();
            var resStr = "";
            return resStr;
        }

        public string DecryptTables(string text, long key) {
            text = text.ToLower();
            var resStr = "";
            return resStr;
        }

        #endregion

        #region MagicSquare 5x5
            public string EncryptMagicSquare(string plainMessage) {
            var resStr = "";
            char[,] arr = {
                {'_', '_', '_', '_', '_'},
                {'_', '_', '_', '_', '_'},
                {'_', '_', '_', '_', '_'},
                {'_', '_', '_', '_', '_'},
                {'_', '_', '_', '_', '_'}
            };
            for (int i = 0; i < plainMessage.Length; i++) {
                for (int j = 0; j < magicSquare.GetLength(0); j++) {
                    for (int k = 0; k < magicSquare.GetLength(1); k++) {
                        if (magicSquare[j, k] == i + 1) {
                            arr[j, k] = plainMessage[i];
                        }
                    }
                }
            }
            for (int i = 0; i < arr.GetLength(0); i++) {
                for (int j = 0; j < arr.GetLength(1); j++) {
                    resStr += arr[i, j];
                }
            }
            return resStr;
        }

        public string DecryptMagicSquare(string encryptedMessage) {
            var resStr = "";
            var index = 0;
            var arr = new char[5, 5];
            for (int i = 0; i < arr.GetLength(0); i++) {
                for (int j = 0; j < arr.GetLength(1); j++) {
                    arr[i, j] = encryptedMessage[index];
                    index++;
                }
            }
            for (int j = 0; j < encryptedMessage.Length; j++) {
                for (int i = 0; i < arr.GetLength(0); i++) {
                    for (int k = 0; k < arr.GetLength(1); k++) {
                        if (magicSquare[i, k] == j + 1) {
                            resStr += arr[i, k];
                        }
                    }
                }
            }
            return resStr;
        }
        #endregion

        #region Wheatstone
        public string EncryptWheatstone(string plainMessage) {
            var resStr = "";
            var arr = new char[5, 5];
            for (int i = 0; i < plainMessage.Length; i++) {
                for (int j = 0; j < magicSquare.GetLength(0); j++) {
                    for (int k = 0; k < magicSquare.GetLength(1); k++) {
                        if (magicSquare[j, k] == i + 1) {
                            arr[j, k] = plainMessage[i];
                        }
                    }
                }
            }

            for (int i = 0; i < arr.GetLength(0); i++) {
                for (int j = 0; j < arr.GetLength(1); j++) {
                    resStr += arr[i, j];
                }
            }
            return resStr;
        }


        #endregion
    }
}