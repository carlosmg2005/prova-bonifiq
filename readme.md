# RESOLUÇÃO

### Parte 1
Correção do Comportamento Aleatório: O Parte1Controller foi desenvolvido para gerar uma API que retorna um número aleatório a cada chamada. No entanto, identificou-se que o código inicial gerava sempre o mesmo valor, devido à reinicialização da semente a cada chamada. A solução implementada consistiu em criar uma instância única de Random durante a inicialização do serviço RandomService, utilizando uma semente única baseada no hash de uma nova instância de Guid. Isso garante a geração de números aleatórios diferentes em cada chamada subsequente.

### Parte 2
Correção do Bug de Paginação e Melhorias na Estrutura: O Parte2Controller, responsável por retornar produtos paginados, apresentava um bug na lógica de paginação. A correção envolveu ajustes no método ListProducts para garantir que os resultados retornassem corretamente com base no número da página. Além disso, foram realizadas melhorias na estrutura, adotando a injeção de dependência e refatorando as classes CustomerList e ProductList para herdar de uma classe base PagedList<T>. A classe ProductService foi ajustada para herdar de uma classe base ListService<T>, eliminando repetições de código e seguindo boas práticas

### Parte 3
Refatoração para Estratégias de Pagamento: O Parte3Controller, responsável pelos pagamentos de compras, apresentava uma estrutura problemática no método PayOrder da classe OrderService, utilizando instruções if para diferentes métodos de pagamento. A solução adotou o princípio Open-Closed, refatorando os métodos de pagamento para estratégias independentes implementadas pelas classes PixPayment, CreditCardPayment e PayPalPayment. Foi introduzido um dicionário para mapear cada forma de pagamento à sua respectiva estratégia, proporcionando uma arquitetura mais flexível e preparada para futuras extensões.

### Parte 4
Testes Unitários e Melhorias na Estrutura: O Parte4Controller, envolvido na validação de negócios para determinar se um consumidor pode realizar uma compra, foi submetido à criação de testes unitários. Utilizando o framework Xunit, os testes foram estruturados para cobrir diversos cenários, garantindo uma ampla cobertura de código. A estratégia adotada incluiu simulações de cenários como clientes não registrados tentando comprar, clientes com compras recentes tentando comprar novamente e clientes fazendo a primeira compra excedendo o valor máximo. A utilização do Entity Framework Core em memória permitiu a simulação controlada do banco de dados para garantir testes isolados e independentes.
Essas melhorias buscam não apenas corrigir os problemas identificados, mas também promover boas práticas de programação, modularidade e preparação para futuras extensões no código-fonte do sistema.

### Melhorias Adicionais no endpoint "/Parte3/orders": 
Além das correções e melhorias mencionadas anteriormente, fiz algumas alterações adicionais no retorno do endpoint para proporcionar uma experiência mais consistente e informativa:

Incremento do ID da Order: O ID da ordem agora é gerado de forma incremental, melhorando a consistência e a clareza na identificação das ordens.

Atualização Dinâmica da Data: A data de criação da ordem, que estava estática, agora é gerada dinamicamente, refletindo a data e hora atuais.

Melhoria na Representação do Cliente: No retorno da ordem, o cliente agora é representado de forma mais completa, incluindo o ID do cliente, e o nome. Isso fornece informações mais abrangentes sobre o cliente associado à ordem.
Agora, o retorno do endpoint apresenta uma estrutura mais rica e coerente, contribuindo para uma experiência de API mais robusta e compreensível para os consumidores da API.





# Prova BonifiQ Backend
Olá!
Essa prova foi criada para testar suas habilidades com .NET e C# em geral. 
Por favor, siga atentamente as instruções antes de começar, ok?

Não conseguiu fazer alguma etapa? Blza, entrega o que você conseguir ;)

## Para começar
O primeiro passo é criar uma **cópia** deste repositório. Por favor, perceba que fazer uma cópia é diferente de realizar um clone ou fork. Siga os passos abaixo para fazer a cópia:

- Crie um novo repositório em sua conta do GitHub. Dê o nome de ***prova-bonifiq***
- Abra seu client do git e siga os comandos:
```
git clone --bare https://github.com/bonifiq/prova-backend.git
```
Esse comando gera uma cópia do repositório da prova em seu computador. Agora, continue com os comandos
```
cd prova-backend.git
git push --mirror https://github.com/SEUSUARIO/prova-bonifiq.git
```
Note que você precisa alterar o SEUUSUARIO pelo seu username do GitHub, utilizado para criar o repositório no primeiro passo.
Você pode apagar o diretório ```prova-backend.git``` que foi criado em seu computador.

