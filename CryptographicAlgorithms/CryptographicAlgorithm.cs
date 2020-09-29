using System;
using System.Collections.Generic;
using System.Linq;
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
            if (Math.Abs(k) > alphabet.Length) {
                k %= alphabet.Length;
            }
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
        private char[,] GetPolybiusOrWheatstoneSquare(string key, bool polybius = true) {
            var newAlphabet = alphabet;
            for (int i = 0; i < key.Length; i++) {
                newAlphabet = newAlphabet.Replace(key[i].ToString(), "");
            }
            newAlphabet = key + newAlphabet;
            var n = (int)Math.Ceiling(Math.Sqrt(newAlphabet.Length));
            var square = new char[n, n];
            var index = 0;
            var randStringIndex = 0;
            var randStr = "1234567890!@#$%^&*";
            if (polybius) {
                for (int i = 0; i < n; i++) {
                    for (int j = 0; j < n; j++) {
                        if (index < newAlphabet.Length) {
                            square[i, j] = newAlphabet[index];
                            index++;
                        }
                        else {
                            square[i, j] = ' ';
                        }
                    }
                }
            } else {
                for (int i = 0; i < n; i++) {
                    for (int j = 0; j < n; j++) {
                        if (index < newAlphabet.Length) {
                            square[i, j] = newAlphabet[index];
                            index++;
                        } else {
                            square[i, j] = randStr[randStringIndex];
                            randStringIndex++;
                        }
                    }
                }
            }

            return square;
        }
        public string EncryptPolybius(string text, string key) {
            text = text.ToLower();
            var square = GetPolybiusOrWheatstoneSquare(key);
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
            var square = GetPolybiusOrWheatstoneSquare(key);
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
                plainMessage += new string('_', key - k);
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

        #region TableTransposition
        public string CodeEncodeTableTransposition(string text, string key, bool encrypt = true) {
            text = text.ToLower();
            int[] _key = new int[key.Length];
            for (int i = 0; i < key.Length; i++) {
                _key[i] = int.Parse(key[i].ToString());
            }
            var resStr = "";

            for (int i = 0; i < text.Length % _key.Length; i++) {
                text += "_";
            }

            for (int i = 0; i < text.Length; i += _key.Length) {
                var transposition = new char[_key.Length];

                for (int j = 0; j < _key.Length; j++) {
                    if (encrypt) {
                        transposition[_key[j] - 1] = text[i + j];
                    } else {
                        transposition[j] = text[i + _key[j] - 1];
                    }
                }

                for (int j = 0; j < _key.Length; j++) {
                    resStr += transposition[j];
                }
            }

            return resStr;
        }
        public string EncryptTableTransposition(string plainMessage, string key) => CodeEncodeTableTransposition(plainMessage, key);
        public string DecryptTableTransposition(string encryptedMessage, string key) => CodeEncodeTableTransposition(encryptedMessage, key, false);

        #endregion

        #region DoubleTransposition
        private int[] StringToArrayNumberPos(string key) {
            var alphabetDictionary = new Dictionary<char, int> {
                { '1', 1 }, { '2', 2 }, { '3', 3 }, { '4', 4 }, { '5', 5 }, { '6', 6 },
                { '7', 7 }, { '8', 8 }, { '9', 9 }, { 'а', 10 }, { 'б', 11 }, { 'в', 12 },
                { 'г', 13 }, { 'д', 14 }, { 'е', 15 }, { 'ё', 16 }, { 'ж', 17 }, { 'з', 18 },
                { 'и', 19 }, { 'й', 20 }, { 'к', 21 }, { 'л', 22 }, { 'м', 23 }, { 'н', 24 },
                { 'о', 25 }, { 'п', 26 }, { 'р', 27 }, { 'с', 28 }, { 'т', 29 }, { 'у', 30 },
                { 'ф', 31 }, { 'х', 32 }, { 'ц', 33 }, { 'ч', 34 }, { 'ш', 35 }, { 'щ', 36 },
                { 'ъ', 37 }, { 'ы', 38 }, { 'ь', 39 }, { 'э', 40 }, { 'ю', 41 }, { 'я', 42 },
                { 'a', 43 }, { 'b', 44 }, { 'c', 45 }, { 'd', 46 }, { 'e', 47 }, { 'f', 48 },
                { 'g', 49 }, { 'h', 50 }, { 'i', 51 }, { 'j', 52 }, { 'k', 53 }, { 'l', 54 },
                { 'm', 55 }, { 'n', 56 }, { 'o', 57 }, { 'p', 58 }, { 'q', 59 }, { 'r', 60 },
                { 's', 61 }, { 't', 62 }, { 'u', 63 }, { 'v', 64 }, { 'w', 65 }, { 'x', 66 },
                { 'y', 67 }, { 'z', 68 }
            };
            var arr = new int[key.Length];
            var count = 0;
            foreach (var literal in key) {
                arr[count++] = alphabetDictionary[literal];
            }
            return arr;
        }
        private string CodeEncodeDoubleTransposition(string text, string key, bool encrypt = true) {
            text = text.ToLower();
            key = key.ToLower();
            var resStr = "";
            var _cryptFirstKey = key.Substring(0, key.IndexOf(' '));
            var _cryptSecondKey = key.Substring(key.IndexOf(' ') + 1);

            var cryptFirstKey = StringToArrayNumberPos(_cryptFirstKey);
            var cryptSecondKey = StringToArrayNumberPos(_cryptSecondKey);

            var sortedFirstKey = cryptFirstKey.OrderBy(x => x).ToArray();
            var sortedSecondKey = cryptSecondKey.OrderBy(x => x).ToArray();


            var encryptStr = new char[sortedFirstKey.Length, sortedSecondKey.Length];
            var originalStrArray = new char[sortedFirstKey.Length, sortedSecondKey.Length];

            // преобразуем шифруемую строку в массив
            int count = 0;
            for (int i = 0; i < sortedFirstKey.Length; i++) {
                for (int j = 0; j < sortedSecondKey.Length; j++) {
                    if (encrypt) {
                        originalStrArray[i, j] = text.Length <= count ? '_' : text[count];
                    } else {
                        originalStrArray[j, i] = text.Length <= count ? '_' : text[count];
                    }
                    count++;
                }
            }

            // шифрование
            for (int i = 0; i < sortedFirstKey.Length; i++) {
                for (int j = 0; j < sortedSecondKey.Length; j++) {
                    var rowNumber = encrypt ? Array.IndexOf(cryptSecondKey, sortedSecondKey[i]) : Array.IndexOf(sortedSecondKey, cryptSecondKey[i]);
                    var columnNumber = encrypt ? Array.IndexOf(cryptFirstKey, sortedFirstKey[j]) : Array.IndexOf(sortedFirstKey, cryptFirstKey[j]);
                    encryptStr[i, j] = originalStrArray[rowNumber, columnNumber];
                }
            }

            for (int i = 0; i < sortedFirstKey.Length; i++) {
                for (int j = 0; j < sortedSecondKey.Length; j++) {
                    if (encrypt) {
                        resStr += encryptStr[j, i];
                    } else {
                        resStr += encryptStr[i, j];
                    }
                }
            }
            return resStr;
        }
        public string EncryptDoubleTransposition(string plainMessage, string key) => CodeEncodeDoubleTransposition(plainMessage, key);
        public string DecryptDoubleTransposition(string encryptedMessage, string key) => CodeEncodeDoubleTransposition(encryptedMessage, key, false);
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
            char[,] arr = new char[5, 5];
            for (int i = 0; i < arr.GetLength(0); i++) {
                for (int j = 0; j < arr.GetLength(1); j++) {
                    if (index == encryptedMessage.Length) {
                        arr[i, j] = '_';
                    } else {
                        arr[i, j] = encryptedMessage[index];
                        index++;
                    }
                }
            }
            for (int j = 0; j < magicSquare.Length; j++) {
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
        private int[] FindIndexes(char symbol, char[,] matrix) {
            var a = new int[2];
            for (int i = 0; i < matrix.GetLength(0); i++) {
                for (int j = 0; j < matrix.GetLength(1); j++) {
                    if (symbol == matrix[i, j]) {
                        a[0] = i;
                        a[1] = j;
                    }
                }
            }
            return a;
        }
        private string CodeEncodeWheatstone(string message, string key, bool encrypt = true) {
            message = message.ToLower();
            var firstKey = key.Substring(0, key.IndexOf(' '));
            var secondKey = key.Substring(key.IndexOf(' ') + 1);
            var wheatStoneFirstMatrix = GetPolybiusOrWheatstoneSquare(firstKey, false);
            var wheatStoneSecondMatrix = GetPolybiusOrWheatstoneSquare(secondKey, false);
            var resStr = "";
            if (message.Length % 2 != 0) {
                message += '_';
            }
            var length = message.Length / 2;
            var k = 0;
            var bigram = new char[length, 2];
            var crypto_bigram = new char[length, 2];

            for (int i = 0; i < length; i++) {
                for (int j = 0; j < 2; j++) {
                    bigram[i, j] = message[k];
                    k++;
                }
            }
            var step = 0;
            while (step < length) {
                var cortege1 = encrypt ? FindIndexes(bigram[step, 0], wheatStoneFirstMatrix) : FindIndexes(bigram[step, 0], wheatStoneSecondMatrix);
                var cortege2 = encrypt ? FindIndexes(bigram[step, 1], wheatStoneSecondMatrix) : FindIndexes(bigram[step, 1], wheatStoneFirstMatrix);
                crypto_bigram[step, 0] = encrypt ? wheatStoneSecondMatrix[cortege1[0], cortege2[1]] : wheatStoneFirstMatrix[cortege1[0], cortege2[1]];
                crypto_bigram[step, 1] = encrypt ? wheatStoneFirstMatrix[cortege2[0], cortege1[1]] : wheatStoneSecondMatrix[cortege2[0], cortege1[1]];
                step++;
            }

            for (int i = 0; i < length; i++) {
                for (int j = 0; j < 2; j++) {
                    resStr += crypto_bigram[i, j].ToString();
                }
            }
            return resStr;
        }
        public string EncryptWheatstone(string message, string key) => CodeEncodeWheatstone(message, key);
        public string DecryptWheatstone(string encryptedMessage, string key) => CodeEncodeWheatstone(encryptedMessage, key, false);
        #endregion
    }
}