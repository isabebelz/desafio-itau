# Desafio Técnico: Sistema de Compra Programada de Ações

## Visão Geral

Este repositório contém a solução para o desafio técnico de Engenharia de Software da **Itaú Corretora**. O projeto consiste no desenvolvimento de um **Sistema de Compra Programada de Ações**, uma plataforma robusta que permite a clientes aderirem a um plano de investimento recorrente e automatizado em uma carteira de ações recomendada ("Top Five").

O sistema foi projetado para gerenciar todo o ciclo de vida do investimento, desde o aporte mensal do cliente até a execução consolidada de ordens na B3, a distribuição proporcional de ativos e o cálculo de regras complexas do mercado financeiro, como preço médio, imposto de renda (dedo-duro e isenção de 20k) e rebalanceamento de carteira.

## Tecnologias e Arquitetura

A solução foi construída utilizando uma stack moderna e aderente aos requisitos do desafio, com foco em boas práticas de desenvolvimento, testabilidade e escalabilidade.

- **Backend:** .NET Core (C#)
- **Banco de Dados:** MySQL
- **Mensageria Assíncrona:** Apache Kafka
- **API:** Arquitetura REST com documentação OpenAPI (Swagger)
- **Containerização:** Docker e Docker Compose
- **Fonte de Dados de Mercado:** Parse de arquivos de cotações históricas (COTAHIST) da B3

O projeto segue os princípios da **Arquitetura Limpa (Clean Architecture)** para garantir a separação de responsabilidades e um baixo acoplamento entre as camadas de Domínio, Aplicação e Infraestrutura.

## Pré-requisitos

Antes de começar, certifique-se de que você tem a seguinte ferramenta instalada em sua máquina:

- [Docker Desktop](https://www.docker.com/products/docker-desktop)

## Como Executar o Projeto

Siga os passos abaixo para configurar e executar a aplicação localmente.

### 1. Clone o Repositório

```bash
git clone https://github.com/seu-usuario/desafio-itau-corretora.git
cd desafio-itau-corretora
```

### 2. Preparação dos Dados (COTAHIST)

Devido ao tamanho elevado dos arquivos de cotações históricas da B3 (que podem exceder 150MB), o arquivo de dados **não está incluso neste repositório**.

- Realize o download do arquivo COTAHIST (ex: `COTAHIST_A2026.TXT`) diretamente no portal de dados da B3.
- Na raiz do projeto, crie, caso não exista, uma pasta chamada `cotacoes`.
- Mova o arquivo baixado para dentro desta pasta `/cotacoes` para que o sistema possa processá-lo via volume do Docker.

### 3. Inicie o Ambiente com Docker

O `docker-compose` irá orquestrar e iniciar todos os serviços de backend, a API e o Worker.

Na raiz do projeto, execute:

```bash
docker-compose up -d
```

- O comando com a flag `-d` (detached) irá executar os contêineres em segundo plano.
- Para verificar se os serviços subiram corretamente, utilize o comando `docker ps`.
- Você deve ver os contêineres `itau-mysql`, `itau-kafka`, `itau-zookeeper`, `itau-compra-programada-api` e `itau-compra-programada-worker` com o status `Up`.

A aplicação estará escutando na porta [http://localhost:5000](http://localhost:5000).

### 4. Acesse a Documentação da API (Swagger)

Com o Docker em execução, abra seu navegador e acesse a interface do Swagger para explorar e interagir com os endpoints disponíveis:

[http://localhost:5000/swagger](http://localhost:5000/swagger)

## Estrutura dos Projetos

A solução está organizada em projetos distintos, seguindo a Arquitetura Limpa:

- **CompraProgramada.Domain**: Camada mais interna. Contém as entidades de negócio, regras puras e interfaces.
- **CompraProgramada.Application**: Lógica dos casos de uso, DTOs, validações e orquestração do Motor de Compra.
- **CompraProgramada.Api**: Ponto de entrada REST da aplicação.
- **CompraProgramada.Infra.Data**: Acesso a dados (EF Core/Dapper) e integração com Kafka.
- **CompraProgramada.Infra.Ioc**: Configuração central de Injeção de Dependência.
- **CompraProgramada.Worker**: Serviço para processamento assíncrono e consumo de mensagens do Kafka.

---