Tudo certo: você possui um repositório em seu nome com tudo que precisa para começar responder sua prova. Agora sim, faça o clone (git clone) em sua máquina e você está pronto para trabalhar.
```
git clone https://github.com/SEUSUARIO/prova-bonifiq.git
```

> Lembre-se: NÃO gere um Fork do repositório. Siga os passos acima para copiar o repositório para sua conta.

## Conhecendo o projeto
> Nós recomendamos que você utilize o Visual Studio 2022 (pode ser a versão community). Você também precisa do .NET 6 instalado, ok?
Ah, não esquece de instalar o pacote de desenvolvimento para o ASP NET durante a instalação do Visual Studio.

Ao abrir o projeto no Visual Studio, você pode notar que se trata de um projeto Web API do ASP NET.  Você pode se orientar pela pasta ```Controllers``. 
Dentro dela, cada Controller representa uma etapa da prova.  Logo abaixo vamos falar mais sobre essas etapas e como resolvê-las.

Antes de rodar o projeto, você precisa rodar as migrations. Para isso, primeiro instale o [EF Tools](https://learn.microsoft.com/en-us/ef/core/get-started/overview/install#get-the-entity-framework-core-tools):
```
dotnet tool install --global dotnet-ef
```
Agora, pode rodar as migrations de fato:
```
dotnet ef database update 
``` 

Pronto, o projeto já criou as tabelas e alguns registros no seu localDB. 


Rode o projeto e, se tudo deu certo, você deverá ver uma página do Swagger com as APIs que utilizaremos no teste.

Dê uma passeada pelo projeto e note que ele tem alguns probleminhas de arquitetura. Vamos resolver isso já já


## Seu trabalho
Certo, tudo configurado e rodando. Agora vamos explicar o que você precisa fazer.

### Parte1Controller
Esse controller foi criado para gerar uma API que sempre retorna um número aleatório. 
Você pode vê-lo funcionando ao rodar o projeto e na página do Swagger, clique em Parte 1 > Try it Out > Execute.

Esse código, no entanto, tem algum problema: ele sempre retorna o mesmo valor.
Seu trabalho, portanto, é corrigir esse comportamente: cada vez que a chamada é realizada um número diferente deverá ser retornado.

### Parte2Controller
Essa API deveria retornar os produtos cadastrados de forma paginada. O usuário informa a página (page) desejada e o sistema retorna os 10 itens da mesma.
O problema é que não importa qual número de página é utilizado: os resultados estão vindo sempre os mesmos. E não apenas os 10.

Você precisa portanto:
1. Corrigir o bug de paginação. Ao passar page=1, deveria ser retornado os 10 (0 a 9) primeiros itens do banco. Ao passar page=2 deveria ser retornado os itens subsequentes (10 a 19), etc
2. Note que na Action do Controller, chamamos o ```ProductService```. Fazemos isso, instanciando o mesmo (```var productService = new ProductService(_ctx);```). Essa é uma prática ruim. Altere o código para que utilize Injeção de Dependência.
3. Agora, explore os arquivos ```/Models/CustomerList``` e ```/Models/ProductList```. Eles são bem parecidos. De fato, deve haver uma forma melhor de criar esses objetos, com menos repetição de código. Faça essa alteração.
4. Da mesma forma, como você melhoraria o ```CustomerService```e o ```ProductService``` para evitar repetição de código?

### Parte3Controller
Essa API cria o pagamento de uma compra (```PlaceOrder```). Verifique o método ```PayOrder``` da classe ```OrderService```.
Você deve ter percebido que existem diversas formas de pagamento (Pix, cartão de crédito, paypal), certo?
Essa classe, no entanto, é problemática. Imagine que teríamos que incluir um novo método de pagamento, seria mais um ```if```na estrutura.

Você precisa:
1. Faça uma alteração na arquitetura para que fique mais bem estruturado e preparado para o futuro.
Tenha certeza que o princípio Open-Closed será respeitado.

### Parte4Controller
Essa API faz uma validação de negócio e retorna se o consumidor pode realizar uma compra.
Verifique o arquivo ```CanPurchase``` da classe ```CustomerService``` e note que ele aplica diversas regras de negócio.

Seu trabalho aqui será:
1. Crie testes unitários para este método. Tente obter o máximo de cobertura possível. Se precisar, pode rearquitetar o código para facilitar nos testes.

Você pode utilizar qualquer framework de testes que desejar. 

## Como entregar
Oba! Terminou tudinho? Agora faça o seguinte:
1. Faça ```push``` para seu repositório (sim, aquele que você criou lá em cima. Nada de fork).
2. Forneça acesso ao repositório no GitHub para o usuário ```sandercamargo```
2. Preencha o formulário abaixo:
[https://forms.gle/mHipmDJJnij7FRHE7](https://forms.gle/mHipmDJJnij7FRHE7)

A gente te responde em breve, ok?
