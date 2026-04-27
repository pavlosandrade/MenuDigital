# 🍔 API - Menu Digital

## 📌 Visão Geral

Esta API é responsável por gerenciar e disponibilizar os dados de cardápios digitais para restaurantes, food trucks e estabelecimentos similares.

Ela será consumida por:

* Uma aplicação web pública (Next.js + Tailwind) para exibição do cardápio com foco em UX, Performance e SEO.
* Um painel administrativo (Blazor .NET 10) para gestão de conteúdo B2B.

A API será construída em **C# .NET 10**, seguindo o padrão REST e Arquitetura Limpa (Clean Architecture), sendo projetada nativamente para suportar múltiplos estabelecimentos (Multi-Tenant).

---

## 🎯 Objetivo da API

* Fornecer dados de cardápio de forma performática e escalável
* Permitir gestão completa via painel administrativo
* Suportar múltiplos estabelecimentos (SaaS)
* Garantir isolamento de dados entre restaurantes
* Ser preparada para evolução futura (pedidos online, pagamentos, etc.)

---

## 👥 Usuários do Sistema

### Cliente Final

* Acessa o cardápio público
* Navega por categorias e produtos
* Visualiza informações como preço, descrição e imagem

### Administrador do Estabelecimento

* Gerencia menus
* Gerencia categorias
* Gerencia produtos
* Controla disponibilidade
* Define horários de exibição

---

## 🧱 Entidades Principais

### Tenant (Restaurante)

Representa um estabelecimento dentro da plataforma.

* Id
* Name
* Slug (identificador público)
* Subdomain (uso futuro)
* IsActive

---

### RestaurantSettings (Configurações do Restaurante)

Define a identidade visual e informações públicas do restaurante, utilizadas para montar a página tipo "linktree".

* Id
* TenantId
* Theme
* ContactInfo
* SocialLinks
* CustomLinks
* IsActive

#### Theme

* PrimaryColor
* SecondaryColor
* LogoUrl
* BannerUrl

#### ContactInfo

* Phone
* WhatsApp
* Address
* GoogleMapsUrl

#### SocialLinks

* InstagramUrl
* FacebookUrl
* WebsiteUrl

#### CustomLinks

Lista de links adicionais personalizados pelo restaurante:

* Title
* Url
* DisplayOrder

---

### Menu (Cardápio)

Representa um conjunto de categorias e produtos exibidos em um determinado período (ex: almoço, jantar).

* Id
* TenantId
* Name (ex: Almoço, Jantar, Happy Hour)
* StartTime
* EndTime
* IsActive

---

### Category (Categoria)

Agrupa produtos dentro de um menu.

* Id
* TenantId
* MenuId
* Name
* Description
* DisplayOrder
* IsActive

---

### Product (Produto)

Item exibido no cardápio.

* Id
* TenantId
* CategoryId
* Name
* Description
* Price
* ImageUrl
* IsAvailable
* DisplayOrder
* HasAddons

---

### ProductAddon (Adicional / Sub-produto)

Representa itens adicionais vinculados a um produto.

* Id
* TenantId
* ProductId
* Name
* Description
* Price
* IsRequired
* MaxSelection
* DisplayOrder
* IsAvailable

---

## 🔗 Relacionamentos

* Um **Tenant** possui um **RestaurantSettings**

* Um **Tenant** possui vários **Menus**

* Um **Menu** possui várias **Categories**

* Uma **Category** possui vários **Products**

* Um **Product** pode possuir vários **ProductAddons**

* Todos os dados são rigorosamente isolados por **TenantId** utilizando filtros globais no banco de dados.

---

## 🌐 Estratégia de Multi-Tenant

A aplicação utiliza **slug como identificador principal do tenant**, com suporte futuro para subdomínio.

### 🔹 Formato atual (MVP)

```
/r/{slug}
```

### 🔹 Suporte futuro

```
{slug}.app.com
```

### 🔑 Regra Arquitetural

