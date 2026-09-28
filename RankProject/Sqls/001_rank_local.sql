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
-- Usuários de teste (líderes dos coletivos cadastrados abaixo).
-- Senha temporária das contas novas: 123
-- Cada senha tem salt aleatório próprio; emails já cadastrados não são alterados.
-- Cada usuário terá uma organização e seu coletivo correspondente.
-- Nenhum vínculo é criado em coletivo_usuario nesta etapa.
-- Nomes abaixo identificam os coletivos associados a cada email.
-- O asterisco da lista original marcou King Barov Crew e Monarquia da Coroa.

START TRANSACTION;

-- Grupos
INSERT IGNORE INTO usuario (name, email, passwordhash, isactive, admin, idOrganizacao)
VALUES
    ('Adran', 'adran@gmail.com', 'pbkdf2-sha256$210000$9rFVZaDOWJaRwhHvSMY8vw==$l6dVdpuu7A12J36O+4OJKASm0gRwjYVmr3XkvrAqRKc=', TRUE, FALSE, NULL),
    ('Alton King Alara', 'alton@gmail.com', 'pbkdf2-sha256$210000$zKwjPZAMVnFp/GxJltmXnw==$4FqsTNMXnGaoVLMFq8E7Zza1MwXslKJqEN1uuo/sms8=', TRUE, FALSE, NULL),
    ('Eleftheria', 'khovi@gmail.com', 'pbkdf2-sha256$210000$OPfPQB7ClecNiejMBe9XpQ==$25ckokNZMBHNa0JibHQTIb94Adbyk/UjKgNEVEoi1E4=', TRUE, FALSE, NULL),
    ('Circulo dos Oito', 'kalazan@gmail.com', 'pbkdf2-sha256$210000$rglpv53P/QROBw2/QjCCLg==$8Y1KmXAby7xooZ+RuAIGnNcUojbL5FfLtWlnJ7d/sTw=', TRUE, FALSE, NULL),
    ('King Barov Crew', 'kingbarov@gmail.com', 'pbkdf2-sha256$210000$YXCkYQyH6ev2N9lEwQMZfg==$QJv3j2jb6qUv90a4JaJtxVMUwMBkPu3FaRXGqmjNQhk=', TRUE, FALSE, NULL),
    ('Monarquia da Coroa', 'reiarthurxxxivlendorr@gmail.com', 'pbkdf2-sha256$210000$0qxtpTgOKS3eXfyHlt1ZJQ==$DCFv/CxrSTG04jOTCtYcJf7EzEEfNjs1I8ZsLBvTVFo=', TRUE, FALSE, NULL),
    ('Nahiri', 'nahiri@gmail.com', 'pbkdf2-sha256$210000$cttOn+hl/FT8G67Rl8wGVA==$736MkLketwccclIHPHDGw5gQ62DbLfzc7xtcFCG3p7w=', TRUE, FALSE, NULL);

