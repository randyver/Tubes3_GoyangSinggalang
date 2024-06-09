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

# Real stress test
stress-real:
	@echo "Starting Real Stress Test"
	@cd src && dotnet run --stress real > ../test/stress_test_real.txt
	@echo "Finished Real Stress Test"

# Easy stress test
stress-easy:
	@echo "Starting Easy Stress Test"
	@cd src && dotnet run --stress easy > ../test/stress_test_easy.txt
	@echo "Finished Easy Stress Test"

# Medium stress test
stress-medium:
	@echo "Starting Medium Stress Test"
	@cd src && dotnet run --stress medium > ../test/stress_test_medium.txt
	@echo "Finished Medium Stress Test"

# Hard stress test
stress-hard:
	@echo "Starting Hard Stress Test"
	@cd src && dotnet run --stress hard > ../test/stress_test_hard.txt
	@echo "Finished Hard Stress Test"

# cli
cli:
	@echo "Starting CLI Program"
	@cd src && dotnet run --cli
	@echo "Finished CLI Program"

