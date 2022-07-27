# WebApiDotNetCore
A simple wet api in Asp.Net Core 5


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