-- Guildas
INSERT IGNORE INTO usuario (name, email, passwordhash, isactive, admin, idOrganizacao)
VALUES
    /* Guilda: Abzan */
    ('Ousey', 'ousey@gmail.com', 'pbkdf2-sha256$210000$vW+Kb/mAr9mRN7xyCx62Hg==$wRV84ZrHxbKKfv3UB3Lx6dpSEhHdvRUTko2msI79cZ0=', TRUE, FALSE, NULL),
    /* Guilda: Azorius */
    ('Salomao', 'salomao@gmail.com', 'pbkdf2-sha256$210000$NsmfNwWMpwezaWn2NRrFFw==$0QIEv7B0zfttpTrK6UtcJn4VFFLfmaFKswH9zw7qdFg=', TRUE, FALSE, NULL),
    /* Guilda: Bant */
    ('Iona', 'iona@gmail.com', 'pbkdf2-sha256$210000$lpRTWxZMewOhwAB/udQKfA==$0lzwWxOnMhFG2B2FrjAeO6LKZXNrnuEHsSj2GoWRC1Y=', TRUE, FALSE, NULL),
    /* Guilda: Boros */
    ('Stronmaus', 'stronmaus@gmail.com', 'pbkdf2-sha256$210000$Z9Vr75G1vpqM1mQ2N7eQhg==$dGe+kFd54qoY6wQuG6MF7ZmJ4ZEy3hR5IMnufbMikZ0=', TRUE, FALSE, NULL),
    /* Guilda: Dimir */
    ('Ashiok', 'ashiok@gmail.com', 'pbkdf2-sha256$210000$Xv9pD+DnnSTFm10dnD+W4w==$2+0hPUSrlFrBwTbX1rvBI6CXfRoqj1orGUUXg9m8Vtk=', TRUE, FALSE, NULL),
    /* Guilda: Grixis */
    ('Griselbrand', 'griselbrand@gmail.com', 'pbkdf2-sha256$210000$6HC+3PPYL4JobifgybU73w==$Rl03ToApoYHHTxv6tCvs2lGlnhsmZ7rxdhKlqjUbV2Q=', TRUE, FALSE, NULL),
    /* Guilda: Gruul */
    ('Rowan', 'rowan@gmail.com', 'pbkdf2-sha256$210000$rEPqKnj6bWAfb4FGZfw5Yg==$CLnm1Syodyndzo5Wz3ATFsF+2oBEgwdERvG4ia8lgvE=', TRUE, FALSE, NULL),
    /* Guilda: Ink */
    ('Amedaca', 'amedaca@gmail.com', 'pbkdf2-sha256$210000$t2fgvkrXu+yNdGniGYMNaA==$c51W4qwC+mOejr2y989g1TqDE2J0MGYVuLCvZ8M3wHE=', TRUE, FALSE, NULL),
    /* Guilda: Izzet */
    ('Jace', 'jace@gmail.com', 'pbkdf2-sha256$210000$NTCulLwNfDSDzf+WTAyFbQ==$iUjyqxZfkX8a67yfxgjT1qqpVMgibnbo83HT/bc8PWU=', TRUE, FALSE, NULL),
    /* Guilda: Jund */
    ('Dlarer', 'dlarer@gmail.com', 'pbkdf2-sha256$210000$s6Y+hfhm6ytFzm2Pd7RJ8A==$pls1AVCOIj1zyyq1iJI41R7RzIem89TyIsIyYPjb+Lw=', TRUE, FALSE, NULL),
    /* Guilda: Mardur */
    ('Sarkhan', 'sarkhan@gmail.com', 'pbkdf2-sha256$210000$aktz77gzzd18tarVb6lP7g==$n2cjqtW3FDf6cf1A5VEG+jFRdWiW5zcZ3KRgWz/eOvA=', TRUE, FALSE, NULL),
    /* Guilda: Naya */
    ('Udyr', 'udyr@gmail.com', 'pbkdf2-sha256$210000$8apQJ3pjepGcvQVzz/KR1g==$hLJaA4SI8Y6HrEy9jiMHiS6xVhqsROr8boC3AHmhoMk=', TRUE, FALSE, NULL),
    /* Guilda: Orzhov */
    ('Cain', 'cain@gmail.com', 'pbkdf2-sha256$210000$Phk2HShdsPLBAPP5abh0xg==$hLYvtwnyIgiPptxRi2WivV4VUpzNyP8YVwHSSwU1SAk=', TRUE, FALSE, NULL),
    /* Guilda: Rakdos Demons */
    ('Aslog', 'aslog@gmail.com', 'pbkdf2-sha256$210000$41qJ9H3NliawOBALX9C6HQ==$diao6UEvzMh3ktgXLUbMalRRCd6HwXNkUE/JFzsnCoY=', TRUE, FALSE, NULL),
    /* Guilda: Selesnya */
    ('Vitughazi', 'vitughazi@gmail.com', 'pbkdf2-sha256$210000$HGVhNZYp1kWmn9w/xrHuJQ==$FJBKd0GDXARPQXQmwhpbpZLJlpFgUZX4sEtfsiR7CLY=', TRUE, FALSE, NULL),
    /* Guilda: Simic */
    ('Kiora', 'kiora@gmail.com', 'pbkdf2-sha256$210000$vIIXNU7mlLnNWJAQxlXy2Q==$pYNAVUCyKW8hdmWxTAy5yaD9Y8ejdlBQrm/gCgNkxpE=', TRUE, FALSE, NULL),
    /* Guilda: The Pact */
    ('Edgarmarkov', 'edgarmarkov@gmail.com', 'pbkdf2-sha256$210000$bSMnb6Q1kQ6BO2xCLTQ+bw==$YFE8yhjRnmhnM9BEqb+w2oLRdUtnLGY57UPxqKDaH2A=', TRUE, FALSE, NULL);
