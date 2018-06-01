ARG labelversion
FROM microsoft/aspnetcore-build
LABEL labelversion $labelversion
WORKDIR /app
EXPOSE 80
COPY ./TestJenkins/TestJenkins/dist/portal portal/
WORKDIR /app/portal
ENTRYPOINT ["dotnet", "TestJenkins.dll"]
