# Rank

API .NET 8 para um projeto pessoal. A estrutura segue o fluxo do ConstruCode: `Controller → App → Service → Repository`. Controllers, Apps e Services recebem `IServiceProvider` no construtor e resolvem as classes concretas com `GetRequiredService<T>()` dentro dos métodos. O repositório usa Dapper. Os casos de uso atuais incluem usuários, login, organizações, coletivos, membros e tarefas.

## Projetos

| Projeto | Responsabilidade |
| --- | --- |
| `Rank.WebAPI` | Controller HTTP, configuração de CORS e autenticação JWT |
| `Rank.Application` | Entrada da aplicação; encaminha o caso de uso ao serviço |
| `Rank.Core` | Entidades, DTOs, Services, Repositories e geração de token |
| `Rank.Infra.Data.MySql` | `DBDapperComponent`: abre conexões e executa consultas e comandos Dapper |
| `Rank.Infra.IoC` | Registro das dependências |

Como o ConstruCode, esta versão usa MySQL. Para senhas novas, usa PBKDF2-SHA256 com salt aleatório em vez de MD5. Não há migração de usuários do outro projeto.

Em `Rank.Core.Repository`, cada método monta o `commandText` e chama `QuerySingleAsync`, `QueryListAsync`, `ExecuteAsync`, `InsertAndGetIdAsync` ou `QuerySingleTransactionAsync` da base. A abertura e o descarte da conexão, assim como o início, commit e rollback da transação, ficam em `DBDapperComponent`.

Todos os IDs usam `int` na aplicação (`int?` quando aceitam `null`). No SQL, as chaves primárias com auto incremento usam `INT(11)` e os demais IDs usam `INT`, preservando os valores padrão e a nulabilidade de cada coluna. O script de criação não converte os tipos de tabelas já existentes; essa alteração precisa ser feita separadamente no banco com `ALTER TABLE`.

## Executar localmente

1. Execute o script completo [Sqls/001_rank_local.sql](Sqls/001_rank_local.sql) no MySQL local. Ele cria o banco `rank_local`, seleciona esse banco e cria as tabelas `organizacao`, `usuario`, `coletivo`, `coletivo_usuario` e `tarefa`, com engine InnoDB para suportar transações. O script usa `IF NOT EXISTS`: pode ser executado novamente sem apagar dados, mas não altera a estrutura de tabelas existentes.
2. A conexão e uma chave JWT privada já foram salvas nos *user secrets* do .NET nesta máquina. Para configurar outra máquina, rode os comandos abaixo dentro da pasta deste repositório:

   ```powershell
   dotnet user-secrets set "ConnectionStrings:Rank" "Server=localhost;Port=3306;Database=rank_local;User ID=SEU_USUARIO;Password=SUA_SENHA;SslMode=None;AllowPublicKeyRetrieval=True" --project Rank.WebAPI
   $bytes = New-Object byte[] 48
   $gerador = [Security.Cryptography.RandomNumberGenerator]::Create()
   $gerador.GetBytes($bytes)
   $gerador.Dispose()
   $chave = [Convert]::ToBase64String($bytes)
   dotnet user-secrets set "Jwt:Key" $chave --project Rank.WebAPI
   ```

   As opções `SslMode=None` e `AllowPublicKeyRetrieval=True` são apenas para este MySQL em `localhost`. Para outro servidor, configure TLS e revise a autenticação antes de reutilizar a conexão.

3. Inicie a API:

   ```powershell
   dotnet run --project Rank.WebAPI --launch-profile https
   ```

   O perfil de desenvolvimento abre `https://localhost:7199/swagger`, onde os endpoints ficam listados e podem ser executados. O Swagger fica disponível apenas em desenvolvimento.

4. Crie a primeira conta pela API:

   ```powershell
   $body = @{ email = "voce@exemplo.com"; name = "Seu nome"; password = "uma-senha-forte" } | ConvertTo-Json
   Invoke-RestMethod -Uri "https://localhost:7199/api/users" -Method Post -ContentType "application/json" -Body $body
   ```

   A API calcula o hash da senha e executa um único comando SQL dentro de uma transação: cria uma organização com o mesmo nome do usuário, obtém seu ID com `LAST_INSERT_ID()` e insere a conta com `idOrganizacao` preenchido. A resposta inclui esse vínculo. Se qualquer operação falhar, os dois cadastros são desfeitos; email duplicado continua retornando `409`, sem deixar uma organização criada por esse cadastro.

## Usuários de teste