-- Organizações correspondentes aos usuários acima.
-- Cada par de comandos usa o LAST_INSERT_ID() da organização criada nesta mesma conexão.
-- Se o usuário já tem idOrganizacao, ambos os comandos do par não alteram nada.
-- Grupos
-- Grupo: Adran
INSERT INTO organizacao (nome, dataCriacao, logo)
SELECT u.name, UTC_TIMESTAMP(), NULL
FROM usuario u
WHERE u.email = 'adran@gmail.com' AND u.idOrganizacao IS NULL;
UPDATE usuario
SET idOrganizacao = LAST_INSERT_ID()
WHERE email = 'adran@gmail.com' AND idOrganizacao IS NULL;

-- Grupo: Alton King Alara
INSERT INTO organizacao (nome, dataCriacao, logo)
SELECT u.name, UTC_TIMESTAMP(), NULL
FROM usuario u
WHERE u.email = 'alton@gmail.com' AND u.idOrganizacao IS NULL;
UPDATE usuario
SET idOrganizacao = LAST_INSERT_ID()
WHERE email = 'alton@gmail.com' AND idOrganizacao IS NULL;

-- Grupo: Eleftheria
INSERT INTO organizacao (nome, dataCriacao, logo)
SELECT u.name, UTC_TIMESTAMP(), NULL
FROM usuario u
WHERE u.email = 'khovi@gmail.com' AND u.idOrganizacao IS NULL;
UPDATE usuario
SET idOrganizacao = LAST_INSERT_ID()
WHERE email = 'khovi@gmail.com' AND idOrganizacao IS NULL;

-- Grupo: Circulo dos Oito
INSERT INTO organizacao (nome, dataCriacao, logo)
SELECT u.name, UTC_TIMESTAMP(), NULL
FROM usuario u
WHERE u.email = 'kalazan@gmail.com' AND u.idOrganizacao IS NULL;
UPDATE usuario
SET idOrganizacao = LAST_INSERT_ID()
WHERE email = 'kalazan@gmail.com' AND idOrganizacao IS NULL;

-- Grupo: King Barov Crew
INSERT INTO organizacao (nome, dataCriacao, logo)
SELECT u.name, UTC_TIMESTAMP(), NULL
FROM usuario u
WHERE u.email = 'kingbarov@gmail.com' AND u.idOrganizacao IS NULL;
UPDATE usuario
SET idOrganizacao = LAST_INSERT_ID()
WHERE email = 'kingbarov@gmail.com' AND idOrganizacao IS NULL;

-- Grupo: Monarquia da Coroa
INSERT INTO organizacao (nome, dataCriacao, logo)
SELECT u.name, UTC_TIMESTAMP(), NULL
FROM usuario u
WHERE u.email = 'reiarthurxxxivlendorr@gmail.com' AND u.idOrganizacao IS NULL;
UPDATE usuario
SET idOrganizacao = LAST_INSERT_ID()
WHERE email = 'reiarthurxxxivlendorr@gmail.com' AND idOrganizacao IS NULL;

-- Grupo: Nahiri
INSERT INTO organizacao (nome, dataCriacao, logo)
SELECT u.name, UTC_TIMESTAMP(), NULL
FROM usuario u
WHERE u.email = 'nahiri@gmail.com' AND u.idOrganizacao IS NULL;
UPDATE usuario
SET idOrganizacao = LAST_INSERT_ID()
WHERE email = 'nahiri@gmail.com' AND idOrganizacao IS NULL;

