create table User (
    Id              int auto_increment primary key,
    Name            varchar(100) not null,
    Password        varchar(64) not null,
    Latitude        float null,
    Longitude       float null,
    NoteApp         float null,
    Email           varchar(100) unique not null,
    GRR             varchar(8) unique not null,
    Telefone		varchar(15),
    LastLogon       datetime(6) null,
    CreatedOn       datetime(6) null,
    ActivationCode  int         null,
    Token           varchar(64) null,
    IsVendedor      boolean not null
);

create table Vendedor (
	idVendedor 		int auto_increment primary key,
    idUser			int unique not null,
    isAtivo			bool not null,
    isOpen			bool not null,
    foreign key (idUser) references User (Id)
);

create table Cliente (
	idCliente 		int auto_increment primary key,
    idUser			int unique not null,
    foreign key (idUser) references User (Id)
);

create table Produto (
	idProduto				int auto_increment primary key,
    idVendedor				int not null,
    nome					varchar(50) not null,
    isAtivo					bool not null,
    preco					float not null,
    notaProduto				float default 0,
    descricao				varchar(200),
    imagem					mediumtext,
    quantidadeDisponivel	int not null,
    foreign key (idVendedor) references Vendedor(idVendedor)
);

create table StatusReserva (
	codigo int not null primary key,
    descricao varchar(10) not null unique
);

INSERT INTO StatusReserva (codigo, descricao) VALUES
	(0, "PENDENTE"),
    (1, "CONCLUIDA"),
    (2, "EXPIRADA"),
    (3, "CANCELADA");

create table Reserva (
	idReserva			int auto_increment primary key,
    idCliente 			int not null,
    idProduto 			int not null,
    statusReserva		int not null default 0,
    dataCriacao			timestamp not null default NOW(),
    quantidadeDesejada	int not null,
    localizacaoLat		float,
    localizacaoLong		float,
    foreign key (idCliente) references Cliente (idCliente),
    foreign key (idProduto) references Produto (idProduto),
    foreign key (statusReserva) references StatusReserva (codigo)
);

create table VendedorFavorito(
	idVendedor 			int not null,
    idCliente 			int not null,
    primary key (idVendedor, idCliente),
    foreign key (idVendedor) references Vendedor (idVendedor),
    foreign key (idCliente) references Cliente (idCliente)
);

create table TipoAvaliacao(
	codigo int auto_increment primary key,
    descricao varchar(50)
);

INSERT INTO TipoAvaliacao VALUES 
	(1, "Cliente => Produto"), (2, "Cliente => Vendedor"), (3, "Vendedor => Cliente");

create table Avaliacao(
	idAvaliacao 		int auto_increment primary key,
	tipoAvaliacao 		int not null,
    idCliente 			int,
    idVendedor 			int,
    idProduto 			int,
    titulo				varchar(50) not null,
    nota 				int not null,
    descricao 			varchar(200),
    dataCriacao			datetime default NOW(),
    foreign key (idVendedor) references Vendedor (idVendedor),
    foreign key (idCliente) references Cliente (idCliente),
    foreign key (idProduto) references Produto (idProduto),
    foreign key (tipoAvaliacao) references TipoAvaliacao(codigo)
);

create table FormaPagamento(
	idFormaPagamento int auto_increment primary key,
    descricao varchar(50) unique not null,
    icone varchar(100) not null
);

create table VendedorFormaPagamento(
	idFormaPagamento int not null,
    idVendedor int not null,
    foreign key (idFormaPagamento) references FormaPagamento (idFormaPagamento),
    foreign key (idVendedor) references Vendedor (idVendedor),
    primary key (idFormaPagamento, idVendedor)
);

INSERT INTO FormaPagamento (descricao, icone) VALUES
	("PIX", "assets/pix.png"),
    ("Cartão de débito", "assets/cartao.png"),
    ("Cartão de crédito", "assets/cartao.png"),
    ("Dinheiro", "assets/dinheiro.png");