O final de [Sqls/001_rank_local.sql](Sqls/001_rank_local.sql) cadastra 24 usuários de teste: 7 líderes de Grupos e 17 líderes de Guildas. Nos Grupos, `name` usa o nome informado na lista; nas Guildas, usa o nome derivado do email, como `Ousey` para `ousey@gmail.com`. A senha temporária é `123`, armazenada como PBKDF2-SHA256 com salt individual. Os `INSERT IGNORE` preservam contas já cadastradas. Na mesma transação, o script cria uma organização com o `name` de cada usuário ainda sem organização e atualiza seu `idOrganizacao` com `LAST_INSERT_ID()`. Depois, cria um coletivo para cada líder com o `idOrganizacao` de sua conta: `Grupo = 3` e `Guilda = 1`. As logos sem URL correspondente ficam `NULL`; executar novamente o script não duplica coletivos com o mesmo nome na mesma organização. Vínculos de membros em `coletivo_usuario` ficam para depois.

## Endpoints

| Método e rota | Acesso | Resultado |
| --- | --- | --- |
| `POST /api/users` | Público | Cria organização e conta na mesma transação; retorna `201` com os dados públicos e `idOrganizacao` |
| `POST /api/auth/authenticate` | Público | Retorna JWT com `200` |
| `GET /api/users` | JWT | Lista usuários sem hashes de senha |
| `GET /api/users/{id}` | JWT | Consulta um usuário |
| `PUT /api/users/{id}` | Qualquer JWT válido | Atualiza email, nome e, opcionalmente, senha |
| `DELETE /api/users/{id}` | Qualquer JWT válido | Exclui a conta e retorna `204` |
| `GET /api/organizacao` | JWT | Lista organizações |
| `GET /api/organizacao/{id}` | JWT | Consulta uma organização |
| `PUT /api/organizacao/{id}` | JWT | Atualiza nome e logo |
| `DELETE /api/organizacao/{id}` | JWT | Exclui uma organização e retorna `204` |
| `POST /api/coletivo` | JWT com organização | Cria um coletivo na organização do token e retorna `201` |
| `GET /api/coletivo` | JWT | Lista coletivos conforme admin, organização e vínculos do usuário |
| `GET /api/coletivo/{id}` | JWT | Consulta um coletivo |
| `PUT /api/coletivo/{id}` | JWT | Atualiza nome, logo e tipo |
| `DELETE /api/coletivo/{id}` | JWT | Exclui um coletivo e retorna `204` |
| `POST /api/membro` | JWT | Vincula um usuário pelo email a um coletivo e retorna `201` |
| `GET /api/membro?idColetivo={id}` | JWT | Lista membros do coletivo informado |
| `GET /api/membro/{id}` | JWT | Consulta um vínculo de membro |
| `PUT /api/membro/{id}` | JWT | Atualiza o coletivo e o usuário do vínculo, identificado pelo email |
| `DELETE /api/membro/{id}` | JWT | Exclui o vínculo e retorna `204` |
| `POST /api/tarefa` | JWT | Cria uma tarefa e retorna `201` |
| `GET /api/tarefa?idColetivo={id}` | JWT | Lista as tarefas do coletivo informado |
| `GET /api/tarefa/{id}` | JWT | Consulta uma tarefa |
| `PUT /api/tarefa/{id}` | JWT | Atualiza uma tarefa |
| `DELETE /api/tarefa/{id}` | JWT | Exclui uma tarefa e retorna `204` |

Depois de cadastrar, chame `POST https://localhost:7199/api/auth/authenticate` com JSON:

```json
{ "email": "voce@exemplo.com", "password": "sua-senha" }
```

Em sucesso, a API devolve `access_token` e `expiration_date`. Envie o token nas chamadas protegidas como `Authorization: Bearer <access_token>`. A senha não tem restrições de tamanho ou composição, inclusive podendo ser vazia, e continua armazenada como hash. No `PUT`, envie `email` e `name`; `password` é opcional: omitido ou `null` mantém a senha atual, enquanto uma string vazia define uma senha vazia. As respostas públicas nunca incluem `PasswordHash`. Email duplicado retorna `409`; credenciais incorretas recebem `401`; entrada inválida recebe `400`. Qualquer usuário autenticado pode alterar ou excluir outras contas.

No Swagger, execute o login e cole o valor de `access_token` no botão **Authorize**. A interface adiciona `Bearer` ao cabeçalho automaticamente.

