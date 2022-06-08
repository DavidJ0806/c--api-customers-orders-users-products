## description
api that allows crud functionality with a domain containing customers, orders, users, and products.

## pre-requisites
Required VSCode extensions:
```
C# (powered by omnisharp)
Coverage Gutters
```
packages used: 
```
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Sqlite --version 6.0
Serilog
Serilog.AspNetCore
Serilog.Sinks.Seq
```

## usage
see `https://localhost:7075/swagger/index.html` while running the project for documentation, or view the postman collection `https://www.getpostman.com/collections/ae7e5156e06c0ed7eb31` to demonstrate how you can use the api.
## testing
From the solution folder: `dotnet test -p:CollectCoverage=true -p:CoverletOutputFormat=lcov -p:CoverletOutput=./lcov.info`

`ctrl + shift + p` - Coverage gutters display coverage

## linting
To lint the project, make sure the C# extension (powered by OmniSharp) for vscode is installed.
Press `ctrl + ,` to open the Settings panel and make sure editor.formatOnType is enabled.
`ctrl + s` to save (and lint) the open file or run `dotnet format`