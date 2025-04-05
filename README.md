# 🎉 Sistema de Gestão de Festas

Este é um projeto em .NET 8 que implementa um sistema de gestão de festas usando **Minimal API**, **Entity Framework Core (SQLite)**.
O foco foi mais no backend, mas futuramente pretendo adicionar um frontend também.

## ✨ Funcionalidades

- Cadastro de **eventos**, incluindo contratante, data, preço e serviços prestados.
- Cadastro de **funcionários**.
- Associação de funcionários a eventos.
- Visualização de funcionários associados a eventos e vice-versa.
- Remoção de associação entre funcionário e evento.
- Integração com **Swagger** para testes da API.

## 💻 Tecnologias Utilizadas

- [.NET 8](https://dotnet.microsoft.com/)
- ASP.NET Core Minimal API
- Entity Framework Core
- SQLite
- Swagger

## 🗂️ Pacotes Necessários do Projeto

Ao criar o arquico EventContext.cs é preciso instalar e adicionar pacotes:
  > "dotnet tool install --global dotnet-ef --version 8.*"

E dentro da pasta do projeto é precio adicionar os pacotes:
  > "dotnet add package Microsoft.EntityFrameworkCore --version 8.*"
  > "dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.*"
  > "dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 8.*"

## 🧪 Como Rodar o Projeto

- Clone o repositório
- Execute os comandos:
  > dotnet restore
  > dotnet ef database update
  > dotnet run
- Teste via Swagger

## 🛠️ Melhorias Futuras

- Tela para cadastro e edição de eventos via Razor;
- Validação de campos;
- Sistema de autenticação;
- Deploy em nuvem;