O payload do JWT inclui `sub` (ID do usuário), `email`, `name`, `admin` (booleano `true` ou `false`), `idOrganizacao` (número ou `null`) e `fotoAccount` (link ou `null`), além dos metadados do token. Organização e foto estão sempre presentes no payload, mesmo quando o cadastro não tem esses valores. O front pode ler `admin` ao decodificar o `access_token` para adaptar a interface. Os dados refletem o usuário no momento do login; após alterá-los no banco, faça login novamente para gerar um token atualizado. Os endpoints atuais continuam acessíveis a qualquer usuário autenticado; essa claim não adiciona restrições de administrador à API.

### Usuário da requisição

Assim como no projeto base, `BaseController` resolve `_currentUser` pelo provider. `CurrentUser` é registrado por requisição (`Scoped`) e lê as claims de `HttpContext.User`, preenchido pela autenticação JWT. Em qualquer controller que herde de `BaseController`, use:

```csharp
var userId = GetUserId();
var email = GetUserEmail();
var name = GetUserName();
var admin = IsAdmin();
var idOrganizacao = GetOrganizacaoId();
var fotoAccount = GetAvatar();
```

Esses métodos têm `[NonAction]` e não geram endpoints. A base exige JWT válido por meio de `[Authorize]`; login e cadastro mantêm `[AllowAnonymous]`. Em chamadas sem autenticação nesses endpoints públicos, o usuário atual tem ID `0`, email e nome vazios, admin `false`, organização e foto `null`. Não há gravação de logs de ações nesse fluxo.

## Organizações

O esquema da tabela está no script único [Sqls/001_rank_local.sql](Sqls/001_rank_local.sql). Para atualizar uma organização, envie:

```json
{ "nome": "Minha organização", "logo": "https://exemplo.com/logo.png" }
```

`nome` é obrigatório e aceita até 150 caracteres; `logo` é opcional e aceita até 2048. Enviar `logo: null` remove o link. A atualização preserva `dataCriacao`. Uma organização inexistente retorna `404`.

A criação da organização acontece automaticamente no cadastro de cada novo usuário. O nome inicial é o mesmo do usuário, `logo` começa como `null` e a data de criação é definida em UTC pelo Service. O Repository do usuário monta os dois INSERTs e o SELECT final, executados por `QuerySingleTransactionAsync` na mesma conexão e transação. Os métodos internos de criação em `OrganizacaoService` e `OrganizacaoRepository` continuam disponíveis, mas não há método de criação no App ou endpoint POST no controller. Alterar o nome do usuário posteriormente não renomeia a organização.

## Coletivos

O CRUD segue `ColetivoController → ColetivoApp → ColetivoService → ColetivoRepository`, com resolução pelo provider e execução SQL em `DBDapperComponent`. Todos os endpoints exigem JWT.

Para criar, envie:

```json
{ "nome": "Meu coletivo", "idTipoColetivo": 3, "logo": "https://exemplo.com/logo.png" }
```

`nome` é obrigatório e aceita até 150 caracteres; `logo` é opcional e aceita até 2048. No cadastro e na edição, `idTipoColetivo` é obrigatório e usa o enum `Rank.Core.Enum.TipoColetivo`:

| Valor | Tipo |
| --- | --- |
| 1 | Guilda |
| 2 | GuildaEspecial |
| 3 | Grupo |
| 4 | Alianca |
| 5 | Coalizao |

O front envia o número do tipo escolhido; omitir `idTipoColetivo`, enviar `0` ou um valor fora do enum retorna `400`. A propriedade `IdTipoColetivo` usa `TipoColetivo` nos DTOs de entrada, na entidade e no DTO de resposta, e o Service repassa o enum diretamente. O JSON continua usando números e a coluna no banco continua sendo `INT`. O Service grava esse valor, sem aplicar um tipo padrão. O controller continua obtendo `idOrganizacao` exclusivamente por `GetOrganizacaoId()`: esse campo não faz parte dos DTOs de entrada. Sem uma organização válida no token, o cadastro retorna `403`; faça login novamente para obter a claim, caso esteja usando um token antigo.

Para atualizar, envie `nome`, `idTipoColetivo` e `logo`:

```json
{ "nome": "Meu coletivo atualizado", "idTipoColetivo": 4, "logo": null }
```

As datas são definidas pelo Service em UTC. A atualização altera nome, logo, tipo e `dataAtualizacao`, preservando `dataCriacao` e organização. Enviar `logo: null` remove o link. Consulta, atualização e exclusão de um coletivo inexistente retornam `404`. Consulta por ID, atualização e exclusão continuam acessíveis a qualquer usuário autenticado, sem filtro de organização.

### Listagem de coletivos

