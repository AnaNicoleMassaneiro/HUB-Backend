<div align="center">
<h1>HUB UFPR - API</h1>

<a href="https://www.emojione.com/emoji/1f410">
  <img
    height="80"
    width="180"
    alt="goat"
    src="https://servicos.nc.ufpr.br/PortalNC/painel/assets/img/logos/logo_ufpr.jpg"
  />
</a>
  <br />
 </div>
  


## Instalação / Configuração:

1) Clone o repositório 
git clone https://github.com/AnaNicoleMassaneiro/HUB-Backend.git

2) Abra o arquivo do projeto (Visual Studio 2019)

3) Crie um banco de dados (hubUfprDb) no MySQL.
```sql
    create database hubUfprDb;
    use hubUfprDb;
```

4) Modifique o arquivo ** appsettings.json ** em ** WebAPI Project **, connectionstring:
   ```json
     "AppSettings": {
         "MySqlConnectionString": "Server=localhost;Port=3306;Database=hubUfprDb;Uid=nomeDoAutenticadoDoBanco;Pwd=senha;"
   ```
Modifique usando seus valores.
5) Execute o script da tabela do usuário (localizado no projeto do banco de dados)
```sql
create table User
(
    Id              int auto_increment primary key,
    Name            varchar(1000) not null,
    Password        varchar(64) not null,
    Latitude        float null,
    Longitude       float null,
    NoteApp         float null,
    Email           varchar(100) unique not null,
    GRR             varchar(8) unique not null,
    LastLogon       datetime(6) null,
    CreatedOn       datetime(6) null,
    ActivationCode  int         null,
    Token           varchar(64) null,
    IsVendedor      boolean not null
);
```
6) Execute o projeto.
7) Depois de executar o projeto, na barra de endereço você terá algo como: http://localhost:5000/ (a porta pode mudar) adicione swagger ao endereço, por exemplo: http://localhost:5000/swagger, um A página Swagger deve ser exibida.
8) Você notará 2 endpoints, **api/user** e ** api/user/create **, o primeiro é para fazer o login e o segundo para criar um novo usuário para gerar o token para a autenticação JWT.
9) Acesse o endpoind **/api/user/create** e crie um novo usuário, você deve receber o resultado "Usuário criado com sucesso! :)"
10) Crie um usuario com  (ainda em desenvolvimento)
```json
{
  "nome": "matheus",
  "usuario": "theus",
  "senha": "123",
  "confirmacaoSenha": "123",
  "grr": "grr",
  "email": "teste@teste.com"
}
```

11) Teste o login com o usuário criado no endpoint **/api/user**, com a seguinte solicitação
```json
{
  "usuario": "ananicole",
  "senha": "teset"
}
```
12) Você deve receber um ReponseBody como este:
```json
{
    "id": 1,
    "name": "Ana",
    "surname": "Nicole",
    "email": "ananicole@ufpr.br",
    "phone": "123123213",
    "lastLogon": "2021-07-21T10:50:58",
    "createdOn": "2021-07-21T10:51:02",
    "activationCode": "1",
    "login": null,
    "password": null,
    "token": null,
    "grr": "grr20175437",
    "typeUser": "Vendedor",
    "noteApp": "10"
}
```


## Técnologias utilizadas
WebAPI (.NET Core 3.1) Base Project with JWT Auth, Dapper (ORM), MySQL, Swagger, DI

## Desenvolvido por:
Ana Nicole Massaneiro, Dandi Teoa e Matheus Augusto de Moraes Ribeiro
