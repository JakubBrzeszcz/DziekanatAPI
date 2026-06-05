# System Dziekanat API

Projekt zaliczeniowy realizujący backendową część systemu do zarządzania ocenami i studentami (Dziekanat). Aplikacja została zbudowana zgodnie z zasadami Czystej Architektury (Clean Architecture) w środowisku .NET 8.

## 👥 Autorzy

- **Jakub Brzeszcz** - 15732

## 🔗 Repozytorium

Link do repozytorium GitHub: https://github.com/JakubBrzeszcz/DziekanatAPI

## ✨ Zrealizowane funkcje

Zgodnie z wymaganiami projektu zrealizowano następujące funkcjonalności:

1. **Czysta Architektura (Clean Architecture)**
   - Projekt podzielony na warstwy: `CoreApp` (logika biznesowa, interfejsy, DTO, Value Objects), `Infrastructure` (dostęp do danych, EF Core, autoryzacja) oraz `WebApi` (prezentacja, Swagger, endpointy).
2. **Entity Framework Core & SQLite**
   - Baza danych obsługiwana relacyjnie za pomocą EF Core z użyciem lekkiej bazy SQLite (`university.db`).
3. **ASP.NET Core Identity**
   - System użytkowników, ról (np. Administrator, DeaneryWorker, Lecturer) oraz polityk dostępu (Policies).
4. **Autoryzacja JWT (JSON Web Tokens)**
   - Bezpieczne logowanie z wydawaniem tokenów dostępu (Access Token) oraz mechanizmem ich odświeżania (Refresh Token).
5. **Zarządzanie Ocenami (Panel Wykładowcy - Zadanie 11)**
   - Przeglądanie listy studentów.
   - Dodawanie i edycja ocen dla studentów z automatycznym zapisem historii zmian (encja `GradeHistory`).
   - Kontrola uprawnień - tylko autoryzowani użytkownicy (np. Administrator) mogą zarządzać ocenami.
6. **Inicjowanie Danych (Data Seeder)**
   - Aplikacja automatycznie uzupełnia bazę danych testowymi użytkownikami, rolami, kursami, wykładowcami oraz studentami przy pierwszym uruchomieniu w środowisku deweloperskim.
7. **Domain-Driven Design (DDD) - Value Objects**
   - Zaimplementowano klasę `Pesel` jako _Value Object_ z automatyczną weryfikacją poprawności (długość, znaki, suma kontrolna), mapowaną bezpośrednio na kolumnę w bazie danych.

## 🚀 Uruchomienie projektu

Aby uruchomić projekt na swoim środowisku lokalnym, postępuj zgodnie z poniższymi instrukcjami:

### Wymagania wstępne

- Zainstalowany zestaw **.NET 8 SDK**
- Dowolne środowisko IDE (np. Visual Studio, JetBrains Rider, lub VS Code)

### Krok po kroku

1. Sklonuj repozytorium na swój dysk.
2. Otwórz terminal (lub wiersz poleceń) i przejdź do głównego katalogu projektu.
3. Upewnij się, że baza danych jest zaktualizowana, wykonując polecenie:
   ```bash
   dotnet ef database update --project Infrastructure --startup-project WebApi
   ```
4. Uruchom aplikację za pomocą narzędzia .NET CLI:
   ```bash
   dotnet run --project WebApi
   ```
5. Po pomyślnym uruchomieniu, otwórz przeglądarkę i przejdź pod adres wygenerowany w konsoli (domyślnie dodając `/swagger`), np.:
   - `http://localhost:5247/swagger`

### Logowanie i testowanie API

Aby uzyskać pełny dostęp do zabezpieczonych endpointów (np. w Panelu Wykładowcy), użyj poniższych danych logowania testowego Administratora w metodzie `POST /api/auth/login`:

- **Email:** `admin@wsei.edu.pl`
- **Hasło:** `Admin123!`

Otrzymany `accessToken` skopiuj i umieść w autoryzacji Swaggera (zielony przycisk **Authorize** na górze strony).

---

_Projekt wykonany w ramach zaliczenia przedmiotu._
