# Configuração



- Para configurar o serviço de mensageria é preciso primeiro ter uma conta no gmail.com. Depois disso é preciso mudar sua conta para autenticação em dois fatores,
logo apos, deverá gerar senha de App.

![image](https://user-images.githubusercontent.com/45561988/181139755-414f5754-4128-430c-952b-e4a9babc0622.png)

- Logo após gerar uma senha de App, tu terá que logar com sua conta do gmail. Logo após isso, tu deverá selecionar o serviço de email, e o sistema que desejar como mostra a imagem abaixo.

![image](https://user-images.githubusercontent.com/45561988/181139953-a76d1661-6d14-4b2a-b1d7-be14e390f5a5.png)

- Logo após isso, clicar em gerar.

- Irá aparecer uma senha, e essa senha deverá ser utilizada para inserir em appsettings.json.



- No projeto, é preciso configurar o teu servidor de email em appsettings.json do projeto principal (APIHavan), abaixo está o exemplo.

  "EmailConfiguration": {
    "From": "email@gmail.com",
    "SmtpServer": "smtp.gmail.com",
    "Port": 465,
    "Username": "email@gmail.com",
    "Password": "senha"
  },
  
 - Em "From", deverá colocar o seu usuário da sua conta do gmail, assim como em username. E em "password", deverá colocar a senha de App gerada.

- O proximo passo, será configurar o email a ser enviado. Adicionar algum email de destino em em Constants/Constants.emailCliente (tanto no projeto de teste (APIHavan.Test) quanto no principal (APIHavan)).

# Arquitetura - BD

Para criar a arquitetura, foram adicionadas as tabelas abaixo com os seguintes atributos.

- condicoespagamentos: condicaoPagamentoId, descricao, dias.
- produtos: Id, Sku, descricao.
- historicoprecos: Id, ProdutoId, Preco.
- relatoriopagamentos: Id, HistorioPrecoId, condicaoPagamentoId, clienteId.
- clientes: clienteId, cnpj, razaoSocial, email.

A tabela principal é a relatorioPagamentos que será utilizada para mostrar todo o historico de uma compra ou venda de um cliente. 

Simulando casos:

- Quando você faz uma compra, primeiro é pego o preço e depois o produto que está sendo vendido, então será feito uma requisição get para a tabela de historicopreco verificando se o produto existe e pega o ultimo valor daquele produto com aquele preço. 
- Também tem o processo do parcelamento, isso é possivel fazer com o condicoespagamentos, em descrição é falado se foi pago ou está atrasado, e a data a ser pago (dias).
- Se o cliente pagou a vista, é apenas criando uma condição pagamento descrito pago e o dia em que foi pago. Quando ele parcela, é adicionado a quantidade de condições pagamentos e ajusta em dias o dia que terá que ser pago. Quando atrasar, é enviado uma requisição put para a tabela e edita de "a pagar" para "atrasado".
- Depois do processo de adicionar no historico preços e condicoes pagamentos. Será preciso adicionar tudo em relatorio pagamentos. Ali está disponivel os valores do cliente e suas compras.

# Arquitetura - API

Clientes: Get, Post, Get (por id), Put, Delete

CondicaoPagamentos: Get, Post, Get (por id), Put, Delete

HistoricoPrecos: Get, Post, Get (por id), Put, Delete

Messsage: Get

Produtos: Get, Post, Get (por id), Put, Delete

RelatorioFiltrado: Get (cnpj ou razao social)

RelatorioPagamentos: Get, Post, Get (por id), Delete

- Em historico preços, ao enviar uma requisição put ou post, é enviado um email para o cliente avisando da mudança de preços.
- Em RelatorioFiltrado é retornado as vendas de produtos para um cliente. Retorna um KeyValuePair com o valor do Produto e o preço dele. Para essa requisição, tem-se a necessidade de inserir o cnpj ou a razao social do cliente.




