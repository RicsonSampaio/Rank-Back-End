-- Execute este script completo no MySQL local.
-- IF NOT EXISTS permite executar novamente sem apagar dados.
-- Tabelas existentes não têm sua estrutura alterada por este script.
CREATE DATABASE IF NOT EXISTS rank_local
    CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

USE rank_local;

CREATE TABLE IF NOT EXISTS organizacao
(
    id BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(150) NOT NULL,
    dataCriacao DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    logo VARCHAR(2048) NULL
) ENGINE=InnoDB CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS usuario
(
    id BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    email VARCHAR(320) NOT NULL,
    name VARCHAR(150) NOT NULL,
    passwordhash VARCHAR(255) NOT NULL,
    isactive BOOLEAN NOT NULL DEFAULT TRUE,
    idOrganizacao BIGINT NULL,
    admin BOOLEAN NOT NULL DEFAULT FALSE,
    dataCriacao DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    fotoAccount VARCHAR(2048) NULL,
    CONSTRAINT uq_usuario_email UNIQUE (email)
) ENGINE=InnoDB CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS coletivo
(
    id BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(150) NOT NULL,
    dataCriacao DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    dataAtualizacao DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    idOrganizacao BIGINT NOT NULL,
    idTipoColetivo BIGINT NOT NULL,
    logo VARCHAR(2048) NULL
) ENGINE=InnoDB CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS coletivo_usuario
(
    id BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    idColetivo BIGINT NOT NULL,
    idUsuario BIGINT NOT NULL
) ENGINE=InnoDB CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
