using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using CryptographicAlgorithms.Enums;
using CryptographicAlgorithms.Extensions;

namespace CryptographicAlgorithms {
    public partial class MainWindow : Window, INotifyPropertyChanged {
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

        private bool algorithmDirection = true;
        public MainWindow() {
            InitializeComponent();
            DataContext = this;
        }

        private void ClearTextBox() {
            resultLabel.Content = "";
            inputTextBox.Text = "";
            keyTextBox.Text = "";
        }
        private void Apply_Click(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(inputTextBox.Text)) {
                return;
            }
            var cryptographicAlgorithm = new CryptographicAlgorithm(AlphabetSelectedItem);
            switch (algorithmDirection) {
                case true:
                    switch (AlgorithmSelectedItem) {
                        case Algorithm.Polybius:
                            resultLabel.Content =
                                cryptographicAlgorithm.EncryptPolybius(inputTextBox.Text, keyTextBox.Text);
                            break;
                        case Algorithm.Caesar:
                            resultLabel.Content = cryptographicAlgorithm.EncryptCaesar(inputTextBox.Text, int.Parse(keyTextBox.Text));
                            break;
                        case Algorithm.Gronsfeld:
                            resultLabel.Content =
                                cryptographicAlgorithm.EncryptGronsfeld(inputTextBox.Text,
                                    int.Parse(keyTextBox.Text));
                            break;
                        case Algorithm.Vigenere:
                            resultLabel.Content =
                                cryptographicAlgorithm.EncryptVigenere(inputTextBox.Text, keyTextBox.Text);
                            break;
                        case Algorithm.Scytale:
                            resultLabel.Content =
                                cryptographicAlgorithm.EncryptScytale(inputTextBox.Text, int.Parse(keyTextBox.Text));
                            break;
                    }
                    break;
                case false:
                    switch (AlgorithmSelectedItem) {
                        case Algorithm.Polybius:
                            resultLabel.Content =
                                cryptographicAlgorithm.DecryptPolybius(inputTextBox.Text, keyTextBox.Text);
                            break;
                        case Algorithm.Caesar:
                            resultLabel.Content =
                                cryptographicAlgorithm.DecryptCaesar(inputTextBox.Text, int.Parse(keyTextBox.Text));
                            break;
                        case Algorithm.Gronsfeld:
                            resultLabel.Content =
                                cryptographicAlgorithm.DecryptGronsfeld(inputTextBox.Text,
                                    int.Parse(keyTextBox.Text));
                            break;
                        case Algorithm.Vigenere:
                            resultLabel.Content =
                                cryptographicAlgorithm.DecryptVigenere(inputTextBox.Text, keyTextBox.Text);
                            break;
                        case Algorithm.Scytale:
                            resultLabel.Content =
                                cryptographicAlgorithm.DecryptScytale(inputTextBox.Text, int.Parse(keyTextBox.Text));
                            break;
                    }
                    break;
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
