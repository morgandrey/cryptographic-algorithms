using System;
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
            var cryptographicAlgorithm = new CryptographicAlgorithm(AlphabetSelectedItem);
            try {
                switch (algorithmDirection) {
                    case true:
                        switch (AlgorithmSelectedItem) {
                            case Algorithm.Polybius:
                                if (keyTextBox.Text.Distinct().Count() != keyTextBox.Text.Length) {
                                    MessageBox.Show("Введите ключ без повторяющих символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                                    return;
                                }
                                resultLabel.Text =
                                    cryptographicAlgorithm.EncryptPolybius(InputString, keyTextBox.Text);
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
                                resultLabel.Text =
                                    cryptographicAlgorithm.EncryptScytale(InputString,
                                        long.Parse(keyTextBox.Text));
                                break;
                            case Algorithm.MagicSquare:
                                if (InputString.Length > 25) {
                                    MessageBox.Show("Введите сообщение меньше 25 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                                    return;
                                }
                                resultLabel.Text =
                                    cryptographicAlgorithm.EncryptMagicSquare(InputString);
                                break;
                            case Algorithm.Tables:
                                resultLabel.Text =
                                    cryptographicAlgorithm.EncryptTables(InputString, long.Parse(keyTextBox.Text));
                                break;
                        }
                        break;
                    case false:
                        switch (AlgorithmSelectedItem) {
                            case Algorithm.Polybius:
                                if (keyTextBox.Text.Distinct().Count() != keyTextBox.Text.Length) {
                                    MessageBox.Show("Введите ключ без повторяющих символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                                    return;
                                }
                                resultLabel.Text =
                                    cryptographicAlgorithm.DecryptPolybius(InputString, keyTextBox.Text);
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
                                resultLabel.Text =
                                    cryptographicAlgorithm.DecryptScytale(InputString,
                                        long.Parse(keyTextBox.Text));
                                break;
                            case Algorithm.MagicSquare:
                                if (InputString.Length > 25) {
                                    MessageBox.Show("Введите сообщение меньше 25 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                                    return;
                                }
                                resultLabel.Text =
                                    cryptographicAlgorithm.DecryptMagicSquare(InputString);
                                break;
                            case Algorithm.Tables:
                                resultLabel.Text =
                                    cryptographicAlgorithm.DecryptTables(InputString, long.Parse(keyTextBox.Text));
                                break;
                        }
                        break;
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void ChangeDirection_OnClick(object sender, RoutedEventArgs e) {
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

        private void KeyTextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e) {
            Regex r = null;
            switch (AlgorithmSelectedItem) {
                case Algorithm.Gronsfeld:
                case Algorithm.Tables:
                case Algorithm.Caesar:
                case Algorithm.Scytale:
                    r = new Regex("^[0-9]*$");
                    break;
                case Algorithm.Vigenere:
                case Algorithm.Polybius:
                    switch (AlphabetSelectedItem) {
                        case Alphabet.Russian:
                            r = new Regex("^[а-яА-ЯёЁ]*$");
                            break;
                        case Alphabet.Latin:
                            r = new Regex("^[a-zA-Z]*$");
                            break;
                    }
                    break;
            }
            e.Handled = !r.IsMatch(e.Text);
        }

        private void KeyTextBox_OnPreviewKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Space) {
                e.Handled = true;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