-- Guildas
-- Guilda: Abzan / líder: Ousey
INSERT INTO organizacao (nome, dataCriacao, logo)
SELECT u.name, UTC_TIMESTAMP(), NULL
FROM usuario u
WHERE u.email = 'ousey@gmail.com' AND u.idOrganizacao IS NULL;
UPDATE usuario
SET idOrganizacao = LAST_INSERT_ID()
WHERE email = 'ousey@gmail.com' AND idOrganizacao IS NULL;

-- Guilda: Azorius / líder: Salomao
INSERT INTO organizacao (nome, dataCriacao, logo)
SELECT u.name, UTC_TIMESTAMP(), NULL
FROM usuario u
WHERE u.email = 'salomao@gmail.com' AND u.idOrganizacao IS NULL;
UPDATE usuario
SET idOrganizacao = LAST_INSERT_ID()
WHERE email = 'salomao@gmail.com' AND idOrganizacao IS NULL;

-- Guilda: Bant / líder: Iona
INSERT INTO organizacao (nome, dataCriacao, logo)
SELECT u.name, UTC_TIMESTAMP(), NULL
FROM usuario u
WHERE u.email = 'iona@gmail.com' AND u.idOrganizacao IS NULL;
UPDATE usuario
SET idOrganizacao = LAST_INSERT_ID()
WHERE email = 'iona@gmail.com' AND idOrganizacao IS NULL;

-- Guilda: Boros / líder: Stronmaus
INSERT INTO organizacao (nome, dataCriacao, logo)
SELECT u.name, UTC_TIMESTAMP(), NULL
FROM usuario u
WHERE u.email = 'stronmaus@gmail.com' AND u.idOrganizacao IS NULL;
UPDATE usuario
SET idOrganizacao = LAST_INSERT_ID()
WHERE email = 'stronmaus@gmail.com' AND idOrganizacao IS NULL;

-- Guilda: Dimir / líder: Ashiok
INSERT INTO organizacao (nome, dataCriacao, logo)
SELECT u.name, UTC_TIMESTAMP(), NULL
FROM usuario u
WHERE u.email = 'ashiok@gmail.com' AND u.idOrganizacao IS NULL;
UPDATE usuario
SET idOrganizacao = LAST_INSERT_ID()
WHERE email = 'ashiok@gmail.com' AND idOrganizacao IS NULL;

-- Guilda: Grixis / líder: Griselbrand
INSERT INTO organizacao (nome, dataCriacao, logo)
SELECT u.name, UTC_TIMESTAMP(), NULL
FROM usuario u
WHERE u.email = 'griselbrand@gmail.com' AND u.idOrganizacao IS NULL;
UPDATE usuario
SET idOrganizacao = LAST_INSERT_ID()
WHERE email = 'griselbrand@gmail.com' AND idOrganizacao IS NULL;

-- Guilda: Gruul / líder: Rowan
INSERT INTO organizacao (nome, dataCriacao, logo)
SELECT u.name, UTC_TIMESTAMP(), NULL
FROM usuario u
WHERE u.email = 'rowan@gmail.com' AND u.idOrganizacao IS NULL;
UPDATE usuario
SET idOrganizacao = LAST_INSERT_ID()
WHERE email = 'rowan@gmail.com' AND idOrganizacao IS NULL;

-- Guilda: Ink / líder: Amedaca
INSERT INTO organizacao (nome, dataCriacao, logo)
SELECT u.name, UTC_TIMESTAMP(), NULL
FROM usuario u
WHERE u.email = 'amedaca@gmail.com' AND u.idOrganizacao IS NULL;
UPDATE usuario
SET idOrganizacao = LAST_INSERT_ID()
WHERE email = 'amedaca@gmail.com' AND idOrganizacao IS NULL;

-- Guilda: Izzet / líder: Jace
INSERT INTO organizacao (nome, dataCriacao, logo)
SELECT u.name, UTC_TIMESTAMP(), NULL
FROM usuario u
WHERE u.email = 'jace@gmail.com' AND u.idOrganizacao IS NULL;
UPDATE usuario
SET idOrganizacao = LAST_INSERT_ID()
WHERE email = 'jace@gmail.com' AND idOrganizacao IS NULL;

