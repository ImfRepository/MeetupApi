# Meetup API
[Here](Task.pdf) is a task. There is an attempt to realize a kind of clean architecture like [here](https://github.com/jasontaylordev/CleanArchitecture/tree/main).

## How to start

1. Clone repository
2. Use [this](db.sql) file to restore empty db for app
3. Setup user secrets 
(Web API)
```
{
  "ConnectionStrings": {
    "PgContext": "Host={};Port={};Database={};Username={};Password={};"
  },
  "JWT": {
    "Issuer": "{}",
    "Audience": "{}",
    "Key": "{}"
  }
}
```

(Tests)
```
{
  "ConnectionStrings:PgContext": "Host={};Port={};Database={};Username={};Password={};"
}
```

4. Run

### Used
- [Microsoft tutorials](https://learn.microsoft.com/ru-ru/)
- [Metanit](https://metanit.com/)
- [Postgres tutorials](https://www.postgresqltutorial.com/)
- [Issue with Result(T) in mediatr behavior](https://github.com/jbogard/MediatR/issues/626)
- Some answers from StackOverflow

### Time spent
Around 31 hour. (Later spend about 30+ hours)