# FIAP Cloud Games - PaymentsAPI

Microsservico responsavel por simular o processamento de pagamentos das
compras de jogos.

## Responsabilidades

- Consome `OrderPlacedEvent` publicado pela CatalogAPI.
- Simula o pagamento de forma **deterministica**: o resultado depende do
  `OrderId`, entao o mesmo pedido sempre gera o mesmo resultado (80% de
  aprovacao). Nao ha chamada a nenhum gateway de pagamento real.
- Publica `PaymentProcessedEvent` com `Status = Approved` ou `Rejected`.

Este servico nao possui banco de dados nem endpoints HTTP de negocio - apenas
um endpoint raiz e um `/health` para checagem de disponibilidade. Toda a
logica acontece via mensageria (RabbitMQ + MassTransit).

## Variaveis de ambiente

| Variavel | Descricao | Sensivel |
|---|---|---|
| `RabbitMq__Host` | Host do RabbitMQ | Nao (ConfigMap) |
| `RabbitMq__Username` | Usuario do RabbitMQ | Nao (ConfigMap) |
| `RabbitMq__Password` | Senha do RabbitMQ | Sim (Secret) |
| `PaymentSimulation__ForcedStatus` | Forca `Approved` ou `Rejected` em demos; vazio mantem simulacao deterministica | Nao (ConfigMap) |

## Executando localmente

```bash
cd src/FiapCloudGames.PaymentsApi
dotnet run
```

## Rodando os testes

```bash
cd tests/FiapCloudGames.PaymentsApi.Tests
dotnet test
```

## Docker

```bash
docker build -t payments-api:latest .
docker run -p 5103:8080 \
  -e RabbitMq__Host=host.docker.internal \
  payments-api:latest
```

## Kubernetes

Manifestos em `/k8s`: `ConfigMap`, `Secret`, `Deployment` e `Service`.

```bash
kubectl apply -f k8s/
```
