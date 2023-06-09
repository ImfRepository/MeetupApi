# Meetup API
Meetup API with CRUD operations, PostgresSQL, JWT authentication and Swagger UI. [Here](Task.pdf) is a task.

## Content
- Database
- Architecture
- Authentication
- User secrets
- Quick start
- Used
- Time spent

## Database
I'm using PostgreSQL for this app. At the beginning I decided to split meetup information into 4 tables: meetups, organizers, places, plan steps. The last one is obvious by normalization rules, but organizers and places not. I think, it's just a very likely them to be separated tables in the future, as far as the same ones they can be used in many meetups.

## Architecture
Here I'm trying to use clean architecture as [here](https://github.com/jasontaylordev/CleanArchitecture/tree/main)

## Authentication
I already worked with JWT, so just added them here. During testing, you can choose an admin token or user (just imagine u get it somewhere not here ^). User can only read. Admin can do whatever he wants to.

## User secrets
At some point, I found a little problem: where to hide connection string for tests. But, later, I found that user secrets also worked in Test project. So, if you want to test the app, just add in test project user secret with following content:
```
{
  "ConnectionStrings:PgContext": "Host={};Port={};Database={};Username={};Password={};"
}
```

## Quick start 
Now, here is a plan how to quick start:
- clone repo
- use [this](db.sql) file to restore empty db for app (also optionally for tests)
- setup user secrets for WebApi project:
```
{
  "ConnectionStrings": {
    "PgContext": "Host={};Port={};Database={};Username={};Password={};"
  },
  "JWT": {
    "Issuer": "me?",
    "Audience": "Why not",
    "Key": "Whatever u can imagine"
  }
}
```
- if u want to run unit tests, add user to Tests project.
- run WebApi or Tests

### Used
- [Microsoft tutorials](https://learn.microsoft.com/ru-ru/)
- [Metanit](https://metanit.com/)
- [Postgres tutorials](https://www.postgresqltutorial.com/)

### Time spent
Around 31 hour.