-- Guilda: Jund / líder: Dlarer
INSERT INTO organizacao (nome, dataCriacao, logo)
SELECT u.name, UTC_TIMESTAMP(), NULL
FROM usuario u
WHERE u.email = 'dlarer@gmail.com' AND u.idOrganizacao IS NULL;
UPDATE usuario
SET idOrganizacao = LAST_INSERT_ID()
WHERE email = 'dlarer@gmail.com' AND idOrganizacao IS NULL;

-- Guilda: Mardur / líder: Sarkhan
INSERT INTO organizacao (nome, dataCriacao, logo)
SELECT u.name, UTC_TIMESTAMP(), NULL
FROM usuario u
WHERE u.email = 'sarkhan@gmail.com' AND u.idOrganizacao IS NULL;
UPDATE usuario
SET idOrganizacao = LAST_INSERT_ID()
WHERE email = 'sarkhan@gmail.com' AND idOrganizacao IS NULL;

-- Guilda: Naya / líder: Udyr
INSERT INTO organizacao (nome, dataCriacao, logo)
SELECT u.name, UTC_TIMESTAMP(), NULL
FROM usuario u
WHERE u.email = 'udyr@gmail.com' AND u.idOrganizacao IS NULL;
UPDATE usuario
SET idOrganizacao = LAST_INSERT_ID()
WHERE email = 'udyr@gmail.com' AND idOrganizacao IS NULL;

-- Guilda: Orzhov / líder: Cain
INSERT INTO organizacao (nome, dataCriacao, logo)
SELECT u.name, UTC_TIMESTAMP(), NULL
FROM usuario u
WHERE u.email = 'cain@gmail.com' AND u.idOrganizacao IS NULL;
UPDATE usuario
SET idOrganizacao = LAST_INSERT_ID()
WHERE email = 'cain@gmail.com' AND idOrganizacao IS NULL;

-- Guilda: Rakdos Demons / líder: Aslog
INSERT INTO organizacao (nome, dataCriacao, logo)
SELECT u.name, UTC_TIMESTAMP(), NULL
FROM usuario u
WHERE u.email = 'aslog@gmail.com' AND u.idOrganizacao IS NULL;
UPDATE usuario
SET idOrganizacao = LAST_INSERT_ID()
WHERE email = 'aslog@gmail.com' AND idOrganizacao IS NULL;

-- Guilda: Selesnya / líder: Vitughazi
INSERT INTO organizacao (nome, dataCriacao, logo)
SELECT u.name, UTC_TIMESTAMP(), NULL
FROM usuario u
WHERE u.email = 'vitughazi@gmail.com' AND u.idOrganizacao IS NULL;
UPDATE usuario
SET idOrganizacao = LAST_INSERT_ID()
WHERE email = 'vitughazi@gmail.com' AND idOrganizacao IS NULL;

-- Guilda: Simic / líder: Kiora
INSERT INTO organizacao (nome, dataCriacao, logo)
SELECT u.name, UTC_TIMESTAMP(), NULL
FROM usuario u
WHERE u.email = 'kiora@gmail.com' AND u.idOrganizacao IS NULL;
UPDATE usuario
SET idOrganizacao = LAST_INSERT_ID()
WHERE email = 'kiora@gmail.com' AND idOrganizacao IS NULL;

-- Guilda: The Pact / líder: Edgarmarkov
INSERT INTO organizacao (nome, dataCriacao, logo)
SELECT u.name, UTC_TIMESTAMP(), NULL
FROM usuario u
WHERE u.email = 'edgarmarkov@gmail.com' AND u.idOrganizacao IS NULL;
UPDATE usuario
SET idOrganizacao = LAST_INSERT_ID()
WHERE email = 'edgarmarkov@gmail.com' AND idOrganizacao IS NULL;

