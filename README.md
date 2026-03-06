# 🏷️ Sistema de Etiquetas de Validade

> **Sistema automatizado para cadastro de produtos e emissão de etiquetas de validade em impressoras térmicas.**

Um projeto full-stack focado na otimização de processos do setor alimentício. O sistema calcula automaticamente as datas de validade com base na data de fabricação e nas regras de cada produto, garantindo segurança sanitária e agilidade na cozinha. 

Desenvolvido como parte do meu portfólio profissional de desenvolvimento backend, o projeto adota as melhores práticas de **Clean Architecture** e desenvolvimento em **C# / .NET**.

---

## 🎯 Objetivo do Projeto

Desenvolver um software capaz de:
- Cadastrar e gerenciar produtos e suas respectivas validades padrão.
- Calcular automaticamente a data de vencimento a partir da data de fabricação.
- Integrar-se com impressoras térmicas para emissão ágil de etiquetas.
- Servir como uma base sólida e escalável de backend, com uma API REST pronta para consumo pelo frontend.

---

## 🛠️ Tecnologias Utilizadas

**Backend & Arquitetura**
- **Linguagem:** C#
- **Framework:** .NET / ASP.NET Core
- **Arquitetura:** Clean Architecture
- **API:** RESTful API

**Banco de Dados & ORM**
- **Banco de Dados:** SQL Server
- **ORM:** Entity Framework (EF) Core

**Frontend (Planejado)**
- **Framework:** Angular

**Ferramentas de Desenvolvimento**
- Visual Studio / Visual Studio Code
- CLI do .NET
- Swagger (Testes de API)

---

## 🏗️ Arquitetura do Projeto

O sistema foi desenhado sob os princípios da **Clean Architecture**, garantindo baixo acoplamento e alta coesão através da separação clara de responsabilidades:



- `Etiquetas.Domain`: Coração do sistema, contendo as entidades principais e regras de negócio puras.
- `Etiquetas.Application`: Orquestração do sistema, contendo serviços e casos de uso (Use Cases).
- `Etiquetas.Infrastructure`: Camada de detalhes técnicos, responsável pelo acesso ao banco de dados (EF Core) e integrações externas.
- `Etiquetas.Api`: Camada de apresentação e entrada da aplicação, expondo os endpoints via API REST.

---

## 📦 Modelagem de Entidades

A estrutura de dados foi pensada para atender múltiplos clientes (SaaS) com rastreabilidade:

- **Empresa:** Representa o cliente que contrata e utiliza o sistema.
- **Usuario:** Funcionário vinculado a uma `Empresa` específica, responsável pelas operações.
- **Produto:** Item cadastrado com seu respectivo tempo de vida útil (dias de validade).
- **Producao:** Registro de um lote fabricado, que gera as etiquetas finais com as datas calculadas.

---

## 🧠 Regra de Negócio Principal

O core lógico do sistema reside no cálculo seguro da validade sanitária. 

**Exemplo Prático:**
1. O usuário seleciona um produto cadastrado *(ex: Parmegiana de Carne)*.
2. O sistema recupera a validade padrão *(ex: 3 dias)*.
3. O usuário informa a data e hora de fabricação.
4. O backend **calcula automaticamente** a data e hora exatas de descarte.
5. O sistema gera a matriz de dados para a impressão da etiqueta térmica.

---

## ⚙️ Configuração e Execução

### Pré-requisitos
- SDK do .NET 8 (ou superior)
- SQL Server rodando localmente ou em container Docker

### Passos para rodar a API

1. Clone o repositório:
   ```bash
   git clone [https://github.com/SeuUsuario/sistema-etiquetas.git](https://github.com/SeuUsuario/sistema-etiquetas.git)
