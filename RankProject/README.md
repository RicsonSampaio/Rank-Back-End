# Rank

API .NET 8 para um projeto pessoal. A estrutura foi inspirada no fluxo do ConstruCode: `Controller → App → Service → Repository`. O repositório usa Dapper. Os casos de uso atuais são cadastro, consulta, atualização, exclusão de usuários e login.

## Projetos

| Projeto | Responsabilidade |
| --- | --- |
| `Rank.WebAPI` | Controller HTTP, configuração de CORS e autenticação JWT |
| `Rank.Application` | Entrada da aplicação; encaminha o caso de uso ao serviço |
| `Rank.Core` | Entidade, DTOs, regras de usuários e interfaces |
| `Rank.Infra.Data.MySql` | Consultas MySQL com Dapper |
| `Rank.Infra.IoC` | Registro das dependências |

Como o ConstruCode, esta versão usa MySQL. Para senhas novas, usa PBKDF2-SHA256 com salt aleatório em vez de MD5. Não há migração de usuários do outro projeto.

## Executar localmente

1. Use o banco local `rank_local` e a tabela `usuario` que você já criou. O esquema esperado está em [Sqls/001_usuario.sql](Sqls/001_usuario.sql); não execute o `CREATE TABLE` novamente se a tabela já existe.
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

   A API calcula o hash da senha e insere a conta na tabela `usuario`.

## Endpoints

| Método e rota | Acesso | Resultado |
| --- | --- | --- |
| `POST /api/users` | Público | Cadastra uma conta e retorna `201` com os dados públicos |
| `POST /api/auth/authenticate` | Público | Retorna JWT com `200` |
| `GET /api/users` | JWT | Lista usuários sem hashes de senha |
| `GET /api/users/{id}` | JWT | Consulta um usuário |
| `PUT /api/users/{id}` | Qualquer JWT válido | Atualiza email, nome e, opcionalmente, senha |
| `DELETE /api/users/{id}` | Qualquer JWT válido | Exclui a conta e retorna `204` |

Depois de cadastrar, chame `POST https://localhost:7199/api/auth/authenticate` com JSON:

```json
{ "email": "voce@exemplo.com", "password": "sua-senha" }
```

Em sucesso, a API devolve `access_token` e `expiration_date`. Envie o token nas chamadas protegidas como `Authorization: Bearer <access_token>`. No `PUT`, envie `email` e `name`; `password` é opcional e, quando enviado, precisa ter ao menos 8 caracteres. As respostas públicas nunca incluem `PasswordHash`. Email duplicado retorna `409`; credenciais incorretas recebem `401`; entrada inválida recebe `400`. Qualquer usuário autenticado pode alterar ou excluir outras contas.

No Swagger, execute o login e cole o valor de `access_token` no botão **Authorize**. A interface adiciona `Bearer` ao cabeçalho automaticamente.

O token expira após 8 horas por padrão. A exclusão da conta invalida seu acesso aos endpoints protegidos. No desenvolvimento, o CORS permite `http://localhost:5173` e `http://localhost:3000`; ajuste `Cors:AllowedOrigins` para a origem real do seu front-end. A senha do banco e a chave JWT não ficam versionadas.
