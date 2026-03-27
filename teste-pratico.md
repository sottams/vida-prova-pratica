# Teste Prático

## Conhecimentos gerais:
- C# Rest Web Api com padrões de projeto
- Utilização do Entity Framework
- Utilização do ASP.NET Core para criação de Web Api com Swagger;
- Utilização do padrão de projeto Repository
- Utilização do padrão de projeto Dependency Injection
- Utilização do padrão de projeto Inversion of Control
- Utilização do padrão de projeto CQRS
- Utilização do padrão DDD
- Utilização de Mensageria

## Bloco 1

Neste bloco é avaliado:
- C# Rest Web Api
- Utilização do Entity Framework
- Utilização do ASP.NET Core para criação de Web Api com Swagger;
- Utilização do padrão de projeto Repository
- Utilização do padrão de projeto Dependency Injection
- Utilização do padrão de projeto Inversion of Control
- Utilização do padrão de projeto CQRS
- Utilização do padrão DDD
- Utilização de Mensageria

Perspectivas da avaliação:
- Completude da tarefa;
- Acurácia da solução;
- Qualidade do código;
- Clareza de leitura;
- Testes de software automáticos.

## Tarefas – Bloco 1

1. Abra a solução SistemaCompras;

2. O projeto consiste em uma API simples para os objetos Produto e SolicitacaoCompra. Para objeto Produto já existe uma controller com métodos já implementados. Sua tarefa é evoluir, seguindo os padrões existentes na solução, o objeto de SolicitacaoCompra.

3. A entidade SolicitacaoCompra no domínio, já está implementada, adicione método necessário de forma a garantir que as seguintes regras de negócio sejam validadas:
   a. Se o Total Geral for maior que 50000 a condição de pagamento deve ser 30 dias.
   b. O total de itens de compra deve ser maior que 0.

4. Crie a interface que contém o método RegistrarCompra do repositório para SolicitacaoCompra;

5. Crie o repositório da entidade SolicitacaoCompra;

6. No projeto SistemaCompras.Application, utilizando o padrão CQRS, será necessário criar classes e interfaces do comando, são elas:
   a. RegistrarCompraCommand
   b. RegistrarCompraCommandHandler

7. No projeto SistemaCompras.API, crie a controller de SolicitacaoCompra.

8. Crie o método necessário utilizando o command criado no projeto SistemaCompras.Application.

9. Quando a compra for efetivada enviar um e-mai para o cliente e notificar o fornecedor.

10. Inclua testes de unidade e integração. Use o xUnit para os testes de unidade e o TestContainer para os testes de integração.

11. Crie os arquivos dockerfile e o Docker-compose para trabalhar de forma conteinerizada com a aplicação e o banco de dados.
