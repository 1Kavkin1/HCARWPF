using System;

namespace CarWpfApp
{
    // Делегат для виведення інформації про автомобіль на дисплей
    public delegate void DisplayHandler(string message);

    /// <summary>
    /// Клас Автомобіль — реалізує патерн спостерігача через делегат/подію.
    /// Якщо дисплей підключається пізніше, він починає отримувати інформацію
    /// одразу після присвоєння делегату.
    /// </summary>
    public class Car
    {
        // ─── Властивості ────────────────────────────────────────────────────

        /// <summary>Марка автомобіля</summary>
        public string Brand { get; set; }

        /// <summary>Модель автомобіля</summary>
        public string Model { get; set; }

        /// <summary>Рік випуску</summary>
        public int Year { get; set; }

        /// <summary>Максимальна швидкість (км/год)</summary>
        public double MaxSpeed { get; set; }

        /// <summary>Колір кузова</summary>
        public string Color { get; set; }

        // ─── Делегат / callback для дисплея ─────────────────────────────────

        /// <summary>
        /// Делегат-обробник виведення інформації.
        /// Присвоюється ззовні — тоді "підключається дисплей".
        /// Якщо null — дисплей відсутній, вивід не відбувається.
        /// </summary>
        public DisplayHandler? DisplayCallback;

        // Альтернативно можна використати Action<string>:
        // public Action<string>? OnDisplayInfo;

        // ─── Конструктор ─────────────────────────────────────────────────────

        /// <summary>
        /// Ініціалізує новий екземпляр автомобіля.
        /// Дисплей при створенні ще не підключений (DisplayCallback == null).
        /// </summary>
        public Car(string brand, string model, int year, double maxSpeed, string color)
        {
            Brand    = brand;
            Model    = model;
            Year     = year;
            MaxSpeed = maxSpeed;
            Color    = color;
        }

        // ─── Методи ──────────────────────────────────────────────────────────

        /// <summary>
        /// Виводить повну інформацію про автомобіль через підключений дисплей.
        /// Якщо дисплей не підключено — кидає виняток із відповідним повідомленням.
        /// </summary>
        public void ShowInfo()
        {
            if (DisplayCallback == null)
                throw new InvalidOperationException("Дисплей не підключено!");

            string info = BuildInfoString();
            DisplayCallback(info);       // викликаємо делегат → дані йдуть на дисплей
        }

        /// <summary>
        /// Підключає дисплей (призначає обробник делегату) і одразу надсилає
        /// йому поточну інформацію про авто.
        /// </summary>
        public void AttachDisplay(DisplayHandler handler)
        {
            DisplayCallback = handler;   // «підключаємо» дисплей
            ShowInfo();                  // одразу виводимо інформацію
        }

        /// <summary>Від'єднує дисплей.</summary>
        public void DetachDisplay()
        {
            DisplayCallback = null;
        }

        /// <summary>
        /// Формує текстовий рядок з усіма параметрами автомобіля.
        /// </summary>
        private string BuildInfoString()
        {
            return $"╔══════════════════════════════╗\n" +
                   $"  🚗 Інформація про автомобіль\n" +
                   $"╠══════════════════════════════╣\n" +
                   $"  Марка:       {Brand}\n" +
                   $"  Модель:      {Model}\n" +
                   $"  Рік:         {Year}\n" +
                   $"  Макс. швид.: {MaxSpeed} км/год\n" +
                   $"  Колір:       {Color}\n" +
                   $"╚══════════════════════════════╝";
        }

        /// <summary>Повертає коротке представлення об'єкта.</summary>
        public override string ToString() => $"{Brand} {Model} ({Year})";
    }
}
