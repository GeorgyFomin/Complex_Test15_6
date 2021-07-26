using System;
using System.IO;

namespace ComplexLib
{
    public static class Ex
    {
        public static void WriteBin(this Complex c, string fileName)
        {
            using (BinaryWriter binaryWriter = new BinaryWriter(File.Create(fileName)))
            {
                binaryWriter.Write(c.Real);
                binaryWriter.Write(c.Imaginary);
            }
        }
    }
    public struct Complex : IFormattable, IEquatable<Complex>
    {
        /// <summary>
        /// Хранит мнимую единицу.
        /// </summary>
        public static readonly Complex imaginaryOne = new Complex(0, 1);
        #region Properties (свойства)
        /// <summary>
        /// Возвращает вещественную часть комплексного числа.
        /// </summary>
        public double Real
        { get; }
        /// <summary>
        /// Возвращает мнимую часть комплексного числа.
        /// </summary>
        public double Imaginary
        { get; }
        /// <summary>
        /// Возвращает значение модуля комплексного числа.
        /// </summary>
        public double Magnitude => Math.Sqrt(Real * Real + Imaginary * Imaginary);
        /// <summary>
        /// Возвращает фазу комплексного числа.
        /// </summary>
        public double Phase => Math.Atan2(Imaginary, Real);
        #endregion
        /// <summary>
        /// Конструктор. Инициализирует поля структуры. 
        /// </summary>
        /// <param name="re">Вещественная часть.</param>
        /// <param name="im">Мнимая часть.</param>
        public Complex(double re, double im)
        {
            Real = re; Imaginary = im;
        }
        #region Operators
        /// <summary>
        /// Неявно преобразует число типа double в число типа Complex.
        /// </summary>
        /// <param name="d">Значение числа типа double.</param>
        public static implicit operator Complex(double d) => new Complex(d, 0);
        public static Complex operator +(Complex c1, Complex c2) => new Complex(c1.Real + c2.Real, c1.Imaginary + c2.Imaginary);
        public static Complex operator -(Complex c) => new Complex(-c.Real, -c.Imaginary);
        public static Complex operator -(Complex c1, Complex c2) => c1 + (-c2);
        public static Complex operator *(Complex c1, Complex c2) => new Complex(c1.Real * c2.Real - c1.Imaginary * c2.Imaginary,
                c1.Real * c2.Imaginary + c1.Imaginary * c2.Real);
        /// <summary>
        /// Определяет сопряженное число.
        /// </summary>
        /// <param name="c">Исходное число.</param>
        /// <returns>Сопряженное исходному.</returns>
        public static Complex operator ~(Complex c) => new Complex(c.Real, -c.Imaginary);
        public static Complex operator /(Complex c1, Complex c2)
        {
            double temp = 1 / c2.Magnitude;
            return temp * temp * c1 * ~c2;
        }
        public static bool operator ==(Complex c1, Complex c2) => c1.Equals(c2);
        public static bool operator !=(Complex c1, Complex c2) => !(c1 == c2);
        #endregion
        #region Methods
        /// <summary>
        /// Создает комплексное число по заданному модулю и фазе.
        /// </summary>
        /// <param name="magnitude">Модуль.</param>
        /// <param name="phase">Фаза.</param>
        /// <returns>Комплексное число.</returns>
        public static Complex FromPolarCoordinates(double magnitude, double phase) =>
            new Complex(magnitude * Math.Cos(phase), magnitude * Math.Sin(phase));
        #region Элементарные функции
        /// <summary>
        /// Извлекает квадратный корень из комплексного числа.
        /// </summary>
        /// <param name="c">Заданное число.</param>
        /// <returns></returns>
        public static Complex Sqrt(Complex c) => FromPolarCoordinates(Math.Sqrt(c.Magnitude), .5 * c.Phase);
        public static Complex Exp(Complex c) => FromPolarCoordinates(Math.Exp(c.Real), c.Imaginary);
        public static Complex Pow(Complex value, Complex power) =>
            value.Magnitude == 0 ? 0 : Exp((Math.Log(value.Magnitude) + value.Phase * imaginaryOne) * power);
        public static Complex Log(Complex c) => Math.Log(c.Magnitude) + imaginaryOne * c.Phase;
        public static Complex Cos(Complex c)
        {
            Complex exp = Exp(imaginaryOne * c);
            return .5 * (exp + 1 / exp);
        }
        public static Complex Acos(Complex c)
        {
            Complex exp = c + Sqrt(c * c - 1);
            return Log(exp) / imaginaryOne;
        }
        #endregion
        public static Complex Parse(string s)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException();
            if (!s.EndsWith(">") || !s.StartsWith("<") || !s.Contains(";"))
                throw new FormatException();
            int indx = s.IndexOf(';');
            return double.Parse(s.Substring(1, indx - 1)) +
               imaginaryOne * double.Parse(s.Substring(indx + 1, s.IndexOf('>') - indx - 1));
        }
        public static bool TryParse(string s, out Complex c)
        {
            c = double.NaN;
            try
            {
                c = Parse(s);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public void WriteToFile(string fileName)
        {
            BinaryWriter bw = new BinaryWriter(File.Create(fileName));
            using (bw)
            {
                bw.Write(Real);
                bw.Write(Imaginary);
            }
        }
        #region Переопределенные методы предков
        public override bool Equals(object obj) => base.Equals(obj);
        public override int GetHashCode() => base.GetHashCode();
        #endregion
        #region Методы, реализующие интерфейсы
        public bool Equals(Complex other) => Equals(other as object);
        public string ToString(string format = null, IFormatProvider formatProvider = null)
            => "<" + Real.ToString(format, formatProvider) + ";" + Imaginary.ToString(format, formatProvider) + ">";
        #endregion
        #endregion
        class BinaryWriter : System.IO.BinaryWriter
        {
            internal BinaryWriter(Stream stream) : base(stream)
            { }
            internal void Write(Complex c)
            {
                base.Write(c.Real);
                base.Write(c.Imaginary);
            }
        }
    }
}
