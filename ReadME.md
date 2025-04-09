# Criando um Projeto de Teste de Integração

## Tecnologias Utilizadas

Este projeto utiliza as seguintes tecnologias e ferramentas:

- **Banco de Dados**: SQL Server (ou outro banco relacional de sua escolha), configurado com Entity Framework Core.
- **Framework Web**: ASP.NET Core para criação de APIs RESTful.
- **Arquitetura**: Segue o padrão de camadas, com separação de responsabilidades em Controladores, Serviços, Repositórios e Domínio.
- **Injeção de Dependência**: Configurada nativamente com o ASP.NET Core.
- **Swagger**: Para documentação e teste da API.
- **Testes**: Utiliza o framework xUnit para testes unitários e de integração.
- **ORM**: Entity Framework Core para manipulação do banco de dados.
- **AutoBogus**: Uma biblioteca para geração automática de dados fictícios (fakes) em testes, facilitando a criação de objetos complexos com valores preenchidos automaticamente.
- **Dapper**: Um micro ORM leve e de alto desempenho para consultas SQL diretas, ideal para cenários onde o Entity Framework pode ser excessivo.
- **Testcontainers.Mssql**: Uma biblioteca para criação e gerenciamento de containers Docker para SQL Server em testes de integração, permitindo configurar bancos de dados isolados para cada execução de teste.
- **Outras Ferramentas**: 
  - `Microsoft.EntityFrameworkCore.SqlServer` (ou outro provedor de banco de dados) para conexão com o banco.
  - `Swashbuckle.AspNetCore` para integração com Swagger.
  - `NSubstitute` para criação de mocks em testes.
  - `FluentAssertions` para facilitar as asserções nos testes.
---

## Preparando o Ambiente

1. **Criando o Projeto Principal**  
    - Crie um novo projeto do tipo **Web API REST**. Este será o projeto principal que servirá como base para a aplicação.

2. **Preparando o Banco de Dados**  
    - Configure o banco de dados no projeto de comunicação, utilizando o Entity Framework ou outra tecnologia de sua escolha.

3. **Configurando o Ambiente de Execução**  
    - Utilize ferramentas como Docker para criar e gerenciar o ambiente de execução do banco de dados:
      - Configure o Docker CLI para rodar containers em conjunto com o WSL 2 (Windows Subsystem for Linux).
      - Escolha uma distribuição Linux de sua preferência (ex.: Ubuntu) e configure-a como backend do Docker:
        ```bash
        wsl --set-default-version 2
        wsl --set-version <nome-da-distribuicao> 2
        ```
    - Para facilitar o gerenciamento dos containers, utilize ferramentas como o **Rancher Desktop**:
      - Visualize e gerencie os containers de forma gráfica.
      - Inicie, pare ou remova containers conforme necessário.
      
---

## Criando o Projeto de Teste

