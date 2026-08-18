using Microsoft.EntityFrameworkCore;
using NooshRewardsApi.Data;
using NooshRewardsApi.Repositories;
using NooshRewardsApi.Repositories.Interfaces;
using NooshRewardsApi.Services;
using NooshRewardsApi.Services.Interfaces;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

var builder = WebApplication.CreateBuilder(args);

var firebaseKeyPath = builder.Configuration["Firebase:ServiceAccountPath"];
FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.FromFile(firebaseKeyPath)
});

builder.Services.AddDbContext<RewardsDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IRewardRuleRepository, RewardRuleRepository>();
builder.Services.AddScoped<IPunchCardRepository, PunchCardRepository>();
builder.Services.AddScoped<IScanTokenRepository, ScanTokenRepository>();
builder.Services.AddScoped<IReceiptSubmissionRepository, ReceiptSubmissionRepository>();
builder.Services.AddScoped<IRewardsService, RewardsService>();
builder.Services.AddScoped<NooshRewardsApi.Auth.FirebaseAuthFilter>();
builder.Services.AddScoped<NooshRewardsApi.Auth.StaffPinFilter>();
builder.Services.AddScoped<NooshRewardsApi.Auth.AdminKeyFilter>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    // Wide open for local testing only — will be locked down before any
    // real integration with NooshApp.
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RewardsDbContext>();
    db.Database.Migrate();
    DbSeeder.Seed(db);
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();