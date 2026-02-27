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
