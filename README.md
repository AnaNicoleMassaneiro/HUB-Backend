# HUB - Backend

## Instalação / Configuração:

1) Clone o repositório 
git clone https://github.com/AnaNicoleMassaneiro/HUB-Backend.git

2) Abra o arquivo do projeto (Visual Studio 2019)

3) Crie um banco de dados (hubUfprDb) no MySQL.

4) Modifique o arquivo ** appsettings.json ** em ** WebAPI Project **, connectionstring:
   ```json
     "AppSettings": {
         "MySqlConnectionString": "Server=localhost;Port=3306;Database=hubUfprDb;Uid=nomeDoAutenticadoDoBanco;Pwd=senha;"
   ```
Modifique usando seus valores.

5) Execute o script da tabela do usuário (localizado no projeto do banco de dados)

```sql
create table table_name
(
	Id int auto_increment,
	Name VARCHAR(50) null,
	Surname VARCHAR(50) null,
	Email VARCHAR(50) null,
	Phone VARCHAR(20) null,
	LastLogon DATETIME(6) null,
	CreatedOn datetime(6) null,
	ActivationCode int null,
	Login VARCHAR(50) null,
	Passaword VARCHAR(50) null,
	GRR varchar(50) null,
	TypeUser varchar(50) null,
	NoteApp VARCHAR(50) null,
	constraint table_name_pk
		primary key (Id)
);

```

6) Execute o projeto.

7) Depois de executar o projeto, na barra de endereço você terá algo como: http://localhost:5000/ (a porta pode mudar) adicione swagger ao endereço, por exemplo: http://localhost:5000/swagger, um A página Swagger deve ser exibida.

8) Você notará 2 endpoints, ** api/user** e ** api/user/ create **, o primeiro é para fazer o login e o segundo para criar um novo usuário para gerar o token para a autenticação JWT.

9) Acesse o endpoind **/api/user/ create ** e crie um novo usuário, você deve receber o resultado "Usuário criado com sucesso! :)"

10) Crie um usuario com  (ainda em desenvolvimento)

```json
 {
     "username": "ana",
     "password": "teste",
     "confirmpassword": "este"
}
```

11) Teste o login com o usuário criado no endpoint **/api/user**, com a seguinte solicitação
```json
{
  "username": "ana",
  "password": "teste"
}
```
12) Você deve receber um ReponseBody como este:
```json
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
