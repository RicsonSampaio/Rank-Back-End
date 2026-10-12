-- Execute este script completo no MySQL local.
-- IF NOT EXISTS permite executar novamente sem apagar dados.
-- Tabelas existentes não têm sua estrutura alterada por este script.

USE rank_local;

CREATE TABLE IF NOT EXISTS tarefa_status
(
    id INT(11) NOT NULL AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(150) NOT NULL
) ENGINE=InnoDB CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- Status iniciais com IDs fixos.
-- INSERT IGNORE: em nova execução, IDs já existentes são ignorados (sem erro e sem duplicar).
START TRANSACTION;

INSERT IGNORE INTO tarefa_status (id, nome)
VALUES
    (1, 'Pendente'),
    (2, 'Em andamento'),
    (3, 'Concluída');

COMMIT;