### Jak odpalić 

1. trzeba zbudować obraz dockera (jeżeli już to kiedyś zrobiłeś pomiń!)

```bash
    docker build . --no-cache
```

2. jak się skończy budować trzeba go odpalić 

```bash
    docker compose up -d 
```

3. jak się skończy odpalać trzeba chwile poczekać aż aplikacja się zbuduje.
Jak już się zbuduje aplikacja będzie działać na: (jeżeli wejdziesz tam zanim się skończy budować dostaniesz EMPTY_RESPONSE)

[Aplikacja działająca na localhost](http://localhost:5000)

Dodałem też wach'a czyli zmiany coś co wykyrwa zmiany w kodzie i samo przebudowuje aplikację~


### Wykonywanie komend
jeśli chcesz wykonać jakieś polecenie z np. `dotnet-ef` musisz wejść do kontenera żeby je wykonać 

```bash
    docker exec -it <nazwa-kontenera> bash
```

żeby wyjść wciskasz `ctrl + d`