﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using CryptographicAlgorithms.Enums;
using CryptographicAlgorithms.Extensions;

namespace CryptographicAlgorithms {
    public partial class MainWindow : Window, INotifyPropertyChanged {

        #region Properties
        private Alphabet alphabetSelectedItem;
        public Alphabet AlphabetSelectedItem {
            get => alphabetSelectedItem;
            set {
                ClearTextBox();
                alphabetSelectedItem = value;
                OnPropertyChanged();
            }
        }

        private Algorithm algorithmSelectedItem;
        public Algorithm AlgorithmSelectedItem {
            get => algorithmSelectedItem;
            set {
                ClearTextBox();
                algorithmSelectedItem = value;
                OnPropertyChanged();
            }
        }
        private string inputString;
        public string InputString {
            get => inputString;
            set {
                inputString = value;
                NumberOfSymbols = $"Количество символов: {inputString.Length}";
                OnPropertyChanged();
            }
        }

        private string numberOfSymbols = "Количество символов: 0";
        public string NumberOfSymbols {
            get => numberOfSymbols;
            set {
                numberOfSymbols = value;
                OnPropertyChanged();
            }
        }
        private bool algorithmDirection = true;
        #endregion

        public MainWindow() {
            InitializeComponent();
            DataContext = this;
        }

        private void ClearTextBox() {
            resultLabel.Text = "";
            InputString = "";
            keyTextBox.Text = "";
        }

