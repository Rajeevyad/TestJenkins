ARG version
FROM microsoft/aspnetcore-build
LABEL multi.label1=$version
WORKDIR /app
EXPOSE 80
COPY ./dist/portal portal/
WORKDIR /app/portal
ENTRYPOINT ["dotnet", "TestJenkins.dll"]
