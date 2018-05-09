FROM microsoft/aspnetcore-build:2.0-nanoserver-sac2016
WORKDIR /app
EXPOSE 80
COPY ./dist/portal portal/
WORKDIR /app/portal
ENTRYPOINT ["dotnet", "TestJenkins.dll"]
