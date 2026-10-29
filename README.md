# CarWpfApp — WPF Додаток: Повзунок + Патерн Делегата

## Структура проєкту

```
CarWpfApp/
├── CarWpfApp.sln              ← Файл рішення Visual Studio
└── CarWpfApp/
    ├── CarWpfApp.csproj       ← Файл проєкту (.NET 8, WPF)
    ├── App.xaml / App.xaml.cs ← Точка входу додатку
    ├── MainWindow.xaml        ← UI: повзунок + форма авто
    ├── MainWindow.xaml.cs     ← Code-behind: логіка UI
    ├── Car.cs                 ← Модель автомобіля з делегатом
    └── Assets/
        └── car.png            ← Іконка авто (замініть своєю)
```

---

## Як запустити

### Варіант 1 — Visual Studio 2022+
1. Відкрийте `CarWpfApp.sln` у Visual Studio 2022.
2. Натисніть **F5** або кнопку ▶ **Start**.

### Варіант 2 — .NET CLI (термінал)
```bash
cd CarWpfApp
dotnet run
```
> Потрібен .NET 8 SDK і Windows (WPF працює лише на Windows).

---

## Як замінити зображення автомобіля

1. Покладіть власний файл `car.png` у папку `CarWpfApp/Assets/`.
2. У `CarWpfApp.csproj` переконайтеся, що рядок `<Resource Include="Assets\car.png" />` присутній.
3. Перезберіть проєкт — зображення буде вбудовано в EXE.

---

## Ключові концепції коду

### 1. Повзунок ↔ Розмір зображення (WPF Binding)
```xml
<!-- XAML: Width/Height прив'язані до Value повзунка -->
<Image Width="{Binding ElementName=SizeSlider, Path=Value}"
       Height="{Binding ElementName=SizeSlider, Path=Value}" />

<Slider x:Name="SizeSlider" Minimum="50" Maximum="300" Value="150"
        ValueChanged="SizeSlider_ValueChanged" />
```

### 2. Делегат у класі Car
```csharp
// Оголошення делегата
public delegate void DisplayHandler(string message);

// Поле-делегат (спочатку null — дисплей не підключено)
public DisplayHandler? DisplayCallback;

// Підключення дисплея ПІД ЧАС РОБОТИ та негайний вивід
public void AttachDisplay(DisplayHandler handler)
{
    DisplayCallback = handler;  // «підключаємо дисплей»
    ShowInfo();                  // одразу виводимо інформацію
}
```

### 3. Підключення делегата у code-behind
```csharp
// Метод форми, який є «дисплеєм»
private void OnCarDisplayInfo(string message)
{
    DisplayOutput.Text = message;
}

// При натисканні кнопки:
_currentCar = new Car(brand, model, year, speed, color);
// Дисплей підключається ПІСЛЯ створення об'єкта
_currentCar.AttachDisplay(OnCarDisplayInfo);
```

---

## Вимоги
- **Windows** 10 / 11
- **.NET 8 SDK** (або .NET 6 / 7 — змініть `TargetFramework` у .csproj)
- **Visual Studio 2022** (або VS Code з C# Dev Kit)