        private void Apply_Click(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(InputString)) {
                MessageBox.Show("Введите текст", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrEmpty(keyTextBox.Text)) {
                if (AlgorithmSelectedItem != Algorithm.MagicSquare) {
                    MessageBox.Show("Введите ключ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            var cryptographicAlgorithm = new CryptographicAlgorithm(AlphabetSelectedItem);
            try {
                switch (algorithmDirection) {
                    case true:
                        switch (AlgorithmSelectedItem) {
                            case Algorithm.Polybius:
                                resultLabel.Text =
                                        cryptographicAlgorithm.EncryptPolybius(InputString,keyTextBox.Text.ToCharSquare());
                                    break;
                            case Algorithm.Caesar:
                                resultLabel.Text =
                                    cryptographicAlgorithm.EncryptCaesar(InputString, long.Parse(keyTextBox.Text));
                                break;
                            case Algorithm.Gronsfeld:
                                resultLabel.Text =
                                    cryptographicAlgorithm.EncryptGronsfeld(InputString,
                                        long.Parse(keyTextBox.Text));
                                break;
                            case Algorithm.Vigenere:
                                resultLabel.Text =
                                    cryptographicAlgorithm.EncryptVigenere(InputString, keyTextBox.Text);
                                break;
                            case Algorithm.Scytale:
                                if (CheckScytaleKeyTextBox()) {
                                    resultLabel.Text =
                                        cryptographicAlgorithm.EncryptScytale(InputString,
                                            int.Parse(keyTextBox.Text));
                                    break;
                                }
                                return;
                            case Algorithm.MagicSquare:
                                resultLabel.Text =
                                        cryptographicAlgorithm.EncryptMagicSquare(InputString, keyTextBox.Text.ToIntSquare());
                                    break;
                            case Algorithm.TableTransposition:
                                if (CheckTableTranspositionAlgorithmKeyTextBox()) {
                                    resultLabel.Text =
                                        cryptographicAlgorithm.EncryptTableTransposition(InputString, keyTextBox.Text);
                                    break;
                                }
                                return;
                            case Algorithm.Wheatstone:
                                resultLabel.Text =
                                        cryptographicAlgorithm.EncryptWheatstone(InputString, keyTextBox.Text);
                                    break;
                            case Algorithm.DoubleTransposition:
                                if (CheckDoubleTransposition()) {
                                    resultLabel.Text =
                                        cryptographicAlgorithm.EncryptDoubleTransposition(InputString, keyTextBox.Text);
                                    break;
                                }
                                return;
                        }
                        break;
                    case false:
                        switch (AlgorithmSelectedItem) {
                            case Algorithm.Polybius:
                                resultLabel.Text =
                                        cryptographicAlgorithm.DecryptPolybius(InputString, keyTextBox.Text.ToCharSquare());
                                    break;
                            case Algorithm.Caesar:
                                resultLabel.Text =
                                    cryptographicAlgorithm.DecryptCaesar(InputString, long.Parse(keyTextBox.Text));
                                break;
                            case Algorithm.Gronsfeld:
                                resultLabel.Text =
                                    cryptographicAlgorithm.DecryptGronsfeld(InputString,
                                        long.Parse(keyTextBox.Text));
                                break;
                            case Algorithm.Vigenere:
                                resultLabel.Text =
                                    cryptographicAlgorithm.DecryptVigenere(InputString, keyTextBox.Text);
                                break;
                            case Algorithm.Scytale:
                                if (CheckScytaleKeyTextBox()) {
                                    resultLabel.Text =
                                        cryptographicAlgorithm.DecryptScytale(InputString,
                                            int.Parse(keyTextBox.Text));
                                    break;
                                }
                                return;
                            case Algorithm.MagicSquare:
                                resultLabel.Text =
                                        cryptographicAlgorithm.DecryptMagicSquare(InputString, keyTextBox.Text.ToIntSquare());
                                    break;
                            case Algorithm.TableTransposition:
                                if (CheckTableTranspositionAlgorithmKeyTextBox()) {
                                    resultLabel.Text =
                                        cryptographicAlgorithm.DecryptTableTransposition(InputString, keyTextBox.Text);
                                    break;
                                }
                                return;
                            case Algorithm.Wheatstone:
                                resultLabel.Text =
                                        cryptographicAlgorithm.DecryptWheatstone(InputString, keyTextBox.Text);
                                    break;
                            case Algorithm.DoubleTransposition:
                                if (CheckDoubleTransposition()) {
                                    resultLabel.Text =
                                        cryptographicAlgorithm.DecryptDoubleTransposition(InputString, keyTextBox.Text);
                                    break;
                                }
                                return;
                        }
                        break;
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void ChangeAlgorithmDirection_OnClick(object sender, RoutedEventArgs e) {
            if (encryptBtn.IsEnabled == false) {
                encryptBtn.IsEnabled = true;
                decryptBtn.IsEnabled = false;
                algorithmDirection = false;
            } else {
                encryptBtn.IsEnabled = false;
                decryptBtn.IsEnabled = true;
                algorithmDirection = true;
            }
        }

        private void InputTextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e) {
            Regex r = null;
            switch (AlphabetSelectedItem) {
                case Alphabet.Russian:
                    r = new Regex("^[а-яА-ЯёЁ]*$");
                    break;
                case Alphabet.Latin:
                    r = new Regex("^[a-zA-Z]*$");
                    break;
            }
            e.Handled = !r.IsMatch(e.Text);
        }

        #region CheckFunctions
        private bool CheckScytaleKeyTextBox() {
            if (int.Parse(keyTextBox.Text) > InputString.Length) {
                MessageBox.Show("Введите диаметр меньший, чем длина сообщения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
        private bool CheckTableTranspositionAlgorithmKeyTextBox() {
            var str = keyTextBox.Text;
            var arr = new int[str.Length];
            for (int i = 0; i < str.Length; i++) {
                arr[i] = int.Parse(str[i].ToString());
            }
            var check = false;
            Array.Sort(arr);
            for (int i = 0; i < arr.Length; i++) {
                if (i + 1 != arr[i]) {
                    check = true;
                    break;
                }
            }
            if (keyTextBox.Text.Distinct().Count() != keyTextBox.Text.Length || keyTextBox.Text.Contains('0') || check) {
                MessageBox.Show("В ключе не должно быть повторяющихся цифр, нуля и цифр пропускающих числовую последовательность.\nПример ключа: 4132, 948172536", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private bool CheckDoubleTransposition() {
            var cryptFirstKey = keyTextBox.Text.Substring(0, keyTextBox.Text.IndexOf(' '));
            var cryptSecondKey = keyTextBox.Text.Substring(keyTextBox.Text.IndexOf(' ') + 1);
            if (cryptFirstKey.Length != cryptSecondKey.Length
                || cryptFirstKey.Length * cryptSecondKey.Length < InputString.Length
                || cryptSecondKey.Distinct().Count() != cryptSecondKey.Length
                || cryptFirstKey.Distinct().Count() != cryptFirstKey.Length) {
                MessageBox.Show(
                    "Ключи должны быть одинаковой длины, не содержать повторяющихся символов и количество символов в строке для шифрования должно быть больше произведения количества символов двух ключей.\nПример ключа: 839_дом",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
        #endregion

        private void KeyTextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            switch (AlgorithmSelectedItem) {
                case Algorithm.Gronsfeld:
                case Algorithm.TableTransposition:
                case Algorithm.Caesar:
                case Algorithm.Scytale:
                case Algorithm.MagicSquare:
                    var r1 = new Regex("^[0-9]*$");
                    e.Handled = !r1.IsMatch(e.Text);
                    break;
                case Algorithm.Vigenere:
                    Regex r2;
                    switch (AlphabetSelectedItem) {
                        case Alphabet.Russian:
                            r2 = new Regex("^[а-яА-ЯёЁ]*$");
                            e.Handled = !r2.IsMatch(e.Text);
                            break;
                        case Alphabet.Latin:
                            r2 = new Regex("^[a-zA-Z]*$");
                            e.Handled = !r2.IsMatch(e.Text);
                            break;
                    }
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
