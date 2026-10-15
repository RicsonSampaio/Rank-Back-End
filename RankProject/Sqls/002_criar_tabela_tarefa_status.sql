-- Execute este script completo no MySQL local.
-- IF NOT EXISTS permite executar novamente sem apagar dados.
-- Tabelas existentes não têm sua estrutura alterada por este script.

USE rank_local;

CREATE TABLE IF NOT EXISTS tarefa_status
(
    id INT(11) NOT NULL AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(150) NOT NULL
) ENGINE=InnoDB CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;


INSERT INTO tarefa_status (nome)

VALUES
    ('Pendente'),
    ('Em andamento'),
    ('Concluída');
