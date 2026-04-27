# 🧠 Memória do Projeto (MenuApp)

> Este arquivo serve como a memória de trabalho do projeto. Ele é atualizado continuamente para garantir que todo o contexto, decisões arquiteturais, tarefas concluídas e próximos passos não se percam entre as sessões.

## 📍 Status Atual
**Data da última atualização:** 27 de Abril de 2026
**Fase Atual:** Definição final da stack tecnológica e refinamento das diretrizes arquiteturais.

## ✅ O que já fizemos (Histórico de Ações)
- [x] **Leitura e Análise de Contexto:** Processados os arquivos de instrução do diretório.
- [x] **Reestruturação do `README.md`:** Documentação base do repositório consolidada com personas e ênfase em UX/UI.
- [x] **Criação desta Memória de Trabalho:** Estabelecimento deste arquivo (`Memory.md`) para não perdermos contexto.
- [x] **Definição da Stack Tecnológica:** Validamos a escolha de **Next.js + Tailwind** (WebApp Público), **Blazor** (PainelCMS Administrativo) e **C# .NET 10** (API Backend).
- [x] **Refinamento do `ApiInstructions.md`:** Atualizado com diretrizes Sênior (.NET 10).
- [x] **Scaffolding Enterprise (Estrutura Final):** Reconstruímos a estrutura de pastas seguindo padrões de DX e Clean Architecture (src/, tests/, docs/, scripts/).
- [x] **Configuração do `.editorconfig`:** Adicionado arquivo global para padronizar o estilo de código (C#, TS, JS) para todo o time.
- [x] **Solução da API e Testes:** Os projetos da API e os projetos de Testes (Unit e Integration) foram gerados e amarrados com sucesso.

## 🚧 Onde paramos
- A estrutura de pastas macro está no padrão Microsoft eShop (Enterprise).
- A API está no seu diretório `src/Api` e os testes isolados em `tests/`.
- Estamos 100% prontos para escrever código no nível do negócio.

## 🎯 Próximos Passos
- [x] Instalar as dependências de arquitetura (MediatR, FluentValidation, EF Core) nos projetos corretos.
- [x] Criar a estrutura base do Domínio (`EntityBase`, `Result<T>`).
- [x] Construir nossas primeiras **Entidades de Domínio Ricas** (Tenant, Menu) no projeto `MenuDigital.Domain`.
- [x] Construir o restante das entidades (`Category`, `Product`, `RestaurantSettings`).

## 🎯 Próximos Passos
- [x] Configurar a camada de Persistência (EF Core `DbContext`, Configurations e Global Query Filters).
- [x] Configurar a Injeção de Dependência e Iniciar a camada Application (Casos de Uso com MediatR, Ex: `CreateTenantCommand`).
- [x] Adicionar Mapeamentos Restantes no EF Core (Menu, Category, Settings).

## 🎯 Próximos Passos
- [x] Implementar a camada Application (CQRS e MediatR).
- [x] Criar nossos primeiros comandos: `CreateTenantCommand`, `Validator` e `Handler`.
- [x] Limpeza de arquivos padrão (`Class1.cs`).

## 🎯 Próximos Passos
- [x] Configurar os comportamentos transversais (Pipeline Behaviors do MediatR e FluentValidation).
- [x] Construir o `DependencyInjection.cs` da camada Application.

## 🎯 Próximos Passos
- [x] Configurar injeção de dependências do Infrastructure e do Api (`Program.cs`).
- [x] Criar nosso primeiro endpoint Minimal API (Post `/api/tenants`).

## 🎯 Próximos Passos
- [x] Subir container do PostgreSQL via Docker / Configurar credenciais.
- [x] Rodar a API e testar o endpoint Minimal API (`POST /api/tenants`).
- [x] Implementar Global Exception Handling (Tratamento global de exceções não previstas da API).

## 🚀 Fase 2: Módulos de Cardápio
- [x] Implementar os endpoints e Commands para `Menus` e `Categories`.
- [ ] Implementar os endpoints e Commands para `Products` (com relação aos addons).
