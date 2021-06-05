.PHONY: build run clean test clearlogs publish

#solution_root is the directory containing the root .sln file
solution_root_BL = AppBL
solution_root_UI = AppUI	

#solution_main is the directory containing the project with a main method. Relative to solution_root.
solution_main_BL = GACDRest
solution_main_UI = GACDAngular

solution_dl = $(solution_root_BL)/GACDDL

#log_dir is the name of directory with logs relative to solution_root
log_dir = logs
restore:
	dotnet restore ./$(solution_root_BL);
	dotnet restore ./$(solution_root_UI)
build:
	dotnet build $(solution_root_BL);
	dotnet build $(solution_root_UI)
runui: 
	dotnet run --project $(solution_root_UI)/$(solution_main_UI)
runbl: 
	dotnet run --project $(solution_root_BL)/$(solution_main_BL)
test:
	dotnet test $(solution_root_BL)
publish:
# dotnet publish  $(solution_root)/$(solution_main) -c Release -o $(solution_root)/$(solution_main)/publish
rebuild-db:
    #dotnet ef database drop -c StoreDBContext --startup-project ../$(solution_root)
	cd ./$(solution_dl) && dotnet ef migrations remove --startup-project ../$(solution_main_BL);
	cd ./$(solution_dl) && dotnet ef migrations add newMigration -c StoreDBContext --startup-project ../$(solution_main_BL);
	cd ./$(solution_dl) && dotnet ef database update newMigration --startup-project ../$(solution_main_BL)
clean: clearlogs
	dotnet clean $(solution_root_BL);
	dotnet clean $(solution_root_UI)
clearlogs:
	rm -f $(solution_root_BL)/$(log_dir)/*;
	rm -f $(solution_root_UI)/$(log_dir)/*;
