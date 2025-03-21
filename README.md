# DT191G, Webbutveckling med .NET - Projekt
## Titz N Trix
Titz N Trix (fiktivt företag) är en webbplats med företagspresentation, nyhetsfunktionalitet samt möjlighet att skapa användarkonto för att bli medlem i Titz N Trix-communityt.  
Projektet har skapats med ASP.NET MVC med Entity Framework och en Sqlite-databas och är det sista momentet i kursen Webbutveckling med .NET.  

## Funktionalitet:
* Skapa, läsa, uppdatera och ta bort nyhetsinlägg och kategorier
* Filtrering av nyheter baserat på kategori
* Inloggning, utloggning och skapa konto (ASP.NET Identity)
* Rollbaserat innehåll och åtkomst baserat på användarroll (admin, editor och medlem)
* Responsiv design anpassad till små och stora skärmar

### Användarkonton
* **Admin:** Full behörighet för att se och hantera samtliga nyheter och kategorier
* **Editor:** Begränsad behörighet, kan se och hantera samtliga nyheter och kategorier med undantag för borttagning av kategori
* **Medlem:** Tillgång till medlem-sida  

## Sidor
* **Hem:** Startsida med företagspresentation (publik)
* **Nyheter:** Nyhetssida (publik)
* **Kategorier:** Hantering av kategorier (skyddad)
* **Medlem:** Medlem-sida (skyddad)

Innehåll på sidor och länkar i huvudmenyn visas eller döljs baserat på om en användare är inloggad eller inte samt vilken roll användaren har.

## Verktyg och teknologier
* **ASP.NET Core MVC:** Backend
* **Entity Framework Core & Sqlite:** Databas
* **ASP.NET Identity:** Autentisering
* **Razor Views & Bootstrap:** Frontend
* **Git & Github:** Versionshantering

## Skapad:
**Av:** Ronja Norlén  
**Kurs:** Webbutveckling med .NET  
**Program:** Webbutveckling  
**Skola:** Mittuniversitetet 