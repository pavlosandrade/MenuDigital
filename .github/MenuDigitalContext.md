# 📱 MenuApp - Contexto do Produto

## 🧩 O que estamos construindo

O MenuApp é uma aplicação de menu digital para restaurantes, bares e food trucks.

A proposta é permitir que estabelecimentos criem e gerenciem seus cardápios digitais, que poderão ser acessados por clientes através de link ou QR Code.

---

## 🎯 Objetivo principal

Oferecer uma forma simples, rápida e moderna para:

- Estabelecimentos exibirem seus produtos
- Clientes acessarem menus digitais sem necessidade de aplicativos
- Centralizar links importantes (menu, redes sociais, contato)

---

## 👥 Perfis de usuário

### 🏢 Estabelecimento (Admin)
Responsável por:
- Configurar o negócio
- Criar e organizar menus
- Gerenciar produtos
- Compartilhar o acesso com clientes

### 📱 Cliente (Usuário final)
Responsável por:
- Acessar o link ou QR Code
- Navegar pelos menus
- Visualizar produtos e informações

---

## 🧠 Como o sistema funciona

1. O estabelecimento realiza o cadastro
2. Configura suas informações (nome, contato, redes sociais, etc.)
3. Cria menus, categorias e produtos
4. O sistema gera um link e QR Code
5. O cliente acessa esse link
6. Visualiza uma página estilo "linktree"
7. Navega até o menu e visualiza os produtos

---

## 🧱 Estrutura do Produto

### 🔐 Painel Administrativo

#### Login
- Acesso do estabelecimento

#### Configuração Geral
- Dados do estabelecimento
- Localização
- Contato
- Redes sociais
- Personalização visual (cores/tema)

#### Menu Digital
- Menus
- Categorias
- Produtos
- Variações / adicionais de produtos

#### Marketing
- QR Codes
- NPS (avaliação do cliente)

---

### 🌐 Experiência do Cliente

#### Link de Acesso (Linktree)
Página inicial contendo:
- Links para menus
- Redes sociais
- Informações de contato

#### Menu Digital
- Lista de categorias
- Lista de produtos por categoria
- Detalhes dos produtos
- Variações/adicionais (quando existirem)

---

## 📦 Entidades principais

### Estabelecimento
Representa o negócio dono do menu.

### Menu
Agrupa categorias de produtos.

### Categoria
Organiza os produtos dentro do menu.

### Produto
Item exibido para o cliente.

### Variação / Adicional
Opções extras de um produto (ex: tamanho, complementos).

---

## 🔗 Relações importantes

- Um estabelecimento pode ter múltiplos menus
- Um menu possui múltiplas categorias
- Uma categoria possui múltiplos produtos
- Um produto pode ter variações/adicionais
- Apenas itens ativos devem ser exibidos para o cliente

---

## ⚠️ Regras de negócio

- O cliente não precisa estar logado
- O acesso é feito via link público
- O conteúdo exibido deve respeitar o status (ativo/inativo)
- A navegação deve ser simples e direta
- O foco é mobile-first

---

## 🚀 Direção do produto

O sistema deve priorizar:

- Simplicidade
- Clareza na navegação
- Facilidade de gestão para o estabelecimento
- Boa experiência para o cliente final