# Run GUI
run:
	@echo "Starting GUI"
	@cd src && dotnet run

# Run Migration (From encrypted schema db/schema.sql)
migrate:
	@echo "Starting Migration"
	@cd src && dotnet run --migrate
	@echo "Finished Migration"

# Run Generate Seeding (For Encrypted Schema)
seed:
	@echo "Starting Seeding"
	@cd src && dotnet run --seed
	@echo "Finished Seeding"

# Load dump (From db/seeded.sql)
load-dump:
	@echo "Starting Load Dump"
	@cd src && dotnet run --load-dump
	@echo "Finished Load Dump"

# Raw Migration (from db/raw-schema.sql)
raw-migrate:
	@echo "Starting Raw Migration"
	@cd src && dotnet run --raw-migrate
	@echo "Finished Raw Migration"


# Generate Raw Seeding, without encryption (from db/raw-seed.sql)
raw-seed:
	@echo "Starting Raw Seeding"
	@cd src && dotnet run --raw-seed
	@echo "Finished Raw Seeding"

# Run dump/schema converter from kating's database dump
convert-dump:
	@echo "Starting Convert Dump"
	@cd src && dotnet run --convert-dump
	@echo "Finished Convert Dump"

# cli
cli:
	@echo "Starting CLI Program"
	@cd src && dotnet run --cli
	@echo "Finished CLI Program"

