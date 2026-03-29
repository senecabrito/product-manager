# 📦 Product Manager API

Este projeto é uma API de gerenciamento de produtos desenvolvida para aplicar conceitos de **Clean Architecture**, e as melhores práticas do ecossistema **.NET 10**. 

O foco principal foi construir um sistema onde a lógica de negócio é protegida e a infraestrutura é independente, facilitando a manutenção e a escalabilidade.

---

## 🏗️ Arquitetura do Projeto

A solução foi dividida em camadas para garantir a separação de responsabilidades:

* **ProductManager.API**: A camada de interface. Contém os *Controllers*, e a configuração do *Swagger*.
* **ProductManager.Business**: Onde residem as regras de negócio. Contém os *Services*, as interfaces de abstração e os *DTOs* (Data Transfer Objects) imutáveis.
* **ProductManager.Data**: Camada de infraestrutura e persistência. Além do `DbContext` e das *Migrations*, esta camada contém os **Models** (entidades que representam as tabelas do banco) e a implementação dos repositórios.

---

## 🛠️ Tecnologias e Bibliotecas

* **C# & .NET**: Framework principal para o desenvolvimento.
* **Entity Framework Core**: ORM para comunicação com o banco de dados.
* **SQL Server**: Banco de dados relacional para persistência de dados.
* **Mapster**: Biblioteca de alto desempenho para mapeamento entre Entidades e DTOs.
* **Swagger/OpenAPI**: Documentação interativa para testes de endpoints.

---

## 🚦 Como Executar o Projeto

### 1. Pré-requisitos
* [.NET SDK](https://dotnet.microsoft.com/download) instalado.
* Instância do SQL Server ativa.

### 2. Configuração do Banco de Dados
No arquivo `appsettings.json` (em `ProductManager.API`), configure sua string de conexão:
```json
"MSSQLServerSQLConnectionString": "Server=localhost;Database=ProductManagerDb;Trusted_Connection=True;TrustServerCertificate=True;"
```

### 3. Aplicando as Tabelas
Abra o terminal na pasta raiz da solução e execute o comando das Migrations:
```bash
dotnet ef database update --project ProductManager.Data --startup-project ProductManager.API
```

### 4. Rodando a API
```bash
dotnet run --project ProductManager.API
```

Acesse a documentação interativa em: https://localhost:PORTA/swagger

### 🧠 Conceitos Implementados

* Encapsulamento nos Models: Validações básicas nas entidades para garantir a integridade dos dados antes da persistência.
* DTOs Imutáveis: Uso de records para garantir que os dados não sejam alterados acidentalmente entre as camadas.
* Conventional Commits: Histórico de mensagens do Git organizado por tipos (feat, chore, docs).