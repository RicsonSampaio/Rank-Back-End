-- Execute este script completo no MySQL local.
-- IF NOT EXISTS permite executar novamente sem apagar dados.
-- Tabelas existentes não têm sua estrutura alterada por este script.
CREATE DATABASE IF NOT EXISTS rank_local
    CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

USE rank_local;

CREATE TABLE IF NOT EXISTS organizacao
(
    id INT(11) NOT NULL AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(150) NOT NULL,
    dataCriacao DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    logo VARCHAR(2048) NULL
) ENGINE=InnoDB CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS usuario
(
    id INT(11) NOT NULL AUTO_INCREMENT PRIMARY KEY,
    email VARCHAR(320) NOT NULL,
    name VARCHAR(150) NOT NULL,
    passwordhash VARCHAR(255) NOT NULL,
    isactive BOOLEAN NOT NULL DEFAULT TRUE,
    idOrganizacao INT NULL,
    admin BOOLEAN NOT NULL DEFAULT FALSE,
    dataCriacao DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    fotoAccount VARCHAR(2048) NULL,
    CONSTRAINT uq_usuario_email UNIQUE (email)
) ENGINE=InnoDB CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS coletivo
(
    id INT(11) NOT NULL AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(150) NOT NULL,
    dataCriacao DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    dataAtualizacao DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    idOrganizacao INT NOT NULL,
    idTipoColetivo INT NOT NULL,
    logo VARCHAR(2048) NULL
) ENGINE=InnoDB CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS coletivo_usuario
(
    id INT(11) NOT NULL AUTO_INCREMENT PRIMARY KEY,
    idColetivo INT NOT NULL,
    idUsuario INT NOT NULL
) ENGINE=InnoDB CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS tarefa
(
    id INT(11) NOT NULL AUTO_INCREMENT PRIMARY KEY,
    idColetivo INT NOT NULL DEFAULT 0,
    idEspaco INT NOT NULL DEFAULT 0,
    idEscopo INT NULL DEFAULT 0,
    idstatus INT NOT NULL DEFAULT 0,
    idCategoria INT NOT NULL DEFAULT 0,
    idUsuarioCriacao INT NOT NULL DEFAULT 0,
    titulo TEXT NOT NULL,
    privada BOOLEAN NOT NULL DEFAULT FALSE,
    descricao TEXT NULL,
    dataCriacao DATETIME NOT NULL,
    dataAtualizacao DATETIME NULL DEFAULT NULL,
    lastDoneDate DATETIME NULL DEFAULT NULL,
    idTarefaPai INT NULL DEFAULT NULL,
    userListMarcados TEXT NULL DEFAULT NULL,
    userListParticipantes TEXT NOT NULL,
    idDocumento INT NULL DEFAULT 0,
    prazoInicial DATETIME NULL DEFAULT NULL,
    prazoFinal DATETIME NULL DEFAULT NULL,
    idResponsavel INT NULL DEFAULT 0,
    idFase INT NULL DEFAULT 0,
    idRelevancia INT NOT NULL DEFAULT 1
) ENGINE=InnoDB CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