-- Coletivos de teste. A organização vem do usuário líder, sem depender de IDs fixos.
-- Logos sem correspondência na lista fornecida ficam NULL.
-- Tipos: Guilda = 1; Grupo = 3.
-- Em uma nova execução, não duplica um coletivo com o mesmo nome na organização.
INSERT INTO coletivo (nome, dataCriacao, dataAtualizacao, idOrganizacao, idTipoColetivo, logo)
SELECT seed.nome, UTC_TIMESTAMP(), UTC_TIMESTAMP(), u.idOrganizacao, seed.idTipoColetivo, seed.logo
FROM (
    SELECT 'Adran' AS nome, 'adran@gmail.com' AS email, 3 AS idTipoColetivo, NULL AS logo
    UNION ALL SELECT 'Alton King Alara', 'alton@gmail.com', 3, 'https://i.imgur.com/Qf61eSm.png'
    UNION ALL SELECT 'Eleftheria', 'khovi@gmail.com', 3, 'https://i.imgur.com/xGpSpYU.png'
    UNION ALL SELECT 'Circulo dos Oito', 'kalazan@gmail.com', 3, 'https://i.imgur.com/OdjNKCc.png'
    UNION ALL SELECT 'King Barov Crew', 'kingbarov@gmail.com', 3, NULL
    UNION ALL SELECT 'Monarquia da Coroa', 'reiarthurxxxivlendorr@gmail.com', 3, 'https://i.imgur.com/fILVd0W.png'
    UNION ALL SELECT 'Nahiri', 'nahiri@gmail.com', 3, 'https://i.imgur.com/nzjYtc0.png'
    UNION ALL SELECT 'Abzan', 'ousey@gmail.com', 1, 'https://i.imgur.com/DfZzVSv.png'
    UNION ALL SELECT 'Azorius', 'salomao@gmail.com', 1, 'https://i.imgur.com/qzHnWXL.png'
    UNION ALL SELECT 'Bant', 'iona@gmail.com', 1, 'https://i.imgur.com/IGGZAhm.png'
    UNION ALL SELECT 'Boros', 'stronmaus@gmail.com', 1, 'https://i.imgur.com/1P5vCkX.png'
    UNION ALL SELECT 'Dimir', 'ashiok@gmail.com', 1, 'https://i.imgur.com/5AxQgqU.png'
    UNION ALL SELECT 'Grixis', 'griselbrand@gmail.com', 1, NULL
    UNION ALL SELECT 'Gruul', 'rowan@gmail.com', 1, 'https://i.imgur.com/mLNG0S0.png'
    UNION ALL SELECT 'Ink', 'amedaca@gmail.com', 1, 'https://i.imgur.com/FXacBRX.png'
    UNION ALL SELECT 'Izzet', 'jace@gmail.com', 1, 'https://i.imgur.com/gpqusVe.png'
    UNION ALL SELECT 'Jund', 'dlarer@gmail.com', 1, 'https://i.imgur.com/v3aWP46.png'
    UNION ALL SELECT 'Mardur', 'sarkhan@gmail.com', 1, NULL
    UNION ALL SELECT 'Naya', 'udyr@gmail.com', 1, 'https://i.imgur.com/K8LYBXd.png'
    UNION ALL SELECT 'Orzhov', 'cain@gmail.com', 1, NULL
    UNION ALL SELECT 'Rakdos', 'aslog@gmail.com', 1, NULL
    UNION ALL SELECT 'Selesnya', 'vitughazi@gmail.com', 1, 'https://i.imgur.com/Ur1kYTb.png'
    UNION ALL SELECT 'Simic', 'kiora@gmail.com', 1, 'https://i.imgur.com/N4yII8H.png'
    UNION ALL SELECT 'The Pact', 'edgarmarkov@gmail.com', 1, NULL
) AS seed
INNER JOIN usuario u ON u.email = seed.email
WHERE u.idOrganizacao IS NOT NULL
  AND NOT EXISTS (
      SELECT 1
      FROM coletivo c
      WHERE c.idOrganizacao = u.idOrganizacao AND c.nome = seed.nome
  );

COMMIT;
