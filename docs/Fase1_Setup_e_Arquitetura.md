# MenuDigital - Documentação da Fase 1: Setup e Arquitetura Base

Nesta primeira fase do projeto MenuDigital, focamos em estabelecer uma fundação robusta, escalável e segura. Como o sistema será um SaaS Multi-Tenant (onde vários restaurantes utilizam a mesma plataforma simultaneamente), cada decisão arquitetural foi tomada pensando em isolamento de dados, manutenibilidade e performance.

Abaixo estão listadas as grandes escolhas de engenharia e os "porquês" de cada uma:

---

## 1. Clean Architecture (Arquitetura Limpa) e as 4 Camadas
Dividimos nossa API em 4 camadas (projetos físicos `.csproj`) com fronteiras muito restritas (seguindo a *Dependency Rule*: as camadas de fora só podem depender das de dentro, nunca o contrário).

### 🧅 MenuDigital.Domain (O Núcleo)
* **O que é:** O coração e a razão de existir do software. Contém as regras vitais do negócio. 
* **O que entra aqui:** Nossas Entidades ricas (`Tenant`, `Menu`, `Product`), Interfaces-Contrato de comportamento (`ITenantService`), e classes utilitárias compartilhadas (`Result` e `Error`).
* **Regra de Ouro:** ZERO dependências externas. O Domain não sabe que existe Entity Framework, PostgreSQL, ou internet (HTTP). Ele é C# puro e imutável.

### 🧅 MenuDigital.Application (O Cérebro / Orquestrador)
* **O que é:** A camada que sabe **o que** o sistema tem que fazer. É a casa dos "Use Cases" (Casos de Uso).
* **O que entra aqui:** Commands e Queries (padrão CQRS com MediatR), Validações de negócio (FluentValidation), os "Handlers" (que pegam a entrada, processam regras e salvam coisas), e DTOs (Data Transfer Objects).
* **Regra de Ouro:** A Application só conhece o `Domain`. Se ela precisa buscar no banco, ela exige a interface `IMenuDigitalDbContext` (que foi definida aqui dentro), e não quer nem saber como essa interface é conectada ao banco real lá na frente.

### 🧅 MenuDigital.Infrastructure (Os Músculos Tecnológicos)
* **O que é:** A camada que lida com o "mundo real" e a tecnologia palpável. É a encarregada do trabalho sujo.
* **O que entra aqui:** Entity Framework Core, Scripts de Banco de Dados (Migrations), Mapeamento das Tabelas (Fluent API Configurations), conexão real com o PostgreSQL, e futuras integrações com serviços de terceiros (Stripe para pagamentos, AWS S3 para imagens do cardápio).
* **Regra de Ouro:** A Infraestrutura implementa os contratos da `Application` e mapeia o `Domain`. Porém, a `Application` e o `Domain` jamais podem enxergar a Infraestrutura diretamente!

### 🧅 MenuDigital.Api (A Porta / Presentation)
* **O que é:** A casca mais fina e externa do sistema. É por onde a internet entra e sai.
* **O que entra aqui:** Minimal APIs (nossas rotas `app.MapPost(...)`), Middlewares, Configuração de Injeção de Dependências (`Program.cs`), `appsettings.json`, e o nosso `GlobalExceptionHandler`.
* **Regra de Ouro:** A API é uma "ponte burra". Ela recebe um JSON HTTP, limpa e injeta no MediatR (para a `Application` lidar com o problema), pega o resultado e devolve como HTTP novamente (ex: 200 OK ou 400 Bad Request). Jamais fazemos lógica de negócio nela.

> **💡 O Maior Benefício:** Se amanhã o banco de dados falir e formos obrigados a trocar para o MongoDB, nós literalmente deletamos a pasta `Infrastructure` inteira e fazemos uma nova. O coração do seu produto (`Domain` e `Application`) fica **100% intacto e reaproveitável**!

## 2. Rich Domain (Domínio Rico) e Imutabilidade
Evitamos o anti-pattern de "Entidades Anêmicas" (classes que são apenas sacos de propriedades soltas com `get` e `set` abertos para qualquer um alterar de qualquer lugar).

* Todos os `setters` das entidades são privados (`private set`).
* A criação de entidades é feita através de métodos de fábrica (`Create(...)`), garantindo que uma entidade seja validada no momento que nasce.

## 3. CQRS e MediatR
Na camada de Application, utilizamos o padrão **CQRS** (Segregação de Responsabilidades de Comando e Consulta) junto com a biblioteca **MediatR**.

* Pegamos o fluxo de "Criação de Restaurante" e separamos os dados de entrada (`CreateTenantCommand`) de quem faz a lógica pesada de inserção (`CreateTenantCommandHandler`).
> **💡 Decisão:** Isso evita a criação dos famosos "Services gigantescos" (ex: `TenantService.cs` com 5.000 linhas e 20 injeções de dependência que ninguém consegue dar manutenção ou testar). 

## 4. Validação Inteligente (Pipeline Behavior + FluentValidation)
Implementamos um padrão avançado e DRY (Don't Repeat Yourself) de validação:

* Criamos o `ValidationBehavior`, uma classe que senta "no meio" do duto da requisição (como um pedágio).
* Quando a API recebe o pedido, esse "pedágio" automaticamente roda todas as regras definidas no `FluentValidation` (ex: "o nome do restaurante deve ter no mínimo 3 letras").
* Se falhar, a execução é interrompida imediatamente devolvendo uma lista amigável de erros. A classe de banco de dados (`Handler`) nunca precisa gastar linhas de código com `if(string.IsNullOrEmpty(nome))`, mantendo o código impecável.

## 5. Result Pattern (Adeus Exceptions de Negócio)
Em vez de usar o caro `throw new Exception("Restaurante inválido")`, implementamos a estrutura estritamente tipada do `Result<T>`.

* O uso de Exceptions (Try/Catch) para controle de fluxo lógico no .NET custa muita memória e processamento.
* Ao retornar um `Result.Failure(Error)`, deixamos o código altamente performático, previsível e forçamos o programador (em tempo de compilação) a lidar com o cenário de falha.

## 6. Persistência Multi-Tenant Avançada no PostgreSQL
Trocamos a tecnologia do SQL Server pelo **PostgreSQL** por ser moderno, excelente para microsserviços e de baixíssimo custo de infraestrutura. 

A "Mágica" do nosso Multitenant:
* Implementamos o **Global Query Filters** direto no coração do nosso banco (`MenuDigitalDbContext`). Ele injeta um `WHERE TenantId = X` invisivelmente em **todas** as consultas do banco de dados, baseado no cabeçalho HTTP `X-Tenant-ID` da requisição atual.
> **💡 Decisão:** Essa é a blindagem final de segurança. Se um desenvolvedor Júnior esquecer de filtrar um SQL, o EF Core entra em ação e filtra obrigatoriamente. É **impossível** um restaurante acabar puxando o faturamento ou cardápio de seu concorrente!

## 7. Tratamento Global de Exceções (Problem Details)
Criamos o `GlobalExceptionHandler` para blindar nossa API contra falhas não tratadas.

> **💡 Decisão:** Em APIs amadoras, quando o banco cai, uma tela html gigante com a "Stack Trace" vaza o código-fonte para o Front-end. Isso é um erro gravíssimo (Information Exposure).
Com o nosso handler, qualquer desastre interno é capiturado, silenciado no terminal do cliente e ele recebe apenas um JSON seguro e padronizado mundialmente (RFC 7807): *"Um erro inesperado ocorreu na nossa infraestrutura."*
