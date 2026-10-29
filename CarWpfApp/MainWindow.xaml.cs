using System;
using System.Windows;
using System.Windows.Controls;

namespace CarWpfApp
{
    /// <summary>
    /// Code-behind для MainWindow.xaml.
    ///
    /// Містить дві незалежні частини:
    ///   1. Обробка повзунка → динамічна зміна розміру зображення.
    ///   2. Форма для створення об'єкта Car з підключенням делегата-дисплея.
    /// </summary>
    public partial class MainWindow : Window
    {
        // ─── Поле для поточного об'єкта автомобіля ───────────────────────────
        private Car? _currentCar;

        // ─── Конструктор ─────────────────────────────────────────────────────
        public MainWindow()
        {
            InitializeComponent();
        }

        // ════════════════════════════════════════════════════════════════════
        //  ЧАСТИНА 1 — ПОВЗУНОК (динамічна зміна розміру зображення)
        // ════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Викликається щоразу, коли значення повзунка змінюється.
        /// Оновлює підпис із поточним розміром.
        /// Сам розмір зображення змінюється автоматично через Binding у XAML
        /// (Width/Height Image прив'язані до SizeSlider.Value).
        /// </summary>
        private void SizeSlider_ValueChanged(object sender,
                                             RoutedPropertyChangedEventArgs<double> e)
        {
            // Захист від виклику до завершення InitializeComponent
            if (SizeLabel == null) return;

            int size = (int)e.NewValue;
            SizeLabel.Text = $"Розмір: {size} × {size} px";
        }

        // ════════════════════════════════════════════════════════════════════
        //  ЧАСТИНА 2 — ФОРМА АВТОМОБІЛЯ + ДЕЛЕГАТ
        // ════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Метод-обробник, який буде призначений як делегат DisplayCallback.
        /// Саме він «є дисплеєм» — отримує рядок від Car і виводить у TextBox.
        /// </summary>
        /// <param name="message">Текст, сформований класом Car</param>
        private void OnCarDisplayInfo(string message)
        {
            // Виводимо повідомлення в TextBox на формі
            DisplayOutput.Text = message;

            // Прокручуємо до початку, щоб все було видно
            DisplayOutput.ScrollToHome();
        }

        /// <summary>
        /// Обробник натискання кнопки «Створити автомобіль».
        /// 
        /// Алгоритм:
        ///   1. Зчитати та перевірити дані з полів введення.
        ///   2. Створити об'єкт Car (без дисплея).
        ///   3. Якщо чекбокс «Дисплей» активний — підключити делегат і вивести інфо.
        ///   4. Якщо чекбокс не активний — повідомити, що дисплей відсутній.
        /// </summary>
        private void CreateCarButton_Click(object sender, RoutedEventArgs e)
        {
            // Скидаємо попередній статус
            StatusLabel.Text = "";
            DisplayOutput.Text = "";

            // ── Крок 1: Валідація введених даних ──────────────────────────
            string brand = BrandBox.Text.Trim();
            string model = ModelBox.Text.Trim();
            string color = ColorBox.Text.Trim();

            if (string.IsNullOrEmpty(brand) ||
                string.IsNullOrEmpty(model) ||
                string.IsNullOrEmpty(color))
            {
                ShowError("Будь ласка, заповніть усі текстові поля.");
                return;
            }

            if (!int.TryParse(YearBox.Text.Trim(), out int year) ||
                year < 1886 || year > DateTime.Now.Year + 2)
            {
                ShowError($"Невірний рік випуску. Введіть число від 1886 до {DateTime.Now.Year + 2}.");
                return;
            }

            if (!double.TryParse(SpeedBox.Text.Trim(), out double speed) || speed <= 0)
            {
                ShowError("Невірна швидкість. Введіть позитивне число.");
                return;
            }

            // ── Крок 2: Створення об'єкта Car (дисплей ще НЕ підключено) ─
            _currentCar = new Car(brand, model, year, speed, color);

            bool displayEnabled = DisplayCheckBox.IsChecked == true;

            if (displayEnabled)
            {
                // ── Крок 3а: Підключаємо делегат до методу OnCarDisplayInfo ──
                // Це і є «підключення дисплея пізніше» — після створення об'єкта.
                // AttachDisplay призначає DisplayCallback і одразу викликає ShowInfo().
                try
                {
                    _currentCar.AttachDisplay(OnCarDisplayInfo);

                    StatusLabel.Foreground =
                        new System.Windows.Media.SolidColorBrush(
                            System.Windows.Media.Color.FromRgb(166, 227, 161)); // зелений
                    StatusLabel.Text = $"✅ Автомобіль «{_currentCar}» створено. Дисплей підключено.";
                }
                catch (Exception ex)
                {
                    ShowError($"Помилка під час виводу: {ex.Message}");
                }
            }
            else
            {
                // ── Крок 3б: Дисплей НЕ підключений (делегат = null) ─────
                DisplayOutput.Text = "(Дисплей не підключено — інформація не виводиться)";
                StatusLabel.Foreground =
                    new System.Windows.Media.SolidColorBrush(
                        System.Windows.Media.Color.FromRgb(243, 139, 168)); // червоний
                StatusLabel.Text = $"⚠️ Автомобіль «{_currentCar}» створено, але дисплей вимкнено.";
            }
        }

        /// <summary>Відображає повідомлення про помилку у StatusLabel.</summary>
        private void ShowError(string message)
        {
            StatusLabel.Foreground =
                new System.Windows.Media.SolidColorBrush(
                    System.Windows.Media.Color.FromRgb(243, 139, 168));
            StatusLabel.Text = "❌ " + message;
        }
    }
}
