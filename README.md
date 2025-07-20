# 🧼 CleanSpeak API
CleanSpeak API to REST API do moderacji tekstu w języku polskim. Wykorzystuje model uczenia maszynowego oparty o **ML.NET** do klasyfikacji tekstu jako:
- `vulgar` – wulgarny, obraźliwy,
- `friendly` – przyjazny, pozytywny,
- `neutral` – neutralny.

## 🔧 Technologie użyte w projekcie
- **.NET 6 / ASP.NET Core** – REST API
- **ML.NET** – trenowanie i użycie modelu ML
- **Swagger** – dokumentacja interaktywna
- **C#** – język główny projektu

## 📁 Struktura katalogów
```
CleanSpeakAPI/
├── Controllers/ # Kontrolery API
├── Services/ # Logika moderacji i ML
├── Models/ # Klasy danych i predykcji
├── Utils/ # Narzędzia (np. normalizacja tekstu)
├── Data/data.csv # Dane treningowe
├── Program.cs # Główna konfiguracja aplikacji
└── README.md
```
## 🚀 Jak uruchomić projekt lokalnie

1. Sklonuj repozytorium:
   ```bash
   git clone https://github.com/xAxee/CleanSpeakAPI.git
   cd CleanSpeakAPI
   ```
2. Przygotuj dane treningowe (Data/data.csv) – przykładowy plik już znajduje się w repo.
3. Zbuduj i uruchom projekt:
```
dotnet restore
dotnet run --project CleanSpeakAPI
```
4. Wejdź do interaktywnej dokumentacji:
```
https://localhost:8080/api
```
## 📨 Endpointy API
### POST /api/TextModeration/analyze
#### Opis: Analizuje przesłany tekst i klasyfikuje go jako vulgar, friendly lub neutral.

✅ Przykładowy request:
```json
POST /api/TextModeration/analyze
Content-Type: application/json

{
  "text": "Dziękuję za Twoją pomoc!"
}
```
🔁 Przykładowa odpowiedź:
```json
{
  "category": "friendly",
  "probabilities": {
    "vulgar": 0.01,
    "friendly": 0.97,
    "neutral": 0.02
  }
}
```
🧾 Odpowiedzi HTTP:
- 200 OK – predykcja udana
- 400 Bad Request – brak tekstu wejściowego
- 500 Internal Server Error – błąd serwera

## 🧠 Jak działa model ML
- Dane wejściowe są najpierw normalizowane (zamiana na małe litery, usunięcie znaków specjalnych).
- Tekst jest zamieniany na wektory cech (FeaturizeText).
- Model jest trenowany metodą **LbfgsMaximumEntropy**.
- Wynik to kategoria oraz rozkład prawdopodobieństw.

## 📊 Dane treningowe
Plik data.csv zawiera ręcznie przygotowane przykłady dla trzech klas:
- vulgar: np. "Zamknij się", "Idź do diabła"
- friendly: np. "Miłego dnia!", "Dziękuję za pomoc"
- neutral: np. "Dzisiaj pada deszcz", "Oglądałem film wczoraj"

## 📌 TODO
- Dodanie nowych klas: spam, hejt, ironia
- Większy zbiór danych

## 🧑‍💻 Autor
**Hubert Iwan** | [github.com/xAxee](https://github.com/xAxee)

## 📜 Licencja
Projekt dostępny na licencji MIT.
