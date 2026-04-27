# Dossiê da Fase 2: Backend Core - Domínio de Catálogo (Categorias, Produtos e Addons)

O objetivo principal desta fase foi estruturar o "coração" do MenuDigital: a gestão do cardápio do restaurante. Toda a arquitetura foi desenhada pensando em **Domain-Driven Design (DDD)**, **Clean Architecture** e forte **Isolamento Multi-Tenant**.

## 1. Padrão CQRS e Minimal APIs (Isolamento de Responsabilidades)

**O que foi feito:**
Criamos os Endpoints `/api/categories`, `/api/products` e `/api/products/{id}/addons` usando **Minimal APIs** puras. Todo o trabalho de orquestração e acesso a dados foi delegado ao **MediatR** através de `Commands` e `Handlers` (ex: `CreateCategoryCommand`, `CreateProductCommand`, `AddProductAddonCommand`).

**Por que fizemos:**
- Minimal APIs nos dão a melhor performance possível no ASP.NET Core 8 com alocação mínima de memória.
- O padrão **CQRS** (separação de Comandos de escrita e Consultas de leitura) garante que o nosso código de inserção não se misture com regras de listagem.
- Os *Handlers* de criação agora estão totalmente blindados e focados em regras de negócio.
- Desacoplamento extremo: Se no futuro a aplicação for migrada para gRPC, Worker Services ou GraphQL, as classes de `Commands` não sofrem nenhuma alteração.

## 2. Domain-Driven Design (DDD): Aggregate Roots (Raízes de Agregação)

**O que foi feito:**
Modelamos a entidade `Product` (Produto) como uma *Aggregate Root*. A entidade secundária `ProductAddon` (Adicional do Produto) não possui um `Handler` ou Endpoint próprio de criação avulsa. Para criar um Addon, o fluxo obrigatório passa pelo método da entidade raiz: `product.AddAddon(...)`.

**Por que fizemos:**
- **Regra de Ouro do DDD:** Objetos "filhos" (Addons) não devem existir sem estarem atrelados ao seu "dono". O Produto gerencia seu próprio ciclo de vida e de seus agregados.
- **Proteção de Invariantes:** A regra de negócio para a validação de um adicional (ex: `if (price < 0)`) vive exclusivamente dentro de `Product.cs`. Isso blinda o sistema contra estados inconsistentes.
- Nenhum desenvolvedor pode burlar essas regras, pois a coleção `_addons` é uma lista privada e o acesso externo é feito por uma `IReadOnlyCollection`.

## 3. Segurança Multi-Tenant (Proteção contra IDOR)

**O que foi feito:**
Asseguramos que o ID do Restaurante (`TenantId`) trafegue implicitamente pelo Header HTTP `X-Tenant-ID` através do `CurrentTenantService` injetado na aplicação.

**Por que fizemos:**
- A vulnerabilidade **IDOR (Insecure Direct Object Reference)** é letal em aplicações SaaS. Sem validação de Tenant forte, um Restaurante "A" poderia alterar o ID na URL para manipular categorias do Restaurante "B". 
- Ao configurarmos o `TenantId == _tenantService.GetTenantId()` no `Global Query Filter` do DbContext, o EF Core intercepta toda e qualquer query, ocultando em nível de banco de dados os registros que não pertencem àquele usuário logado.

## 4. O Grande Desafio Arquitetural do EF Core (Tratamento de Bugs)

Durante o desenvolvimento da inserção do `ProductAddon`, deparamo-nos com uma exceção de banco de dados (`DbUpdateConcurrencyException`). Este erro levou a um aprofundamento nos comportamentos ocultos do Entity Framework Core que resultou em duas correções vitais de arquitetura:

### Bug 1: O Cache do Global Query Filter (O Erro de "0 Linhas Afetadas")
- **O Erro:** Ao tentar dar `UPDATE` (por conta do `UpdateTimestamp()`), o EF Core não encontrava a linha, alegando concorrência.
- **O Motivo:** O método `OnModelCreating` do EF Core roda **apenas uma vez** no ciclo de vida do App. Ao declararmos a variável local fora do lambda (ex: `var tenantId = _tenantService.GetTenantId();`), o EF Core fez o cache estático do Tenant da primeira requisição (geralmente `Guid.Empty`).
- **A Solução:** Passamos a invocar o `_tenantService.GetTenantId()` diretamente *dentro* da expressão Lambda `e => e.TenantId == _tenantService.GetTenantId()`. Assim, o EF Core avalia a árvore de expressões dinamicamente a cada requisição, salvando o isolamento Multi-Tenant.

### Bug 2: Tracking State de Entidades com Identidade Própria (Guid.NewGuid)
- **O Erro:** Ao ser adicionado à coleção de `Product`, o EF Core tentava realizar um `UPDATE` num Addon recém-criado.
- **O Motivo:** No DDD, nossa `EntityBase` já constrói a entidade com um `Guid.NewGuid()`. O mecanismo de *Change Tracking* do EF Core assume que, se uma chave primária não for o seu valor *default* (`Guid.Empty`), a entidade **já existe no banco** e foi apenas recuperada e modificada.
- **A Solução:** No Handler, refatoramos o método do domínio para retornar a instância criada `Result<ProductAddon>` e incluímos o comando direto `_context.ProductAddons.Add(result.Value);`. Chamar `.Add()` de um `DbSet` sobrescreve o rastreio e informa explicitamente ao EF Core que o estado da entidade é `EntityState.Added`, forçando um `INSERT`.

---

## Conclusão da Fase 2
A base de Escrita (Commands) do Catálogo agora é um "cofre-forte": modular, limpa e padronizada. Não importa a origem dos dados (front-end web, mobile, jobs), o banco de dados está protegido contra dados inconsistentes e acessos indevidos. A infraestrutura provou sua maturidade ao falhar de maneira previsível sob condições anômalas, permitindo capturar os erros rapidamente no `GlobalExceptionHandler`.
