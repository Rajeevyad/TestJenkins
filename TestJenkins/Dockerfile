ARG version
FROM microsoft/aspnetcore-build
LABEL version=$version
WORKDIR /app
EXPOSE 80
COPY /dist/portal portal/
WORKDIR /app/portal
ENTRYPOINT ["dotnet", "TestJenkins.dll"]
