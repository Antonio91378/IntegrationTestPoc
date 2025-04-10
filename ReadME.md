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

    ### Configurando dependências compartilhadas e metadados para os testes (Fixture, Collection e Trait no xUnit) 

    O xUnit oferece recursos avançados para organizar e compartilhar configurações entre testes, como `Fixture`, `Collection` e `Trait`. Abaixo, explicamos cada um deles:

    #### 1. **Fixture**
    Fixtures são usadas para compartilhar objetos ou configurações entre vários testes. Elas ajudam a evitar a repetição de código e garantem que os recursos sejam configurados e descartados corretamente.

    - **Exemplo de Fixture**:
        ```csharp
        public class DatabaseFixture : IDisposable
        {
                public DatabaseFixture()
                {
                        // Configuração inicial, como criar um banco de dados em memória
                }

                public void Dispose()
                {
                        // Limpeza de recursos
                }
        }
        ```

    #### 2. **Collection**
    Collections permitem agrupar testes que compartilham a mesma fixture. Isso é útil para cenários onde múltiplas classes de teste precisam acessar os mesmos recursos.

    - **Exemplo de Collection**:
        ```csharp
        [CollectionDefinition("Database collection")]
        public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
        {
                // Esta classe não precisa conter código, apenas define a coleção
        }

        [Collection("Database collection")]
        public class TestClass1
        {
                private readonly DatabaseFixture _fixture;

                public TestClass1(DatabaseFixture fixture)
                {
                        _fixture = fixture;
                }

                [Fact]
                public void Test1()
                {
                        // Teste utilizando a fixture compartilhada
                }
        }

        [Collection("Database collection")]
        public class TestClass2
        {
                private readonly DatabaseFixture _fixture;

                public TestClass2(DatabaseFixture fixture)
                {
                        _fixture = fixture;
                }

                [Fact]
                public void Test2()
                {
                        // Outro teste utilizando a mesma fixture
                }
        }
        ```

    #### 3. **Trait**
    Traits são usados para categorizar testes, permitindo filtrá-los durante a execução. Você pode utilizá-los para marcar testes com atributos como "Categoria", "Prioridade", etc.

    - **Exemplo de Trait**:
        ```csharp
        public class TraitExampleTests
        {
                [Fact]
                [Trait("Category", "Integration")]
                public void IntegrationTest()
                {
                        // Teste de integração
                }

                [Fact]
                [Trait("Category", "Unit")]
                public void UnitTest()
                {
                        // Teste unitário
                }
        }
        ```

    - **Filtrando por Trait**:
        Você pode executar testes específicos com base no `Trait` utilizando o comando:
        ```bash
        dotnet test --filter "Category=Integration"
        ```

    Esses recursos tornam o xUnit uma ferramenta poderosa para organizar e executar testes de forma eficiente, especialmente em projetos de grande escala.
