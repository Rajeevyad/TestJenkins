FROM docker pull microsoft/aspnetcore-build
LABEL multi.label1="value1"
WORKDIR /app
EXPOSE 80
COPY ./dist/portal portal/
WORKDIR /app/portal
ENTRYPOINT ["dotnet", "TestJenkins.dll"]
