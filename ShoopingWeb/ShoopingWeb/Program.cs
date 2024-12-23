using Microsoft.EntityFrameworkCore;
using ShoopingWeb.Data;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography.Xml;
using ShoopingWeb.Helpers;
using ShoopingWeb.Services.Implmentation;
using ShoopingWeb.Services.Interfaces;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

    });



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    //����api�ı�����ʽ��
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "ʹ��Bearer������JWT��Ȩ��ͷ",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type=SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    //��api���ȫ�ְ�ȫҪ��
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
             
            },
               new List<String>()
        }
    });


   
}

);


builder.Services.AddDbContext<ShopDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("ShopConn"));
});


//���ÿ������
builder.Services.AddCors(options =>
{
    options.AddPolicy("cors", p =>
    {
        p.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
      
    });
});


// ע�����
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection(nameof(AuthSettings)));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUsersloginService, UsersloginService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICategoryDataService, CategoryDataService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IFile_Management, File_Management>();
builder.Services.AddScoped<IPurchaseService, PurchaseService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("cors");
app.UseMiddleware<JwtMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
