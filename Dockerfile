FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

# Copy everything  into WORKDIR
COPY ./ ./

RUN ls -laR \
    && dotnet restore ./ApiGateway.csproj \
    && dotnet publish ./ApiGateway.csproj -c Release -o /app/publish \
    && ls -la /app/publish
#RUN ls -la /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy the build to WORKDIR
COPY --from=build /app/publish .

# Sets the entry point to start the app
ENTRYPOINT ["dotnet", "ApiGateway.dll"]