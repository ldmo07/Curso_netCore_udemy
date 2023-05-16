services.AddScoped<IUsuariosRepositorio, RepositorioUsuarios>();

services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();