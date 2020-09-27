using CryptographicAlgorithms.Enums;
using Xunit;

namespace CryptographicAlgorithms.Tests {
    public class CryptographicAlgorithmsTests {
        private readonly CryptographicAlgorithm cryptographicAlgorithm = new CryptographicAlgorithm(Alphabet.Russian);
        [Fact]
        public void EncryptCaesarRus() {
            Assert.Equal("эщедечсающехщюкреицфт", cryptographicAlgorithm.EncryptCaesar("Чу_я_слышу_пушек_гром", 6));
            Assert.Equal("фрэьэоишхрэмрхвзэанлй", cryptographicAlgorithm.EncryptCaesar("Чу_я_слышу_пушек_гром", 31));
        }
        [Fact]
        public void DecryptCaesarRus() {
            Assert.Equal("снъщълёхтнъйнт_еъюкиж", cryptographicAlgorithm.DecryptCaesar("Чу_я_слышу_пушек_гром", 6));
            Assert.Equal("ъцвбвфоюыцвтцызнвёусп", cryptographicAlgorithm.DecryptCaesar("Чу_я_слышу_пушек_гром", 31));
        }
        [Fact]
        public void EncryptVigenereRus() {
            Assert.Equal("чфббгцсб_уасцьйрёкрпо", cryptographicAlgorithm.EncryptVigenere("Чу_я_слышу_пушек_гром", "абвгдеёжз"));
            Assert.Equal("бяэхйэисвяэёюгвбйонеч", cryptographicAlgorithm.EncryptVigenere("Чу_я_слышу_пушек_гром", "кЛюЧ"));
        }
        [Fact]
        public void DecryptVigenereRus() {
            Assert.Equal("чтюььмёфруянрфаещьрнк", cryptographicAlgorithm.DecryptVigenere("Чу_я_слышу_пушек_гром", "абвгдеёжз"));
            Assert.Equal("мзвзхёоднзвщимзфхшушв", cryptographicAlgorithm.DecryptVigenere("Чу_я_слышу_пушек_гром", "кЛюЧ"));
        }
        [Fact]
        public void EncryptGronsfeldRus() {
            Assert.Equal("шубгасн_щубффшжпагтун", cryptographicAlgorithm.EncryptGronsfeld("Чу_я_слышу_пушек_гром", 1025));
            Assert.Equal("_щвжеффаыьетьюзуеёщфп", cryptographicAlgorithm.EncryptGronsfeld("Чу_я_слышу_пушек_гром", 963));
        }
        [Fact]
        public void DecryptGronsfeldRus() {
            Assert.Equal("ок_ючилъпк_окпейчырнд", cryptographicAlgorithm.DecryptGronsfeld("Чу_я_слышу_пушек_гром", 9901));
            Assert.Equal("снъщълёхтнъйнт_еъюкиж", cryptographicAlgorithm.DecryptGronsfeld("Чу_я_слышу_пушек_гром", 666666));
            Assert.Equal("цсэыылеупу_жлс_йюамйж", cryptographicAlgorithm.DecryptGronsfeld("Чу_я_слышу_пушек_гром", 123456789009876));
        }
        [Fact]
        public void EncryptScytaleRus() {
            Assert.Equal("пз_ьоржвм_иаоо_еюсг_", cryptographicAlgorithm.EncryptScytale("Приезжаю_восьмого", 5));
            Assert.Equal("чыеушк_у_я_г_прсуолшм", cryptographicAlgorithm.EncryptScytale("Чу_я_слышу_пушек_гром", 3));
        }
        [Fact]
        public void DecryptScytaleRus() {
            Assert.Equal("приезжаю_восьмого___", cryptographicAlgorithm.DecryptScytale("пз_ьоржвм_иаоо_еюсг_", 5));
            Assert.Equal("чу_я_слышу_пушек_гром", cryptographicAlgorithm.DecryptScytale("чыеушк_у_я_г_прсуолшм", 3));
        }
        [Fact]
        public void EncryptMagicSquareRus() {
            Assert.Equal("иг__о_ю_мра_ьП__сз_жоеов_", cryptographicAlgorithm.EncryptMagicSquare("Приезжаю_восьмого"));
            Assert.Equal("_кш_еоымшул_уЧр_п_гс_я_у_", cryptographicAlgorithm.EncryptMagicSquare("Чу_я_слышу_пушек_гром"));
        }
        [Fact]
        public void DecryptMagicSquareRus() {
            Assert.Equal("Чу_я_слышу_пушек_гром____", cryptographicAlgorithm.DecryptMagicSquare("_кш_еоымшул_уЧр_п_гс_я_у_"));
            Assert.Equal("Приезжаю_восьмого________", cryptographicAlgorithm.DecryptMagicSquare("иг__о_ю_мра_ьП__сз_жоеов_"));
        }
        [Fact]
        public void EncryptPolybiusRus() {
            Assert.Equal("эзхгбцёчфт", cryptographicAlgorithm.EncryptPolybius("чамирнеоль", "привет"));
            Assert.Equal("абгёонзрвдчштхчкч", cryptographicAlgorithm.EncryptPolybius("Приезжаю_восьмого", "привет"));
        }
        [Fact]
        public void DecryptPolybiusRus() {
            Assert.Equal("чамирнеоль", cryptographicAlgorithm.DecryptPolybius("эзхгбцёчфт", "привет"));
            Assert.Equal("приезжаю_восьмого", cryptographicAlgorithm.DecryptPolybius("абгёонзрвдчштхчкч", "привет"));
        }
    }
}
