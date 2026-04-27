# 📱 MenuApp (Menu Digital)

> Uma plataforma moderna e escalável de cardápios digitais para restaurantes, bares e food trucks, desenhada com foco absoluto em Experiência do Usuário (UX/UI) e Arquitetura Limpa.

---

## 🎯 Visão Geral do Produto

O **MenuApp** oferece uma forma simples, rápida e moderna para estabelecimentos exibirem seus produtos e para clientes acessarem cardápios digitais sem a necessidade de baixar aplicativos, tudo através de um link ou QR Code.

### 👥 Perfis de Usuário

- **🏢 Estabelecimento (Admin):** Gerencia o negócio, cria menus, categorias, produtos com variações e personaliza a identidade visual da sua página. Acompanha o engajamento através de QR Codes e NPS.
- **📱 Cliente (Usuário Final):** Acessa o link público ou lê o QR Code para navegar de forma extremamente fluida e mobile-first pelo cardápio, visualizar detalhes dos produtos e acessar redes sociais do estabelecimento.

---

## 🏗️ Arquitetura e Engenharia

Este projeto adota padrões rigorosos de engenharia de software para garantir escalabilidade, manutenibilidade e a melhor experiência possível.

### 🎨 Front-end (UX/UI & Performance)
- **Design System & Hierarquia Visual:** Foco em tipografia fluida, espaçamentos consistentes (escala 4px/8px) e acessibilidade (WCAG AA/AAA).
- **Mobile-First & Performance:** Otimização agressiva de LCP, FCP e CLS, garantindo carregamento instantâneo para o cliente final.
- **Clean Architecture Front-end:** Separação clara de responsabilidades (UI components, containers, hooks, services), evitando acoplamento e re-renders desnecessários.

### ⚙️ Back-end (Escalabilidade & Segurança)
- **Domain-Driven Design (DDD):** Lógica de negócio isolada das ferramentas (Clean Architecture / Hexagonal).
- **APIs Robustas:** Endpoints RESTful/GraphQL bem documentados, validados rigorosamente via DTOs.
- **Segurança e Performance:** Proteção contra OWASP Top 10, caching estratégico, e prevenção contra gargalos como N+1 queries.

---

## 📜 Diretrizes de Contribuição e IA (Copilot/Cursor)

A inteligência de engenharia deste projeto está centralizada na pasta `..github`. Todo código gerado por IA ou por desenvolvedores deve obedecer estritamente aos seguintes manuais:

- [Contexto do Negócio (`..github/MenuDigitalContext.md`)](../..github/MenuDigitalContext.md): Entenda as entidades (Estabelecimento, Menu, Categoria, Produto, Variação) e regras de negócio.
- [Diretrizes de Front-end (`..github/FrontEndInstructions.md`)](../..github/FrontEndInstructions.md): Padrões de Clean Code, acessibilidade e UX/UI.
- [Diretrizes de Back-end (`..github/BackendInstructions.md`)](../..github/BackendInstructions.md): Arquitetura, testes, segurança e design de APIs.
- [Padrões de Commit (`..github/CommitsInstructions.md`)](../..github/CommitsInstructions.md): Uso obrigatório de **Semantic Commits**.

### 📝 Padrão de Commits

Siga o formato de Semantic Commits. O histórico do repositório deve ser limpo e descritivo:

```text
<type>(<scope>): <short description>
```
*Exemplos:* `feat(menu): add category filter`, `fix(ui): correct button contrast`, `refactor(api): extract logic to service`.

---

## 🚀 Como iniciar o projeto

*(Em breve: Instruções de setup, variáveis de ambiente e comandos de build de acordo com a stack definida)*

---

*Desenvolvido com foco em Código Limpo, Arquitetura Limpa e uma Experiência do Usuário excepcional.*