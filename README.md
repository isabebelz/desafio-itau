# Desafio Técnico: Sistema de Compra Programada de Ações

## Visão Geral

Este repositório contém a solução para o desafio técnico de Engenharia de Software da **Itaú Corretora**. O projeto consiste no desenvolvimento de um **Sistema de Compra Programada de Ações**, uma plataforma robusta que permite a clientes aderirem a um plano de investimento recorrente e automatizado em uma carteira de ações recomendada ("Top Five").

O sistema foi projetado para gerenciar todo o ciclo de vida do investimento, desde o aporte mensal do cliente até a execução consolidada de ordens na B3, a distribuição proporcional de ativos e o cálculo de regras complexas do mercado financeiro, como preço médio, imposto de renda (dedo-duro e isenção de 20k) e rebalanceamento de carteira.

## Tecnologias e Arquitetura

A solução foi construída utilizando uma stack moderna e aderente aos requisitos do desafio, com foco em boas práticas de desenvolvimento, testabilidade e escalabilidade.

*   **Backend:** .NET Core (C#)
*   **Banco de Dados:** MySQL
*   **Mensageria Assíncrona:** Apache Kafka
*   **API:** Arquitetura REST com documentação OpenAPI (Swagger)
*   **Containerização:** Docker e Docker Compose
*   **Fonte de Dados de Mercado:** Parse de arquivos de cotações históricas (COTAHIST) da B3

O projeto segue os princípios da **Arquitetura Limpa (Clean Architecture)** para garantir a separação de responsabilidades e um baixo acoplamento entre as camadas de Domínio, Aplicação e Infraestrutura.

## Pré-requisitos

Antes de começar, certifique-se de que você tem a seguinte ferramenta instalada em sua máquina:

*   [Docker Desktop](https://www.docker.com/products/docker-desktop/)

## Como Executar o Projeto

Siga os passos abaixo para configurar e executar a aplicação localmente.

### 1. Clone o Repositório

```bash
git clone https://github.com/seu-usuario/desafio-itau-corretora.git
cd desafio-itau-corretora
```

### 2. Inicie o Ambiente de Infraestrutura com Docker

O `docker-compose` irá orquestrar e iniciar todos os serviços de backend necessários.

Na raiz do projeto, execute:

```bash
docker-compose up -d
```

O comando com a flag `-d` (detached) irá executar os contêineres em segundo plano. Para verificar se os serviços subiram corretamente, utilize o comando `docker ps`. Você deve ver os contêineres `itau-mysql`, `itau-kafka` e `itau-zookeeper` com o status `Up`.

A aplicação será iniciada e estará escutando nas portas definidas  (por exemplo, `http://localhost:5000`).

### 3. Acesse a Documentação da API (Swagger)

Com o docker em execução, abra seu navegador e acesse a interface do Swagger para explorar e interagir com os endpoints disponíveis:

**[http://localhost:5000/swagger](http://localhost:5000/swagger)**

*(A porta pode variar. Verifique o output do comando `dotnet run` para a porta HTTP correta).*

## Estrutura dos Projetos

A solução está organizada em projetos distintos, seguindo a Arquitetura Limpa, cada um com uma responsabilidade clara:

*   **`CompraProgramada.Domain`**: Camada mais interna. Contém as entidades de negócio (ex: Cliente, Ação, Ordem), regras de negócio puras e as interfaces dos repositórios. Não possui dependências de frameworks.
*   **`CompraProgramada.Application`**: Contém a lógica dos casos de uso da aplicação (CQRS), DTOs (Data Transfer Objects), validações e mapeamentos. Orquestra o fluxo de dados entre o domínio e a infraestrutura.
*   **`CompraProgramada.Api`**: O ponto de entrada da aplicação. Expõe os endpoints REST e lida com as requisições e respostas HTTP.
*   **`CompraProgramada.Infra.Data`**: Implementação concreta do acesso a dados (Entity Framework Core, Repositórios), configurações do banco de dados e serviços de mensageria (Kafka).
*   **`CompraProgramada.Infra.Ioc`**: Projeto centralizador para a configuração da Injeção de Dependência (IoC), registrando todas as interfaces e suas implementações.
*   **`CompraProgramada.Worker`**: Serviço responsável pelo processamento assíncrono das ordens de compra programada, que consome mensagens do Kafka e executa tarefas/background jobs desacoplados da API principal.
