# Stored Procedures

515 stored procedures exist in the live `SalesrollDB_ZAS` database (confirmed via `sys.procedures`), reverse-engineered directly from their live T-SQL definitions (`sys.sql_modules.definition`) on 2026-09-05, replacing the prior app-code-only version (which covered ~119 procedures with no bodies). Split into 4 parts below by name range for manageability; each part documents ~124-131 procedures with full verbatim T-SQL, parameters, tables read/written, transaction/error handling, business logic, result sets, and application callers (traced via grep across `Library.DAL`, `Library.BLL`, `Solution.Web`).

See totals and COVERED/UNUSED breakdown in [`../reverse-engineering/database-inventory.md`](../reverse-engineering/database-inventory.md).

## Part 1

See [`stored-procedures-part1.md`](stored-procedures-part1.md).

## Part 2

See [`stored-procedures-part2.md`](stored-procedures-part2.md).

## Part 3

See [`stored-procedures-part3.md`](stored-procedures-part3.md).

## Part 4

See [`stored-procedures-part4.md`](stored-procedures-part4.md).