A API **não deve depender diretamente da URL** para lógica de negócio.

A identificação do tenant deve ser abstraída por um resolver.

---

## 📦 Funcionalidades

### Página Pública (Linktree do Restaurante)

* Exibir identidade visual (cores, logo, banner)
* Exibir informações de contato
* Exibir redes sociais
* Exibir links personalizados
* Listar menus disponíveis

---

### Cardápio Público

* Obter menu ativo baseado no horário atual
* Listar categorias do menu
* Listar produtos por categoria
* Exibir cardápio completo

---

### Painel Administrativo

* Configurar identidade visual (cores, logo, banner)
* Gerenciar contatos e redes sociais
* Gerenciar links personalizados
* CRUD de menus
* Definição de horários
* CRUD de categorias
* CRUD de produtos
* Gerenciar adicionais
* Ordenação de exibição
* Controle de disponibilidade

---

## 🌐 Consumo pelo Frontend (Next.js)

A aplicação pública consumirá endpoints otimizados para leitura.

### Exemplo de endpoints

```
GET /api/public/home/{slug}
```

Retorna:

* Configurações do restaurante
* Links e redes sociais
* Lista de menus disponíveis

---

```
GET /api/public/menu/{slug}
```

Retorna:

* Menu ativo (baseado no horário)
* Categorias
* Produtos agrupados

---

## 🔐 Autenticação e Segurança

### Público

* Endpoints de leitura sem autenticação

### Administrativo

* Autenticação obrigatória
* Escopo por tenant
* Proteção contra acesso cruzado entre tenants

---

## 🚀 Regras de Negócio

* Apenas menus ativos e dentro do horário devem ser exibidos
* Um produto só pode ser exibido se `IsAvailable = true`
* Categorias e produtos devem respeitar `DisplayOrder`
* Todos os dados devem ser filtrados por tenant
* Um tenant inativo não deve ter seu cardápio exibido

---

## ⚠️ Considerações Técnicas

### Performance

* Priorizar leitura rápida (cardápio é altamente acessado)
* Preparar para uso de cache (ex: Redis)
* Evitar overfetching

### Escalabilidade

* Estrutura preparada para SaaS
* Separação clara de responsabilidades

### Evolução futura

* Pedidos online
* Integração com pagamentos
* QR Code por mesa
* Analytics de acesso
* White-label com subdomínio

---

## 🧠 Diretrizes de Arquitetura & Engenharia (.NET 10)

* **Clean Architecture:** Camadas estritas (Domain, Application, Infrastructure, Presentation/API). A camada de Domínio não deve ter dependências externas.
* **CQRS (Command Query Responsibility Segregation):** Separar os fluxos de leitura (Queries) e escrita (Commands), preferencialmente utilizando `MediatR` para baixo acoplamento.
* **Validações Limpas:** Utilizar `FluentValidation` na camada de Application. O Domínio também deve ser rico e auto-validável, proibindo estados inválidos.
* **Result Pattern:** Abolir o uso de Exceptions para controle de fluxo de negócio. Utilizar um padrão de Resultado (`Result<T>`) para transitar sucessos ou erros de domínio.
* **Isolamento Multi-Tenant Blindado:** Implementar a separação por inquilino a nível de banco de dados (ex: `Global Query Filters` no EF Core). O `TenantId` deve ser injetado via escopo da requisição (Header, Claim do Token ou interceptador de rota) de forma invisível para o desenvolvedor final.
* **Controllers Magros / Minimal APIs:** A camada de API atua apenas como adaptador. Deve receber a requisição, despachar para o Application (MediatR) e retornar o status HTTP correspondente. Sem lógica de negócio em endpoints.
* **Performance e Escalabilidade:** Queries do portal público (Next.js) devem ser altamente eficientes (ex: usar `.AsNoTracking()`, queries projetadas com DTOs e Cache distribuído) para aguentar picos de acesso de clientes finais.
