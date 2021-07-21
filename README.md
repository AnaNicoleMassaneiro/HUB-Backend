# HUB - Backend

## Instalação / Configuração:

1) Clone o repositório 
git clone https://github.com/AnaNicoleMassaneiro/HUB-Backend.git

2) Abra o arquivo do projeto (Visual Studio 2019)

3) Crie um banco de dados (hubUfprDb) no MySQL.

4) Modifique o arquivo ** appsettings.json ** em ** WebAPI Project **, connectionstring:
   ```
     "AppSettings": {
    "MySqlConnectionString": "Server=localhost;Port=3306;Database=hubUfprDb;Uid=nomeDoAutenticadoDoBanco;Pwd=senha;"
   ```
Modifique usando seus valores.

5) Execute o script da tabela do usuário (localizado no projeto do banco de dados)

```
CREATE TABLE User (
    `Id`             INT            AUTO_INCREMENT  NOT NULL,
    `Name`           VARCHAR (50)   NULL,
    `Surname`        VARCHAR (50)   NULL,
    `Email`          VARCHAR (50)   NULL,
    `Phone`          NCHAR (10)     NULL,
    `LastLogon`      DATETIME (6)  NULL,
    `CreatedOn`      DATETIME (6)  NULL,
    `ActivationCode` INT            NULL,
    `Login`          VARCHAR (50)   NOT NULL,
    `Password`       VARCHAR (50)   NOT NULL,
    CONSTRAINT `PK_User` PRIMARY KEY (`Id` ASC)
);

```

6) Execute o projeto.

7) Depois de executar o projeto, na barra de endereço você terá algo como: http://localhost:52915/ (a porta pode mudar) adicione swagger ao endereço, por exemplo: http://localhost:52915/swagger, um A página Swagger deve ser exibida.

8) Você notará 2 endpoints, ** api/user** e ** api/user/ create **, o primeiro é para fazer o login e o segundo para criar um novo usuário para gerar o token para a autenticação JWT.

9) Acesse o endpoind **/api/user/ create ** e crie um novo usuário, você deve receber o resultado "Usuário criado com sucesso! :)"

10) Teste o login com o usuário criado no endpoint **/api/user**, com a seguinte solicitação
```
{
  "username": "UserNameHere",
  "password": "PasswordHere"
}
```
11) Você deve receber um ReponseBody como este:
```
{
  "Id": 2,
  "Name": null,
  "Surname": null,
  "Email": null,
  "Phone": null,
  "LastLogon": "2017-09-29T23:00:56.3166667",
  "CreatedOn": "2017-09-29T23:00:56.3166667",
  "ActivationCode": null,
  "Login": null,
  "Password": null,
  "Token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InRlIiwibmJmIjoxNTA2NzE4ODk0LCJleHAiOjE1MDY3MjAwOTQsImlhdCI6MTUwNjcxODg5NH0.L5LEVLclhj8MSx4stFO44HYRkkdVwb3Pk_ILejRtqVA"
}
```

### Técnologias utilizadas
WebAPI (.NET Core 3.1) Base Project with JWT Auth, Dapper (ORM), MySQL, Swagger, DI

### Desenvolvido por:
Ana Nicole Massaneiro, Dandi Teoa e Matheus Augusto Morais