1. **Adicionando o Projeto de Teste**  
    - Na solução existente, adicione um novo projeto do tipo **xUnit Test Project**.
    - Nomeie o projeto como [IntegrationTestPoc.Tests](http://_vscodecontentref_/1) (ou outro nome que faça sentido para sua aplicação).

2. **Configurando Dependências**  
    - Adicione referências ao projeto principal e aos projetos de biblioteca.
    - Instale os seguintes pacotes necessários para configurar o ambiente de testes. Certifique-se de utilizar as versões mais atualizadas disponíveis no momento:

      1. **`AutoBogus` (v2.13.1)**  
         - Biblioteca para geração automática de dados fictícios (fakes) em testes, facilitando a criação de objetos complexos com valores preenchidos automaticamente. (opcional)

      2. **`coverlet.collector` (v6.0.0)**  
         - Ferramenta para coleta de cobertura de código durante a execução dos testes.

      3. **`Dapper` (v2.1.35)**  
         - Micro ORM leve e de alto desempenho para consultas SQL diretas, ideal para cenários onde o Entity Framework pode ser excessivo.

      4. **`FluentAssertions` (v6.12.0)**  
         - Biblioteca que facilita a escrita de asserções em testes, tornando-as mais legíveis e expressivas.

      5. **`Microsoft.AspNetCore.Mvc.Testing` (v8.0.6)**  
         - Ferramenta que simplifica a criação de testes de integração para aplicações ASP.NET Core.

      6. **`Microsoft.NET.Test.Sdk` (v17.8.0)**  
         - SDK necessário para executar testes no ambiente .NET.

      7. **`NSubstitute` (v5.1.0)**  
         - Biblioteca para criação de mocks e substituições em testes unitários.

      8. **`System.Net.Http` (v4.3.4)**  
         - Biblioteca para realizar chamadas HTTP em testes de integração.

      9. **`System.Text.RegularExpressions` (v4.3.1)**  
         - Biblioteca para trabalhar com expressões regulares em testes.

      10. **`Testcontainers.MsSql` (v3.9.0)**  
          - Biblioteca para criação e gerenciamento de containers Docker para SQL Server em testes de integração.

      11. **`xunit` (v2.5.3)**  
          - Framework de testes unitários amplamente utilizado no ecossistema .NET.

      12. **`xunit.runner.visualstudio` (v2.5.3)**  
          - Ferramenta que permite executar testes xUnit diretamente no Visual Studio ou via linha de comando.

    - Certifique-se de adicionar os pacotes ao arquivo `.csproj` do projeto de teste, ou instale-os via NuGet Package Manager no Visual Studio ou com o comando `dotnet add package` no terminal.


    ### Implementando a Classe `TestStarterHelper`

    A classe `TestStarterHelper` é uma ferramenta auxiliar para facilitar a configuração e execução de testes de integração. Ela gerencia o ciclo de vida do banco de dados, permitindo criar, popular e limpar tabelas de forma eficiente. Siga os passos abaixo para implementá-la:

    1. **Adicione a Classe ao Projeto de Teste**  
        - Crie uma pasta chamada `Helper` no projeto de teste.
        - Adicione um novo arquivo chamado `TestStarterHelper.cs` e copie o código fornecido no arquivo `#file:TestStarterHelper.cs`.

    2. **Configuração do Banco de Dados**  
        - Certifique-se de que o projeto de teste possui uma configuração válida para o banco de dados. A classe utiliza o `IAppConfiguration` para obter a string de conexão. Implemente ou ajuste essa interface conforme necessário.

    3. **Principais Funcionalidades**  
        - **Criação do Banco de Dados**: O método `GenerateDatabase` cria o banco de dados com base no modelo definido.
        - **Manipulação de Dados**: Métodos como `SeedAsync`, `SeedRangeAsync`, `GetEntityAsync`, e `GetEntitiesAsync` permitem inserir e consultar dados no banco.
        - **Limpeza de Dados**: Métodos como `DeleteAllAsync` e `DeleteAllTablesAsync` ajudam a limpar as tabelas antes ou depois dos testes.
        - **Gerenciamento de Transações**: O método `SeedRangeAsync` suporta a inserção de dados com `IDENTITY_INSERT` habilitado, útil para cenários onde IDs precisam ser controlados manualmente.

    4. **Exemplo de Uso nos Testes**  
        - Utilize a classe em seus testes para configurar o ambiente antes de cada execução. Por exemplo:
          ```csharp
          public class ExampleTests : IClassFixture<TestStarterHelper>
          {
                private readonly TestStarterHelper _helper;

                public ExampleTests(TestStarterHelper helper)
                {
                     _helper = helper;
                }

                [Fact]
                public async Task TestDatabaseSetup()
                {
                     // Limpa as tabelas antes do teste
                     await _helper.DeleteAllTablesAsync();

                     // Insere dados de teste
                     var entity = new MyEntity { Id = 1, Name = "Test" };
                     await _helper.SeedAsync(entity);

                     // Valida os dados
                     var result = await _helper.GetEntityAsync<MyEntity>(e => e.Id == 1);
                     Assert.NotNull(result);
                     Assert.Equal("Test", result.Name);
                }
          }
          ```

    5. **Gerenciamento de Recursos**  
        - A classe implementa `IDisposable` para garantir que o contexto do banco de dados seja descartado corretamente após os testes. Certifique-se de utilizá-la em conjunto com o padrão `using` ou como um fixture no xUnit.

    Com a `TestStarterHelper`, você pode simplificar a configuração e execução de testes de integração, garantindo um ambiente limpo e consistente para cada execução.

<!-- 3. **Estruturando os Testes**  
    - Crie pastas para organizar os testes, como `Controllers`, `Services`, `Repositories`, etc.
    - Para cada camada, implemente testes cobrindo os cenários principais, como:
      - Testes de integração para os controladores.
      - Testes unitários para os serviços e repositórios. -->

<!-- 4. **Configurando o Banco de Dados para Testes**  
    - Utilize o [DbContext](http://_vscodecontentref_/3) configurado com o provedor `InMemory` para simular o banco de dados nos testes:
      ```csharp
      var options = new DbContextOptionsBuilder<Context>()
          .UseInMemoryDatabase(databaseName: "TestDatabase")
          .Options;

      using var context = new Context(options); -->
      ```

5. **Executando os Testes**  
    - Execute os testes utilizando o Test Explorer do Visual Studio ou o comando `dotnet test` no terminal.

---

## Observações Finais

Este projeto foi desenvolvido como uma PoC (Prova de Conceito) para demonstrar como implementar testes de integração em aplicações ASP.NET Core. Ele pode ser adaptado para diferentes cenários e tecnologias, servindo como base para outras aplicações.

Pronto! Agora você tem um projeto de teste configurado e integrado à sua aplicação.