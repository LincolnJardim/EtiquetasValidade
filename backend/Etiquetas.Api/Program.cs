using Etiquetas.Api.Exceptions;
using Etiquetas.Api.Security;
using Etiquetas.Application.Interfaces;
using Etiquetas.Application.Services;
using Etiquetas.Domain.Entities;
using Etiquetas.Infrastructure.Data;
using Etiquetas.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);


/*
    Lê as configurações utilizadas para
    gerar e validar os tokens JWT.
*/
string jwtIssuer =
    builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException(
        "O emissor do JWT não foi configurado."
    );

string jwtAudience =
    builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException(
        "O público do JWT não foi configurado."
    );

string jwtKeyBase64 =
    builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException(
        "A chave do JWT não foi configurada."
    );


/*
    A chave JWT foi criada e armazenada em Base64.
    Aqui ela é convertida novamente para bytes.
*/
byte[] jwtKeyBytes;

try
{
    jwtKeyBytes =
        Convert.FromBase64String(jwtKeyBase64);
}
catch (FormatException)
{
    throw new InvalidOperationException(
        "A chave do JWT não possui um formato Base64 válido."
    );
}


/*
    Garante que a chave possua tamanho suficiente
    para a assinatura dos tokens.
*/
if (jwtKeyBytes.Length < 32)
{
    throw new InvalidOperationException(
        "A chave do JWT deve possuir pelo menos 32 bytes."
    );
}


// ======================================================
// CONTROLLERS E DOCUMENTAÇÃO
// ======================================================

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(options =>
{
    /*
        Define que a API utiliza autenticação
        HTTP do tipo Bearer com tokens JWT.
    */
    options.AddSecurityDefinition(
        "bearer",
        new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",

            Description =
                "Informe o token JWT gerado pelo endpoint de login."
        }
    );

    /*
        Faz o Swagger enviar o token informado
        nas requisições realizadas pela interface.
    */
    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            [
                new OpenApiSecuritySchemeReference(
                    "bearer",
                    document
                )
            ] = []
        }
    );
});


// ======================================================
// TRATAMENTO GLOBAL DE ERROS
// ======================================================

/*
    Registra o manipulador personalizado responsável
    por tratar exceções inesperadas da aplicação.
*/
builder.Services.AddExceptionHandler<
    GlobalExceptionHandler
>();


/*
    Registra o serviço de Problem Details.

    Além de fornecer respostas padronizadas para APIs,
    ele funciona como fallback para o middleware
    UseExceptionHandler().
*/
builder.Services.AddProblemDetails();


// ======================================================
// CORS
// ======================================================

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "MyPolicy",
        policy =>
        {
            policy
                .WithOrigins(
                    "http://127.0.0.1:5500"
                )
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    );
});


// ======================================================
// AUTENTICAÇÃO JWT
// ======================================================

/*
    Configura a API para procurar e validar
    tokens enviados como Bearer Token.
*/
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        /*
            Mantém os nomes originais das claims:
            sub, name, email e jti.
        */
        options.MapInboundClaims = false;

        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                /*
                    Verifica se o token foi assinado
                    com a chave correta.
                */
                ValidateIssuerSigningKey = true,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        jwtKeyBytes
                    ),

                /*
                    Verifica quem emitiu o token.
                */
                ValidateIssuer = true,

                ValidIssuer =
                    jwtIssuer,

                /*
                    Verifica para qual aplicação
                    o token foi criado.
                */
                ValidateAudience = true,

                ValidAudience =
                    jwtAudience,

                /*
                    Rejeita tokens expirados ou que
                    não possuam data de expiração.
                */
                ValidateLifetime = true,

                RequireExpirationTime = true,

                /*
                    Pequena tolerância para diferenças
                    entre os relógios dos servidores.
                */
                ClockSkew =
                    TimeSpan.FromMinutes(1)
            };
    });


/*
    Registra os serviços utilizados posteriormente
    pelo atributo [Authorize].
*/
builder.Services.AddAuthorization();


// ======================================================
// INJEÇÃO DE DEPENDÊNCIA
// ======================================================

// Produto
builder.Services.AddScoped<
    IProdutoRepository,
    ProdutoRepository
>();

builder.Services.AddScoped<
    ProdutoService
>();


// Produção
builder.Services.AddScoped<
    IProducaoRepository,
    ProducaoRepository
>();

builder.Services.AddScoped<
    ProducaoService
>();


// Usuário e autenticação
builder.Services.AddScoped<
    IUsuarioRepository,
    UsuarioRepository
>();

builder.Services.AddScoped<
    UsuarioService
>();

builder.Services.AddScoped<
    AuthService
>();

builder.Services.AddScoped<
    ITokenService,
    JwtTokenService
>();

builder.Services.AddScoped<
    IPasswordHasher<Usuario>,
    PasswordHasher<Usuario>
>();


// ======================================================
// BANCO DE DADOS
// ======================================================

builder.Services.AddDbContext<
    EtiquetaDbContext
>(options =>
{
    options.UseSqlServer(
        builder.Configuration
            .GetConnectionString(
                "DefaultConnection"
            )
    );
});


var app = builder.Build();


// ======================================================
// PIPELINE DE MIDDLEWARES
// ======================================================

/*
    Deve ser executado no início do pipeline para
    capturar exceções geradas pelos componentes
    configurados posteriormente.
*/
app.UseExceptionHandler();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}


app.UseHttpsRedirection();


/*
    O CORS deve ser executado antes dos middlewares
    de autenticação e autorização.
*/
app.UseCors("MyPolicy");


/*
    Primeiro identifica o usuário utilizando
    o token recebido.
*/
app.UseAuthentication();


/*
    Depois verifica se o usuário identificado
    possui acesso ao endpoint.
*/
app.UseAuthorization();



// ======================================================
// CONTROLLERS
// ======================================================

app.MapControllers();

app.Run();