O front continua chamando `GET /api/coletivo` sem enviar filtros. A controller encaminha apenas `GetUserId()` ao App. As regras são aplicadas em `ColetivoService`, que consulta o usuário pelo ID do token e usa `Admin` e `IdOrganizacao` atuais da tabela `usuario`:

- Admin recebe todos os coletivos.
- Os demais recebem os coletivos com a mesma organização do usuário e também aqueles vinculados ao seu ID na tabela `coletivo_usuario`.
- O Service reúne as duas listas sem duplicar coletivos, ordenando por ID. Sem organização, o usuário ainda pode receber coletivos por vínculo. Sem acesso a nenhum coletivo, recebe uma lista vazia.

O Repository apenas executa as consultas por organização e por vínculo, usando `EXISTS` para que vínculos repetidos não dupliquem os registros. Os vínculos são gerenciados pelo CRUD de membros.

## Membros

O CRUD segue `MembroController → MembroApp → MembroService → MembroRepository` e opera sobre `coletivo_usuario`. Todos os endpoints exigem JWT e são acessíveis a qualquer usuário autenticado nesta etapa.

No cadastro e na edição, envie:

```json
{ "idColetivo": 1, "email": "membro@exemplo.com" }
```

O Service normaliza o email e busca o usuário cadastrado. Se não encontrar, retorna `400` com `{ "message": "Este usuário ainda não está cadastrado no sistema." }`. Também confere a existência do coletivo: um coletivo inexistente retorna `400` com a mensagem correspondente. Quando ambos existem, grava apenas `idColetivo` e `idUsuario` no vínculo; o front não precisa informar o ID do usuário.

`GET /api/membro?idColetivo=1` lista somente os membros desse coletivo. `idColetivo` é obrigatório e deve ser positivo. As respostas incluem `id` (ID do vínculo), `idColetivo`, `idUsuario`, `email`, `nome` e `fotoAccount`. Os dados de usuário são consultados por JOIN e não são duplicados na tabela de vínculos.

Nas rotas `/{id}`, o ID é o registro de `coletivo_usuario`, não o ID do usuário. Vínculo inexistente retorna `404`. Excluir um membro remove somente a associação; a conta do usuário continua no sistema. A associação passa a ser considerada automaticamente pela listagem de coletivos do usuário vinculado.

## Tarefas

O CRUD segue `TarefaController → TarefaApp → TarefaService → TarefaRepository`. Todos os endpoints exigem JWT. Para listar, envie o parâmetro obrigatório na query, por exemplo `GET /api/tarefa?idColetivo=1`. A listagem retorna somente as tarefas desse coletivo, ordenadas por ID. Não há filtros adicionais de usuário, organização ou privacidade nesta etapa.

Exemplo de cadastro:

```json
{
  "titulo": "Minha primeira tarefa",
  "idColetivo": 1,
  "descricao": "Descrição opcional",
  "userListParticipantes": "1,2"
}
```

Os DTOs de cadastro e edição aceitam os campos da tabela, exceto `id`, `dataCriacao` e `dataAtualizacao`. IDs seguem os valores padrão do script, incluindo `idRelevancia = 1`; IDs e datas opcionais também aceitam `null` conforme a tabela. `titulo` é obrigatório; `userListParticipantes` não aceita `null`, mas pode ser uma string vazia e começa vazio quando omitido. As listas de usuários são armazenadas como texto, sem validação dos IDs ou interpretação dos valores separados por vírgula. `idUsuarioCriacao` vem da request e começa em `0` quando omitido.

`idColetivo` também faz parte dos DTOs de cadastro, edição e resposta; começa em `0` quando omitido no cadastro ou edição. Na tabela, é `INT NOT NULL DEFAULT 0`, logo após `id`. Caso a tabela `tarefa` já exista sem essa coluna, o `CREATE TABLE IF NOT EXISTS` não a adiciona. Nesse caso, execute separadamente:

```sql
ALTER TABLE rank_local.tarefa ADD COLUMN idColetivo INT NOT NULL DEFAULT 0 AFTER id;
```

O Service define `dataCriacao` em UTC e deixa `dataAtualizacao` como `null` no cadastro. Na edição, preserva a data de criação e preenche a data de atualização em UTC. O `PUT` recebe os dados completos da tarefa: campos omitidos assumem os valores padrão do DTO. Consulta, edição ou exclusão de um ID inexistente retornam `404`.

O token expira após 8 horas por padrão. A exclusão da conta invalida seu acesso aos endpoints protegidos. No desenvolvimento, o CORS permite `http://localhost:5173` e `http://localhost:3000`; ajuste `Cors:AllowedOrigins` para a origem real do seu front-end. A senha do banco e a chave JWT não ficam versionadas.
