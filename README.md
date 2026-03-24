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

### Executar testes

```bash
dotnet test
```

---

## 📊 Cobertura de Testes

Para gerar relatório de cobertura:

```bash
dotnet test --collect:"XPlat Code Coverage"
reportgenerator -reports:**/coverage.cobertura.xml -targetdir:coveragereport
```

### 📈 Status atual

* **Line Coverage:** 82%
* **Branch Coverage:** 50%

### 🎯 Como melhorar

* Testar cenários de erro (exceptions)
* Testar validações de domínio
* Testar fluxos negativos (ex: ID inválido, produto inexistente)
* Testar soft delete

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
dotnet user-secrets set "MongoDb:ConnectionString" "mongodb://localhost:27017"
dotnet user-secrets set "MongoDb:DatabaseName" "ProductsDb"
```

Ou, utilizando MongoDB Atlas:

```bash
dotnet user-secrets set "MongoDb:ConnectionString" "mongodb+srv://<user>:<password>@cluster.mongodb.net/?retryWrites=true&w=majority"
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

Para execução via container, utilize variáveis de ambiente no `docker-compose.yml`:

```yaml
environment:
  - MongoDb__ConnectionString=mongodb://mongo:27017
  - MongoDb__DatabaseName=ProductsDb
```

Suba os serviços:

```bash
docker-compose up --build
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

## 📌 Melhorias Futuras

* Autenticação (JWT)
* Paginação avançada
* Filtros dinâmicos
* Logs estruturados (Serilog)
* CI/CD pipeline
* Versionamento de API

---

## 👨‍💻 Autor

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

---

## 💡 Considerações Finais

Este projeto não é apenas um CRUD simples — ele demonstra:

* Organização arquitetural real
* Aplicação de DDD
* Boas práticas de mercado
* Preparação para sistemas maiores

---
