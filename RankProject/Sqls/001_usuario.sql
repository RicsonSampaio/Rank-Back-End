-- Execute no banco MySQL escolhido para o projeto.
-- A criacao do banco fica a seu criterio; este script nao fixa o nome dele.
CREATE TABLE usuario
(
    Id BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Email VARCHAR(320) NOT NULL,
    Name VARCHAR(150) NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    IsActive BOOLEAN NOT NULL DEFAULT TRUE,
    CONSTRAINT UQ_usuario_Email UNIQUE (Email)
) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
