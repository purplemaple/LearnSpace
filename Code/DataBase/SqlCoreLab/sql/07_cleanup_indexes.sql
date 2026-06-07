-- Optional cleanup for repeating index experiments.

DROP INDEX IF EXISTS idx_lab_test_batches_device_started;
DROP INDEX IF EXISTS idx_lab_test_batches_started_device;
DROP INDEX IF EXISTS idx_lab_test_batches_status;
DROP INDEX IF EXISTS idx_lab_test_batches_started_id;

ANALYZE lab_test_batches;
