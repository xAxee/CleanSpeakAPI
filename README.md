# CleanSpeak API
### Wstęp 🚀
Ten projekt implementuje API do wykrywania wulgaryzmów w tekście, wykorzystując model uczenia maszynowego. Projekt jest zbudowany w oparciu o framework .NET Core i bibliotekę ML.NET.

### Przykład Działania 📊
Aby zobaczyć przykład działania API, odwiedź stronę: http://cleanspeak.hubertiwan.pl/

### Technologie 🛠️
- **.NET Core**: Framework aplikacji
- **ML.NET**: Biblioteka do uczenia maszynowego
- **Swagger**: Dokumentacja API

### Struktura Projektu 🗂️
- **ProfanityController.cs**: Kontroler API odpowiedzialny za wykrywanie wulgaryzmów
- **ProfanityDetector.cs**: Usługa odpowiedzialna za predykcję modelu ML
- **ProfanityData.cs**: Modele danych dla modelu ML
- **Program.cs**: Plik główny aplikacji

### Dokumentacja API 📚
#### Endpoints
- **POST /api/Profanity/check**: Sprawdza, czy podany tekst zawiera wulgaryzmy.
  - Parametry: `text` (ciąg znaków)
  - Zwraca: `Text`, `IsProfane`, `DetectedProfanities`
  
- **POST /api/Profanity/checkList**: Sprawdza listę tekstów pod kątem wulgaryzmów.
  - Parametry: `texts` (tablica ciągów znaków)
  - Zwraca: Lista obiektów z `Text`, `IsProfane`, `DetectedProfanities`

### Uruchomienie 🚀
1. Sklonuj repozytorium.
2. Zainstaluj wymagane pakiety NuGet.
3. Wypełnij pliki `Data/keywords.txt` i `Data/data.csv` swoimi danymi.
4. Uruchom aplikację za pomocą `dotnet run`.

### Dziękuję za uwagę! 😊
Jeśli masz jakieś pytania lub chcesz wesprzeć rozwój, zapraszam do kontaktu.