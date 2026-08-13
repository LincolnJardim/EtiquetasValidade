# Etiqueta Express

Sistema web para **gestão e impressão de etiquetas de validade**, desenvolvido para facilitar o controle de produtos e produções em estabelecimentos do setor alimentício.

O projeto nasceu a partir de uma necessidade observada em cozinhas profissionais e une minha experiência anterior na Gastronomia com minha formação atual em Ciência da Computação e desenvolvimento de software.

---

## 🎯 Problema que o projeto resolve

Em cozinhas profissionais, o controle de validade de produtos depende frequentemente de processos manuais, como preenchimento de etiquetas à mão.

Além de consumir tempo da equipe, esse processo pode gerar erros de preenchimento, inconsistências nas datas e dificuldades de padronização.

O **Etiqueta Express** busca simplificar esse fluxo permitindo que produtos e suas respectivas validades sejam cadastrados no sistema e utilizados para gerar etiquetas de forma padronizada.

---

## 🚀 Funcionalidades

### Implementadas

* 🔐 Autenticação de usuários com JWT
* 🔒 Proteção de endpoints utilizando autorização
* 👤 Cadastro técnico de usuários com armazenamento seguro de senha
* 📦 Cadastro e gerenciamento de produtos
* 📅 Cálculo automático da validade a partir da data de fabricação
* 🗄️ Persistência de dados utilizando Entity Framework Core e SQL Server
* 🌐 API REST desenvolvida com ASP.NET Core
* 📖 Documentação e testes dos endpoints através do Swagger
* 🔑 Suporte a Bearer Token no Swagger
* 🖥️ Interface web utilizando HTML, CSS e JavaScript
* 🔗 Integração do front-end com endpoints protegidos da API
* 📱 Interface desenvolvida com abordagem mobile-first
* 🖨️ Preparação da interface para impressão de etiquetas

### Em desenvolvimento

* Fluxo completo de produção
* Refinamentos na geração e impressão das etiquetas
* Testes com impressora térmica real
* Validações adicionais de campos e regras de negócio
* Tratamento e padronização de erros
* Testes automatizados
* Deploy da aplicação para utilização piloto

---

## 🛠️ Tecnologias

### Back-end

* C#
* .NET 10
* ASP.NET Core Web API
* Entity Framework Core
* JWT Authentication
* REST APIs

### Banco de Dados

* Microsoft SQL Server

### Front-end

* HTML5
* CSS3
* JavaScript

### Ferramentas

* Git
* GitHub
* Swagger / OpenAPI
* Visual Studio / VS Code

---

## 🏗️ Arquitetura

O back-end está organizado em camadas inspiradas nos princípios da **Clean Architecture**, buscando separar responsabilidades e facilitar a manutenção e evolução da aplicação.

```text
backend/
│
├── Etiquetas.Domain
│   └── Entidades e regras de negócio
│
├── Etiquetas.Application
│   └── Services, DTOs e interfaces
│
├── Etiquetas.Infrastructure
│   └── Entity Framework Core, DbContext e repositórios
│
└── Etiquetas.Api
    └── Controllers, autenticação e configuração da aplicação
```

### Domain

Contém as entidades e regras diretamente relacionadas ao domínio da aplicação.

Exemplos:

* Produto
* Produção
* Usuário
* Empresa

### Application

Responsável pela comunicação entre o domínio e as demais camadas.

Contém elementos como:

* Services
* DTOs
* Interfaces de repositórios
* Serviços de autenticação

### Infrastructure

Responsável pelo acesso e persistência dos dados.

Inclui:

* Entity Framework Core
* DbContext
* Repositórios
* Migrations
* SQL Server

### API

Camada responsável por expor as funcionalidades da aplicação através de endpoints REST.

Inclui:

* Controllers
* Injeção de dependência
* Autenticação JWT
* Autorização
* Swagger / OpenAPI

---

## 📅 Regra de negócio — cálculo de validade

Uma das principais regras do sistema é o cálculo automático da validade do produto.

Cada produto possui uma quantidade de dias de validade cadastrada.

Ao registrar uma produção, o sistema utiliza:

```text
Data de fabricação + Dias de validade do produto
```

### Exemplo

```text
Produto: Molho de tomate
Dias de validade: 3 dias
Data de fabricação: 10/08/2026

Data de validade calculada: 13/08/2026
```

Dessa forma, o operador não precisa calcular manualmente a data antes de gerar a etiqueta.

---

## 📸 Screenshots

> As imagens abaixo serão adicionadas conforme a evolução da interface.

### Login

<!--
![Tela de Login](docs/images/login.png)
-->

### Produtos

<!--
![Listagem de Produtos](docs/images/produtos.png)
-->

### Produção

<!--
![Tela de Produção](docs/images/producao.png)
-->

### Etiqueta

<!--
![Etiqueta Gerada](docs/images/etiqueta.png)
-->

---

## ▶️ Como executar o projeto

### Pré-requisitos

Antes de executar o projeto, certifique-se de possuir:

* .NET 10 SDK
* Microsoft SQL Server
* Git

### Clone o repositório

```bash
git clone https://github.com/LincolnJardim/EtiquetasValidade.git
```

Entre na pasta do back-end:

```bash
cd EtiquetasValidade/backend
```

Restaure as dependências:

```bash
dotnet restore
```

Configure sua conexão com o SQL Server conforme seu ambiente local.

Depois, aplique as migrations do Entity Framework Core e execute a API.

> As configurações sensíveis, como chaves utilizadas na autenticação JWT, não devem ser versionadas no repositório.

---

## 📌 Status do projeto

🚧 **Em desenvolvimento**

O projeto encontra-se atualmente na construção da primeira versão funcional, com foco na validação da solução em um ambiente real.

A API já possui sua estrutura principal, autenticação e funcionalidades relacionadas ao gerenciamento de produtos, enquanto o front-end está sendo integrado progressivamente aos endpoints protegidos.

---

## 🗺️ Roadmap

Entre as próximas etapas planejadas estão:

* [ ] Finalizar o fluxo completo de produção
* [ ] Realizar testes de impressão em impressora térmica
* [ ] Refinar o layout específico para etiquetas
* [ ] Implementar validações adicionais
* [ ] Adicionar testes automatizados
* [ ] Realizar o primeiro deploy
* [ ] Disponibilizar a aplicação para um projeto piloto
* [ ] Evoluir o front-end para Angular
* [ ] Implementar perfis e níveis de acesso mais refinados
* [ ] Avaliar evolução futura para arquitetura multiempresa / SaaS

---

## 💡 Origem do projeto

Antes de iniciar minha trajetória na área de Tecnologia, trabalhei profissionalmente com Gastronomia.

Durante essa experiência, tive contato direto com processos de controle de produção, armazenamento e identificação de alimentos.

O **Etiqueta Express** surgiu da ideia de utilizar desenvolvimento de software para resolver um problema que conheci na prática, transformando uma necessidade do setor alimentício em um projeto real de tecnologia.

---

## 👨‍💻 Autor

**Lincoln Jardim**

Estudante de Ciência da Computação com foco em desenvolvimento Back-end utilizando C# e .NET.

* GitHub: [LincolnJardim](https://github.com/LincolnJardim)
* LinkedIn: [Lincoln Alves](https://www.linkedin.com/in/lincoln-alves-245857197/)