5. **Executando os Testes**  
    - Execute os testes utilizando o Test Explorer do Visual Studio ou o comando `dotnet test` no terminal.

    ### Entendendo o Ciclo de Vida de Execução de Testes no xUnit

    O xUnit é projetado para ser simples e eficiente, gerenciando automaticamente o ciclo de vida de execução dos testes. Abaixo, detalhamos como ele lida com a instanciação de fixtures, collections e a execução dos testes:

    #### 1. **Instanciação de Classes de Teste**
        - Para cada teste, o xUnit cria uma nova instância da classe de teste. Isso garante que os testes sejam isolados uns dos outros, evitando efeitos colaterais causados por estados compartilhados.
        - Exemplo:
          ```csharp
          public class MyTests
          {
                private int _counter;

                [Fact]
                public void Test1()
                {
                     _counter++;
                     Assert.Equal(1, _counter); // Sempre será 1, pois a classe é recriada para cada teste
                }

                [Fact]
                public void Test2()
                {
                     _counter++;
                     Assert.Equal(1, _counter); // Também será 1
                }
          }
          ```

    #### 2. **Fixtures e o Ciclo de Vida Compartilhado**
        - Quando você utiliza uma `Fixture`, o xUnit cria uma única instância da fixture e a compartilha entre os testes que a utilizam.
        - Isso é útil para inicializar recursos caros, como conexões de banco de dados ou configurações de ambiente, que podem ser reutilizados em vários testes.
        - Exemplo:
          ```csharp
          public class SharedFixture : IDisposable
          {
                public SharedFixture()
                {
                     // Inicialização de recursos
                }

                public void Dispose()
                {
                     // Liberação de recursos
                }
          }

          public class MyTests : IClassFixture<SharedFixture>
          {
                private readonly SharedFixture _fixture;

                public MyTests(SharedFixture fixture)
                {
                     _fixture = fixture;
                }

                [Fact]
                public void Test1()
                {
                     // Usa a mesma instância de _fixture
                }

                [Fact]
                public void Test2()
                {
                     // Usa a mesma instância de _fixture
                }
          }
          ```

    #### 3. **Collections e Execução Paralela**
        - O xUnit agrupa testes em `Collections` para controlar a execução paralela. Testes na mesma coleção são executados sequencialmente, enquanto testes em coleções diferentes podem ser executados em paralelo.
        - Isso é útil para evitar conflitos em recursos compartilhados, como bancos de dados ou arquivos.
        - Exemplo:
          ```csharp
          [CollectionDefinition("Database collection")]
          public class DatabaseCollection : ICollectionFixture<SharedFixture>
          {
          }

          [Collection("Database collection")]
          public class TestClass1
          {
                // Testes que compartilham a mesma fixture
          }

          [Collection("Database collection")]
          public class TestClass2
          {
                // Testes que compartilham a mesma fixture
          }
          ```

    #### 4. **Execução dos Testes**
        - O xUnit segue uma ordem específica para executar os testes:
          1. Inicializa as fixtures e collections necessárias.
          2. Cria uma instância da classe de teste.
          3. Executa o método de teste.
          4. Descarrega a classe de teste e, se aplicável, libera os recursos das fixtures.

    #### 5. **Gerenciamento de Recursos**
        - O xUnit utiliza o padrão `IDisposable` para liberar recursos automaticamente após a execução dos testes. Isso é especialmente útil para evitar vazamentos de memória ou conexões abertas.
        - Exemplo:
          ```csharp
          public class ResourceFixture : IDisposable
          {
                public ResourceFixture()
                {
                     // Inicializa recursos
                }

                public void Dispose()
                {
                     // Libera recursos
                }
          }
          ```

    Com esse entendimento, você pode projetar testes mais eficientes e organizados, aproveitando ao máximo os recursos do xUnit.

---

## Fixture de Banco de Dados

A classe DatabaseFixture contida nesta POC implementa a interface IAsyncLifetime, que é usada pelo xUnit para gerenciar o ciclo de vida de fixtures assíncronas. Ela é responsável por configurar e limpar o ambiente de banco de dados antes e depois dos testes.
### Construtor da Classe DatabaseFixture

O construtor da classe `DatabaseFixture` realiza as seguintes operações:

1. **Carregamento de Configurações**  
    - Utiliza o `ConfigurationBuilder` para carregar os arquivos `appsettings.json` e `appsettings.Test.json`, além de variáveis de ambiente.
    - Cria uma instância de `AppConfiguration` para acessar configurações como a string de conexão e a porta do banco de dados.

2. **Criação de Rede Docker**  
    - Chama `ContainerNetworkUtils.Build()` para criar uma rede Docker personalizada onde o container do banco de dados será executado.

3. **Inicialização do Banco de Dados**  
    - Instancia a classe `Database`, passando a rede Docker e as configurações do aplicativo para configurar o ambiente de testes.

    ### Método DisposeAsync()

    O método `DisposeAsync` é responsável por realizar a limpeza do ambiente de testes após a execução. Ele executa as seguintes operações:

    1. **Limpeza do Banco de Dados**  
        - Chama o método `DisposeAsync` da classe `Database` para parar e remover o container do banco de dados utilizado nos testes.

    2. **Remoção da Rede Docker**  
        - Remove a rede Docker criada especificamente para os testes, garantindo que não haja resíduos no ambiente.

    3. **Tratamento de Exceções**  
        - Captura e registra qualquer exceção que ocorra durante o processo de limpeza, evitando que erros não tratados interrompam o fluxo de execução.

    Essa abordagem garante que o ambiente de testes seja completamente limpo e preparado para futuras execuções, evitando conflitos ou interferências.


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

    Com a `TestStarterHelper`, você pode simplificar a configuração e execução de testes de integração, garantindo um ambiente limpo e consistente para cada execução.


## Observações Finais

Este projeto foi desenvolvido como uma PoC (Prova de Conceito) para demonstrar como implementar testes de integração em aplicações ASP.NET Core. Ele pode ser adaptado para diferentes cenários e tecnologias, servindo como base para outras aplicações.

Pronto! Agora você tem um projeto de teste configurado e integrado à sua aplicação.