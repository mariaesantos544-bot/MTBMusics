CREATE DATABASE MTBMusics;
GO

USE MTBMusics;
GO

-- 1. Tabela Usuario
CREATE TABLE Usuario (
    IdUsuario INT IDENTITY(1, 1) PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Email VARCHAR(150) NOT NULL UNIQUE,
    Senha VARCHAR(255) NOT NULL,
    TipoUsuario VARCHAR(20) NOT NULL,
    CHECK (TipoUsuario IN ('Admin', 'Usuario')),
    Ativo BIT NOT NULL DEFAULT 1,
    DataCadastro DATE NOT NULL DEFAULT GETDATE()
);
GO

-- 2. Tabela Musica
CREATE TABLE Musica (
    IdMusica INT IDENTITY(1, 1) PRIMARY KEY,
    Titulo VARCHAR(150) NOT NULL,
    Artista VARCHAR(150) NOT NULL,
    DataLancamento DATE NOT NULL,
    CHECK (YEAR(DataLancamento) BETWEEN 1860 AND YEAR(GETDATE())),
    DuracaoSegundos INT NOT NULL,
    CHECK (DuracaoSegundos > 0),
    Letra VARCHAR(MAX) NULL,
    DataCadastro DATE NOT NULL DEFAULT GETDATE(),
    DataAtualizacao DATE NULL
);
GO

-- 3. Tabela Genero
CREATE TABLE Genero (
    IdGenero INT IDENTITY(1, 1) PRIMARY KEY,
    Nome VARCHAR(50) NOT NULL UNIQUE,
    Descricao VARCHAR(255) NULL
);
GO

-- Tabela MusicaGenero
CREATE TABLE MusicaGenero (
    IdMusica INT NOT NULL,
    IdGenero INT NOT NULL,

    CONSTRAINT PK_MusicaGenero
    PRIMARY KEY (IdMusica, IdGenero),

    CONSTRAINT FK_MusicaGenero_Musica
    FOREIGN KEY (IdMusica) REFERENCES Musica(IdMusica),

    CONSTRAINT FK_MusicaGenero_Genero
    FOREIGN KEY (IdGenero) REFERENCES Genero(IdGenero)
);
GO

-- TRIGGER: Exclusao de Usuario - soft delete
CREATE TRIGGER TR_UsuarioSoftDelete
ON Usuario
INSTEAD OF DELETE
AS
BEGIN
    UPDATE Usuario
    SET Ativo = 0
    WHERE IdUsuario IN
    (SELECT IdUsuario FROM deleted)
END;
GO

-- INSERT Usuario
INSERT INTO Usuario (Nome, Email, Senha, TipoUsuario)
VALUES
(
    'Ana Silva',
    'ana@email.com',
    CONVERT(VARCHAR(255), HASHBYTES('SHA2_256', 'admin123'), 2),
    'Admin'
);
GO

-- INSERT Genero
INSERT INTO Genero (Nome, Descricao)
VALUES
('Rock', 'Gênero musical caracterizado por instrumentos como guitarra, baixo e bateria.'),
('Pop', 'Gênero musical popular com melodias marcantes.'),
('MPB', 'Música Popular Brasileira.');
GO

-- INSERT Musica
INSERT INTO Musica (Titulo, Artista, DataLancamento, DuracaoSegundos, Letra)
VALUES
(
    'Bohemian Rhapsody',
    'Queen',
    '19751031',
    354,
    NULL
);
GO

INSERT INTO MusicaGenero (IdMusica, IdGenero)
VALUES 
(1, 1),
(1, 2);
GO
