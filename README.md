# Net9-SwaggerToScalar
Net9 Swagger平替Scalar 
授权 Bearer
```text
 builder.Services.AddOpenApi(options =>
 {
     options.AddDocumentTransformer((document, context, cancellationToken) =>
     {
         document.Info = new()
         {
             Title = "Scalar API",
             Version = "V1",
             Description = ".NET 10 Scalar"
         };
         return Task.CompletedTask;
     });
     options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
    
 });

public sealed class BearerSecuritySchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider) : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();
        if (authenticationSchemes.Any(authScheme => authScheme.Name == "Bearer"))
        {
            // Add the security scheme at the document level
            var requirements = new Dictionary<string,
              IOpenApiSecurityScheme>
            {
                ["Bearer"] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // "bearer" refers to the header name here
                    In = ParameterLocation.Header,
                    BearerFormat = "Json Web Token"
                }
            };
            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes = requirements;

            //Apply it as a requirement for all operations
            //foreach (var operation in document.Paths.Values.SelectMany(path => path.Operations))
            //{
            //    if (operation.Value.Security != null)
            //    {
            //        var securityRequirement = new OpenApiSecurityRequirement
            //        {
            //            {
            //                new OpenApiSecuritySchemeReference("Bearer"),  // 这里的字符串必须和安全方案定义的键一致
            //                new List<string>()  // 作用域列表，对于非OAuth2方案可以为空
            //            }
            //        };
            //        operation.Value.Security.Add(securityRequirement);
            //    }
            //}
        }
    }
}

```
