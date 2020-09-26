using System;
using CryptographicAlgorithms.Enums;

namespace CryptographicAlgorithms {
    public class CryptographicAlgorithm {
        private const string rusAlphabetLower = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        private const string engAlphabetLower = "abcdefghijklmnopqrstuvwxyz";
        private readonly string _alphabet;
        public CryptographicAlgorithm(Alphabet alphabet) {
            switch (alphabet) {
                case Alphabet.Russian:
                    _alphabet = rusAlphabetLower;
                    break;
                case Alphabet.Latin:
                    _alphabet = engAlphabetLower;
                    break;
            }
        }

        #region Caesar
        private string CodeEncodeCaesar(string text, int k) {
            var retVal = "";
            text = text.ToLower();
            var letterQty = _alphabet.Length;
            for (int i = 0; i < text.Length; i++) {
                var c = text[i];
                var index = _alphabet.IndexOf(c);
                if (index < 0) {
                    retVal += c.ToString();
                } else {
                    var codeIndex = (letterQty + index + k) % letterQty;
                    retVal += _alphabet[codeIndex];
                }
            }
            return retVal;
        }
        public string EncryptCaesar(string plainMessage, int key) => CodeEncodeCaesar(plainMessage, key);
        public string DecryptCaesar(string encryptedMessage, int key) => CodeEncodeCaesar(encryptedMessage, -key);
        #endregion

        #region Vigenere
        private string CodeEncodeVigenere(string text, string key, bool encrypt = true) {
            text = text.ToLower();
            key = key.ToLower();
            var resStr = "";
            var keyStr = "";
            for (int i = 0, j = 0; i < text.Length; i++, j++) {
                if (text[i] == ' ') {
                    keyStr += " ";
                    j--;
                    continue;
                }
                if (j == key.Length) {
                    j = 0;
                }
                keyStr += key[j];
            }
            var letterQty = rusAlphabetLower.Length;
            for (int i = 0; i < text.Length; i++) {
                var p = text[i];
                var c = keyStr[i];
                var indexKey = _alphabet.IndexOf(c);
                var index = _alphabet.IndexOf(p);
                if (index < 0 || indexKey < 0) {
                    resStr += p.ToString();
                } else {
                    var codeIndex = (letterQty + index + (encrypt ? 1 : -1) * indexKey) % letterQty;
                    resStr += _alphabet[codeIndex];
                }
            }
            return resStr;
        }
        public string EncryptVigenere(string plainMessage, string key) => CodeEncodeVigenere(plainMessage, key);
        public string DecryptVigenere(string encryptedMessage, string key) => CodeEncodeVigenere(encryptedMessage, key, false);
        #endregion

        #region Gronsfeld
        private string CodeEncodeGronsfeld(string text, int key, bool encrypt = true) {
            text = text.ToLower();
            var resStr = "";
            var keyStr = "";
            for (int i = 0, j = 0; i < text.Length; i++, j++) {
                if (j == key.ToString().Length) {
                    j = 0;
                }
                keyStr += key.ToString()[j];
            }
            var letterQty = rusAlphabetLower.Length;
            for (int i = 0; i < text.Length; i++) {
                var p = text[i];
                var index = _alphabet.IndexOf(p);
                var codeIndex = (letterQty + index + (encrypt ? 1 : -1) * (int)char.GetNumericValue(keyStr[i])) % letterQty;
                resStr += _alphabet[codeIndex];
            }
            return resStr;
        }
        public string EncryptGronsfeld(string plainMessage, int key) => CodeEncodeGronsfeld(plainMessage, key);
        public string DecryptGronsfeld(string encryptedMessage, int key) => CodeEncodeGronsfeld(encryptedMessage, key, false);
        #endregion

        #region Polybius
        private char[,] GetPolybiusSquare(string key) {
            var newAlphabet = _alphabet;
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
            var square = GetPolybiusSquare(key);
            var resStr = "";
            var n = square.GetLength(0);
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
        public string EncryptScytale(string plainMessage, int key) {
            plainMessage = plainMessage.ToLower();
            var k = plainMessage.Length % key;
            if (k > 0) {
                plainMessage += new string(' ', key - k);
            }

            var column = plainMessage.Length / key;
            var result = "";

            for (int i = 0; i < column; i++) {
                for (int j = 0; j < key; j++) {
                    result += plainMessage[i + column * j].ToString();
                }
            }

            return result;
        }
        public string DecryptScytale(string encryptedMessage, int key) {
            encryptedMessage = encryptedMessage.ToLower();
            var column = encryptedMessage.Length / key;
            var symbols = new char[encryptedMessage.Length];
            int index = 0;
            for (int i = 0; i < column; i++) {
                for (int j = 0; j < key; j++) {
                    symbols[i + column * j] = encryptedMessage[index];
                    index++;
                }
            }
            return string.Join("", symbols);
        }
        #endregion
    }
}