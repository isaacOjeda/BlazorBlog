using BlazorBlog.ApplicationCore.Common;
using BlazorBlog.ApplicationCore.Entities;
using BlazorBlog.Components;
using BlazorBlog.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddIdentityCore<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblyContaining<Program>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


await SeedData();

app.Run();



async Task SeedData()
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    await context.Database.MigrateAsync();



    // Admin User
    if (!context.Users.Any())
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var admin = new User
        {
            Email = "admin@balusoft.com",
            UserName = "admin",
            FirstName = "Admin",
            LastName = "User"
        };

        await userManager.CreateAsync(admin, "Admin@123");
        await userManager.AddToRoleAsync(admin, Roles.Admin);

        var user = new User
        {
            Email = "user@balusoft.com",
            UserName = "user",
            FirstName = "Normal",
            LastName = "User"
        };

        await userManager.CreateAsync(user, "User@123");
        await userManager.AddToRoleAsync(user, Roles.User);
    }


    if (!context.Posts.Any())
    {
        var admin = await context.Users.FirstOrDefaultAsync(x => x.UserName == "admin");

        context.Posts.Add(new Post
        {
            Title = "Man must explore, and this is exploration at its greatest",
            Content = "<p>Never in all their history have men been able truly to conceive of the world as one: a single sphere, a globe, having the qualities of a globe, a round earth in which all the directions eventually meet, in which there is no center because every point, or none, is center — an equal earth which all men occupy as equals. The airman's earth, if free men make it, will be truly round: a globe in practice, not in theory.</p>\r\n                        <p>Science cuts two ways, of course; its products can be used for both good and evil. But there's no turning back from science. The early warnings about technological dangers also come from science.</p>\r\n                        <p>What was most significant about the lunar voyage was not that man set foot on the Moon but that they set eye on the earth.</p>\r\n                        <p>A Chinese tale tells of some men sent to harm a young girl who, upon seeing her beauty, become her protectors rather than her violators. That's how I felt seeing the Earth for the first time. I could not help but love and cherish her.</p>\r\n                        <p>For those who have seen the Earth from space, and for the hundreds and perhaps thousands more who will, the experience most certainly changes your perspective. The things that we share in our world are far more valuable than those which divide us.</p>",
            AuthorId = admin!.Id,
            Created = DateTime.UtcNow,
            Subtitle = "Problems look mighty small from 150 miles up",
            Slug = "my-first-post"
        });

        context.Posts.Add(new Post
        {
            Title = "Science has not yet mastered prophecy",
            Content = "<p>Never in all their history have men been able truly to conceive of the world as one: a single sphere, a globe, having the qualities of a globe, a round earth in which all the directions eventually meet, in which there is no center because every point, or none, is center — an equal earth which all men occupy as equals. The airman's earth, if free men make it, will be truly round: a globe in practice, not in theory.</p>\r\n                        <p>Science cuts two ways, of course; its products can be used for both good and evil. But there's no turning back from science. The early warnings about technological dangers also come from science.</p>\r\n                        <p>What was most significant about the lunar voyage was not that man set foot on the Moon but that they set eye on the earth.</p>\r\n                        <p>A Chinese tale tells of some men sent to harm a young girl who, upon seeing her beauty, become her protectors rather than her violators. That's how I felt seeing the Earth for the first time. I could not help but love and cherish her.</p>\r\n                        <p>For those who have seen the Earth from space, and for the hundreds and perhaps thousands more who will, the experience most certainly changes your perspective. The things that we share in our world are far more valuable than those which divide us.</p>",
            AuthorId = admin!.Id,
            Created = DateTime.UtcNow,
            Subtitle = "We predict too much for the next year and yet far too little for the next ten",
            Slug = "my-second-post"
        });


        await context.SaveChangesAsync();
    }
}