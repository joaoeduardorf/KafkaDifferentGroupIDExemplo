# KafkaDifferentGroupIDExemplo


## Serviços

Este ambiente Docker Compose inclui os seguintes serviços:

- **zookeeper**: Serviço Zookeeper, necessário para o Kafka. Porta `2183` (interna `2181`) configurada para permitir o gerenciamento de brokers do Kafka.

- **kafka**: Serviço de mensageria Kafka. Configurado com as portas `29093` (externa) e `9093` (interna, usada por outros contêineres), conectado ao Zookeeper.

## Requisitos

- **Docker** e **Docker Compose** devem estar instalados no sistema.

## Configuração das Variáveis de Ambiente

As variáveis de ambiente principais estão configuradas diretamente no `docker-compose.yml` e incluem:

- **ASPNETCORE_ENVIRONMENT**: Define o ambiente para as APIs como `Production`.
- **Kafka__BootstrapServers**: Servidor bootstrap do Kafka (`kafka:9093`).

## Uso

Para iniciar o ambiente Docker Compose, execute o seguinte comando na raiz do projeto:

```bash
docker-compose up --build
```
Para parar e remover os contêineres e as redes associadas, execute:
```bash
docker-compose down
```
