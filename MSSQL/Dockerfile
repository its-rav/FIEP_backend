FROM microsoft/mssql-server-linux:latest
#Copy all scripts into working directory
COPY MSSQL/ /var/opt/mssql/
EXPOSE 1433
CMD ["/bin/bash","/var/opt/mssql/entrypoint.sh"]