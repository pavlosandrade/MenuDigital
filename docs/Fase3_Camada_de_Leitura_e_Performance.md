# Dossiê da Fase 3: Camada de Leitura (Queries) e Performance

Nesta fase, implementamos o lado de "Leitura" do padrão **CQRS**, focando em entregar o cardápio completo para o cliente final com a menor latência e maior eficiência possível.

## 1. CQRS: Separação de Read-Models (DTOs)

**O que foi feito:**
Criamos um conjunto de **Records** em `MenuCatalogDto.cs` que representam a visão "achatada" e otimizada do cardápio para o front-end. 

**Por que fizemos:**
- Diferente das Entidades de Domínio, que são ricas em lógica e comportamento, os DTOs de leitura são apenas estruturas de dados (POCOs).
- Isso nos permite "moldar" o JSON exatamente como o front-end precisa, sem expor campos desnecessários do banco de dados (como `TenantId`, `CreatedAt`, etc).

## 2. Otimização de Performance com EF Core

**O que foi feito:**
Implementamos o `GetMenuCatalogQueryHandler` utilizando duas estratégias fundamentais de performance:

1. **`.AsNoTracking()`**: Informamos ao EF Core para não rastrear essas entidades na memória. Como é uma consulta apenas de leitura, economizamos CPU e RAM significativamente.
2. **Projeção Direta (LINQ `Select`)**: Em vez de carregar objetos de domínio e depois mapear para DTOs (o que geraria múltiplas queries ou overhead), fizemos o `Select` diretamente para os DTOs.

**Benefício:**
O EF Core gera um SQL otimizado que traz apenas as colunas necessárias, resolvendo o problema de *N+1 queries* de forma nativa e elegante.

## 3. Desafio Técnico: Object Initializers vs Construtores Primários

**O que foi feito:**
Refatoramos os DTOs de registros (`records`) para usarem propriedades com `init` em vez de argumentos de construtor.

**Por que fizemos:**
O motor de tradução LINQ do Entity Framework Core tem dificuldades em mapear coleções aninhadas (como `IEnumerable<ProductDto>`) através de construtores de Records. Ao mudar para **Object Initializers** (chaves `{ }`), o EF Core consegue construir a árvore de objetos de forma muito mais estável, evitando erros de execução (Runtime 500).

## 4. Endpoint de Catálogo Público

**O que foi feito:**
Exposição do endpoint `GET /api/menus/{id}/catalog`.

**Por que fizemos:**
Este é o endpoint principal que será consumido pelo WebApp do cliente. Ele retorna o cardápio inteiro (Menu -> Categorias -> Produtos -> Adicionais) em uma única viagem ao servidor (*Single Roundtrip*), garantindo uma experiência de carregamento instantânea para o usuário final.

---

## Conclusão da Fase 3
Com a camada de leitura estabelecida, o projeto possui agora um ciclo completo de dados. O backend está pronto para suportar uma interface de usuário complexa, mantendo a integridade do domínio e a rapidez nas consultas.
