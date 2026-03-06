Projeto: Sistema de Etiquetas de Validade
Objetivo do Projeto
Desenvolver um software capaz de cadastrar produtos e imprimir etiquetas de validade em impressoras
térmicas. O sistema calcula automaticamente a data de validade com base na data de fabricação e na
validade padrão cadastrada para cada produto. O projeto também servirá como portfólio profissional de
desenvolvimento backend.
Tecnologias Utilizadas
• Backend: C#
• Framework: .NET / ASP.NET Core
• ORM: Entity Framework Core
• Banco de dados: SQL Server
• Arquitetura: Clean Architecture
• API REST para comunicação com frontend
• Frontend planejado: Angular
• Ferramentas: Visual Studio Code, Visual Studio, CLI do .NET
Arquitetura do Projeto
O projeto foi estruturado utilizando princípios de Clean Architecture, separando responsabilidades em
diferentes camadas.
• Etiquetas.Domain → contém as entidades principais do sistema
• Etiquetas.Application → conterá regras de negócio e serviços
• Etiquetas.Infrastructure → acesso ao banco de dados e Entity Framework
• Etiquetas.Api → camada de entrada da aplicação (API REST)
Modelagem das Entidades
• Empresa → representa o cliente que utiliza o sistema.
• Usuario → usuário que pertence a uma empresa.
• Produto → produto cadastrado contendo nome e dias de validade.
• Producao → representa a fabricação de um produto e gera as etiquetas.
Regra de Negócio Principal
A regra central do sistema é calcular automaticamente a data de validade de um produto a partir da data
de fabricação e dos dias de validade cadastrados no produto.
Exemplo lógico:
• Selecionar produto cadastrado (ex: Parmegiana de Carne)
• Obter dias de validade cadastrados (ex: 3 dias)
• Informar data de fabricação
• Calcular data de validade automaticamente
• Gerar etiquetas com essas informações
Configuração do Banco de Dados
• Instalação do Entity Framework Core
• Criação da classe DbContext
• Registro do DbContext no Program.cs da API
• Configuração da connection string no appsettings.json
• Criação da migration inicial usando CLI do .NET
• Execução do comando para criar o banco automaticamente
Comandos Importantes Utilizados
• dotnet ef migrations add InitialCreate
• dotnet ef database update --project Etiquetas.Infrastructure --startup-project Etiquetas.Api
Resultado Obtido
• Banco de dados 'EtiquetasDB' criado automaticamente
• Tabelas criadas: Empresas, Usuarios, Produtos, Producoes
• Relacionamentos entre tabelas definidos via Entity Framework
• Estrutura inicial do backend funcional
Próximos Passos do Projeto
• Criar Repositories para acesso ao banco
• Criar Services contendo regras de negócio
• Criar Controllers da API
• Testar endpoints usando Swagger
• Desenvolver frontend em Angular
• Implementar geração e impressão de etiquetas
