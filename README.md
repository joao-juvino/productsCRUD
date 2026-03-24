# 🧱 ProductsCRUD - .NET 8 + DDD + MongoDB

API RESTful para gerenciamento de produtos, desenvolvida com **.NET 8**, seguindo **Domain-Driven Design (DDD)**, boas práticas de arquitetura e foco em código limpo, testável e escalável.

---

## 🚀 Tecnologias Utilizadas

* .NET 8 (ASP.NET Core Web API)
* MongoDB
* Docker & Docker Compose
* xUnit (testes)
* FluentValidation
* Swagger (OpenAPI)
* Dependency Injection nativa
* Middleware global de exceções
* Cobertura de testes (Cobertura)

---

## 🏗️ Arquitetura

O projeto segue uma arquitetura em camadas baseada em DDD:

```
src/
├── Api              → Camada de entrada (Controllers, Middlewares)
├── Application      → Casos de uso, DTOs, validações
├── Domain           → Entidades, regras de negócio, interfaces
└── Infrastructure   → Banco de dados, repositórios, configs externas
```

### 🔹 Princípios aplicados

* Separação de responsabilidades (SRP)
* Inversão de dependência (DIP)
* Domínio rico (não anêmico)
* Encapsulamento de regras de negócio
* Baixo acoplamento entre camadas

---

## 📦 Entidade Principal

### Product

A entidade contém:

* `Id` (Guid)
* `Name`
* `Description`
* `Price`
* `Stock`
* `CreatedAt`
* `UpdatedAt`
* `IsDeleted` (soft delete)

### ✔️ Regras de domínio

* Nome não pode ser vazio
* Preço deve ser positivo
* Estoque não pode ser negativo

As validações são encapsuladas na própria entidade, lançando `DomainException`.

---

## 🧪 Testes

O projeto possui testes de integração utilizando:

* `WebApplicationFactory`
* Banco Mongo em ambiente de teste isolado

### Adicionar as variáveis de ambiente

```bash
dotnet user-secrets init
dotnet user-secrets set "MongoDb:ConnectionString" "<your-private-connection-string>"
```

### Executar testes

```bash
dotnet test
```

---

## 📊 Cobertura de Testes

Para gerar relatório de cobertura:

```bash
dotnet test --collect:"XPlat Code Coverage"
~/.dotnet/tools/reportgenerator "-reports:tests/ProductsCRUD.Tests/TestResults/**/coverage.cobertura.xml" "-targetdir:coveragereport" -reporttypes:Html
```

*Dica*: uma das melhores formas de visualizar o relatório é através do arquivo coveragereport/index.html

### 📈 Status atual

* **Line Coverage:** 82%
* **Branch Coverage:** 50%

---
## ▶️ Executando o Projeto

### 🔹 Configuração (User Secrets - recomendado para desenvolvimento)

Este projeto utiliza **User Secrets do .NET** para armazenar dados sensíveis localmente.

1. Inicialize (caso ainda não tenha feito):

```bash
dotnet user-secrets init
```

2. Configure a string de conexão:

```bash
dotnet user-secrets set "MongoDb:ConnectionString" "<your-private-connection-string>"
```

3. Verifique:

```bash
dotnet user-secrets list
```

---

### 🔹 Executando sem Docker

```bash
dotnet build
dotnet run
```

---

### 🔹 Executando com Docker

Para execução via container, utilize adicione as variáveis de ambiente em um arquivo .env:

```bash
cp .env.example .env
```

*Nota*: para preencher os valores privados, entre em contato com o time de desenvolvimento.

Suba os serviços:

```bash
docker-compose up --build -d
```

---

### 🔹 Swagger

Após rodar a aplicação:

👉 Acesse:

```
http://localhost:5279/swagger
```

---

## 📌 Endpoints Principais

| Método | Endpoint           | Descrição             |
| ------ | ------------------ | --------------------- |
| GET    | /api/products      | Listar produtos       |
| GET    | /api/products/{id} | Buscar por ID         |
| POST   | /api/products      | Criar produto         |
| PUT    | /api/products/{id} | Atualizar produto     |
| DELETE | /api/products/{id} | Remover (soft delete) |

---

## 🧠 Soft Delete

O projeto utiliza **soft delete**, ou seja:

* O registro não é removido do banco
* Apenas o campo `IsDeleted = true` é atualizado

### Vantagens

* Recuperação de dados
* Auditoria
* Segurança contra exclusões acidentais

---

## ⚠️ Tratamento de Erros

Foi implementado um middleware global:

```
GlobalExceptionMiddleware
```

Ele captura:

* Exceptions de domínio
* Erros inesperados

E retorna respostas padronizadas para o cliente.

---

## 🧾 Padrão de Commits

Este projeto segue o padrão **Conventional Commits**:

```
feat: adiciona criação de produto
fix: corrige validação de preço
refactor: melhora estrutura do service
test: adiciona testes para controller
chore: ajustes de configuração
```

---

## 📂 Estrutura de Testes

```
tests/
└── ProductsCRUD.Tests
    ├── Controllers
    ├── Infrastructure
    └── Integration Tests
```

---

## 🔧 Boas Práticas Aplicadas

* DTOs para entrada e saída
* Validação com FluentValidation
* Repositórios desacoplados via interface
* Injeção de dependência
* Separação clara de camadas
* Código preparado para escalabilidade

---

## 👨‍💻 Autor

**João Pedro Juvino dos Santos** - [Seu LinkedIn](https://www.linkedin.com/in/jo%C3%A3o-santos-b6864123b/)

Desenvolvido como parte de avaliação técnica, com foco em:

* Arquitetura limpa
* Boas práticas
* Código profissional

---

## ✅ Status do Projeto

✔️ Funcional
✔️ Testado
✔️ Documentado
✔️ Pronto para evolução
